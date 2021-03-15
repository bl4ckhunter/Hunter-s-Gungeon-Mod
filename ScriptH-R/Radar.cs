using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;

namespace Mod
{
    //Call this method from the Start() method of your ETGModule extension
    public class Radar : PassiveItem
    {
        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Radar";
            string resourceName = "ClassLibrary1/Resources/Radar_Scanner"; ;
            GameObject gameObject = new GameObject();
            Radar Radar = gameObject.AddComponent<Radar>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Scanning....";
            string longDesc = "Killing a boss reveals the map \n" + "A portable radar system, maps the floor but you need a boss room pedestal for it to work properly.";
            Radar.SetupItem(shortDesc, longDesc, "ror");
            Radar.quality = PickupObject.ItemQuality.D;
            Radar.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
        }
		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{
			bool flag = enemy.aiActor && enemy.IsBoss && fatal;
			if (flag)
			{
				LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(137).gameObject, base.Owner);
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000F6CF File Offset: 0x0000D8CF
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000F6FC File Offset: 0x0000D8FC
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			return result;
		}
        protected override void OnDestroy()
        {
            PlayerController player = base.Owner;
            if (base.Owner)
            {
                player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            }
            base.OnDestroy();
        }
    }
}



