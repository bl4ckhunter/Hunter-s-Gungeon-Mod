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
        public class Headstomper : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Headstompers";
                string resourceName = "ClassLibrary1/Resources/headstompers"; ;
                GameObject gameObject = new GameObject();
                Headstomper headstomper = gameObject.AddComponent<Headstomper>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Graviton kick!";
                string longDesc = "Dodgeroll damage up and blank effect on dodgeroll damage \n " + "Ankle braces originally designed to allow low gravity planet dwellers to safely live on heavier planets,\n" + 
                "these ones were modified by a survivor of a spaceship crash them to enhance vertical kinetic energy, allowing him to literally stomp hostile lifeforms into the ground; \n" +
                "Of limited usefulness in the gungeon due to the low cielings an enterprising gungeoneer realigned the gravitic enhancer for horizontal energy delivery."; 
            ItemBuilder.AddPassiveStatModifier(headstomper, PlayerStats.StatType.DodgeRollDamage, 15, StatModifier.ModifyMethod.MULTIPLICATIVE);

            headstomper.SetupItem( shortDesc, longDesc, "ror");
                headstomper.quality = PickupObject.ItemQuality.B;
            headstomper.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnRolledIntoEnemy += this.Blank;
        }

        public override DebrisObject Drop(PlayerController player)
        {
            player.OnRolledIntoEnemy -= this.Blank;
            DebrisObject result = base.Drop(player);
            return result;
        }
        private void Blank(PlayerController player, AIActor enemy)
        {
            player.ForceBlank(6f, 0.5f, false, true, null, false, -1f);
        }
    }
}

