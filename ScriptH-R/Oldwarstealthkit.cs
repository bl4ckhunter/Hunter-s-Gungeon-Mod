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
        public class OWStealthkit : PassiveItem
        {
            // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
            public static void Init()
            {
                string name = "Stealthkit";
                string resourceName = "ClassLibrary1/Resources/Stealthkit"; ;
                GameObject gameObject = new GameObject();
                OWStealthkit stealthkit = gameObject.AddComponent<OWStealthkit>();
                ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
                string shortDesc = "Just give it a good whack";
                string longDesc = "A relic of the Old War, this stealth kit prototype granted near invisibility at the press of a button, until someone broke it that is. \n" +
                "Still worth keeping around aroud though, maybe if you get hit it'll turn back on.";
                stealthkit.SetupItem( shortDesc, longDesc, "ror");
                stealthkit.quality = PickupObject.ItemQuality.C;
            stealthkit.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            stealthkit.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
        }
            public override void Pickup(PlayerController player)
        {
            player.OnReceivedDamage += this.StealthEffect;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnReceivedDamage -= this.StealthEffect;
            DebrisObject result = base.Drop(player);
            return result;
        }

        private void StealthEffect(PlayerController player)
        {
            PlayerController owner = base.Owner;
            this.BreakStealth(owner);
            owner.OnItemStolen += this.BreakStealthOnSteal;
            owner.ChangeSpecialShaderFlag(1, 1f);
            owner.healthHaver.OnDamaged += new HealthHaver.OnDamagedEvent(this.OnDamaged);
            owner.SetIsStealthed(true, "table");
            owner.SetCapableOfStealing(true, "table", null);
            GameManager.Instance.StartCoroutine(this.Unstealthy());
        }
        private IEnumerator Unstealthy()
        {
            PlayerController player = base.Owner;
            yield return new WaitForSeconds(0.15f);
            player.OnDidUnstealthyAction += this.BreakStealth;
            yield break;
        }
        private void BreakStealth(PlayerController player)
        {
            player.ChangeSpecialShaderFlag(1, 0f);
            player.OnItemStolen -= this.BreakStealthOnSteal;
            player.SetIsStealthed(false, "table");
            player.healthHaver.OnDamaged -= new HealthHaver.OnDamagedEvent(this.OnDamaged);
            player.SetCapableOfStealing(false, "table", null);
            player.OnDidUnstealthyAction -= this.BreakStealth;
            AkSoundEngine.PostEvent("Play_ENM_wizardred_appear_01", base.gameObject);
        }
        private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
        {
            PlayerController owner = base.Owner;
            this.BreakStealth(owner);
        }
        private void BreakStealthOnSteal(PlayerController arg1, ShopItemController arg2)
        {
            this.BreakStealth(arg1);
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnReceivedDamage -= this.StealthEffect;
            }
            base.OnDestroy();
        }
    }
    }