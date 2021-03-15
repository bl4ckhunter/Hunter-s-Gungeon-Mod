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
    public class Ukulele : PassiveItem
    {
        private static bool onCooldown;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Ukulele";
            string resourceName = "ClassLibrary1/Resources/Ukulele"; ;
            GameObject gameObject = new GameObject();
            Ukulele ukulele = gameObject.AddComponent<Ukulele>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "And his music is still electric...";
            string longDesc = "Chance to leave a pool of water on hit \n" + "An artifact belonging to a legendary musician from mars, famous for being both skilled and a paragon of virtue, rumors have it he inherited the power of lightning from a god. \n"
                + "Someone dropped into a fairy fountain a long ago, though undamaged it's still damp to this day. \n" + "You found batteries with it, you might not have the power of lighting but can make do";
            ukulele.SetupItem(shortDesc, longDesc, "ror");
            ukulele.CanBeDropped = false;
            ItemBuilder.AddPassiveStatModifier(ukulele, PlayerStats.StatType.Coolness, 3, StatModifier.ModifyMethod.ADDITIVE);
            ukulele.quality = PickupObject.ItemQuality.A;
            ukulele.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            ukulele.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            player.OnDealtDamageContext += this.OnDealtDamage;
            base.Pickup(player);
            LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(410).gameObject, player);

        }
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnDealtDamageContext -= this.OnDealtDamage;
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnDealtDamageContext -= this.OnDealtDamage;
            }
            base.OnDestroy();
        }

            private void Shock(PlayerController user)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/water goop.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(base.Owner.sprite.WorldCenter, 15f, 0.01f, false);
        }
        private void OnDealtDamage(PlayerController player, float amount, bool fatal, HealthHaver target)
        {
            if (!onCooldown)
            {
                float value = UnityEngine.Random.value;
                bool flag = value < 0.1f;
                if (flag)
                {
                    this.Shock(player);
                }
            }
            GameManager.Instance.StartCoroutine(StartCooldown());
        }
        private static IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(0.3f);
            Ukulele.onCooldown = false;
            yield break;
        }
    }
}