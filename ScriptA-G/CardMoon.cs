using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using Mod;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class MoonCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "The Moon";
			string resourceName = "ClassLibrary1/Resources/card18";
			GameObject gameObject = new GameObject();
			MoonCard MoonCard = gameObject.AddComponent<MoonCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Find what was hidden";
			string longDesc = "On use, reveals the current floor and removes 2 curse.";
			MoonCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(MoonCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			MoonCard.quality = PickupObject.ItemQuality.COMMON;
			MoonCard.consumable = true;
			MoonCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(MoonCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			user.GiveItem("map");
			StatModifier statModifier = new StatModifier();
			statModifier.statToBoost = PlayerStats.StatType.Curse;
			statModifier.amount = -2f;
			statModifier.modifyType = StatModifier.ModifyMethod.ADDITIVE;
			user.ownerlessStatModifiers.Add(statModifier);
			user.stats.RecalculateStats(user, false, false);



		}
		private void Stats(PlayerController playerController)
		{
			float num = (float)PlayerStats.GetTotalCurse();
			float num2 = new float();
			if (num > 1.4) { num2 = 1.5f; }
			else
			{ num2 = num; }
			StatModifier item2 = new StatModifier
			{
				statToBoost = PlayerStats.StatType.Curse,
				amount = num2,
				modifyType = StatModifier.ModifyMethod.ADDITIVE
			};
			playerController.ownerlessStatModifiers.Add(item2);
			playerController.stats.RecalculateStats(playerController, false, false);
		}

		public override void Update()
		{
			base.Update();
			if (base.LastOwner)
			{ 
				foreach (PlayerItem item in base.LastOwner.activeItems)
				{   
					if
					(RoRItems.cards.Contains(item)) 
					{ 
				    itemcount += 1;
					}
				}
				if (itemcount > 1)
				{ base.Drop(base.LastOwner);
					itemcount -= 1;
				}
			}
		}

	}
}
	

	







	


