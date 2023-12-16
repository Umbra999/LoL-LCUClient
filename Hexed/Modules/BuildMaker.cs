using Hexed.API;
using Hexed.Objects;
using Hexed.Wrappers;

namespace Hexed.Modules
{
    internal class BuildMaker
    {
        private static void RecreateRunes(UGGObjects.ChampionBuild ChampData, LeagueObjects.GameMode Mode)
        {
            LeagueObjects.LolPerksPerkPageResource CurrentPage = APIClient.GetCurrentRunePage();

            APIClient.DeleteRunePageById(CurrentPage.id);

            int[] PerkIds = new int[9];
            PerkIds[0] = ChampData.Runes[0].Keystone;
            PerkIds[1] = ChampData.Runes[0].Slot1;
            PerkIds[2] = ChampData.Runes[0].Slot2;
            PerkIds[3] = ChampData.Runes[0].Slot3;
            PerkIds[4] = ChampData.Runes[0].Slot4;
            PerkIds[5] = ChampData.Runes[0].Slot5;
            PerkIds[6] = ChampData.Runes[0].Shard1;
            PerkIds[7] = ChampData.Runes[0].Shard2;
            PerkIds[8] = ChampData.Runes[0].Shard3;

            var LeagueChamp = APIClient.GetChampionById(ChampData.Id);

            LeagueObjects.LolPerksPerkPageResource NewPage = new()
            {
                name = $"{Mode} - {ChampData.Runes[0].Name} - {LeagueChamp.name}",
                primaryStyleId = ChampData.Runes[0].PrimaryPath,
                subStyleId = ChampData.Runes[0].SecondaryPath,
                selectedPerkIds = PerkIds,
                current = true,
            };

            APIClient.CreateRunePage(NewPage);
        }

        private static void RecreateSpells(UGGObjects.ChampionBuild ChampData, LeagueObjects.GameMode Mode)
        {

        }

        private static void PrintOrder(UGGObjects.ChampionBuild ChampData)
        {
            var LeagueChamp = APIClient.GetChampionById(ChampData.Id);

            Logger.Log($"{LeagueChamp.name} Skillorder: {ChampData.ChampionSkill.Priority}");
        }

        public static void RecreateBuild(int ChampionId)
        {
            LeagueObjects.GameMode Gamemode = APIClient.GetCurrentLobby().gameConfig.gameMode;
            if (Gamemode == LeagueObjects.GameMode.TFT || Gamemode == LeagueObjects.GameMode.NONE) return;

            // add code to get role
            UGGObjects.ChampionBuild ChampData = UGGClient.GetChampion(ChampionId, Gamemode, LeagueObjects.Role.RECOMENDED);

            RecreateRunes(ChampData, Gamemode);
            RecreateSpells(ChampData, Gamemode);
            PrintOrder(ChampData);

            Logger.Log("Build has been recreated");
        }
    }
}
