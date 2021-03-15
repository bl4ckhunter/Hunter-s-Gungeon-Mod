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
	internal class TemperanceCard : PlayerItem
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
			string name = "Temperance";
			string resourceName = "ClassLibrary1/Resources/card14";
			GameObject gameObject = new GameObject();
			TemperanceCard TemperanceCard = gameObject.AddComponent<TemperanceCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Enrichment through sacrifice";
			string longDesc = "On use, take damage equal to half your maximum health and gain 35 casings for each half heart lost, free 50 casings when under half a heart.";
			TemperanceCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(TemperanceCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			TemperanceCard.consumable = true;
			TemperanceCard.quality = PickupObject.ItemQuality.COMMON;
			TemperanceCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(TemperanceCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			if (user.healthHaver.GetCurrentHealth() > 0.5f )
			{
				user.healthHaver.ForceSetCurrentHealth(user.healthHaver.GetCurrentHealth() / 2);
				LastOwner.carriedConsumables.Currency += (int)Math.Ceiling((user.healthHaver.GetCurrentHealth()) * 60);
			}
			else 
			{ LastOwner.carriedConsumables.Currency += 50;}

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
	

	







	


