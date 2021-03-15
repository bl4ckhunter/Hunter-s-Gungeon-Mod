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
	public class Hammer : PassiveItem
	{
		private bool onCooldown;
		private float extraDamage;
		private float extraBossDamage;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Hammering Justice";
			string resourceName = "ClassLibrary1/Resources/Hammer"; ;
			GameObject gameObject = new GameObject();
			Hammer Hammer = gameObject.AddComponent<Hammer>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Everything looks like a nail...";
			string longDesc = "...when all you have is a GIANT HAMMER! \n" + "Hits deal 10% of enemies' missing health as additional damage (1% of bosses')";
			Hammer.SetupItem(shortDesc, longDesc, "ror");
			Hammer.quality = PickupObject.ItemQuality.A;
			Hammer.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Hammer.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[13]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					component.Owner = base.Owner;
					component.AdjustPlayerProjectileTint(Color.red.WithAlpha(Color.red.a / 2f), 5, 0f);
					component.Shooter = base.Owner.specRigidbody;
					component.AdditionalScaleMultiplier = 2f;
					component.baseData.damage = this.extraDamage;
					component.baseData.speed = 30f;
					component.SetOwnerSafe(base.Owner, "Player");
				}
			}
		}
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.35f);
			this.onCooldown = false;
			yield break;
		}
		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			player.PostProcessProjectile += this.PostProcessProjectile1;
			player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
		}

		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{   
			float maxhp = enemy.GetMaxHealth();
			float hp = enemy.GetCurrentHealth();
			float num = (maxhp - hp) * 0.1f ;
			this.extraDamage = num;
			if(enemy.healthHaver.IsBoss)
			this.extraDamage = num * 0.1f;
			;
			
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile1;
			player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.PostProcessProjectile -= this.PostProcessProjectile1;
				player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
				player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			}
			base.OnDestroy();
		}

	}
 }     