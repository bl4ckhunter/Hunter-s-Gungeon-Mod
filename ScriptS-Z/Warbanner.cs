using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemAPI;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class Warbanner : PassiveItem
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584
		public static void Init()
		{
			string name = "Warbanner";
			string resourceName = "ClassLibrary1/Resources/Warbanner"; ;
			GameObject gameObject = new GameObject();
			Warbanner Warbanner = gameObject.AddComponent<Warbanner>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Charge Into The Fray";
			string longDesc = "Boost stats for the first few seconds of any fight \n" + "This banner might be cursed, old and tattered but the enchantments on it are still strong, if somewhat short-lasting.";
			Warbanner.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(Warbanner, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
			Warbanner.quality = PickupObject.ItemQuality.B;
			Warbanner.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Warbanner.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000E510 File Offset: 0x0000C710
		private void NewFloor()
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(255f, 0f, 0f, 50f));
			this.Boost = 0.25f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
			GameManager.Instance.StartCoroutine(StartCooldown(base.Owner));
		}
		private void Reset()
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
				this.RemoveStat(PlayerStats.StatType.Damage);
				this.RemoveStat(PlayerStats.StatType.RateOfFire);
				this.RemoveStat(PlayerStats.StatType.ReloadSpeed);
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
				this.AddStat(PlayerStats.StatType.Damage, amount, StatModifier.ModifyMethod.MULTIPLICATIVE);
				this.AddStat(PlayerStats.StatType.RateOfFire, amount, StatModifier.ModifyMethod.MULTIPLICATIVE);
				this.AddStat(PlayerStats.StatType.ReloadSpeed, amount, StatModifier.ModifyMethod.MULTIPLICATIVE);
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

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.Boost = 0f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
			player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(this.NewFloor));
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E660 File Offset: 0x0000C860
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);
			player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(this.NewFloor));
            this.Boost = 0f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
			debrisObject.GetComponent<Warbanner>().m_pickedUpThisRun = true;
			return debrisObject;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(this.NewFloor));
				this.Boost = 0f;
				this.CheckBoost = 0f;
				this.LastCheckBoost = -1f;
			}
			base.OnDestroy();
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
		private IEnumerator StartCooldown(PlayerController player)
		{
			yield return new WaitForSeconds(7f);
			this.Reset();
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f, 50f));

			yield break;
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
		private bool onCooldown;
	}
}
