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
	internal class JudgementCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Judgement";
			string resourceName = "ClassLibrary1/Resources/card20";
			GameObject gameObject = new GameObject();
			JudgementCard JudgementCard = gameObject.AddComponent<JudgementCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Show No Mercy";
			string longDesc = "Spawns a keybulletkin and a chancebulletkin.";
			JudgementCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(JudgementCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			JudgementCard.quality = PickupObject.ItemQuality.COMMON;
			JudgementCard.consumable = true;
			JudgementCard.numberOfUses = 1;
			JudgementCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 45f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(JudgementCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			string guid;
			for (int e = 0; e <= 1; e++)
			{
				if (e == 1)
				{
					guid = "a446c626b56d4166915a4e29869737fd";
				}
				else
				{
					guid = "699cd24270af4cd183d671090d8323a1";
				}
				AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
				IntVector2? intVector = new IntVector2?(user.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
				AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Awaken, true);
				aiactor.CanTargetEnemies = false;
				aiactor.CanTargetPlayers = true;
				PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
				aiactor.IsHarmlessEnemy = false;
				aiactor.IgnoreForRoomClear = true;
				aiactor.HandleReinforcementFallIntoRoom(-1f);
			}
		}


	}
}
	

	







	


