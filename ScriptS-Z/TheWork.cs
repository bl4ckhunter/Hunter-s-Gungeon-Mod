using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Gungeon;
using Dungeonator;
using MonoMod.RuntimeDetour;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MultiplayerBasicExample;
using Items;
using SaveAPI;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class TheWork : IounStoneOrbitalItem
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "The Work";
			string resourcePath = "ClassLibrary1/Resources/work";
			GameObject gameObject = new GameObject(name);
			TheWork TheWork = gameObject.AddComponent<TheWork>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "The End Of Everything";
			string longDesc = "A relic from a distant plane, fron an holy mountain by a finnish witch, said to hold the secret to gold transmutation and eternal life. \n The Golden King conspired with the Spectre to restore the relic's power. \n Their partial success was a source of endless tragedy.";
			ItemBuilder.SetupItem(TheWork, shortDesc, longDesc, "ror");
			TheWork.quality = PickupObject.ItemQuality.S;
			ItemBuilder.AddPassiveStatModifier(TheWork, PlayerStats.StatType.AdditionalBlanksPerFloor, 1, StatModifier.ModifyMethod.ADDITIVE);
			TheWork.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			TheWork.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			TheWork.SetupUnlockOnCustomStat(CustomTrackedStats.MOBIUS_CHEST_ALLTIME_MONEY, 2000f, DungeonPrerequisite.PrerequisiteOperation.GREATER_THAN);
			TheWork.BuildPrefab();
			TheWork.OrbitalPrefab = TheWork.orbitalPrefab;
			TheWork.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			TheWork.onCooldown = false;
		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{
			bool flag = TheWork.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/work", null, true);
				gameObject.name = "TheWork";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(13, 17));
				Shader shader = ShaderCache.Acquire("Brave/ItemSpecific/LootGlintAdditivePass");
				Material material = new Material(ShaderCache.Acquire("Brave/ItemSpecific/LootGlintAdditivePass"));
				material.name = "HologramMaterial";
				Material material2 = material;
				tk2dSprite sprite = gameObject.GetComponent<tk2dSprite>();
				material.SetTexture("_MainTex", sprite.renderer.material.GetTexture("_MainTex"));
				material2.SetTexture("_MainTex", sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
				material.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
				material2.SetColor(Shader.PropertyToID("_OverrideColor"), new Color32(235, 208, 103, 255));
				sprite.renderer.material.shader = shader;
				sprite.renderer.material = material;
				sprite.renderer.sharedMaterial = material2;
				sprite.usesOverrideMaterial = true;
				TheWork.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				TheWork.orbitalPrefab.shouldRotate = false;
				TheWork.orbitalPrefab.orbitRadius = 2.5f;
				TheWork.orbitalPrefab.orbitDegreesPerSecond = 90f;
				TheWork.orbitalPrefab.orbitDegreesPerSecond = 120f;
				TheWork.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		public override void Pickup(PlayerController player)
		{
			this.onCooldown = false;
			base.Pickup(player);
			bool flag = this.m_extantOrbital != null;
			

			if (flag)
			{
				SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
				player.OnUsedBlank += Fire;


			}
		}

		


		private void Fire(PlayerController player, int one)
		{
			BuildGoop();
			PickupObject bobject = UnityEngine.Object.Instantiate<PickupObject>(PickupObjectDatabase.GetById(278));
			GameActorFreezeEffect Freeze = (bobject.GetComponent<BulletStatusEffectItem>().FreezeModifierEffect);
			Engolden = Freeze;
			goopManagerForGoopType.TimedAddGoopCircle(player.transform.position, 25f);	
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
			goopDefinition.usesLifespan = false;
			goopDefinition.eternal = true;
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
					if (!arg2.healthHaver.IsBoss)
					{
						arg2.aiActor.IgnoreForRoomClear = true;

					}
					arg2.ApplyEffect(Engolden, 0.1f, null);
					if (!arg2.gameObject.GetComponent<SpawnCasings>())
					{ arg2.gameObject.AddComponent<SpawnCasings>(); }



				}
			}
		}





		// Token: 0x0400001B RID: 27
		public static Hook guonHook;

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;

		// Token: 0x0400001D RID: 29
		private bool onCooldown;
		private GameObject HellSynergyVFX;
		private static BlinkPassiveItem m_BlinkPassive;
		private static SpawnObjectPlayerItem HoleObject;
		private GameObject synergyobject;
		private Material m_distortMaterial;
		private float elapsed;
		private GoopDefinition goopDefinition;
		private GameActorFreezeEffect Engolden;
		private DeadlyDeadlyGoopManager goopManagerForGoopType;
	}
}
