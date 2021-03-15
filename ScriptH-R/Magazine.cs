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
        public class Magazine : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Taped Magazines";
                string resourceName = "ClassLibrary1/Resources/Backup_Magazine"; ;
                GameObject gameObject = new GameObject();
                Magazine Magazine = gameObject.AddComponent<Magazine>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Killing for blood, Reloading for tape";
                string longDesc = "Someone stuck a bunch of magazines toghether with duct tape, \n" + "there's a small chance you might be able to reuse that if you kill something before reloading.";
                Magazine.SetupItem( shortDesc, longDesc, "ror");
            ItemBuilder.AddPassiveStatModifier(Magazine, PlayerStats.StatType.AdditionalClipCapacityMultiplier, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(Magazine, PlayerStats.StatType.ReloadSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(Magazine, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
            Magazine.tape += 0f;
            Magazine.quality = PickupObject.ItemQuality.A;



            Magazine.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            player.OnReloadedGun += this.Roll;
            player.OnKilledEnemy += this.Flag;

            base.Pickup(player);
        }
            public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnReloadedGun -= this.Roll;
            player.OnKilledEnemy -= this.Flag;
            return result;
        }

        private void Roll(PlayerController player, Gun gun)
        {
            float value = UnityEngine.Random.value;
            bool flag = value < 0.35f;
            if (flag)
            {
                if (this.tape < 2f)

                {
                    bool flag1 = this.kill;
                    if (flag1)
                    {
                        this.Gainammo(player);
                        this.kill = false;
                        this.tape += 1f;
                    }
                }
            }
        }

        private void Flag(PlayerController player) { this.kill = true; }
        private void Gainammo(PlayerController player)
        { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(239).gameObject, player); }
        private bool kill;
        private float tape;
    } 


}
