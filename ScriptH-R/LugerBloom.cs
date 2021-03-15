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
	public class Flower : IounStoneOrbitalItem
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "Gungeon Bloom";
			string resourcePath = "ClassLibrary1/Resources/luger";
			GameObject gameObject = new GameObject(name);
			Flower Flower = gameObject.AddComponent<Flower>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Gunpowder Flower";
			string longDesc = "This plant means buisness.";
			ItemBuilder.SetupItem(Flower, shortDesc, longDesc, "ror");
			Flower.quality = PickupObject.ItemQuality.EXCLUDED;
			Flower.BuildPrefab();
			Flower.OrbitalPrefab = Flower.orbitalPrefab;
			Flower.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			Flower.CanBeDropped = false;
		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{
			bool flag = Flower.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/luger", null, true);
				gameObject.name = "Flower";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(12, 12));
				Flower.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				Flower.orbitalPrefab.shouldRotate = false;
				Flower.orbitalPrefab.orbitRadius = 5.5f;
				Flower.orbitalPrefab.orbitDegreesPerSecond = 90f;
				Flower.orbitalPrefab.orbitDegreesPerSecond = 120f;
				Flower.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			GameManager.Instance.StartCoroutine(Wilt());
            

		}

		private IEnumerator Wilt()
		{yield return new WaitForSeconds(20f);
		base.Owner.RemovePassiveItem(this.PickupObjectId);
			yield break;
		}



		protected override void Update()
		{
			bool flag2 = this.m_extantOrbital.transform.position.GetAbsoluteRoom() != null && this.m_extantOrbital.transform.position.GetAbsoluteRoom().HasActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (flag2)
			{
				AIActor aiactor = null;
				float num = float.MaxValue;
				List<AIActor> activeEnemies = this.m_extantOrbital.transform.position.GetAbsoluteRoom().GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor2 = activeEnemies[i];
					bool flag3 = !aiactor2.healthHaver.IsDead;
					bool flag4 = flag3;
					if (flag4)
					{
						float num2 = Vector2.Distance(this.m_extantOrbital.transform.position, aiactor2.CenterPosition);
						bool flag5 = num2 < num;
						bool flag6 = flag5;
						if (flag6)
						{
							num = num2;
							aiactor = aiactor2;
						}
					}
				}
				AIActor aiactor3 = aiactor;
				bool flag7 = aiactor3 != null;
				if (flag7 && !this.onCooldown)
				{ this.onCooldown = true;
					GameManager.Instance.StartCoroutine(StartCooldown());
					if (UnityEngine.Random.value > 0.5f)
					{ this.sign = -1f; }
					else { this.sign = 1f; }
					float value = UnityEngine.Random.value * 10f * sign;
					Projectile projectile = ((Gun)global::ETGMod.Databases.Items[620]).DefaultModule.chargeProjectiles[0].Projectile;
					projectile.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
					GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, this.m_extantOrbital.GetComponent<tk2dSprite>().WorldCenter, Quaternion.Euler(0f, 0f, BraveMathCollege.Atan2Degrees(aiactor3.sprite.WorldCenter - this.m_extantOrbital.GetComponent<tk2dSprite>().WorldCenter) + value), true);
					Projectile component = gameObject.GetComponent<Projectile>();
					component.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
					bool flag8 = component != null;
					if (flag8)
					{
						
							component.AdjustPlayerProjectileTint(Color.blue, 1, 0f);
							component.baseData.damage = 1f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
							component.Owner = this.m_owner;
							component.Shooter = this.m_owner.specRigidbody;
						
					}
				}	}	}
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.1f);
			this.onCooldown = false;
			yield break;
		}







		// Token: 0x0400001B RID: 27
		public static Hook guonHook;

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;
		private bool onCooldown;
		private float sign;

		// Token: 0x0400001D RID: 29
	}
}
