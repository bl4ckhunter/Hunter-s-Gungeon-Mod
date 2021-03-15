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
	public class Satellite : PassiveItem
	{
		private bool onCooldown;
		private bool fired;
		private bool onCooldown1;
		private BeamController beam;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Artillery Marker Bullets";
			string resourceName = "ClassLibrary1/Resources/Laser"; ;
			GameObject gameObject = new GameObject();
			Satellite Satellite = gameObject.AddComponent<Satellite>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Death from above";
			string longDesc = "'BWWWWWWEEEEEEEEEEEOOOOOO'";
			Satellite.SetupItem(shortDesc, longDesc, "ror");
			Satellite.quality = PickupObject.ItemQuality.A;
			Satellite.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Satellite.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }

		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown && !this.onCooldown1)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(this.StartCooldown());
				if (!onCooldown1 && UnityEngine.Random.value < 0.3f)
				{
					this.onCooldown1 = true;
					GameManager.Instance.StartCoroutine(this.HandleFireShortBeam(arg2, base.Owner.CurrentGun.CurrentAngle));
					GameManager.Instance.StartCoroutine(this.StartCooldown1());
				} 
			}
		}
		private IEnumerator HandleFireShortBeam(SpeculativeRigidbody arg2, float targetangle)
		{   
			Projectile projectileToSpawn = ((Gun)global::ETGMod.Databases.Items[515]).DefaultModule.projectiles[0];
			float elapsed = 0f;
			float duration = 3f;
			BeamController beam = this.BeginFiringBeam(projectileToSpawn, base.Owner, targetangle, arg2.sprite.WorldCenter);
			this.beam = beam;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				this.ContinueFiringBeam(beam, base.Owner, targetangle, arg2.sprite.WorldCenter);
				if (arg2.aiActor.healthHaver.GetCurrentHealth() == 0f)
				{ this.CeaseBeam(beam);
				yield break;
				}
				if (arg2.aiActor.healthHaver.IsDead)
				{
					this.CeaseBeam(beam);
					yield break;
				}
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
			bool flag = component != null;
			if (flag)
			{
				HomingModifier homingModifier = component.gameObject.AddComponent<HomingModifier>();
				homingModifier.HomingRadius = 100f;
				homingModifier.AngularVelocity = 600f;
				PierceProjModifier piercer = component.gameObject.AddComponent<PierceProjModifier>();
				piercer.penetration = 10;
				component.Owner = source;
				component.baseData.damage = 0f;
				component.baseData.force = -5f;
			}
            BeamController component2 = gameObject.GetComponent<BeamController>();
			component2.HitsPlayers = false;
			component2.HitsEnemies = true;
			Vector3 v = BraveMathCollege.DegreesToVector(targetAngle, 1f);
			component2.Direction = v;
			component2.Origin = vector;
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
			yield return new WaitForSeconds(0.5f);
			this.onCooldown = false;
			yield break;
		}
		private IEnumerator StartCooldown1()
		{
			yield return new WaitForSeconds(3f);
			this.onCooldown1 = false;
			yield break;
		}
		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile1;
			player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
			player.OnRoomClearEvent += this.Failsafe1;
			player.OnEnteredCombat += this.Failsafe;
		}

		private void Failsafe()
		{
			if (this.beam != null)
			{ this.CeaseBeam(this.beam); }
		}

		private void Failsafe1(PlayerController obj)
		{   if(this.beam != null)
			{ this.CeaseBeam(this.beam); }
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