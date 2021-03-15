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
    public class Relic : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Ice Relic";
            string resourceName = "ClassLibrary1/Resources/FrostRelic"; ;
            GameObject gameObject = new GameObject();
            Relic Relic = gameObject.AddComponent<Relic>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Freeze Their Soul";
            string longDesc = "A millennia old ice cristal, it's mere sight is enough to make everyone stop in their tracks. It's covered in frosted flakes of what looks suspiciously like blood. Cool.";
            Relic.SetupItem(shortDesc, longDesc, "ror");
            Relic.quality = PickupObject.ItemQuality.B;
            ItemBuilder.AddPassiveStatModifier(Relic, PlayerStats.StatType.Coolness, 1f, StatModifier.ModifyMethod.ADDITIVE);
            Relic.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);

        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += this.OnDealtDamage;
            player.OnKilledEnemy += this.Burst;
            player.OnKilledEnemy += this.Burst;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamageContext -= this.OnDealtDamage;
            player.OnKilledEnemy -= this.Burst;
            player.OnKilledEnemy -= this.Burst;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnDealtDamageContext -= this.OnDealtDamage;
                player.OnKilledEnemy -= this.Burst;
                player.OnKilledEnemy -= this.Burst;
            }
            base.OnDestroy();
        }

        private void Burst(PlayerController usingPlayer)
        {
            bool flag21 = !Relic.onCooldown1;
            if (flag21)
            {
                Relic.onCooldown1 = true;
                GameManager.Instance.StartCoroutine(Relic.StartCooldown1());
                this.RelicSalvo(usingPlayer);
            }
        }
        private void OnDealtDamage(PlayerController usingPlayer, float amount, bool fatal, HealthHaver targetr)
        {
            
                {
                    bool flag2 = !Relic.onCooldown;
                    if (flag2)

                {
                    Relic.onCooldown = true;
                    GameManager.Instance.StartCoroutine(Relic.StartCooldown());
                        float value = UnityEngine.Random.value;
                        bool flag = (double)value < 0.35f;
                        if (flag)
                        {
                            this.Burst(usingPlayer);
                        }
                    }
                }

        }
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(0.1f);
            Relic.onCooldown = false;
            yield break;
        }
        private static IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(0.1f);
            Relic.onCooldown1 = false;
            yield break;
        }

        protected override void Update()
        {
            base.Update();
            if (base.Owner)
            {
                this.CheckDamage();
            }
        }
        private void CheckDamage()
        {
            this.Damage = base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            bool flag = this.Damage == this.lastDamage;
            if (!flag)
            {
                this.RelicTrue = this.RelicBase * this.Damage;
                this.lastDamage = this.Damage;
            }
        }

        private void RelicSalvo(PlayerController player)
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[130]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 0f), true);

            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = base.Owner;
                BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
                ExplosiveModifier explosive = component.gameObject.GetComponent<ExplosiveModifier>();
                if (explosive)
                { Destroy(explosive); }
                piercer.penetration = 10;
                component.specRigidbody.OnPreRigidbodyCollision += Checkchest;
                bouncer.projectile.specRigidbody.CollideWithTileMap = false;
                bouncer.projectile.ResetDistance();
                bouncer.projectile.baseData.range = Mathf.Max(bouncer.projectile.baseData.range, 500f);
                if (bouncer.projectile.baseData.speed > 50f)
                {
                    bouncer.projectile.baseData.speed = 20f;
                    bouncer.projectile.UpdateSpeed();
                }
                OrbitProjectileMotionModule orbitProjectileMotionModule = new OrbitProjectileMotionModule();
                orbitProjectileMotionModule.lifespan = 15f;
                if (bouncer.projectile.OverrideMotionModule != null && bouncer.projectile.OverrideMotionModule is HelixProjectileMotionModule)
                {
                    orbitProjectileMotionModule.StackHelix = true;
                    orbitProjectileMotionModule.ForceInvert = (bouncer.projectile.OverrideMotionModule as HelixProjectileMotionModule).ForceInvert;
                }
                bouncer.projectile.OverrideMotionModule = orbitProjectileMotionModule;
            }
   
            }

        private void Checkchest(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {   if (otherRigidbody.gameObject != null && myRigidbody.gameObject != null)
            {
                if (otherRigidbody.GetComponent<Chest>())
                { Destroy(myRigidbody.gameObject); }
            }
            }


        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;


        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private static bool onCooldown; private static bool onCooldown1;
        private float RelicBase = 3.5f;

        // Token: 0x0400001D RID: 29
        private float RelicTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;

        public GameActorFreezeEffect FreezeModifierEffect;
        private float FreezeAmountPerDamage;
    }
      
 }     