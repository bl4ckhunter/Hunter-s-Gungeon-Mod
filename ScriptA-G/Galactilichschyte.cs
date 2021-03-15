using System;
using System.Collections;
using Gungeon;
using ItemAPI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Items
{
	public class Galactilichschyte : GunBehaviour
	{
		// Token: 0x0600019C RID: 412 RVA: 0x00011308 File Offset: 0x0000F508
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Galactilichschyte", "galactilichschyte");
			Game.Items.Rename("outdated_gun_mods:galactilichschyte", "ror:galactilichschyte");
			gun.gameObject.AddComponent<Galactilichschyte>();
			GunExt.SetShortDescription(gun, "Take a Bite");
			GunExt.SetLongDescription(gun, "A kind of guard dog thing from the mushroom kingdom, this odd creature has no trouble shredding gundead now just as easily as it did plumbers then.");
			GunExt.SetupSprite(gun, null, "galactilichschyte_idle_001", 8);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 10);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 10);
			gun.AddProjectileModuleFrom(((Gun)global::ETGMod.Databases.Items[541]) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			//gun.DefaultModule.preventFiringDuringCharge = true;
			//gun.DefaultModule.triggerCooldownForAnyChargeAmount = true;
			gun.DefaultModule.angleVariance = 0f;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = 0;
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(541) as Gun).gunSwitchGroup;
			gun.reloadTime = 0.5f;
			gun.DefaultModule.cooldownTime = 0.7f;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.SetBaseMaxAmmo(150);
			gun.InfiniteAmmo = true;
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
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
			Projectile projectile = Object.Instantiate<Projectile>(((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.chargeProjectiles[0].Projectile);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage *= 2.75f;
			projectile.transform.parent = gun.barrelOffset;
			projectile.baseData.range = 8f;
			projectile.AdditionalScaleMultiplier = 7f;
			Shader shader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			Material material = new Material(Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass"));
			material.name = "HologramMaterial";
			Material material2 = material;
			material.SetTexture("_MainTex", gun.sprite.renderer.material.GetTexture("_MainTex"));
			material2.SetTexture("_MainTex", gun.sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
			material.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
			gun.barrelOffset.localPosition = new Vector3(3f, 2.1f, 0f);
			gun.sprite.renderer.material.shader = shader;
			gun.sprite.renderer.material = material;
			gun.sprite.renderer.sharedMaterial = material2;
			gun.sprite.usesOverrideMaterial = true;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Galactilichschyte.Shytheid = gun.PickupObjectId;

		}

		public override void PostProcessProjectile(Projectile projectile)
		{			
			BounceProjModifier bounceProjModifier = projectile.gameObject.AddComponent<BounceProjModifier>();
			PierceProjModifier pierceProjModifier = projectile.gameObject.AddComponent<PierceProjModifier>();
			pierceProjModifier.penetration = 100;
			AkSoundEngine.PostEvent("Play_WPN_sword_impact_01", base.gameObject);
			bounceProjModifier.projectile.specRigidbody.CollideWithTileMap = false;
			base.PostProcessProjectile(projectile);
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			this.Owner = gun.CurrentOwner;
			gun.PreventNormalFireAudio = true;
		}

		protected void Update()
		{
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
		public static int Shytheid;
	}
}
