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
	public class WallPierce : PassiveItem
	{
		private bool onCooldown;
		private Vector2 aimpoint;
		private float m_currentAngle;
		private float m_currentDistance;
		private static bool bol;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Anti-Materiel Ammo";
			string resourceName = "ClassLibrary1/Resources/prof"; ;
			GameObject gameObject = new GameObject();
			WallPierce Soul = gameObject.AddComponent<WallPierce>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Aim true";
			string longDesc = "Designed with accuracy in mind these projectiles will always hit exactly the center of your crosshair regardless of what's between you and your target, not closer and not further.";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.B;
			ItemBuilder.AddPassiveStatModifier(Soul, PlayerStats.StatType.Accuracy, 0.01f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			Soul.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			List<string> mandatoryConsoleIDs = new List<string> {
			"ror:anti-materiel_ammo"
			};
			List<string> oprionalConsoleIDs = new List<string> {
			"bouncy_bullets",
			"rube_adyne_prototype",
			"rube_adyne_mk2"
			};
			CustomSynergies.Add("Phantom Ricochet", mandatoryConsoleIDs, oprionalConsoleIDs, true);
		}
		private void PostProcessProjectile(Projectile arg1, float arg3)
		{
            arg1.specRigidbody.CollideWithTileMap = false;
			PlayerController player = base.Owner;
				if (BraveInput.GetInstanceForPlayer(player.PlayerIDX).IsKeyboardAndMouse(false))
				{
					this.aimpoint = player.unadjustedAimPoint.XY();
				}
				else
				{
					BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(player.PlayerIDX);
					Vector2 a2 = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
					a2 += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
					this.m_currentAngle = BraveMathCollege.Atan2Degrees(a2 - player.CenterPosition);
					this.m_currentDistance = Vector2.Distance(a2, player.CenterPosition);
					this.m_currentDistance = Mathf.Min(this.m_currentDistance, 15);
					this.aimpoint = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
				}
			arg1.baseData.range = Vector2.Distance(this.aimpoint, player.CenterPosition);
			if (arg1.gameObject.GetComponent<BounceProjModifier>() || base.Owner.HasPassiveItem(288))
			{
				arg1.OnDestruction += Rebound;
				if (arg1.gameObject.GetComponent<BounceProjModifier>() && arg1.gameObject.GetComponent<BounceProjModifier>().numberOfBounces > 3)
				{
					arg1.OnDestruction += Rebound;
				}
			}
			
		}

		private void Rebound(Projectile obj)
		{ 
			GameObject gameObject = SpawnManager.SpawnProjectile(obj.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 180 + base.Owner.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-10,10)), true);

			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag = component != null;
			if (flag)
			{
				component.baseData.range *= 1.75f;
				component.Owner = base.Owner;
				component.OnDestruction += Rebound1;

			}
		}

		private void Rebound1(Projectile obj)
		{
			GameObject gameObject = SpawnManager.SpawnProjectile(obj.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-15, 15)), true);

			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag = component != null;
			if (flag)
			{
				component.baseData.range *= 1.3f;
				component.Owner = base.Owner;
				component.OnDestruction += Rebound2;


			}
		}
		private void Rebound2(Projectile obj)
		{
			GameObject gameObject = SpawnManager.SpawnProjectile(obj.gameObject, obj.specRigidbody.UnitCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : 180 + base.Owner.CurrentGun.CurrentAngle + UnityEngine.Random.Range(-20,20)), true);

			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag = component != null;
			if (flag)
			{
				component.Owner = base.Owner;
			}
		}



		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile;
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.PostProcessProjectile -= this.PostProcessProjectile;
			}
			base.OnDestroy();
		}

	}
 }     