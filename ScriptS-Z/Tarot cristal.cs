using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class ObsidianShard : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Obsidian Shard";
			string resourceName = "ClassLibrary1/Resources/luckyrock"; ;
			GameObject gameObject = new GameObject();
			ObsidianShard ObsidianShard = gameObject.AddComponent<ObsidianShard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Crystal Power!";
			string longDesc = "Extra chance of finding cards on roomclear";
			ItemBuilder.AddPassiveStatModifier(ObsidianShard, PlayerStats.StatType.Coolness, 3, StatModifier.ModifyMethod.ADDITIVE);
			ObsidianShard.SetupItem(shortDesc, longDesc, "ror");
			ObsidianShard.quality = PickupObject.ItemQuality.B;
			ObsidianShard.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}
		
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnRoomClearEvent += Spawncard;
		}
		private void Spawncard(PlayerController obj)
		{
			float f = obj.stats.GetStatValue(PlayerStats.StatType.Coolness) / 60f;
			float chance = 0.11f + f;
			if (UnityEngine.Random.value < chance)
			{
				int c = RoRItems.cards.Count;
				int i = UnityEngine.Random.Range(0, c);
				LootEngine.SpawnItem(RoRItems.cards[i].gameObject, obj.CenterPosition, new Vector2(0, 0), 0f);
			}
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.OnRoomClearEvent -= Spawncard;
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnRoomClearEvent -= Spawncard;
			}
			base.OnDestroy();
		}

	}
 }     