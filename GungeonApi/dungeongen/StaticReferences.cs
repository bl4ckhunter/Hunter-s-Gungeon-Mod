using System;
using System.Collections.Generic;
using Dungeonator;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x0200000F RID: 15
	public static class StaticReferences
	{
		// Token: 0x0600004E RID: 78 RVA: 0x00008EB4 File Offset: 0x000070B4
		public static void Init()
		{
			StaticReferences.AssetBundles = new Dictionary<string, AssetBundle>();
			foreach (string text in StaticReferences.assetBundleNames)
			{
				try
				{
					AssetBundle x = ResourceManager.LoadAssetBundle(text);
					if (x == null)
					{
						Tools.PrintError<string>("Failed to load asset bundle: " + text, "FF0000");
					}
					else
					{
						StaticReferences.AssetBundles.Add(text, ResourceManager.LoadAssetBundle(text));
					}
				}
				catch (Exception e)
				{
					Tools.PrintError<string>("Failed to load asset bundle: " + text, "FF0000");
					Tools.PrintException(e, "FF0000");
				}
			}
			StaticReferences.RoomTables = new Dictionary<string, GenericRoomTable>();
			foreach (KeyValuePair<string, string> keyValuePair in StaticReferences.roomTableMap)
			{
				try
				{
					GenericRoomTable fallbackRoomTable = DungeonDatabase.GetOrLoadByName("base_" + keyValuePair.Key).PatternSettings.flows[0].fallbackRoomTable;
					StaticReferences.RoomTables.Add(keyValuePair.Key, fallbackRoomTable);
				}
				catch (Exception e2)
				{
					Tools.PrintError<string>("Failed to load room table: " + keyValuePair.Key + ":" + keyValuePair.Value, "FF0000");
					Tools.PrintException(e2, "FF0000");
				}
			}
			foreach (KeyValuePair<string, string> keyValuePair2 in StaticReferences.specialRoomTableMap)
			{
				try
				{
					GenericRoomTable asset = StaticReferences.GetAsset<GenericRoomTable>(keyValuePair2.Value);
					StaticReferences.RoomTables.Add(keyValuePair2.Key, asset);
				}
				catch (Exception e3)
				{
					Tools.PrintError<string>("Failed to load special room table: " + keyValuePair2.Key + ":" + keyValuePair2.Value, "FF0000");
					Tools.PrintException(e3, "FF0000");
				}
			}
			StaticReferences.subShopTable = StaticReferences.AssetBundles["shared_auto_001"].LoadAsset<SharedInjectionData>("_global injected subshop table");
			Tools.Print<string>("Static references initialized.", "FFFFFF", false);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000090F4 File Offset: 0x000072F4
		public static GenericRoomTable GetRoomTable(GlobalDungeonData.ValidTilesets tileset)
		{
			if (tileset <= GlobalDungeonData.ValidTilesets.MINEGEON)
			{
				switch (tileset)
				{
					case GlobalDungeonData.ValidTilesets.GUNGEON:
						return StaticReferences.RoomTables["gungeon"];
					case GlobalDungeonData.ValidTilesets.CASTLEGEON:
						return StaticReferences.RoomTables["castle"];
					case GlobalDungeonData.ValidTilesets.GUNGEON | GlobalDungeonData.ValidTilesets.CASTLEGEON:
						break;
					case GlobalDungeonData.ValidTilesets.SEWERGEON:
						return StaticReferences.RoomTables["sewer"];
					default:
						if (tileset == GlobalDungeonData.ValidTilesets.CATHEDRALGEON)
						{
							return StaticReferences.RoomTables["cathedral"];
						}
						if (tileset == GlobalDungeonData.ValidTilesets.MINEGEON)
						{
							return StaticReferences.RoomTables["mines"];
						}
						break;
				}
			}
			else
			{
				if (tileset == GlobalDungeonData.ValidTilesets.CATACOMBGEON)
				{
					return StaticReferences.RoomTables["catacombs"];
				}
				if (tileset == GlobalDungeonData.ValidTilesets.FORGEGEON)
				{
					return StaticReferences.RoomTables["forge"];
				}
				if (tileset == GlobalDungeonData.ValidTilesets.HELLGEON)
				{
					return StaticReferences.RoomTables["bullethell"];
				}
			}
			return StaticReferences.RoomTables["gungeon"];
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000091D8 File Offset: 0x000073D8
		public static T GetAsset<T>(string assetName) where T : UnityEngine.Object
		{
			T t = default(T);
			foreach (AssetBundle assetBundle in StaticReferences.AssetBundles.Values)
			{
				t = assetBundle.LoadAsset<T>(assetName);
				if (t != null)
				{
					break;
				}
			}
			return t;
		}

		// Token: 0x04000061 RID: 97
		public static Dictionary<string, AssetBundle> AssetBundles;

		// Token: 0x04000062 RID: 98
		public static Dictionary<string, GenericRoomTable> RoomTables;

		// Token: 0x04000063 RID: 99
		public static SharedInjectionData subShopTable;

		// Token: 0x04000064 RID: 100
		public static Dictionary<string, string> roomTableMap = new Dictionary<string, string>
		{
			{
				"castle",
				"Castle_RoomTable"
			},
			{
				"gungeon",
				"Gungeon_RoomTable"
			},
			{
				"mines",
				"Mines_RoomTable"
			},
			{
				"catacombs",
				"Catacomb_RoomTable"
			},
			{
				"forge",
				"Forge_RoomTable"
			},
			{
				"sewer",
				"Sewer_RoomTable"
			},
			{
				"cathedral",
				"Cathedral_RoomTable"
			},
			{
				"bullethell",
				"BulletHell_RoomTable"
			}
		};

		// Token: 0x04000065 RID: 101
		public static Dictionary<string, string> specialRoomTableMap = new Dictionary<string, string>
		{
			{
				"special",
				"basic special rooms (shrines, etc)"
			},
			{
				"shop",
				"Shop Room Table"
			},
			{
				"secret",
				"secret_room_table_01"
			}
		};

		// Token: 0x04000066 RID: 102
		public static string[] assetBundleNames = new string[]
		{
			"shared_auto_001",
			"shared_auto_002",
			"brave_resources_001"
		};

		// Token: 0x04000067 RID: 103
		public static string[] dungeonPrefabNames = new string[]
		{
			"base_gungeon",
			"base_castle",
			"base_mines",
			"base_catacombs",
			"base_forge",
			"base_sewer",
			"base_cathedral",
			"base_bullethell"
		};
	}
}
