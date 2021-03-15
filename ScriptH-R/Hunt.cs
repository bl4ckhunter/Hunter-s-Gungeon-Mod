using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Mod
{
	// Token: 0x0200000E RID: 14
	public class Hunt : PassiveItem
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00004874 File Offset: 0x00002A74
		public static void Init()
		{
			string name = "Huntress's Tools";
			string resourcePath = "ClassLibrary1/Resources/arrow";
			GameObject gameObject = new GameObject(name);
			Hunt HuntnaDash = gameObject.AddComponent<Hunt>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Apex Predator";
			string longDesc = "...and so she left, her soul still remaining on the planet.\n\n\n"
				+ "Pressing reload throws an homing glaive, 5s cooldown";
			ItemBuilder.SetupItem(HuntnaDash, shortDesc, longDesc, "ror");
			HuntnaDash.quality = PickupObject.ItemQuality.C;
			HuntnaDash.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			HuntnaDash.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			HuntnaDash.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			HuntnaDash.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
			HuntnaDash.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000048CC File Offset: 0x00002ACC
		public override void Pickup(PlayerController player)
		{
			foreach (PickupObject pickupObject in Gungeon.Game.Items.Entries)
			{
				bool flag = pickupObject is ConsumableStealthItem;
				if (flag)
				{
					this.poofVFX = ((ConsumableStealthItem)pickupObject).poofVfx;
					break;
				}
			}
			base.Pickup(player);
			player.OnReloadPressed += this.Punch;
		}

		private void Punch(PlayerController obj, Gun gun)
		{
			if (!this.oncoodlown)
			{
				this.oncoodlown = true;
				this.Effect(base.Owner);
				GameManager.Instance.StartCoroutine(StartCooldown1());

			}

		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.OnReloadPressed -= this.Punch;
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			player.healthHaver.IsVulnerable = true;
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnReloadPressed -= this.Punch;
			}
			base.OnDestroy();
		}



		// Token: 0x06000055 RID: 85 RVA: 0x00004958 File Offset: 0x00002B58

		// Token: 0x06000056 RID: 86 RVA: 0x0000496C File Offset: 0x00002B6C

		// Token: 0x06000057 RID: 87 RVA: 0x000049A4 File Offset: 0x00002BA4

		// Token: 0x06000058 RID: 88 RVA: 0x000049D8 File Offset: 0x00002BD8
		private void Effect(PlayerController user)
		{
			Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[656]).DefaultModule.chargeProjectiles[0].Projectile;
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, user.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag4 = component != null;
			if (flag4)
			{
				component.Owner = base.Owner;
				component.AdjustPlayerProjectileTint(new Color(0f, 1f, 1f), 5, 0f);
				HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
				homingModifier.HomingRadius = 100f;
				homingModifier.AngularVelocity = 600f;
				component.AdditionalScaleMultiplier = 2f;
				component.baseData.damage = 10f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage); 
				component.BossDamageMultiplier = 10f;
				component.baseData.speed = 30f;
				component.baseData.force = 0f;
				component.SetOwnerSafe(base.Owner, "Player");
				component.Shooter = base.Owner.specRigidbody;
				component.OnDestruction += (Action<Projectile>)Delegate.Combine(component.OnHitEnemy, new Action<Projectile>(this.Fierypoop));
			}


		}

		private void Fierypoop(Projectile arg1)
		{      
			{ 
				Projectile projectile = ((Gun)ETGMod.Databases.Items[12]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, arg1.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle +  120f), true);
				GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile.gameObject, arg1.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + 0f), true);
				GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, arg1.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + 240f), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				Projectile component2 = gameObject2.GetComponent<Projectile>();
				Projectile component3 = gameObject3.GetComponent<Projectile>();
				bool flag = component != null;
				if (flag)
				{
					component.Owner = base.Owner;
					HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
					component.AdjustPlayerProjectileTint(new Color(0f, 3f, 3f), 5, 0f);
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 350f;
				PierceProjModifier pierce = component.gameObject.AddComponent<PierceProjModifier>();
				pierce.penetration = 3;
				component.Shooter = base.Owner.specRigidbody;
					component.baseData.speed = 20f;
					component.SetOwnerSafe(base.Owner, "Player");
					component.baseData.damage = 3 * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage); ;
				}
				bool flag2 = component2 != null;
				if (flag2)
				{
					component2.Owner = base.Owner;
					HomingModifier homingModifier2 = component2.gameObject.AddComponent<HomingModifier>();
					component2.AdjustPlayerProjectileTint(new Color(0f, 3f, 3f), 5, 0f);
					homingModifier2.HomingRadius = 100f;
					homingModifier2.AngularVelocity = 350f;
					PierceProjModifier pierce = component2.gameObject.AddComponent<PierceProjModifier>();
				pierce.penetration = 3;
				component2.Shooter = base.Owner.specRigidbody;
					component2.baseData.speed = 20f;
					component2.SetOwnerSafe(base.Owner, "Player");
					component2.baseData.damage = 3 * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage); ;
				}
				bool flag3 = component3 != null;
				if (flag3)
				{
					component3.Owner = base.Owner;
					HomingModifier homingModifier3 = component3.gameObject.AddComponent<HomingModifier>();
					component3.AdjustPlayerProjectileTint(new Color(0f, 3f, 3f), 5, 0f);
					homingModifier3.HomingRadius = 100f;
					homingModifier3.AngularVelocity = 350f;
					PierceProjModifier pierce = component3.gameObject.AddComponent<PierceProjModifier>();
					pierce.penetration = 3;
					component3.Shooter = base.Owner.specRigidbody;
					component3.SetOwnerSafe(base.Owner, "Player");
					component3.baseData.speed = 20f;
					component3.baseData.damage = 3 * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
				}

			}
		}



		private IEnumerator StartCooldown1()
		{
			yield return new WaitForSeconds(4f);
			this.oncoodlown = false;
			yield break;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004A60 File Offset: 0x00002C6

		// Token: 0x04000011 RID: 17
		public float dashDistance = 6f;

		// Token: 0x04000012 RID: 18
		public float dashSpeed = 70f;

		// Token: 0x04000013 RID: 19
		public float collisionDamage = 20f;

		// Token: 0x04000014 RID: 20
		public float finalDelay = 0f;

		// Token: 0x04000015 RID: 21

		// Token: 0x04000016 RID: 22
		public GameObject poofVFX;

		// Token: 0x04000017 RID: 23
		private bool m_isDashing;

		// Token: 0x04000018 RID: 24

		// Token: 0x04000019 RID: 25
		private List<AIActor> actorsPassed = new List<AIActor>();

		// Token: 0x0400001A RID: 26
		private List<MajorBreakable> breakablesPassed = new List<MajorBreakable>();
		private bool oncoodlown;
	}
}


// Token: 0x06007473 RID: 29811 RVA: 0x002D5E2C File Offset: 0x002D402C
