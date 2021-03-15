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
        public class Bandolier : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Bandolier";
                string resourceName = "ClassLibrary1/Resources/Bandolier"; ;
                GameObject gameObject = new GameObject();
                Bandolier bandolier = gameObject.AddComponent<Bandolier>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Killing for ammo";
                string longDesc = "3% chance to spread ammo on kill \n" + "The bandolier of a long lost explorer, it's full of bullets but the gunpowder is long gone. \n" + "Thankfully the gundead have more than enough to spare";
                bandolier.SetupItem( shortDesc, longDesc, "ror");
                bandolier.quality = PickupObject.ItemQuality.D;
            bandolier.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            player.OnKilledEnemy += this.Roll;
            base.Pickup(player);
        }
            public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnKilledEnemy -= this.Roll;
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                DebrisObject result = base.Drop(player);
                player.OnKilledEnemy -= this.Roll;
            }
            base.OnDestroy();
        }
        private void Roll(PlayerController player)
        {
            float value = UnityEngine.Random.value;
            bool flag = value < 0.03f;
            if (flag)
            {
                this.Gainammo(player);
            }
        }
        private void Gainammo(PlayerController player)
        { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(600).gameObject, player); }

    }

}
