using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;
using ClassLibrary1;
using System.Runtime.CompilerServices;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Abyss : PassiveItem
	{
		private bool onCooldown;
		private PlayerController m_buffedTarget;
		private StatModifier m_temporaryModifier;
		private GameObject TentacleVFX;
		private bool m_radialIndicatorActive;
		private HeatIndicatorController m_radialIndicator;
		private bool active;
		private bool active1;
		private bool unreloaded;
		private bool flag;
		private bool used;
		private float radius;
		private bool spapi;
		private bool error;
		private bool added;

		public object CheckBoost { get; private set; }
		public object LastCheckBoost { get; private set; }
		public float Boost { get; private set; }
		public float ColorBoost { get; private set; }
		public float TrueColorBoost { get; private set; }


		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Hungering Chamber";
			string resourceName = "ClassLibrary1/Resources/MimicChamber"; ;
			GameObject gameObject = new GameObject();
			Abyss Abyss = gameObject.AddComponent<Abyss>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Reloading sucks!";
			string longDesc = "A mimic gun in its larval stage, when your gun's magazine is empty this little critter is more than happy to jump inside your gun's chamber and suck in nearby gundead as you reload, sometimes it'll even excrete a tiny bit of casings, synergizes with all things big.";
			ItemBuilder.AddPassiveStatModifier(Abyss, PlayerStats.StatType.ReloadSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			Abyss.SetupItem(shortDesc, longDesc, "ror");
			Abyss.quality = PickupObject.ItemQuality.B;
			Abyss.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Abyss.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Abyss.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			
				List<string> mandatoryConsoleIDs = new List<string> {
			"ror:hungering_chamber"
			};
				List<string> oprionalConsoleIDs = new List<string> {
			"big_boy",
			"big_shotgun"
			};
				CustomSynergies.Add("The Big Succ", mandatoryConsoleIDs, oprionalConsoleIDs, true);
		
			


		}

		private void PostProcessProjectile(PlayerController player, Gun gun)
		{
			if (base.Owner.CurrentGun.IsEmpty && !base.Owner.CurrentGun.InfiniteAmmo)
			{
				player.CurrentRoom.ApplyActionToNearbyEnemies(player.CenterPosition, this.radius, new Action<AIActor, float>(this.ProcessEnemy));
			}
		}
		private void ProcessEnemy(AIActor activeEnemies, float distance)
		{
			for (int i = 0; i < 6; i++)
			{

				if (activeEnemies && activeEnemies.HasBeenEngaged && activeEnemies.healthHaver && activeEnemies.IsNormalEnemy && !activeEnemies.healthHaver.IsDead && !activeEnemies.healthHaver.IsBoss)
				{
					GameManager.Instance.Dungeon.StartCoroutine(this.HandleEnemySuck(activeEnemies));
					activeEnemies.EraseFromExistenceWithRewards(true);
					break;
				}
			}
		}





		private IEnumerator HandleEnemySuck(AIActor target)
		{
			Transform copySprite = this.CreateEmptySprite(target);
			target.healthHaver.ApplyDamage(100000f, Vector2.zero, "Telefrag", CoreDamageTypes.Void, DamageCategory.Normal, true, null, false);
			target.EraseFromExistenceWithRewards(false);
			Vector3 startPosition = copySprite.transform.position;
			float elapsed = 0f;
			float duration = 0.5f;
			while (elapsed < duration)
			{
				elapsed += BraveTime.DeltaTime;
				bool flag1 = copySprite;
				if (flag1)
				{
					Vector3 position = base.Owner.CurrentGun.PrimaryHandAttachPoint.position;
					float t = elapsed / duration * (elapsed / duration);
					copySprite.position = Vector3.Lerp(startPosition, position, t);
					copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
					copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
					position = default(Vector3);
				}
				yield return null;
			}
			bool flag = copySprite;
			if (flag)
			{ UnityEngine.Object.Destroy(copySprite.gameObject); }
			if (UnityEngine.Random.value < 0.50f)
			{ LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(68).gameObject, base.Owner); }

			yield break;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00004C5C File Offset: 0x00002E5C
		private Transform CreateEmptySprite(AIActor target)
		{
			GameObject gameObject = new GameObject("suck image");
			gameObject.layer = target.gameObject.layer;
			tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
			gameObject.transform.parent = SpawnManager.Instance.VFX;
			tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
			tk2dSprite.transform.position = target.sprite.transform.position;
			GameObject gameObject2 = new GameObject("image parent");
			gameObject2.transform.position = tk2dSprite.WorldCenter;
			tk2dSprite.transform.parent = gameObject2.transform;
			bool flag = target.optionalPalette != null;
			if (flag)
			{
				tk2dSprite.renderer.material.SetTexture("_PaletteTex", target.optionalPalette);
			}
			return gameObject2.transform;
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Combine(player.OnReloadedGun, new Action<PlayerController, Gun>(this.PostProcessProjectile));
			player.PostProcessProjectile += this.Mimicgun;
			this.used = false;
			this.radius = 5.5f;

		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(player.OnReloadedGun, new Action<PlayerController, Gun>(this.PostProcessProjectile));
			player.PostProcessProjectile -= this.Mimicgun;
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnReloadedGun = (Action<PlayerController, Gun>)Delegate.Remove(player.OnReloadedGun, new Action<PlayerController, Gun>(this.PostProcessProjectile));
				player.PostProcessProjectile -= this.Mimicgun;
			}
			base.OnDestroy();
		}

		private void Mimicgun(Projectile arg1, float arg2)
		{
			this.flag = base.Owner.CurrentGun.PickupObjectId == 734;
			if (this.flag)
			{
				arg1.baseData.damage *= 2.3f;
				arg1.AdditionalScaleMultiplier = 2f;
				HomingModifier homingModifier = arg1.gameObject.AddComponent<HomingModifier>();
				homingModifier.HomingRadius = 10f;
				homingModifier.AngularVelocity = 300f;
				this.radius = 10f;

			}
			if (!this.flag)
			{ this.radius = 5.5f; }
		}

		protected override void Update()
		{

			if (base.Owner.HasPickupID(601) && !this.used)
			{
				LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(734).gameObject, base.Owner);

				this.used = true;
			}
			if (base.Owner.HasPickupID(157) && !this.used)
			{
				LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(734).gameObject, base.Owner);

				this.used = true;
			}
			if (base.Owner.HasPickupID(443) && !this.used)
			{
				LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(734).gameObject, base.Owner);

				this.used = true;
			}
			if (!error)
			{
				try
				{
					{
						if (Gungeon.Game.Items["spapi:big_chamber"] != null && Gungeon.Game.Items["spapi:big_gun"] != null)
						{
							PickupObject pickupObject1 = Gungeon.Game.Items["spapi:big_gun"];
							PickupObject pickupObject2 = Gungeon.Game.Items["spapi:big_chamber"];
							if (!this.added)
							{
								this.added = true;
								List<string> mandatoryConsoleIDs = new List<string> {
								"ror:hungering_chamber"
								};
								List<string> oprionalConsoleIDs = new List<string> {
								"spapi:big_chamber",
								"spapi:big_gun",
								"spapi:lichs_faithful_gun"
							   };
								CustomSynergies.Add("The Big Succ!", mandatoryConsoleIDs, oprionalConsoleIDs, true);
							}

							if (base.Owner.HasPickupID(pickupObject1.PickupObjectId) || base.Owner.HasPickupID(pickupObject2.PickupObjectId))
							{
								if (!this.used)
								{
									LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(734).gameObject, base.Owner);

									this.used = true;
								}
							}
						}
						if (Gungeon.Game.Items["spapi:lichs_faithful_gun"] != null)
						{
							PickupObject pickupObject = Gungeon.Game.Items["spapi:lichs_faithful_gun"];

							if (base.Owner.HasPickupID(pickupObject.PickupObjectId))
							{
								if (!this.used)
								{
									LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(734).gameObject, base.Owner);

									this.used = true;
								}
							}
						}
					}
				}
				catch
				{ this.error = true;
					ETGModConsole.Log($"<color={Color.cyan}>{"no synergy 4 u"}</color>");
				}
			}

			
			base.Update();
		}
	}
} 