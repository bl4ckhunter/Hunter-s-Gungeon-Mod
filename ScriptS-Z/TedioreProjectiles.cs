using Mod;
using System;
using System.Collections;
using UnityEngine;

namespace Items
{
	
	internal class TediorePistolProjectile : MonoBehaviour
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
		public void Start()
		{
			{

				this.projectile = base.GetComponent<Projectile>();
				this.player = (this.projectile.Owner as PlayerController);
				Projectile projectile = this.projectile;
				PlayerController man = projectile.Owner as PlayerController;
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					component.baseData.damage = 5f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
					component.SetOwnerSafe(man, "Player");
					component.projectile.SetProjectileSpriteRight("TPistol_projectile_001", 35, 19, null, null);
					component.Shooter = man.specRigidbody;
					component.baseData.force = 70f;
					Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
					ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
					ExplosiveModifier exploder = projectile.gameObject.AddComponent<ExplosiveModifier>();
					exploder.explosionData = boomer.explosionData;
					exploder.explosionData.damage = 40f;
					exploder.explosionData.damageRadius = 4f;
				}
			}
		}
        private Projectile projectile;

		// Token: 0x04000048 RID: 72
		private PlayerController player;
	}

	internal class TedioreLauncherProjectile : MonoBehaviour
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
		public void Start()
		{
			{

				this.projectile = base.GetComponent<Projectile>();
				this.player = (this.projectile.Owner as PlayerController);
				Projectile projectile = this.projectile;
				PlayerController man = projectile.Owner as PlayerController;
				Projectile component = gameObject.GetComponent<Projectile>();
				GameManager.Instance.StartCoroutine(ChainBoom(projectile));
				bool flag4 = component != null;
				if (flag4)
				{
					component.SetProjectileSpriteRight("tedioreRocket_projectile_001", 27, 11, null, null);
					component.baseData.damage = 25f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
					component.SetOwnerSafe(man, "Player");
					component.Shooter = man.specRigidbody;
					component.baseData.force = 70f;
					component.baseData.speed = 10f;
					component.pierceMinorBreakables = true;
					BounceProjModifier bouncer = component.gameObject.GetOrAddComponent<BounceProjModifier>();
					bouncer.bounceTrackRadius = 1000;
					bouncer.bouncesTrackEnemies = true;
					bouncer.numberOfBounces = 4;
					Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[372]).DefaultModule.projectiles[0];
					ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
					ExplosiveModifier exploder = projectile.gameObject.GetOrAddComponent<ExplosiveModifier>();
					exploder.explosionData = boomer.explosionData;
					exploder.explosionData.damage = 30f;
					exploder.explosionData.damageRadius = 3f;

				}
			}
		}

		private IEnumerator ChainBoom(Projectile projectile)
		{
			PlayerController man = projectile.Owner as PlayerController;
			while (projectile != null)
			{
				yield return new WaitForSeconds(0.25f);
			    Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
				ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
				boomer.explosionData.doScreenShake = false;
				boomer.explosionData.damageRadius = 4.5f;
				boomer.explosionData.doExplosionRing = true;
				boomer.explosionData.damageToPlayer = 0f;
				boomer.explosionData.force = 90f;
				boomer.explosionData.preventPlayerForce = true;
				boomer.explosionData.doDestroyProjectiles = false;
				boomer.explosionData.pushRadius = boomer.explosionData.damageRadius;
				boomer.explosionData.damage = 35f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
				boomer.IgnoreQueues = true;
				Exploder.Explode(projectile.specRigidbody.UnitCenter, boomer.explosionData, Vector2.zero, null, false, CoreDamageTypes.None, false);
				yield return null;
			}
			yield break;
		}

		// Token: 0x0600792E RID: 31022 RVA: 0x002F7899 File Offset: 0x002F5A99

		
		private bool charged;
		private float charge;
		private GameObject Nuke;
		private bool onCooldown;
		private Projectile projectile;

		// Token: 0x04000048 RID: 72
		private PlayerController player;
	}
	internal class TedioreSMGProjectile : MonoBehaviour
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
		public void Start()
		{
			{

				this.projectile = base.GetComponent<Projectile>();
				this.player = (this.projectile.Owner as PlayerController);
				Projectile projectile = this.projectile;
				PlayerController man = projectile.Owner as PlayerController;
				Projectile component = gameObject.GetComponent<Projectile>();
				GameManager.Instance.StartCoroutine(ChainShot(projectile));
				bool flag4 = component != null;
				if (flag4)
				{
					component.SetProjectileSpriteRight("TedioreSMG_projectile_001", 32, 14, null, null);
					component.baseData.damage = 5f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
					component.SetOwnerSafe(man, "Player");
					component.Shooter = man.specRigidbody;
					HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 5000f;
					component.baseData.force = 200f;
					component.baseData.speed = 8f;
					Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[129]).DefaultModule.projectiles[0];
					ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
					ExplosiveModifier exploder = projectile.gameObject.GetOrAddComponent<ExplosiveModifier>();
					exploder.explosionData = boomer.explosionData;
					exploder.explosionData.damage = 20f;
					exploder.explosionData.damageRadius = 3f;

				}
			}
		}

		private IEnumerator ChainShot(Projectile projectile)
		{ float shot = 0f;
			while (projectile != null && shot < 30)
			{
				yield return new WaitForSeconds(0.12f);
				AkSoundEngine.PostEvent("Play_WPN_magnum_shot_01", base.gameObject);
				shot += 1f;
				Projectile projectile1 = ((Gun)ETGMod.Databases.Items[685]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile1.gameObject, projectile.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (projectile.Owner.CurrentGun == null) ? 0f : projectile.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag = component != null;
				if (flag)
				{
					component.Owner = projectile.Owner;
					HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
					homingModifier.HomingRadius = 100f;
					homingModifier.AngularVelocity = 5000f;
					component.Shooter = projectile.Owner.specRigidbody;
					component.baseData.speed = 20f;
					component.SetOwnerSafe(projectile.Owner, "Player");
				}
				yield return null;
				
			}
			if(projectile !=null && shot >= 30)
			{ projectile.baseData.speed = 20;}

			yield break;
		}

		// Token: 0x0600792E RID: 31022 RVA: 0x002F7899 File Offset: 0x002F5A99


		private bool charged;
		private float charge;
		private GameObject Nuke;
		private bool onCooldown;
		private Projectile projectile;

		// Token: 0x04000048 RID: 72
		private PlayerController player;
	}
	internal class TedioreShotgunProjectile : MonoBehaviour
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00006D7C File Offset: 0x00004F7C
		public void Start()
		{
			{

				this.projectile = base.GetComponent<Projectile>();
				this.player = (this.projectile.Owner as PlayerController);
				Projectile projectile = this.projectile;
				PlayerController man = projectile.Owner as PlayerController;
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					component.baseData.damage = 5f * man.stats.GetStatValue(PlayerStats.StatType.Damage);
					component.SetOwnerSafe(man, "Player");
					component.Shooter = man.specRigidbody;
					component.SetProjectileSpriteRight("Tshotgun_projectile_001", 35, 19, null, null);
					component.baseData.force = 70f;
					Projectile Boomprojectile = ((Gun)ETGMod.Databases.Items[593]).DefaultModule.projectiles[0];
					ExplosiveModifier boomer = Boomprojectile.GetComponent<ExplosiveModifier>();
					ExplosiveModifier exploder = projectile.gameObject.AddComponent<ExplosiveModifier>();
					exploder.explosionData = boomer.explosionData;
					exploder.explosionData.damage = 0f;
					exploder.explosionData.force = 0f;
					exploder.explosionData.damageRadius = 4f;
					component.OnDestruction += Flak;
				}
			}


		}
		private void Flak(Projectile obj)
		{


			PlayerController man = (this.projectile.Owner as PlayerController);
			TedioreShotgunProjectile.m_bomb = PickupObjectDatabase.GetById(567).GetComponent<FireVolleyOnRollItem>();
			ProjectileModule projectilemod = TedioreShotgunProjectile.m_bomb.ModVolley.projectiles[0];
			Projectile projectile = projectilemod.GetCurrentProjectile();
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 0f), true);
			GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 180f), true);
			GameObject gameObject4 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 270f), true);
			GameObject gameObject5 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 45f), true);
			GameObject gameObject6 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 135f), true);
			GameObject gameObject7 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 225f), true);
			GameObject gameObject8 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 315f), true);
			GameObject gameObject9 = SpawnManager.SpawnProjectile(projectile.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (man.CurrentGun == null) ? 0f : 90f), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			Projectile component9 = gameObject9.GetComponent<Projectile>();
			Projectile component3 = gameObject3.GetComponent<Projectile>();
			Projectile component4 = gameObject4.GetComponent<Projectile>();
			Projectile component5 = gameObject5.GetComponent<Projectile>();
			Projectile component6 = gameObject6.GetComponent<Projectile>();
			Projectile component7 = gameObject7.GetComponent<Projectile>();
			Projectile component2 = gameObject8.GetComponent<Projectile>();
			bool flag = component != null;
			if (flag)
			{
				component.Owner = man;
				component.Shooter = man.specRigidbody;
				component.baseData.range = 3f;
				component.baseData.damage = 1f;
				component.baseData.force = 0f;
				component.baseData.speed = 3f;
				component.SetOwnerSafe(man, "Player");
				component.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
			bool flag2 = component != null;
			if (flag2)
			{
				component2.Owner = man;
				component2.Shooter = man.specRigidbody;
				component2.baseData.range = 3f;
				component2.baseData.damage = 1f;
				component2.baseData.force = 0f;
								component.baseData.speed = 3f;
				component2.SetOwnerSafe(man, "Player");
				component2.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component2.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component2.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
			bool flag3 = component != null;
			if (flag3)
			{
				component3.Owner = man;
				component3.Shooter = man.specRigidbody;
				component3.baseData.range = 3f;
				component3.baseData.damage = 1f;
				component3.baseData.force = 0f;
				component3.baseData.speed = 3f;
				component3.SetOwnerSafe(man, "Player");
				component3.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component3.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component3.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
			bool flag4 = component != null;
			if (flag4)
			{
				component4.Owner = man;
				component4.Shooter = man.specRigidbody;
				component4.baseData.range = 3f;
				component4.baseData.force = 0f;
				component4.baseData.damage = 1f;
				component3.baseData.speed = 3f;
				component4.SetOwnerSafe(man, "Player");
				component4.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component4.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component4.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
			bool flag5 = component != null;
			if (flag5)
			{
				component5.Owner = man;
				component5.Shooter = man.specRigidbody;
				component5.baseData.range = 3f;
				component5.baseData.damage = 1f;
				component5.baseData.force = 0f;
				component3.baseData.speed = 3f;
				component5.SetOwnerSafe(man, "Player");
				component5.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component5.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component5.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
			bool flag6 = component != null;
			if (flag6)
			{
				component6.Owner = man;
				component6.Shooter = man.specRigidbody;
				component6.baseData.range = 3f;
				component6.baseData.damage = 1f;
				component6.baseData.force = 0f;
				component3.baseData.speed = 3f;
				component6.SetOwnerSafe(man, "Player");
				component6.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component6.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component6.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
			bool flag7 = component != null;
			if (flag7)
			{
				component7.Owner = man;
				component7.Shooter = man.specRigidbody;
				component7.baseData.range = 3f;
				component7.baseData.force = 0f;
				component7.baseData.damage = 1f;
				component3.baseData.speed = 3f;
				component7.SetOwnerSafe(man, "Player");
				component7.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component7.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component7.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
			bool flag9 = component != null;
			if (flag9)
			{
				component9.Owner = man;
				component9.Shooter = man.specRigidbody;
				component9.baseData.range = 3f;
				component9.baseData.damage = 1f;
				component9.baseData.force = 0f;
				component3.baseData.speed = 3f;
				component9.SetOwnerSafe(man, "Player");
				component9.ignoreDamageCaps = false;
				BounceProjModifier bouncer = component9.gameObject.AddComponent<BounceProjModifier>();
				PierceProjModifier piercer = component9.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				bouncer.projectile.specRigidbody.CollideWithTileMap = false;
			}
		}
		private Projectile projectile;

		// Token: 0x04000048 RID: 72
		private PlayerController player;
		private static FireVolleyOnRollItem m_bomb;
	}


}

