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
	public class Boulder : PassiveItem
	{
		private bool onCooldown;
		private GameObject Mines_Cave_In;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Seismic Shells";
			string resourceName = "ClassLibrary1/Resources/Stone"; ;
			GameObject gameObject = new GameObject();
			Boulder Soul = gameObject.AddComponent<Boulder>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Rocks from the blasted sky";
			string longDesc = "These Shells are imbued with the power of the Earth and will occasionally cause earthquakes and rockslides on impact with enemies";
			Soul.SetupItem(shortDesc, longDesc, "ror");
			Soul.quality = PickupObject.ItemQuality.C;
			Soul.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Soul.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				if(UnityEngine.Random.value < 0.16)
				{
					AssetBundle assetBundle2 = ResourceManager.LoadAssetBundle("shared_auto_002");
					this.Mines_Cave_In = assetBundle2.LoadAsset<GameObject>("Mines_Cave_In");
					base.StartCoroutine(this.HandleTriggerRockSlide(base.Owner, Mines_Cave_In, arg2.specRigidbody.UnitCenter));
				}
			}
		}
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.4f);
			this.onCooldown = false;
			yield break;
		}
		private IEnumerator HandleTriggerRockSlide(PlayerController user, GameObject RockSlidePrefab, Vector2 targetPosition)
		{
			RoomHandler currentRoom = user.CurrentRoom;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(RockSlidePrefab, targetPosition, Quaternion.identity);
			HangingObjectController RockSlideController = gameObject.GetComponent<HangingObjectController>();
			RockSlideController.triggerObjectPrefab = null;
			GameObject[] additionalDestroyObjects = new GameObject[]
			{
				RockSlideController.additionalDestroyObjects[1]
			};
			RockSlideController.additionalDestroyObjects = additionalDestroyObjects;
			UnityEngine.Object.Destroy(gameObject.transform.Find("Sign").gameObject);
			RockSlideController.ConfigureOnPlacement(currentRoom);
			yield return new WaitForSeconds(0.01f);
			RockSlideController.Interact(user);
			yield break;
		}
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
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