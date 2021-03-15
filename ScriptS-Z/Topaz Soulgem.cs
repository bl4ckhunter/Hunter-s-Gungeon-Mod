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
        public class SoulGem : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Topaz Soulgem";
                string resourceName = "ClassLibrary1/Resources/Gem"; ;
                GameObject gameObject = new GameObject();
                SoulGem SoulGem = gameObject.AddComponent<SoulGem>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Crystallized Gundead Souls";
                string longDesc = "Killing enemies generates ephemereal guon stones \n" + "Rumor has it that necromancers form the glass kingdom could mold the souls of the dead into guon stones, that art has long since been lost but this artifact remains";
                SoulGem.SetupItem( shortDesc, longDesc, "ror");
                SoulGem.quality = PickupObject.ItemQuality.C;
            SoulGem.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
            SoulGem.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            SoulGem.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
            ItemBuilder.AddPassiveStatModifier(SoulGem, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
        }
            public override void Pickup(PlayerController player)
        {
            player.OnNewFloorLoaded += this.Free;
            player.OnKilledEnemy += this.Roll;
            base.Pickup(player);
        }
            public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnKilledEnemy -= this.Roll;
            player.OnNewFloorLoaded -= this.Free;
            return result;
        }

        private void Roll(PlayerController player)
        {   if (UnityEngine.Random.value < 0.3f)
            {
                PickupObject pickupObject = Gungeon.Game.Items["ror:topaz_guon_soulstone"];
                player.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
            }
        }
        private void Free(PlayerController player)
        {

                PickupObject pickupObject = Gungeon.Game.Items["ror:topaz_guon_soulstone"];
                player.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);

        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnKilledEnemy -= this.Roll;
                player.OnNewFloorLoaded -= this.Free;
            }
            base.OnDestroy();
        }
    }
       
   

}
