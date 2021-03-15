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
    public class Ntouch : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "N'kuhana's Touch";
            string resourceName = "ClassLibrary1/Resources/Mini_Mushrum"; ;
            GameObject gameObject = new GameObject();
            Ntouch Ntouch = gameObject.AddComponent<Ntouch>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "What is life whithout death?";
            string longDesc = "Chance to leave huge pool of poison on hit, poison immunity \n" +" Her Concepts, bestowed unto you. Poison to your surroundings.";
            Ntouch.SetupItem(shortDesc, longDesc, "ror");
            Ntouch.quality = PickupObject.ItemQuality.B;
            Ntouch.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            Ntouch.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += this.OnDealtDamage;
            this.m_PoisonImmunity = new DamageTypeModifier();
            this.m_PoisonImmunity.damageMultiplier = 0f;
            this.m_PoisonImmunity.damageType = CoreDamageTypes.Poison;
            player.healthHaver.damageTypeModifiers.Add(this.m_PoisonImmunity);
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamageContext -= this.OnDealtDamage;
            this.m_PoisonImmunity.damageMultiplier -= 0f;
            this.m_PoisonImmunity.damageType -= CoreDamageTypes.Poison;
            player.healthHaver.damageTypeModifiers.Remove(this.m_PoisonImmunity);
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnDealtDamageContext -= this.OnDealtDamage;
                this.m_PoisonImmunity.damageMultiplier -= 0f;
                this.m_PoisonImmunity.damageType -= CoreDamageTypes.Poison;
                player.healthHaver.damageTypeModifiers.Remove(this.m_PoisonImmunity);
                DebrisObject result = base.Drop(player);
            }
            base.OnDestroy();
        }


        private DamageTypeModifier m_PoisonImmunity;
        private static bool onCooldown;

        private void Shock(PlayerController user)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(base.Owner.sprite.WorldCenter, 18f, 0.1f, false);
        }
        private void OnDealtDamage(PlayerController player, float amount, bool fatal, HealthHaver target)
        {
            if (!onCooldown)
            {
                float value = UnityEngine.Random.value;
                bool flag = value < 0.015f;
                if (flag)
                {
                    this.Shock(player);
                }
            }
            GameManager.Instance.StartCoroutine(StartCooldown());
        }
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(1.3f);
            Ntouch.onCooldown = false;
            yield break;
        }
    }
}
