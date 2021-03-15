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
        public class FrailCrown : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Frail Crown";
                string resourceName = "ClassLibrary1/Resources/BrittleCrown"; ;
                GameObject gameObject = new GameObject();
                FrailCrown frailCrown = gameObject.AddComponent<FrailCrown>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Rise and fall";
                string longDesc = "Casings per kill, get hit and lose it all \n" + "A crown found on an enigmatic planet, great success and terrible failures seem to befall whomever holds it in rapid succession. \n" + 
                "You cannot bear to give it up, oh well, nothing ventured nothing gained.";
                frailCrown.SetupItem( shortDesc, longDesc, "ror");
                frailCrown.quality = PickupObject.ItemQuality.C;
            frailCrown.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            player.OnKilledEnemy += this.Gainmoney;
            player.OnReceivedDamage += this.Losemoney;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnKilledEnemy += this.Gainmoney;
            player.OnReceivedDamage += this.Losemoney;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnKilledEnemy += this.Gainmoney;
                player.OnReceivedDamage += this.Losemoney;
            }
            base.OnDestroy();
        }
        private void Gainmoney(PlayerController player)
        {
            PlayerController owner = base.Owner;
            owner.carriedConsumables.Currency += 10;
        }
        private void Losemoney(PlayerController player)
        {
            player.carriedConsumables.Currency -= 9999;
        } 

    }
}
