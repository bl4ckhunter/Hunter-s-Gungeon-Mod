using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using GungeonAPI;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using Mod;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class TemeletryCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private bool m_usedOverrideMaterial;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "TelemetryCard";
			string resourceName = "ClassLibrary1/Resources/card7";
			GameObject gameObject = new GameObject();
			TemeletryCard ChariotCard = gameObject.AddComponent<TemeletryCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Unstoppable";
			string longDesc = "Invulnerable for 25s.";
			ChariotCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(ChariotCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			ChariotCard.quality = PickupObject.ItemQuality.COMMON;
			ChariotCard.consumable = true;
			ChariotCard.numberOfUses = 1;
			ChariotCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 25f);
			
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(ChariotCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			ETGModConsole.Log( $"{base.LastOwner.transform.position.x - user.CurrentRoom.area.basePosition.x}x ,{base.LastOwner.transform.position.y - user.CurrentRoom.area.basePosition.y}y");
			AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("humphrey");
			IntVector2? intVector = new IntVector2?(user.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
			AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Awaken, true);
			aiactor.CanTargetEnemies = false;
			aiactor.CanTargetPlayers = true;
			PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
			aiactor.IsHarmlessEnemy = false;
			aiactor.IgnoreForRoomClear = true;
			aiactor.HandleReinforcementFallIntoRoom(-1f);

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
	

	







	


