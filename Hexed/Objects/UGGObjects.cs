namespace Hexed.Objects
{
    internal class UGGObjects
    {
        public class Rune
        {
            public string Name { get; set; }
            public int PrimaryPath { get; set; }
            public int SecondaryPath { get; set; }
            public int Keystone { get; set; }
            public int Slot1 { get; set; }
            public int Slot2 { get; set; }
            public int Slot3 { get; set; }
            public int Slot4 { get; set; }
            public int Slot5 { get; set; }
            public int Shard1 { get; set; }
            public int Shard2 { get; set; }
            public int Shard3 { get; set; }
        }

        public class ChampionBuild
        {
            public int Id { get; set; }
            public ChampionSkill ChampionSkill { get; set; }
            public LeagueObjects.Role Role { get; set; }
            public Rune[] Runes { get; set; }
            public Spell[] Spells { get; set; }
        }

        public class Spell
        {
            public int First { get; set; }
            public int Second { get; set; }
        }

        public class ChampionSkill
        {
            public ChampSkill[] Order { get; set; }
            public string Priority { get; set; }
        }

        public class ChampSkill
        {
            public int Index { get; set; }
            public string Skill { get; set; }
        }
    }
}
