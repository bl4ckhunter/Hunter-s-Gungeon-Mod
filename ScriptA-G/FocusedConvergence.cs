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
    public class Glass : PassiveItem
    {
        private float health;
        private float damage;
        private float damage1;
        private float damageUpdated;
        private float flag;
        private float damage2;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Shaped Glass";
            string resourceName = "ClassLibrary1/Resources/shapedglass";
            GameObject gameObject = new GameObject();
            Glass glass = gameObject.AddComponent<Glass>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "The razor's edge.";
            string longDesc = "A glass dagger spun from time itself. \n" + "This ancient artifact causes multiple timestreams to converge on a singular timeline, causing your every action to experience a manifold increase in consequences. Thought the strain is great, if you have heart you will live.";
            glass.SetupItem(shortDesc, longDesc, "ror");
            glass.quality = PickupObject.ItemQuality.D;
            ItemBuilder.AddPassiveStatModifier(glass, PlayerStats.StatType.Curse, 5, StatModifier.ModifyMethod.ADDITIVE);
            glass.CanBeDropped = false;
            glass.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
            this.Stats();
            HealthHaver healthHaver = player.healthHaver;
            this.damage1 = this.GetStatValue(PlayerStats.StatType.Damage);
            healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.DoHealOnDeath));
        }
        protected override void Update()
        {
            base.Update();
            this.Stats1();
        }
        public void DoHealOnDeath(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
        {
            this.health = base.Owner.healthHaver.GetCurrentHealth();
            bool flag = health > 1.5f;
            if (flag)
            { args.ModifiedDamage = 1.5f;}
            bool flag2 = health < 1f;
            if (flag2) { args.ModifiedDamage = 0.75f; }

        }
        private void Stats1()
        {
            this.damageUpdated = this.GetStatValue(PlayerStats.StatType.Damage);
            bool flag = this.damage == this.damageUpdated;
            if (!flag)
            {

                this.damage2 = this.GetStatValue(PlayerStats.StatType.Damage) - this.damage;
                StatModifier item = new StatModifier
                {
                    statToBoost = PlayerStats.StatType.Damage,
                    amount = damage2,
                    modifyType = StatModifier.ModifyMethod.ADDITIVE
                };
                base.Owner.ownerlessStatModifiers.Add(item);
                base.Owner.stats.RecalculateStats(Owner, false, false);
                this.damage = this.GetStatValue(PlayerStats.StatType.Damage);

            }
        }




        private void Stats()
        {
            this.damage = this.GetStatValue(PlayerStats.StatType.Damage);
            StatModifier item = new StatModifier
            {
                statToBoost = PlayerStats.StatType.Damage,
                amount = damage * 0.5f,
                modifyType = StatModifier.ModifyMethod.ADDITIVE
            };
            base.Owner.ownerlessStatModifiers.Add(item);
            base.Owner.stats.RecalculateStats(Owner, false, false);
        }
        public float GetStatValue(PlayerStats.StatType type, int playerID = 0){
            {
                PlayerController playerController;
                if (playerID != 0)
                {
                    GameManager instance = GameManager.Instance;
                    playerController = ((instance != null) ? instance.SecondaryPlayer : null);
                }
                else
                {
                    GameManager instance2 = GameManager.Instance;
                    playerController = ((instance2 != null) ? instance2.PrimaryPlayer : null);
                }
                PlayerController playerController2 = playerController;
                bool flag = !playerController2;
                float result;
                if (flag)
                {
                    result = -1f;
                }
                else
                {
                    bool flag2 = type == PlayerStats.StatType.Coolness;
                    if (flag2)
                    {
                        float num = playerController2.stats.GetStatValue(PlayerStats.StatType.Coolness);
                        bool flag3 = PassiveItem.IsFlagSetForCharacter(playerController2, typeof(ChamberOfEvilItem));
                        if (flag3)
                        {
                            num += playerController2.stats.GetStatValue(PlayerStats.StatType.Curse) * 2f;
                        }
                        result = num;
                    }
                    else
                    {
                        result = playerController2.stats.GetStatValue(type);
                    }
                }
                return result;
            }
        }


}
   

}