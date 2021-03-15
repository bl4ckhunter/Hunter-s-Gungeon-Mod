
using ClassLibrary1.Scripts;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using Items;
using MonoMod.RuntimeDetour;
using MultiplayerBasicExample;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using tk2dRuntime.TileMap;
using Pathfinding;
using GungeonAPI;
using ClassLibrary1;
using SaveAPI;
using System.Security.Cryptography.X509Certificates;

namespace Mod
{

    public class RoRItems : ETGModule
    {
        public static readonly string MOD_NAME = "Realms Of Ruin";
        public static readonly string VERSION = "0.7.0";
        public static readonly string TEXT_COLOR = "#00FFFF";
        public override void Start()
        {
            RoRItems.ZipFilePath = this.Metadata.Archive;
            RoRItems.FilePath = this.Metadata.Directory + "/rooms";
            ItemAPI.FakePrefabHooks.Init();
            SaveAPIManager.Setup("ror");
            ItemBuilder.Init(); //This needs to be called to be able to use embedded resources
            Berserk.Init();
            Bandolier.Init();
            Ukulele.Init();
            Will.Init();
            Drug.Init();
            OWStealthkit.Init();
            Headstomper.Init();
            HSeed.Init();
            Atg.Init();
            Dml.Init();
            Gloop.Init();
            Whip.Init();
            FrailCrown.Init();
            Key.Init();
            NRG.Init();
            TimeAmmolet.Init();
            Shroom.Init();
            Magazine.Init();
            Crowbar.Init();
            CF.Init();
            Afterburner.Init();
            Glass.Init();
            Obelisk.Init();
            Horn.Init();
            Relic.Init();
            Nchallange.Init();
            Nspoils.Init();
            Ntouch.Init();
            Radar.Init();
            Laser.Init();
            Beads.Init();
            Breath.Init();
            Soul.Init();
            Broken.Init();
            Medkit.Init();
            Atom.Init();
            CBlast.Init();
            Crowdfunder.Init();
            Guillotine.Init();
            Warbanner.Init();
            Heresy.Init();
            Hammer.Init();
            Coil.Init();
            Bird.Init();
            Gem.Init();
            SoulGem.Init();
            Bait.Init();
            Fish.Init();
            Meat.Init();
            Daisy.Init();
            Flower.Init();
            Dagger.Init();
            Chaos.Init();
            Abyss.Init();
            Tentacle.Init();
            Satellite.Init();
            Abbeyscroll.Init();
            Minescroll.Init();
            RatScroll.Init();
            HollowScroll.Init();
            ForgeScroll.Init();
            OfficeScroll.Init();
            HellScroll.Init();
            NeonBlade.Init();
            Blad.Init();
            Satel.Init();
            LoaderFram.Init();
            Kata.Init();
            Katar.Init();
            Commando.Init();
            Flamer.Init();
            Acrid.Init();
            Hunt.Init();
            Nuklearkin.Init();
            Tank1.Init();
            SmallBoiNuke.Add();
            HeavyFlamer.Add();
            Icer.Add();
            Bhole.Add();
            Thorn.Add();
            Stargun.Add();
            Spark.Add();
            Barrel.Add();
            Megadrill.Add();
            Revolver1.Add();
            MissileMassacre.Add();
            Bomblauncher.Add();
            Wand.Add();
            Heartlance.Add();
            TediorePistol.Add();
            TedioreRocket.Add();
            TedioreSMG.Add();
            TedioreShotgun.Add();
            Mshells.Init();
            MoxyMwanSMG.Add();
            MwanRadSniper.Add();
            SlagSMG.Add();
            Slagshells.Init();
            Sshells.Init();
            Rshells.Init();
            ElectricFireAR.Add();
            EridianRandomizer.Init();
            Sickle.Add();
            Revolution.Add();
            VladofMortar.Add();
            Chomp.Add();
            CritCola.Init();
            Gesture.Init();
            Order.Init();
            Printer.Init();
            Boulder.Init();
            GeoStaff.Add();
            Spitfire.Add();
            Basher.Add();
            Masher.Add();
            Pcube.Init();
            VoidBul.Init();
            SwordSplosion.Add();
            RocketBlaster.Add();
            StickyRocket.Init();
            ProtonWhip.Add();
            LaserDisco.Add();
            LaserSploder.Add();
            Sawmill.Add();
            LightDancer.Add();
            Prismone.Add();
            SteelCutter.Add();
            PortalGun.Init();
            Punch.Add();
            MicrowaveCannon.Add();
            ShrinkRay.Add();
            KillTheFuture.Add();
            SmokingGun.Add();
            ToxicGun.Add();
            WallPierce.Init();
            Loudspeaker.Add();
            Katanagun.Add();
            Butchers.Add();
            //cards
            FoolCard.Init();
            Magiciancard.Init();
            Priestess.Init();
            EmpressCard.Init();
            EmperorCard.Init();
            HierophantCard.Init();
            LoversCard.Init();
            ChariotCard.Init();
            JusticeCard.Init();
            HeremitCard.Init();
            WheelCard.Init();
            StrenghtCard.Init();
            HangCard.Init();
            DeathCard.Init();
            TemperanceCard.Init();
            DevilCard.Init();
            TowerCard.Init();
            StarsCard.Init();
            MoonCard.Init();
            SunCard.Init();
            JudgementCard.Init();
            WorldCard.Init();
            //unlockablecards
            MageCard.Init();
            //randomitems 
            ReturnCard.Init();
            Bcard.Init();
            ObsidianShard.Init();
            TarotDeck.Init();
            Rose.Add();
            IonRifle.Add();
            BanditRevolver.Add();
            StormLantern.Add();
            QTEgun.Add();
            ShadowEdge.Add();
            GravitonLauncher.Add();
            Megarailgun.Add();
            Flamechainsawgun.Add();
            //shrines
            WitchShop.Add();
            ModManShrine.Add();
            EntryWayShrine.Add();
            DeadkingShrine.Add();
            ExitWayShrine.Add();
            ChronoBattery.Init();
            OrbShrine.Add();
            Galactilichschyte.Add();
            Galactilich1.Init();
            Galactilichphase2.Init();
            Galactilichphase3.Init();
            MobiusShop.Add();
            MobiusDonationCoffer.Add();
            SamsonChain.Init();
            //unlockableitems
            GoldGoopBottle.Init();
            GoldenShotgun.Add();
            GoldBullets.Init();
            GoldenHeart.Init();
            TheWork.Init();
            //
            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
            ETGModConsole.Commands.AddGroup("ban_items", new Action<string[]>(this.Weight));
            this.Synergies();
            RoRItems.TrueCards();
            DungeonHooks.OnPostDungeonGeneration += Cards;
            DungeonHooks.OnPostDungeonGeneration += CardWitch;
            DungeonHooks.OnPostDungeonGeneration += SecretArenaTrigger;
            DungeonHooks.OnPostDungeonGeneration += MobiusCoffer;
            CreatedRooms = new List<RoomHandler>();
            CreatedStickyprojs = new List<GameObject>();
            stickiesAlive = new List<Projectile>();


        }
        Hook hookactivereloadattempt = new Hook(typeof(Gun).GetMethod("FinishReload", BindingFlags.Instance | BindingFlags.NonPublic), typeof(RoRItems).GetMethod("FinishReloadHook"));
        public static void FinishReloadHook(Action<Gun, bool, bool, bool> orig, Gun self, bool activeReload, bool silent, bool isImmediate)
        {
            orig(self, activeReload, silent, isImmediate);
            activereloadsuccess = activeReload;
        }
        Hook GoopEffect = new Hook(typeof(DeadlyDeadlyGoopManager).GetMethod("DoTimelessGoopEffect", BindingFlags.Instance | BindingFlags.Public), typeof(RoRItems).GetMethod("ApplyGoopEffect"));
        public static void ApplyGoopEffect(Action<DeadlyDeadlyGoopManager, GameActor, IntVector2> orig, DeadlyDeadlyGoopManager self, GameActor actor, IntVector2 goopPosition)
        {
            orig(self, actor, goopPosition);

                Action<DeadlyDeadlyGoopManager, GameActor, IntVector2> touchgoop = RoRItems.OnGoopTouched;
                if (touchgoop != null && self != null && actor != null && goopPosition != null)
                {
                    touchgoop(self, actor, goopPosition);
                }
            return;
        }
        private void SecretArenaTrigger()
        {
            try
            {
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
                {
                    SecretArena();
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON && !GameManager.Instance.IsFoyer)
                {
                    if (UnityEngine.Random.value < 0.076f)
                    {
                        bool generated = false;
                        List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
                        BaseShopController[] componentsInChildren = GameManager.Instance.Dungeon.data.Entrance.hierarchyParent.parent.GetComponentsInChildren<BaseShopController>(true);
                        if (componentsInChildren != null && componentsInChildren.Length > 0)
                        {
                            RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(componentsInChildren[0].transform.position.IntXY(VectorConversions.Round));

                            if (roomHandler2.IsShop && !generated)
                            {
                                IntVector2 intVector = roomHandler2.Epicenter + new IntVector2 (-5,0);
                                if (intVector != null && intVector != IntVector2.Zero)
                                {
                                    ShrineFactory.builtShrines.TryGetValue("ror:mod_man", out GameObject gabject);
                                    GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, new Vector3(intVector.x, intVector.y), Quaternion.identity);
                                    IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
                                    IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
                                    RoomHandler absoluteRoom = roomHandler2;
                                    for (int i = 0; i < interfaces.Length; i++)
                                    {
                                        absoluteRoom.RegisterInteractable(interfaces[i]);
                                    }
                                    for (int j = 0; j < interfaces2.Length; j++)
                                    {
                                        interfaces2[j].ConfigureOnPlacement(absoluteRoom);
                                    }
                                    generated = true;
                                }

                            }
                        }
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON)
                {
                    bool generated = false;
                    List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
                    for (int ig = 0; ig < GameManager.Instance.Dungeon.data.rooms.Count; ig++)
                    {
                        RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[list[ig]];
                        if (roomHandler2.IsStandardRoom && !generated)
                        {

                            IntVector2 intVector = roomHandler2.GetRandomVisibleClearSpot(6, 6);
                            if (intVector != null && intVector != IntVector2.Zero)
                            {
                                ShrineFactory.builtShrines.TryGetValue("ror:orb_shrine", out GameObject gabject);
                                GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, new Vector3(intVector.x, intVector.y), Quaternion.identity);
                                IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
                                IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
                                RoomHandler absoluteRoom = roomHandler2;
                                for (int i = 0; i < interfaces.Length; i++)
                                {
                                    absoluteRoom.RegisterInteractable(interfaces[i]);
                                }
                                for (int j = 0; j < interfaces2.Length; j++)
                                {
                                    interfaces2[j].ConfigureOnPlacement(absoluteRoom);
                                }
                                generated = true;
                            }

                        }

                    }

                }
            }
            catch { }
        }   

