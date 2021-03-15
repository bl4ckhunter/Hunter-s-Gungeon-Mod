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
	internal class DeathCard : PlayerItem
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
			string name = "Death";
			string resourceName = "ClassLibrary1/Resources/card13";
			GameObject gameObject = new GameObject();
			DeathCard DeathCard = gameObject.AddComponent<DeathCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "One Shot. Make it count.";
			string longDesc = "Take one hit of damage and fire a powerful blast that one shots every enemy in its path.";
			DeathCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(DeathCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			DeathCard.consumable = true;
			DeathCard.quality = PickupObject.ItemQuality.COMMON;
			DeathCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(DeathCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			Projectile projectile = ((Gun)ETGMod.Databases.Items[508]).DefaultModule.projectiles[0];
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : base.LastOwner.CurrentGun.CurrentAngle), true);
			Projectile proj = gameObject.GetComponent<Projectile>();
			proj.ChangeTintColorShader(0f, Color.black);
			proj.projectile.baseData.damage *= 9999999999999f;



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
	

	







	


