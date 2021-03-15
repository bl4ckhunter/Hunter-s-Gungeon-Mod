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
	internal class SunCard : PlayerItem
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
			string name = "The Sun";
			string resourceName = "ClassLibrary1/Resources/card19";
			GameObject gameObject = new GameObject();
			SunCard SunCard = gameObject.AddComponent<SunCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Grossly Incandescent";
			string longDesc = "Fire radial laser blast and gives a spread ammo.";
			SunCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(SunCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			SunCard.quality = PickupObject.ItemQuality.COMMON;
            SunCard.consumable = true;
			SunCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 0.5f);
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(SunCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			user.GiveItem("partial_ammo");
            this.LaserSalvo();



		}

        private void LaserSalvo()
        {
            Projectile projectile = ((Gun)ETGMod.Databases.Items[508]).DefaultModule.projectiles[0];
            GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 0f), true);
            GameObject gameObject1 = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 315f), true);
            GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 90f), true);
            GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 180f), true);
            GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 270f), true);
            GameObject gameObject5 = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 45f), true);
            GameObject gameObject6 = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 135f), true);
            GameObject gameObject7 = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : 225f), true);
            Projectile component = gameObject.GetComponent<Projectile>();
            Projectile component0 = gameObject2.GetComponent<Projectile>();
            Projectile component3 = gameObject3.GetComponent<Projectile>();
            Projectile component4 = gameObject4.GetComponent<Projectile>();
            Projectile component5 = gameObject5.GetComponent<Projectile>();
            Projectile component6 = gameObject6.GetComponent<Projectile>();
            Projectile component7 = gameObject7.GetComponent<Projectile>();
            Projectile component2 = gameObject3.GetComponent<Projectile>();
            bool flag = component != null;
            if (flag)
            {
                component.Owner = base.LastOwner;
                component.SetOwnerSafe(base.LastOwner, "playe");
                component.Shooter = base.LastOwner.specRigidbody;
                component.baseData.speed = 50f;
                component.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            Projectile component1 = gameObject1.GetComponent<Projectile>();
            bool flag1 = component != null;
            if (flag1)
            {
                component1.Owner = base.LastOwner;
                component1.SetOwnerSafe(base.LastOwner, "playe");
                component1.Shooter = base.LastOwner.specRigidbody;
                component1.baseData.speed = 50f;
                component1.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            bool flag0 = component0 != null;
            if (flag0)
            {
                component0.Owner = base.LastOwner;
                component0.SetOwnerSafe(base.LastOwner, "playe");
                component0.Shooter = base.LastOwner.specRigidbody;
                component0.baseData.speed = 50f;
                component0.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            bool flag2 = component != null;
            if (flag2)
            {
                component2.Owner = base.LastOwner;
                component2.Shooter = base.LastOwner.specRigidbody;
                component2.baseData.speed = 50f;
                component2.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            bool flag3 = component != null;
            if (flag3)
            {
                component3.Owner = base.LastOwner;
                component3.Shooter = base.LastOwner.specRigidbody;
                component3.baseData.speed = 50f;
                component3.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            bool flag4 = component != null;
            if (flag4)
            {
                component4.Owner = base.LastOwner;
                component4.Shooter = base.LastOwner.specRigidbody;
                component4.baseData.speed = 50f;
                component4.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            bool flag5 = component != null;
            if (flag5)
            {
                component5.Owner = base.LastOwner;
                component5.Shooter = base.LastOwner.specRigidbody;
                component5.baseData.speed = 50f;
                component5.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            bool flag6 = component != null;
            if (flag6)
            {
                component6.Owner = base.LastOwner;
                component6.Shooter = base.LastOwner.specRigidbody;
                component6.baseData.speed = 50f;
                component6.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
            bool flag7 = component != null;
            if (flag7)
            {
                component7.Owner = base.LastOwner;
                component7.Shooter = base.LastOwner.specRigidbody;
                component7.baseData.speed = 50f;
                component7.baseData.damage = base.LastOwner.stats.GetStatValue(PlayerStats.StatType.Damage) * 50f;
            }
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
	

	







	


