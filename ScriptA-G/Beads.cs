using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Beads : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Prayer Beads";
            string resourceName = "ClassLibrary1/Resources/Beads"; ;
            GameObject gameObject = new GameObject();
            Beads Beads = gameObject.AddComponent<Beads>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Blank Mind";
            string longDesc = "A string of prayer beads made from lunar material, they have been blessed to provide protection when blanking";
            Beads.SetupItem(shortDesc, longDesc, "ror");
            Beads.quality = PickupObject.ItemQuality.B;
            Beads.AddPassiveStatModifier(PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
            Beads.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);

        }
        public override void Pickup(PlayerController player)
        {
            player.OnUsedBlank+= this.Burst;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedBlank -= this.Burst;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnUsedBlank -= this.Burst;
            }
            base.OnDestroy();
        }
        private void Burst(PlayerController usingPlayer, int blank)
        {
            bool flag21 = !Beads.onCooldown1;
            if (flag21)
            {
                Beads.onCooldown1 = true;
                GameManager.Instance.StartCoroutine(Beads.StartCooldown1());
                this.BeadsSalvo(usingPlayer);
                this.Shock(usingPlayer);
            }
        }
        private static IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(0.5f);
            Beads.onCooldown1 = false;
            yield break;
        }

        protected override void Update()
        {
            base.Update();
            this.CheckDamage();
        }
        private void CheckDamage()
        {
            this.Damage = base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            bool flag = this.Damage == this.lastDamage;
            if (!flag)
            {
                this.BeadsTrue = this.BeadsBase * this.Damage;
                this.lastDamage = this.Damage;
            }
        }

        private void BeadsSalvo(PlayerController player)
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[365]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 0f), true);
            GameObject gameObject1 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 180f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component1 = gameObject1.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = base.Owner;
                BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
                HungryProjectileModifier hungryProjectileModifier = component.gameObject.AddComponent<HungryProjectileModifier>();
                hungryProjectileModifier.MaximumBulletsEaten = 9999; 
                hungryProjectileModifier.DamagePercentGainPerSnack = 0f;
                component.AdjustPlayerProjectileTint(Color.white.WithAlpha(Color.white.a / 0.5f), 5, 0f);
                piercer.penetration = 10000; 
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
                bouncer.projectile.ResetDistance();
                bouncer.projectile.baseData.range = Mathf.Max(bouncer.projectile.baseData.range, 500f);
                if (bouncer.projectile.baseData.speed > 30f)
                {
                    bouncer.projectile.baseData.speed = 20f;
                    bouncer.projectile.UpdateSpeed();
                }
                OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
                orbitProjectileMotionModule.lifespan = 20f;
                orbitProjectileMotionModule.MaxRadius = 3f;
                orbitProjectileMotionModule.MinRadius = 3f;
                
                if (bouncer.projectile.OverrideMotionModule != null && bouncer.projectile.OverrideMotionModule is HelixProjectileMotionModule)
                {
                    orbitProjectileMotionModule.StackHelix = true;
                    orbitProjectileMotionModule.ForceInvert = (bouncer.projectile.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
                }
                bouncer.projectile.OverrideMotionModule = orbitProjectileMotionModule;
            }
            bool flag1 = component1 != null;
            if (flag1)
            {
                component1.Owner = base.Owner;
                BounceProjModifier bouncer = component1.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component1.gameObject.AddComponent<PierceProjModifier>();
                component1.AdjustPlayerProjectileTint(Color.black.WithAlpha(Color.black.a / 0.99f), 5, 0f);
                HungryProjectileModifier hungryProjectileModifier = component1.gameObject.AddComponent<HungryProjectileModifier>();
                hungryProjectileModifier.MaximumBulletsEaten = 9999;
                hungryProjectileModifier.DamagePercentGainPerSnack = 0f;
                piercer.penetration = 10000;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
                bouncer.projectile.ResetDistance();
                bouncer.projectile.baseData.range = Mathf.Max(bouncer.projectile.baseData.range, 500f);
                if (bouncer.projectile.baseData.speed > 30f)
                {
                    bouncer.projectile.baseData.speed = 20f;
                    bouncer.projectile.UpdateSpeed();
                }
                OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
                orbitProjectileMotionModule.lifespan = 20f;
                orbitProjectileMotionModule.MaxRadius = 3f;
                orbitProjectileMotionModule.MinRadius = 3f;

                if (bouncer.projectile.OverrideMotionModule != null && bouncer.projectile.OverrideMotionModule is HelixProjectileMotionModule)
                {
                    orbitProjectileMotionModule.StackHelix = true;
                    orbitProjectileMotionModule.ForceInvert = (bouncer.projectile.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
                }
                bouncer.projectile.OverrideMotionModule = orbitProjectileMotionModule;
            }

        }
        private void Shock(PlayerController user)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/water goop.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(base.Owner.sprite.WorldCenter, 5f, 0.01f, false);
        }
        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;


        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private static bool onCooldown; private static bool onCooldown1;
        private float BeadsBase = 0f;

        // Token: 0x0400001D RID: 29
        private float BeadsTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;

        public GameActorFreezeEffect FreezeModifierEffect;
        private float FreezeAmountPerDamage;
    }
      
 }     