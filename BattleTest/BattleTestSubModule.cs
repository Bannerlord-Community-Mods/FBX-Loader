using System;
using System.ComponentModel;
using System.IO;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.LegacyGUI.Missions;
// using TaleWorlds.MountAndBlade.GauntletUI;
// using TaleWorlds.MountAndBlade.LegacyGUI.Missions;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.View.Screen;
using TL = TaleWorlds.Library;
using TaleWorlds.TwoDimension;

namespace Modbed
{
    public class BattleTestSubModule : TaleWorlds.MountAndBlade.MBSubModuleBase
    {
        private static BattleTestSubModule _instance;
        private bool _initialized;
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            ModuleLogger.Writer.WriteLine("BattleTestSubModule::OnSubModuleLoad");
            BattleTestSubModule._instance = this;
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption(
              "battletest",
              new TextObject("{=battletest}Battle Test", (Dictionary<string, TextObject>)null),
              10001,
              () => ScreenManager.PushScreen(new BattleTestScreen()),
              false
            ));
        }

        protected override void OnSubModuleUnloaded()
        {
            ModuleLogger.Writer.WriteLine("BattleTestSubModule::OnSubModuleUnloaded");
            ModuleLogger.Writer.Close();
            BattleTestSubModule._instance = (BattleTestSubModule)null;
            base.OnSubModuleUnloaded();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (this._initialized)
                return;
            this._initialized = true;
        }

        protected override void OnApplicationTick(float dt)
        {
            // ModuleLogger.Writer.WriteLine("BattleTestSubModule::OnApplicationTick {0}", dt);
            base.OnApplicationTick(dt);
        }
    }
}