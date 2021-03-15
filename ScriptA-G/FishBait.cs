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
	public class Fish : IounStoneOrbitalItem
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "Fish bait";
			string resourcePath = "ClassLibrary1/Resources/Fish";
			GameObject gameObject = new GameObject(name);
			Fish Fish = gameObject.AddComponent<Fish>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Chum in the water";
			string longDesc = "Shark food.";
			ItemBuilder.SetupItem(Fish, shortDesc, longDesc, "ror");
			Fish.quality = PickupObject.ItemQuality.EXCLUDED;
			Fish.BuildPrefab();
			Fish.OrbitalPrefab = Fish.orbitalPrefab;
			Fish.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			Fish.CanBeDropped = false;
		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{
			bool flag = Fish.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/Fish", null, true);
				gameObject.name = "Fish";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(12, 12));
				Fish.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				Fish.orbitalPrefab.shouldRotate = true;
				Fish.orbitalPrefab.orbitRadius = 2.5f;
				Fish.orbitalPrefab.orbitDegreesPerSecond = 90f;
				Fish.orbitalPrefab.orbitDegreesPerSecond = 120f;
				Fish.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnNewFloorLoaded += this.Rebooth;
			bool flag = this.m_extantOrbital != null;
			

				SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
				specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.Fire));

		}

		private void Fire(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{
			Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[359]).DefaultModule.chargeProjectiles[0].Projectile;
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, base.Owner.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag7 = component != null;
			bool flag8 = flag7;
			if (flag8)
			{
				component.Owner = base.Owner;
				component.Shooter = base.Owner.specRigidbody;
				component.baseData.speed = 10f;
				component.baseData.damage = 50f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
				component.ignoreDamageCaps = false;
			}
			base.Owner.RemovePassiveItem(this.PickupObjectId);


		}

		private void Rebooth(PlayerController user)

		{
			user.RemovePassiveItem(this.PickupObjectId);
			PickupObject pickupObject = Gungeon.Game.Items["ror:fish_bait"];
			user.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
		}






		// Token: 0x0400001B RID: 27
		public static Hook guonHook;

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;

		// Token: 0x0400001D RID: 29
	}
}
