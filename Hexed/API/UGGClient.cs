using Hexed.Objects;
using Newtonsoft.Json.Linq;
using System.Net;

namespace Hexed.API
{
    internal class UGGClient
    {
        private static readonly string APIVersion = "1.5";
        private static readonly string UGGVersion = "1.5.0";

        private const int OVERVIEW_WORLD = 12;
        private const int OVERVIEW_PLATINUM_PLUS = 10;

        public static UGGObjects.ChampionBuild GetChampion(int ChampionID, LeagueObjects.GameMode Mode, LeagueObjects.Role Role)
        {
            JObject Data = GetChampionData(ChampionID, DDragon.GetLatestVersionFormatted(), Mode);

            LeagueObjects.Role MatchingRole = Role == LeagueObjects.Role.RECOMENDED && (Mode == LeagueObjects.GameMode.CLASSIC || Mode == LeagueObjects.GameMode.PRACTICETOOL) ? GetPossibleRoles(Data)[0] : Role;

            UGGObjects.ChampionBuild Build = new()
            {
                Id = ChampionID,
                Runes = GetRunes(Mode, MatchingRole, Data),
                Spells = GetSpellCombos(Mode, MatchingRole, Data),
                ChampionSkill = GetSkillOrder(Mode, MatchingRole, Data),
            };

            return Build;
        }

        private static JObject GetChampionData(int ChampionID, string Version, LeagueObjects.GameMode Mode)
        {
            WebClient Client = new();

            string Data = Client.DownloadString($"https://stats2.u.gg/lol/{APIVersion}/overview/{Version}/{GetGamemode(Mode)}/{ChampionID}/{UGGVersion}.json");

            JObject RawChampionData = JObject.Parse(Data);

            if (Mode != LeagueObjects.GameMode.ARAM)
            {
                var championJObject = (JObject)RawChampionData[OVERVIEW_WORLD.ToString()][OVERVIEW_PLATINUM_PLUS.ToString()];
                return championJObject;
            }
            return RawChampionData;
        }

        private static LeagueObjects.Role[] GetPossibleRoles(JObject jObject)
        {
            JToken championData = jObject;
            int totalGames = championData.Sum(o => ((JProperty)o).Value[0][0][0].ToObject<int>());

            return championData.Cast<JProperty>().Select((o, i) => o.Value[0][0][0].ToObject<float>() / totalGames > 0.1f ? ((LeagueObjects.Role)i + 1) : LeagueObjects.Role.RECOMENDED).Where(r => r != LeagueObjects.Role.RECOMENDED).ToArray();
        }

        private static string GetGamemode(LeagueObjects.GameMode Mode)
        {
            switch (Mode)
            {
                case LeagueObjects.GameMode.PRACTICETOOL:
                case LeagueObjects.GameMode.CLASSIC:
                    return "ranked_solo_5x5";

                case LeagueObjects.GameMode.ARAM:
                    return "normal_aram";
            }

            return null;
        }

        private static UGGObjects.Rune[] GetRunes(LeagueObjects.GameMode gm, LeagueObjects.Role role, JObject championData)
        {
            List<UGGObjects.Rune> runes = new();

            if (gm != LeagueObjects.GameMode.ARAM)
            {
                var root = championData[((int)role).ToString()].FirstOrDefault();
                var rune = FilterRune(root, role.ToString());
                runes.Add(rune);

                return runes.ToArray();
            }

            List<JToken> tokens = new();
            foreach (var item in championData.Children())
            {
                item.ToList().ForEach(i => tokens.Add(i["8"]["6"]));
            }

            foreach (var token in tokens)
            {
                var root = token.FirstOrDefault();
                var runeTemp = FilterRune(root, "Aram");

                runes.Add(runeTemp);
            }

            if (runes.Count == 0) return null;

            return runes.ToArray();
        }

