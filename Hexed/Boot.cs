using Hexed.API;
using Hexed.Core;
using Hexed.Objects;
using Hexed.Wrappers;
using System.Diagnostics;

namespace Hexed
{
    internal class Boot
    {
        public static DDragonObjects.Champions Champions;
        public static DDragonObjects.Perk[] Runes;

        public static void Main()
        {
            Process LeagueProc = Utils.GetProcessByName("LeagueClient");
            if (LeagueProc == null)
            {
                Logger.LogError("League of Legends is not running");
                Thread.Sleep(-1);
            }

            APIClient.leagueClient = new(LeagueProc);

            string Version = DDragon.GetLatestVersion();
            Logger.LogDebug($"Running on Version {Version}");

            Champions = DDragon.GetChampions(Version);
            Logger.LogDebug($"Fetched {Champions.Data.Count} Champions");

            Runes = DDragon.GetRunes(Version);
            Logger.LogDebug($"Fetched {Runes.Length} Runes");

            LeagueObjects.LolSummonerSummoner Summoner = APIClient.GetCurrentSummoner();

            Logger.Log($"Logged in as {Summoner.displayName} [{Summoner.puuid}]");

            SocketManager.Init();

            LeagueProc.WaitForExit();
        }
    }
}
