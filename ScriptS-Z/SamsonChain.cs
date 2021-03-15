using System; 
using System.Collections;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace Items
{
	// Token: 0x02000036 RID: 54
	public class SamsonChain : PlayerOrbitalItem
	{


		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "Convict's Chain";
			string resourcePath = "ClassLibrary1/Resources/PrimordialCube1";
			GameObject gameObject = new GameObject(name);
			SamsonChain SamsonChain = gameObject.AddComponent<SamsonChain>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "The Ol' Ball And Chain";
			string longDesc = "God knows what the hegemony makes these out of but they're virtually indestructible, the convict tossed this down from the breach, but maybe it could come useful still";
			ItemBuilder.SetupItem(SamsonChain, shortDesc, longDesc, "ror");
			SamsonChain.quality = PickupObject.ItemQuality.D;
			SamsonChain.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			SamsonChain.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			SamsonChain.BuildPrefab();
			SamsonChain.OrbitalFollowerPrefab = orbitalitemPrefab;
			SamsonChain.OrbitalPrefab = orbitalPrefab;
			SamsonChain.LinkVFXPrefab = FakePrefab.Clone((PickupObjectDatabase.GetById(29) as Gun).DefaultModule.projectiles[0].GetComponent<ChainLightningModifier>().LinkVFXPrefab);			

		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{

			GameObject gameObject = new GameObject("SamsonChain");
			gameObject = UnityEngine.Object.Instantiate<GameObject>(PickupObjectDatabase.GetById(578).GetComponent<SprenOrbitalItem>().gameObject);
			tk2dBaseSprite spritetoshade = gameObject.GetComponent<tk2dBaseSprite>();
			spritetoshade.usesOverrideMaterial = true;
			spritetoshade.renderer.material.shader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			orbitalitemPrefab = gameObject.GetComponent<PlayerOrbitalItem>().OrbitalFollowerPrefab;
			orbitalitemPrefab.GetComponentInChildren<tk2dSprite>().scale = new Vector3(1.2f, 1.2f, 0);
			orbitalPrefab = gameObject.GetComponent<PlayerOrbitalItem>().OrbitalPrefab;
			gameObjectprefab = gameObject;
			
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}
		private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{
			if (otherRigidbody.GetComponent<Projectile>())
			{
				Projectile component = otherRigidbody.GetComponent<Projectile>();
				if (component != null && !(component.Owner is PlayerController))
				{
					PassiveReflectItem.ReflectBullet(component, true, GameManager.Instance.PrimaryPlayer, 10f, 1f, 5f, 0f);
					PhysicsEngine.SkipCollision = true;
				}
			}
			
		}
		
		protected override void Update()
		{   
			base.Update();
			if(m_extantOrbital != null && !collided)
			{
				m_extantOrbital.GetComponentInChildren<SpeculativeRigidbody>().OnPreRigidbodyCollision += OnPreCollision;
				m_extantOrbital.GetComponentInChildren<SpeculativeRigidbody>().AddCollisionLayerOverride(CollisionMask.LayerToMask(CollisionLayer.EnemyCollider));
				collided = true;
			}
			else {
				collided = false;
			}
			if (this.LinkVFXPrefab == null)
			{
				this.LinkVFXPrefab = FakePrefab.Clone((PickupObjectDatabase.GetById(29) as Gun).DefaultModule.projectiles[0].GetComponent<ChainLightningModifier>().LinkVFXPrefab);
			}
			if (this.Owner && this.extantLink == null)
			{
				tk2dTiledSprite component = SpawnManager.SpawnVFX(this.LinkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
				this.extantLink = component;
			}
			else if (this.Owner && this.extantLink != null)
			{
				UpdateLink(this.Owner, this.extantLink);
			}
			else if (extantLink != null)
			{
				SpawnManager.Despawn(extantLink.gameObject);
				extantLink = null;
			}
			tk2dSprite sprite = base.m_extantOrbital.GetComponentInChildren<tk2dSprite>();
			Shader shader = ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader");
			Material material = new Material(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
			material.name = "HologramMaterial";
			Material material2 = material;
			material.SetTexture("_MainTex", sprite.renderer.material.GetTexture("_MainTex"));
			material2.SetTexture("_MainTex", sprite.renderer.sharedMaterial.GetTexture("_MainTex"));
			sprite.renderer.material.shader = shader;
			sprite.renderer.material = material;
			sprite.renderer.sharedMaterial = material2;
			sprite.usesOverrideMaterial = true;
			PlayerController man = base.Owner as PlayerController;
		}

		private void UpdateLink(PlayerController target, tk2dTiledSprite m_extantLink)
		{
			Vector2 unitCenter = base.m_extantOrbital.GetComponentInChildren<tk2dSprite>().sprite.WorldCenter;
			Vector2 unitCenter2 = target.specRigidbody.HitboxPixelCollider.UnitBottomCenter;
			m_extantLink.transform.position = unitCenter;
			Vector2 vector = unitCenter2 - unitCenter;
			float num = BraveMathCollege.Atan2Degrees(vector.normalized);
			int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
			m_extantLink.dimensions = new Vector2((float)num2, m_extantLink.dimensions.y);
			m_extantLink.transform.rotation = Quaternion.Euler(0f, 0f, num);
			m_extantLink.UpdateZDepth();
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
		private Projectile component1;
		private object component;
		private GameObject gameObject1;
		private GameObject gameObject0;
		private GameObject LinkVFXPrefab;
		private tk2dTiledSprite extantLink;
		private bool collided;
	}
}
	







