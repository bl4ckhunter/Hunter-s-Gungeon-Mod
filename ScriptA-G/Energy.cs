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
    public class NRG : PassiveItem
    {   
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "NRG";
            string resourceName = "ClassLibrary1/Resources/Energy_Drink"; 
            GameObject gameObject = new GameObject();
            NRG NRG = gameObject.AddComponent<NRG>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Dilated perception";
            string longDesc = "Active items slow down time \n" + "Part of a batch of energy drinks, recalled instantly after a disastrous launch party, they were dumped in the gungeon to hide proof that they ever existed. \n" + 
                "Goopton says the cocktail contains just about every drug in existence in just the right amount to not be instantly lethal.\n";
            NRG.SetupItem(shortDesc, longDesc, "ror");
            ItemBuilder.AddPassiveStatModifier(NRG, PlayerStats.StatType.MovementSpeed, 1.25f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            NRG.quality = PickupObject.ItemQuality.B;
            NRG.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            NRG.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
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
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(10f);
            NRG.onCooldown = false;
            yield break;
        }

        private void OnDealtDamage(PlayerController usingPlayer, PlayerItem usedItem)
        {
            IEnumerator routine = HandleDuration(usingPlayer);
            GameManager.Instance.StartCoroutine(routine);
        }

        private IEnumerator HandleDuration(PlayerController user)
        {
            bool flag = NRG.onCooldown;
            if (!flag)

            {
                test.RadialSlowHoldTime = 4f;
                test.RadialSlowOutTime = 2f;
                test.RadialSlowTimeModifier = 0.25f;
                test.DoesCirclePass = true;
                AkSoundEngine.PostEvent("State_Bullet_Time_on", this.gameObject);
                this.test.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
                NRG.onCooldown = true;
                IEnumerator routine3 = StartCooldown();
                GameManager.Instance.StartCoroutine(routine3);
            }

            yield break;
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