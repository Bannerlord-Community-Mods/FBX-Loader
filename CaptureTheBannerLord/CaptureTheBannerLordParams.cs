
using TaleWorlds.Library;

namespace Modbed
{
    public struct CaptureTheBannerLordParams
    {
        public int playerSoldierCount, enemySoldierCount;
        public float distance;
        public float soldierXInterval, soldierYInterval;
        public int soldiersPerRow;
        public Vec2 FormationPosition;
        public Vec2 formationDirection;
        public float skyBrightness;
        public float rainDensity;
        public float tempVar;
        public string playerCharacterId;
        public string playerSoldierCharacterId;
        public string enemySoldierCharacterId;
        public bool useFreeCamera;
        
        public static CaptureTheBannerLordParams createDefault() {
            var p = new CaptureTheBannerLordParams();
            p.playerSoldierCount = 1;
            p.enemySoldierCount = 0;
            // p.playerSoldierCount = 50;
            // p.enemySoldierCount = 1;
            p.distance = 1;
            p.soldierXInterval = 5f;
            p.soldierYInterval = 1.5f;
            p.soldiersPerRow = 100;
            // p.soldiersPerRow = 10;
            p.FormationPosition = new Vec2(150, 100);
            p.formationDirection = new Vec2(1, 0);
            p.skyBrightness = -1;
            p.rainDensity = 0;
            p.tempVar = 0.0f;
            p.playerCharacterId = "mp_light_cavalry_vlandia_hero";
            p.playerSoldierCharacterId = "mp_heavy_infantry_vlandia_troop";
            p.enemySoldierCharacterId = "mp_heavy_ranged_aserai_troop";
            p.useFreeCamera = false;
            return p;
        }

        public bool validate() {
            return this.playerSoldierCount >= 0
                && this.enemySoldierCount >= 0
                && this.distance > 0
                && soldierXInterval > 0
                && soldierYInterval > 0
                && soldiersPerRow > 0
                && formationDirection.Length > 0
                && playerCharacterId != null
                && playerSoldierCharacterId != null
                && enemySoldierCharacterId != null
            ;
        }
    }
}