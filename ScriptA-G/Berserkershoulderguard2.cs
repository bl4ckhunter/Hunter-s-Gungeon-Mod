using System;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class Berserk : PassiveItem
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584
		public static void Init()
		{
			string name = "Berserker Shoulderguard";
			string resourceName = "ClassLibrary1/Resources/BS"; ;
			GameObject gameObject = new GameObject();
			Berserk berserk = gameObject.AddComponent<Berserk>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Death begets death";
			string longDesc = "MOAR DAKKA!";
			berserk.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(berserk, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
			berserk.quality = PickupObject.ItemQuality.S;
			berserk.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			berserk.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000E410 File Offset: 0x0000C610
		private void Kill(PlayerController player)
		{
			this.Boost += 0.3f;
			this.CheckBoost += 1f;
			this.ColorBoost += 41f;
			bool flag = this.ColorBoost > 210f;
			if (flag)
			{
				this.ColorBoost = 210f;
			}
			bool flag2 = this.Boost > 3f;
			if (flag2)
			{
				this.Boost = 3f;
			}
		}
		private void OnDealtDamage(PlayerController player, float amount, bool fatal, HealthHaver target)
		{
			float value = UnityEngine.Random.value;
			bool flag = value < 0.05f;
			if (flag)
			{
				this.Kill(player);
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000E490 File Offset: 0x0000C690
		private void Damaged(PlayerController player)
		{
			this.Boost -= 4f;
			this.ColorBoost -= 210f;
			bool flag = this.ColorBoost < 0f;
			if (flag)
			{
				this.ColorBoost = 0f;
			}
			bool flag2 = this.Boost < 0f;
			if (flag2)
			{
				this.Boost = 0f;
			}
			this.CheckBoost -= 1f;
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000E510 File Offset: 0x0000C710
		private void NewFloor(PlayerController player)
		{
			this.Boost = 0f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000E534 File Offset: 0x0000C734
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

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E5CE File Offset: 0x0000C7CE
		protected override void Update()
		{
			base.Update();
			if (base.Owner)
			{
				this.Stats();
			}
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnKilledEnemy -= this.Kill;
				player.OnReceivedDamage -= this.Damaged;
				player.OnDealtDamageContext -= this.OnDealtDamage;
				player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.NewFloor));
				this.Boost = 0f;
				this.CheckBoost = 0f;
				this.LastCheckBoost = -1f;
			}
			base.OnDestroy();
		}
		// Token: 0x060001D5 RID: 469 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.Boost = 0f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
			player.OnKilledEnemy += this.Kill;
			player.OnDealtDamageContext += this.OnDealtDamage;
			player.OnReceivedDamage += this.Damaged;
			player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Combine(player.OnNewFloorLoaded, new Action<PlayerController>(this.NewFloor));
			player.OnRoomClearEvent += this.NewFloor;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E660 File Offset: 0x0000C860
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			player.OnKilledEnemy -= this.Kill;
			player.OnReceivedDamage -= this.Damaged;
			player.OnDealtDamageContext -= this.OnDealtDamage;
			player.OnNewFloorLoaded = (Action<PlayerController>)Delegate.Remove(player.OnNewFloorLoaded, new Action<PlayerController>(this.NewFloor));
			this.Boost = 0f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
			debrisObject.GetComponent<Berserk>().m_pickedUpThisRun = true;
			return debrisObject;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000E6F0 File Offset: 0x0000C8F0
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

		// Token: 0x040000BA RID: 186
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
	}
}
