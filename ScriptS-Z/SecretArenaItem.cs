using System;
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace Items
{
	// Token: 0x02000036 RID: 54
	public class ChronoBattery : PlayerOrbitalItem
	{

		
			// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
			public static void Init()
			{
				string name = "Chronosphere";
				string resourcePath = "ClassLibrary1/Resources/PrimordialCube1";
				GameObject gameObject = new GameObject(name);
				ChronoBattery ChronoBattery = gameObject.AddComponent<ChronoBattery>();
				ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
				string shortDesc = "Time, Frozen In Time";
				string longDesc = "A powerful power source, to be sure, but for what? Maybe in the forge you'll find the answer.";
				ItemBuilder.SetupItem(ChronoBattery, shortDesc, longDesc, "ror");
				ChronoBattery.quality = PickupObject.ItemQuality.SPECIAL;
				ChronoBattery.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
				ChronoBattery.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			ChronoBattery.BuildPrefab();
			ChronoBattery.OrbitalFollowerPrefab = orbitalitemPrefab;
			ChronoBattery.OrbitalPrefab = orbitalPrefab;
			ChronoBattery.Orbid = ChronoBattery.PickupObjectId;
			
                
			}



			// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
			public static void BuildPrefab()
			{
			
			GameObject gameObject = new GameObject("Chronobattery");
			gameObject = UnityEngine.Object.Instantiate<GameObject>(PickupObjectDatabase.GetById(578).GetComponent<SprenOrbitalItem>().gameObject);
			tk2dBaseSprite spritetoshade = gameObject.GetComponent<tk2dBaseSprite>();
			spritetoshade.usesOverrideMaterial = true;
			spritetoshade.renderer.material.shader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			orbitalitemPrefab = gameObject.GetComponent<PlayerOrbitalItem>().OrbitalFollowerPrefab;
			orbitalPrefab = gameObject.GetComponent<PlayerOrbitalItem>().OrbitalPrefab;
			gameObjectprefab = gameObject;
			
			}

			public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnEnteredCombat += IDPD;
			
		}
		public override DebrisObject Drop(PlayerController player)
		{
			
			player.OnEnteredCombat -= IDPD;
			DebrisObject result = base.Drop(player);
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnEnteredCombat -= IDPD;
			}
			base.OnDestroy();
		}

		private void IDPD()
		{   if (GameManager.Instance.Dungeon.tileIndices.tilesetId != GlobalDungeonData.ValidTilesets.RATGEON)
			{
				int Count = UnityEngine.Random.Range(2, 5);
				for (int e = 0; e < Count; e++)
				{
					AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid("57255ed50ee24794b7aac1ac3cfb8a95");
					IntVector2? intVector = new IntVector2?(base.Owner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
					AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
					aiactor.CanTargetEnemies = false;
					aiactor.CanTargetPlayers = true;
					PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
					aiactor.IsHarmlessEnemy = false;
					aiactor.IgnoreForRoomClear = true;
					aiactor.HandleReinforcementFallIntoRoom(3f);
					Material = new Material(ShaderCache.Acquire("Brave/Effects/SimplicityDerivativeShader"));
					MeshRenderer component = aiactor.GetComponent<MeshRenderer>();
					Material[] sharedMaterials = component.sharedMaterials;
					Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
					Material material = UnityEngine.Object.Instantiate<Material>(Material);
					material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
					sharedMaterials[sharedMaterials.Length - 1] = material;
					component.sharedMaterials = sharedMaterials;
					GameManager.Instance.StartCoroutine(Glowy(aiactor));

				}
			}
		}

		private IEnumerator Glowy(AIActor actor)
		{
			while(actor != null)
			{
				float floaty = (float)Mathf.PingPong(elapsed / 10, 0.98f) + 0.01f;
				SpriteOutlineManager.AddOutlineToSprite(actor.GetComponent<tk2dBaseSprite>().sprite, Color.HSVToRGB(floaty, 1f, 500f));
				SpriteOutlineManager.GetOutlineMaterial(actor.GetComponent<tk2dBaseSprite>().sprite).SetColor("_OverrideColor", Color.HSVToRGB(floaty, 1f, 500f));
				yield return null;
			}
			yield break;
		}

		protected override void Update()
		{
			base.Update();
			try
			{
				tk2dSprite sprite = base.m_extantOrbital.GetComponentInChildren<tk2dSprite>();
				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(sprite);
				elapsed += BraveTime.DeltaTime;
				float floaty = (float)Mathf.PingPong(elapsed / 10, 0.98f) + 0.01f;
				outlineMaterial.SetColor("_OverrideColor", Color.HSVToRGB(floaty, 1f, 2f));
				Shader shader = Shader.Find("Brave/Internal/HologramShader");
				Material material = new Material(Shader.Find("Brave/Internal/HologramShader"));
				material.name = "HologramMaterial";
				Material material2 = material;
				material.SetTexture("_MainTex", sprite.renderer.material.GetTexture("_MainTex"));
				material2.SetTexture("_MainTex", sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
				sprite.renderer.material.shader = shader;
				sprite.renderer.material = material;
				sprite.renderer.sharedMaterial = material2;
				sprite.usesOverrideMaterial = true;
			}
			catch 
			{ }



		}

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;
		private static GameObject gameObjectprefab;

		// Token: 0x0400001D RID: 29
		private bool onCooldown;
			private GameObject HellSynergyVFX;
			private static BlinkPassiveItem m_BlinkPassive;
			private static SpawnObjectPlayerItem HoleObject;
			private GameObject synergyobject;
			private Material m_distortMaterial;
		private static PlayerOrbitalFollower orbitalitemPrefab;
		private Shader m_glintShader;
		private float elapsed;


		public static int Orbid;
		private Material Material;
	}
}
	







