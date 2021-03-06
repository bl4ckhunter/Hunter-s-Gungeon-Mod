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
	internal class EmperorCard : PlayerItem
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
			string name = "The Emperor";
			string resourceName = "ClassLibrary1/Resources/card4";
			GameObject gameObject = new GameObject();
			EmperorCard EmperorCard = gameObject.AddComponent<EmperorCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Protects!";
			string longDesc = "+2 to Armor.";
			EmperorCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(EmperorCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			EmperorCard.quality = PickupObject.ItemQuality.COMMON;
			EmperorCard.consumable = true;
			EmperorCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(EmperorCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			base.LastOwner.healthHaver.Armor += 2;



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
	

	







	


