using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using Dungeonator;
using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MultiplayerBasicExample;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Bird : IounStoneOrbitalItem
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "White Dove";
			string resourcePath = "ClassLibrary1/Resources/WaxQuail";
			GameObject gameObject = new GameObject(name);
			Bird Bird = gameObject.AddComponent<Bird>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "A bird carved from soft wax";
			string longDesc = "No one knows how this item made it to the gungeon nevermind stayed intact but despite not knowing its significance you can still feel the love that went into making it.";
			ItemBuilder.SetupItem(Bird, shortDesc, longDesc, "ror");
			Bird.quality = PickupObject.ItemQuality.B;
			Bird.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Bird.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Bird.BuildPrefab();
			Bird.OrbitalPrefab = Bird.orbitalPrefab;
			Bird.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			Bird.onCooldown = false;
		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{
			bool flag = Bird.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/WaxQuail", null, true);
				gameObject.name = "Birb";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(12, 12));
				Bird.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				Bird.orbitalPrefab.shouldRotate = false;
				Bird.orbitalPrefab.orbitRadius = 2.5f;
				Bird.orbitalPrefab.orbitDegreesPerSecond = 90f;
				Bird.orbitalPrefab.orbitDegreesPerSecond = 120f;
				Bird.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		public override void Pickup(PlayerController player)
		{
			this.onCooldown = false;
			base.Pickup(player);
			bool flag = this.m_extantOrbital != null;
			

			if (flag)
			{
				SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
				specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.Fire));


			}
				player.OnNewFloorLoaded += this.Rebooth;
		}

		private void Fire(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{ bool flag = this.onCooldown;
			if (!flag)
			{
				AkSoundEngine.PostEvent("Play_BOSS_hatch_squeak_01", base.gameObject);
				Projectile projectile = ((Gun)ETGMod.Databases.Items[445]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, myRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag1 = component != null;
				if (flag1)
				{
					component.Owner = base.Owner;
					HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 500f;
					component.Shooter = base.Owner.specRigidbody;
					component.baseData.speed = 9f;
					component.SetOwnerSafe(base.Owner, "Player");
					component.baseData.damage = 6f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
					component.AdditionalScaleMultiplier = 2f;
				}
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(this.StartCooldown());
			}
		}
		private void Rebooth(PlayerController user)

		{
				user.RemovePassiveItem(this.PickupObjectId);
				PickupObject pickupObject = Gungeon.Game.Items["ror:white_dove"];
			    user.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
		}



		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(3f);
			this.onCooldown = false;
			yield break;
		}




		// Token: 0x0400001B RID: 27
		public static Hook guonHook;

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;

		// Token: 0x0400001D RID: 29
		private bool onCooldown;
	}
}
