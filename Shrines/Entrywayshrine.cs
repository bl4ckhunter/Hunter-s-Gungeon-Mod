using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using GungeonAPI;
using Items;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

namespace Mod
{
	// Token: 0x02000035 RID: 53
	public static class EntryWayShrine
	{
		// Token: 0x06000145 RID: 325 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "entryway",
				modID = "ror",
				spritePath = "ClassLibrary1/Resources/BoomSprites/Gateway/entryway_idle_001.png",
				acceptText = "<Put him out of his misery>",
				declineText = "<Walk away>",
				OnAccept = new Action<PlayerController, GameObject>(EntryWayShrine.Accept),
				OnDecline = new Action<PlayerController, GameObject>(EntryWayShrine.Decline),
				CanUse = new Func<PlayerController, GameObject, bool>(EntryWayShrine.CanUse),
				offset = new Vector3(-0.5f, 0f, 1f),
				talkPointOffset = new Vector3(0f, 0f, 0f),
				isToggle = false,
				isBreachShrine = true,


				interactableComponent = typeof(EntryWayShrineInteractible)
			};
			GameObject gameObject = shrineFactory.Build();
			EntryWayShrineInteractible component = gameObject.GetComponent<EntryWayShrineInteractible>();
			component.conversation = new List<string>()
			{
				"The unseeing eyes of a dying soldier, doomed to relive his last moments forever, bore into you...."
			};
			gameObject.SetActive(false);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000B5C0 File Offset: 0x000097C0
		private static bool CanUse(PlayerController player, GameObject npc)
		{
			bool flag = player.HasPassiveItem(ChronoBattery.Orbid);
			return flag;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000B5D8 File Offset: 0x000097D8
		public static void Accept(PlayerController player, GameObject npc)

		{
			
			if (player.HasPassiveItem(ChronoBattery.Orbid))
			{
				try{


					GameManager.Instance.StartCoroutine(StartUpRoom(player));

				}
				catch
				{
					List<Chest> consumables = new List<Chest>();
					consumables.Add(GameManager.Instance.RewardManager.A_Chest);
					consumables.Add(GameManager.Instance.RewardManager.B_Chest);
					consumables.Add(GameManager.Instance.RewardManager.C_Chest);
					Chest chest = Chest.Spawn(consumables[UnityEngine.Random.Range(0, 4)], new IntVector2((int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.x), (int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.y)));
					chest.IsLocked = false;


				}
				player.RemovePassiveItem(ChronoBattery.Orbid);
				Used = true;
			}
		}

		private static IEnumerator StartUpRoom(PlayerController player)
		{
			GameObject gameObject = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Teleport_Beam");
			if (gameObject != null)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(player.specRigidbody.UnitBottomCenter + new Vector2(0f, -0.5f), tk2dBaseSprite.Anchor.LowerCenter);
				gameObject2.transform.position = gameObject2.transform.position.Quantize(0.0625f);
				gameObject2.GetComponent<tk2dBaseSprite>().UpdateZDepth();
			}
			player.IsVisible = false;
			Pixelator.Instance.FadeToBlack(0.5f, false, 3f);
			yield return new WaitForSeconds(1f);
			SecretArena();
			ArenaCard.TeleportToRoom(player, RoRItems.SecretArenaRoom);
			ETGMod.StartGlobalCoroutine(RoRItems.Assault(RoRItems.SecretArenaRoom, player));
			player.IsVisible = true; 
			yield break;
		}

		private static void SecretArena()
		{
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
			{
				bool generated1 = false;
				RoomFactory.rooms.TryGetValue("damnedtemple.room", out value);
				PrototypeDungeonRoom prototypeDungeonRoom = value.room;
				RoomHandler roomHandler = ArenaCard.AddCustomRuntimeRoom(prototypeDungeonRoom, true, true, false, null, DungeonData.LightGenerationStyle.STANDARD);
				Pathfinder.Instance.InitializeRegion(GameManager.Instance.Dungeon.data, roomHandler.area.basePosition, roomHandler.area.dimensions);
				roomHandler.hasEverBeenVisited = false;
				RoRItems.SecretArenaRoom = roomHandler;
				ArenaCard.AddModman(roomHandler);
				ArenaCard.AddTempleShrine(roomHandler);
				ArenaCard.AddMobiusCoffer(roomHandler);
				ArenaCard.AddDeadkingShrine(roomHandler);
				List<int> list = Enumerable.Range(0, GameManager.Instance.Dungeon.data.rooms.Count).ToList<int>().Shuffle<int>();
				for (int ig = 0; ig < GameManager.Instance.Dungeon.data.rooms.Count; ig++)
				{
					RoomHandler roomHandler2 = GameManager.Instance.Dungeon.data.rooms[list[ig]];
					if (roomHandler2.IsStandardRoom && !generated1)
					{
						IntVector2 intVector = roomHandler2.GetRandomVisibleClearSpot(6, 6);
						if (intVector != null && intVector != IntVector2.Zero)
						{
							ShrineFactory.builtShrines.TryGetValue("ror:entryway", out GameObject gabject);
							GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, new Vector3(intVector.x, intVector.y), Quaternion.identity);
							IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
							IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
							RoomHandler absoluteRoom = roomHandler2;
							for (int i = 0; i < interfaces.Length; i++)
							{
								absoluteRoom.RegisterInteractable(interfaces[i]);
							}
							for (int j = 0; j < interfaces2.Length; j++)
							{
								interfaces2[j].ConfigureOnPlacement(absoluteRoom);
							}
							generated1 = true;
						}

					}

				}

			}
			else
			{
				RoRItems.SecretArenaRoom = null;

			}

		}

		// Token: 0x06000148 RID: 328 RVA: 0x0000B68F File Offset: 0x0000988F
		public static void Decline(PlayerController player, GameObject npc)
		{
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000B694 File Offset: 0x00009894



		// Token: 0x0600014B RID: 331 RVA: 0x0000B780 File Offset: 0x00009980



		// Token: 0x04000060 RID: 96
		private static PlayerController storedPlayer;

		// Token: 0x04000061 RID: 97
		public static int Char = 0;

		// Token: 0x04000062 RID: 98
		private static string header = "";

		// Token: 0x04000063 RID: 99
		private static string text = "";

		// Token: 0x04000064 RID: 100
		private static bool hmmYes = true;
		private static float elapsed;
		public static GameObject GameObject;
		private static bool Used;
		private static RoomFactory.RoomData value;
		private static GameObject GameObject1;
	}
}