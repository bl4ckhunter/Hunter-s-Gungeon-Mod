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
	internal class Abbeyscroll : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Chancellor's Secret Plan";
			string resourceName = "ClassLibrary1/Resources/Chancellor";
			GameObject gameObject = new GameObject();
			Abbeyscroll Abbeyscroll = gameObject.AddComponent<Abbeyscroll>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Usurp the throne!";
			string longDesc = "Use it after killing a boss to open the path to the Abbey of the True Gun from anywhere in the Gungeon, from there you'll be able to go to any floor lower than the abbey including secret ones directly. \n The deeper you are when you use this, the thougher future enemies will get.\n\n" + "This scroll contains the Chancellor's plan to take the Bullet King's throne, from back when the Old King was still the ruler, until now he has always been too much of a coward to make good on them but maybe you can put them to good use. \n";
			Abbeyscroll.SetupItem(shortDesc, longDesc, "ror");
			Abbeyscroll.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
			Abbeyscroll.quality = PickupObject.ItemQuality.A;
			Abbeyscroll.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Abbeyscroll.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
			Abbeyscroll.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
			Abbeyscroll.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
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
			LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(316).gameObject, user);
			user.OnNewFloorLoaded += this.CheckFloor;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
			{ AIActor.HealthModifier *= 1.6f; ; }
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
			{ AIActor.HealthModifier *= 1.8f; ; }
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON)
			{ AIActor.HealthModifier *= 2f; }
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
			{ AIActor.HealthModifier *= 2.5f; }
			bool flag = GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.SEWERGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON;
			if(!flag)
			{ AIActor.HealthModifier *= 1.9f; }



		}

		public static void BackKan(PlayerController obj)
		{
			PickupObject pickupObject1 = Gungeon.Game.Items["junkan"];
			LootEngine.GivePrefabToPlayer(pickupObject1.gameObject, obj);
			obj.OnNewFloorLoaded -= BackKan;
		}

		private void CheckFloor(PlayerController obj)
		{
			PickupObject pickupObject = Gungeon.Game.Items["ror:mines_escape_route"];
			LootEngine.GivePrefabToPlayer(pickupObject.gameObject, obj);
			PickupObject pickupObject1 = Gungeon.Game.Items["ror:rat's_lair_escape_route"];
			LootEngine.GivePrefabToPlayer(pickupObject1.gameObject, obj);
			PickupObject pickupObject2 = Gungeon.Game.Items["ror:hollow_escape_route"];
			LootEngine.GivePrefabToPlayer(pickupObject2.gameObject, obj);
			PickupObject pickupObject3 = Gungeon.Game.Items["ror:offices_escape_route"];
			LootEngine.GivePrefabToPlayer(pickupObject3.gameObject, obj);
			PickupObject pickupObject4 = Gungeon.Game.Items["ror:forge_escape_route"];
			LootEngine.GivePrefabToPlayer(pickupObject4.gameObject, obj);
			PickupObject pickupObject5 = Gungeon.Game.Items["ror:old_king's_banishment_writ"];
			LootEngine.GivePrefabToPlayer(pickupObject5.gameObject, obj);

		}

		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{
			if(enemy.IsBoss)
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
			player.OnNewFloorLoaded = this.Reset;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000F6FC File Offset: 0x0000D8FC

		public override bool CanBeUsed(PlayerController user)
		{
			return user.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) <= 0 && this.flag; } 
		
		public IEnumerator DoOrDie(PlayerController user)
		{
			new WaitForSeconds(0.05f);
			Pixelator.Instance.FadeToBlack(1f);
			GameManager.Instance.LoadCustomLevel("tt_cathedral");
			yield break;

		}

		public void Reset(PlayerController obj)
		{ if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				this.flag = false;
			}
		}




	}

	}







	


