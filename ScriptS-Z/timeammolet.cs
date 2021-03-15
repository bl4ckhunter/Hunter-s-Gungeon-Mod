using System;
using System.Collections;
using ItemAPI;
using UnityEngine;

namespace Mod
{
	// Token: 0x02000016 RID: 22
	public class TimeAmmolet : BlankModificationItem
	{
		// Token: 0x060000A3 RID: 163 RVA: 0x0000538C File Offset: 0x0000358C
		public static void Init()
		{
			string name = "Chronobauble Ammolet";
			string resourcePath = "ClassLibrary1/Resources/Chronobauble";
			GameObject gameObject = new GameObject();
			TimeAmmolet timeAmmolet = gameObject.AddComponent<TimeAmmolet>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Blank Time";
			string longDesc = "Blanks stop time.\n" + "Originally nothing more than a curiosity, a trinket that marginally slowed down time in a very small area, the High Priest transmuted the sand inside into arcane gunpowder with a secret ritual.";
			timeAmmolet.SetupItem(shortDesc, longDesc, "ror");
			timeAmmolet.AddPassiveStatModifier(PlayerStats.StatType.AdditionalBlanksPerFloor, 1f, StatModifier.ModifyMethod.ADDITIVE);
			timeAmmolet.AddPassiveStatModifier(PlayerStats.StatType.Curse, 3f, StatModifier.ModifyMethod.ADDITIVE);
			timeAmmolet.quality = PickupObject.ItemQuality.A;
			timeAmmolet.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			timeAmmolet.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x0000541A File Offset: 0x0000361A
		public override void Pickup(PlayerController player)
		{
			player.OnUsedBlank += this.Blank;
			base.Pickup(player);
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00005438 File Offset: 0x00003638
		public override DebrisObject Drop(PlayerController player)
		{
			player.OnUsedBlank -= this.Blank;
			return base.Drop(player);
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnUsedBlank -= this.Blank;
			}
			base.OnDestroy();
		}
		// Token: 0x060000A6 RID: 166 RVA: 0x00005464 File Offset: 0x00003664
		private void Blank(PlayerController player, int blank)
		{
			IEnumerator routine = this.HandleDuration(player);
			GameManager.Instance.StartCoroutine(routine);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000054A7 File Offset: 0x000036A7
		private IEnumerator HandleDuration(PlayerController user)
		{
			test.RadialSlowHoldTime = 6f;
			test.RadialSlowOutTime = 2.5f;
			test.RadialSlowTimeModifier = 0f;
			test.DoesSepia = true;
			AkSoundEngine.PostEvent("State_Bullet_Time_on", base.gameObject);
			this.test.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
			yield break;
		}

		// Token: 0x0400006B RID: 107
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
	}
}
