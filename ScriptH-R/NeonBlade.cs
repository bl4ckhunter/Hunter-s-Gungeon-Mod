using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ItemAPI;
using Mod;
using MultiplayerBasicExample;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	public class NeonBlade : PassiveItem
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584
		public static void Init()
		{
			string name = "Mercenary's Sheath";
			string resourceName = "ClassLibrary1/Resources/Move"; ;
			GameObject gameObject = new GameObject();
			NeonBlade NeonBlade = gameObject.AddComponent<NeonBlade>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Neon Blades";
			string longDesc = "This cool looking blade is in actuality an high tech sheath that hones blades kept whitin with a plasma edge, when a blade is drawn that energy is diverted to a short range teleportation system as well as to enhancing the owner's movement speed, reflexes end ensuring that their footing is strong.\n\n" +
			" The wooden blade inside was meant as little more than a way to stop the sheath from deforming during shipping but it'll do until you find a new one, in a pinch blade shaped guns will do just as well but you still are going to need to get up close and personal if you want to get the full effect. Plasma augmented strikes deal massive damage to bosses";
			NeonBlade.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(NeonBlade, PlayerStats.StatType.Curse, 2, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(NeonBlade, PlayerStats.StatType.Coolness, 3, StatModifier.ModifyMethod.ADDITIVE);
			NeonBlade.quality = PickupObject.ItemQuality.C;
			NeonBlade.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			NeonBlade.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			NeonBlade.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
			NeonBlade.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
			NeonBlade.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
		}



		// Token: 0x060001D2 RID: 466 RVA: 0x0000E510 File Offset: 0x0000C710
		// Token: 0x060001D3 RID: 467 RVA: 0x0000E534 File Offset: 0x0000C734

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E5CE File Offset: 0x0000C7CE

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.downwellAfterimage = player.gameObject.AddComponent<AfterImageTrailController>();
			this.downwellAfterimage.spawnShadows = false;
			this.downwellAfterimage.shadowTimeDelay = 0.05f;
			this.downwellAfterimage.shadowLifetime = 0.3f;
			this.downwellAfterimage.minTranslation = 0.05f;
			this.downwellAfterimage.dashColor = Color.cyan;
			this.downwellAfterimage.OverrideImageShader = ShaderCache.Acquire("Brave/Internal/DownwellAfterImage");
			player.OnDealtDamageContext += this.Mimicgun;
			player.GunChanged += this.Reflag;

			this.m_glintShader = Shader.Find("Brave/ItemSpecific/LootGlintAdditivePass");
			if (!this.used)
			{
				LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(256).gameObject, base.Owner);
				LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(312).gameObject, base.Owner);
				LootEngine.GivePrefabToPlayer(PickupObjectDatabase.GetById(574).gameObject, base.Owner);
				this.used = true;
				

			}
			
		}

		private IEnumerator HandleAfterImageStop(PlayerController player)
		{
			this.downwellAfterimage.spawnShadows = true;
			while (base.Owner.CurrentGun.PickupObjectId == 417 || base.Owner.CurrentGun.PickupObjectId == 574 || base.Owner.CurrentGun.PickupObjectId == 813 || base.Owner.CurrentGun.PickupObjectId == 345 || base.Owner.CurrentGun.PickupObjectId == 377 || base.Owner.CurrentGun.PickupObjectId == 537)
			{
				yield return null;
			}
			if (this.downwellAfterimage)
			{
				this.downwellAfterimage.spawnShadows = false;
			}
			yield break;
		}

		private void Reflag(Gun blade, Gun any, bool blarp)
		{
			PickupObject pickupObject = Gungeon.Game.Items["ror:bloodsoaked_tassel"];
			this.flag = base.Owner.CurrentGun.PickupObjectId == 417 || base.Owner.CurrentGun.PickupObjectId == Butchers.flamberig || base.Owner.CurrentGun.PickupObjectId == Katanagun.Katanid || base.Owner.CurrentGun.PickupObjectId == 574 || base.Owner.CurrentGun.PickupObjectId == 813 || base.Owner.CurrentGun.PickupObjectId == 345 ||base.Owner.CurrentGun.PickupObjectId == 377 || base.Owner.CurrentGun.PickupObjectId == 537;
			if (this.flag && !this.active) {
				this.ProcessGunShader(base.Owner.CurrentGun);
				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.CurrentGun.sprite);
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 255f, 255f).WithAlpha(Color.blue.a / 90f));
				base.Owner.StartCoroutine(this.HandleAfterImageStop(base.Owner));
				this.m_buffedTarget = base.Owner;
				this.m_temporaryModifier = new StatModifier();
				this.m_temporaryModifier.statToBoost = PlayerStats.StatType.EnemyProjectileSpeedMultiplier;
				this.m_temporaryModifier.amount = 0.80f;
				this.m_temporaryModifier.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
				base.Owner.ownerlessStatModifiers.Add(this.m_temporaryModifier);
				this.m_temporaryModifier1 = new StatModifier();
				this.m_temporaryModifier1.statToBoost = PlayerStats.StatType.MovementSpeed;
				this.m_temporaryModifier1.amount = 1.30f;
				this.m_temporaryModifier1.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
				this.m_temporaryModifier2 = new StatModifier();
				this.m_temporaryModifier2.statToBoost = PlayerStats.StatType.DodgeRollDistanceMultiplier;
				this.m_temporaryModifier2.amount = 0.50f;
				this.m_temporaryModifier2.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
				this.m_temporaryModifier3 = new StatModifier();
				this.m_temporaryModifier3.statToBoost = PlayerStats.StatType.DodgeRollSpeedMultiplier;
				this.m_temporaryModifier3.amount = 7f;
				this.m_temporaryModifier3.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
				base.Owner.ownerlessStatModifiers.Add(this.m_temporaryModifier1);
				base.Owner.ownerlessStatModifiers.Add(this.m_temporaryModifier2);
				base.Owner.ownerlessStatModifiers.Add(this.m_temporaryModifier3);
				base.Owner.stats.RecalculateStats(base.Owner, false, false);
				base.Owner.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
				this.active = true;
				this.CanBeDropped = false;
			}
			if(this.flag && this.active)
			{
				Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(base.Owner.CurrentGun.sprite);
				outlineMaterial.SetColor("_OverrideColor", new Color(0f, 255f, 255f).WithAlpha(Color.blue.a / 90f));
			}
			if(!this.flag)
			{
				this.active = false;
				this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier);
				this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier1);
				this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier2);
				this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier3);
				base.Owner.stats.RecalculateStats(base.Owner, false, false);
				base.Owner.RemovePassiveItem(pickupObject.PickupObjectId);
				this.CanBeDropped = true;


			}
			

		}


		private void ProcessGunShader(Gun g)
		{
			MeshRenderer component = g.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == this.m_glintShader)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(this.m_glintShader);
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;
			return;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E660 File Offset: 0x0000C860
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject debrisObject = base.Drop(player);

			player.OnDealtDamageContext -= this.Mimicgun;
			player.GunChanged -= this.Reflag;
			debrisObject.GetComponent<NeonBlade>().m_pickedUpThisRun = true;
			return debrisObject;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnDealtDamageContext -= this.Mimicgun;
				player.GunChanged -= this.Reflag;
			}
			base.OnDestroy();
		}

		private void Mimicgun(PlayerController player, float arg2, bool fatal, HealthHaver ai)
		{
			
			this.flag = base.Owner.CurrentGun.PickupObjectId == 417 || base.Owner.CurrentGun.PickupObjectId == 574 || base.Owner.CurrentGun.PickupObjectId == 813 || base.Owner.CurrentGun.PickupObjectId == 345 || base.Owner.CurrentGun.PickupObjectId == 377 || base.Owner.CurrentGun.PickupObjectId == 537;
			if (this.flag && Vector2.Distance(ai.specRigidbody.UnitCenter, base.Owner.specRigidbody.UnitTopCenter) < 4f)
			{
				Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[748]).DefaultModule.projectiles[0];
				projectile2.AdjustPlayerProjectileTint(Color.blue.WithAlpha(Color.cyan.a / 2f), 5, 0f);
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, ai.specRigidbody.UnitTopCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					component.baseData.damage = 120f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
					component.SetOwnerSafe(base.Owner, "Player");
					component.Shooter = base.Owner.specRigidbody;
					projectile.ignoreDamageCaps = true;
					component.baseData.force = 0f;
					HomingModifier homingModifier2 = component.gameObject.AddComponent<HomingModifier>();
					homingModifier2.HomingRadius = 4f;
					homingModifier2.AngularVelocity = 500000f;
				}

			}
			
		}

		private Shader m_glintShader;

		// Token: 0x040000BC RID: 188
		private float Boost = 0f;

		// Token: 0x040000BD RID: 189
		private float TrueColorBoost;

		// Token: 0x040000BE RID: 190
		private float ColorBoost;

		// Token: 0x040000BF RID: 191
		private PlayerController m_player;


		private bool flag;
		private bool used;
		private bool active;
		public ScarfAttachmentDoer ScarfPrefab;
		private AfterImageTrailController downwellAfterimage;
		// Token: 0x0400009D RID: 157
		protected PlayerController m_buffedTarget;

		// Token: 0x0400009E RID: 158
		protected StatModifier m_temporaryModifier;
		private StatModifier m_temporaryModifier1;
		private StatModifier m_temporaryModifier2;
		private StatModifier m_temporaryModifier3;

		public object CheckBoost;
		public object LastCheckBoost;
	}

}

