using System.Collections.Generic;

using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace Modbed
{
    public class BattleTestVM : ViewModel
    {
        static BattleTestParams lastParams = BattleTestParams.createDefault();

        private string _text = "hello";
        private string _playerSoldierCount, _enemySoldierCount;
        private string _distance;
        private string _soldierXInterval, _soldierYInterval;
        private string _soldiersPerRow;
        private string _tempVar;

        private string _playerCharacterName;
        private string _playerSoldierCharacterName;
        private string _enemySoldierCharacterName;

        private CharacterInfo _playerCharacter;
        private CharacterInfo _playerSoldierCharacter;
        private CharacterInfo _enemySoldierCharacter;

        private List<CharacterInfo> _allCharacters;

        [DataSourceProperty]
        public string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                if (value == this._text)
                    return;
                this._text= value;
                this.OnPropertyChanged(nameof(Text));
            }
        }

        [DataSourceProperty]
        public string PlayerSoldierCount
        {
            get
            {
                return this._playerSoldierCount;
            }
            set
            {
                if (value == this._playerSoldierCount)
                    return;
                this._playerSoldierCount = value;
                this.OnPropertyChanged(nameof(PlayerSoldierCount));
            }
        }

        [DataSourceProperty]
        public string EnemySoldierCount
        {
            get
            {
                return this._enemySoldierCount;
            }
            set
            {
                if (value == this._enemySoldierCount)
                    return;
                this._enemySoldierCount = value;
                this.OnPropertyChanged(nameof(EnemySoldierCount));
            }
        }

        [DataSourceProperty]
        public string Distance
        {
            get
            {
                return this._distance;
            }
            set
            {
                if (value == this._distance)
                    return;
                this._distance = value;
                this.OnPropertyChanged(nameof(Distance));
            }
        }

        [DataSourceProperty]
        public string SoldierXInterval
        {
            get
            {
                return this._soldierXInterval;
            }
            set
            {
                if (value == this._soldierXInterval)
                    return;
                this._soldierXInterval = value;
                this.OnPropertyChanged(nameof(SoldierXInterval));
            }
        }

        [DataSourceProperty]
        public string SoldierYInterval
        {
            get
            {
                return this._soldierYInterval;
            }
            set
            {
                if (value == this._soldierYInterval)
                    return;
                this._soldierYInterval = value;
                this.OnPropertyChanged(nameof(SoldierYInterval));
            }
        }
    
        [DataSourceProperty]
        public string SoldiersPerRow
        {
            get
            {
                return this._soldiersPerRow;
            }
            set
            {
                if (value == this._soldiersPerRow)
                    return;
                this._soldiersPerRow = value;
                this.OnPropertyChanged(nameof(SoldiersPerRow));
            }
        }

        [DataSourceProperty]
        public string TempVar
        {
            get
            {
                return this._tempVar;
            }
            set
            {
                if (value == this._tempVar)
                    return;
                this._tempVar = value;
                this.OnPropertyChanged(nameof(TempVar));
            }
        }

        [DataSourceProperty]
        public string PlayerCharacterName
        {
            get
            {
                return this._playerCharacterName;
            }
            set
            {
                if (value == this._playerCharacterName)
                    return;
                this._playerCharacterName = value;
                this.OnPropertyChanged(nameof(PlayerCharacterName));
            }
        }

        public CharacterInfo PlayerCharacter
        {
            get
            {
                return this._playerCharacter;
            }
            set
            {
                this._playerCharacter = value;
                this.PlayerCharacterName = value?.Name;
            }
        }

        [DataSourceProperty]
        public string PlayerSoldierCharacterName
        {
            get
            {
                return this._playerSoldierCharacterName;
            }
            set
            {
                if (value == this._playerSoldierCharacterName)
                    return;
                this._playerSoldierCharacterName = value;
                this.OnPropertyChanged(nameof(PlayerSoldierCharacterName));
            }
        }

        public CharacterInfo PlayerSoldierCharacter
        {
            get
            {
                return this._playerSoldierCharacter;
            }
            set
            {
                this._playerSoldierCharacter = value;
                this.PlayerSoldierCharacterName = value?.Name;
            }
        }

        [DataSourceProperty]
        public string EnemySoldierCharacterName
        {
            get
            {
                return this._enemySoldierCharacterName;
            }
            set
            {
                if (value == this._enemySoldierCharacterName)
                    return;
                this._enemySoldierCharacterName = value;
                this.OnPropertyChanged(nameof(EnemySoldierCharacterName));
            }
        }

        public CharacterInfo EnemySoldierCharacter
        {
            get
            {
                return this._enemySoldierCharacter;
            }
            set
            {
                this._enemySoldierCharacter = value;
                this.EnemySoldierCharacterName = value?.Name;
            }
        }


        public string SkyBrightness { get; set; }
        public string RainDensity { get; set; }
        public string FormationPosition { get; set; }
        public string FormationDirection { get; set; }

        public MBBindingList<AgentInfoVM> AgentInfoList { get; set; }
        public int SelectedIndex { get; set; }
        public bool UseFreeCamera { get; set; }


        public BattleTestVM()
            : base()
        {
            this._playerSoldierCount = lastParams.playerSoldierCount.ToString();
            this._enemySoldierCount = lastParams.enemySoldierCount.ToString();
            this._distance = lastParams.distance.ToString();
            this._soldierXInterval = lastParams.soldierXInterval.ToString();
            this._soldierYInterval = lastParams.soldierYInterval.ToString();
            this._soldiersPerRow = lastParams.soldiersPerRow.ToString();
            this.FormationPosition = string.Format("{0},{1}", lastParams.FormationPosition.x, lastParams.FormationPosition.y);
            this.FormationDirection = string.Format("{0},{1}", lastParams.formationDirection.x, lastParams.formationDirection.y);
            this.SkyBrightness = lastParams.skyBrightness.ToString();
            this.RainDensity = lastParams.rainDensity.ToString();
            this._tempVar = lastParams.tempVar.ToString();
            this.UseFreeCamera = lastParams.useFreeCamera;

            this.AgentInfoList = new MBBindingList<AgentInfoVM>() {
                new AgentInfoVM() { Name = "hello1" },
                new AgentInfoVM() { Name = "hello2" },
                new AgentInfoVM() { Name = "hello3" },
                new AgentInfoVM() { Name = "hello4" },
            };
            var characters = getCharacters();
            foreach (var c in characters)
            {
                var nameTextObject = new TaleWorlds.Localization.TextObject(c.name);
                var name = nameTextObject.ToString();
                this.AgentInfoList.Add(new AgentInfoVM() {
                    Name = name,
                });
            }
            this.SelectedIndex = 0;
            this.PlayerCharacter = characters.Find(c => c.id == lastParams.playerCharacterId);
            this.PlayerSoldierCharacter = characters.Find(c => c.id == lastParams.playerSoldierCharacterId);
            this.EnemySoldierCharacter = characters.Find(c => c.id == lastParams.enemySoldierCharacterId);

            if (this.PlayerCharacter == null) this.PlayerCharacter = characters[0];
            if (this.PlayerSoldierCharacter == null) this.PlayerSoldierCharacter = characters[0];
            if (this.EnemySoldierCharacter == null) this.EnemySoldierCharacter = characters[0];
            this._allCharacters = characters;
        }

        private void StartBattleTest() {
            var p = new BattleTestParams();
            try {
                p.playerSoldierCount = System.Convert.ToInt32(this.PlayerSoldierCount);
                p.enemySoldierCount = System.Convert.ToInt32(this.EnemySoldierCount);
                p.distance = System.Convert.ToSingle(this.Distance);
                p.soldierXInterval = System.Convert.ToSingle(this.SoldierXInterval);
                p.soldierYInterval = System.Convert.ToSingle(this.SoldierYInterval);
                p.soldiersPerRow = System.Convert.ToInt32(this.SoldiersPerRow);
                var posParts = this.FormationPosition.Split(',');
                var dirParts = this.FormationDirection.Split(',');
                p.FormationPosition = new Vec2(System.Convert.ToSingle(posParts[0]), System.Convert.ToSingle(posParts[1]));
                p.formationDirection = (new Vec2(System.Convert.ToSingle(dirParts[0]), System.Convert.ToSingle(dirParts[1]))).Normalized();
                p.skyBrightness = System.Convert.ToSingle(this.SkyBrightness);
                p.rainDensity = System.Convert.ToSingle(this.RainDensity);
                p.tempVar = System.Convert.ToSingle(this.TempVar);
                p.playerCharacterId = this.PlayerCharacter.id;
                p.playerSoldierCharacterId = this.PlayerSoldierCharacter.id;
                p.enemySoldierCharacterId = this.EnemySoldierCharacter.id;
                p.useFreeCamera = this.UseFreeCamera;
            } catch {
                return;
            }
            if (!p.validate()) {
                return;
            }
            lastParams = p;
            ModuleLogger.Writer.WriteLine("StartBattleTest");
            MBGameManager.StartNewGame(new BattleTestGameManager(p));
        }

        private void GoBack() {
            TaleWorlds.Engine.Screens.ScreenManager.PopScreen();
        }

        private void SelectPlayerCharacter() {
            ModuleLogger.Log("SelectPlayerCharacter");
            var p = new CharacterSelectionParams {
                characters = this._allCharacters,
                selectedIndex = this._allCharacters.IndexOf(this.PlayerCharacter),
                setCharacter = (c => PlayerCharacter = c),
            };
            TaleWorlds.Engine.Screens.ScreenManager.PushScreen(new CharacterSelectionScreen(p));
        }

        private void SelectPlayerSoldierCharacter() {
            ModuleLogger.Log("SelectPlayerSoldierCharacter");
            var p = new CharacterSelectionParams {
                characters = this._allCharacters,
                selectedIndex = this._allCharacters.IndexOf(this.PlayerSoldierCharacter),
                setCharacter = (c => PlayerSoldierCharacter = c),
            };
            TaleWorlds.Engine.Screens.ScreenManager.PushScreen(new CharacterSelectionScreen(p));
        }

        private void SelectEnemySoldierCharacter() {
            ModuleLogger.Log("SelectPlayerCharacter");
            var p = new CharacterSelectionParams {
                characters = this._allCharacters,
                selectedIndex = this._allCharacters.IndexOf(this.EnemySoldierCharacter),
                setCharacter = (c => EnemySoldierCharacter = c),
            };
            TaleWorlds.Engine.Screens.ScreenManager.PushScreen(new CharacterSelectionScreen(p));
        }

        private List<CharacterInfo> getCharacters()
        {
            var characters = new List<CharacterInfo>();
            var p = BasePath.Name + "Modules/Native/ModuleData/MPCharacters.xml";
            var streamReader = new System.IO.StreamReader(p);
            var xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(streamReader);
            var rootNode = xmlDoc.ChildNodes[1];
            ModuleLogger.Writer.WriteLine("rootName.name {0}", rootNode.Name);
            ModuleLogger.Writer.Flush();
            if (rootNode.Name != "MPCharacters")
            {
                throw new System.Exception(rootNode.Name);
            }
            for (var i = 0; i < rootNode.ChildNodes.Count; i += 1)
            {
                var child = rootNode.ChildNodes[i];
                if (child.Name != "NPCCharacter")
                {
                    continue;
                }
                var id = child.Attributes["id"].Value;
                var name = child.Attributes["name"].Value;
                var culture = child.Attributes["culture"]?.Value;
                var defaultGroup = child.Attributes["default_group"]?.Value;
                if (culture == null) {
                    culture = "other";
                } else if (culture.StartsWith("Culture.")) {
                    culture = culture.Substring("Culture.".Length);
                }
                if (defaultGroup == null) {
                    defaultGroup = "other";
                }
                var character = new CharacterInfo {
                    id = id,
                    name = name,
                    culture = culture,
                    defaultGroup = defaultGroup,
                };
                characters.Add(character);
            }
            return characters;
        }
    }

    public class AgentInfoVM : ViewModel
    {
        public string Name { get; set; }
    }

    public class CharacterInfo {
        public string id;
        public string name;
        public string culture;
        public string defaultGroup;
        private string _name;

        public string Name {
            get {
                var s = new TextObject(this.name).ToString();
                if (this.id.EndsWith("troop")) {
                    return s + " (troop)";
                } else if (this.id.EndsWith("hero")) {
                    return s + " (hero)";
                } else {
                    return s;
                }
            }
        }
    }
}