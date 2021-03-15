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
	internal class Priestess : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private static TeleporterPrototypeItem teleporter;

		public IEnumerator Hangon()
		{
			new WaitForSeconds(0.2f);
		   base.LastOwner.RemoveActiveItem(this.PickupObjectId);
			yield break;
		}


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "The High Priestess";
			string resourceName = "ClassLibrary1/Resources/card2";
			GameObject gameObject = new GameObject();
			Priestess Priestess = gameObject.AddComponent<Priestess>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Squish";
			string longDesc = "Shrinks all enemies in the room, stomping on them yields ammo.";
			Priestess.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(Priestess, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			Priestess.quality = PickupObject.ItemQuality.COMMON;
			Priestess.consumable = true;
			Priestess.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(Priestess);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			List<AIActor> activeEnemies = base.LastOwner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{   
				int count = activeEnemies.Count;
				for (int i = 0; i < count; i++)
				{
					if (activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss)
					{
						SpeculativeRigidbody arg2 = activeEnemies[i].specRigidbody;
						Shrinker large = arg2.aiActor.gameObject.GetOrAddComponent<Shrinker>();
						large.largeness = 0.40f;
						arg2.aiActor.EnemyScale = new Vector2(1, 1) * large.largeness;
						if (large.largeness < 0.45f)
						{
							arg2.behaviorSpeculator.Stun(10000000f, true);
							arg2.aiActor.IgnoreForRoomClear = true;
							arg2.specRigidbody.OnPreRigidbodyCollision += Splat;
							arg2.AddCollisionLayerIgnoreOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyCollider));

						}
					}
				}
			}


		}




		private void Splat(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			Priestess.teleporter = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>();
			if (myRigidbody.UnitCenter != null)
			{
				UnityEngine.Object.Instantiate<GameObject>(teleporter.TelefragVFXPrefab, myRigidbody.UnitCenter, Quaternion.identity);
			}
			if (myRigidbody.aiActor.gameObject.GetComponent<ExplodeOnDeath>())
			{
				Destroy(myRigidbody.aiActor.gameObject.GetComponent<ExplodeOnDeath>());
			}
			Destroy(myRigidbody.gameObject);
			base.LastOwner.CurrentGun.GainAmmo(5);
			PhysicsEngine.SkipCollision = true;
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
	

	







	


