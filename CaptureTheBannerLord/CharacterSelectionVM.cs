using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace Modbed
{
    public class CharacterSelectionVM : ViewModel
    {
        SortedDictionary<string, SortedDictionary<string, List<CharacterInfo>>> allCharacters;
        CharacterSelectionParams _params;
        bool _inChange;

        MBBindingList<NameVM> _cultures;
        MBBindingList<NameVM> _groups;
        MBBindingList<CharacterVM> _characters;

        public int SelectedCultureIndex { get; set; }

        public int SelectedGroupIndex { get; set; }

        public int SelectedCharacterIndex { get; set; }

        [DataSourceProperty]
        public MBBindingList<NameVM> Cultures
        {
            get
            {
                return this._cultures;
            }
            set
            {
                if (value == this._cultures)
                    return;
                this._cultures = value;
                this.OnPropertyChanged(nameof(Cultures));
            }
        }

        [DataSourceProperty]
        public MBBindingList<NameVM> Groups
        {
            get
            {
                return this._groups;
            }
            set
            {
                if (value == this._groups)
                    return;
                this._groups = value;
                this.OnPropertyChanged(nameof(Groups));
            }
        }
        [DataSourceProperty]
        public MBBindingList<CharacterVM> Characters
        {
            get
            {
                return this._characters;
            }
            set
            {
                if (value == this._characters)
                    return;
                this._characters = value;
                this.OnPropertyChanged(nameof(Characters));
            }
        }
        

        public CharacterSelectionVM(CharacterSelectionParams p)
            : base()
        {
            ModuleLogger.Log("begin character selection vm construction");
            this._params = p;
            this.allCharacters = new SortedDictionary<string, SortedDictionary<string, List<CharacterInfo>>>();
            foreach (var c1 in p.characters)
            {
                if (!this.allCharacters.ContainsKey(c1.culture)) {
                    this.allCharacters.Add(c1.culture, new SortedDictionary<string, List<CharacterInfo>>());
                }
                var cultureDict = this.allCharacters[c1.culture];
                if (!cultureDict.ContainsKey(c1.defaultGroup)) {
                    cultureDict.Add(c1.defaultGroup, new List<CharacterInfo>());
                }
                cultureDict[c1.defaultGroup].Add(c1);
            }
            var c = p.characters[p.selectedIndex];
            Cultures = new MBBindingList<NameVM>();
            foreach (var culture in this.allCharacters.Keys)
            {
                Cultures.Add(new NameVM { Name = culture });
            }
            Groups = new MBBindingList<NameVM>();
            foreach (var group in this.allCharacters[c.culture].Keys)
            {
                Groups.Add(new NameVM { Name = group });
            }
            Characters = new MBBindingList<CharacterVM>();
            foreach (var character in this.allCharacters[c.culture][c.defaultGroup])
            {
                Characters.Add(new CharacterVM(character));
            }
            SelectedCultureIndex = Cultures.FindIndex(n => n.Name == c.culture);
            SelectedGroupIndex = Groups.FindIndex(n => n.Name == c.defaultGroup);
            SelectedCharacterIndex = Characters.FindIndex(n => n.character == c);
            ModuleLogger.Log("end character selection vm construction");
        }

        public void SelectedCultureChanged(ListPanel listPanel)
        {
            this._inChange = true;
            try {
            var index = listPanel.IntValue;
            ModuleLogger.Log("SelectedCultureChanged {0}", index);
            var culture = Cultures[index].Name;
            string groupName = null;
            Groups.Clear();
            Characters.Clear();
            foreach (var group in this.allCharacters[culture].Keys)
            {
                if (groupName == null) groupName = group;
                Groups.Add(new NameVM { Name = group });
            }
            foreach (var character in this.allCharacters[culture][groupName])
            {
                Characters.Add(new CharacterVM(character));
            }
            SelectedCultureIndex = index;
            SelectedGroupIndex = 0;
            SelectedCharacterIndex = 0;
            } catch (System.Exception e) {
                ModuleLogger.Log("{0}", e);
                throw;
            }
            this._inChange = false;
        }

        public void SelectedGroupChanged(ListPanel listPanel)
        {
            var index = listPanel.IntValue;
            if (index < 0 || this._inChange) return;
            this._inChange = true;
            ModuleLogger.Log("SelectedGroupChanged {0} {1}", index, Groups.Count);
            var culture = Cultures[SelectedCultureIndex].Name;
            string group = Groups[index].Name;
            Characters.Clear();
            foreach (var character in this.allCharacters[culture][group])
            {
                Characters.Add(new CharacterVM(character));
            }
            SelectedGroupIndex = index;
            SelectedCharacterIndex = 0;
            this._inChange = false;
        }

        public void SelectedCharacterChanged(ListPanel listPanel)
        {
            var index = listPanel.IntValue;
            if (index < 0 || this._inChange) return;
            ModuleLogger.Log("SelectedCharacterChanged {0}", index);
            SelectedCharacterIndex = index;
        }

        private void Done()
        {
            var character = Characters[this.SelectedCharacterIndex].character;
            this._params.setCharacter(character);
            TaleWorlds.Engine.Screens.ScreenManager.PopScreen();
        }
    }

    public class NameVM : ViewModel
    {
        public string Name { get; set; }
    }

    public class CharacterVM : ViewModel
    {
        public CharacterInfo character;
        public CharacterVM(CharacterInfo character)
        {
            this.character = character;
        }
        public string Name { get { return this.character.Name; } }
    }

    public class CharacterSelectionParams {
        public List<CharacterInfo> characters;
        public int selectedIndex;
        public System.Action<CharacterInfo> setCharacter;
    } 
}