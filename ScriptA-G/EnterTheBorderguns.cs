using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;
using ClassLibrary1.Scripts;
using Items;
using MultiplayerBasicExample;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class EridianRandomizer : PassiveItem
	{
		private bool onCooldown;
		private FireVolleyOnRollItem m_bomb;
		private List<int> borderguns;
		private GameActorEffect freezeModifierEffect;
		private GameActorFreezeEffect FireModifierEffect;
		private GameActorHealthEffect HealthModifierEffect;
		private List<int> modifiedguns;
		private List<float> fireratemodifiers;
		private List<float> reloadmodifier;
		private List<float> accuracymodifier;
		private List<float> clipmodifier;
		private List<int> elementlist;
		private List<int> modifierlist;
		private Gun modifiedgun;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Eridian Infuser";
			string resourceName = "ClassLibrary1/Resources/Eridium"; ;
			GameObject gameObject = new GameObject();
			EridianRandomizer Soul = gameObject.AddComponent<EridianRandomizer>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Enter the Gunderlands";
			string longDesc = "An ancient artifact from an alternate reality, it inbues your guns with a variety of wondrous effects.";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.B;
			Soul.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
			Soul.CanBeDropped = false;
		}

           

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			modifiedguns = new List<int>();
			modifierlist = new List<int>();
			elementlist = new List<int>();
			fireratemodifiers = new List<float>();
			reloadmodifier = new List<float>();
			accuracymodifier = new List<float>();
			clipmodifier = new List<float>();
			player.PostProcessProjectile += Modifiers;
		}

		private void Modifiers(Projectile projectile, float arg2)
		{ if(base.Owner.CurrentGun != null && !base.Owner.CurrentGun.InfiniteAmmo)
			if (modifiedguns.Contains(base.Owner.CurrentGun.PickupObjectId))
			{
				Projectile projectile1 = projectile.gameObject.GetComponent<Projectile>();
				int index = modifiedguns.IndexOf(base.Owner.CurrentGun.PickupObjectId);
				int Element = elementlist.ElementAt(index);
				int Modifier = modifierlist.ElementAt(index);
				if (Element == 1)
				{
					projectile.gameObject.GetOrAddComponent<SlagProjectile>();
					projectile1.baseData.damage *= 0.4f;
				}
				if (Element == 2)
				{
					projectile.gameObject.GetOrAddComponent<ShockProjectile>();
					projectile1.baseData.damage *= 0.4f;

				}
				if (Element == 3)
				{
					projectile.gameObject.GetOrAddComponent<RadProjectile>();
					projectile1.baseData.damage *= 0.5f;
					projectile.AdjustPlayerProjectileTint(Color.yellow, 5, 0);
				}
				if (Element == 4)
				{
					projectile.gameObject.AddComponent<IcyBullets>();
					projectile1.baseData.damage *= 0.5f;
				}
				if (Element == 5)
				{
					projectile.gameObject.GetOrAddComponent<PoisonModifier>();
					projectile1.baseData.damage *= 0.6f;
				}
				if (Element == 6)
				{
					projectile.gameObject.GetOrAddComponent<FireModifier>();
					projectile1.baseData.damage *= 0.6f;
				}
				if (Element == 7)
				{
					projectile.baseData.damage *= 1.15f;
				}
				if (Element == 8)
				{
					projectile1.baseData.speed *= 0.50f;
					projectile.gameObject.GetOrAddComponent<ExplosiveProjectile>();

				}
				if (Modifier == 4)
				{
					projectile1.baseData.damage *= 1.05f;
				}
				if (Modifier == 1)
				{
					BounceProjModifier bouncer = projectile1.gameObject.GetOrAddComponent<BounceProjModifier>();
					bouncer.bouncesTrackEnemies = true;
					bouncer.bounceTrackRadius = 100f;
					bouncer.ExplodeOnEnemyBounce = true;
					bouncer.numberOfBounces += 1;
					bouncer.damageMultiplierOnBounce *= UnityEngine.Random.Range(0.75f, 1.25f);
				}
				if (Modifier == 2)
				{
					PierceProjModifier piercer = projectile1.gameObject.GetOrAddComponent<PierceProjModifier>();
					piercer.penetration += 1;
					piercer.penetratesBreakables = true;
				}
				if (Modifier == 3)
				{
					HomingModifier piercer = projectile1.gameObject.GetOrAddComponent<HomingModifier>();
					piercer.HomingRadius = 4f;
					piercer.AngularVelocity = 200;
				}
			}
		}

		private void GunChange()
		{   Gun currentgun = new Gun();
			Gun modifiedgun = this.modifiedgun;
			currentgun = base.Owner.CurrentGun;
			if (currentgun != modifiedgun || this.modifiedgun == null)
			{
				this.modifiedgun = base.Owner.CurrentGun;
				foreach (ProjectileModule projectileModule in base.Owner.CurrentGun.Volley.projectiles)
				{
					try
					{
						int index = modifiedguns.IndexOf(base.Owner.CurrentGun.PickupObjectId);
						projectileModule.cooldownTime *= fireratemodifiers.ElementAt(index);
						int clipsize = Convert.ToInt32(Math.Ceiling(base.Owner.CurrentGun.DefaultModule.numberOfShotsInClip * clipmodifier.ElementAt(index)));
						projectileModule.numberOfShotsInClip = clipsize;
						projectileModule.angleVariance *= accuracymodifier.ElementAt(index);
					}
					catch
					{ }
				}
				int index1 = modifiedguns.IndexOf(base.Owner.CurrentGun.PickupObjectId);
				base.Owner.CurrentGun.reloadTime = reloadmodifier.ElementAt(index1);
			}
		}

		protected override void Update()
		{
			if (base.Owner)
			{
				this.Bordergun();
				this.GunChange();
			}
			base.Update();
		}

		private void Bordergun()
		{

			List<Gun> OwnedGuns = base.Owner.inventory.AllGuns;
			List<int> modguns = modifiedguns;
			List<int> elementlist = this.elementlist;
			List<int> modifierlist = this.modifierlist;
			List<float> Fireratemodifiers = this.fireratemodifiers;
			List<float> Reloadmodifer = this.reloadmodifier;
			List<float> Accuracymodifier = this.accuracymodifier;
			List<float> Clipmodifier = this.clipmodifier;

			if (OwnedGuns != null)
			{  
				int count = OwnedGuns.Count;
				for (int i = 0; i < count; i++)
				{
					int Element = UnityEngine.Random.Range(1, 9);
					int Modifier = UnityEngine.Random.Range(1, 5);
					float Firerate = UnityEngine.Random.Range(0.40f, 1.40f);
					float Accuracy = UnityEngine.Random.Range(0.30f, 2.10f);
					float ClipSize = UnityEngine.Random.Range(0.10f, 1.50f);
					float ReloadSpeed = UnityEngine.Random.Range(0.40f, 1.90f);

					if (OwnedGuns[i] != null && OwnedGuns[i].DefaultModule.shootStyle != ProjectileModule.ShootStyle.Beam && !modguns.Contains(OwnedGuns[i].PickupObjectId))
						{
						Fireratemodifiers.Add(Firerate);
						Accuracymodifier.Add(Accuracy);
						Reloadmodifer.Add(ReloadSpeed);
						Clipmodifier.Add(ClipSize);
						elementlist.Add(Element);
						modifierlist.Add(Modifier);
						modguns.Add(OwnedGuns[i].PickupObjectId);

					}
				}
			}
		}


		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= Modifiers;
			return result;
		}



	}
 }     