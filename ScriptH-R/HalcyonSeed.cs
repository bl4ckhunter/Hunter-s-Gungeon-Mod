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
        public class HSeed : PassiveItem
        {
        private float Armor;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
            {
                string name = "Gun Knight's Spoils";
                string resourceName = "ClassLibrary1/Resources/SpectralArmor"; ;
                GameObject gameObject = new GameObject();
                HSeed seed = gameObject.AddComponent<HSeed>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "At Long Last";
                string longDesc = "A throphy belonging to a gungeoneer, who after decades trying to acquire all the gunknight pieces and failing miserably went to the past and killed Cormorant.\n ";
                seed.SetupItem( shortDesc, longDesc, "ror");
                seed.quality = PickupObject.ItemQuality.S;
            ItemBuilder.AddPassiveStatModifier(seed, PlayerStats.StatType.DodgeRollSpeedMultiplier, 0.8f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(seed, PlayerStats.StatType.DodgeRollDistanceMultiplier, 1.1f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            seed.CanBeDropped = false;
                seed.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            float maxHealth = player.healthHaver.GetMaxHealth();
            float num = maxHealth - 1f;
            player.stats.RecalculateStats(player, false, false);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(160).gameObject, player);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(161).gameObject, player);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(162).gameObject, player);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(163).gameObject, player);
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void Update()
        {
            base.Update();
            if (base.Owner)
            {
                
                float maxHealth = Owner.healthHaver.GetMaxHealth();
                float num = maxHealth - 2f;
                StatModifier item = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.Health,
                    amount = -num,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE
                };
                Owner.ownerlessStatModifiers.Add(item);
                Owner.stats.RecalculateStats(base.Owner, false, false);
                if (Owner.healthHaver.Armor >= 8f)
                { Owner.healthHaver.Armor = 8f; }
            } }
    }   
}


