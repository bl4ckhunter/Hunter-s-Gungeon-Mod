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
using SaveAPI;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class MageCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private List<PickupObject> todestroy;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Archmage's Promise";
			string resourceName = "ClassLibrary1/Resources/iou";
			GameObject gameObject = new GameObject();
			MageCard MageCard = gameObject.AddComponent<MageCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Make A Wish!";
			string longDesc = "Transmutes a nearby item on the floor into a modded item of the same type and quality";
			MageCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(MageCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			MageCard.consumable = true;
			MageCard.quality = PickupObject.ItemQuality.COMMON;
			MageCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			MageCard.SetupUnlockOnCustomStat(CustomTrackedStats.MOBIUS_CHEST_ALLTIME_MONEY, 0f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(MageCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			List<PickupObject> Sguns = new List<PickupObject>();
			List<PickupObject> Spassives = new List<PickupObject>();
			List<PickupObject> Sactives = new List<PickupObject>();
			List<PickupObject> Aguns = new List<PickupObject>();
			List<PickupObject> Apassives = new List<PickupObject>();
			List<PickupObject> Aactives = new List<PickupObject>();
			List<PickupObject> Bguns = new List<PickupObject>();
			List<PickupObject> Bpassives = new List<PickupObject>();
			List<PickupObject> Bactives = new List<PickupObject>();
			List<PickupObject> Cguns = new List<PickupObject>();
			List<PickupObject> Cpassives = new List<PickupObject>();
			List<PickupObject> Cactives = new List<PickupObject>();
			List<PickupObject> Dguns = new List<PickupObject>();
			List<PickupObject> Dpassives = new List<PickupObject>();
			List<PickupObject> Dactives = new List<PickupObject>();


			todestroy = new List<PickupObject>();
			try
			{
				foreach (PickupObject p in PickupObjectDatabase.Instance.Objects)
				{
					if (p != null && p.PickupObjectId > 824)
					{
						if (p is Gun)
						{
							if (p.quality == PickupObject.ItemQuality.S)
							{ Sguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.A)
							{ Aguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.B)
							{ Bguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.C)
							{ Cguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.D)
							{ Dguns.Add(p); }
						}
						if (p is PassiveItem)
						{
							if (p.quality == PickupObject.ItemQuality.S)
							{ Spassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.A)
							{ Apassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.B)
							{ Bpassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.C)
							{ Cpassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.D)
							{ Dpassives.Add(p); }
						}
						if (p is PlayerItem)
						{
							if (p.quality == PickupObject.ItemQuality.S)
							{ Sactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.A)
							{ Aactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.B)
							{ Bactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.C)
							{ Cactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.D)
							{ Dactives.Add(p); }
						}

					}
				}
			}
			catch
			{ ETGModConsole.Log("---------------------ouch-------------------"); }
			try
			{
				PickupObject p = (user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user)) as PickupObject;
				int pickupidreplacement = new int();
				if (p is Gun)
				{
					if (p.quality == PickupObject.ItemQuality.S)
					{ pickupidreplacement = Sguns[UnityEngine.Random.Range(0, Sguns.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.A)
					{ pickupidreplacement = Aguns[UnityEngine.Random.Range(0, Aguns.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.B)
					{ pickupidreplacement = Bguns[UnityEngine.Random.Range(0, Bguns.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.C)
					{ pickupidreplacement = Cguns[UnityEngine.Random.Range(0, Cguns.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.D)
					{ pickupidreplacement = Dguns[UnityEngine.Random.Range(0, Dguns.Count)].PickupObjectId; }

					LootEngine.SpawnItem(PickupObjectDatabase.GetById(pickupidreplacement).gameObject, p.transform.position, new Vector2(0, 2), 2f);
					UnityEngine.Object.Destroy(p.gameObject);


				}
				if (p is PassiveItem)
				{
					if (p.quality == PickupObject.ItemQuality.S)
					{ pickupidreplacement = Spassives[UnityEngine.Random.Range(0, Spassives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.A)
					{ pickupidreplacement = Apassives[UnityEngine.Random.Range(0, Apassives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.B)
					{ pickupidreplacement = Bpassives[UnityEngine.Random.Range(0, Bpassives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.C)
					{ pickupidreplacement = Cpassives[UnityEngine.Random.Range(0, Cpassives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.D)
					{ pickupidreplacement = Dpassives[UnityEngine.Random.Range(0, Dpassives.Count)].PickupObjectId; }

					LootEngine.SpawnItem(PickupObjectDatabase.GetById(pickupidreplacement).gameObject, p.transform.position, new Vector2(0, 2), 2f);
					UnityEngine.Object.Destroy(p.gameObject);
				}
				if (p is PlayerItem)
				{
					if (p.quality == PickupObject.ItemQuality.S)
					{ pickupidreplacement = Sactives[UnityEngine.Random.Range(0, Sactives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.A)
					{ pickupidreplacement = Aactives[UnityEngine.Random.Range(0, Aactives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.B)
					{ pickupidreplacement = Bactives[UnityEngine.Random.Range(0, Bactives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.C)
					{ pickupidreplacement = Cactives[UnityEngine.Random.Range(0, Cactives.Count)].PickupObjectId; }
					if (p.quality == PickupObject.ItemQuality.D)
					{ pickupidreplacement = Dactives[UnityEngine.Random.Range(0, Dactives.Count)].PickupObjectId; }

					LootEngine.SpawnItem(PickupObjectDatabase.GetById(pickupidreplacement).gameObject, p.transform.position, new Vector2(0, 2), 2f);
					UnityEngine.Object.Destroy(p.gameObject);

				}
			}catch{ }
		}
		public override bool CanBeUsed(PlayerController user)
		{
			IPlayerInteractable nearestInteractable = user.CurrentRoom.GetNearestInteractable(user.CenterPosition, 1f, user);
			return nearestInteractable is PassiveItem | nearestInteractable is PlayerItem | nearestInteractable is Gun;

		}

		

	}
}
	

	







	


