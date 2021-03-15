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
using tk2dRuntime.TileMap;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class HeremitCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private static GenericRoomTable shop_room_table;
		private List<PrototypeDungeonRoom> NPCRoomList;
		private static Dungeon forge;
		private PrototypeDungeonRoom prototypeDungeonRoom;
		private bool m_IsTeleporting;
		private RoomHandler Lastroom;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "The Heremit";
			string resourceName = "ClassLibrary1/Resources/card9";
			GameObject gameObject = new GameObject();
			HeremitCard HeremitCard = gameObject.AddComponent<HeremitCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "May you find what you seek";
			string longDesc = "Spawns a forge shop on the floor you are on and teleports you there.";
			HeremitCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(HeremitCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			HeremitCard.quality = PickupObject.ItemQuality.COMMON;
			HeremitCard.consumable = true;
			HeremitCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(HeremitCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.RATGEON)
			{
				HeremitCard.forge = DungeonDatabase.GetOrLoadByName("Base_Forge");
				prototypeDungeonRoom = HeremitCard.forge.PatternSettings.flows[0].AllNodes[10].overrideExactRoom;
				base.LastOwner.CurrentRoom.AddProceduralTeleporterToRoom(); 
				RoomHandler roomHandler = RoRItems.AddCustomRuntimeRoom(prototypeDungeonRoom, true, true, false, null, DungeonData.LightGenerationStyle.STANDARD); 
				Lastroom = base.LastOwner.CurrentRoom;
				RoRItems.CreatedRooms.Add(roomHandler);
				RoRItems.TeleportToRoom(base.LastOwner, roomHandler);
				GameManager.Instance.StartCoroutine(RoRItems.Returnticket(user));
				used = true;
			}




		}
		public override bool CanBeUsed(PlayerController user)
		{
			return user.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) <= 0 && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.RATGEON;
		}
		private IEnumerator Return(PlayerController user, RoomHandler roomHandler)
		{ yield return new WaitForSeconds(3f);
			while (user.CurrentRoom == roomHandler) 
			{ yield return null; }

		}

		public void TeleportToRoom(PlayerController targetPlayer, RoomHandler targetRoom)
		{
			this.m_IsTeleporting = true;
			Pixelator.Instance.FadeToBlack(1.5f, false, 0f);
			IntVector2? randomAvailableCell = targetRoom.GetRandomAvailableCell(new IntVector2?(new IntVector2(2, 2)), new CellTypes?(CellTypes.FLOOR), false, null);
			if (randomAvailableCell == null)
			{
				this.m_IsTeleporting = false;
				return;
			}
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				PlayerController otherPlayer = GameManager.Instance.GetOtherPlayer(targetPlayer);
				if (otherPlayer)
				{
					this.TeleportToRoom(otherPlayer, targetRoom);
				}
			}
			targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
			GameManager.Instance.StartCoroutine(this.HandleTeleportToRoom(targetPlayer, randomAvailableCell.Value.ToCenterVector2()));
			targetPlayer.specRigidbody.Velocity = Vector2.zero;
			targetPlayer.knockbackDoer.TriggerTemporaryKnockbackInvulnerability(1f);
			targetRoom.EnsureUpstreamLocksUnlocked();
		}

		private void StunEnemiesForTeleport(RoomHandler targetRoom, float StunDuration = 0.5f)
		{
			if (!targetRoom.HasActiveEnemies(RoomHandler.ActiveEnemyType.All))
			{
				return;
			}
			List<AIActor> activeEnemies = targetRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies == null | activeEnemies.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < activeEnemies.Count; i++)
			{
				if (activeEnemies[i].IsNormalEnemy && activeEnemies[i].healthHaver && !activeEnemies[i].healthHaver.IsBoss)
				{
					if (activeEnemies[i].specRigidbody)
					{
						Vector2 unitBottomLeft = activeEnemies[i].specRigidbody.UnitBottomLeft;
					}
					else
					{
						Vector2 worldBottomLeft = activeEnemies[i].sprite.WorldBottomLeft;
					}
					if (activeEnemies[i].specRigidbody)
					{
						Vector2 unitTopRight = activeEnemies[i].specRigidbody.UnitTopRight;
					}
					else
					{
						Vector2 worldTopRight = activeEnemies[i].sprite.WorldTopRight;
					}
					if (activeEnemies[i] && activeEnemies[i].behaviorSpeculator)
					{
						activeEnemies[i].behaviorSpeculator.Stun(StunDuration, false);
					}
				}
			}
		}
		private IEnumerator HandleTeleportToRoom(PlayerController targetPlayer, Vector2 targetPoint)
		{
			if (targetPlayer.transform.position.GetAbsoluteRoom() != null)
			{
				this.StunEnemiesForTeleport(targetPlayer.transform.position.GetAbsoluteRoom(), 1f);
			}
			targetPlayer.healthHaver.IsVulnerable = false;
			CameraController cameraController = GameManager.Instance.MainCameraController;
			Vector2 offsetVector = cameraController.transform.position - targetPlayer.transform.position;
			offsetVector -= cameraController.GetAimContribution();
			Minimap.Instance.ToggleMinimap(false, false);
			cameraController.SetManualControl(true, false);
			cameraController.OverridePosition = cameraController.transform.position;
			targetPlayer.CurrentInputState = PlayerInputState.NoInput;
			yield return new WaitForSeconds(0.1f);
			yield return new WaitForSeconds(1f);
			targetPlayer.ToggleRenderer(false, "arbitrary teleporter");
			targetPlayer.ToggleGunRenderers(false, "arbitrary teleporter");
			targetPlayer.ToggleHandRenderers(false, "arbitrary teleporter");
			yield return new WaitForSeconds(1f);
			Pixelator.Instance.FadeToBlack(0.15f, false, 0f);
			yield return new WaitForSeconds(0.15f);
			targetPlayer.transform.position = targetPoint;
			targetPlayer.specRigidbody.Reinitialize();
			targetPlayer.specRigidbody.RecheckTriggers = true;
			if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
			{
				cameraController.OverridePosition = cameraController.GetIdealCameraPosition();
			}
			else
			{
				cameraController.OverridePosition = (targetPoint + offsetVector).ToVector3ZUp(0f);
			}
			targetPlayer.WarpFollowersToPlayer(false);
			targetPlayer.WarpCompanionsToPlayer(false);
			Pixelator.Instance.MarkOcclusionDirty();
			Pixelator.Instance.FadeToBlack(0.15f, true, 0f);
			yield return null;
			cameraController.SetManualControl(false, true);
			yield return new WaitForSeconds(0.15f);
			targetPlayer.DoVibration(Vibration.Time.Normal, Vibration.Strength.Medium);
			targetPlayer.ToggleRenderer(true, "arbitrary teleporter");
			targetPlayer.ToggleGunRenderers(true, "arbitrary teleporter");
			targetPlayer.ToggleHandRenderers(true, "arbitrary teleporter");
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(targetPlayer.specRigidbody, null, false);
			targetPlayer.CurrentInputState = PlayerInputState.AllInput;
			targetPlayer.healthHaver.IsVulnerable = true;
			this.m_IsTeleporting = false;
			base.LastOwner.RemoveActiveItem(this.PickupObjectId);
			yield break;
		}
		public RoomHandler AddCustomRuntimeRoom(PrototypeDungeonRoom prototype, bool addRoomToMinimap = true, bool addTeleporter = true, bool isSecretRatExitRoom = false, Action<RoomHandler> postProcessCellData = null, DungeonData.LightGenerationStyle lightStyle = DungeonData.LightGenerationStyle.STANDARD)
		{
			Dungeon dungeon = GameManager.Instance.Dungeon;
			tk2dTileMap mainTilemap = dungeon.MainTilemap;
			if (mainTilemap == null)
			{
				global::ETGModConsole.Log("ERROR: TileMap object is null! Something seriously went wrong!", false);
				Debug.Log("ERROR: TileMap object is null! Something seriously went wrong!");
				return null;
			}
			TK2DDungeonAssembler tk2DDungeonAssembler = new TK2DDungeonAssembler();
			tk2DDungeonAssembler.Initialize(dungeon.tileIndices);
			IntVector2 zero = IntVector2.Zero;
			IntVector2 intVector = new IntVector2(50, 50);
			int x = intVector.x;
			int y = intVector.y;
			IntVector2 intVector2 = new IntVector2(int.MaxValue, int.MaxValue);
			IntVector2 lhs = new IntVector2(int.MinValue, int.MinValue);
			intVector2 = IntVector2.Min(intVector2, zero);
			IntVector2 intVector3 = IntVector2.Max(lhs, zero + new IntVector2(prototype.Width, prototype.Height)) - intVector2;
			IntVector2 b = IntVector2.Min(IntVector2.Zero, -1 * intVector2);
			intVector3 += b;
			IntVector2 intVector4 = new IntVector2(dungeon.data.Width + x, x);
			int newWidth = dungeon.data.Width + x * 2 + intVector3.x;
			int newHeight = Mathf.Max(dungeon.data.Height, intVector3.y + x * 2);
			CellData[][] array = BraveUtility.MultidimensionalArrayResize<CellData>(dungeon.data.cellData, dungeon.data.Width, dungeon.data.Height, newWidth, newHeight);
			dungeon.data.cellData = array;
			dungeon.data.ClearCachedCellData();
			IntVector2 intVector5 = new IntVector2(prototype.Width, prototype.Height);
			IntVector2 b2 = zero + b;
			IntVector2 intVector6 = intVector4 + b2;
			CellArea cellArea = new CellArea(intVector6, intVector5, 0);
			cellArea.prototypeRoom = prototype;
			RoomHandler roomHandler = new RoomHandler(cellArea);
			for (int i = -x; i < intVector5.x + x; i++)
			{
				for (int j = -x; j < intVector5.y + x; j++)
				{
					IntVector2 intVector7 = new IntVector2(i, j) + intVector6;
					if ((i >= 0 && j >= 0 && i < intVector5.x && j < intVector5.y) || array[intVector7.x][intVector7.y] == null)
					{
						CellData cellData = new CellData(intVector7, CellType.WALL);
						cellData.positionInTilemap = cellData.positionInTilemap - intVector4 + new IntVector2(y, y);
						cellData.parentArea = cellArea;
						cellData.parentRoom = roomHandler;
						cellData.nearestRoom = roomHandler;
						cellData.distanceFromNearestRoom = 0f;
						array[intVector7.x][intVector7.y] = cellData;
					}
				}
			}
			dungeon.data.rooms.Add(roomHandler);
			try
			{
				roomHandler.WriteRoomData(dungeon.data);
			}
			catch (Exception)
			{
				global::ETGModConsole.Log("WARNING: Exception caused during WriteRoomData step on room: " + roomHandler.GetRoomName(), false);
			}
			try
			{
				dungeon.data.GenerateLightsForRoom(dungeon.decoSettings, roomHandler, GameObject.Find("_Lights").transform, lightStyle);
			}
			catch (Exception)
			{
				global::ETGModConsole.Log("WARNING: Exception caused during GeernateLightsForRoom step on room: " + roomHandler.GetRoomName(), false);
			}
			if (postProcessCellData != null)
			{
				postProcessCellData(roomHandler);
			}
			if (roomHandler.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.SECRET)
			{
				roomHandler.BuildSecretRoomCover();
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(BraveResources.Load("RuntimeTileMap", ".prefab"));
			tk2dTileMap component = gameObject.GetComponent<tk2dTileMap>();
			string str = UnityEngine.Random.Range(10000, 99999).ToString();
			gameObject.name = "Glitch_RuntimeTilemap_" + str;
			component.renderData.name = "Glitch_RuntimeTilemap_" + str + " Render Data";
			component.Editor__SpriteCollection = dungeon.tileIndices.dungeonCollection;
			try
			{
				TK2DDungeonAssembler.RuntimeResizeTileMap(component, intVector3.x + y * 2, intVector3.y + y * 2, mainTilemap.partitionSizeX, mainTilemap.partitionSizeY);
				IntVector2 intVector8 = new IntVector2(prototype.Width, prototype.Height);
				IntVector2 b3 = zero + b;
				IntVector2 intVector9 = intVector4 + b3;
				for (int k = -y; k < intVector8.x + y; k++)
				{
					for (int l = -y; l < intVector8.y + y + 2; l++)
					{
						tk2DDungeonAssembler.BuildTileIndicesForCell(dungeon, component, intVector9.x + k, intVector9.y + l);
					}
				}
				RenderMeshBuilder.CurrentCellXOffset = intVector4.x - y;
				RenderMeshBuilder.CurrentCellYOffset = intVector4.y - y;
				component.ForceBuild();
				RenderMeshBuilder.CurrentCellXOffset = 0;
				RenderMeshBuilder.CurrentCellYOffset = 0;
				component.renderData.transform.position = new Vector3((float)(intVector4.x - y), (float)(intVector4.y - y), (float)(intVector4.y - y));
			}
			catch (Exception exception)
			{
				global::ETGModConsole.Log("WARNING: Exception occured during RuntimeResizeTileMap / RenderMeshBuilder steps!", false);
				Debug.Log("WARNING: Exception occured during RuntimeResizeTileMap/RenderMeshBuilder steps!");
				Debug.LogException(exception);
			}
			roomHandler.OverrideTilemap = component;
			for (int m = 0; m < roomHandler.area.dimensions.x; m++)
			{
				for (int n = 0; n < roomHandler.area.dimensions.y + 2; n++)
				{
					IntVector2 intVector10 = roomHandler.area.basePosition + new IntVector2(m, n);
					if (dungeon.data.CheckInBoundsAndValid(intVector10))
					{
						CellData currentCell = dungeon.data[intVector10];
						TK2DInteriorDecorator.PlaceLightDecorationForCell(dungeon, component, currentCell, intVector10);
					}
				}
			}
			Pathfinder.Instance.InitializeRegion(dungeon.data, roomHandler.area.basePosition + new IntVector2(-3, -3), roomHandler.area.dimensions + new IntVector2(3, 3));
			if (prototype.usesProceduralDecoration && prototype.allowFloorDecoration)
			{
				new TK2DInteriorDecorator(tk2DDungeonAssembler).HandleRoomDecoration(roomHandler, dungeon, mainTilemap);
			}
			roomHandler.PostGenerationCleanup();
			if (addRoomToMinimap)
			{
				roomHandler.visibility = RoomHandler.VisibilityStatus.VISITED;
				base.StartCoroutine(Minimap.Instance.RevealMinimapRoomInternal(roomHandler, true, true, false));
				if (isSecretRatExitRoom)
				{
					roomHandler.visibility = RoomHandler.VisibilityStatus.OBSCURED;
				}
			}
			if (addTeleporter)
			{
				roomHandler.AddProceduralTeleporterToRoom();
			}
			if (addRoomToMinimap)
			{
				Minimap.Instance.InitializeMinimap(dungeon.data);
			}
			DeadlyDeadlyGoopManager.ReinitializeData();
			return roomHandler;
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
	

	







	


