using System;
using ItemAPI;
using UnityEngine;

namespace Mod
{
	// Token: 0x02000015 RID: 21
	public class Obelisk : PassiveItem
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00005140 File Offset: 0x00003340
		public static void Init()
		{
			string name = "Altar of Time";
			string resourcePath = "ClassLibrary1/Resources/Altar";
			GameObject gameObject = new GameObject();
			Obelisk obelisk = gameObject.AddComponent<Obelisk>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "True respite - Obliterate Yourself";
			string longDesc = "When you die, respawn at the beginning of the floor, lose all curse and gain coolness propotionally \n" + "Wipe the slate clean, forget all your curses and be reborn as a new You, a better, cooler You.";
			obelisk.SetupItem(shortDesc, longDesc, "ror");
			obelisk.AddPassiveStatModifier(PlayerStats.StatType.Curse, 2f, StatModifier.ModifyMethod.ADDITIVE);
			obelisk.quality = PickupObject.ItemQuality.S;
			obelisk.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			obelisk.CanBeDropped = false;
			Obelisk.DropDarkSoulItems = false;
			obelisk.DarkSoulsCursedHealthMax = 1;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000051C6 File Offset: 0x000033C6
		public override void Pickup(PlayerController player)
		{
			player.healthHaver.OnPreDeath += this.HandlePreDeath;
			base.Pickup(player);
			this.used = false;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000051F0 File Offset: 0x000033F0
		private void HandlePreDeath(Vector2 damageDirection)
		{
			this.DoEffect();
			this.Stats(base.Owner);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005204 File Offset: 0x00003404
		public void DoEffect()
		{
			bool flag = !this.used;
			if (flag)
			{
				this.m_owner.TriggerDarkSoulsReset(this.DropDarkSoulsItems, this.DarkSoulsCursedHealthMax);
				this.used = true;
			}
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005248 File Offset: 0x00003448
		private void Stats(PlayerController playerController)
		{
			float num = (float)PlayerStats.GetTotalCurse();
			float num2 = num;
			StatModifier item = new StatModifier
			{
				statToBoost = PlayerStats.StatType.Coolness,
				amount = num2 * 3f,
				modifyType = StatModifier.ModifyMethod.ADDITIVE
			};
			playerController.ownerlessStatModifiers.Add(item);
			playerController.stats.RecalculateStats(playerController, false, false);
			StatModifier item2 = new StatModifier
			{
				statToBoost = PlayerStats.StatType.Curse,
				amount = -num2,
				modifyType = StatModifier.ModifyMethod.ADDITIVE
			};
			playerController.ownerlessStatModifiers.Add(item2);
			playerController.stats.RecalculateStats(playerController, false, false);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000052DD File Offset: 0x000034DD
		protected override void OnDestroy()
		{
			base.OnDestroy();
		}

		// Token: 0x04000087 RID: 135
		public GameObject healVFX;

		// Token: 0x04000088 RID: 136
		private bool used;

		// Token: 0x04000089 RID: 137
		private static bool DropDarkSoulItems;

		// Token: 0x0400008A RID: 138
		private int DarkSoulsCursedHealthMax;

		// Token: 0x0400008B RID: 139
		private bool DropDarkSoulsItems;
	}
}
