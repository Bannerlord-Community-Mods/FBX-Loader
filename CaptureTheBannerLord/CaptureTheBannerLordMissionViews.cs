using System.Collections.Generic;
using TL = TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions;
using TaleWorlds.MountAndBlade.View.Missions;

namespace Modbed
{
    [ViewCreatorModule]
    public class CaptureTheBannerLordMissionViews
    {
        [ViewMethod("CaptureTheBannerLord")]
        public static MissionView[] OpenTestMission(Mission mission)
        {
            List<MissionView> missionViewList = new List<MissionView>();
            missionViewList.Add(ViewCreator.CreateMissionAgentStatusUIHandler(mission));
            // missionViewList.Add(ViewCreator.CreateMissionAgentLabelUIHandler(mission));
            missionViewList.Add(ViewCreator.CreateOrderTroopPlacerView(mission));
            // missionViewList.Add(ViewCreator.CreateMissionScoreBoardUIHandler(mission, false));
            missionViewList.Add(ViewCreator.CreateMissionKillNotificationUIHandler());
            missionViewList.Add((MissionView)new MissionItemContourControllerView());
            missionViewList.Add((MissionView)new MissionAgentContourControllerView());
            missionViewList.Add(ViewCreator.CreateMissionFlagMarkerUIHandler());
            // missionViewList.Add(ViewCreator.CreateOptionsUIHandler());
            // missionViewList.Add(ViewCreator.CreateMissionBoundaryCrossingView());
            // missionViewList.Add((MissionView) new MissionBoundaryWallView());
            // missionViewList.Add((MissionView) new SpectatorCameraView());
            missionViewList.Add(ViewCreator.CreateMissionOrderUIHandler());
            missionViewList.Add(new CaptureTheBannerLordMissionView(mission));
            return missionViewList.ToArray();
        }
    }

    public class CaptureTheBannerLordMissionView : MissionView
    {
        private Mission _mission;
        public CaptureTheBannerLordMissionView(Mission mission)
            : base()
        {
            this._mission = mission;
        }
        public override void OnMissionScreenActivate()
        {
            foreach (var ml in this._mission.MissionLogics)
            {
                var CaptureTheBannerLordMissionController = ml as CaptureTheBannerLordMissionController;
                if (CaptureTheBannerLordMissionController == null)
                    continue;
                var pos = CaptureTheBannerLordMissionController.freeCameraPosition;
                if (pos == null)
                    break;
                var missionScreen = TaleWorlds.Engine.Screens.ScreenManager.TopScreen as TaleWorlds.MountAndBlade.View.Screen.MissionScreen;
                missionScreen.CombatCamera.Position = pos;
                break;
            }

        }
    }
}