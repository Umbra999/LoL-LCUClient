using Hexed.Wrappers;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using WebSocketSharp;

namespace Hexed.LCU
{
    internal class LeagueClient
    {
        private static HttpClient client;

        private static Process LeagueProcess;

        private readonly Dictionary<string, Action<OnWebsocketEventArgs>> Subscribers = new();

        private WebSocket socketConnection;

        private readonly KeyValuePair<string, string> processInfo;


        public LeagueClient(Process proc)
        {
            LeagueProcess = proc;
            client = new HttpClient(new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (a, b, c, d) => true
            });
            client.DefaultRequestHeaders.Add("User-Agent", "LeagueOfLegendsClient");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            processInfo = GetLeagueStatus();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Utils.ToBase64("riot:" + processInfo.Key));

            TryConnect();
        }

        public Task<string> Request(HttpMethod method, string url, object body = null)
        {
            return client.SendAsync(new HttpRequestMessage(method, "https://127.0.0.1:" + processInfo.Value + url)
            {
                Content = body == null ? null : new StringContent(body.ToString(), Encoding.UTF8, "application/json")
            }).Result.Content.ReadAsStringAsync();
        }

        private void TryConnect()
        {
            socketConnection = new WebSocket("wss://127.0.0.1:" + processInfo.Value + "/", "wamp");
            socketConnection.SetCredentials("riot", processInfo.Key, true);

            socketConnection.SslConfiguration.EnabledSslProtocols = SslProtocols.Tls12;
            socketConnection.SslConfiguration.ServerCertificateValidationCallback = (a, b, c, d) => true;
            socketConnection.OnMessage += HandleMessage;
            socketConnection.OnClose += HandleDisconnect;
            socketConnection.Connect();
        }

        public void SubscribeEvent(string Event, Action<OnWebsocketEventArgs> args)
        {
            if (Subscribers.ContainsKey(Event)) return;

            Subscribers.Add(Event, args);
            socketConnection.Send($"[5, \"{Event}\"]");
        }

        public void UnsubscribeEvent(string Event)
        {
            if (!Subscribers.ContainsKey(Event)) return;

            Subscribers.Remove(Event);
            socketConnection.Send($"[6, \"{Event}\"]");
        }

        private void HandleDisconnect(object sender, CloseEventArgs args)
        {
            TryConnect();
        }

        private void HandleMessage(object sender, MessageEventArgs args)
        {
            try
            {
                if (!args.IsText) return;

                JArray payload = JArray.Parse(args.Data);

                if (payload.Count != 3 || (long)payload[0] != 8) return;

                string Key = payload[1].ToString();
                if (Subscribers.ContainsKey(Key))
                {
                    var ev = (dynamic)payload[2];
                    Subscribers[Key](new OnWebsocketEventArgs()
                    {
                        Path = ev["uri"],
                        Type = ev["eventType"],
                        Data = ev["eventType"] == "Delete" ? null : ev["data"]
                    });
                }
            }
            catch (Exception e)
            {
                Wrappers.Logger.LogError($"Error receiving Websocket: {e}");
            }
        }

        private static KeyValuePair<string, string> GetLeagueStatus()
        {
            var processDirectory = Path.GetDirectoryName(LeagueProcess.MainModule.FileName);
            string lockfilePath = Path.Combine(processDirectory, "lockfile");

            using var stream = File.Open(lockfilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new StreamReader(stream);
            string lockfile = reader.ReadToEnd();
            var splitContent = lockfile.Split(':');
            return new KeyValuePair<string, string>
            (
                splitContent[3],
                splitContent[2]
            );
        }
    }
}
