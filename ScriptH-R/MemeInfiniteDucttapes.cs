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
        public class Broken : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Broken Magazine";
                string resourceName = "ClassLibrary1/Resources/Backup_Magazine"; ;
                GameObject gameObject = new GameObject();
                Broken Broken = gameObject.AddComponent<Broken>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Infinite tape!!!!";
                string longDesc = "";
                Broken.SetupItem( shortDesc, longDesc, "ror");
            ItemBuilder.AddPassiveStatModifier(Broken, PlayerStats.StatType.AdditionalClipCapacityMultiplier, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(Broken, PlayerStats.StatType.ReloadSpeed, 0.75f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(Broken, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
            Broken.quality = PickupObject.ItemQuality.EXCLUDED;


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
            bool flag = value < 1f;
            if (flag)
            {
                    bool flag1 = this.kill;
                    if (flag1)
                    {
                        this.Gainammo(player);
                        this.kill = false;

                    
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
