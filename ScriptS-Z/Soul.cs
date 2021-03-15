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
	public class Soul : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Soulbound Bullets";
			string resourceName = "ClassLibrary1/Resources/Soul"; ;
			GameObject gameObject = new GameObject();
			Soul Soul = gameObject.AddComponent<Soul>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Steal their soul";
			string longDesc = "You can hear the wailing of the jammed every time you fire one of these.";
			ItemBuilder.AddPassiveStatModifier(Soul, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.A;
			Soul.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[761]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + 180f), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					component.baseData.damage = 0f;
					component.baseData.speed = 0f;
					component.baseData.force = 0f;
					component.SetOwnerSafe(base.Owner, "Player");
					component.Shooter = base.Owner.specRigidbody;
				}
			}
		}
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(1f);
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