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
	internal class WheelCard : PlayerItem
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
			string name = "The Wheel Of Fortune";
			string resourceName = "ClassLibrary1/Resources/card10";
			GameObject gameObject = new GameObject();
			WheelCard WheelCard = gameObject.AddComponent<WheelCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "What may be, will be";
			string longDesc = "On use, consumes half your casings and spawns a random card for every 20 casings lost, rounded up.";
			WheelCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(WheelCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			WheelCard.quality = PickupObject.ItemQuality.COMMON;
			WheelCard.consumable = true;
			WheelCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(WheelCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			if (user.carriedConsumables.Currency > 39)
			{
				int money = user.carriedConsumables.Currency;
				user.carriedConsumables.Currency -= money / 2;
				float floattries = money / 40;
				int tries = (int)Math.Ceiling(floattries);
				int c = RoRItems.cards.Count;
				for (int i = 0; i < tries; i++)
				{
					LootEngine.SpawnItem(PickupObjectDatabase.GetById(RoRItems.cards[UnityEngine.Random.Range(0, c)].PickupObjectId).gameObject, user.CenterPosition, new Vector2(0, 2), 2f);
				}
			}

		}

		public override bool CanBeUsed(PlayerController user)
		{
			bool falg = user.carriedConsumables.Currency > 39;
			return falg;
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
	

	







	


