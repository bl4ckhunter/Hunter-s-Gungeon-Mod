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

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Rshells : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Rad Rounds";
			string resourceName = "ClassLibrary1/Resources/Radshell"; ;
			GameObject gameObject = new GameObject();
			Rshells Soul = gameObject.AddComponent<Rshells>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Radioactive Afterglow";
			string longDesc = "These highly radioactive bullets have a chance to irradiate enemies, eventually melting them into a pool of radioactive goo";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.B;
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
		{   
			if (UnityEngine.Random.value < 0.15f)
			{
				RadProjectile transModifier = arg1.gameObject.AddComponent<RadProjectile>();
				arg1.AdjustPlayerProjectileTint(Color.yellow, 100, 0);
			}
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