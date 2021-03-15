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
	public class Atom : PassiveItem
	{

		public GoopDefinition goopDefinition;
		public ExplosionData strikeExplosionData;

		public PlayerController m_currentUser;

		public string TransmogrifyTargetGuid;


		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Atomic Behemoth";
			string resourceName = "ClassLibrary1/Resources/Nuclear_behemot"; ;
			GameObject gameObject = new GameObject();
			Atom Atom = gameObject.AddComponent<Atom>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "It's the only way to be sure";
			string longDesc = "An ancient cannon retrofitted to fire nuclear warheads, shoots automatically once charged.";
			ItemBuilder.AddPassiveStatModifier(Atom, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
			Atom.SetupItem(shortDesc, longDesc, "ror");
			Atom.quality = PickupObject.ItemQuality.A;
			Atom.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Atom.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f); 
			Atom.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

		}
		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{

				this.charge += damage;
				bool flag = this.charge > 450f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
				if (flag)
				{
					this.charged = true;
					this.charge = 0f;

				}
			
		}

		private IEnumerator OnCooldown()
		{
			yield return new WaitForSeconds(7f);
			this.onCooldown = false;
			yield break;
		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{    if(this.charged && !this.onCooldown)

			{ this.onCooldown = true;
					GameManager.Instance.StartCoroutine(this.OnCooldown());
					this.charged = false;
				this.m_currentUser = base.Owner;
				Vector2 worldCenter = arg2.sprite.WorldCenter;
				Vector3 target = arg2.sprite.WorldBottomCenter;
				Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
				projectile2.AdjustPlayerProjectileTint(Color.green.WithAlpha(Color.yellow.a / 1f), 5, 0f);
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					component.baseData.damage = 0f;
					component.baseData.speed = 100f;
					component.AdditionalScaleMultiplier = 2.5f;
					component.SetOwnerSafe(base.Owner, "Player");
					component.Shooter = base.Owner.specRigidbody;
					component.Owner = base.Owner;
				}
				this.Boom(target);
				base.Owner.ForceBlank(3f, 0.5f, false, true, worldCenter, false, -1f);
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
				DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
				goopManagerForGoopType.TimedAddGoopCircle(arg2.sprite.WorldCenter, 5f, 0.1f, false);
				this.Nuke = assetBundle.LoadAsset<GameObject>("assets/data/vfx prefabs/impact vfx/vfx_explosion_nuke.prefab");
				GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(this.Nuke);
				gameObject1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(arg2.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.LowerCenter);
				gameObject1.transform.position = gameObject.transform.position.Quantize(0.0625f);
				gameObject1.GetComponent<tk2dBaseSprite>().UpdateZDepth();



				List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				if (activeEnemies != null)
				{
					int count = activeEnemies.Count;
					for (int i = 0; i < count; i++)
					{
						if (activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss && !activeEnemies[i].IsTransmogrified)
						{
							activeEnemies[i].Transmogrify(EnemyDatabase.GetOrLoadByGuid("d4a9836f8ab14f3fadd0f597438b1f1f"), null);
						}
					}
				}

			}
		}
		public void Boom(Vector3 position)
		{
			ExplosionData defaultSmallExplosionData2 = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
			this.smallPlayerSafeExplosion.effect = defaultSmallExplosionData2.effect;
			this.smallPlayerSafeExplosion.ignoreList = defaultSmallExplosionData2.ignoreList;
			this.smallPlayerSafeExplosion.ss = defaultSmallExplosionData2.ss;
			Exploder.Explode(position, this.smallPlayerSafeExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
		}

		// Token: 0x0600792E RID: 31022 RVA: 0x002F7899 File Offset: 0x002F5A99

		private ExplosionData smallPlayerSafeExplosion = new ExplosionData
		{
			damageRadius = 6.5f,
			damageToPlayer = 0f,
			doDamage = true,
			damage = 150f,
			doExplosionRing = true,
			doDestroyProjectiles = true,
			doForce = true,
			debrisForce = 30f,
			preventPlayerForce = true,
			explosionDelay = 0f,
			usesComprehensiveDelay = false,
			doScreenShake = false,
			playDefaultSFX = true
		};
		private bool charged;
		private float charge;
		private GameObject Nuke;
		private bool onCooldown;

		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			player.PostProcessProjectile += this.PostProcessProjectile1;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile1;
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.PostProcessProjectile -= this.PostProcessProjectile1;
				player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			}
			base.OnDestroy();
		}




	}
 }     