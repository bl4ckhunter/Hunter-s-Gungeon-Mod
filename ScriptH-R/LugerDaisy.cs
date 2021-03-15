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
        public class Daisy : PassiveItem
        {
        private bool chargeboost;
        private float charge;
        private bool onCooldown;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
            {
                string name = "Gungeon Daisy";
                string resourceName = "ClassLibrary1/Resources/daisy"; ;
                GameObject gameObject = new GameObject();
                Daisy Daisy = gameObject.AddComponent<Daisy>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Flower power";
                string longDesc = "Deal damage to charge, when charged spawns 2 gungeon blooms orbiting around the player for 20s\n" + "Putting flowers in your cannons takes an entirely different meaning when it's gungeon flora we're talking about" ;
                Daisy.SetupItem( shortDesc, longDesc, "ror");
                Daisy.quality = PickupObject.ItemQuality.B;
            Daisy.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            Daisy.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
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



        private void Roll(PlayerController player)
        {   
            PickupObject pickupObject = Gungeon.Game.Items["ror:gungeon_bloom"];
            player.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
            player.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);



        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
            this.charge += damage;
            bool flag = this.charge > 450f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
            if (flag && !this.onCooldown)
            {
                this.onCooldown = true;
                this.charge = 0f;
                GameManager.Instance.StartCoroutine(StartCooldown());
                this.Roll(base.Owner);
            }
        }
        private IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(20f);
            this.onCooldown = false;
            yield break;
        }
    }
       
   

}
