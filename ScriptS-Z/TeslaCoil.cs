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
    public class Coil : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Tesla Coil";
            string resourceName = "ClassLibrary1/Resources/Coil"; ;
            GameObject gameObject = new GameObject();
            Coil Coil = gameObject.AddComponent<Coil>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Shocking Experience";
            string longDesc = "This powerful weapon gathers static electricity from your surroundings to charge, once fully charged it zaps enemies nearby.";
            Coil.SetupItem(shortDesc, longDesc, "ror");
            Coil.quality = PickupObject.ItemQuality.B;
            Coil.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            Coil.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);

        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {   
            this.charge += damage;
            bool flag = this.charge > 250f;
            if (flag && !this.Active)
            {
                this.charge = 0f;
                this.ShockRing();
                this.Active = true;
            }
        }
        private void ShockRing() {
            this.m_radialIndicatorActive = true;
            this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), base.Owner.CenterPosition, Quaternion.identity, base.transform)).GetComponent<HeatIndicatorController>();
            this.m_radialIndicator.CurrentColor = Color.blue;
            this.m_radialIndicator.IsFire = true;
            this.m_radialIndicator.CurrentRadius = 6.5f;

        }



        // Token: 0x060001C9 RID: 457 RVA: 0x0000F6CF File Offset: 0x0000D8CF
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            player.OnRoomClearEvent += this.Stop;
            player.OnEnteredCombat += this.Stop1;
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
            if (this.Active)
            {
                List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                Vector2 position = base.Owner.CenterPosition;
                foreach (AIActor ai in activeEnemies)

                { if (Vector2.Distance(ai.CenterPosition, position) < 6.5f && !this.onCooldown && ai.healthHaver.GetMaxHealth() > 0f )

                    {
                        this.Zap(ai);
                        this.onCooldown = true;
                    }
                        
                            
                }
            }
        }

        private void Stop(PlayerController pla)
        {
            this.Active = false;
            if(this.charge >= 250f) 
            { this.charge = 100f; }
            if (this.m_radialIndicatorActive)
            {
                this.m_radialIndicatorActive = false;
                if (this.m_radialIndicator)
                {
                    this.m_radialIndicator.EndEffect();
                }
                this.m_radialIndicator = null;
            }
        }
        private void Stop1()
        {
            this.Active = false;
            if (this.charge >= 250f)
            { this.charge = 100f; }
            if (this.m_radialIndicatorActive)
            {
                this.m_radialIndicatorActive = false;
                if (this.m_radialIndicator)
                {
                    this.m_radialIndicator.EndEffect();
                }
                this.m_radialIndicator = null;
            }
        }
        private void Zap( AIActor ai)
        {
            GameManager.Instance.StartCoroutine(this.StartCooldown());
            Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[748]).DefaultModule.projectiles[0];
            projectile2.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, ai.CenterPosition, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            bool flag4 = component != null;
            if (flag4)
            {
                component.baseData.damage = 7f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
                component.SetOwnerSafe(base.Owner, "Player");
                component.Shooter = base.Owner.specRigidbody;
                component.AdjustPlayerProjectileTint(Color.blue, 1, 0f);
                component.baseData.force = 0f;
            }
        }
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(0.2f);
            this.onCooldown = false;
            yield break;
        }





        private float charge;
        private bool m_radialIndicatorActive;
        private HeatIndicatorController m_radialIndicator;
        private bool Active;
        private bool onCooldown;
    }
      
 }     