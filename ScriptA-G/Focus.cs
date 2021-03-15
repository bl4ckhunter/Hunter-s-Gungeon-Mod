using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using Dungeonator;
using MultiplayerBasicExample;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class CF : PassiveItem
    {
        private float health;


           public static void Init()
        {
            string name = "Convergent Focus";
            string resourceName = "ClassLibrary1/Resources/Focused_Convergence";
            GameObject gameObject = new GameObject();
            CF CF = gameObject.AddComponent<CF>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Gungeon squared";
            string longDesc = "Another ancient artifact, this one shifts you to another plane of existence, one where the rules of the gungeon are different.";
            CF.SetupItem(shortDesc, longDesc, "ror");
            CF.quality = PickupObject.ItemQuality.EXCLUDED;
            ItemBuilder.AddPassiveStatModifier(CF, PlayerStats.StatType.ProjectileSpeed, 2.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            CF.active = true;
           CF.TemporaryDamageMultiplier = 2f;
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            player.OnDealtDamage += this.Trigger;
            player.OnRoomClearEvent += this.Heal;
            this.TemporaryDamageMultiplier = 2f;
            this.Stats(player);
        }
        // Token: 0x04007698 RID: 30360
        protected PlayerController m_buffedTarget;

        // Token: 0x04007699 RID: 30361
        protected StatModifier m_temporaryModifier;
        
        private void Trigger (PlayerController player, float amount)
        {

            {
                IEnumerator routine = this.HandleDuration(player);
                GameManager.Instance.StartCoroutine(routine);
            }
        }
        
        public override DebrisObject Drop(PlayerController player)
        {
            player.OnDealtDamage -= this.Trigger;
            player.OnRoomClearEvent -= this.Heal;
            this.RemoveTemporaryBuff();
            return base.Drop(player);
        }

        private void Stats(PlayerController user)
        {
            this.m_buffedTarget = user;
            this.m_temporaryModifier = new StatModifier();
            this.m_temporaryModifier.statToBoost = PlayerStats.StatType.Damage;
            this.m_temporaryModifier.amount = 2f;
            this.m_temporaryModifier.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
            user.ownerlessStatModifiers.Add(this.m_temporaryModifier);
            user.stats.RecalculateStats(user, false, false);
        }

        // Token: 0x04007693 RID: 30355

        public float TemporaryDamageMultiplier;

        private void Heal(PlayerController player)
        {
            this.health = base.Owner.healthHaver.GetMaxHealth() / 6f;
            player.healthHaver.ApplyHealing(this.health); bool flag2 = player.characterIdentity == PlayableCharacters.Robot;
            bool flag3 = player.healthHaver.Armor < 4f; 
            if (flag2 && flag3)
            { player.healthHaver.Armor += 1f; }

        }
        private IEnumerator HandleDuration(PlayerController user)
        {
            this.test.RadialSlowHoldTime = 1000f;
            this.test.RadialSlowOutTime = 2.75f;
            this.test.RadialSlowTimeModifier = 2.75f;
            this.test.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
            yield break;
        }




        private void RemoveTemporaryBuff()
        {
            this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier);
            this.m_buffedTarget.stats.RecalculateStats(this.m_buffedTarget, false, false);
            this.m_temporaryModifier = null;
            this.m_buffedTarget = null;
        }






        public float RadialSlowTimeModifier;

        // Token: 0x0400006C RID: 108
        public float timeScale;

        // Token: 0x0400006D RID: 109
        public float duration;

        // Token: 0x0400006E RID: 110
        public bool HasSynergy;

        // Token: 0x0400006F RID: 111
        [LongNumericEnum]
        public CustomSynergyType RequiredSynergy;

        // Token: 0x04000070 RID: 112
        public float overrideTimeScale;

        // Token: 0x04000071 RID: 113
        public RadialSlowInterface test;

        // Token: 0x04000072 RID: 114
        public float RadialSlowInTime;

        // Token: 0x04000073 RID: 115
        public float RadialSlowHoldTime;

        // Token: 0x04000074 RID: 116
        public float RadialSlowOutTime;
        private bool active;


        // Token: 0x0400768E RID: 30350
        public float healingAmount;

        // Token: 0x0400768F RID: 30351
        public GameObject healVFX;

        // Token: 0x04007690 RID: 30352
        public bool HealsBothPlayers;

        // Token: 0x04007691 RID: 30353
        public bool DoesRevive;

        // Token: 0x04007692 RID: 30354
        public bool ProvidesTemporaryDamageBuff;

        // Token: 0x04007693 RID: 30355

        // Token: 0x04007694 RID: 30356
        public bool IsOrange;

        // Token: 0x04007695 RID: 30357
        public bool HasHealingSynergy;

        // Token: 0x04007696 RID: 30358
        [LongNumericEnum]
        public CustomSynergyType HealingSynergyRequired;

        // Token: 0x04007697 RID: 30359
        [ShowInInspectorIf("HasHealingSynergy", false)]
        public float synergyHealingAmount;
        private bool breachproof;
        private bool trigger;

        // Token: 0x04007698 RID: 30360


    }




}