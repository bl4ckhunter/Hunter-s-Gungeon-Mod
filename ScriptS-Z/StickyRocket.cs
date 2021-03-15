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
	public class StickyRocket : PassiveItem
	{
		private bool onCooldown;
		private GameObject Mines_Cave_In;
		private static SpawnObjectPlayerItem HoleObject;
		private GameObject synergyobject;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Sticky Rocket Bullets";
			string resourceName = "ClassLibrary1/Resources/rocketbullet"; ;
			GameObject gameObject = new GameObject();
			StickyRocket Soul = gameObject.AddComponent<StickyRocket>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Baby You're A Firework";
			string longDesc = "Come on let your colors burst.\n\n" + "Chance to transmogrify enemies into guided rockets.";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.B;
			Soul.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) 
		{ this.PostProcessProjectile(sourceProjectile); }
		private void PostProcessProjectile(Projectile arg1)
		{
			if (!this.onCooldown)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				if (UnityEngine.Random.value < 0.20)
				{
					if (arg1 != null)
					{ arg1.gameObject.GetOrAddComponent<EnemyRocketProjectile>(); }

				}
			}
		}
		

		// Token: 0x0600002E RID: 46 RVA: 0x00004C5C File Offset: 0x00002E5C
		

		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.3f);
			this.onCooldown = false;
			yield break;
		}
		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile1;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile1;
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.PostProcessProjectile -= this.PostProcessProjectile1;
			}
			base.OnDestroy();
		}

	}
 }     