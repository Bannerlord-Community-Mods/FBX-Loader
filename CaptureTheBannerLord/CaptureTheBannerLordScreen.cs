using System.Collections.Generic;
using System.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.GauntletUI.PrefabSystem;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.Missions;
using TaleWorlds.MountAndBlade.View.Screen;

namespace Modbed
{
    public class CaptureTheBannerLordScreen : ScreenBase
    {
        private CaptureTheBannerLordVM _datasource;
        private GauntletLayer _gauntletLayer;
        private GauntletMovie _movie;

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this._datasource = new CaptureTheBannerLordVM();
            this._gauntletLayer = new GauntletLayer(100, "GauntletLayer");
            this._gauntletLayer.IsFocusLayer = true;
            this.AddLayer((ScreenLayer)this._gauntletLayer);
            this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            ScreenManager.TrySetFocus((ScreenLayer)this._gauntletLayer);
            ModuleLogger.Writer.WriteLine("before load movie");
            ModuleLogger.Writer.Flush();
            this._movie = this._gauntletLayer.LoadMovie("CaptureTheBannerLordScreen", (ViewModel)this._datasource);
            ModuleLogger.Writer.WriteLine("after load movie");
            ModuleLogger.Writer.Flush();
        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            var input = this._gauntletLayer.Input;
            if (input.IsKeyReleased(InputKey.Escape)) {
                ScreenManager.PopScreen();
                return;
            }
            if (input.IsKeyPressed(InputKey.F5)) {
                this._movie.WidgetFactory.CheckForUpdates();
                this._movie = this._gauntletLayer.LoadMovie("CaptureTheBannerLordScreen", (ViewModel)this._datasource);
            }
        }
    }
}