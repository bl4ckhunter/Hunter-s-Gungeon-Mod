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
    public class Armageddon : PassiveItem
    {
        private GameObject Nuke;

        // Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
        public static void Init()
        {
            string name = "Armageddon";
            string resourceName = "ClassLibrary1/Resources/arrow"; ;
            GameObject gameObject = new GameObject();
            Armageddon Armageddon = gameObject.AddComponent<Armageddon>();
            ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
            string shortDesc = "Scanning....";
            string longDesc = "Killing a boss reveals the map \n" + "A portable Armageddon system, maps the floor but you need a boss room pedestal for it to work properly.";
            Armageddon.SetupItem(shortDesc, longDesc, "ror");
            Armageddon.quality = PickupObject.ItemQuality.D;
            Armageddon.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
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
            player.OnTableFlipCompleted += Armagedon;
		}

        private void Armagedon(FlippableCover obj)
        {
            StartCoroutine(RoRItems.TowerRockSlide(base.Owner));
            StartCoroutine(Nukestrike(base.Owner));
        }

        private IEnumerator Nukestrike(PlayerController owner)
        {
            float elapsed = 0f;
            while (elapsed < 9f)
            {
                yield return new WaitForSeconds(0.6f);
                elapsed += 0.6f;
                Vector3 intVector = new Vector3(GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(2, 2).x, GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(2, 2).y, base.Owner.transform.position.z);
                Nukestrike(intVector);
                yield return null;
            }
            yield break;
        }

        private void Nukestrike(Vector3 arg)
        {
            AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
            GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
            DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
            goopManagerForGoopType.TimedAddGoopCircle(arg, 5f, 0.1f, false);
            GameObject Nuke = assetBundle.LoadAsset<GameObject>("assets/data/vfx prefabs/impact vfx/vfx_explosion_nuke.prefab");
            GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(Nuke, arg, Quaternion.identity);
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



