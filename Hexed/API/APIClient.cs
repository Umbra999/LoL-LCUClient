using Hexed.LCU;
using Hexed.Wrappers;
using Newtonsoft.Json;
using static Hexed.Objects.LeagueObjects;

namespace Hexed.API
{
    internal class APIClient
    {
        public static LeagueClient leagueClient;

        public static LolSummonerSummoner GetCurrentSummoner()
        {
            string data = leagueClient.Request(HttpMethod.Get, "/lol-summoner/v1/current-summoner").Result;

            if (data == null) return null;

            return JsonConvert.DeserializeObject<LolSummonerSummoner>(data);
        }

        public static LolSummonerSummoner GetSummonerById(string uuid)
        {
            string data = leagueClient.Request(HttpMethod.Get, $"/lol-summoner/v2/summoners/puuid/{uuid}").Result;

            if (data == null) return null;

            return JsonConvert.DeserializeObject<LolSummonerSummoner>(data);
        }

        public static LolPerksPerkPageResource GetCurrentRunePage()
        {
            string data = leagueClient.Request(HttpMethod.Get, "/lol-perks/v1/currentpage").Result;

            if (data == null) return null;

            return JsonConvert.DeserializeObject<LolPerksPerkPageResource>(data);
        }

        public static LolLobbyLobbyDto GetCurrentLobby()
        {
            string data = leagueClient.Request(HttpMethod.Get, "/lol-lobby/v2/lobby").Result;

            if (data == null) return null;

            return JsonConvert.DeserializeObject<LolLobbyLobbyDto>(data);
        }

        public static LolChampSelectChampGridChampion GetChampionById(int id)
        {
            string data = leagueClient.Request(HttpMethod.Get, $"/lol-champ-select/v1/grid-champions/{id}").Result;

            if (data == null) return null;

            return JsonConvert.DeserializeObject<LolChampSelectChampGridChampion>(data);
        }

        public static void DeleteRunePageById(int ID)
        {
            string data = leagueClient.Request(HttpMethod.Delete, $"/lol-perks/v1/pages/{ID}").Result;
        }

        public static void CreateRunePage(LolPerksPerkPageResource RunePage)
        {
            string Body = JsonConvert.SerializeObject(RunePage);
            string data = leagueClient.Request(HttpMethod.Post, $"/lol-perks/v1/pages", Body).Result;
        }

        public static void AcceptReadyCheck()
        {
            string data = leagueClient.Request(HttpMethod.Post, $"/lol-matchmaking/v1/ready-check/accept").Result;
        }

        public static void SubscribeEvent(string Event)
        {
            string data = leagueClient.Request(HttpMethod.Post, $"/Subscribe?{Event}").Result;
        }

        public static void UnsubscribeEvent(string Event)
        {
            string data = leagueClient.Request(HttpMethod.Post, $"/Unsubscribe?{Event}").Result;
        }

        public static void GetHelp()
        {
            string data = leagueClient.Request(HttpMethod.Get, $"/Help").Result;
            Logger.LogDebug(data);
        }
    }
}
