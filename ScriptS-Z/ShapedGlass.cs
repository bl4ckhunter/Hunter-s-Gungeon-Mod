using System;
using ItemAPI;
using UnityEngine;

namespace Mod
{
	// Token: 0x02000018 RID: 24
	public class Glass : PassiveItem
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00005688 File Offset: 0x00003888
		public static void Init()
		{
			string name = "Shaped Glass";
			string resourcePath = "ClassLibrary1/Resources/shapedglass";
			GameObject gameObject = new GameObject();
			Glass glass = gameObject.AddComponent<Glass>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "The razor's edge.";
			string longDesc = "Double damage but lose 1.5 hearts (or 2 shields if you have shields) per hit unless you're already at 1.5 hearts or below \n" + "A glass dagger spun from time itself, this ancient artifact causes multiple timestreams to converge on a singular timeline, causing your every action to experience a manifold increase in consequences.";
			glass.SetupItem(shortDesc, longDesc, "ror");
			glass.quality = PickupObject.ItemQuality.D;
			glass.AddPassiveStatModifier(PlayerStats.StatType.Curse, 5f, StatModifier.ModifyMethod.ADDITIVE);
			glass.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			glass.TemporaryDamageMultiplier = 2f;
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00005704 File Offset: 0x00003904
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.Stats(player);
			player.OnReceivedDamage += this.ArmorShred;
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Combine(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.DoHealOnDeath));
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00005760 File Offset: 0x00003960
		public override DebrisObject Drop(PlayerController player)
		{ 
			player.OnReceivedDamage -= this.ArmorShred;
			HealthHaver healthHaver = player.healthHaver;
			healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.DoHealOnDeath));
			this.RemoveTemporaryBuff();
			return base.Drop(player);
		}

		protected override void OnDestroy()
		{
			if (base.Owner)
			{
				base.Owner.OnReceivedDamage -= this.ArmorShred;
				HealthHaver healthHaver = base.Owner.healthHaver;
				healthHaver.ModifyDamage = (Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>)Delegate.Remove(healthHaver.ModifyDamage, new Action<HealthHaver, HealthHaver.ModifyDamageEventArgs>(this.DoHealOnDeath));
				this.RemoveTemporaryBuff();
			}
			base.OnDestroy();
		}

		

		// Token: 0x060000B9 RID: 185 RVA: 0x00005794 File Offset: 0x00003994
		private void ArmorShred(PlayerController player)
		{
			this.armor = base.Owner.healthHaver.Armor;
			bool flag = this.armor > 1f;
			if (flag)
			{
				this.m_owner.healthHaver.Armor += -1f;
			}
		}

		// Token: 0x060000BA RID: 186 RVA: 0x000057E8 File Offset: 0x000039E8
		public void DoHealOnDeath(HealthHaver source, HealthHaver.ModifyDamageEventArgs args)
		{
			this.health = base.Owner.healthHaver.GetCurrentHealth();
			bool flag = this.health > 1.5f;
			bool flag2 = flag;
			bool flag3 = flag2;
			if (flag3)
			{
				args.ModifiedDamage = 1.5f;
			}
			bool flag4 = this.health < 1f;
			bool flag5 = flag4;
			bool flag6 = flag5;
			if (flag6)
			{
				args.ModifiedDamage = 0.75f;
			}
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00005858 File Offset: 0x00003A58
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

		// Token: 0x060000BC RID: 188 RVA: 0x000058C4 File Offset: 0x00003AC4
		private void RemoveTemporaryBuff()
		{
			this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier);
			this.m_buffedTarget.stats.RecalculateStats(this.m_buffedTarget, false, false);
			this.m_temporaryModifier = null;
			this.m_buffedTarget = null;
		}

		// Token: 0x0400008D RID: 141
		private float health;

		// Token: 0x0400008E RID: 142
		private float damage;

		// Token: 0x0400008F RID: 143
		private float damage1;

		// Token: 0x04000090 RID: 144
		private float damageUpdated;

		// Token: 0x04000091 RID: 145
		private float flag;

		// Token: 0x04000092 RID: 146
		private float damage2;

		// Token: 0x04000093 RID: 147
		public float healingAmount;

		// Token: 0x04000094 RID: 148
		public GameObject healVFX;

		// Token: 0x04000095 RID: 149
		public bool HealsBothPlayers;

		// Token: 0x04000096 RID: 150
		public bool DoesRevive;

		// Token: 0x04000097 RID: 151
		public bool ProvidesTemporaryDamageBuff;

		// Token: 0x04000098 RID: 152
		public float TemporaryDamageMultiplier;

		// Token: 0x04000099 RID: 153
		public bool IsOrange;

		// Token: 0x0400009A RID: 154
		public bool HasHealingSynergy;

		// Token: 0x0400009B RID: 155
		[LongNumericEnum]
		public CustomSynergyType HealingSynergyRequired;

		// Token: 0x0400009C RID: 156
		[ShowInInspectorIf("HasHealingSynergy", false)]
		public float synergyHealingAmount;

		// Token: 0x0400009D RID: 157
		protected PlayerController m_buffedTarget;

		// Token: 0x0400009E RID: 158
		protected StatModifier m_temporaryModifier;

		// Token: 0x0400009F RID: 159
		private bool active;

		// Token: 0x040000A0 RID: 160
		private float armor;
	}
}
