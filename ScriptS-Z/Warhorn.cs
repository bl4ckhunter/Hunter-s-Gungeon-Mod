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
    public class Horn : PassiveItem
    {   
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Horn of War";
            string resourceName = "ClassLibrary1/Resources/WarHorn"; 
            GameObject gameObject = new GameObject();
            Horn Horn = gameObject.AddComponent<Horn>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "A Timeless Classic.";
            string longDesc = "Thought you can't hear it when you use an active item this horn emits sound at a peculiar frequence that only guns can hear, inspiring them to to fire much faster for a while." ;
            Horn.SetupItem(shortDesc, longDesc, "ror");
            Horn.quality = PickupObject.ItemQuality.B;
            Horn.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
        }
        public override void Pickup(PlayerController player)
        {
            this.Boost = 0f;
            this.CheckBoost = 0f;
            this.LastCheckBoost = -1f;
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
            yield return new WaitForSeconds(20f);
            Horn.onCooldown = false;
            yield break;
        }

        private void OnDealtDamage(PlayerController usingPlayer, PlayerItem usedItem)
        {
            IEnumerator routine = HandleDuration(usingPlayer);
            GameManager.Instance.StartCoroutine(routine);
        }

        private IEnumerator HandleDuration(PlayerController user)
        {
            bool flag = Horn.onCooldown;
            if (!flag)

            {
                Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
                outlineMaterial.SetColor("_OverrideColor", new Color(255f, 255f, 0f, 50f));
                this.Boost = 0.70f;
                this.CheckBoost = 0f;
                this.LastCheckBoost = -1f;
                yield return new WaitForSeconds(10f);
                this.Boost = 0f;
                this.CheckBoost = 0f;
                this.LastCheckBoost = -1f;

                Horn.onCooldown = true;
                outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
                IEnumerator routine1 = StartCooldown();
                GameManager.Instance.StartCoroutine(routine1);
            }

            yield break;
        }

        protected override void Update()
        {
            base.Update();
            this.Stats();
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
                this.RemoveStat(PlayerStats.StatType.RateOfFire);
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
                this.AddStat(PlayerStats.StatType.RateOfFire, amount, StatModifier.ModifyMethod.MULTIPLICATIVE);
                base.Owner.stats.RecalculateStats(base.Owner, true, false);
                this.LastCheckBoost = this.CheckBoost;
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
        private float Boost;
        private float CheckBoost;
        private float LastCheckBoost;
        private float ColorBoost;
        private float TrueColorBoost;
    } 
      
 }     