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
	internal class ReturnCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private RoomHandler roomHandler;
		private RoomHandler room;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Return Ticket";
			string resourceName = "ClassLibrary1/Resources/cardreturn";
			GameObject gameObject = new GameObject();
			ReturnCard ReturnCard = gameObject.AddComponent<ReturnCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "EXIT here";
			string longDesc = "Teleports you to a random room, disappears when you leave the shop by any means";
			ReturnCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(ReturnCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			ReturnCard.quality = PickupObject.ItemQuality.EXCLUDED;
			ReturnCard.consumable = true;
			ReturnCard.CanBeDropped = false;
			ReturnCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			base.LastOwner.CurrentRoom.UnsealRoom();
			List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
			for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
			{
				RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[list[i]];
				if (roomHandler2.IsStandardRoom && roomHandler2.IsVisible && !RoRItems.CreatedRooms.Contains(roomHandler2))
				{
					roomHandler = roomHandler2;
				}

			}
			for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++)
			{
				RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[list[i]];
				if (roomHandler == null && roomHandler2.IsShop && roomHandler2 != base.LastOwner.CurrentRoom && !RoRItems.CreatedRooms.Contains(roomHandler2))
				{
					roomHandler = roomHandler2;
				}

			}

			RoRItems.TeleportToRoom(user, roomHandler);


		}

		public override void Update()
		{
			if (base.LastOwner)
			{
				if (this.room == null)
				{
					RoomHandler room1 = base.LastOwner.CurrentRoom;
					room = room1;
				}
					
				if(this.room != base.LastOwner.CurrentRoom)
				{ base.LastOwner.RemoveActiveItem(this.PickupObjectId);
				this.room = null;
				}
			}
			base.Update();
		}

	}
}
	

	







	


