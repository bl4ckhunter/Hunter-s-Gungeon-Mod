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
	public class LogBullets : PassiveItem
	{
		private bool onCooldown;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Log Bullets";
			string resourceName = "ClassLibrary1/Resources/Soul"; ;
			GameObject gameObject = new GameObject();
			LogBullets Soul = gameObject.AddComponent<LogBullets>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Steal their soul";
			string longDesc = "Reveal projectile components.";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.EXCLUDED;

		}

		private void PostProcessProjectile(Projectile arg1, float arg3)
		{
			LogComponents(arg1.gameObject);
			LogComponents(base.Owner.CurrentGun.gameObject);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

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
		public static void LogComponents(GameObject obj)
		{
			ETGModConsole.Log("---------------------COMPONENTS:-------------------");
			foreach (Component component in obj.GetComponents<Component>())
			{
				ETGModConsole.Log(component.GetType().ToString());
			}
			ETGModConsole.Log("---------------------IN CHILDREN:-------------------");
			foreach (Component component in obj.GetComponentsInChildren<Component>())
			{
				ETGModConsole.Log(component.GetType().ToString());
			}
			ETGModConsole.Log("---------------------IN PARENT:-------------------");
			foreach (Component component in obj.GetComponentsInParent<Component>())
			{
				ETGModConsole.Log(component.GetType().ToString());
			}
			if (obj.GetComponent<FireOnReloadSynergyProcessor>())
			{ ETGModConsole.Log(obj.GetComponent<FireOnReloadSynergyProcessor>().SynergyToCheck.ToString()); }
		}

	}
 }     