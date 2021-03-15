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
    public class Dml : PassiveItem
    {   
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Aftermarket Missile Launcher";
            string resourceName = "ClassLibrary1/Resources/Disposable_Missile_Launcher"; 
            GameObject gameObject = new GameObject();
            Dml dml = gameObject.AddComponent<Dml>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Macross Missile Massacre";
            string longDesc = "Active item fires missile salvo \n" + "Who in their right mind would make a missile launcher out of cardboard and what madman would reload it with over 60 missiles \n " +
                "and rig the activation mechanism to a ring of triggers? \n" + "Originally designed as a one time use device orcish engineering has turned it into the tool of mass destruction that it is now,\n" + 
                " you'd still never trust it not to kill you on the first use if you didn't know Trorc personally.";
            dml.SetupItem(shortDesc, longDesc, "ror");
            dml.quality = PickupObject.ItemQuality.A;
            dml.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            player.OnUsedPlayerItem += this.OnDealtDamage;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedPlayerItem -= this.OnDealtDamage;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnUsedPlayerItem -= this.OnDealtDamage;
            }
            base.OnDestroy();
        }
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(10f);
            Dml.onCooldown = false;
            yield break;
        }

        private void OnDealtDamage(PlayerController usingPlayer, PlayerItem usedItem)
        {
            IEnumerator routine = Blast(usingPlayer, usedItem);
            GameManager.Instance.StartCoroutine(routine);
        }

        private IEnumerator Blast(PlayerController usingPlayer, PlayerItem usedItem)
        {
            bool flag2 = !Dml.onCooldown;
            if (flag2)
            {
                Dml.onCooldown = true;
                GameManager.Instance.StartCoroutine(Dml.StartCooldown());
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                 yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield return new WaitForSeconds(0.15f);
                this.AtgSalvo(usingPlayer);
                yield break;
            }
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
                this.DmlTrue = this.DmlBase * this.Damage;
                this.lastDamage = this.Damage;
            }
        }

        private void AtgSalvo(PlayerController player)
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[275]).DefaultModule.projectiles[0];
            projectile.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.yellow.a / 2f), 5, 0f);
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
                component.AdjustPlayerProjectileTint(Color.yellow.WithAlpha(Color.yellow.a / 2f), 5, 0f);
                homingModifier.HomingRadius = 100f;
                homingModifier.AngularVelocity = 500f;
                component.Shooter = base.Owner.specRigidbody;
                component.baseData.speed = 20f;
                component.baseData.damage = this.DmlTrue;
                component.SetOwnerSafe(base.Owner, "Player");
            }
            bool flag2 = component2 != null;
            if (flag2)
            {
                component2.Owner = base.Owner;
                HomingModifier homingModifier2 = component2.gameObject.AddComponent<HomingModifier>();
                component2.AdjustPlayerProjectileTint(Color.yellow.WithAlpha(Color.yellow.a / 2f), 5, 0f);
                homingModifier2.HomingRadius = 100f;
                homingModifier2.AngularVelocity = 500f;
                component2.Shooter = base.Owner.specRigidbody;
                component2.baseData.speed = 20f;
                component2.baseData.damage = this.DmlTrue;
                component2.SetOwnerSafe(base.Owner, "Player");
            }
            bool flag3 = component3 != null;
            if (flag3)
            {
                component3.Owner = base.Owner;
                HomingModifier homingModifier3 = component3.gameObject.AddComponent<HomingModifier>();
                component3.AdjustPlayerProjectileTint(Color.yellow.WithAlpha(Color.yellow.a / 2f), 5, 0f);
                homingModifier3.HomingRadius = 100f;
                homingModifier3.AngularVelocity = 500f;
                component3.Shooter = base.Owner.specRigidbody;
                component3.baseData.speed = 20f;
                component3.baseData.damage = this.DmlTrue;
                component3.SetOwnerSafe(base.Owner, "Player");
            }
        }
        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;

        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private float DmlBase = 10f;

        // Token: 0x0400001D RID: 29
        private float DmlTrue;
        private static bool onCooldown;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;

    }
      
 }     