        private static UGGObjects.Rune FilterRune(JToken root, string runeName)
        {
            var rune = new UGGObjects.Rune();

            var perksRoot = root.First();
            var shardsRoot = root[8][2];

            var primaryPath = perksRoot[2].ToObject<int>();
            var secondaryPath = perksRoot[3].ToObject<int>();
            var runeIds = perksRoot[4].Select(p => p.ToObject<int>()).Concat(shardsRoot.Select(s => int.Parse(s.ToString()))).ToArray();

            rune.Name = runeName;
            rune.PrimaryPath = primaryPath;
            rune.Keystone = runeIds[0];

            var primaryPerks = Boot.Runes.Where(p => p.id == rune.PrimaryPath).ToList();

            int[] primaryPerksFound = new int[2];
            int primaryPerkSlot = 1;

            for (int i = 0; i < 3; i++)
            {
                foreach (var path in primaryPerks)
                {
                    foreach (var runeInfo in path.slots[primaryPerkSlot].runes)
                    {
                        if (runeIds.Contains(runeInfo.id))
                        {
                            switch (primaryPerkSlot)
                            {
                                case 1:
                                    rune.Slot1 = runeInfo.id;
                                    break;
                                case 2:
                                    rune.Slot2 = runeInfo.id;
                                    break;
                                case 3:
                                    rune.Slot3 = runeInfo.id;
                                    break;
                            }
                            primaryPerkSlot++;
                            break;
                        }
                    }
                    if (primaryPerkSlot > i) break;
                }
            }

            rune.SecondaryPath = secondaryPath;

            int secondaryPerkFound = 0;
            int tempSecondaryPerkFound = 0;

            var availableRunes = Boot.Runes.Where(p => p.id == rune.SecondaryPath).SelectMany(path => path.slots).SelectMany(slot => slot.runes).Where(runeInfo => runeIds.Contains(runeInfo.id) && runeInfo.id != tempSecondaryPerkFound);

            foreach (var runeInfo in availableRunes)
            {
                if (secondaryPerkFound == 0)
                {
                    tempSecondaryPerkFound = runeInfo.id;
                    rune.Slot4 = runeInfo.id;
                    secondaryPerkFound++;
                }
                else if (secondaryPerkFound == 1)
                {
                    rune.Slot5 = runeInfo.id;
                    break;
                }
            }

            rune.Shard1 = runeIds[6];
            rune.Shard2 = runeIds[7];
            rune.Shard3 = runeIds[8];

            return rune;
        }

        private static UGGObjects.Spell[] GetSpellCombos(LeagueObjects.GameMode gm, LeagueObjects.Role role, JObject championData)
        {
            List<UGGObjects.Spell> spells = new();

            if (gm != LeagueObjects.GameMode.ARAM)
            {
                var root = championData[((int)role).ToString()].FirstOrDefault();
                var spellRoots = root[1][2];

                var spellTemp = new UGGObjects.Spell
                {
                    First = spellRoots[0].ToObject<int>(),
                    Second = spellRoots[1].ToObject<int>()
                };
                spells.Add(spellTemp);
            }
            else
            {
                List<JToken> tokens = new List<JToken>();
                foreach (var item in championData.Children())
                {
                    item.ToList().ForEach(i => tokens.Add(i["8"]["6"]));
                }

                foreach (var token in tokens)
                {
                    var spellTemp = new UGGObjects.Spell();

                    var root = token.First();
                    var spellRoots = root[1][2];

                    spellTemp.First = spellRoots[0].ToObject<int>();
                    spellTemp.Second = spellRoots[1].ToObject<int>();

                    spells.Add(spellTemp);
                }

                if (spells.Count == 0) return null;
            }

            return spells.ToArray();
        }

        private static UGGObjects.ChampionSkill GetSkillOrder(LeagueObjects.GameMode gm, LeagueObjects.Role role, JObject championData)
        {
            UGGObjects.ChampionSkill championSkill = new();

            var root = gm != LeagueObjects.GameMode.ARAM ? championData[((int)role).ToString()].FirstOrDefault() : championData.Children().FirstOrDefault();
            var champSkill = gm != LeagueObjects.GameMode.ARAM ? root[4] : root.FirstOrDefault()["8"]["6"][0][4];


            var order = champSkill[2].Select(o => o.ToString()).ToArray();
            championSkill.Order = new UGGObjects.ChampSkill[order.Length];

            for (int i = 0; i < order.Length; i++)
            {
                championSkill.Order[i] = new UGGObjects.ChampSkill()
                {
                    Index = i + 1,
                    Skill = order[i]
                };
            }

            championSkill.Priority = champSkill[3].ToString();

            return championSkill;
        }
    }
}
