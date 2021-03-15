using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Mod
{
	public class Katanagun : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Nodachi", "Katanagun");
			Game.Items.Rename("outdated_gun_mods:nodachi", "ror:nodachi");
			gun.gameObject.AddComponent<Katanagun>();
			GunExt.SetShortDescription(gun, "Swift Cut");
			GunExt.SetLongDescription(gun, "Dash towards the cursor and cut every enemy in your path.");
			GunExt.SetupSprite(gun, null, "Katanagun_idle_001", 0);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 45);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 15);
			GunExt.AddProjectileModuleFrom(gun, (Gun)ETGMod.Databases.Items[541], true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.angleVariance = 0f;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.MovesPlayerForwardOnChargeFire = true;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0.5f;
			gun.DefaultModule.preventFiringDuringCharge = true;
			gun.DefaultModule.cooldownTime = 0.5f;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.muzzleFlashEffects = null;
			gun.SetBaseMaxAmmo(120);
			gun.InfiniteAmmo = true;
			gun.quality = PickupObject.ItemQuality.D;
			//gun.encounterTrackable.EncounterGuid = "KatanaGunBlade";
			ProjectileModule.ChargeProjectile item = new ProjectileModule.ChargeProjectile();
			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				item
			};
			gun.DefaultModule.chargeProjectiles[0].ChargeTime = 2f;
			gun.DefaultModule.chargeProjectiles[0].UsedProperties = ((Gun)ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].UsedProperties;
			gun.DefaultModule.chargeProjectiles[0].VfxPool = ((Gun)ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].VfxPool;
			gun.DefaultModule.chargeProjectiles[0].VfxPool.type = ((Gun)ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].VfxPool.type;
			ProjectileModule.ChargeProjectile chargeProjectile = gun.DefaultModule.chargeProjectiles[0];
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(((Gun)ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].Projectile);
			chargeProjectile.Projectile = projectile;
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.barrelOffset.localPosition = new Vector3(gun.sprite.WorldBottomLeft.x + 2.34f, gun.sprite.WorldBottomLeft.y + 2.34f, gun.barrelOffset.position.z);
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.speed *= 2f;
			projectile.AdditionalScaleMultiplier = 2f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Katanagun.Katanid = gun.PickupObjectId;
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			base.PostProcessProjectile(projectile);
			PlayerController user = this.gun.CurrentOwner as PlayerController;
			PlayerController playerController = this.gun.CurrentOwner as PlayerController;
			bool flag = BraveInput.GetInstanceForPlayer(playerController.PlayerIDX).IsKeyboardAndMouse(true);
			this.aimpoint = playerController.unadjustedAimPoint.XY();
			if (flag)
			{
				this.dashDistance = Vector2.Distance(this.aimpoint, playerController.CenterPosition);
			}
			else
			{
				this.dashDistance = 10f;
			}
			this.Effect(user);
			AkSoundEngine.PostEvent("Play_WPN_blasphemy_shot_01", base.gameObject);
		}

		private void Effect(PlayerController user)
		{
			this.downwellAfterimage = user.gameObject.AddComponent<AfterImageTrailController>();
			this.downwellAfterimage.spawnShadows = true;
			this.downwellAfterimage.shadowTimeDelay = 0.0005f;
			this.downwellAfterimage.shadowLifetime = 0.3f;
			this.downwellAfterimage.minTranslation = 0.05f;
			this.downwellAfterimage.dashColor = Color.cyan.WithAlpha(Color.cyan.a / 5f);
			this.downwellAfterimage.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
			bool isDashing = this.m_isDashing;
			bool flag = !isDashing;
			if (flag)
			{
				Vector2 vector = this.aimpoint - user.specRigidbody.UnitCenter;
				bool flag2 = vector.x == 0f && vector.y == 0f;
				bool flag3 = !flag2;
				if (flag3)
				{
					base.StartCoroutine(this.HandleDash(user, vector));
				}
			}
		}

		private IEnumerator HandleDash(PlayerController user, Vector2 dashDirection)
		{
			this.m_isDashing = true;
			bool flag = this.poofVFX != null;
			bool flag4 = flag;
			if (flag4)
			{
				user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
			}
			Vector2 startPosition = user.sprite.WorldCenter;
			this.actorsPassed.Clear();
			this.breakablesPassed.Clear();
			user.IsVisible = true;
			user.SetInputOverride("katana");
			user.healthHaver.IsVulnerable = false;
			user.FallingProhibited = true;
			PixelCollider playerHitbox = user.specRigidbody.HitboxPixelCollider;
			playerHitbox.CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			SpeculativeRigidbody specRigidbody = user.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.KatanaPreCollision));
			float duration = Mathf.Max(0.0001f, this.dashDistance / this.dashSpeed);
			float elapsed = -BraveTime.DeltaTime;
			while (elapsed < duration)
			{
				user.healthHaver.IsVulnerable = false;
				elapsed += BraveTime.DeltaTime;
				float adjSpeed = Mathf.Min(this.dashSpeed, this.dashDistance / BraveTime.DeltaTime);
				user.specRigidbody.Velocity = dashDirection.normalized * adjSpeed;
				yield return null;
			}
			base.transform.localPosition = new Vector3(-0.3f, -0.4f, 0f);
			base.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
			bool flag2 = this.poofVFX != null;
			bool flag5 = flag2;
			if (flag5)
			{
				user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
			}
			base.StartCoroutine(this.EndAndDamage(new List<AIActor>(this.actorsPassed), new List<MajorBreakable>(this.breakablesPassed), user, dashDirection, startPosition, user.sprite.WorldCenter));
			bool flag3 = this.finalDelay > 0f;
			bool flag6 = flag3;
			if (flag6)
			{
				user.healthHaver.IsVulnerable = false;
				yield return new WaitForSeconds(this.finalDelay);
			}
			playerHitbox.CollisionLayerCollidableOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
			specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.KatanaPreCollision));
			user.FallingProhibited = false;
			user.ClearInputOverride("katana");
			this.m_isDashing = false;
			yield return new WaitForSeconds(0.6f);
			user.healthHaver.IsVulnerable = true;
			this.downwellAfterimage.spawnShadows = false;
			yield break;
		}

		private IEnumerator EndAndDamage(List<AIActor> actors, List<MajorBreakable> breakables, PlayerController user, Vector2 dashDirection, Vector2 startPosition, Vector2 endPosition)
		{
			yield return new WaitForSeconds(this.finalDelay);
			Exploder.DoLinearPush(user.sprite.WorldCenter, startPosition, 13f, 5f);
			int num;
			for (int i = 0; i < actors.Count; i = num + 1)
			{
				bool flag = actors[i];
				bool flag3 = flag;
				if (flag3)
				{
					actors[i].healthHaver.ApplyDamage(this.collisionDamage, dashDirection, "Katana", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
					this.Zap(actors[i]);
				}
				num = i;
			}
			for (int j = 0; j < breakables.Count; j = num + 1)
			{
				bool flag2 = breakables[j];
				bool flag4 = flag2;
				if (flag4)
				{
					breakables[j].ApplyDamage(100f, dashDirection, false, false, false);
				}
				num = j;
			}
			yield break;
		}

		private void Zap(AIActor ai)
		{
			PlayerController playerController = this.gun.CurrentOwner as PlayerController;
			Projectile projectile = ((Gun)ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].Projectile;
			projectile.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, ai.CenterPosition, Quaternion.Euler(0f, 0f, (this.gun == null) ? 0f : this.gun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag = component != null;
			bool flag2 = flag;
			if (flag2)
			{
				component.baseData.damage = 100f * playerController.stats.GetStatValue(PlayerStats.StatType.Damage);
				component.SetOwnerSafe(playerController, "Player");
				component.Shooter = playerController.specRigidbody;
				component.baseData.force = 0f;
				this.teleporter = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>();
				Vector2 centerPosition = ai.CenterPosition;
				bool flag3 = true;
				if (flag3)
				{
					UnityEngine.Object.Instantiate<GameObject>(this.teleporter.TelefragVFXPrefab, ai.CenterPosition, Quaternion.identity);
					UnityEngine.Object.Instantiate<GameObject>(this.teleporter.TelefragVFXPrefab, ai.CenterPosition, Quaternion.identity);
					UnityEngine.Object.Instantiate<GameObject>(this.teleporter.TelefragVFXPrefab, ai.CenterPosition, Quaternion.identity);
				}
			}
		}

		private void KatanaPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			bool flag = otherRigidbody.projectile != null;
			bool flag2 = flag;
			if (flag2)
			{
				PhysicsEngine.SkipCollision = true;
			}
			bool flag3 = otherRigidbody.aiActor != null;
			bool flag4 = flag3;
			if (flag4)
			{
				PhysicsEngine.SkipCollision = true;
				bool flag5 = !this.actorsPassed.Contains(otherRigidbody.aiActor);
				bool flag6 = flag5;
				if (flag6)
				{
					otherRigidbody.aiActor.DelayActions(1f);
					this.actorsPassed.Add(otherRigidbody.aiActor);
				}
			}
			bool flag7 = otherRigidbody.majorBreakable != null;
			bool flag8 = flag7;
			if (flag8)
			{
				PhysicsEngine.SkipCollision = true;
				bool flag9 = !this.breakablesPassed.Contains(otherRigidbody.majorBreakable);
				bool flag10 = flag9;
				if (flag10)
				{
					this.breakablesPassed.Add(otherRigidbody.majorBreakable);
				}
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			this.Owner = gun.CurrentOwner;
			gun.PreventNormalFireAudio = true;
		}

		protected void Update()
		{
			bool flag = this.gun.CurrentOwner;
			if (flag)
			{
				PlayerController playerController = this.gun.CurrentOwner as PlayerController;
				bool flag2 = playerController.HasPassiveItem(815) && !playerController.HasPickupID(Game.Items["ror:red_flamberge"].PickupObjectId);
				if (flag2)
				{
					playerController.inventory.AddGunToInventory(Game.Items["ror:red_flamberge"] as Gun, true);
				}
				bool flag3 = !this.gun.PreventNormalFireAudio;
				if (flag3)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag4 = !this.gun.IsReloading && !this.HasReloaded;
				if (flag4)
				{
					this.HasReloaded = true;
					this.unused = false;
				}
			}
		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			bool flag = gun.IsReloading && this.HasReloaded;
			if (flag)
			{
				this.HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
			}
		}

		public float dashDistance;

		public float dashSpeed = 170f;

		public float collisionDamage = 20f;

		public float finalDelay = 0f;

		public GameObject poofVFX;

		private bool m_isDashing;

		private List<AIActor> actorsPassed = new List<AIActor>();

		private List<MajorBreakable> breakablesPassed = new List<MajorBreakable>();

		public float trackingSpeed;

		public float trackingTime;

		[CurveRange(0f, 0f, 1f, 1f)]
		public AnimationCurve trackingCurve;

		private bool HasReloaded;

		public float ActivationChance;

		public bool TriggersRadialBulletBurst;

		[ShowInInspectorIf("TriggersRadialBulletBurst", false)]
		public RadialBurstInterface RadialBurstSettings;


		private GameActor Owner;


		private bool unused;

		private Vector2 aimpoint;

		private TeleporterPrototypeItem teleporter;

		private AfterImageTrailController downwellAfterimage;

		public static int Katanid;
	}
}
