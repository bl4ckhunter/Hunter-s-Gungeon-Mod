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
    public class CritCola : PassiveItem
    {   
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Crit Cola";
            string resourceName = "ClassLibrary1/Resources/Critcola"; 
            GameObject gameObject = new GameObject();
            CritCola NRG = gameObject.AddComponent<CritCola>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "All in the reflexes";
            string longDesc = "Rolling over a bullet makes your bullets crit for a short while \n" + 	
            "I don't know how it did it, but dis actually makes me more handsome! -- The Scout\n";
            NRG.SetupItem(shortDesc, longDesc, "ror");
            NRG.quality = PickupObject.ItemQuality.B;
            NRG.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            NRG.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            NRG.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            player.OnIsRolling += HandleRollFrame;
            base.Pickup(player);
        }
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnIsRolling -= HandleRollFrame;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnIsRolling -= HandleRollFrame;
            }
            base.OnDestroy();
        }
        private void HandleRollFrame(PlayerController obj)
        {
            if (!this.onCooldown && obj.CurrentRollState == PlayerController.DodgeRollState.InAir)
            {
                Vector2 centerPosition = obj.CenterPosition;
                for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
                {
                    Projectile projectile = StaticReferenceManager.AllProjectiles[i];
                    if (projectile && projectile.Owner is AIActor)
                    {
                        float sqrMagnitude = (projectile.transform.position.XY() - centerPosition).sqrMagnitude;
                        if (sqrMagnitude < 0.75f)
                        {
                            base.StartCoroutine(this.HandleDuration(obj));
                            break;
                        }
                    }
                }
            }
        }

        private IEnumerator HandleDuration(PlayerController user)
        {
            bool flag = this.onCooldown;
            if (!flag)

            {
                this.onCooldown = true;
                user.OnPreFireProjectileModifier += Crit;
                Material outlineMaterial1 = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
                outlineMaterial1.SetColor("_OverrideColor", new Color(255f, 0f, 255f, 50f));
                yield return new WaitForSeconds(1.15f);
                this.onCooldown = false;
                user.OnPreFireProjectileModifier -= Crit;
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f, 50f));

            }

            yield break;
        }

        private Projectile Crit(Gun arg1, Projectile arg2)
        {
            ComplexProjectileModifier proj = PickupObjectDatabase.GetById(640).GetComponent<ComplexProjectileModifier>();
            return proj.CriticalProjectile;
        }






        // Token: 0x0400001D RID: 29
        private bool onCooldown;

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