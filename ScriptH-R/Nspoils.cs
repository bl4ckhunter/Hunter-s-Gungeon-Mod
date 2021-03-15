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
    public class Nspoils : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "N'Kuhana's Spoils";
            string resourceName = "ClassLibrary1/Resources/N'kuhana's_Opinion"; ;
            GameObject gameObject = new GameObject();
            Nspoils Nspoils = gameObject.AddComponent<Nspoils>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "She sees worth in your life still";
            string longDesc = "A small share of what was gifted to Her, returned onto you.";
            Nspoils.SetupItem(shortDesc, longDesc, "ror");
            Nspoils.quality = PickupObject.ItemQuality.EXCLUDED;

        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += this.OnDealtDamage;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamageContext -= this.OnDealtDamage;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnDealtDamageContext -= this.OnDealtDamage;
            }
            base.OnDestroy();
        }
        private void Burst(PlayerController usingPlayer)
        {
            bool flag21 = !Nspoils.onCooldown1;
            if (flag21)
            {
                Nspoils.onCooldown1 = true;
                GameManager.Instance.StartCoroutine(Nspoils.StartCooldown1());
                this.NspoilsSalvo(usingPlayer); 
            }
        }
        private void OnDealtDamage(PlayerController usingPlayer, float amount, bool fatal, HealthHaver targetr)
        {
            
                {
                    bool flag2 = !Nspoils.onCooldown;
                    if (flag2)

                {
                    Nspoils.onCooldown = true;
                    GameManager.Instance.StartCoroutine(Nspoils.StartCooldown());
                        float value = UnityEngine.Random.value;
                        bool flag = value < 0.30f;
                        if (flag)
                        {
                            this.Burst(usingPlayer);
                        }
                    }
                }

        }
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1f);
            Nspoils.onCooldown = false;
            yield break;
        }
        private static IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(0.6f);
            Nspoils.onCooldown1 = false;
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
                this.NspoilsTrue = this.NspoilsBase * this.Damage;
                this.lastDamage = this.Damage;
            }
        }

        private void NspoilsSalvo(PlayerController player)
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[704]).DefaultModule.projectiles[0];
            projectile.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 0f), true);
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 90f), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 180f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component2 = gameObject2.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = base.Owner;
                HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
                BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
                component.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                piercer.penetration = 7;
                bouncer.numberOfBounces = 5;
                bouncer.onlyBounceOffTiles = true;
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 500f;
                component.Shooter = base.Owner.specRigidbody;
                component.baseData.speed = 3f;
                component.baseData.damage = this.NspoilsTrue;
            }
            bool flag2 = component2 != null;
            if (flag2)
            {
                component2.Owner = base.Owner;
                HomingModifier homingModifier2 = component2.gameObject.AddComponent<HomingModifier>();
                BounceProjModifier bouncer = component2.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component2.gameObject.AddComponent<PierceProjModifier>();
                component2.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                piercer.penetration = 7;
                bouncer.numberOfBounces = 5;
                bouncer.onlyBounceOffTiles = true;
                homingModifier2.HomingRadius = 100f;
                homingModifier2.AngularVelocity = 500f;
                component2.Shooter = base.Owner.specRigidbody;
                component2.baseData.speed = 3f;
                component2.baseData.damage = this.NspoilsTrue;
            }
            bool flag3 = component3 != null;
            if (flag3)
            {
                component3.Owner = base.Owner;
                HomingModifier homingModifier3 = component3.gameObject.AddComponent<HomingModifier>();
                component3.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                BounceProjModifier bouncer = component3.gameObject.AddComponent<BounceProjModifier>();
                PierceProjModifier piercer = component3.gameObject.AddComponent<PierceProjModifier>();
                piercer.penetration = 7;
                bouncer.numberOfBounces = 5;
                bouncer.onlyBounceOffTiles = true;
                homingModifier3.HomingRadius = 100f;
                homingModifier3.AngularVelocity = 500f;
                component3.Shooter = base.Owner.specRigidbody;
                component3.baseData.speed = 3f;
                component3.baseData.damage = this.NspoilsTrue;
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
        private float NspoilsBase = 0.75f;

        // Token: 0x0400001D RID: 29
        private float NspoilsTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;

    }
      
 }     