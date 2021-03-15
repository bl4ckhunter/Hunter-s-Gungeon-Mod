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
	public class Dagger : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Monotip Dagger";
			string resourceName = "ClassLibrary1/Resources/Blade"; ;
			GameObject gameObject = new GameObject();
			Dagger dagger = gameObject.AddComponent<Dagger>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Make 'em Bleed";
			string longDesc = "This dagger is so sharp that merely having it in your possession makes you bullets cut enemies.";
			ItemBuilder.AddPassiveStatModifier(dagger, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
			dagger.SetupItem(shortDesc, longDesc, "ror");
			dagger.quality = PickupObject.ItemQuality.B;
			dagger.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown)
			{
				GameManager.Instance.StartCoroutine(Bleed(arg2));
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());

			} 
		}

		private void Sharp(SpeculativeRigidbody arg2)
		{
         arg2.aiActor.healthHaver.ApplyDamage(1f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
		GlobalSparksDoer.DoRadialParticleBurst(50, arg2.specRigidbody.HitboxPixelCollider.UnitBottomLeft, arg2.specRigidbody.HitboxPixelCollider.UnitTopRight, 90f, 2f, 0f, null, null, Color.red, GlobalSparksDoer.SparksType.BLOODY_BLOOD);

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

		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.5f);
			this.onCooldown = false;
			yield break;
		}
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
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