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

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class ChariotCard : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;
		private int junkcount;
		private PlayerController m_buffedTarget;
		private int itemcount;
		private bool m_usedOverrideMaterial;

		public StatModifier m_temporaryModifier { get; private set; }
		public PlayerController player { get; private set; }


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "The Chariot";
			string resourceName = "ClassLibrary1/Resources/card7";
			GameObject gameObject = new GameObject();
			ChariotCard ChariotCard = gameObject.AddComponent<ChariotCard>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Unstoppable";
			string longDesc = "Invulnerable for 25s.";
			ChariotCard.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(ChariotCard, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
			ChariotCard.quality = PickupObject.ItemQuality.COMMON;
			ChariotCard.consumable = true;
			ChariotCard.numberOfUses = 1;
			ChariotCard.SetCooldownType(ItemBuilder.CooldownType.Timed, 25f);
			
			List<PlayerItem> cards = RoRItems.cards;
			cards.Add(ChariotCard);

		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", user.gameObject);
			base.LastOwner.healthHaver.IsVulnerable = false;
			ETGMod.StartGlobalCoroutine(RoRItems.HandleShield(user));
			base.CanBeDropped = false;
			
			

		}

		private IEnumerator HandleShield(PlayerController user)
		{
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(75f, 75f, 0f));
			this.m_usedOverrideMaterial = user.sprite.usesOverrideMaterial;
			user.sprite.usesOverrideMaterial = true;
			user.SetOverrideShader(ShaderCache.Acquire("Brave/ItemSpecific/MetalSkinShader"));
			SpeculativeRigidbody specRigidbody = user.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
			user.healthHaver.IsVulnerable = false;
			float elapsed = 0f;
			while (elapsed < 25f)
			{
				elapsed += BraveTime.DeltaTime;
				user.healthHaver.IsVulnerable = false;
				yield return null;
			}
			bool flag = user;
			if (flag)
			{
				user.healthHaver.IsVulnerable = true;
				user.ClearOverrideShader();
				user.sprite.usesOverrideMaterial = this.m_usedOverrideMaterial;
				SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
				specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
				specRigidbody2 = null;
			}
			bool flag2 = this;
			if (flag2)
			{
				AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
				Material outlineMaterial1 = SpriteOutlineManager.GetOutlineMaterial(user.sprite);
				outlineMaterial1.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			}
			yield break;
		}


		private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherCollider)
		{
			Projectile component = otherRigidbody.GetComponent<Projectile>();
			bool flag = component != null && !(component.Owner is PlayerController);
			if (flag)
			{
				PassiveReflectItem.ReflectBullet(component, true, base.LastOwner.specRigidbody.gameActor, 10f, 1f, 1f, 0f);
				PhysicsEngine.SkipCollision = true;
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
}
	

	







	


