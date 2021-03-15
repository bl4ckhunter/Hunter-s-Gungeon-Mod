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
	internal class StrenghtCard : PlayerItem
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
			string name = "Strength";
			string resourceName = "ClassLibrary1/Resources/card11";
			GameObject gameObject = new GameObject();
			StrenghtCard StrenghtCard = gameObject.AddComponent<StrenghtCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "May your power bring them rage.";
			string longDesc = "projectiles explode for 45s.";
			StrenghtCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(StrenghtCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			StrenghtCard.quality = PickupObject.ItemQuality.COMMON;
			StrenghtCard.consumable = true;
			StrenghtCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 45f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(StrenghtCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			base.LastOwner.PostProcessProjectile += RoRItems.Strenght;
			ETGMod.StartGlobalCoroutine(RoRItems.Strenghtcard(user));
			base.CanBeDropped = false;
			
			

		}

		private IEnumerator Antimage(PlayerController user)
		{
			yield return new WaitForSeconds(45f);
			base.LastOwner.PostProcessProjectile -= Mage;
			yield break;

		}

		private void Mage(Projectile arg2, float arg3)
		{
			Projectile arg1 = arg2.GetComponent<Projectile>();
			arg1.ChangeTintColorShader(1f, Color.yellow);
			arg1.gameObject.GetOrAddComponent<ExplosiveProjectile>();


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
	

	







	


