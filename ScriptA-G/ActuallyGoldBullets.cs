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
using SaveAPI;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class GoldBullets : PassiveItem
	{
		private bool onCooldown;
		private GoopDefinition goopDefinition;
		private DeadlyDeadlyGoopManager goopManagerForGoopType;
		private GameActorFreezeEffect Engolden;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Auric Bullets";
			string resourceName = "ClassLibrary1/Resources/goldbullets"; ;
			GameObject gameObject = new GameObject();
			GoldBullets GoldBullets = gameObject.AddComponent<GoldBullets>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Gold and Brass";
			string longDesc = "The Golden King carved these bullets out of his own flesh and gifted them to his most trusted retainers. \n When they shot him in the back, he didn't even feel it.";
			GoldBullets.SetupItem(shortDesc, longDesc, "ror");
			GoldBullets.quality = PickupObject.ItemQuality.A;
			GoldBullets.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			GoldBullets.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			GoldBullets.SetupUnlockOnCustomStat(CustomTrackedStats.MOBIUS_CHEST_ALLTIME_MONEY, 1500f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);


		}

		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown && UnityEngine.Random.value < 0.36f)
			{
				this.onCooldown = true;
				PickupObject bobject = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(278));
				GameActorFreezeEffect Freeze = (bobject.GetComponent<BulletStatusEffectItem>().FreezeModifierEffect);
				Engolden = Freeze;
				BuildGoop();
				GameManager.Instance.StartCoroutine(StartCooldown());
				Projectile projectile = ((Gun)ETGMod.Databases.Items[292]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, arg1.transform.position, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile proj = gameObject.GetComponent<Projectile>();
				proj.baseData.range = 1f;
				proj.GetComponent<GoopModifier>().goopDefinition = goopDefinition;
				proj.GetComponent<GoopModifier>().InFlightSpawnRadius = 1f;
				proj.GetComponent<GoopModifier>().SpawnGoopInFlight = true;
				proj.GetComponent<GoopModifier>().SpawnGoopOnCollision = true;
				proj.GetComponent<GoopModifier>().CollisionSpawnRadius = 3.5f;
			}
		}
		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(0.3f);
			this.onCooldown = false;
			yield break;
		}
		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004


		private void BuildGoop()
		{
			goopDefinition = ScriptableObject.CreateInstance<GoopDefinition>();
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDefinition1 = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/oil goop.asset");
			goopDefinition.CanBeIgnited = false;
			goopDefinition.damagesEnemies = false;
			goopDefinition.damagesPlayers = false;
			goopDefinition.baseColor32 = new Color32(235, 208, 103, 255);
			goopDefinition.CanBeFrozen = false;
			goopDefinition.usesAcidAudio = true;
			goopDefinition.isOily = true;
			goopDefinition.usesLifespan = true;
			goopDefinition.lifespan = 18f;
			goopDefinition.usesOverrideOpaqueness = true;
			goopDefinition.overrideOpaqueness = 3.4f;
			goopDefinition.CanBeElectrified = true;
			goopDefinition.goopTexture = goopDefinition1.goopTexture;
			goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
			RoRItems.OnGoopTouched += PopStars;
		}
		private void PopStars(DeadlyDeadlyGoopManager arg1, GameActor arg2, IntVector2 arg3)
		{
			if (arg1 == goopManagerForGoopType)
			{
				if (arg2 is AIActor)
				{
					Shader shader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
					Material material = new Material(Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass"));
					material.name = "HologramMaterial";
					Material material2 = material;
					material.SetTexture("_MainTex", arg2.sprite.renderer.material.GetTexture("_MainTex"));
					material2.SetTexture("_MainTex", arg2.sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
					material.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
					material2.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
					arg2.sprite.renderer.material.shader = shader;
					arg2.sprite.renderer.material = material;
					arg2.sprite.renderer.sharedMaterial = material2;
					arg2.sprite.usesOverrideMaterial = true;
					arg2.healthHaver.flashesOnDamage = false;
					Engolden.AppliesTint = false;
					Engolden.duration = 100f;
					Engolden.crystalNum = 0;
					arg2.ApplyEffect(Engolden, 0.1f, null);
					if (!arg2.gameObject.GetComponent<SpawnCasings>())
					{ arg2.gameObject.AddComponent<SpawnCasings>(); }



				}
			}
		}
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