using Hexed.Objects;
using Newtonsoft.Json;
using System.Net;

namespace Hexed.API
{
    internal class DDragon
    {
        public static string GetLatestVersion()
        {
            WebClient Client = new();

            string Data = Client.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");

            string[] Versions = JsonConvert.DeserializeObject<string[]>(Data);
            return Versions[0];
        }

        public static DDragonObjects.Perk[] GetRunes(string Version)
        {
            WebClient Client = new();

            string Data = Client.DownloadString($"https://ddragon.leagueoflegends.com/cdn/{Version}/data/en_US/runesReforged.json");

            return JsonConvert.DeserializeObject<DDragonObjects.Perk[]>(Data);
        }

        public static DDragonObjects.Champions GetChampions(string Version)
        {
            WebClient Client = new();

            string Data = Client.DownloadString($"https://ddragon.leagueoflegends.com/cdn/{Version}/data/en_US/champion.json");

            return JsonConvert.DeserializeObject<DDragonObjects.Champions>(Data);
        }

        public static string GetLatestVersionFormatted()
        {
            WebClient Client = new();

            string Data = Client.DownloadString("https://ddragon.leagueoflegends.com/api/versions.json");

            string[] Versions = JsonConvert.DeserializeObject<string[]>(Data);
            string[] Splitted = Versions[0].Split('.');
            return Splitted[0] + "_" + Splitted[1];
        }
    }
}
