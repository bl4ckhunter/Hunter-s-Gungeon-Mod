using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Mod
{
	// Token: 0x0200000E RID: 14
	public class Kata : PassiveItem
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00004874 File Offset: 0x00002A74
		public static void Init()
		{
			string name = "Fist of The People";
			string resourcePath = "ClassLibrary1/Resources/Fist";
			GameObject gameObject = new GameObject(name);
			Kata katanaDash = gameObject.AddComponent<Kata>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject, true);
			string shortDesc = "Mechanized Revolution!";
			string longDesc = "A century long era of worker exploitation ended awfully quick when corporate overlords galaxy wide discovered that there's pretty much nothing in the known universe that can stop a pissed off woman that has been stuck in a time loop fighting monsters on a forsaken planet for the subjective equivalent of ten millenniums, at least not when she's equipped with a colony ship's worth of powerful artifacts and can flatten a state of the art armored vehicle with a single punch.\n\n"
				+ "Pressing reload while moving to perform a dash attack that knocks enemies towards where you're aiming, 5s cooldown";
			ItemBuilder.SetupItem(katanaDash, shortDesc, longDesc, "ror");
			katanaDash.quality = PickupObject.ItemQuality.D;
			katanaDash.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			katanaDash.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			katanaDash.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000048CC File Offset: 0x00002ACC
		public override void Pickup(PlayerController player)
		{
			foreach (PickupObject pickupObject in Gungeon.Game.Items.Entries)
			{
				bool flag = pickupObject is ConsumableStealthItem;
				if (flag)
				{
					this.poofVFX = ((ConsumableStealthItem)pickupObject).poofVfx;
					break;
				}
			}
			base.Pickup(player);
			player.OnReloadPressed += this.Punch;
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(75f, 75f, 0f));
		}

		private void Punch(PlayerController obj, Gun gun)
		{
			if (!this.oncoodlown)
			{
				this.oncoodlown = true;
				this.Effect(base.Owner);
				GameManager.Instance.StartCoroutine(StartCooldown1());

			}

		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.OnReloadPressed -= this.Punch;
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			player.healthHaver.IsVulnerable = true;
			outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			return result;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnReloadPressed -= this.Punch;
				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
				player.healthHaver.IsVulnerable = true;
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			}
			base.OnDestroy();
		}




		// Token: 0x06000055 RID: 85 RVA: 0x00004958 File Offset: 0x00002B58

		// Token: 0x06000056 RID: 86 RVA: 0x0000496C File Offset: 0x00002B6C

		// Token: 0x06000057 RID: 87 RVA: 0x000049A4 File Offset: 0x00002BA4

		// Token: 0x06000058 RID: 88 RVA: 0x000049D8 File Offset: 0x00002BD8
		private void Effect(PlayerController user)
		{
			bool isDashing = this.m_isDashing;
			if (!isDashing)
			{
				Vector2 vector = BraveInput.GetInstanceForPlayer(user.PlayerIDX).ActiveActions.Move.Vector;
				bool flag = vector.x == 0f && vector.y == 0f;
				if (!flag)
				{
					AkSoundEngine.PostEvent("Play_ENM_electric_charge_01", base.gameObject);
					AkSoundEngine.PostEvent("Play_BOSS_dragun_rpg_01", base.gameObject); 
					base.StartCoroutine(this.HandleDash(user, vector));
				}
			}
		}
		private IEnumerator StartCooldown1()
		{
			yield return new WaitForSeconds(5f);
			Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			outlineMaterial.SetColor("_OverrideColor", new Color(75f, 75f, 0f));
			this.oncoodlown = false;
			yield break;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004A60 File Offset: 0x00002C60
		private IEnumerator HandleDash(PlayerController user, Vector2 dashDirection)
		{
			this.m_isDashing = true;
			bool flag = this.poofVFX != null;
			if (flag)
			{
				user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
			}
			Vector2 startPosition = user.sprite.WorldCenter;
			this.actorsPassed.Clear();
			this.breakablesPassed.Clear();
			user.IsVisible = true;
			user.SetInputOverride("katana");
			user.healthHaver.IsVulnerable = false;
			user.FallingProhibited = true;
			PixelCollider playerHitbox = user.specRigidbody.HitboxPixelCollider;
			playerHitbox.CollisionLayerCollidableOverride |= CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			SpeculativeRigidbody specRigidbody = user.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.KatanaPreCollision));
			float duration = Mathf.Max(0.0001f, this.dashDistance / this.dashSpeed);
			float elapsed = -BraveTime.DeltaTime;
			while (elapsed < duration)
			{
				user.healthHaver.IsVulnerable = false;
				elapsed += BraveTime.DeltaTime;
				float adjSpeed = Mathf.Min(this.dashSpeed, this.dashDistance / BraveTime.DeltaTime);
				user.specRigidbody.Velocity = dashDirection.normalized * adjSpeed;
				yield return null;
			}
			user.IsVisible = true;
			user.ToggleGunRenderers(false, "katana");
			base.renderer.enabled = true;
			base.transform.localPosition = new Vector3(-0.3f, -0.4f, 0f);
			base.transform.localRotation = new Quaternion(0f, 0f, 0f, 1f);
			bool flag2 = this.poofVFX != null;
			if (flag2)
			{
				user.PlayEffectOnActor(this.poofVFX, Vector3.zero, false, false, false);
			}
			base.StartCoroutine(this.EndAndDamage(new List<AIActor>(this.actorsPassed), new List<MajorBreakable>(this.breakablesPassed), user, dashDirection, startPosition, user.sprite.WorldCenter));
			bool flag3 = this.finalDelay > 0f;
			if (flag3)
			{
				user.healthHaver.IsVulnerable = false;
				yield return new WaitForSeconds(this.finalDelay);
			}
			base.renderer.enabled = false;
			user.ToggleGunRenderers(true, "katana");
			playerHitbox.CollisionLayerCollidableOverride &= ~CollisionMask.LayerToMask(CollisionLayer.EnemyHitBox);
			SpeculativeRigidbody specRigidbody2 = user.specRigidbody;
			specRigidbody2.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Remove(specRigidbody2.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.KatanaPreCollision));
			user.FallingProhibited = false;
			user.ClearInputOverride("katana");
			this.m_isDashing = false;
			yield return new WaitForSeconds(0.6f);
			user.healthHaver.IsVulnerable = true;
			Material outlineMaterial1 = SpriteOutlineManager.GetOutlineMaterial(base.Owner.sprite);
			outlineMaterial1.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			yield break;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004A7D File Offset: 0x00002C7D
		private IEnumerator EndAndDamage(List<AIActor> actors, List<MajorBreakable> breakables, PlayerController user, Vector2 dashDirection, Vector2 startPosition, Vector2 endPosition)
		{
			yield return new WaitForSeconds(this.finalDelay);
			Exploder.DoLinearPush(user.sprite.WorldCenter, startPosition, 13f, 5f);
			int num;
			for (int i = 0; i < actors.Count; i = num + 1)
			{
				bool flag = actors[i];
				if (flag)
				{
					actors[i].healthHaver.ApplyDamage(this.collisionDamage, dashDirection, "Katana", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);
					this.Zap(actors[i]);
					this.Zap1(actors[i]);
				}
				num = i;
			}
			for (int j = 0; j < breakables.Count; j = num + 1)
			{
				bool flag2 = breakables[j];
				if (flag2)
				{
					breakables[j].ApplyDamage(100f, dashDirection, false, false, false);
				}
				num = j;
			}
			yield break;
		}
		private void Zap(AIActor ai)
		{
			Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[748]).DefaultModule.projectiles[0];
			projectile2.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, ai.CenterPosition, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag4 = component != null;
			if (flag4)
			{
				component.baseData.damage = 1f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
				component.SetOwnerSafe(base.Owner, "Player");
				component.Shooter = base.Owner.specRigidbody;
				component.AdjustPlayerProjectileTint(Color.blue, 1, 0f);
				component.baseData.force = 1500f;
			}
		}

		private void Zap1(AIActor ai)
		{
			Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[541]).DefaultModule.projectiles[0];
			projectile2.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.blue.a / 2f), 5, 0f);
			GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, ai.CenterPosition, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
			Projectile component = gameObject.GetComponent<Projectile>();
			bool flag4 = component != null;
			if (flag4)
			{
				component.baseData.damage = 0f;
				component.SetOwnerSafe(base.Owner, "Player");
				component.Shooter = base.Owner.specRigidbody;
				component.AdjustPlayerProjectileTint(Color.blue, 1, 0f);
				component.baseData.force = 1500f;
			}
		}
		// Token: 0x0600005B RID: 91 RVA: 0x00004ABC File Offset: 0x00002CBC
		private void KatanaPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
		{
			bool flag = otherRigidbody.projectile != null;
			if (flag)
			{
				PhysicsEngine.SkipCollision = true;
			}
			bool flag2 = otherRigidbody.aiActor != null;
			if (flag2)
			{
				PhysicsEngine.SkipCollision = true;
				bool flag3 = !this.actorsPassed.Contains(otherRigidbody.aiActor);
				if (flag3)
				{
					otherRigidbody.aiActor.DelayActions(1f);
					this.actorsPassed.Add(otherRigidbody.aiActor);
				}
			}
			bool flag4 = otherRigidbody.majorBreakable != null;
			if (flag4)
			{
				PhysicsEngine.SkipCollision = true;
				bool flag5 = !this.breakablesPassed.Contains(otherRigidbody.majorBreakable);
				if (flag5)
				{
					this.breakablesPassed.Add(otherRigidbody.majorBreakable);
				}
			}
		}

		// Token: 0x04000011 RID: 17
		public float dashDistance = 6f;

		// Token: 0x04000012 RID: 18
		public float dashSpeed = 70f;

		// Token: 0x04000013 RID: 19
		public float collisionDamage = 20f;

		// Token: 0x04000014 RID: 20
		public float finalDelay = 0f;

		// Token: 0x04000015 RID: 21

		// Token: 0x04000016 RID: 22
		public GameObject poofVFX;

		// Token: 0x04000017 RID: 23
		private bool m_isDashing;

		// Token: 0x04000018 RID: 24

		// Token: 0x04000019 RID: 25
		private List<AIActor> actorsPassed = new List<AIActor>();

		// Token: 0x0400001A RID: 26
		private List<MajorBreakable> breakablesPassed = new List<MajorBreakable>();
		private bool oncoodlown;
	}
}


// Token: 0x06007473 RID: 29811 RVA: 0x002D5E2C File Offset: 0x002D402C
