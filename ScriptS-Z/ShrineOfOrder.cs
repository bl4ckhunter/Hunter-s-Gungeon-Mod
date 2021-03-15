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
	public class Order : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Altar of Order";
			string resourceName = "ClassLibrary1/Resources/Order";
			GameObject gameObject = new GameObject();
			Order Nchallange = gameObject.AddComponent<Order>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "You Will Be ...Sequenced";
			string longDesc = "Randomly selects one passive item for each chest quality in the player's inventory and transforms all other passive items of the same rarity into that item\n" + "From Order, Perfection. From Perfection, Power. \n" + "Many fear Chaos, but what of Order? Is it perhaps becouse it is a more elusive, intangible, energy or is it simply so destructive, so dangerous, that there no witnesses left to warn us of it?";
			Nchallange.SetupItem(shortDesc, longDesc, "ror");
			Nchallange.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
			Nchallange.quality = PickupObject.ItemQuality.B;
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_ENM_blobulord_reform_01", base.gameObject);
			this.Thing();
			this.LastOwner.RemoveActiveItem(this.PickupObjectId);
		}

		private void Thing()
		{
			LootEngine.DoDefaultSynergyPoof(base.LastOwner.sprite.WorldCenter, false);
			List<PassiveItem> passiveItems = base.LastOwner.passiveItems;
			List<PassiveItem> Srank = new List<PassiveItem>();
			List<PassiveItem> Arank = new List<PassiveItem>();
			List<PassiveItem> Brank = new List<PassiveItem>();
			List<PassiveItem> Crank = new List<PassiveItem>();
			List<PassiveItem> Drank = new List<PassiveItem>();
			int count = passiveItems.Count;
			for (int i = 0; i < count; i++)
			{ if (passiveItems[i].CanBeDropped && !passiveItems[i].PreventStartingOwnerFromDropping)
				{
					if (passiveItems[i].quality == PickupObject.ItemQuality.S)
					{
						Srank.Add(passiveItems[i]);
					}
					if (passiveItems[i].quality == PickupObject.ItemQuality.A)
					{
						Arank.Add(passiveItems[i]);
					}
					if (passiveItems[i].quality == PickupObject.ItemQuality.B)
					{
						Brank.Add(passiveItems[i]);
					}
					if (passiveItems[i].quality == PickupObject.ItemQuality.C)
					{
						Crank.Add(passiveItems[i]);
					}
					if (passiveItems[i].quality == PickupObject.ItemQuality.D)
					{
						Drank.Add(passiveItems[i]);
					}
				}
			}

			for (int i = base.LastOwner.passiveItems.Count - 1; i >= 0; i--)
			{
				if (passiveItems[i].CanBeDropped && passiveItems[i].quality != PickupObject.ItemQuality.COMMON && passiveItems[i].quality != PickupObject.ItemQuality.SPECIAL && passiveItems[i].quality != PickupObject.ItemQuality.EXCLUDED && !passiveItems[i].PreventStartingOwnerFromDropping)
				{
					base.LastOwner.RemovePassiveItem(passiveItems[i].PickupObjectId);
				}
			}
			
			if (Srank != null)
			{
				int Sidex = UnityEngine.Random.Range(0, Srank.Count);
				for (int iS = 0; iS < Srank.Count; iS++)
				{ this.LastOwner.AcquirePassiveItemPrefabDirectly(Srank[Sidex]); }
			
		}
			if (Arank != null)
			{
					int Aidex = UnityEngine.Random.Range(0, Arank.Count);
					for (int iA = 0; iA < Arank.Count; iA++)
					{ this.LastOwner.AcquirePassiveItemPrefabDirectly(Arank[Aidex]); }
			}
			if (Brank != null)
			{
				int Bidex = UnityEngine.Random.Range(0, Brank.Count);
					for (int iB = 0; iB < Brank.Count; iB++)
					{ this.LastOwner.AcquirePassiveItemPrefabDirectly(Brank[Bidex]); }
			}
			if (Crank != null)
			{
				int Cidex = UnityEngine.Random.Range(0, Crank.Count);
					for (int iC = 0; iC < Crank.Count; iC++)
					{ this.LastOwner.AcquirePassiveItemPrefabDirectly(Crank[Cidex]); }
			}
			if (Drank != null)
			{
				int Didex = UnityEngine.Random.Range(0, Drank.Count);
				for (int iD = 0; iD < Drank.Count; iD++)
				{ this.LastOwner.AcquirePassiveItemPrefabDirectly(Drank[Didex]); }

			}
		}
	}
}







	


