// Decompiled with JetBrains decompiler
// Type: TaleWorlds.MountAndBlade.CustomGameManager
// Assembly: TaleWorlds.MountAndBlade, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5D21862-28AB-45FC-8C12-16AF95A20751
// Assembly location: D:\SteamLibrary\steamapps\common\Mount & Blade II Bannerlord - Beta\bin\Win64_Shipping_Client\TaleWorlds.MountAndBlade.dll

using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
namespace Modbed
{
    public class CaptureTheBannerLordGameManager : MBGameManager
    {
        private int _stepNo;
        private CaptureTheBannerLordParams _params;

        public CaptureTheBannerLordGameManager(CaptureTheBannerLordParams p)
            : base()
        {
            this._params = p;
        }

        public override bool DoLoading(
            )
        {
            bool result = false;
            if (this._stepNo == 0)
            {
                MBGameManager.LoadModuleData(false);
                MBGlobals.InitializeReferences();
                new Game(new CaptureTheBannerLordGame(), this).DoLoading();
                this._stepNo++;
            }
            else if (this._stepNo == 1)
            {
                bool flag = true;
                foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.SubModules)
                {
                    flag = (flag && mbsubModuleBase.DoLoading(Game.Current));
                }
                if (flag)
                {
                    this._stepNo++;
                }
            }
            else if (this._stepNo == 2)
            {
                MBGameManager.StartNewGame();
                this._stepNo++;
            }
            else if (this._stepNo == 3)
            {
                if (Game.Current.DoLoading())
                {
                    this._stepNo++;
                }
            }
            else
            {
                result = true;
            }
            return result;
        
        }


        public override void OnLoadFinished()
        {
            ModuleLogger.Writer.WriteLine("CaptureTheBannerLordGameManager.OnLoadFinished");
            ModuleLogger.Writer.Flush();
            // string scene = "mp_test_bora";
            string scene = "mp_skirmish_map_001a";
            // string scene = "battle_test";
            // string scene = "mp_duel_001_winter";
            // string scene = "mp_sergeant_map_001";
            // string scene = "mp_tdm_map_001";
            // string scene = "scn_world_map";
            // string scene = "mp_compact";
            MissionState.OpenNew(
                "CaptureTheBannerLord",
                new MissionInitializerRecord(scene),
                missionController => new MissionBehaviour[3] {
                    new CaptureTheBannerLordMissionController(this._params),
                    // new BattleTeam1MissionController(),
                    // new TaleWorlds.MountAndBlade.Source.Missions.SimpleMountedPlayerMissionController(),
                    new AgentBattleAILogic(),
                    new FieldBattleController(),
                    // new MissionBoundaryPlacer(),
                }
            );
        }
    }

}