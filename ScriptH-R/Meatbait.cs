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
	public class Meat : IounStoneOrbitalItem
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "Meat bait";
			string resourcePath = "ClassLibrary1/Resources/Meat";
			GameObject gameObject = new GameObject(name);
			Meat Meat = gameObject.AddComponent<Meat>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Blood in the air";
			string longDesc = "Tiger Food";
			ItemBuilder.SetupItem(Meat, shortDesc, longDesc, "ror");
			Meat.quality = PickupObject.ItemQuality.EXCLUDED;
			Meat.BuildPrefab();
			Meat.OrbitalPrefab = Meat.orbitalPrefab;
			Meat.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			Meat.CanBeDropped = false;
		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{
			bool flag = Meat.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/Meat", null, true);
				gameObject.name = "Meat";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(12, 12));
				Meat.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				Meat.orbitalPrefab.shouldRotate = true;
				Meat.orbitalPrefab.orbitRadius = 2.5f;
				Meat.orbitalPrefab.orbitDegreesPerSecond = 90f;
				Meat.orbitalPrefab.orbitDegreesPerSecond = 120f;
				Meat.orbitalPrefab.SetOrbitalTier(0);
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
			Projectile projectile = ((Gun)ETGMod.Databases.Items[369]).DefaultModule.projectiles[0];
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
				component.ignoreDamageCaps = false;
			}
			base.Owner.RemovePassiveItem(this.PickupObjectId);


		}

		private void Rebooth(PlayerController user)

		{
			user.RemovePassiveItem(this.PickupObjectId);
			PickupObject pickupObject = Gungeon.Game.Items["ror:meat_bait"];
			user.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
		}






		// Token: 0x0400001B RID: 27
		public static Hook guonHook;

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;

		// Token: 0x0400001D RID: 29
	}
}
