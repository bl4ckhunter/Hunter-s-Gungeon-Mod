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
    public class Atg : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "GtG Missile MK2";
            string resourceName = "ClassLibrary1/Resources/AtG_Missile_Mk._1"; ;
            GameObject gameObject = new GameObject();
            Atg atg = gameObject.AddComponent<Atg>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Fire and forget!";
            string longDesc = "Chance to fire missiles on hit \n" + "A custom variant on a classic automatic air to ground missile system, the Gungeoneer to Gundead Missile MK2 sports a quick lock on and it's actually safe to use indoors!";
            atg.SetupItem(shortDesc, longDesc, "ror");
            atg.quality = PickupObject.ItemQuality.C;
            atg.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

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
            bool flag21 = !Atg.onCooldown1;
            if (flag21)
            {
                Atg.onCooldown1 = true;
                GameManager.Instance.StartCoroutine(Atg.StartCooldown1());
                this.AtgSalvo(usingPlayer);
            }
        }
        private void OnDealtDamage(PlayerController usingPlayer, float amount, bool fatal, HealthHaver targetr)
        {
            
                {
                    bool flag2 = !Atg.onCooldown;
                    if (flag2)

                {
                    Atg.onCooldown = true;
                    GameManager.Instance.StartCoroutine(Atg.StartCooldown());
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
            Atg.onCooldown = false;
            yield break;
        }
        private static IEnumerator StartCooldown1()
        {
            yield return new WaitForSeconds(1.5f);
            Atg.onCooldown1 = false;
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
                this.AtgTrue = this.AtgBase * this.Damage;
                this.lastDamage = this.Damage;
            }
        }

        private void AtgSalvo(PlayerController player)
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[275]).DefaultModule.projectiles[0];
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
                component.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 500f;
                component.Shooter = base.Owner.specRigidbody;
                component.baseData.speed = 20f;
                component.SetOwnerSafe(base.Owner, "Player");
                component.baseData.damage = this.AtgTrue;
            }
            bool flag2 = component2 != null;
            if (flag2)
            {
                component2.Owner = base.Owner;
                HomingModifier homingModifier2 = component2.gameObject.AddComponent<HomingModifier>();
                component2.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                homingModifier2.HomingRadius = 100f;
                homingModifier2.AngularVelocity = 500f;
                component2.Shooter = base.Owner.specRigidbody;
                component2.baseData.speed = 20f;
                component2.SetOwnerSafe(base.Owner, "Player");
                component2.baseData.damage = this.AtgTrue;
            }
            bool flag3 = component3 != null;
            if (flag3)
            {
                component3.Owner = base.Owner;
                HomingModifier homingModifier3 = component3.gameObject.AddComponent<HomingModifier>();
                component3.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.green.a / 2f), 5, 0f);
                homingModifier3.HomingRadius = 100f;
                homingModifier3.AngularVelocity = 500f;
                component3.Shooter = base.Owner.specRigidbody;
                component3.SetOwnerSafe(base.Owner, "Player");
                component3.baseData.speed = 20f;
                component3.baseData.damage = this.AtgTrue;
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
        private float AtgBase = 7f;

        // Token: 0x0400001D RID: 29
        private float AtgTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;

    }
      
 }     