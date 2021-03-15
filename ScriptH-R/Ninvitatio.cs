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
	public class Nchallange : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "N'kuhana's Invitation";
			string resourceName = "ClassLibrary1/Resources/marc";
			GameObject gameObject = new GameObject();
			Nchallange Nchallange = gameObject.AddComponent<Nchallange>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Do NOT accept lightly!";
			string longDesc = "Spawns two jammed high priests and teleports you to a reward room once they die.\n" + "Is a deathless life truly worth living? This is the question She asks of you, who have evaded true death thousands of times. \n" + "Answer Her summon, confront unknowable danger and let your actions be your argument, or discard this, leave, and let the haunting memory of Her Concepts be the only remnant of this encounter. \n\n" + "Make your decision now but know this, if you accept, failure means death.";
			Nchallange.SetupItem(shortDesc, longDesc, "ror");
			Nchallange.AddPassiveStatModifier(PlayerStats.StatType.Curse, 3f, StatModifier.ModifyMethod.ADDITIVE);
			Nchallange.consumable = true;
			Nchallange.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
			Nchallange.quality = PickupObject.ItemQuality.D;
			Nchallange.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
		}

		protected override void DoEffect(PlayerController user)
		{
			base.LastOwner.CurrentRoom.AddProceduralTeleporterToRoom();
			RoRItems.Rewardport();
			


		}

		public override bool CanBeUsed(PlayerController user)
		{
			return user.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) <= 0 && GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.RATGEON; } 
		
		
		private GameObject DoTentacleVFX(PlayerController user)
		{
				this.TentacleVFX = (GameObject)ResourceCache.Acquire("Global VFX/VFX_Tentacleport");
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TentacleVFX);
				gameObject.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(user.specRigidbody.UnitBottomCenter + new Vector2(0f, -1f), tk2dBaseSprite.Anchor.LowerCenter);
				gameObject.transform.position = gameObject.transform.position.Quantize(0.0625f);
				gameObject.GetComponent<tk2dBaseSprite>().UpdateZDepth();
				return gameObject;
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

		private void CheckRitual(PlayerController user)
		{		
			user.CurrentRoom.SealRoom();
			FloodFillUtility.PreprocessContiguousCells(this.LastOwner.CurrentRoom, this.LastOwner.specRigidbody.UnitCenter.ToIntVector2(VectorConversions.Floor), 0);
			IntVector2? targetCenter = new IntVector2?((user.CenterPosition + new Vector2(-2, 0)).ToIntVector2(VectorConversions.Floor));
			IntVector2? targetCenter1 = new IntVector2?((user.CenterPosition + new Vector2(+1, 0)).ToIntVector2(VectorConversions.Floor));
			this.DevilEnemyGuid = "db97e486ef02425280129e1e27c33118";
					string guid = this.DevilEnemyGuid;
					AIActor enemyPrefab = EnemyDatabase.GetOrLoadByGuid(guid);
                    AIActor aiactor = AIActor.Spawn(enemyPrefab, targetCenter.Value, user.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
			AIActor aiactor1 = AIActor.Spawn(enemyPrefab, targetCenter1.Value, user.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
			aiactor.HandleReinforcementFallIntoRoom(0.5f);
			aiactor1.HandleReinforcementFallIntoRoom(0.5f);
		}
	private IEnumerator ReturnWarp(PlayerController user) 
		{ yield return new WaitForSeconds(1f);
			user.CurrentRoom.CompletelyPreventLeaving = false;
			user.ForceStopDodgeRoll();
			this.DoTentacleVFX(user);
			yield return new WaitForSeconds(1.3f);
			Vector2 target = this.WarpTarget;
			user.WarpToPoint(target, false, false);
			yield return new WaitForSeconds(0.2f);
			RoomHandler room = user.CurrentRoom;
			IntVector2 randomVisibleClearSpot5 = user.CurrentRoom.GetRandomVisibleClearSpot(1, 1);
			Chest rainbow_Chest = GameManager.Instance.RewardManager.A_Chest;
			rainbow_Chest.IsLocked = false;
			Chest.Spawn(rainbow_Chest, new IntVector2((int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.x),(int)Math.Round(GameManager.Instance.PrimaryPlayer.CenterPosition.y)));
			PickupObject pickupObject = Gungeon.Game.Items["ror:n'kuhana's_spoils"];
			LootEngine.GivePrefabToPlayer(pickupObject.gameObject, user);
			yield break;


		}

	}

	}







	


