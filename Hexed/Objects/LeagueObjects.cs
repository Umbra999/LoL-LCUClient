namespace Hexed.Objects
{
    internal class LeagueObjects
    {
        public class LolSummonerSummoner
        {
            public long accountId { get; set; }
            public string displayName { get; set; }
            public string internalName { get; set; }
            public bool nameChangeFlag { get; set; }
            public int percentCompleteForNextLevel { get; set; }
            public string privacy { get; set; } // Enum
            public int profileIconId { get; set; }
            public string puuid { get; set; }
            public LolSummonerSummonerRerollPoints rerollPoints { get; set; }
            public long summonerId { get; set; }
            public int summonerLevel { get; set; }
            public bool unnamed { get; set; }
            public long xpSinceLastLevel { get; set; }
            public long xpUntilNextLevel { get; set; }
        }

        public class LolSummonerSummonerRerollPoints
        {
            public int currentPoints { get; set; }
            public int maxRolls { get; set; }
            public int numberOfRolls { get; set; }
            public int pointsCostToRoll { get; set; }
            public int pointsToReroll { get; set; }
        }

        public class LolPerksPerkPageResource
        {
            public int[] autoModifiedSelections { get; set; }
            public bool current { get; set; }
            public int id { get; set; }
            public bool isActive { get; set; }
            public bool isDeletable { get; set; }
            public bool isEditable { get; set; }
            public bool isValid { get; set; }
            public bool isTemporary { get; set; }
            public long lastModified { get; set; }
            public string name { get; set; }
            public int order { get; set; }
            public int primaryStyleId { get; set; }
            public int[] selectedPerkIds { get; set; }
            public int subStyleId { get; set; }
        }

        public class LolChampSelectChampSelectSummoner
        {
            public string actingBackgroundAnimationState { get; set; }
            public string activeActionType { get; set; }
            public bool areSummonerActionsComplete { get; set; }
            public string assignedPosition { get; set; }
            public string banIntentSquarePortratPath { get; set; }
            public long cellId { get; set; }
            public string championIconStyle { get; set; }
            public string championName { get; set; }
            public int currentChampionVotePercentInteger { get; set; }
            public string entitledFeatureType { get; set; }
            public bool isActingNow { get; set; }
            public bool isDonePicking { get; set; }
            public bool isOnPlayersTeam { get; set; }
            public bool isPickIntenting { get; set; }
            public bool isPlaceholder { get; set; }
            public bool isSelf { get; set; }
            public string nameVisibilityType { get; set; }
            public string obfuscatedPuuid { get; set; }
            public long obfuscatedSummonerId { get; set; }
            public string pickSnipedClass { get; set; }
            public string puuid { get; set; }
            public bool shouldShowActingBar { get; set; }
            public bool shouldShowBanIntentIcon { get; set; }
            public bool shouldShowExpanded { get; set; }
            public bool shouldShowRingAnimations { get; set; }
            public bool shouldShowSelectedSkin { get; set; }
            public bool shouldShowSpells { get; set; }
            public bool showMuted { get; set; }
            public bool showSwaps { get; set; }
            public bool showTrades { get; set; }
            public int skinId { get; set; }
            public string skinSplashPath { get; set; }
            public int slotId { get; set; }
            public string spell1IconPath { get; set; }
            public string spell2IconPath { get; set; }
            public string statusMessageKey { get; set; }
            public long summonerId { get; set; }
            public long swapId { get; set; }
            public long tradeId { get; set; }
            public int championId { get; set; }
        }

        public class LolLobbyLobbyGameConfigDto
        {
            public GameMode gameMode { get; set; }
        }

        public class LolLobbyLobbyDto
        {
            public LolLobbyLobbyGameConfigDto gameConfig { get; set; }
        }

        public class LolChampSelectChampGridChampion
        {
            public bool disabled { get; set; }
            public bool freeToPlay { get; set; }
            public bool freeToPlayForQueue { get; set; }
            public bool freeToPlayReward { get; set; }
            public int id { get; set; }
            public bool masteryChestGranted { get; set; }
            public int masteryLevel { get; set; }
            public int masteryPoints { get; set; }
            public string name { get; set; }
            public bool owned { get; set; }
            public string[] positionsFavorited { get; set; }
            public bool rented { get; set; }
            public string[] roles { get; set; }
            public LolChampSelectChampionSelection selectionStatus { get; set; }
            public string squarePortraitPath { get; set; }
        }

        public class LolChampSelectChampionSelection
        {
            public bool banIntented { get; set; }
            public bool banIntentedByMe { get; set; }
            public bool isBanned { get; set; }
            public bool pickIntented { get; set; }
            public bool pickIntentedByMe { get; set; }
            public string pickIntentedPosition { get; set; }
            public bool pickedByOtherOrBanned { get; set; }
            public bool selectedByMe { get; set; }
        }

        public enum GameMode
        {
            NONE,
            ARAM,
            CLASSIC,
            PRACTICETOOL,
            ULTBOOK,
            TFT,
            ONEFORALL,
            URF,
            ARURF
        }

        public enum Role
        {
            RECOMENDED = 0,
            TOP = 4,
            JUNGLE = 1,
            MID = 5,
            ADC = 3,
            SUPPORT = 2,
        }
    }
}
