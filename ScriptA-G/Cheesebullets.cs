using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;
using Items;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class CBlast : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Elimental Bullets";
			string resourceName = "ClassLibrary1/Resources/Cheese"; ;
			GameObject gameObject = new GameObject();
			CBlast CBlast = gameObject.AddComponent<CBlast>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "If you can't beat 'em, cheese 'em";
			string longDesc = "Hot fondue, poured directly into the casings. \n" + "The Resourceful Rat's take on the classic wax slug.";
			CBlast.SetupItem(shortDesc, longDesc, "ror");
			CBlast.quality = PickupObject.ItemQuality.B;
			CBlast.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			CBlast.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);


		}

		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[626]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg2.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					component.baseData.damage = 0f;
					component.AdditionalScaleMultiplier = 0.2f;
					component.baseData.force = 0f;
				}
			}
		}
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.4f);
			this.onCooldown = false;
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