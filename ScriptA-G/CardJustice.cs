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
	internal class JusticeCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private int Uses;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Justice";
			string resourceName = "ClassLibrary1/Resources/card8";
			GameObject gameObject = new GameObject();
			JusticeCard JusticeCard = gameObject.AddComponent<JusticeCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Perfectly Balanced";
			string longDesc = "Erases half the enemies in the room, three uses.";
			JusticeCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(JusticeCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			JusticeCard.quality = PickupObject.ItemQuality.COMMON;
			JusticeCard.consumable = true;
			JusticeCard.numberOfUses = 3;
			JusticeCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(JusticeCard);

		}


		protected override void DoEffect(PlayerController user)
		{

			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			base.CanBeDropped = false;
			List<AIActor> activeEnemies = base.LastOwner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				int count = activeEnemies.Count;
				for (int i = 0; i < count; i++)
				{
					if (i % 2 == 0)
					{
						if (activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss)
						{
							activeEnemies[i].ForceBlackPhantom = true;
							activeEnemies[i].healthHaver.ApplyDamage(activeEnemies[i].healthHaver.GetMaxHealth(), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
							GlobalSparksDoer.DoRadialParticleBurst(400, activeEnemies[i].specRigidbody.HitboxPixelCollider.UnitBottomLeft, activeEnemies[i].specRigidbody.HitboxPixelCollider.UnitTopRight, 90f, 2f, 0f, null, null, Color.red, GlobalSparksDoer.SparksType.BLACK_PHANTOM_SMOKE);
						}
				
					}
				}
			
			}

		}
				


		



		

	}
}
	

	







	


