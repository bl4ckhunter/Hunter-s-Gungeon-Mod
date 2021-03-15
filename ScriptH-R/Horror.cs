using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;
using MultiplayerBasicExample;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Nuklearkin : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Engineer's Folly";
            string resourceName = "ClassLibrary1/Resources/Turrent"; ;
            GameObject gameObject = new GameObject();
            Nuklearkin Flamer = gameObject.AddComponent<Nuklearkin>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Master Blaster";
            string longDesc = "... and so he left, more steel and circuit than man.\n\n\n\n" + "Reloading with an empty magazine spawns a nuclear pinhead.";
            Flamer.SetupItem(shortDesc, longDesc, "ror");
            Flamer.quality = PickupObject.ItemQuality.S;
            Flamer.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
            Flamer.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);

        }
        public override void Pickup(PlayerController player)
        {
            player.OnReloadedGun += this.OnDealtDamage;
            player.OnEnteredCombat += this.KillBombBoi;
            player.OnNewFloorLoaded += this.NukeCheck;
            this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
            base.Pickup(player);
        }

        private void NukeCheck(PlayerController obj)
        {
            if (this.Bomb != null)
            {
                this.Bomb = null;
                this.Bomb.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false); }
            this.alive = false;
        }

        private void KillBombBoi()
        {
            if(this.Bomb != null)
            { this.Bomb.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false);}
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReloadedGun -= this.OnDealtDamage;
            this.KillBombBoi();
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnReloadedGun -= this.OnDealtDamage;
                this.KillBombBoi();
                base.OnDestroy();
            }
        }

        private void OnDealtDamage(PlayerController usingPlayer, Gun gun)
        {   if(!this.alive && gun.IsEmpty)
            {
                this.alive = true;
                AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("4d37ce3d666b4ddda8039929225b7ede");
                IntVector2? intVector = new IntVector2?(this.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
                AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
                aiactor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
                PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
                aiactor.gameObject.AddComponent<KillOnRoomClear>();
                this.Bomb = aiactor;
                aiactor.IsHarmlessEnemy = true;
                aiactor.IgnoreForRoomClear = true;
                this.ProcessGunShader(this.Bomb);
                aiactor.healthHaver.OnDeath += this.Nukem;
            }
        }
        protected override void Update()
        {
            base.Update();
            if (base.Owner)
            {
                if (this.Bomb.sprite != null)
                {
                    Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(this.Bomb.sprite);
                    outlineMaterial.SetColor("_OverrideColor", new Color(0f, 45f, 0f));

                }
            }
            if (!base.Owner)
            { this.KillBombBoi();}


        }


        private void ProcessGunShader(AIActor g)
        {
            MeshRenderer component = g.GetComponent<MeshRenderer>();
            if (!component)
            {
                return;
            }
            Material[] sharedMaterials = component.sharedMaterials;
            for (int i = 0; i < sharedMaterials.Length; i++)
            {
                if (sharedMaterials[i].shader == this.m_glintShader)
                {
                    return;
                }
            }
            Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
            Material material = new Material(this.m_glintShader);
            material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
            sharedMaterials[sharedMaterials.Length - 1] = material;
            component.sharedMaterials = sharedMaterials;
            return;
        }

        private void Nukem(Vector2 target2)
        {
            Vector2 worldCenter = this.Bomb.sprite.WorldCenter;
            Vector3 target = this.Bomb.sprite.WorldCenter;
            Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[481]).DefaultModule.chargeProjectiles[0].Projectile;
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, this.Bomb.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + 180f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag4 = component != null;
            if (flag4)
            {
                component.baseData.damage = 20f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.baseData.speed = 0f;
                component.AdditionalScaleMultiplier = 2.5f;
                component.SetOwnerSafe(base.Owner, "Player");
                component.Shooter = base.Owner.specRigidbody;
                component.Owner = base.Owner;
            }
            this.Boom(target);
            base.Owner.ForceBlank(3f, 0.5f, false, true, worldCenter, false, -1f);
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition1 = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            GoopDefinition goopDefinition = UnityEngine.Object.Instantiate<GoopDefinition>(goopDefinition1);
            goopDefinition.baseColor32 = new Color32(0, 255, 255, 255);
            goopDefinition.fireColor32 = new Color32(0, 255, 255, 255);
            goopDefinition.UsesGreenFire = true;
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(this.Bomb.sprite.WorldCenter, 5f, 0.1f, false);
            this.Nuke = assetBundle.LoadAsset<GameObject>("assets/data/vfx prefabs/impact vfx/vfx_explosion_nuke.prefab");
            GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(this.Nuke);
            gameObject1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(this.Bomb.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.LowerCenter);
            gameObject1.transform.position = gameObject.transform.position.Quantize(0.0625f);
            gameObject1.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            this.alive = false;
        }


        public void Boom(Vector3 position)
        {
            ExplosionData defaultSmallExplosionData2 = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            this.smallPlayerSafeExplosion.effect = defaultSmallExplosionData2.effect;
            this.smallPlayerSafeExplosion.ignoreList = defaultSmallExplosionData2.ignoreList;
            this.smallPlayerSafeExplosion.ss = defaultSmallExplosionData2.ss;
            Exploder.Explode(position, this.smallPlayerSafeExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
        }

        private ExplosionData smallPlayerSafeExplosion = new ExplosionData
        {
            damageRadius = 4.5f,
            damageToPlayer = 0f,
            doDamage = true,
            damage = 30f,
            doExplosionRing = true,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 30f,
            preventPlayerForce = true,
            explosionDelay = 0f,
            usesComprehensiveDelay = false,
            doScreenShake = false,
            playDefaultSFX = true
        };

        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;

        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private  bool onCooldown; private  bool onCooldown1;
        private float FlamerBase = 7f;

        // Token: 0x0400001D RID: 29
        private float FlamerTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;
        private bool firingflag;
        private bool engaged;
        private float sign;
        private  bool activated;
        private object LastOwner;
        private AIActor Bomb;
        private GameObject Nuke;
        private Shader m_glintShader;
        private bool alive;
    }
      
 }     