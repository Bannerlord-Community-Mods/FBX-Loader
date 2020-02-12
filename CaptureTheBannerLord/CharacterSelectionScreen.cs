using System.Collections.Generic;
using System.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Engine.Screens;
using TaleWorlds.GauntletUI;
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
    public class CharacterSelectionScreen : ScreenBase
    {
        private CharacterSelectionVM _datasource;
        private GauntletLayer _gauntletLayer;
        private GauntletMovie _movie;
        private CharacterSelectionParams _params;

        public CharacterSelectionScreen(CharacterSelectionParams p)
            : base()
        {
            this._params = p;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            var vm = new CharacterSelectionVM(this._params);
            this._datasource = vm;
            this._gauntletLayer = new GauntletLayer(100, "GauntletLayer");
            this._gauntletLayer.IsFocusLayer = true;
            this.AddLayer((ScreenLayer)this._gauntletLayer);
            this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            ScreenManager.TrySetFocus((ScreenLayer)this._gauntletLayer);
            this.HandleLoadMovie();
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
                this.HandleLoadMovie();
            }
        }

        private void HandleLoadMovie()
        {
            var vm = this._datasource;
            this._movie = this._gauntletLayer.LoadMovie("CharacterSelectionScreen", (ViewModel)this._datasource);

            var culturesListPanel = this._movie.RootView.Target.FindChild("Cultures", true) as ListPanel;
            var groupsListPanel = this._movie.RootView.Target.FindChild("Groups", true) as ListPanel;
            var charactersListPanel = this._movie.RootView.Target.FindChild("Characters", true) as ListPanel;

            culturesListPanel.IntValue = vm.SelectedCultureIndex;
            groupsListPanel.IntValue = vm.SelectedGroupIndex;
            charactersListPanel.IntValue = vm.SelectedCharacterIndex;
            ModuleLogger.Log("vm.SelectedCharacterIndex {0}", vm.SelectedCharacterIndex);

            culturesListPanel.SelectEventHandlers.Add(w => {
                vm.SelectedCultureChanged(w as ListPanel);
                groupsListPanel.IntValue = vm.SelectedGroupIndex;
                charactersListPanel.IntValue = vm.SelectedCharacterIndex;
            });
            groupsListPanel.SelectEventHandlers.Add(w => {
                vm.SelectedGroupChanged(w as ListPanel);
                charactersListPanel.IntValue = vm.SelectedCharacterIndex;
            });
            charactersListPanel.SelectEventHandlers.Add(w => vm.SelectedCharacterChanged(w as ListPanel));
        }

    }
}