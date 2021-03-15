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
	internal class TowerCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private GameObject Mines_Cave_In;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "The Tower";
			string resourceName = "ClassLibrary1/Resources/card16";
			GameObject gameObject = new GameObject();
			TowerCard TowerCard = gameObject.AddComponent<TowerCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Destruction and Creation";
			string longDesc = "Rocks fall from the sky for 9s, each rock has a 10% chance to spawn ammo drop on impact.";
			TowerCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(TowerCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			TowerCard.quality = PickupObject.ItemQuality.COMMON;
			TowerCard.consumable = true;
			TowerCard.m_activeDuration = 9f;
			TowerCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(TowerCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			ETGMod.StartGlobalCoroutine(RoRItems.TowerRockSlide(user));
			base.CanBeDropped = false;



		}

		private IEnumerator Antimage(PlayerController user)
		{
			float elapsed = 0f;
			while (elapsed < 9f)
			{ yield return new WaitForSeconds(0.6f);
				elapsed += 0.6f;
				Vector2 intVector = new Vector2 (this.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2).x, this.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2).y);
				AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
				this.Mines_Cave_In = assetBundle2.LoadAsset<GameObject>("Mines_Cave_In");
				base.StartCoroutine(this.HandleTriggerRockSlide(user, Mines_Cave_In, intVector));
				yield return null;
			}
			yield break;

		}

		private IEnumerator HandleTriggerRockSlide(PlayerController user, GameObject RockSlidePrefab, Vector2 targetPosition)
		{
			RoomHandler currentRoom = user.CurrentRoom;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RockSlidePrefab, targetPosition, Quaternion.identity);
			HangingObjectController RockSlideController = gameObject.GetComponent<HangingObjectController>();
			RockSlideController.triggerObjectPrefab = null;
			GameObject[] additionalDestroyObjects = new GameObject[]
			{
				RockSlideController.additionalDestroyObjects[1]
			};
			RockSlideController.additionalDestroyObjects = additionalDestroyObjects;
			UnityEngine.Object.Destroy(gameObject.transform.Find("Sign").gameObject);
			RockSlideController.ConfigureOnPlacement(currentRoom);
			yield return new WaitForSeconds(0.01f);
			RockSlideController.Interact(user);
			if (UnityEngine.Random.value < 0.11)
			{
				LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, user.CenterPosition, targetPosition, 0f);
			} user.ForceBlank(15f, 05, false, true, targetPosition);
			yield break;
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
	

	







	


