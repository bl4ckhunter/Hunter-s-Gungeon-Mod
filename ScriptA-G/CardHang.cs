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
	internal class HangCard : PlayerItem
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
			string name = "The Hanged Man";
			string resourceName = "ClassLibrary1/Resources/card12";
			GameObject gameObject = new GameObject();
			HangCard HangCard = gameObject.AddComponent<HangCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Enlightment through sacrifice";
			string longDesc = "On use, take damage equal to half your current health and gain a card for each full heart lost, rounded up, free card when under half a heart.";
			HangCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(HangCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			HangCard.quality = PickupObject.ItemQuality.COMMON;
			HangCard.consumable = true;
			HangCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(HangCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			if (user.healthHaver.GetCurrentHealth() > 0.5f)
			{
				user.healthHaver.ForceSetCurrentHealth(user.healthHaver.GetCurrentHealth() / 2);
				float floattries = user.healthHaver.GetCurrentHealth();
				int tries = (int)Math.Ceiling(floattries);
				int c = RoRItems.cards.Count;
				for (int i = 0; i < tries; i++)
				{
					LootEngine.SpawnItem(PickupObjectDatabase.GetById(RoRItems.cards[UnityEngine.Random.Range(0, c)].PickupObjectId).gameObject, user.CenterPosition, new Vector2(0, 2), 2f);
				}
			}
			else
			{ LootEngine.SpawnItem(PickupObjectDatabase.GetById(RoRItems.cards[UnityEngine.Random.Range(0, RoRItems.cards.Count)].PickupObjectId).gameObject, user.CenterPosition, new Vector2(0, 2), 2f); }

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
	

	







	