        private void SecretArena()
        {
            if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
            {
                bool generated1 = false;
                List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
                for (int ig = 0; ig < GameManager.Instance.Dungeon.data.rooms.Count; ig++)
                {
                    RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[list[ig]];
                    if (roomHandler2.IsStandardRoom && !generated1)
                    {
                        IntVector2 intVector = roomHandler2.GetRandomVisibleClearSpot(6, 6);
                        if (intVector != null && intVector != IntVector2.Zero)
                        {
                            ShrineFactory.builtShrines.TryGetValue("ror:entryway", out GameObject gabject);
                            GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, new Vector3(intVector.x, intVector.y), Quaternion.identity);
                            IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
                            IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
                            RoomHandler absoluteRoom = roomHandler2;
                            for (int i = 0; i < interfaces.Length; i++)
                            {
                                absoluteRoom.RegisterInteractable(interfaces[i]);
                            }
                            for (int j = 0; j < interfaces2.Length; j++)
                            {
                                interfaces2[j].ConfigureOnPlacement(absoluteRoom);
                            }
                            generated1 = true;
                        }

                    }

                }

            }
            else 
            { SecretArenaRoom = null; 
            
            }
            
        }

        private void CardWitch()
        {
            if (!GameManager.Instance.IsFoyer)
            {
                bool generated = false;
                List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
                BaseShopController[] componentsInChildren = GameManager.Instance.Dungeon.data.Entrance.hierarchyParent.parent.GetComponentsInChildren<BaseShopController>(true);
                if (componentsInChildren != null && componentsInChildren.Length > 0)
                {
                    RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(componentsInChildren[0].transform.position.IntXY(VectorConversions.Round));

                    if (roomHandler2.IsShop && !generated)
                    {
                        IntVector2 intVector = roomHandler2.Epicenter;
                        if (intVector != null && intVector != IntVector2.Zero)
                        {
                            ShrineFactory.builtShrines.TryGetValue("ror:witchshrine", out GameObject gabject);
                            GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, new Vector3(intVector.x, intVector.y), Quaternion.identity);
                            IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
                            IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
                            RoomHandler absoluteRoom = roomHandler2;
                            for (int i = 0; i < interfaces.Length; i++)
                            {
                                absoluteRoom.RegisterInteractable(interfaces[i]);
                            }
                            for (int j = 0; j < interfaces2.Length; j++)
                            {
                                interfaces2[j].ConfigureOnPlacement(absoluteRoom);
                            }
                            generated = true;
                        }

                    }
                }

            }
        }
        private void MobiusCoffer()
        {
            if (!GameManager.Instance.IsFoyer && SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.MOBIUS_CHEST_MONEY) > 0 && pulls < 5)
            {
                bool generated = false;
                List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
                BaseShopController[] componentsInChildren = GameManager.Instance.Dungeon.data.Entrance.hierarchyParent.parent.GetComponentsInChildren<BaseShopController>(true);
                if (componentsInChildren != null && componentsInChildren.Length > 0)
                {
                    RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(componentsInChildren[0].transform.position.IntXY(VectorConversions.Round));

                    if (roomHandler2.IsShop && !generated)
                    {
                        IntVector2 intVector = roomHandler2.Epicenter;
                        if (intVector != null && intVector != IntVector2.Zero)
                        {
                            ShrineFactory.builtShrines.TryGetValue("ror:mobiuscoffer", out GameObject gabject);
                            GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, new Vector3(intVector.x -12, intVector.y), Quaternion.identity);
                            IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
                            IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
                            RoomHandler absoluteRoom = roomHandler2;
                            for (int i = 0; i < interfaces.Length; i++)
                            {
                                absoluteRoom.RegisterInteractable(interfaces[i]);
                            }
                            for (int j = 0; j < interfaces2.Length; j++)
                            {
                                interfaces2[j].ConfigureOnPlacement(absoluteRoom);
                            }
                            generated = true;
                        }

                    }
                }

            }
        }

        public static List<RoomHandler> CreatedRooms;
        public static List<GameObject> CreatedStickyprojs;

        public static List<Projectile> stickiesAlive;

        private static void TrueCards()
        {
            foreach(PlayerItem item in RoRItems.cards)
            {
                item.CustomCost = 10;
                item.UsesCustomCost = true;

                WeightedGameObject weightedObject = new WeightedGameObject();
                weightedObject.SetGameObject(item.gameObject);
                weightedObject.weight = 0.25f;
                weightedObject.rawGameObject = item.gameObject;
                weightedObject.pickupId = item.PickupObjectId;
                weightedObject.forceDuplicatesPossible = true;
                weightedObject.additionalPrerequisites = new DungeonPrerequisite[0];

                foreach (FloorRewardData rewardData in GameManager.Instance.RewardManager.FloorRewardData)
                {
                    if (rewardData.SingleItemRewardTable.defaultItemDrops.elements != null && rewardData.SingleItemRewardTable.defaultItemDrops.elements.Count != 0 && !rewardData.SingleItemRewardTable.defaultItemDrops.elements.Contains(weightedObject)) 
                    {
                        rewardData.SingleItemRewardTable.defaultItemDrops.Add(weightedObject);   
                    }
                }

            }
        }

        

        private void Cards()

        {
            if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON)
            {
                MobiusWithdrawnThisRun = 0;
                RoRItems.Cardprice = 0;
                PlayerController player = GameManager.Instance.PrimaryPlayer;
                player.OnRoomClearEvent += Spawncard;
            }

        }
        private void Spawncard(PlayerController obj)
        {
            float f = obj.stats.GetStatValue(PlayerStats.StatType.Coolness)/60f;
            float chance = 0.035f + f;
            if (UnityEngine.Random.value < chance)
            {
                int c = cards.Count;
                int i = UnityEngine.Random.Range(0, c);
                LootEngine.SpawnItem(cards[i].gameObject, obj.CenterPosition, new Vector2(0, 0), 0f);
            }
        }
        //card effects
        public static IEnumerator AntiEmpress(PlayerController user)
        {
            yield return new WaitForSeconds(45f);
            user.PostProcessProjectile -= Empress;
            yield break;

        }

        public static void Empress(Projectile arg2, float arg3)
        {
            arg2.ChangeTintColorShader(0f, Color.red);
            arg2.projectile.baseData.damage *= 2f;


        }
        public static IEnumerator HandleShield(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(75f, 75f, 0f));
            RoRItems.m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
            user.sprite.usesOverrideMaterial = true;
            user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
            SpeculativeRigidbody specRigidbody = user.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(RoRItems.OnPreCollision));
            user.healthHaver.IsVulnerable = false;
            float elapsed = 0f;
            while (elapsed < 25f)
            {
                elapsed += BraveTime.DeltaTime;
                user.healthHaver.IsVulnerable = false;
                yield return null;
            }
            bool flag = user;
            if (flag)
            {
                user.healthHaver.IsVulnerable = true;
                user.ClearOverrideShader();
                user.sprite.usesOverrideMaterial = RoRItems.m_usedOverrideMaterial;
                SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
                specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(RoRItems.OnPreCollision));
                specRigidbody2 = null;
            }
                Material outlineMaterial1 = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
                outlineMaterial1.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
            
            yield break;
        }

        public static void TeleportToRoom(PlayerController targetPlayer, RoomHandler targetRoom)
        {
            RoRItems.m_IsTeleporting = true;
            Pixelator.Instance.FadeToBlack(1.5f, false, 0f);
            IntVector2? randomAvailableCell = targetRoom.GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, null);
            if (randomAvailableCell == null)
            {
               RoRItems.m_IsTeleporting = false;
                return;
            }
            if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
            {
                PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(targetPlayer);
                if (otherPlayer)
                {
                   RoRItems.TeleportToRoom(otherPlayer, targetRoom);
                }
            }
            targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
            GameManager.Instance.StartCoroutine(RoRItems.HandleTeleportToRoom(targetPlayer, randomAvailableCell.Value.ToCenterVector2()));
            targetPlayer.specRigidbody.Velocity = Vector2.zero;
            targetPlayer.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
            targetRoom.EnsureUpstreamLocksUnlocked();
        }
        public static void Rewardport()
        {
            
            ETGMod.StartGlobalCoroutine(RoRItems.DoOrDie(GameManager.Instance.PrimaryPlayer));

         }
        public static IEnumerator DoOrDie(PlayerController user)
        {
            FloodFillUtility.PreprocessContiguousCells(GameManager.Instance.PrimaryPlayer.CurrentRoom, GameManager.Instance.PrimaryPlayer.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), 0);
            IntVector2? targetCenter = new IntVector2?((user.CenterPosition + new Vector2(-2, 0)).ToIntVector2(VectorConversions.Floor));
            IntVector2? targetCenter1 = new IntVector2?((user.CenterPosition + new Vector2(+1, 0)).ToIntVector2(VectorConversions.Floor));
            string guid = "db97e486ef02425280129e1e27c33118";
            AIActor enemyPrefab = EnemyDatabase.GetOrLoadByGuid(guid);
            AIActor aiactor = AIActor.Spawn(enemyPrefab, targetCenter.Value, user.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
            AIActor aiactor1 = AIActor.Spawn(enemyPrefab, targetCenter1.Value, user.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
            aiactor.BecomeBlackPhantom();
            aiactor1.BecomeBlackPhantom();
            aiactor.HandleReinforcementFallIntoRoom(0.5f);
            aiactor1.HandleReinforcementFallIntoRoom(0.5f);
            while (aiactor != null || aiactor1 != null)
            {
                yield return new WaitForSeconds(3f);
                yield return null;
            }
            yield return new WaitForSeconds(2f);
            RoomHandler currentRoom = user.CurrentRoom;
            if (currentRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear))
            {
                RoRItems.StunEnemiesForTeleport(currentRoom, 4f);
            }
            user.ForceStopDodgeRoll();
            user.CurrentInputState = PlayerInputState.NoInput;
            RoRItems.DoTentacleVFX(user);
            RoomHandler creepyRoom = GameManager.Instance.Dungeon.AddRuntimeRoom(new IntVector2(24, 24), (GameObject)BraveResources.Load("Global Prefabs/CreepyEye_Room", ".prefab"));
            yield return new WaitForSeconds(1.3f);
            user.CurrentInputState = PlayerInputState.AllInput;
            user.WarpToPoint((creepyRoom.area.basePosition + new IntVector2(12, 4)).ToVector2(), false, false);

            if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
            {
                GameManager.Instance.GetOtherPlayer(user).ReuniteWithOtherPlayer(user, false);
            }
            yield return new WaitForSeconds(0.2f);
            Chest rainbow_Chest = GameManager.Instance.RewardManager.A_Chest;
            rainbow_Chest.IsLocked = false;
            Chest.Spawn(rainbow_Chest, new IntVector2((int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.x), (int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.y)));
            PickupObject pickupObject = Gungeon.Game.Items["ror:n'kuhana's_spoils"];
            LootEngine.GivePrefabToPlayer(pickupObject.gameObject, user);
            yield break;
        }
        public static IEnumerator Returnticket(PlayerController user)
        {
            yield return new WaitForSeconds(3f);
            yield return new WaitForSeconds(0.5f);
            PickupObject pickupObject = Gungeon.Game.Items["ror:return_ticket"];
            LootEngine.GivePrefabToPlayer(pickupObject.gameObject, user);
            yield break;
        }
        private static GameObject DoTentacleVFX(PlayerController user)
        {
            RoRItems.TentacleVFX = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Tentacleport");
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RoRItems.TentacleVFX);
            gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(user.specRigidbody.UnitBottomCenter + new Vector2(0f, -1f), tk2dBaseSprite.Anchor.LowerCenter);
            gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
            gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            return gameObject;
        }
        private static void StunEnemiesForTeleport(RoomHandler targetRoom, float StunDuration = 0.5f)
        {
            if (!targetRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
            {
                return;
            }
            List<AIActor> activeEnemies = targetRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies == null | activeEnemies.Count <= 0)
            {
                return;
            }
            for (int i = 0; i < activeEnemies.Count; i++)
            {
                if (activeEnemies[i].IsNormalEnemy && activeEnemies[i].healthHaver && !activeEnemies[i].healthHaver.IsBoss)
                {
                    if (activeEnemies[i].specRigidbody)
                    {
                        Vector2 unitBottomLeft = activeEnemies[i].specRigidbody.UnitBottomLeft;
                    }
                    else
                    {
                        Vector2 worldBottomLeft = activeEnemies[i].sprite.WorldBottomLeft;
                    }
                    if (activeEnemies[i].specRigidbody)
                    {
                        Vector2 unitTopRight = activeEnemies[i].specRigidbody.UnitTopRight;
                    }
                    else
                    {
                        Vector2 worldTopRight = activeEnemies[i].sprite.WorldTopRight;
                    }
                    if (activeEnemies[i] && activeEnemies[i].behaviorSpeculator)
                    {
                        activeEnemies[i].behaviorSpeculator.Stun(StunDuration, false);
                    }
                }
            }
        }
        internal static IEnumerator HandleTeleportToRoom(PlayerController targetPlayer, Vector2 targetPoint)
        {
            if (targetPlayer.transform.position.GetAbsoluteRoom() != null)
            {
               RoRItems.StunEnemiesForTeleport(targetPlayer.transform.position.GetAbsoluteRoom(), 1f);
            }
            targetPlayer.healthHaver.IsVulnerable = false;
            CameraController cameraController = GameManager.Instance.MainCameraController;
            Vector2 offsetVector = cameraController.transform.position - targetPlayer.transform.position;
            offsetVector -= cameraController.GetAimContribution();
            Minimap.Instance.ToggleMinimap(false, false);
            cameraController.SetManualControl(true, false);
            cameraController.OverridePosition = cameraController.transform.position;
            targetPlayer.CurrentInputState = PlayerInputState.NoInput;
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(1f);
            targetPlayer.ToggleRenderer(false, "arbitrary teleporter");
            targetPlayer.ToggleGunRenderers(false, "arbitrary teleporter");
            targetPlayer.ToggleHandRenderers(false, "arbitrary teleporter");
            yield return new WaitForSeconds(1f);
            Pixelator.Instance.FadeToBlack(0.15f, false, 0f);
            yield return new WaitForSeconds(0.15f);
            targetPlayer.transform.position = targetPoint;
            targetPlayer.specRigidbody.Reinitialize();
            targetPlayer.specRigidbody.RecheckTriggers = true;
            if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
            {
                cameraController.OverridePosition = cameraController.GetIdealCameraPosition();
            }
            else
            {
                cameraController.OverridePosition = (targetPoint + offsetVector).ToVector3ZUp(0f);
            }
            targetPlayer.WarpFollowersToPlayer(false);
            targetPlayer.WarpCompanionsToPlayer(false);
            Pixelator.Instance.MarkOcclusionDirty();
            Pixelator.Instance.FadeToBlack(0.15f, true, 0f);
            yield return null;
            cameraController.SetManualControl(false, true);
            yield return new WaitForSeconds(0.15f);
            targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
            targetPlayer.ToggleRenderer(true, "arbitrary teleporter");
            targetPlayer.ToggleGunRenderers(true, "arbitrary teleporter");
            targetPlayer.ToggleHandRenderers(true, "arbitrary teleporter");
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(targetPlayer.specRigidbody, null, false);
            targetPlayer.CurrentInputState = PlayerInputState.AllInput;
            targetPlayer.healthHaver.IsVulnerable = true;
           RoRItems.m_IsTeleporting = false;
            GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
            if (gameObject != null)
            {
                GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(targetPlayer.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
                gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
                gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            }
            yield break;
        }
        public static IEnumerator Assault(RoomHandler roomHandler, PlayerController user)
        {
            roomHandler.SealRoom();
            user.OnKilledEnemy += Kill1;
            float timesincespawn = 0f;
            yield return new WaitForSeconds(2.5f);
            TextBoxManager.ShowTextBox(user.specRigidbody.UnitCenter + new Vector2(0, 2), user.transform, 2f, $"You shouldn't have come here. Brace yourself.", user.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
            yield return new WaitForSeconds(2.5f);
            kills = new int();
            while (kills < 50 && user.CurrentRoom == roomHandler)
            {

                timesincespawn += BraveTime.DeltaTime;
                while (roomHandler.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) < 18)
                {
                    int index = UnityEngine.Random.Range(0, EnemyGuidDatabase.Entries.Count);
                    string text = EnemyGuidDatabase.Entries.Keys.ElementAt(index);
                    string guid = EnemyGuidDatabase.Entries[text];
                    AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
                    IntVector2? intVector = new IntVector2?(roomHandler.GetRandomVisibleClearSpot(2, 2));
                    AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Awaken, true);
                    aiactor.CanTargetEnemies = false;
                    aiactor.CanTargetPlayers = true;
                    PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
                    aiactor.IsHarmlessEnemy = false;
                    aiactor.IgnoreForRoomClear = false;
                    aiactor.gameObject.GetOrAddComponent<KillCount>();
                    timesincespawn = 0;
                    aiactor.HandleReinforcementFallIntoRoom(0.5f);
                    timesincespawn = 0;
                    yield return null;
                }
                if (timesincespawn > 8f)
                {
                    timesincespawn = 0;
                    for (int i = 0; i < UnityEngine.Random.Range(2, 6); i++)
                    {
                        int index = UnityEngine.Random.Range(0, EnemyGuidDatabase.Entries.Count);
                        string text = EnemyGuidDatabase.Entries.Keys.ElementAt(index);
                        string guid = EnemyGuidDatabase.Entries[text];

                        AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
                        IntVector2? intVector = new IntVector2?(roomHandler.GetRandomVisibleClearSpot(2, 2));
                        AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Awaken, true);
                        aiactor.CanTargetEnemies = false;
                        aiactor.CanTargetPlayers = true;
                        PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
                        aiactor.IsHarmlessEnemy = false;
                        aiactor.IgnoreForRoomClear = false;
                        aiactor.gameObject.GetOrAddComponent<KillCount>();
                        aiactor.HandleReinforcementFallIntoRoom(0.5f);
                    }
                }
                yield return null;
            }
                
                
            if (user.CurrentRoom != roomHandler && kills < 50)
                {
                    EntryWayShrineInteractible.activated = false;
            }
            KillAll(roomHandler, user);
            if (user.CurrentRoom == roomHandler)
            {
                Pixelator.Instance.FadeToColor(0.5f, Color.white, true, 1f);
                RoomHandler targetRoom = GameManager.Instance.PrimaryPlayer.CurrentRoom;
                IntVector2? intVector1 = new IntVector2?(new IntVector2(targetRoom.area.basePosition.x + 49, targetRoom.area.basePosition.y + 53));
                AIActor aiactor1 = AIActor.Spawn(Galactilich1.prefab.GetComponent<EnemyBehavior>().aiActor, intVector1.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector1.Value), true, AIActor.AwakenAnimationType.Awaken, true);
                aiactor1.CanTargetEnemies = false;
                aiactor1.CanTargetPlayers = true;
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor1.specRigidbody, null, false);
                aiactor1.IsHarmlessEnemy = false;
                aiactor1.IgnoreForRoomClear = false;
                aiactor1.HandleReinforcementFallIntoRoom(2f);
                ArenaCard.TeleportToRoom(GameManager.Instance.PrimaryPlayer, RoRItems.SecretArenaRoom);
            }
                user.OnKilledEnemy -= Kill1;
                yield break;
            
        }

        private static void KillAll(RoomHandler roomHandler, PlayerController owner)
        {
            try
            {

                    try { 
                     Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[481]).DefaultModule.chargeProjectiles[0].Projectile;
                    GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (owner.CurrentGun == null) ? 0f : owner.CurrentGun.CurrentAngle + 180f), true);
                    Projectile component = gameObject.GetComponent<Projectile>();
                    bool flag4 = component != null;
                    if (flag4)
                    {
                        component.baseData.damage = 10000000000;
                        component.baseData.speed = 0f;
                        component.AdditionalScaleMultiplier = 2.5f;
                        component.SetOwnerSafe(owner, "Player");
                        component.Shooter = owner.specRigidbody;
                        component.Owner = owner;
                    }
                    }
                    catch{ }
                
            }
            catch { }
        }

        private static void Kill1(PlayerController obj)
        {
            if (kills > 0 && kills % 5 == 0)
            TextBoxManager.ShowTextBox(obj.specRigidbody.UnitCenter, obj.transform, 1f, $"{50f - kills} enemies remain", obj.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
        }

        public static int kills;


        public static RoomHandler AddCustomRuntimeRoom(PrototypeDungeonRoom prototype, bool addRoomToMinimap = true, bool addTeleporter = true, bool isSecretRatExitRoom = false, Action<RoomHandler> postProcessCellData = null, DungeonData.LightGenerationStyle lightStyle = DungeonData.LightGenerationStyle.STANDARD)
        {
            Dungeon dungeon = GameManager.Instance.Dungeon;
            tk2dTileMap mainTilemap = dungeon.MainTilemap;
            if (mainTilemap == null)
            {
                global::ETGModConsole.Log("ERROR: TileMap object is null! Something seriously went wrong!", false);
                Debug.Log("ERROR: TileMap object is null! Something seriously went wrong!");
                return null;
            }
            TK2DDungeonAssembler tk2DDungeonAssembler = new TK2DDungeonAssembler();
            tk2DDungeonAssembler.Initialize(dungeon.tileIndices);
            IntVector2 zero = IntVector2.Zero;
            IntVector2 intVector = new IntVector2(50, 50);
            int x = intVector.x;
            int y = intVector.y;
            IntVector2 intVector2 = new IntVector2(int.MaxValue, int.MaxValue);
            IntVector2 lhs = new IntVector2(int.MinValue, int.MinValue);
            intVector2 = IntVector2.Min(intVector2, zero);
            IntVector2 intVector3 = IntVector2.Max(lhs, zero + new IntVector2(prototype.Width, prototype.Height)) - intVector2;
            IntVector2 b = IntVector2.Min(IntVector2.Zero, -1 * intVector2);
            intVector3 += b;
            IntVector2 intVector4 = new IntVector2(dungeon.data.Width + x, x);
            int newWidth = dungeon.data.Width + x * 2 + intVector3.x;
            int newHeight = Mathf.Max(dungeon.data.Height, intVector3.y + x * 2);
            CellData[][] array = BraveUtility.MultidimensionalArrayResize<CellData>(dungeon.data.cellData, dungeon.data.Width, dungeon.data.Height, newWidth, newHeight);
            dungeon.data.cellData = array;
            dungeon.data.ClearCachedCellData();
            IntVector2 intVector5 = new IntVector2(prototype.Width, prototype.Height);
            IntVector2 b2 = zero + b;
            IntVector2 intVector6 = intVector4 + b2;
            CellArea cellArea = new CellArea(intVector6, intVector5, 0);
            cellArea.prototypeRoom = prototype;
            RoomHandler roomHandler = new RoomHandler(cellArea);
            for (int i = -x; i < intVector5.x + x; i++)
            {
                for (int j = -x; j < intVector5.y + x; j++)
                {
                    IntVector2 intVector7 = new IntVector2(i, j) + intVector6;
                    if ((i >= 0 && j >= 0 && i < intVector5.x && j < intVector5.y) || array[intVector7.x][intVector7.y] == null)
                    {
                        CellData cellData = new CellData(intVector7, CellType.WALL);
                        cellData.positionInTilemap = cellData.positionInTilemap - intVector4 + new IntVector2(y, y);
                        cellData.parentArea = cellArea;
                        cellData.parentRoom = roomHandler;
                        cellData.nearestRoom = roomHandler;
                        cellData.distanceFromNearestRoom = 0f;
                        array[intVector7.x][intVector7.y] = cellData;
                    }
                }
            }
            dungeon.data.rooms.Add(roomHandler);
            try
            {
                roomHandler.WriteRoomData(dungeon.data);
            }
            catch (Exception)
            {
                global::ETGModConsole.Log("WARNING: Exception caused during WriteRoomData step on room: " + roomHandler.GetRoomName(), false);
            }
            try
            {
                dungeon.data.GenerateLightsForRoom(dungeon.decoSettings, roomHandler, GameObject.Find("_Lights").transform, lightStyle);
            }
            catch (Exception)
            {
                global::ETGModConsole.Log("WARNING: Exception caused during GeernateLightsForRoom step on room: " + roomHandler.GetRoomName(), false);
            }
            if (postProcessCellData != null)
            {
                postProcessCellData(roomHandler);
            }
            if (roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
            {
                roomHandler.BuildSecretRoomCover();
            }
            GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
            tk2dTileMap component = gameObject.GetComponent<tk2dTileMap>();
            string str = UnityEngine.Random.Range(10000, 99999).ToString();
            gameObject.name = "Glitch_RuntimeTilemap_" + str;
            component.renderData.name = "Glitch_RuntimeTilemap_" + str + " Render Data";
            component.Editor__SpriteCollection = dungeon.tileIndices.dungeonCollection;
            try
            {
                TK2DDungeonAssembler.RuntimeResizeTileMap(component, intVector3.x + y * 2, intVector3.y + y * 2, mainTilemap.partitionSizeX, mainTilemap.partitionSizeY);
                IntVector2 intVector8 = new IntVector2(prototype.Width, prototype.Height);
                IntVector2 b3 = zero + b;
                IntVector2 intVector9 = intVector4 + b3;
                for (int k = -y; k < intVector8.x + y; k++)
                {
                    for (int l = -y; l < intVector8.y + y + 2; l++)
                    {
                        tk2DDungeonAssembler.BuildTileIndicesForCell(dungeon, component, intVector9.x + k, intVector9.y + l);
                    }
                }
                RenderMeshBuilder.CurrentCellXOffset = intVector4.x - y;
                RenderMeshBuilder.CurrentCellYOffset = intVector4.y - y;
                component.ForceBuild();
                RenderMeshBuilder.CurrentCellXOffset = 0;
                RenderMeshBuilder.CurrentCellYOffset = 0;
                component.renderData.transform.position = new Vector3((float)(intVector4.x - y), (float)(intVector4.y - y), (float)(intVector4.y - y));
            }
            catch (Exception exception)
            {
                global::ETGModConsole.Log("WARNING: Exception occured during RuntimeResizeTileMap / RenderMeshBuilder steps!", false);
                Debug.Log("WARNING: Exception occured during RuntimeResizeTileMap/RenderMeshBuilder steps!");
                Debug.LogException(exception);
            }
            roomHandler.OverrideTilemap = component;
            for (int m = 0; m < roomHandler.area.dimensions.x; m++)
            {
                for (int n = 0; n < roomHandler.area.dimensions.y + 2; n++)
                {
                    IntVector2 intVector10 = roomHandler.area.basePosition + new IntVector2(m, n);
                    if (dungeon.data.CheckInBoundsAndValid(intVector10))
                    {
                        CellData currentCell = dungeon.data[intVector10];
                        TK2DInteriorDecorator.PlaceLightDecorationForCell(dungeon, component, currentCell, intVector10);
                    }
                }
            }
            Pathfinder.Instance.InitializeRegion(dungeon.data, roomHandler.area.basePosition + new IntVector2(-3, -3), roomHandler.area.dimensions + new IntVector2(3, 3));
            if (prototype.usesProceduralDecoration && prototype.allowFloorDecoration)
            {
                new TK2DInteriorDecorator(tk2DDungeonAssembler).HandleRoomDecoration(roomHandler, dungeon, mainTilemap);
            }
            roomHandler.PostGenerationCleanup();
            if (addRoomToMinimap)
            {
                roomHandler.visibility = RoomHandler.VisibilityStatus.VISITED;
                ETGMod.StartGlobalCoroutine(Minimap.Instance.RevealMinimapRoomInternal(roomHandler, true, true, false));
                if (isSecretRatExitRoom)
                {
                    roomHandler.visibility = RoomHandler.VisibilityStatus.OBSCURED;
                }
            }
            if (addTeleporter)
            {
                roomHandler.AddProceduralTeleporterToRoom();
            }
            if (addRoomToMinimap)
            {
                Minimap.Instance.InitializeMinimap(dungeon.data);
            }
            DeadlyDeadlyGoopManager.ReinitializeData();
            return roomHandler;
        }
        private static void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            bool flag = component != null && !(component.Owner is PlayerController);
            if (flag)
            {
                PassiveReflectItem.ReflectBullet(component, true, GameManager.Instance.PrimaryPlayer.specRigidbody.gameActor, 10f, 1f, 1f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }
        public static IEnumerator Strenghtcard(PlayerController user)
        {
            yield return new WaitForSeconds(45f);
            GameManager.Instance.PrimaryPlayer.PostProcessProjectile -= Strenght;
            yield break;

        }

        public static void Strenght(Projectile arg2, float arg3)
        {
            Projectile arg1 = arg2.GetComponent<Projectile>();
            arg1.ChangeTintColorShader(0f, Color.yellow);
            arg1.gameObject.GetOrAddComponent<ExplosiveProjectile>();
        }

        public static IEnumerator TowerRockSlide(PlayerController user)
        {
            float elapsed = 0f;
            while (elapsed < 9f)
            {
                yield return new WaitForSeconds(0.6f);
                elapsed += 0.6f;
                Vector2 intVector = new Vector2(GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(2, 2).x, GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(2, 2).y);
                AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
                GameObject Mines_Cave_In = assetBundle2.LoadAsset<GameObject>("Mines_Cave_In");
                ETGMod.StartGlobalCoroutine(RoRItems.HandleTriggerRockSlide(user, Mines_Cave_In, intVector));
                yield return null;
            }
            yield break;

        }

        private static IEnumerator HandleTriggerRockSlide(PlayerController user, GameObject RockSlidePrefab, Vector2 targetPosition)
        {
            RoomHandler currentRoom = user.CurrentRoom;
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RockSlidePrefab, targetPosition, Quaternion.identity);
            HangingObjectController RockSlideController = gameObject.GetComponent<HangingObjectController>();
            RockSlideController.triggerObjectPrefab = null;
            GameObject[] additionalDestroyObjects = new GameObject[]
            {
                RockSlideController.additionalDestroyObjects[1]
            };
            RockSlideController.additionalDestroyObjects = additionalDestroyObjects;
            UnityEngine.Object.Destroy(gameObject.transform.Find("Sign").gameObject);
            RockSlideController.ConfigureOnPlacement(currentRoom);
            yield return new WaitForSeconds(0.01f);
            RockSlideController.Interact(user);
            if (UnityEngine.Random.value < 0.11)
            {
                LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, user.CenterPosition, targetPosition, 0f);
            }
            user.ForceBlank(15f, 05, false, true, targetPosition);
            yield break;
        }
        //card effects

        public static List<PlayerItem> cards = new List<PlayerItem>();
        private static Dungeon forge;
        private PrototypeDungeonRoom prototypeDungeonRoom;
        private static bool m_IsTeleporting;
        private static bool m_usedOverrideMaterial;
        private static GameObject TentacleVFX;
        public static bool activereloadsuccess;
        public static string ZipFilePath;
        public static string FilePath;
        private RoomFactory.RoomData value;

        public static RoomHandler SecretArenaRoom;
        internal static RoomHandler entrywayroom;
        internal static GameObject entryshrine;
        internal static int Cardprice;
        public int MobiusWithdrawnThisRun;
        internal static int pulls;

        public static Action<DeadlyDeadlyGoopManager, GameActor, IntVector2> OnGoopTouched;

        private void Synergies()
        {
            List<string> mandatoryConsoleID1 = new List<string> {
            "orbital_bullets"
            };
            List<string> optionalConsoleID1 = new List<string> {
            "ror:light_dancer",
            "ror:prism",
            "ror:oscilloscope",
            "ror:disco_laser_panic"
            };
            CustomSynergies.Add("The Last Light", mandatoryConsoleID1, optionalConsoleID1);

            List<string> mandatoryConsoleIDBuisness1 = new List<string> {
            "ror:flamesaw"
            };
            List<string> optionalConsoleIDBuisness1 = new List<string> {
            "serious_cannon",
            "briefcase_of_cash",
            "devolver",
            "bomb",
            "ice_bomb"
            };
            CustomSynergies.Add("Serious Buisness", mandatoryConsoleIDBuisness1, optionalConsoleIDBuisness1);

            List<string> mandatoryConsoleIDrose = new List<string> {
            "ror:crescent_rose"
            };
            List<string> optionalConsoleIDrose1 = new List<string> {
            "bloodied_scarf",
            "ancient_heros_bandana",
            "ror:monotip_dagger",
            "ror:mercenary's_sheath"
            };
            CustomSynergies.Add("Petal Burst", mandatoryConsoleIDrose, optionalConsoleIDrose1);

            List<string> mandatoryConsoleIDbandit1 = new List<string> {
            "ror:bandit's_revolver"
            };
            List<string> optionalConsoleIDbandit = new List<string> {
            "flare_gun",
            "hot_lead",
            "ror:will_the_wisp",
            "ror:mercenary's_sheath"
            };
            CustomSynergies.Add("Smoldering Heart", mandatoryConsoleIDbandit1, optionalConsoleIDbandit);

            List<string> mandatoryConsoleIDquantum = new List<string> {
            "ror:quantum_collapser"
            };
            List<string> optionalConsoleIDquantum1 = new List<string> {
            "black_hole_gun",
            "relodestone",
            "ror:abyssal_vortex",
            "ror:primordial_cube"
            };
            CustomSynergies.Add("The Big Blank", mandatoryConsoleIDquantum, optionalConsoleIDquantum1);

            List<string> mandatoryConsoleIDlantern = new List<string> {
            "ror:storm_lantern"
            };
            List<string> optionalConsoleIDlantern1 = new List<string> {
            "thunderclap",
            "shock_rounds",
            "ror:tesla_coil",
            "ror:shock_shells"
            };
            CustomSynergies.Add("OverCharge!", mandatoryConsoleIDlantern, optionalConsoleIDlantern1);

            List<string> mandatoryConsoleIDshadowedge = new List<string> {
            "ror:shadow's_edge"
            };
            List<string> optionalConsoleIDshadowedge1 = new List<string> {
            "shadow_bullets",
            "ror:nodachi",
            "ror:mercenary's_sheath",
            "bloodied_scarf"
            };
            CustomSynergies.Add("The Second Slash", mandatoryConsoleIDshadowedge, optionalConsoleIDshadowedge1);

            List<string> mandatoryConsoleIDion1 = new List<string> {
            "ror:ion_rifle"
            };
            List<string> optionalConsoleIDion = new List<string> {
            "laser_sight",
            "sniper_rifle",
            "cog_of_battle",
            "ror:anti-materiel_ammo",
            "awp",
            "ror:neon_sign"
            };
            CustomSynergies.Add("Deadeye", mandatoryConsoleIDion1, optionalConsoleIDion);

            List<string> mandatoryConsoleID31 = new List<string> {"ror:excellerator",
            "ror:everblast",
            "ror:quickshot",
            "ror:launcher"
            };
            List<string> optionalConsoleID31 = new List<string> {
            "ror:excellerator",
            "ror:everblast",
            "ror:quickshot",
            "ror:launcher"
            };
            CustomSynergies.Add("Brand Loyality", mandatoryConsoleID31, optionalConsoleID31);


            List<string> mandatoryConsoleID21 = new List<string> {
            "ror:megadrill"
            };
            List<string> optionalConsoleID21 = new List<string> {
            "ror:anti-materiel_ammo",
            };
            CustomSynergies.Add("Extreme Penetration", mandatoryConsoleID21, optionalConsoleID21);

            List<string> mandatoryConsoleID261 = new List<string> {
            "ror:futurekiller"
            };
            List<string> optionalConsoleID261 = new List<string> {
            "ror:soulbound_bullets",
            "holey_grail"
            };
            CustomSynergies.Add("Soul Stealer", mandatoryConsoleID261, optionalConsoleID261);

            List<string> mandatoryConsoleID2221 = new List<string> {
            "ror:injector_gun"
            };
            List<string> optionalConsoleID2221 = new List<string> {
            "ror:n'kuhana's_touch",
            };
            CustomSynergies.Add("Poison Threat", mandatoryConsoleID2221, optionalConsoleID2221);

            List<string> mandatoryConsoleID22221 = new List<string> {
            "ror:scorched_hand"
            };
            List<string> optionalConsoleID22221 = new List<string> {
            "ror:will_the_wisp",
            };
            CustomSynergies.Add("Fire Hazard", mandatoryConsoleID22221, optionalConsoleID22221);

            List<string> mandatoryConsoleID221 = new List<string> {
            "ror:darkest_hour"
            };
            List<string> optionalConsoleID221 = new List<string> {
            "ror:hungering_chamber",
            };
            CustomSynergies.Add("The Hungry Dark", mandatoryConsoleID221, optionalConsoleID221);


            List<string> mandatoryConsoleIDs = new List<string> {
            "ror:the_cold_one",
            "ror:dragon's_dream"
            };
            CustomSynergies.Add("Of Fire And Ice", mandatoryConsoleIDs);
            DwieldProcessor advancedDualWieldSynergyProcessor3 = (PickupObjectDatabase.GetById(HeavyFlamer.FlamerInt) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor3.PartnerGunID = Icer.icerInt; 
            advancedDualWieldSynergyProcessor3.SynergyNameToCheck = "Of Fire And Ice";
            DwieldProcessor advancedDualWieldSynergyProcessor4 = (PickupObjectDatabase.GetById(Icer.icerInt) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor4.PartnerGunID = HeavyFlamer.FlamerInt;
            advancedDualWieldSynergyProcessor4.SynergyNameToCheck = "Of Fire And Ice";
            

            List<string> mandatoryConsoleID2rs = new List<string> {
            "ror:main_cannon",
            "ror:noisy_cricket"
            };
            CustomSynergies.Add("Little Big Guns", mandatoryConsoleID2rs);
            DwieldProcessor advancedDualWieldSynergyProcessorr23 = (PickupObjectDatabase.GetById(Revolver1.kriketid) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessorr23.PartnerGunID = Barrel.TankId;
            advancedDualWieldSynergyProcessorr23.SynergyNameToCheck = "Little Big Guns";
            DwieldProcessor advancedDualWieldSynergyProcessorr24 = (PickupObjectDatabase.GetById(Barrel.TankId) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessorr24.PartnerGunID = Revolver1.kriketid;
            advancedDualWieldSynergyProcessorr24.SynergyNameToCheck = "Little Big Guns";


            List<string> mandatoryConsoleID3 = new List<string> {
            "ror:mutant_arm"
            };
            List<string> Optids = new List<string> {
            "ror:rad_rounds",
            "ror:slag_shells",
            "mutation"
            };
            CustomSynergies.Add("Ultra Mutation!", mandatoryConsoleID3, Optids);
            DwieldProcessor advancedDualWieldSynergyProcessor5 = (PickupObjectDatabase.GetById(Punch.PunchId) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor5.PartnerGunID = 333;
            advancedDualWieldSynergyProcessor5.SynergyNameToCheck = "Ultra Mutation!";
            DwieldProcessor advancedDualWieldSynergyProcessor6 = (PickupObjectDatabase.GetById(333) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor6.PartnerGunID = Punch.PunchId;
            advancedDualWieldSynergyProcessor6.SynergyNameToCheck = "Ultra Mutation!";
            
            
            List<string> mandatoryConsoleID2 = new List<string> {
            "ror:slag_nebula",
            "ror:critical_fail"
            };
            CustomSynergies.Add("Shock And Awe", mandatoryConsoleID2);
            DwieldProcessor advancedDualWieldSynergyProcessor1 = (PickupObjectDatabase.GetById(SlagSMG.slagint) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor1.PartnerGunID = MoxyMwanSMG.MMsmgInt;
            advancedDualWieldSynergyProcessor1.SynergyNameToCheck = "Shock And Awe";
            DwieldProcessor advancedDualWieldSynergyProcessor2 = (PickupObjectDatabase.GetById(MoxyMwanSMG.MMsmgInt) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor2.PartnerGunID = SlagSMG.slagint;
            advancedDualWieldSynergyProcessor2.SynergyNameToCheck = "Shock And Awe";
            
            
            List<string> mandatoryConsoleID4 = new List<string> {
            "ror:megathorn"
            };
            List<string> Optid1s = new List<string> {
            "ror:gungeon_bloom",
            "mahoguny",
            "pea_shooter",
            "cactus"
            };
            CustomSynergies.Add("Plant Life", mandatoryConsoleID4, Optid1s);
            
            
            List<string> mandatoryConsoleIDa = new List<string> {
            "ror:plasma_whip"
            };
            List<string> Optid1a = new List<string> {
            "heroine",
            "roll_bomb"
            };
            CustomSynergies.Add("Down-Throw>Down-Special", mandatoryConsoleIDa, Optid1a);
            
            List<string> mandatoryConsoleID72 = new List<string> {
            "ror:mortar",
            "ror:revolution"
            };
            CustomSynergies.Add("Winter War", mandatoryConsoleID72);
            DwieldProcessor advancedDualWieldSynergyProcessor71 = (PickupObjectDatabase.GetById(VladofMortar.MOrtid) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor71.PartnerGunID = Revolution.RevId;
            advancedDualWieldSynergyProcessor71.SynergyNameToCheck = "Winter War";
            DwieldProcessor advancedDualWieldSynergyProcessor72 = (PickupObjectDatabase.GetById(Revolution.RevId) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor72.PartnerGunID = VladofMortar.MOrtid;
            advancedDualWieldSynergyProcessor72.SynergyNameToCheck = "Winter War";

            List<string> mandatoryConsoleID272 = new List<string> {
            "ror:magic_wand",
            "ror:starletite_cannon"
           
            };
            CustomSynergies.Add("Magic Warfare", mandatoryConsoleID272);
            DwieldProcessor advancedDualWieldSynergyProcessor271 = (PickupObjectDatabase.GetById(Wand.Wandid) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor271.PartnerGunID = Stargun.Smid;
            advancedDualWieldSynergyProcessor271.SynergyNameToCheck = "Magic Warfare";
            DwieldProcessor advancedDualWieldSynergyProcessor272 = (PickupObjectDatabase.GetById(Stargun.Smid) as Gun).gameObject.AddComponent<DwieldProcessor>();
            advancedDualWieldSynergyProcessor272.PartnerGunID = Wand.Wandid;
            advancedDualWieldSynergyProcessor272.SynergyNameToCheck = "Magic Warfare";

        }

        private void Weight(string[] args)
        {
            float itemdisabled = 0;
            float failures = 0f;
            if (!ETGModConsole.ArgCount(args, 1))
            {
                return;
            }
            foreach (String String in args)
            {
                try
                {
                    string text = String;
                    if (!Gungeon.Game.Items[text])
                    {
                        ETGModConsole.Log(string.Format("Invalid item ID", text), false);
                        return;
                    }
                    ETGModConsole.Log(string.Concat(new object[]
                    {
            "Attempting to disable item ID ",
            text,
            " , class ",
            Gungeon.Game.Items.Get(text).GetType()
                    }), false);
                    int pickupid = Gungeon.Game.Items[text].PickupObjectId;
                    itemdisabled += 1;
                    foreach (WeightedGameObject weightedGameObject in GameManager.Instance.RewardManager.GunsLootTable.defaultItemDrops.elements)
                    {
                        bool flag2 = weightedGameObject.pickupId == pickupid;
                        if (flag2)
                        {
                            weightedGameObject.weight = 0f;
                            itemdisabled += 1;
                        }
                    }
                }
                catch
                {
                    failures += 1;
                    string text = String;
                    ETGModConsole.Log(string.Concat(new object[]
                    {
                        $"<color=#ff0000>Invalid Item Name ",
                        text,
                        "</color>"  }), false);
                }

            }
            Log($"{itemdisabled} have been banned from the chest/shop pools, Note: master rounds, scripted items and resourceful rat items will still show up in their locations", TEXT_COLOR);
            if (failures > 0) { Log($"<color=#ff0000>{failures} have failed to be banned</color>"); }
        }
        

        public static void Log(string text, string color="FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public override void Exit() { }
        public override void Init() { }
    }
    internal class SynergiesHub
    {
        // Token: 0x06000161 RID: 353 RVA: 0x00011A7C File Offset: 0x0000FC7C
        public static bool PlayerHasPickup(PlayerController p, int pickupID)
        {
            bool flag = p && p.inventory != null && p.inventory.AllGuns != null;
            if (flag)
            {
                for (int i = 0; i < p.inventory.AllGuns.Count; i++)
                {
                    bool flag2 = p.inventory.AllGuns[i].PickupObjectId == pickupID;
                    if (flag2)
                    {
                        return true;
                    }
                }
            }
            bool flag3 = p;
            if (flag3)
            {
                for (int j = 0; j < p.activeItems.Count; j++)
                {
                    bool flag4 = p.activeItems[j].PickupObjectId == pickupID;
                    if (flag4)
                    {
                        return true;
                    }
                }
                for (int k = 0; k < p.passiveItems.Count; k++)
                {
                    bool flag5 = p.passiveItems[k].PickupObjectId == pickupID;
                    if (flag5)
                    {
                        return true;
                    }
                }
                bool flag6 = pickupID == GlobalItemIds.Map && p.EverHadMap;
                if (flag6)
                {
                    return true;
                }
            }
            return false;
        } }
    internal class RunesAndCards : MonoBehaviour
    {
        public int storage = 1;
    }

        

}
