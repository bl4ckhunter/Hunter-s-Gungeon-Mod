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
    public class Laser : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Laser Cell";
            string resourceName = "ClassLibrary1/Resources/Fuel_Cell"; ;
            GameObject gameObject = new GameObject();
            Laser Laser = gameObject.AddComponent<Laser>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "EXTERMINATE!";
            string longDesc = "The energy core of a dueling laser, without the shielding once fully charged it discharges immediately though in no particular direction.";
            Laser.SetupItem(shortDesc, longDesc, "ror");
            Laser.quality = PickupObject.ItemQuality.C;
            Laser.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {   
            this.charge += damage;
            bool flag = this.charge > 250f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            if (flag)
            {
                this.charge = 0f;
                this.cooldown = true;
                base.StartCoroutine(this.HandleShield(base.Owner));
                base.StartCoroutine(this.Cooldown(base.Owner));
            }
        }
        private IEnumerator HandleShield(PlayerController user)
        {   
            user.healthHaver.IsVulnerable = false;
            this.LaserSalvo();
            yield return new WaitForSeconds(0.1f);
          user.healthHaver.IsVulnerable = true;
            yield break;
        }
        private IEnumerator Cooldown(PlayerController user)
        {
            yield return new WaitForSeconds(5f);
            this.cooldown = false;
            yield break;
        }



        // Token: 0x060001C9 RID: 457 RVA: 0x0000F6CF File Offset: 0x0000D8CF
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
        }

        // Token: 0x060001CA RID: 458 RVA: 0x0000F6FC File Offset: 0x0000D8FC
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            }
            base.OnDestroy();
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
                this.LaserTrue = this.LaserBase * this.Damage;
                this.lastDamage = this.Damage;
            }
        }

        private void LaserSalvo()
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[508]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 0f), true);
            GameObject gameObject0 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 45f), true);
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 90f), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 135f), true);
            GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 180f), true);
            GameObject gameObject5 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 225f), true);
            GameObject gameObject6 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 270f), true); 
            GameObject gameObject7 = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 315f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            Projectile component4 = gameObject4.GetComponent<Projectile>();
            Projectile component5 = gameObject5.GetComponent<Projectile>();
            Projectile component6 = gameObject6.GetComponent<Projectile>();
            Projectile component7 = gameObject7.GetComponent<Projectile>();
            Projectile component2 = gameObject3.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = base.Owner;
                component.Shooter = base.Owner.specRigidbody;
                component.baseData.speed = 50f;
                component.baseData.damage = this.LaserTrue;
            }
            Projectile component0 = gameObject0.GetComponent<Projectile>();
            bool flag0 = component0 != null;
            if (flag0)
            {
                component0.Owner = base.Owner;
                component0.Shooter = base.Owner.specRigidbody;
                component0.baseData.speed = 50f;
                component0.baseData.damage = this.LaserTrue;
            }
            Projectile component1 = gameObject2.GetComponent<Projectile>();
            bool flag1 = component1 != null;
            if (flag1)
            {
                component1.Owner = base.Owner;
                component1.Shooter = base.Owner.specRigidbody;
                component1.baseData.speed = 50f;
                component1.baseData.damage = this.LaserTrue;
            }
            bool flag2 = component != null;
            if (flag2)
            {
                component2.Owner = base.Owner;
                component2.Shooter = base.Owner.specRigidbody;
                component2.baseData.speed = 50f;
                component2.baseData.damage = this.LaserTrue;
            }
            bool flag3 = component != null;
            if (flag3)
            {
                component3.Owner = base.Owner;
                component3.Shooter = base.Owner.specRigidbody;
                component3.baseData.speed = 50f;
                component3.baseData.damage = this.LaserTrue;
            }
            bool flag4 = component != null;
            if (flag4)
            {
                component4.Owner = base.Owner;
                component4.Shooter = base.Owner.specRigidbody;
                component4.baseData.speed = 50f;
                component4.baseData.damage = this.LaserTrue;
            }
            bool flag5 = component != null;
            if (flag5)
            {
                component5.Owner = base.Owner;
                component5.Shooter = base.Owner.specRigidbody;
                component5.baseData.speed = 50f;
                component5.baseData.damage = this.LaserTrue;
            }
            bool flag6 = component != null;
            if (flag6)
            {
                component6.Owner = base.Owner;
                component6.Shooter = base.Owner.specRigidbody;
                component6.baseData.speed = 50f;
                component6.baseData.damage = this.LaserTrue;
            }
            bool flag7 = component != null;
            if (flag7)
            {
                component7.Owner = base.Owner;
                component7.Shooter = base.Owner.specRigidbody;
                component7.baseData.speed = 50f;
                component7.baseData.damage = this.LaserTrue;
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
        private float LaserBase = 40f;

        // Token: 0x0400001D RID: 29
        private float LaserTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;
        private float charge;
        private bool cooldown;
    }
      
 }     