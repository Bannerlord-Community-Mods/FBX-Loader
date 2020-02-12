using System;
using System.Collections.Generic;
using System.Reflection;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TL = TaleWorlds.Library;

namespace Modbed
{
    public class BlockMissionObject : SynchedMissionObject
    {
        public override void AddStuckMissile(GameEntity missileEntity)
        {
            base.AddStuckMissile(missileEntity);
        }

        public override void AfterMissionStart()
        {
            base.AfterMissionStart();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void OnEndMission()
        {
            base.OnEndMission();
        }

        public override bool ReadFromNetwork()
        {
            return base.ReadFromNetwork();
        }

        public override void SetDisabledSynched()
        {
            base.SetDisabledSynched();
        }

        public override void SetPhysicsStateSynched(bool value, bool setChildren = true)
        {
            base.SetPhysicsStateSynched(value, setChildren);
        }

        public override void SetTeamColorsSynched(uint color, uint color2)
        {
            base.SetTeamColorsSynched(color, color2);
        }

        public override void SetVisibleSynched(bool value, bool forceChildrenVisible = false)
        {
            base.SetVisibleSynched(value, forceChildrenVisible);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override void WriteToNetwork()
        {
            base.WriteToNetwork();
        }

        protected override void AttachDynamicNavmeshToEntity()
        {
            base.AttachDynamicNavmeshToEntity();
        }

        protected override GameEntity GetEntityToAttachNavMeshFaces()
        {
            return base.GetEntityToAttachNavMeshFaces();
        }

        protected override bool IsOnlyVisual()
        {
            return base.IsOnlyVisual();
        }

        protected override void OnEditModeVisibilityChanged(bool currentVisibility)
        {
            base.OnEditModeVisibilityChanged(currentVisibility);
        }

        protected override void OnEditorInit()
        {
            base.OnEditorInit();
        }

        protected override void OnEditorTick(float dt)
        {
            base.OnEditorTick(dt);
        }

        protected override void OnEditorValidate()
        {
            base.OnEditorValidate();
        }

        protected override void OnEditorVariableChanged(string variableName)
        {
            base.OnEditorVariableChanged(variableName);
        }

        protected override bool OnHit(Agent attackerAgent, int damage, Vec3 impactPosition, Vec3 impactDirection, int weaponKind, int curUsageIndex, out bool reportDamage)
        {
            return base.OnHit(attackerAgent, damage, impactPosition, impactDirection, weaponKind, curUsageIndex, out reportDamage);
        }

        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override void OnMissionReset()
        {
            base.OnMissionReset();
        }

        protected override void OnPhysicsCollision(ref PhysicsContact contact)
        {
            base.OnPhysicsCollision(ref contact);
        }

        protected override void OnPreInit()
        {
            base.OnPreInit();
        }

        protected override void OnRemoved()
        {
            base.OnRemoved();
        }

        protected override void OnSceneSave(string saveFolder)
        {
            base.OnSceneSave(saveFolder);
        }

        protected override void OnTick(float dt)
        {
            /*   MatrixFrame frame2;
               GameEntity.GetFrame(out frame2);
               frame2.Elevate(0.05f * dt);
               GameEntity.SetFrame(ref frame2);
               base.OnTick(dt);*/
        }

        protected override void SetAbilityOfFaces(bool enabled)
        {
            base.SetAbilityOfFaces(enabled);
        }

        protected override void SetScene(Scene scene)
        {
            base.SetScene(scene);
        }
        static Dictionary<string, List<Mesh>> MeshListByFileName = new Dictionary<string, List<Mesh>>();
        static public BlockMissionObject DrawFBXMeshMissionObjectAt(Mission mission, Vec3 pos, string filename)
        {
            // if (prefab == null)

            GameEntity prefab = GameEntity.CreateEmpty(mission.Scene, false);

            
            prefab.EntityFlags |= EntityFlags.DontSaveToScene;

            //prefab.AddMesh(Mesh.GetFromResource("order_arrow_a"), true);

            //   prefab.SetVisibilityExcludeParents(true);

            if (MeshListByFileName.ContainsKey(filename) == false)
            {

                MeshListByFileName.Add(filename, CreateMesh(filename));
            }
            foreach(var mesh in MeshListByFileName[filename])
            {
            prefab.AddMesh(mesh,false);

            }
            prefab.RecomputeBoundingBox();
            PhysicsShape shape = prefab.GetBodyShape();
            // shape.
            prefab.AddPhysics(1000, new Vec3(0, 0, 0), null, new Vec3(), new Vec3(), PhysicsMaterial.GetFromIndex(0), false, 0);

            GameEntity blockEntity = prefab;


            MatrixFrame identity2 = MatrixFrame.Identity;
            identity2.origin = pos;
            identity2.origin.x += 7.5f;

            identity2.origin.z = 7.5f;
            identity2.rotation.ApplyScaleLocal(1f);
            MatrixFrame frame2 = identity2;
            blockEntity.SetFrame(ref frame2);
            blockEntity.SetVisibilityExcludeParents(true);
            blockEntity.SetAlpha(1f);
            blockEntity.CreateAndAddScriptComponent("BlockMissionObject");

            BlockMissionObject missionObject = blockEntity.GetFirstScriptOfType<BlockMissionObject>();
            mission.AddActiveMissionObject(missionObject);
            return missionObject;
        }
        static List<(FbxWrapper.Mesh, FbxWrapper.Material[])> meshesFBX = new List<(FbxWrapper.Mesh, FbxWrapper.Material[])>();
        private static void WalkFBXTree(FbxWrapper.Node node)
        {
            if (node.Attribute != null && node.Attribute.Type == FbxWrapper.AttributeType.Mesh)
            {
                meshesFBX.Add((node.Mesh, node.GetMaterials()));
            }
            int childs = node.GetChildCount();
            for (int i = 0; i < childs; i++)
            {
                WalkFBXTree(node.GetChild(i));
            }
        }

        private static Vec3 fromFBX(FbxWrapper.Vector3 vec3)
        {
            return new Vec3((float)vec3.X, (float)vec3.Y, (float)vec3.Z);
        }
        private static Vec2 fromFBX(FbxWrapper.Vector2 vec2)
        {
            return new Vec2((float)vec2.X, (float)vec2.Y);
        }
        private static List<Mesh> CreateMesh(string filename = "test.fbx")
        {
            List<Mesh> meshes = new List<Mesh>();
            FbxWrapper.Manager wrapper = new FbxWrapper.Manager();
            var scene = FbxWrapper.Scene.Import(filename, -1);
            FbxWrapper.Node root = scene.RootNode;
            WalkFBXTree(root);
            foreach ((FbxWrapper.Mesh mesh, FbxWrapper.Material[] Materialist) in meshesFBX)
            {
                Mesh Mesh = Mesh.CreateMesh();
                UIntPtr ptr = Mesh.LockEditDataWrite();
                int numVertices = mesh.ControlPointsCount;
                var textcoords = mesh.GetTextCoords();
                if (numVertices > 0)
                {
                    int vertexIndex = 0;
                    var polygons = mesh.GetPolygons();

                    for (int polyidx = 0; polyidx < polygons.Length; polyidx++)
                    {
                        int polySize = polygons[polyidx].Indices.Length;
                        switch (polySize)
                        {
                            case 3:
                                Mesh.AddTriangle(fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[0])),
                                        fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[1])),
                                        fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[2])),
                                        fromFBX(textcoords[vertexIndex]),
                                        fromFBX(textcoords[vertexIndex + 1]),
                                        fromFBX(textcoords[vertexIndex + 2]),
                                        536918784U,
                                        ptr);
                                vertexIndex += 3;
                                break;
                            case 4:
                                Mesh.AddTriangle(fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[0])),
                                        fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[1])),
                                        fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[2])),
                                        fromFBX(textcoords[vertexIndex]),
                                        fromFBX(textcoords[vertexIndex + 1]),
                                        fromFBX(textcoords[vertexIndex + 2]),
                                        536918784U,
                                        ptr);
                                Mesh.AddTriangle(fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[0])),
                                    fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[2])),
                                    fromFBX(mesh.GetControlPointAt(polygons[polyidx].Indices[3])),
                                     fromFBX(textcoords[vertexIndex]),
                                        fromFBX(textcoords[vertexIndex + 2]),
                                        fromFBX(textcoords[vertexIndex + 3]),
                                    536918784U,
                                    ptr);
                                vertexIndex += 4;
                                break;

                        }

                    }

                }
                foreach (FbxWrapper.Material material in Materialist)
                {

                    Material mat = Material.GetDefaultMaterial().CreateCopy();
                    var diffuseList = material.TexturePaths(FbxWrapper.LayerElementType.TextureDiffuse);
                    var diffuse = diffuseList.Length > 0 ? diffuseList[0] : null;

                    var bumpList = material.TexturePaths(FbxWrapper.LayerElementType.TextureBump);
                    var bump = bumpList.Length > 0 ? bumpList[0] : null;

                    var specularList = material.TexturePaths(FbxWrapper.LayerElementType.TextureSpecular);
                    var specular = specularList.Length > 0 ? specularList[0] : null;
                        if (!diffuse.IsStringNoneOrEmpty())
                    {

                        mat.SetTexture(Material.MBTextureType.DiffuseMap, Texture.CreateTextureFromPath(System.IO.Path.GetDirectoryName(diffuse), System.IO.Path.GetFileName(diffuse)));
                    }
                    if (!bump.IsStringNoneOrEmpty())
                    {
                        mat.SetTexture(Material.MBTextureType.BumpMap, Texture.CreateTextureFromPath(System.IO.Path.GetDirectoryName(bump), System.IO.Path.GetFileName(bump)));
                    }
                    if (!specular.IsStringNoneOrEmpty())
                    {
                        mat.SetTexture(Material.MBTextureType.SpecularMap, Texture.CreateTextureFromPath(System.IO.Path.GetDirectoryName(specular), System.IO.Path.GetFileName(specular)));
                    }


                    Mesh.SetMaterial(mat);

                }

                /* Mesh.VisibilityMask = (VisibilityMaskFlags.Final | VisibilityMaskFlags.DefaultStatic);
                  Mesh.SetColorAlpha(150U);
                  Mesh.SetMeshRenderOrder(250);
                  Mesh.CullingMode = MBMeshCullingMode.None;
                  float vectorArgument = 25f;

                  Mesh.SetVectorArgument(vectorArgument, 0f, 0f, 0f);
                 ;*/
                Mesh.ComputeNormals();
                Mesh.ComputeTangents();
                Mesh.RecomputeBoundingBox();
                Mesh.UnlockEditDataWrite(ptr);
                meshes.Add(Mesh);
            }
            /*
            TL.Vec3 upperLeftFront = new TL.Vec3(-1, -1, -1);
            TL.Vec3 upperRightFront = new TL.Vec3(1, -1, -1);
            TL.Vec3 lowerLeftFront = new TL.Vec3(-1, 1, -1);
            TL.Vec3 lowerRightFront = new TL.Vec3(1, 1, -1);

            TL.Vec3 upperLeftBack = new TL.Vec3(-1, -1, 1);
            TL.Vec3 upperRightBack = new TL.Vec3(1, -1, 1);
            TL.Vec3 lowerLeftBack = new TL.Vec3(-1, 1, 1);
            TL.Vec3 lowerRightBack = new TL.Vec3(1, 1, 1);
            TL.Vec2 uv = new TL.Vec2(0, 0);
            // Front



            Mesh.AddTriangle(upperLeftFront, lowerLeftFront, upperRightFront, Vec2.Zero, Vec2.Side, Vec2.Forward, 536918784U, ptr);
            Mesh.AddTriangle(upperRightFront, lowerLeftFront, lowerRightFront, Vec2.Forward, Vec2.Side, Vec2.One, 536918784U, ptr);

            // Back
            Mesh.AddTriangle(upperLeftBack, upperRightBack, lowerLeftBack, Vec2.Zero, Vec2.Side, Vec2.Forward, 536918784U, ptr);
            Mesh.AddTriangle(upperRightBack, lowerRightBack, lowerLeftBack, Vec2.Forward, Vec2.Side, Vec2.One, 536918784U, ptr);

            // Upper
            Mesh.AddTriangle(upperLeftBack, upperLeftFront, upperRightBack, Vec2.Zero, Vec2.Side, Vec2.Forward, 536918784U, ptr);
            Mesh.AddTriangle(upperRightBack, upperLeftFront, upperRightFront, Vec2.Forward, Vec2.Side, Vec2.One, 536918784U, ptr);

            // Lower
            Mesh.AddTriangle(lowerLeftBack, lowerRightBack, lowerLeftFront, Vec2.Zero, Vec2.Side, Vec2.Forward, 536918784U, ptr);
            Mesh.AddTriangle(lowerRightBack, lowerRightFront, lowerLeftFront, Vec2.Forward, Vec2.Side, Vec2.One, 536918784U, ptr);

            // Left
            Mesh.AddTriangle(upperLeftFront, upperLeftBack, lowerLeftFront, Vec2.Zero, Vec2.Side, Vec2.Forward, 536918784U, ptr);
            Mesh.AddTriangle(upperLeftBack, lowerLeftBack, lowerLeftFront, Vec2.Forward, Vec2.Side, Vec2.One, 536918784U, ptr);

            // Right
            Mesh.AddTriangle(upperRightFront, lowerRightFront, upperRightBack, uv, uv, uv, 536918784U, ptr);
            Mesh.AddTriangle(upperRightBack, lowerRightFront, lowerRightBack, uv, uv, uv, 536918784U, ptr);*/

           

            meshesFBX.Clear();
            return meshes;
        }
    }
    public class CaptureTheBannerLordMissionController : MissionLogic
    {
        private Game _game;
        public CaptureTheBannerLordParams CaptureTheBannerLordParams;
        private bool _started;
        public TL.Vec3 freeCameraPosition;
        public Agent _playerAgent;
        private Team playerTeam;
        private Team enemyTeam;

        public CaptureTheBannerLordMissionController(CaptureTheBannerLordParams p)
        {
            this._game = Game.Current;
            this.CaptureTheBannerLordParams = p;
        }

        public override void AfterStart()
        {
            try
            {
                this.AfterStart2();
                this.SpawnBlocks();
            }
            catch (System.Exception e)
            {
                ModuleLogger.Log("{0}", e);
            }
        }
        public static TL.Vec3 multiply(TL.Vec3 a, TL.Vec3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;
            a.w *= b.w;
            return a;
        }

        private void SpawnBlocks()
        {
            /*  MBDebug.RenderDebugText3D(Mission.PlayerTeam.Leader.GetWorldPosition().Position, "TEST WORLD TEXT", TeammateColorsExtensions.NEUTRAL_COLOR, time: 20000);
              MBDebug.TestModeEnabled = true;
              var pos = Mission.PlayerTeam.Leader.GetWorldPosition().Position;
              pos = multiply(pos, (TL.Vec3.Forward * 100) + (TL.Vec3.Side * 100) + (TL.Vec3.Up * 100));
              MBDebug.RenderDebugBoxObject(Mission.PlayerTeam.Leader.GetWorldPosition().Position, pos, TeammateColorsExtensions.NEUTRAL_COLOR);
              */


            //Mission.PlayerTeam.Leader.AgentVisuals.GetEntity().AddChild(block, false);
            //        block.SetLocalPosition(Mission.PlayerTeam.Leader.GetWorldPosition().Position);
            //         block.SetGlobalFrame(Mission.PlayerTeam.Leader.AgentVisuals.GetGlobalFrame());
            //  block.SetGlobalFrame(MatrixFrame.Identity);
        }
        private List<Agent> playerTeamAgents;
        public override void OnAgentDeleted(Agent affectedAgent)
        {
            base.OnAgentDeleted(affectedAgent);
            if (playerTeamAgents.Contains(affectedAgent))
            {
                playerTeamAgents.Remove(affectedAgent);
            }
        }
        public void RespawnUnits()
        {

            var scene = this.Mission.Scene;
            var xInterval = this.CaptureTheBannerLordParams.soldierXInterval;
            var yInterval = this.CaptureTheBannerLordParams.soldierYInterval;
            var soldiersPerRow = this.CaptureTheBannerLordParams.soldiersPerRow;

            var startPos = this.CaptureTheBannerLordParams.FormationPosition;
            var xDir = this.CaptureTheBannerLordParams.formationDirection;
            var yDir = this.CaptureTheBannerLordParams.formationDirection.LeftVec();
            var agentDefaultDir = new TL.Vec2(0, 1);
            var useFreeCamera = this.CaptureTheBannerLordParams.useFreeCamera;

            BasicCharacterObject soldierCharacter = this._game.ObjectManager.GetObject<BasicCharacterObject>(this.CaptureTheBannerLordParams.playerSoldierCharacterId);
            var playerSoldierFormationClass = soldierCharacter.CurrentFormationClass;
            this.playerTeam = this.Mission.Teams.Add(BattleSideEnum.Attacker, 0xff3f51b5);
            var mapHasNavMesh = false;
            {
                var formation = playerTeam.GetFormation(playerSoldierFormationClass);
                var width = this.getInitialFormationWidth(playerTeam, playerSoldierFormationClass);
                var centerPos = startPos + yDir * (width / 2);
                var wp = new WorldPosition(scene, centerPos.ToVec3());
                formation.SetPositioning(wp, xDir, null);
                formation.FormOrder = FormOrder.FormOrderCustom(width);
                mapHasNavMesh = wp.GetNavMesh() != System.UIntPtr.Zero;
            }
            if (this.playerTeamAgents.Count < this.CaptureTheBannerLordParams.playerSoldierCount)
            {
                var formation = playerTeam.GetFormation(playerSoldierFormationClass);

                AgentBuildData soldierBuildData = new AgentBuildData(new BasicBattleAgentOrigin(soldierCharacter))
                    .ClothingColor1(playerTeam.Color)
                    .ClothingColor2(playerTeam.Color2)
                    .Banner(playerTeam.Banner)
                    .IsFemale(false)
                    .Team(playerTeam)
                    .Formation(formation);

                if (!mapHasNavMesh)
                {
                    var x = this.playerTeamAgents.Count / soldiersPerRow;
                    var y = this.playerTeamAgents.Count % soldiersPerRow;
                    var mat = TL.Mat3.Identity;
                    var pos = startPos + xDir * (-xInterval * x) + yDir * yInterval * y;
                    mat.RotateAboutUp(agentDefaultDir.AngleBetween(xDir));
                    var agentFrame = new TaleWorlds.Library.MatrixFrame(mat, new TL.Vec3(pos.x, pos.y, 30));
                    soldierBuildData.InitialFrame(agentFrame);
                }

                var agent = this.Mission.SpawnAgent(soldierBuildData);
                agent.SetWatchState(AgentAIStateFlagComponent.WatchState.Alarmed);
                this.playerTeamAgents.Add(agent);
            }


        }
        public void AfterStart2()
        {
            this._started = true;
            playerTeamAgents = new List<Agent>();
            var scene = this.Mission.Scene;

            if (this.CaptureTheBannerLordParams.skyBrightness >= 0)
            {
                scene.SetSkyBrightness(this.CaptureTheBannerLordParams.skyBrightness);
            }

            if (this.CaptureTheBannerLordParams.rainDensity >= 0)
            {
                scene.SetRainDensity(this.CaptureTheBannerLordParams.rainDensity);
            }

            this.Mission.MissionTeamAIType = Mission.MissionTeamAITypeEnum.FieldBattle;
            this.Mission.SetMissionMode(MissionMode.Battle, true);

            var xInterval = this.CaptureTheBannerLordParams.soldierXInterval;
            var yInterval = this.CaptureTheBannerLordParams.soldierYInterval;
            var soldiersPerRow = this.CaptureTheBannerLordParams.soldiersPerRow;

            var startPos = this.CaptureTheBannerLordParams.FormationPosition;
            var xDir = this.CaptureTheBannerLordParams.formationDirection;
            var yDir = this.CaptureTheBannerLordParams.formationDirection.LeftVec();
            var agentDefaultDir = new TL.Vec2(0, 1);
            var useFreeCamera = this.CaptureTheBannerLordParams.useFreeCamera;

            BasicCharacterObject soldierCharacter = this._game.ObjectManager.GetObject<BasicCharacterObject>(this.CaptureTheBannerLordParams.playerSoldierCharacterId);
            var playerSoldierFormationClass = soldierCharacter.CurrentFormationClass;
            this.playerTeam = this.Mission.Teams.Add(BattleSideEnum.Attacker, 0xff3f51b5);

            playerTeam.AddTeamAI(new TeamAIGeneral(this.Mission, playerTeam));
            playerTeam.AddTacticOption(new TacticCharge(playerTeam));
            // playerTeam.AddTacticOption(new TacticFullScaleAttack(playerTeam));
            playerTeam.ExpireAIQuerySystem();
            playerTeam.ResetTactic();
            this.Mission.PlayerTeam = playerTeam;

            var playerPosVec2 = startPos + xDir * -10 + yDir * -10;
            var playerPos = new TL.Vec3(playerPosVec2.x, playerPosVec2.y, 30);
            if (!useFreeCamera)
            {
                var playerMat = TL.Mat3.Identity;
                playerMat.RotateAboutUp(agentDefaultDir.AngleBetween(xDir));
                BasicCharacterObject playerCharacter = this._game.ObjectManager.GetObject<BasicCharacterObject>(this.CaptureTheBannerLordParams.playerCharacterId);
                AgentBuildData agentBuildData = new AgentBuildData(new BasicBattleAgentOrigin(playerCharacter))
                    .ClothingColor1(0xff3f51b5)
                    .ClothingColor2(0xff3f51b5)
                    .Banner(Banner.CreateRandomBanner())
                    .IsFemale(false)
                    .InitialFrame(new TL.MatrixFrame(playerMat, playerPos));
                Agent player = this.Mission.SpawnAgent(agentBuildData, false, 0);
                player.Controller = Agent.ControllerType.Player;
                player.WieldInitialWeapons();
                player.AllowFirstPersonWideRotation();

                Mission.MainAgent = player;
                player.SetTeam(playerTeam, true);
                playerTeam.GetFormation(playerSoldierFormationClass).PlayerOwner = player;
                playerTeam.PlayerOrderController.Owner = player;
                this._playerAgent = player;
            }
            else
            {
                var c = this.CaptureTheBannerLordParams.playerSoldierCount;
                if (c <= 0)
                {
                    this.freeCameraPosition = new TL.Vec3(startPos.x, startPos.y, 30);
                }
                else
                {
                    var rowCount = (c + soldiersPerRow - 1) / soldiersPerRow;
                    var p = startPos + (System.Math.Min(soldiersPerRow, c) - 1) / 2 * yInterval * yDir - rowCount * xInterval * xDir;
                    this.freeCameraPosition = new TL.Vec3(p.x, p.y, 5);
                }
            }


            BasicCharacterObject enemyCharacter = this._game.ObjectManager.GetObject<BasicCharacterObject>(this.CaptureTheBannerLordParams.enemySoldierCharacterId);
            enemyTeam = this.Mission.Teams.Add(BattleSideEnum.Defender, 0xffff6090);
            enemyTeam.AddTeamAI(new TeamAIGeneral(this.Mission, enemyTeam));
            enemyTeam.AddTacticOption(new TacticCharge(enemyTeam));
            // enemyTeam.AddTacticOption(new TacticFullScaleAttack(enemyTeam));
            enemyTeam.SetIsEnemyOf(playerTeam, true);
            playerTeam.SetIsEnemyOf(enemyTeam, true);
            enemyTeam.ExpireAIQuerySystem();
            enemyTeam.ResetTactic();

            var enemyFormationClass = enemyCharacter.CurrentFormationClass;
            var enemyFormation = enemyTeam.GetFormation(FormationClass.Ranged);
            {
                float width = this.getInitialFormationWidth(enemyTeam, enemyFormationClass);
                var centerPos = startPos + yDir * (width / 2) + xDir * this.CaptureTheBannerLordParams.distance;
                var wp = new WorldPosition(scene, centerPos.ToVec3());
                enemyFormation.SetPositioning(wp, -xDir, null);
                enemyFormation.FormOrder = FormOrder.FormOrderCustom(width);
            }

            for (var i = 0; i < this.CaptureTheBannerLordParams.enemySoldierCount; i += 1)
            {
                AgentBuildData enemyBuildData = new AgentBuildData(new BasicBattleAgentOrigin(enemyCharacter))
                    .ClothingColor1(enemyTeam.Color)
                    .ClothingColor2(enemyTeam.Color2)
                    .Banner(enemyTeam.Banner)
                    .Formation(enemyFormation);


                var agent = this.Mission.SpawnAgent(enemyBuildData);
                agent.SetTeam(enemyTeam, true);
                agent.Formation = enemyFormation;
                agent.SetWatchState(AgentAIStateFlagComponent.WatchState.Alarmed);
            }
            {
                var a = this.Mission.IsOrderShoutingAllowed();
                var b = this.Mission.IsAgentInteractionAllowed();
                var c = GameNetwork.IsClientOrReplay;
                var d = playerTeam.PlayerOrderController.Owner == null;
                ModuleLogger.Log("mission allowed shouting: {0} interaction: {1} {2} {3}", a, b, c, d);
            }
        }

        float getInitialFormationWidth(Team team, FormationClass fc)
        {
            var bp = this.CaptureTheBannerLordParams;
            var formation = team.GetFormation(fc);
            var mounted = fc == FormationClass.Cavalry || fc == FormationClass.HorseArcher;
            var unitDiameter = Formation.GetDefaultUnitDiameter(mounted);
            var unitSpacing = 1;
            var interval = mounted ? Formation.CavalryInterval(unitSpacing) : Formation.InfantryInterval(unitSpacing);
            var actualSoldiersPerRow = System.Math.Min(bp.soldiersPerRow, bp.playerSoldierCount);
            var width = (actualSoldiersPerRow - 1) * (unitDiameter + interval) + unitDiameter + 0.1f;
            return width;
        }

        public override bool MissionEnded(ref MissionResult missionResult)
        {
            return this.Mission.InputManager.IsKeyPressed(TaleWorlds.InputSystem.InputKey.Tab);
        }

        public override void OnMissionTick(float dt)
        {

            if (this._started)
            {
                var scene = this.Mission.Scene;
                /* if (this.Mission.InputManager.IsKeyPressed(TaleWorlds.InputSystem.InputKey.C))
                 {
                     this.SwitchCamera();
                 }
                 if (this.Mission.InputManager.IsKeyPressed(TaleWorlds.InputSystem.InputKey.F1))
                 {
                     for (int i = 0; i < this.CaptureTheBannerLordParams.playerSoldierCount; i++)
                     {
                         RespawnUnits();
                     }
                 }
                 if (this.Mission.InputManager.IsKeyPressed(TaleWorlds.InputSystem.InputKey.M))
                 {
                     for (int i = 0; i < 100; i++)
                     {
                         RespawnUnits();
                     }
                 }*/
                Vec3 playerpos = this._playerAgent.GetWorldPosition().Position;
                if (this.Mission.InputManager.IsKeyPressed(TaleWorlds.InputSystem.InputKey.B))
                {

                    var cube = BlockMissionObject.DrawFBXMeshMissionObjectAt(this.Mission, new Vec3(playerpos.x, playerpos.y, playerpos.z), "RubbingMyAxe.fbx");

                }
                if (this.Mission.InputManager.IsKeyPressed(TaleWorlds.InputSystem.InputKey.T))
                {

                    var cube = BlockMissionObject.DrawFBXMeshMissionObjectAt(this.Mission, new Vec3(playerpos.x, playerpos.y, playerpos.z), "Moon 2K.fbx");

                }
            }
            if (this.Mission.InputManager.IsKeyPressed(TaleWorlds.InputSystem.InputKey.X))
            {
                this.CaptureTheBannerLordParams.playerSoldierCount += 10;

            }
        }

        void SwitchCamera()
        {
            ModuleLogger.Log("SwitchCamera");
            if (this._playerAgent == null || !this._playerAgent.IsActive())
            {
                this.displayMessage("no player agent");
                return;
            }
            if (this.Mission.MainAgent == null)
            {
                this._playerAgent.Controller = Agent.ControllerType.Player;
                this.displayMessage("switch to player agent");
            }
            else
            {
                this.Mission.MainAgent = null;
                this._playerAgent.Controller = Agent.ControllerType.AI;
                var wp = this._playerAgent.GetWorldPosition();
                this._playerAgent.SetScriptedPosition(ref wp, Agent.AIScriptedFrameFlags.DoNotRun, "camera switch");
                this.displayMessage("switch to free camera");
            }
        }

        void displayMessage(string msg)
        {
            InformationManager.DisplayMessage(new InformationMessage(new TaleWorlds.Localization.TextObject(msg, null).ToString()));
        }
    }
}