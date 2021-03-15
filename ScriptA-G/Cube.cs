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
	public class Pcube : IounStoneOrbitalItem
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "Primordial Cube";
			string resourcePath = "ClassLibrary1/Resources/PrimordialCube1";
			GameObject gameObject = new GameObject(name);
			Pcube Pcube = gameObject.AddComponent<Pcube>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Blank Origin";
			string longDesc = "Blanking opens a portal to hell that sucks enemies in and explodes for massive damage after 5s" + "An unmarked artifact of unknown origin. Attempts to breach it's surface through mechanical means have been proved unsuccessful but it seems to show reactions to energy signatures";
			ItemBuilder.SetupItem(Pcube, shortDesc, longDesc, "ror");
			Pcube.quality = PickupObject.ItemQuality.A;
			ItemBuilder.AddPassiveStatModifier(Pcube, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);
			Pcube.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Pcube.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Pcube.BuildPrefab();
			Pcube.OrbitalPrefab = Pcube.orbitalPrefab;
			Pcube.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			Pcube.onCooldown = false;
		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{
			bool flag = Pcube.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/PrimordialCube1", null, true);
				gameObject.name = "Pcube";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(13, 17));
				Pcube.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				Pcube.orbitalPrefab.shouldRotate = false;
				Pcube.orbitalPrefab.orbitRadius = 2.5f;
				Pcube.orbitalPrefab.orbitDegreesPerSecond = 90f;
				Pcube.orbitalPrefab.orbitDegreesPerSecond = 120f;
				Pcube.orbitalPrefab.SetOrbitalTier(0);
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
				player.OnUsedBlank += Fire;


			}
		}

		private void Fire(PlayerController player, int one)
		{ 
				Projectile projectile = ((Gun)ETGMod.Databases.Items[130]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.Owner.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag1 = component != null;
				if (flag1)
				{
					component.Owner = base.Owner;
					component.Shooter = base.Owner.specRigidbody;
					component.baseData.speed = 5f;
					component.AdjustPlayerProjectileTint(Color.HSVToRGB(0.8003f, 1, 1.3f), 5, 0f);
					component.SetOwnerSafe(base.Owner, "Player");
					component.baseData.damage = 6f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
					component.AdditionalScaleMultiplier = 1f;
					component.baseData.range = 6f;
					BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
				    GoopModifier gooper = component.gameObject.GetComponent<GoopModifier>();
					PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
					Destroy(piercer);
				    Destroy(gooper);
					Destroy(bouncer);

				component.OnDestruction += Cuuube;
				}
		
		}

		private void Cuuube(Projectile obj)
		{
			Pcube.HoleObject = PickupObjectDatabase.GetById(155).GetComponent<SpawnObjectPlayerItem>();
			Pcube hellcomponent = gameObject.GetComponent<Pcube>();
			Vector2 vector = new Vector2(obj.specRigidbody.UnitCenter.x, obj.specRigidbody.UnitCenter.y);
			Vector3 vector3 = new Vector3(vector.x, vector.y, 0f);
			hellcomponent.synergyobject = Pcube.HoleObject.objectToSpawn;
			BlackHoleDoer holer = synergyobject.GetComponent<BlackHoleDoer>();
			GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(holer.HellSynergyVFX, new Vector3(obj.transform.position.x, obj.transform.position.y, base.transform.position.z + 5f), Quaternion.Euler(0f, 0f, 0f)); ;
			MeshRenderer component = gameObject1.GetComponent<MeshRenderer>();
			base.StartCoroutine(this.HoldPortalOpen(component, vector, gameObject1));
		}

		private IEnumerator HoldPortalOpen(MeshRenderer portal, Vector2 obj, GameObject gameObject1)
		{
			portal.material.SetFloat("_UVDistCutoff", 0f);
			float elapsed = 0f;
			float duration = 5f;
			float t = 0f;
			while (elapsed < duration)
			{
				try
				{
					elapsed += BraveTime.DeltaTime;
					t = Mathf.Clamp01(elapsed / 0.25f);
					portal.material.SetFloat("_UVDistCutoff", Mathf.Lerp(0f, 0.21f, t));
					if (base.Owner.CurrentRoom != null)
					{
						List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
						if (activeEnemies != null)
						{
							int count = activeEnemies.Count;
							for (int i = 0; i < count; i++)
							{
								if (activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss)
								{
									try
									{
										Vector2 dir = (Vector2)(obj - activeEnemies[i].specRigidbody.UnitCenter);
										activeEnemies[i].knockbackDoer.ApplyKnockback(dir, 1f);
										try
										{
											if (activeEnemies[i].gameObject.GetComponent<ExplodeOnDeath>())
											{
												Destroy(activeEnemies[i].gameObject.GetComponent<ExplodeOnDeath>());
											}
										}
										catch
										{ }
										if (Vector2.Distance(obj, activeEnemies[i].specRigidbody.UnitCenter) < 2.5f)
										{
											GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(activeEnemies[i], obj));
											activeEnemies[i].EraseFromExistenceWithRewards(true);
										}
									}
									catch
									{ }
								}
							}
						}
					}
				}
				catch{ }
				yield return null;
			}

			if (gameObject1) {
				Destroy(gameObject1); }
				base.Owner.ForceBlank(overrideCenter:obj);
			List<AIActor> activeEnemies1 = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies1 != null)
			{
				int count = activeEnemies1.Count;
				for (int i = 0; i < count; i++)
				{
					if (activeEnemies1[i] && activeEnemies1[i].HasBeenEngaged && activeEnemies1[i].healthHaver && !activeEnemies1[i].healthHaver.IsDead)
					{
						activeEnemies1[i].healthHaver.ApplyDamage(150 + (activeEnemies1[i].healthHaver.GetCurrentHealth()/3), base.Owner.CenterPosition, "cube", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
					}
				}
			}
			yield break;
		}

		private IEnumerator HandleEnemySuck(AIActor target, Vector2 hole)
		{
			Transform copySprite = this.CreateEmptySprite(target);
			target.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false);
			target.EraseFromExistenceWithRewards(false);
			Vector3 startPosition = copySprite.transform.position;
			float elapsed = 0f;
			float duration = 1f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				bool flag1 = copySprite;
				if (flag1)
				{
					Vector3 position = hole;
					float t = elapsed / duration * (elapsed / duration);
					copySprite.position = Vector3.Lerp(startPosition, position, t);
					copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
					copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
					position = default(Vector3);
				}
				yield return null;
			}
			bool flag = copySprite;
			if (flag)
			{ UnityEngine.Object.Destroy(copySprite.gameObject); }
			yield break;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00004C5C File Offset: 0x00002E5C
		private Transform CreateEmptySprite(AIActor target)
		{
			GameObject gameObject = new GameObject("suck image");
			gameObject.layer = target.gameObject.layer;
			tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
			gameObject.transform.parent = SpawnManager.Instance.VFX;
			tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
			tk2dSprite.transform.position = target.sprite.transform.position;
			GameObject gameObject2 = new GameObject("image parent");
			gameObject2.transform.position = tk2dSprite.WorldCenter;
			tk2dSprite.transform.parent = gameObject2.transform;
			bool flag = target.optionalPalette != null;
			if (flag)
			{
				tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
			}
			return gameObject2.transform;
		}



		// Token: 0x0400001B RID: 27
		public static Hook guonHook;

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;

		// Token: 0x0400001D RID: 29
		private bool onCooldown;
		private GameObject HellSynergyVFX;
		private static BlinkPassiveItem m_BlinkPassive;
		private static SpawnObjectPlayerItem HoleObject;
		private GameObject synergyobject;
		private Material m_distortMaterial;
	}
}
