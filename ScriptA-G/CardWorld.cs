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
	internal class WorldCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		public RadialSlowInterface test;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "The World";
			string resourceName = "ClassLibrary1/Resources/card21";
			GameObject gameObject = new GameObject();
			WorldCard WorldCard = gameObject.AddComponent<WorldCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Useless!";
			string longDesc = "Stops time for 10s and reveals the map.";
			WorldCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(WorldCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			WorldCard.quality = PickupObject.ItemQuality.COMMON;
			WorldCard.consumable = true;
			WorldCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 10f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(WorldCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			user.GiveItem("map");
			StartCoroutine(HandleDuration(user));

			base.CanBeDropped = false;
			
			

		}

		private IEnumerator HandleDuration(PlayerController user)
		{
			test.RadialSlowHoldTime = 10f;
			test.RadialSlowOutTime = 2.5f;
			test.RadialSlowTimeModifier = 0f;
			test.DoesSepia = true;
			AkSoundEngine.PostEvent("State_Bullet_Time_on", base.gameObject);
			this.test.DoRadialSlow(user.CenterPosition, user.CurrentRoom);
			yield break;
		}

		private void Mage(Projectile arg2, float arg3)
		{
			arg2.ChangeTintColorShader(1f, Color.red);
			arg2.projectile.baseData.damage *= 2f;


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
	

	







	


