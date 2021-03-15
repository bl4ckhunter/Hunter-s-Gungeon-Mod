using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Items
{
	public class Chomp : GunBehaviour
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00011308 File Offset: 0x0000F508
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Chomp", "chomp");
			Game.Items.Rename("outdated_gun_mods:chomp", "ror:chomp");
			gun.gameObject.AddComponent<Chomp>();
			GunExt.SetShortDescription(gun, "Take a Bite");
			GunExt.SetLongDescription(gun, "A kind of guard dog thing from the mushroom kingdom, this odd creature has no trouble shredding gundead now just as easily as it did plumbers then.");
			GunExt.SetupSprite(gun, null, "chomp_idle_001", 8);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 15);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 15);
			GunExt.AddProjectileModuleFrom(gun, "38_special", true, false);
			gun.DefaultModule.ammoCost = 1;
			//gun.DefaultModule.preventFiringDuringCharge = true;
			//gun.DefaultModule.triggerCooldownForAnyChargeAmount = true;
			gun.DefaultModule.angleVariance = 0f;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = 0;
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(539) as Gun).gunSwitchGroup;
			gun.reloadTime = 0.5f;
			gun.DefaultModule.cooldownTime = 0.7f;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.SetBaseMaxAmmo(150);
			gun.InfiniteAmmo = true;
			gun.quality = PickupObject.ItemQuality.C;
			//gun.encounterTrackable.EncounterGuid = "ChainChomp";
			/*
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.chargeProjectiles[0].Projectile);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.chargeProjectiles[0].Projectile = projectile;
			gun.barrelOffset.localPosition += new Vector3(0f, 0f, 0f);
			projectile.transform.parent = gun.barrelOffset;
			*/
			Projectile projectile = Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage *= 2.75f;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.range *= 0.07f;
			projectile.sprite.renderer.enabled = false;
			projectile.hitEffects.suppressMidairDeathVfx = true;
			projectile.baseData.speed *= 2f;
			projectile.AdditionalScaleMultiplier = 2.2f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			gun.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
		}

		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.gameObject.AddComponent<BleedGoop>();
			projectile.baseData.force = 0f;
			projectile.baseData.speed *= 1.3f;
			projectile.AdditionalScaleMultiplier = 2f;
			BounceProjModifier bounceProjModifier = projectile.gameObject.AddComponent<BounceProjModifier>();
			PierceProjModifier pierceProjModifier = projectile.gameObject.AddComponent<PierceProjModifier>();
			pierceProjModifier.penetration = 10;
			bounceProjModifier.projectile.specRigidbody.CollideWithTileMap = false;
			base.PostProcessProjectile(projectile);
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			this.Owner = gun.CurrentOwner;
			gun.PreventNormalFireAudio = true;
			AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
		}

		protected void Update()
		{
			PlayerController playerController = this.gun.CurrentOwner as PlayerController;
			bool flag = playerController.CurrentGun == this.gun && playerController.CurrentRoom != null && playerController.CurrentRoom.GetActiveEnemies(0) != null;
			if (flag)
			{
				bool flag2 = Chomp.fleeData == null || Chomp.fleeData.Player != playerController;
				bool flag3 = flag2;
				if (flag3)
				{
					Chomp.fleeData = new FleePlayerData();
					Chomp.fleeData.Player = playerController;
					Chomp.fleeData.StartDistance = 100f;
				}
				foreach (AIActor aiactor in playerController.CurrentRoom.GetActiveEnemies(0))
				{
					bool flag4 = Vector2.Distance(aiactor.CenterPosition, this.gun.barrelOffset.position) < 10f;
					if (flag4)
					{
						bool flag5 = aiactor.behaviorSpeculator != null;
						bool flag6 = flag5;
						if (flag6)
						{
							aiactor.behaviorSpeculator.FleePlayerData = Chomp.fleeData;
							aiactor.gameActor.ApplyEffect(this.EnemyDebuff, 1f, null);
							GameManager.Instance.StartCoroutine(Chomp.RemoveFear(aiactor));
						}
					}
				}
			}
			bool flag7 = this.gun.CurrentOwner;
			if (flag7)
			{
				bool flag8 = !this.gun.PreventNormalFireAudio;
				if (flag8)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				bool flag9 = !this.gun.IsReloading && !this.HasReloaded;
				if (flag9)
				{
					this.HasReloaded = true;
				}
			}
		}

		private static IEnumerator RemoveFear(AIActor aiactor)
		{
			yield return new WaitForSeconds(3f);
			aiactor.behaviorSpeculator.FleePlayerData = null;
			yield break;
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

		public float trackingSpeed;

		public float trackingTime;

		[CurveRange(0f, 0f, 1f, 1f)]
		public AnimationCurve trackingCurve;

		private bool HasReloaded;

		public AIActorDebuffEffect EnemyDebuff = new AIActorDebuffEffect
		{
			SpeedMultiplier = 0.3f,
			KeepHealthPercentage = true,
			duration = 3f
		};

		public float ActivationChance;

		public bool TriggersRadialBulletBurst;

		[ShowInInspectorIf("TriggersRadialBulletBurst", false)]
		public RadialBurstInterface RadialBurstSettings;

		private GameActor Owner;

		private static FleePlayerData fleeData;
	}
}
