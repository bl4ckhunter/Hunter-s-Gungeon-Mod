using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class RatScroll : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Rat's Lair escape route";
			string resourceName = "ClassLibrary1/Resources/Rat";
			GameObject gameObject = new GameObject();
			RatScroll RatScroll = gameObject.AddComponent<RatScroll>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "To the Resourceful Rat's Lair!";
			string longDesc = "Use it after killing a boss to descend to the Resourceful Rat's Lair from an higher floor.\n\n" + "The Old King knew of chancellor Toadstool's treasonous intents and drafted a few escape plans, though he did share both his and Toadstool's plans with the current Bullet King the latter discarded them thoughtlessly, in time he'll regret doing that. \n" + "This route bypasses the sewer grate so why did he attach gnawed key to this scroll? Maybe you can find another use for it. \n";
			RatScroll.SetupItem(shortDesc, longDesc, "ror");
			RatScroll.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
			RatScroll.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		public static void BackKan(PlayerController obj)
		{
			PickupObject pickupObject1 = Gungeon.Game.Items["junkan"];
			LootEngine.GivePrefabToPlayer(pickupObject1.gameObject, obj);
			obj.OnNewFloorLoaded -= BackKan;
		}
		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_ENM_blobulord_reform_01", base.gameObject);
			user.RemoveActiveItem(this.PickupObjectId);
			if (user.HasPassiveItem(580))
			{
				float junkans = 0;
				foreach (PassiveItem junkan in base.LastOwner.passiveItems)
				{
					junkans += 1;
					if (junkan.PickupObjectId == 580)
					{
						user.OnNewFloorLoaded += BackKan;
					}
				}
				for (int i = 0; i < junkans; i++)
				{
					try
					{
						user.RemovePassiveItem(580);
					}
					catch
					{ }

				}
			}
			GameManager.Instance.StartCoroutine(DoOrDie(user));


		}
		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{
			if (enemy.IsBoss)
				this.flag = true;

		}

		private void Flag(Vector2 obj)
		{
			this.flag = true;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000F6CF File Offset: 0x0000D8CF
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			player.OnNewFloorLoaded += this.CheckFloor;
		}

		private void CheckFloor(PlayerController obj)
		{
			this.flag = false;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
			{ obj.RemoveActiveItem(this.PickupObjectId); }
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000F6FC File Offset: 0x0000D8FC

		public override bool CanBeUsed(PlayerController user)
		{
			return user.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) <= 0 && this.flag; } 
		
		public IEnumerator DoOrDie(PlayerController user)
		{
			new WaitForSeconds(0.05f);
			GameManager.Instance.LoadCustomLevel("ss_resourcefulrat");
			yield break;

		}






	}

	}







	


