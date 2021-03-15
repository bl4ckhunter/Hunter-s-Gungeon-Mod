using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;

namespace Mod
{
        //Call this method from the Start() method of your ETGModule extension
        public class Bait : PassiveItem
        {
        private bool chargeboost;
        private float charge;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
            {
                string name = "Sharktiger Tooth";
                string resourceName = "ClassLibrary1/Resources/tooth"; ;
                GameObject gameObject = new GameObject();
                Bait Bait = gameObject.AddComponent<Bait>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Feeding frenzy";
                string longDesc = "Spawns a ring of bait, deal damage to recharge. \n" + "A necklace made with the teeth of a Sharktiger, the legendary missing link between tiger and shark, as much as it sounds like an hoax you've seen far weirder things in the gungeon." ;
                Bait.SetupItem( shortDesc, longDesc, "ror");
                Bait.quality = PickupObject.ItemQuality.C;
            Bait.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            Bait.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
        }
            public override void Pickup(PlayerController player)
        {   
            if (!this.chargeboost)
            {
                this.Roll(player);
                this.chargeboost = true;
            }
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            base.Pickup(player);
        }
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


        private void Roll(PlayerController player)
        {   
            PickupObject pickupObject = Gungeon.Game.Items["ror:fish_bait"];
            PickupObject pickupObject1 = Gungeon.Game.Items["ror:meat_bait"];
            player.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(pickupObject1 as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(pickupObject1 as PassiveItem);


        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            this.charge += damage;
            bool flag = this.charge > 550 * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            if (flag)
            {
                this.charge = 0f;
                
                this.Roll(base.Owner);
            }
        }

    }
       
   

}
