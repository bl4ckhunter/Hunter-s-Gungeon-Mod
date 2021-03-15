using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;
using System.Xml.Serialization;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Tentacle : PassiveItem
	{
		private bool onCooldown3;
		private bool fired;
		private bool onCooldown1;
		private float beaninstances;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Tentacle Shells";
			string resourceName = "ClassLibrary1/Resources/tentacle"; ;
			GameObject gameObject = new GameObject();
			Tentacle Tentacle = gameObject.AddComponent<Tentacle>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "We all know where this is going";
			string longDesc = "You don't have any idea from which creature the tentacles in these shells came from, how on earth Goopton managed to cram them in there or even why the hell would he do such a thing and trust me, you don't want to find out either. \n\n" + "This species is as bloodthirsty as it gets but on the brigth side the bulletkin won't need therapy after it's done with it and you'll dodge potential lawsuits, on the account of, you know, there being no witnesses";
			Tentacle.SetupItem(shortDesc, longDesc, "ror");
			Tentacle.quality = PickupObject.ItemQuality.B;
			Tentacle.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Tentacle.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Tentacle.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}

		private void Beam(float damage, bool fatal, HealthHaver arg)
		{
			SpeculativeRigidbody arg2 = arg.healthHaver.specRigidbody;
			if (!this.onCooldown3 && !this.onCooldown1)
			{
				GameManager.Instance.StartCoroutine(this.StartCooldown());
				if (!onCooldown1 && UnityEngine.Random.value < 0.11f && this.beaninstances < 3f)
				{
					this.onCooldown1 = true;
					GameManager.Instance.StartCoroutine(this.HandleFireShortBeam(arg2, base.Owner.CurrentGun.CurrentAngle));
					this.beaninstances += 1f;
					GameManager.Instance.StartCoroutine(this.StartCooldown1());
				} 
			}
			
		}
		private IEnumerator HandleFireShortBeam(SpeculativeRigidbody arg2, float targetangle)
		{   
			Projectile projectileToSpawn = ((Gun)global::ETGMod.Databases.Items[474]).DefaultModule.projectiles[0];
			float duration = 3f;
			float elapsed = 0f;
			BeamController beam = this.BeginFiringBeam(projectileToSpawn, base.Owner, targetangle, arg2.specRigidbody.UnitCenter);
			while (elapsed < duration && arg2.UnitCenter != null)
			{   

				elapsed += BraveTime.DeltaTime;
				this.ContinueFiringBeam(beam, base.Owner, targetangle, base.Owner.specRigidbody.UnitTopCenter);
				yield return null;
			}
			this.CeaseBeam(beam);
			yield break;
		}
		private void Sharp(SpeculativeRigidbody arg2)
		{
			arg2.aiActor.healthHaver.ApplyDamage(3f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);

		}

		private IEnumerator Bleed(SpeculativeRigidbody arg2)
		{
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield break;

		}

		private BeamController BeginFiringBeam(Projectile projectileToSpawn, PlayerController source, float targetAngle, Vector2? overrideSpawnPoint)
		{
			Vector2 vector = (overrideSpawnPoint == null) ? source.CenterPosition : overrideSpawnPoint.Value;
			GameObject gameObject = SpawnManager.SpawnProjectile(projectileToSpawn.gameObject, vector, Quaternion.identity, true);
			Projectile component = gameObject.GetComponent<Projectile>();
			component.Owner = source;
			BeamController component2 = gameObject.GetComponent<BeamController>();
			component2.Owner = source;
			component2.HitsPlayers = false;
			component2.HitsEnemies = true;
			component2.DamageModifier = 0.35f;
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
			this.beaninstances -= 1f;
		}








		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.7f);
			this.onCooldown3 = false;
			yield break;
		}

		private IEnumerator StartCooldown1()
		{
			yield return new WaitForSeconds(5f);
			this.onCooldown1 = false;
			yield break;
		}
		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.Beam));
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.Beam));
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.Beam));
			}
			base.OnDestroy();
		}
	}
 }     