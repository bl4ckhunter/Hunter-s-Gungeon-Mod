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
	public class VoidBul : PassiveItem
	{
		private bool onCooldown;
		private GameObject Mines_Cave_In;
		private static SpawnObjectPlayerItem HoleObject;
		private GameObject synergyobject;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Abyssal Vortex";
			string resourceName = "ClassLibrary1/Resources/vortex"; ;
			GameObject gameObject = new GameObject();
			VoidBul Soul = gameObject.AddComponent<VoidBul>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Shatter Reality";
			string longDesc = "The very gunshot the Mine Flayer used to part the Curtain, frozen in spacetime. Your bullets echo with its power. (chance to tear holes in reality, holes collapse after 5s and explode.)";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.S;
			ItemBuilder.AddPassiveStatModifier(Soul, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
			Soul.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				if (UnityEngine.Random.value < 0.15)
				{
					this.Cuuube(arg1);

				}
			}
		}
		private void Cuuube(Projectile obj)
		{
			VoidBul.HoleObject = PickupObjectDatabase.GetById(155).GetComponent<SpawnObjectPlayerItem>();
			VoidBul hellcomponent = gameObject.GetComponent<VoidBul>();
			Vector2 vector = new Vector2(obj.specRigidbody.UnitCenter.x, obj.specRigidbody.UnitCenter.y);
			Vector3 vector3 = new Vector3(vector.x, vector.y, 0f);
			hellcomponent.synergyobject = VoidBul.HoleObject.objectToSpawn;
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
				yield return null;
			}

			if (gameObject1)
			{
				Destroy(gameObject1);
			}
			base.Owner.ForceBlank(5, overrideCenter: obj);
			List<AIActor> activeEnemies1 = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies1 != null)
			{
				int count = activeEnemies1.Count;
				for (int i = 0; i < count; i++)
				{
					if (activeEnemies1[i] && activeEnemies1[i].HasBeenEngaged && activeEnemies1[i].healthHaver && !activeEnemies1[i].healthHaver.IsDead)
					{
						activeEnemies1[i].healthHaver.ApplyDamage(35 * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage), base.Owner.CenterPosition, "cube", CoreDamageTypes.None, DamageCategory.Normal, false, null, true);
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

		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.2f);
			this.onCooldown = false;
			yield break;
		}
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
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