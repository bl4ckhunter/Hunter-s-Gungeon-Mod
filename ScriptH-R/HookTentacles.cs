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
	public class Satel : PassiveItem
	{
		private Vector2 position;
		private bool onCooldown;
		private bool fired;
		private bool onCooldown1;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Poisonous Tentacle Shells";
			string resourceName = "ClassLibrary1/Resources/Tendril"; ;
			GameObject gameObject = new GameObject();
			Satel Satellite = gameObject.AddComponent<Satel>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "We all know where this is going";
			string longDesc = "You don't have any idea from which creature the tentacles in these shells came from, how on earth Goopton managed to cram them in there or even why the hell would he do such a thing and trust me, you don't want to find out either.\n\n" + "Though this species is entirely harmless in terms of direct damage it is however poisonous.";
			Satellite.SetupItem(shortDesc, longDesc, "ror");
			Satellite.quality = PickupObject.ItemQuality.C;
			Satellite.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Satellite.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		

		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown && !this.onCooldown1)
			{
				this.position = arg2.sprite.WorldCenter;
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(this.StartCooldown());
				if (!onCooldown1 && UnityEngine.Random.value < 0.25f)
				{
					this.onCooldown1 = true;
					GameManager.Instance.StartCoroutine(this.HandleFireShortBeam(arg2, base.Owner.CurrentGun.CurrentAngle + 72f));
					GameManager.Instance.StartCoroutine(this.HandleFireShortBeam(arg2, base.Owner.CurrentGun.CurrentAngle + 72f + 72f + 72f +72f));
					GameManager.Instance.StartCoroutine(this.StartCooldown1());
				} 
			}
			

		}
		private IEnumerator HandleFireShortBeam(SpeculativeRigidbody arg2, float targetangle)
		{   
			Projectile projectileToSpawn = ((Gun)global::ETGMod.Databases.Items[87]).DefaultModule.projectiles[0];
			float elapsed = 0f;
			float duration = 7f;
			projectileToSpawn.AdditionalScaleMultiplier = 0.3f;
			BeamController beam = this.BeginFiringBeam(projectileToSpawn, base.Owner, targetangle, arg2.sprite.WorldCenter);
			beam.AdjustPlayerBeamTint(Color.cyan, 15, 0f);
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				this.ContinueFiringBeam(beam, base.Owner, targetangle, this.position);

				yield return null;
			}
			this.CeaseBeam(beam);
			yield break;
		}

		private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
		{
			Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
			GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
			Projectile component = gameObject.GetComponent<Projectile>();
			component.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.cyan.a / 2f), 15, 0f);
			component.collidesWithEnemies = false;
			component.baseData.damage = 0f;
			component.AppliesPoison = true;
			component.AppliesSpeedModifier = true;
			component.AdditionalScaleMultiplier = 0.3f;
			component.baseData.force = -40;
            BeamController component2 = gameObject.GetComponent<BeamController>();
			component2.HitsPlayers = false;
			component2.HitsEnemies = true;
			Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
			component2.AdjustPlayerBeamTint(Color.cyan, 15, 0f);
			component2.Direction = v;
			component2.Origin = vector;
			component2.chargeDelay = 0f;
			component2.ChanceBasedHomingRadius += 200f;
			component2.ChanceBasedHomingAngularVelocity += 600f;

			return component2;
		}
		private void ContinueFiringBeam(BeamController beam, PlayerController source, float angle, Vector2? overrideSpawnPoint)
		{
			Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
			beam.Direction = BraveMathCollege.DegreesToVector(angle, 1f);
			beam.Origin = vector;
			beam.LateUpdatePosition(vector);
		}

		private void CeaseBeam(BeamController beam)
		{
			beam.CeaseAttack();
			this.fired = true;
		}









		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.6f);
			this.onCooldown = false;
			yield break;
		}
		private IEnumerator StartCooldown1()
		{
			yield return new WaitForSeconds(15f);
			this.onCooldown1 = false;
			yield break;
		}
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