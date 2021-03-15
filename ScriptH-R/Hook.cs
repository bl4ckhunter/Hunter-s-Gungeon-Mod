using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Breath : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Hook Bullets";
			string resourceName = "ClassLibrary1/Resources/Hook_bullets"; ;
			GameObject gameObject = new GameObject();
			Breath Breath = gameObject.AddComponent<Breath>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "....line and sinker";
			string longDesc = "It's hard to find a quiet fishing spot in the gungeon, but if you do, these will help you keep it that way";
			Breath.SetupItem(shortDesc, longDesc, "ror");
			Breath.quality = PickupObject.ItemQuality.C;
			Breath.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[29]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				GameObject gameObject1 = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + 120f), true);
				GameObject gameObject2 = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle - 120f), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 400f;
					component.Owner = base.Owner;
					component.Shooter = base.Owner.specRigidbody;
					component.baseData.force = -50f;
					component.baseData.damage = 3f;
					component.SetOwnerSafe(base.Owner, "Player");
					component.baseData.speed = 30f;
				}
				Projectile component1 = gameObject1.GetComponent<Projectile>();
				bool flag1 = component1 != null;
				if (flag1)
				{
					HomingModifier homingModifier = component1.gameObject.AddComponent<HomingModifier>();
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 400f;
					component1.Owner = base.Owner;
					component1.Shooter = base.Owner.specRigidbody;
					component1.baseData.force = -50f;
					component1.baseData.damage = 3f;
					component1.baseData.speed = 30f;
					component1.SetOwnerSafe(base.Owner, "Player");
				}
				Projectile component2 = gameObject2.GetComponent<Projectile>();
				bool flag2 = component != null;
				if (flag2)
				{
					HomingModifier homingModifier = component2.gameObject.AddComponent<HomingModifier>();
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 400f;
					component2.Owner = base.Owner;
					component2.Shooter = base.Owner.specRigidbody;
					component2.baseData.force = -50f;
					component2.baseData.damage = 3f;
					component2.baseData.speed = 30f;
					component2.SetOwnerSafe(base.Owner, "Player");
				}
			}
		}
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.5f);
			this.onCooldown = false;
			yield break;
		}
		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile1;
			player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile1;
			player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.PostProcessProjectile -= this.PostProcessProjectile1;
				player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			}
			base.OnDestroy();
		}

	}
 }     