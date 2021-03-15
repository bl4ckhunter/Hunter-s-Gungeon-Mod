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
    public class Will : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Will the Wisp";
            string resourceName = "ClassLibrary1/Resources/Will"; ;
            GameObject gameObject = new GameObject();
            Will will = gameObject.AddComponent<Will>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Hello little guy!";
            string longDesc = "Chance to leave huge pool of fire on hit, fire immunity \n" + "A tiny lifeform from a distant planet,it seems fine sitting in its jar. \n" + "Seeing it cheer when you land an hit sets your heart on fire";
            will.SetupItem(shortDesc, longDesc, "ror");
            will.quality = PickupObject.ItemQuality.B;
            will.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            will.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += this.OnDealtDamage;
            this.m_FireImmunity = new DamageTypeModifier();
            this.m_FireImmunity.damageMultiplier = 0f;
            this.m_FireImmunity.damageType = CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Add(this.m_FireImmunity);
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamageContext -= this.OnDealtDamage;
            this.m_FireImmunity.damageMultiplier -= 0f;
            this.m_FireImmunity.damageType -= CoreDamageTypes.Fire;
            player.healthHaver.damageTypeModifiers.Remove(this.m_FireImmunity);
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnDealtDamageContext -= this.OnDealtDamage;
                this.m_FireImmunity.damageMultiplier -= 0f;
                this.m_FireImmunity.damageType -= CoreDamageTypes.Fire;
                player.healthHaver.damageTypeModifiers.Remove(this.m_FireImmunity);
                DebrisObject result = base.Drop(player);
            }
            base.OnDestroy();
        }



        private DamageTypeModifier m_FireImmunity;
        private static bool onCooldown;

        private IEnumerator Shock(PlayerController user)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(base.Owner.sprite.WorldCenter, 18f, 0.8f, false);
            yield return new WaitForSeconds(1f);
            DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(base.Owner.sprite.WorldCenter, 25f);
            yield break;
        }
        private void OnDealtDamage(PlayerController player, float amount, bool fatal, HealthHaver target)
        {
            if (!onCooldown)
            {
                float value = UnityEngine.Random.value;
                bool flag = value < 0.015f;
                if (flag)
                {
                    GameManager.Instance.StartCoroutine(Shock(player));
                }
            }
            GameManager.Instance.StartCoroutine(StartCooldown());
        }
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1.3f);
            Will.onCooldown = false;
            yield break;
        }
    }

}
