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
	internal class EmpressCard : PlayerItem
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
			string name = "The Empress";
			string resourceName = "ClassLibrary1/Resources/card3";
			GameObject gameObject = new GameObject();
			EmpressCard EmpressCard = gameObject.AddComponent<EmpressCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "May your rage bring power";
			string longDesc = "double damage for 45s.";
			EmpressCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(EmpressCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			EmpressCard.quality = PickupObject.ItemQuality.COMMON;
			EmpressCard.consumable = true;
			EmpressCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 45f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(EmpressCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			base.LastOwner.PostProcessProjectile += RoRItems.Empress;
			ETGMod.StartGlobalCoroutine(RoRItems.AntiEmpress(user));
			base.CanBeDropped = false;
			
			

		}

		private static IEnumerator Antimage(PlayerController user)
		{
			yield return new WaitForSeconds(45f);
			user.PostProcessProjectile -= Mage;
			yield break;

		}

		private static void Mage(Projectile arg2, float arg3)
		{
			arg2.ChangeTintColorShader(0.5f, Color.red);
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
	

	







	


