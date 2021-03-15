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
    public class Medkit : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Auto-Medkit";
            string resourceName = "ClassLibrary1/Resources/Medkit"; ;
            GameObject gameObject = new GameObject();
            Medkit Medkit = gameObject.AddComponent<Medkit>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Patch yourself up";
            string longDesc = "An automatic medkit prototype, if charged when the user takes damage this item restores body armor or heals the holder depending on the extent of the damage. Emits a green glow when fully charged and ready for use.";
            Medkit.SetupItem(shortDesc, longDesc, "ror");
            Medkit.quality = PickupObject.ItemQuality.B;
            Medkit.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {   
            this.charge += damage;
            bool flag = this.charge > 3500f;
            if (flag)
            {
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(54f, 255f, 121f, 50f));
                this.flaghealing = true;
                this.charge = 0f;

            }
        }
        



        // Token: 0x060001C9 RID: 457 RVA: 0x0000F6CF File Offset: 0x0000D8CF
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            player.OnReceivedDamage += this.Medkit1;
        }

        // Token: 0x060001CA RID: 458 RVA: 0x0000F6FC File Offset: 0x0000D8FC
        public override DebrisObject Drop(PlayerController player)
        {
            DebrisObject result = base.Drop(player);
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            player.OnReceivedDamage -= this.Medkit1;
            this.flaghealing = false;
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(player.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));

            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
                player.OnReceivedDamage -= this.Medkit1;
                this.flaghealing = false;
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(player.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
            }
            base.OnDestroy();
        }

        private void Medkit1(PlayerController player)
        { if (this.flaghealing)
            {
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(player.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
                this.flaghealing = false;
                bool flag2 = player.healthHaver.GetCurrentHealth() == (base.Owner.healthHaver.GetMaxHealth()-0.5f);
                bool flag3 = player.healthHaver.GetCurrentHealth() < base.Owner.healthHaver.GetMaxHealth();
                if(flag3)
                { base.Owner.healthHaver.ApplyHealing(1f); }
                if(flag2)
                { LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(120).gameObject, base.Owner);
                    base.Owner.healthHaver.ApplyHealing(0.5f);
                }


            } 
        
        }


        







        
        // Token: 0x040077B6 RID: 30646
        public float ActivationChance;

        // Token: 0x040077B8 RID: 30648
        public bool TriggersRadialBulletBurst;

        // Token: 0x040077B9 RID: 30649
        [ShowInInspectorIf("TriggersRadialBulletBurst", false)]
        public RadialBurstInterface RadialBurstSettings;

        private static bool onCooldown; private static bool onCooldown1;
        private float MedkitBase = 40f;

        // Token: 0x0400001D RID: 29
        private float MedkitTrue;

        // Token: 0x0400001E RID: 30
        private float Damage;

        // Token: 0x0400001F RID: 31
        private float lastDamage = -1f;
        private float charge;
        private bool flaghealing;
    }
      
 }     