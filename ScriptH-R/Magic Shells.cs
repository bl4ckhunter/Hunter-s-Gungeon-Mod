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

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Mshells : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Magic Shells";
			string resourceName = "ClassLibrary1/Resources/MagicalShells"; ;
			GameObject gameObject = new GameObject();
			Mshells Soul = gameObject.AddComponent<Mshells>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "No Chickens, Just Bullets! ";
			string longDesc = "These highly enchanted shells transmodify enemy bullets into friendly bullets!";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.A;
			Soul.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);

		}

           

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
		}

		private void PostProcessProjectile(Projectile arg1, float arg2)
		{   if (UnityEngine.Random.value < 0.12f)
			{ BulletTransModifier transModifier = arg1.gameObject.AddComponent<BulletTransModifier>();
				transModifier.MaximumBulletsEaten = 5;
			}
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile;
			return result;
		}


	}
 }     