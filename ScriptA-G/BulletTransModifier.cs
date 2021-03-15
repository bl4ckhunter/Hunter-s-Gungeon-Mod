using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1.Scripts
{
	using System;
	using UnityEngine;

	// Token: 0x020013BD RID: 5053
	public class BulletTransModifier : MonoBehaviour
	{
		// Token: 0x06007289 RID: 29321 RVA: 0x002C9530 File Offset: 0x002C7730
		public BulletTransModifier()
		{
			this.DamagePercentGainPerSnack = 0.25f;
			this.MaxMultiplier = 3f;
			this.HungryRadius = 3f;
			this.MaximumBulletsEaten = 1;
			this.Multiplier = 1f;
		}

		// Token: 0x0600728A RID: 29322 RVA: 0x002C9564 File Offset: 0x002C7764
		private void Awake()
		{
			this.m_projectile = base.GetComponent<Projectile>();
			this.m_projectile.collidesWithProjectiles = true;
			SpeculativeRigidbody specRigidbody = this.m_projectile.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
			if (this.m_projectile.gameObject.GetComponent<HungryProjectileModifier>())
			{
				Destroy(this.m_projectile.gameObject.GetComponent<HungryProjectileModifier>());
				Multiplier = 3f;
			}
		}

		// Token: 0x0600728B RID: 29323 RVA: 0x002C95DC File Offset: 0x002C77DC
		private void Update()
		{
			if (this.m_sated)
			{
				return;
			}
			Vector2 b = this.m_projectile.transform.position.XY();
			for (int i = 0; i < StaticReferenceManager.AllProjectiles.Count; i++)
			{
				Projectile projectile = StaticReferenceManager.AllProjectiles[i];
				if (projectile && projectile.Owner is AIActor)
				{
					float sqrMagnitude = (projectile.transform.position.XY() - b).sqrMagnitude;
					if (sqrMagnitude < this.HungryRadius)
					{
						this.EatBullet(projectile);
					}
				}
			}
		}

		// Token: 0x0600728C RID: 29324 RVA: 0x002C9680 File Offset: 0x002C7880
		private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			if (this.m_sated)
			{
				return;
			}
			if (otherRigidbody && otherRigidbody.projectile)
			{
				if (otherRigidbody.projectile.Owner is AIActor)
				{
					this.EatBullet(otherRigidbody.projectile);
				}
				PhysicsEngine.SkipCollision = true;
			}
		}

		// Token: 0x0600728D RID: 29325 RVA: 0x002C96DC File Offset: 0x002C78DC
		private void EatBullet(Projectile other)
		{
			if (this.m_sated)
			{
				return;
			}
			other.DieInAir(false, true, true, false);
			Gun randomGun;
			int pickupObjectId;
			do
			{
				randomGun = PickupObjectDatabase.GetRandomGun();
				pickupObjectId = randomGun.PickupObjectId;
			}
			while (randomGun.HasShootStyle(ProjectileModule.ShootStyle.Beam));
			Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[pickupObjectId]).DefaultModule.projectiles[0];
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, m_projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (m_projectile.Owner.CurrentGun == null) ? 0f : m_projectile.Owner.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag4 = component != null;
			if (flag4)
			{
				component.Owner = m_projectile.Owner;
				component.baseData.damage *= this.Multiplier;
				component.Shooter = m_projectile.specRigidbody;
				HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
				component.AdjustPlayerProjectileTint(Color.magenta.WithAlpha(Color.magenta.a / 2f), 5, 0f);
				homingModifier.HomingRadius = 100f;
				homingModifier.AngularVelocity = 500f;
			}
			float num = Mathf.Min(this.MaxMultiplier, 1f + (float)this.m_numberOfBulletsEaten * this.DamagePercentGainPerSnack);
			this.m_numberOfBulletsEaten++;
			float num2 = Mathf.Min(this.MaxMultiplier, 1f + (float)this.m_numberOfBulletsEaten * this.DamagePercentGainPerSnack);
			float b = num2 / num;
			float num3 = Mathf.Max(1f, b);
			if (num3 > 1f)
			{
				this.m_projectile.RuntimeUpdateScale(num3);
				this.m_projectile.baseData.damage *= num3;
			}
			if (this.m_numberOfBulletsEaten >= this.MaximumBulletsEaten)
			{
				this.m_sated = true;
				this.m_projectile.AdjustPlayerProjectileTint(this.m_projectile.DefaultTintColor, 3, 0f);
			}
		}

		// Token: 0x040073DA RID: 29658
		public float DamagePercentGainPerSnack;

		// Token: 0x040073DB RID: 29659
		public float MaxMultiplier;

		// Token: 0x040073DC RID: 29660
		public float HungryRadius;

		// Token: 0x040073DD RID: 29661
		public int MaximumBulletsEaten;

		// Token: 0x040073DE RID: 29662
		private Projectile m_projectile;

		// Token: 0x040073DF RID: 29663
		private int m_numberOfBulletsEaten;

		// Token: 0x040073E0 RID: 29664
		private bool m_sated;
		private float Multiplier;
	}
}