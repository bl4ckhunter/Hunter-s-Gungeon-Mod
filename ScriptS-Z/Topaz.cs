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

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Gem : IounStoneOrbitalItem
	{
		// Token: 0x06000041 RID: 65 RVA: 0x00005BB0 File Offset: 0x00003DB0
		public static void Init()
		{
			string name = "Topaz Guon Soulstone";
			string resourcePath = "ClassLibrary1/Resources/Topaz";
			GameObject gameObject = new GameObject(name);
			Gem Gem = gameObject.AddComponent<Gem>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Shimmering Protection";
			string longDesc = "The soul of a gundead, crystallized into a guon stone";
			ItemBuilder.SetupItem(Gem, shortDesc, longDesc, "ror");
			Gem.quality = PickupObject.ItemQuality.EXCLUDED;
			Gem.BuildPrefab();
			Gem.OrbitalPrefab = Gem.orbitalPrefab;
			Gem.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
			Gem.CanBeDropped = false;
		}



		// Token: 0x06000042 RID: 66 RVA: 0x00005C38 File Offset: 0x00003E38
		public static void BuildPrefab()
		{
			bool flag = Gem.orbitalPrefab != null;
			bool flag2 = !flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			if (flag4)
			{
				GameObject gameObject = SpriteBuilder.SpriteFromResource("ClassLibrary1/Resources/Topaz", null, true);
				gameObject.name = "gem";
				SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(12, 12));
				Gem.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
				speculativeRigidbody.CollideWithTileMap = false;
				speculativeRigidbody.CollideWithOthers = true;
				speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
				Gem.orbitalPrefab.shouldRotate = false;
				Gem.orbitalPrefab.orbitRadius = 1.5f;
				Gem.orbitalPrefab.orbitDegreesPerSecond = 90f;
				Gem.orbitalPrefab.orbitDegreesPerSecond = 120f;
				Gem.orbitalPrefab.SetOrbitalTier(0);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				FakePrefab.MarkAsFakePrefab(gameObject);
				gameObject.SetActive(false);
			}
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			bool flag = this.m_extantOrbital != null;
			

				SpeculativeRigidbody specRigidbody = this.m_extantOrbital.GetComponent<PlayerOrbital>().specRigidbody;
				specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.Fire));

		}

		private void Fire(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{
			base.Owner.RemovePassiveItem(this.PickupObjectId);
			GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
			AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
			GameObject gameObject = new GameObject("silencer");
			SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
			float additionalTimeAtMaxRadius = 0.25f;
			silencerInstance.TriggerSilencer(myRigidbody.sprite.WorldCenter, 20f, 3.5f, silencerVFX, 0f, 3f, 3f, 3f, 30f, 3f, additionalTimeAtMaxRadius, base.Owner, true, false);
		}







		// Token: 0x0400001B RID: 27
		public static Hook guonHook;

		// Token: 0x0400001C RID: 28
		public static PlayerOrbital orbitalPrefab;

		// Token: 0x0400001D RID: 29
	}
}
