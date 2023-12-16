using Hexed.API;
using Hexed.LCU;
using Hexed.Modules;

namespace Hexed.Core
{
    internal class SocketManager
    {
        public static void Init()
        {
            APIClient.leagueClient.SubscribeEvent("OnJsonApiEvent_lol-gameflow_v1_gameflow-phase", OnGameflowChanged);
            APIClient.leagueClient.SubscribeEvent("OnJsonApiEvent_lol-champ-select_v1_current-champion", OnChampionSelected);
        }

        private static void OnGameflowChanged(OnWebsocketEventArgs obj)
        {
            if (obj.Data == null) return;

            switch (obj.Data.ToString())
            {
                case "ReadyCheck":
                    APIClient.AcceptReadyCheck();
                    break;

                case "ChampSelect":
                    //APIClient.GetCurrentParticipants();
                    break;
            }
        }

        private static void OnChampionSelected(OnWebsocketEventArgs obj)
        {
            if (obj.Data == null) return;

            int ChampionId = Convert.ToInt32(obj.Data.ToString());

            BuildMaker.RecreateBuild(ChampionId);
        }
    }
}
