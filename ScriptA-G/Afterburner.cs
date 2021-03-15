using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections.Generic;
using System.Collections;
using Gungeon;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Afterburner : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Warpdrive Afterburner";
            string resourceName = "ClassLibrary1/Resources/Hardlight_Afterburner"; ;
            GameObject gameObject = new GameObject();
            Afterburner Afterburner = gameObject.AddComponent<Afterburner>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Superluminal rolls";
            string longDesc = "Originally designed to give small fighter ships the ability to do tactical jumps in the middle of combat \n" +
            "after drinking an entire six-pack of NRG an engineer stranded on an unknown planet redesigned it for infantry usage.\n" +
            "After usage it needs about 30 seconds to recharge";
            Afterburner.SetupItem(shortDesc, longDesc, "ror");
            Afterburner.quality = PickupObject.ItemQuality.C;
            Afterburner.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
            Afterburner.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 255f, 255f, 50f));
            this.Boost = 1f;
            this.CheckBoost = 0f;
            this.LastCheckBoost = -1f;
            player.OnPreDodgeRoll += this.Blank;

        }

        public override DebrisObject Drop(PlayerController player)
        {
            this.Boost = 0f;
            this.CheckBoost = 0f;
            this.LastCheckBoost = -1f;
            player.OnPreDodgeRoll -= this.Blank;
            DebrisObject result = base.Drop(player);
            return result;
        }
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                this.Boost = 0f;
                this.CheckBoost = 0f;
                this.LastCheckBoost = -1f;
                player.OnPreDodgeRoll -= this.Blank;
            }
            base.OnDestroy();
        }

        private void Blank(PlayerController player)
        {
            Vector2 centerPosition = player.CenterPosition;
            bool flag = !Afterburner.onCooldown;
            if (flag)
            {
                PlayerController owner = base.Owner;
                base.StartCoroutine(this.HandleShield(base.Owner));
                base.StartCoroutine(this.StartCooldown());
                Afterburner.onCooldown = true;
            }
        }
        private IEnumerator StartCooldown()
        {
            yield return
            new WaitForSeconds(25f);
            Afterburner.onCooldown = false;
            this.Boost = 1f;
            this.CheckBoost = 0f;
            this.LastCheckBoost = -1f; 
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 255f, 255f, 50f));
            yield break;
        }


        private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
        {
            foreach (StatModifier statModifier in this.passiveStatModifiers)
            {
                bool flag = statModifier.statToBoost == statType;
                if (flag)
                {
                    return;
                }
            }
            StatModifier statModifier2 = new StatModifier
            {
                amount = amount,
                statToBoost = statType,
                modifyType = method
            };
            bool flag2 = this.passiveStatModifiers == null;
            if (flag2)
            {
                this.passiveStatModifiers = new StatModifier[]
                {
                    statModifier2
                };
                return;
            }
            this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
            {
                statModifier2
            }).ToArray<StatModifier>();
        }

        // Token: 0x060001D8 RID: 472 RVA: 0x0000E788 File Offset: 0x0000C988
        private void RemoveStat(PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            for (int i = 0; i < this.passiveStatModifiers.Length; i++)
            {
                bool flag = this.passiveStatModifiers[i].statToBoost != statType;
                if (flag)
                {
                    list.Add(this.passiveStatModifiers[i]);
                }
            }
            this.passiveStatModifiers = list.ToArray();
        }
        private void Stats()
        {
            bool flag = this.CheckBoost == this.LastCheckBoost;
            if (!flag)
            {
                this.RemoveStat(PlayerStats.StatType.DodgeRollSpeedMultiplier);
                float amount = this.Boost + 1f;
                bool flag2 = this.ColorBoost == 0f;
                if (flag2)
                {
                    this.TrueColorBoost = 0f;
                }
                else
                {
                    this.TrueColorBoost = this.ColorBoost + 45f;
                }
                this.AddStat(PlayerStats.StatType.DodgeRollSpeedMultiplier, amount, StatModifier.ModifyMethod.MULTIPLICATIVE);
                base.Owner.stats.RecalculateStats(base.Owner, true, false);
                this.LastCheckBoost = this.CheckBoost;
            }
        }
        private IEnumerator HandleShield(PlayerController user)
        {
            Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
            outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
            this.Boost = 3f;
            this.CheckBoost = 0f;
            this.LastCheckBoost = -1f;
            this.m_activeDuration = this.duration;
            this.m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
            user.sprite.usesOverrideMaterial = true;
            user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
            SpeculativeRigidbody specRigidbody = user.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
            user.healthHaver.IsVulnerable = false;
            float elapsed = 0f;
            while (elapsed < this.duration)
            {
                elapsed += BraveTime.DeltaTime;
                user.healthHaver.IsVulnerable = false;
                yield return null;
            }
            this.Boost = 0f;
            this.CheckBoost = 0f;
            this.LastCheckBoost = -1f;
            bool flag = user;
            if (flag)
            {
                user.healthHaver.IsVulnerable = true;
                user.ClearOverrideShader();
                user.sprite.usesOverrideMaterial = this.m_usedOverrideMaterial;
                SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
                specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
                specRigidbody2 = null;
            }
            bool flag2 = this;
            if (flag2)
            {
                AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
            }
            yield break;
        }


        protected override void Update()
        {
            base.Update();
            this.Stats();
        }
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
        {
            Projectile component = otherRigidbody.GetComponent<Projectile>();
            bool flag = component != null && !(component.Owner is PlayerController);
            if (flag)
            {
                PassiveReflectItem.ReflectBullet(component, true, base.Owner.specRigidbody.gameActor, 10f, 1f, 1f, 0f);
                PhysicsEngine.SkipCollision = true;
            }
        }



        public PlayerController LastOwner;

        private static bool onCooldown;

        private float Boost = 0f;

        // Token: 0x040000BB RID: 187
        private float LastCheckBoost = -1f;

        // Token: 0x040000BC RID: 188
        private float CheckBoost = 0f;

        // Token: 0x040000BD RID: 189
        private float TrueColorBoost;

        // Token: 0x040000BE RID: 190
        private float ColorBoost;

        // Token: 0x040000BF RID: 191
        private PlayerController m_player;
        private float m_activeDuration;

        // Token: 0x0400005A RID: 90
        private float duration = 5f;

        // Token: 0x0400005B RID: 91
        private bool m_usedOverrideMaterial;

    } }

