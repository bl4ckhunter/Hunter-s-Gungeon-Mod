using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using Mod;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;
using SaveAPI;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class GoldGoopBottle : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private static GoopDefinition goopDefinition;
		private static DeadlyDeadlyGoopManager goopManagerForGoopType;
		private static GameActorFreezeEffect Engolden;
		

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Aureum Ichor";
			string resourceName = "ClassLibrary1/Resources/bottledgold";
			GameObject gameObject = new GameObject();
			GoldGoopBottle GoldGoopBottle = gameObject.AddComponent<GoldGoopBottle>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Brass and Gold";
			string longDesc = "The liquid metal that flowed in the veins of the Golden King, from when something still flowed there.";
			GoldGoopBottle.SetupItem(shortDesc, longDesc, "ror");
			GoldGoopBottle.quality = PickupObject.ItemQuality.D;
			GoldGoopBottle.SetCooldownType(ItemBuilder.CooldownType.Timed, 30f);
			GoldGoopBottle.SetupUnlockOnCustomStat(CustomTrackedStats.MOBIUS_CHEST_ALLTIME_MONEY, 100f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
			PickupObject bobject = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(278));
			GameActorFreezeEffect Freeze = (bobject.GetComponent<BulletStatusEffectItem>().FreezeModifierEffect);
			Engolden = Freeze;

		}

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

		protected override void DoEffect(PlayerController user)
		{
			BuildGoop();
			Projectile projectile = ((Gun)ETGMod.Databases.Items[292]).DefaultModule.projectiles[0];
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, base.LastOwner.CurrentGun.PrimaryHandAttachPoint.position, Quaternion.Euler(0f, 0f, (base.LastOwner.CurrentGun == null) ? 0f : base.LastOwner.CurrentGun.CurrentAngle), true);
			Projectile proj = gameObject.GetComponent<Projectile>();
			proj.GetComponent<GoopModifier>().goopDefinition = goopDefinition;
			proj.GetComponent<GoopModifier>().InFlightSpawnRadius = 3f;
			proj.GetComponent<GoopModifier>().SpawnGoopInFlight = true;
			proj.GetComponent<GoopModifier>().SpawnGoopOnCollision = true;
			proj.GetComponent<GoopModifier>().CollisionSpawnRadius = 6f;
			proj.Owner = base.LastOwner;


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
					if(!arg2.gameObject.GetComponent<SpawnCasings>())
					{ arg2.gameObject.AddComponent<SpawnCasings>(); }



				}
			}
		}




		public override void Update()
		{
			base.Update();
			if (base.LastOwner)
			{
				foreach (PlayerItem item in base.LastOwner.activeItems)
				{
					if
					(RoRItems.cards.Contains(item))
					{
						itemcount += 1;
					}
				}
				if (itemcount > 1)
				{ base.Drop(base.LastOwner);
					itemcount -= 1;
				}
			}
		}
	}
		internal class SpawnCasings :  MonoBehaviour
		{
			private void Start()
			{

			AIActor actor = base.GetComponent<AIActor>();
			actor.AssignedCurrencyToDrop += UnityEngine.Random.Range(1, 5);
			}
						
		
		}
	
}
	

	







	


