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
    public class TarotDeck : PassiveItem
    {   
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Tarot Deck";
            string resourceName = "ClassLibrary1/Resources/tpack"; 
            GameObject gameObject = new GameObject();
            TarotDeck TarotDeck = gameObject.AddComponent<TarotDeck>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "A Few Cards Short";
            string longDesc = "35% chance of dropping a new random card every time a card is used, affected by coolness\n";
            TarotDeck.SetupItem(shortDesc, longDesc, "ror");
            TarotDeck.quality = PickupObject.ItemQuality.C;
            ItemBuilder.AddPassiveStatModifier(TarotDeck, PlayerStats.StatType.Coolness, 2, StatModifier.ModifyMethod.ADDITIVE);
            TarotDeck.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            TarotDeck.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            player.OnUsedPlayerItem += this.OnDealtDamage;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnUsedPlayerItem -= this.OnDealtDamage;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnUsedPlayerItem -= this.OnDealtDamage;
            }
            base.OnDestroy();
        }

        private void OnDealtDamage(PlayerController usingPlayer, PlayerItem usedItem)
        {
            foreach (PlayerItem item in RoRItems.cards)
            {
                if (item.PickupObjectId == usedItem.PickupObjectId)
                {
                    float f = usingPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness) / 50f;
                    float chance = 0.35f + f;
                    if (UnityEngine.Random.value < chance)
                    {
                        int c = RoRItems.cards.Count;
                        int i = UnityEngine.Random.Range(0, c);
                        LootEngine.SpawnItem(RoRItems.cards[i].gameObject, usingPlayer.CenterPosition, new Vector2(0, 0), 0f);
                    }
                }
            }
        }

        






        // Token: 0x0400001D RID: 29
        private static bool onCooldown;

        // Token: 0x04007BE4 RID: 31716
        public float timeScale;

        // Token: 0x04007BE5 RID: 31717
        public float duration;

        // Token: 0x04007BE6 RID: 31718
        public bool HasSynergy;

        // Token: 0x04007BE7 RID: 31719
        [LongNumericEnum]
        public CustomSynergyType RequiredSynergy;

        // Token: 0x04007BE8 RID: 31720
        public float overrideTimeScale;

        // Token: 0x04007BE9 RID: 31721
        public RadialSlowInterface test;

        public float RadialSlowTimeModifier;






        // Token: 0x04007BE7 RID: 31719



        // Token: 0x06000145 RID: 325 RVA: 0x0000CB24 File Offset: 0x0000AD24
        public float RadialSlowInTime;

        // Token: 0x040074E5 RID: 29925
        public float RadialSlowHoldTime;

        // Token: 0x040074E6 RID: 29926
        public float RadialSlowOutTime;
    } 
      
 }     