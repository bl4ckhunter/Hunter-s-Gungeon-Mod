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
	internal class StarsCard : PlayerItem
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
			string name = "The Stars";
			string resourceName = "ClassLibrary1/Resources/card17";
			GameObject gameObject = new GameObject();
			StarsCard StarsCard = gameObject.AddComponent<StarsCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Found Treasure!";
			string longDesc = "On use spawns a random chest.";
			StarsCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(StarsCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			StarsCard.quality = PickupObject.ItemQuality.COMMON;
			StarsCard.consumable = true;
			StarsCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(StarsCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			List<Chest> consumables = new List<Chest>();
				consumables.Add(GameManager.Instance.RewardManager.A_Chest);
				consumables.Add(GameManager.Instance.RewardManager.B_Chest);
				consumables.Add(GameManager.Instance.RewardManager.C_Chest);
				consumables.Add(GameManager.Instance.RewardManager.S_Chest);
			IntVector2 randomVisibleClearSpot5 = user.CurrentRoom.GetRandomVisibleClearSpot(2, 2);
			Chest.Spawn(consumables[UnityEngine.Random.Range(0,4)], new IntVector2((int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.x), (int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.y)));
			

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
	

	







	


