using System;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Collections.Generic;
using System.Linq;

namespace Mod
{
	public class Heresy : PassiveItem
	{

		public static void Init()
		{
			string name = "Heretic Visions";
			string resourceName = "ClassLibrary1/Resources/Heresy"; ;
			GameObject gameObject = new GameObject();
			Heresy Heresy = gameObject.AddComponent<Heresy>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Twisted Reality";
			string longDesc = "A glimpse into a warped world, distorted by curse. Despair not, for in the gungeon not even the most horrible mirage can survive a bullet and the lord of the jammed can't reach you here, no matter how much curse you acquire.";
			ItemBuilder.AddPassiveStatModifier(Heresy, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
			Heresy.SetupItem(shortDesc, longDesc, "ror");
			Heresy.quality = PickupObject.ItemQuality.D;
			Heresy.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			arg2.aiActor.UnbecomeBlackPhantom();
		}

		protected override void Update()
		{
			base.Update();
			if (base.Owner)
			{
				GameManager.Instance.Dungeon.CurseReaperActive = true;
				this.Jam();
			}

		}

		private void Jam()
		{
			List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			if (activeEnemies != null)
			{
				foreach (AIActor ai in activeEnemies)
				{
					if (ai != null)
					{
						float hp = ai.healthHaver.GetCurrentHealth();
						float maxhp = ai.healthHaver.GetMaxHealth();
						bool flag = hp == maxhp;
						if (flag && ai != null)
						{ ai.BecomeBlackPhantom(); }
					}

				};
			}
		}
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }

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