using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class Acrid : PassiveItem
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584
		public static void Init()
		{
			string name = "Acrid's Hunger";
			string resourceName = "ClassLibrary1/Resources/Acrid"; ;
			GameObject gameObject = new GameObject();
			Acrid Acrid = gameObject.AddComponent<Acrid>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "Leaded Stomach";
			string longDesc = "...and so he left, with a new hunger: To be left alone.\n\n\n " + "Dodgeroll into enemies under 40% hp or in kill range to eat them, gain 1% damage but lose 1% movement speed and dodgeroll distance for each enemy in your belly, press reload to spit them out. Enemies in eating range are marked.";
			Acrid.SetupItem(shortDesc, longDesc, "ror");
			ItemBuilder.AddPassiveStatModifier(Acrid, PlayerStats.StatType.DodgeRollDamage, 2, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(Acrid, PlayerStats.StatType.DodgeRollDistanceMultiplier, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(Acrid, PlayerStats.StatType.MovementSpeed, 1.2f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			Acrid.quality = PickupObject.ItemQuality.C;
			Acrid.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Acrid.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Acrid.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);
			Acrid.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
			Acrid.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000E510 File Offset: 0x0000C710
		private void NewFloor(PlayerController player)
		{
			this.Boost += 0.01f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
		}
		private void Reset(PlayerController player, float value)
		{
			this.Boost -= 0.01f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
		}
		// Token: 0x060001D3 RID: 467 RVA: 0x0000E534 File Offset: 0x0000C734
		private void Stats()
		{
			bool flag = this.CheckBoost == this.LastCheckBoost;
			if (!flag)
			{
				this.RemoveStat(PlayerStats.StatType.Damage);
				this.RemoveStat(PlayerStats.StatType.MovementSpeed);
				this.RemoveStat(PlayerStats.StatType.DodgeRollDistanceMultiplier);
				float amount = this.Boost + 1f;
				float amount2 = -this.Boost + 1f;
				bool flag2 = this.ColorBoost == 0f;
				if (flag2)
				{
					this.TrueColorBoost = 0f;
				}
				else
				{
					this.TrueColorBoost = this.ColorBoost + 45f;
				}
				this.AddStat(PlayerStats.StatType.Damage, amount, StatModifier.ModifyMethod.MULTIPLICATIVE);
				this.AddStat(PlayerStats.StatType.MovementSpeed, amount2, StatModifier.ModifyMethod.MULTIPLICATIVE);
				this.AddStat(PlayerStats.StatType.DodgeRollDistanceMultiplier, amount2, StatModifier.ModifyMethod.MULTIPLICATIVE);

				base.Owner.stats.RecalculateStats(base.Owner, true, false);
				this.LastCheckBoost = this.CheckBoost;
			}
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000E5CE File Offset: 0x0000C7CE
		protected override void Update()
		{
			base.Update();
			if (base.Owner.CurrentRoom != null && base.Owner)
			{
				List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				foreach (AIActor ai in activeEnemies)
				{
					if (ai != null)
					{
						float hp = ai.healthHaver.GetCurrentHealthPercentage();
						bool flag = hp <= 0.40f;
						bool flag2 = ai.healthHaver.IsBoss;
						if (flag && !flag2)
						{
							ai.PlayEffectOnActor((GameObject)BraveResources.Load("Global VFX/VFX_LockOn_Predator", ".prefab"), Vector3.zero, true, true, true);
							Material outlineMaterial = SpriteOutlineManager.GetOutlineMaterial(ai.sprite);
							outlineMaterial.SetColor("_OverrideColor", new Color(1f, 0f, 1f, 50f));
						}
					}
				}
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000E5E0 File Offset: 0x0000C7E0
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			this.Boost = 0f;
			this.m_PoisonImmunity = new DamageTypeModifier();
			this.m_PoisonImmunity.damageMultiplier = 0f;
			this.m_PoisonImmunity.damageType = CoreDamageTypes.Poison;
			player.healthHaver.damageTypeModifiers.Add(this.m_PoisonImmunity);
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
			player.OnRolledIntoEnemy += this.Eat;
			player.OnReloadPressed += this.Spit;
		}

		private void Spit(PlayerController arg1, Gun arg2)
		{ if(this.Boost > 0f)
			{
				this.Reset(arg1, 1f);
				Projectile projectile2 = ((Gun)global::ETGMod.Databases.Items[151]).DefaultModule.projectiles[0];
				GameObject gameObject = SpawnManager.SpawnProjectile(projectile2.gameObject, arg1.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (base.Owner.CurrentGun == null) ? 0f : base.Owner.CurrentGun.CurrentAngle), true);
				Projectile component = gameObject.GetComponent<Projectile>();
				bool flag4 = component != null;
				if (flag4)
				{
					
					component.Owner = base.Owner;
					component.AdditionalScaleMultiplier = 3.5f;
					component.baseData.damage = 25f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage);
					PierceProjModifier pierce = component.gameObject.AddComponent<PierceProjModifier>();
					pierce.penetration = 3;
					component.BossDamageMultiplier = 5f;
					component.baseData.speed = 15f;
					component.baseData.force = 90f;
					component.SetOwnerSafe(base.Owner, "Player");
					component.Shooter = base.Owner.specRigidbody;
					component.OnDestruction += (Action<Projectile>)Delegate.Combine(component.OnHitEnemy, new Action<Projectile>(this.Fierypoop));
				}
				this.Stats();

			}
			
		}

		private void Fierypoop(Projectile arg1)
		{
			{
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
				DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(arg1.sprite.WorldBottomCenter, 4f, 1f, false);
			}
		}
		private void Sharp(SpeculativeRigidbody arg2)
		{
			bool flag = arg2 != null && arg2.aiActor != null && base.Owner != null && !arg2.healthHaver.IsBoss;
			if (flag)
			{
				arg2.aiActor.healthHaver.ApplyDamage(5f * base.Owner.stats.GetStatValue(PlayerStats.StatType.Damage), Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, true, null, false);
				GlobalSparksDoer.DoRadialParticleBurst(50, arg2.specRigidbody.HitboxPixelCollider.UnitBottomLeft, arg2.specRigidbody.HitboxPixelCollider.UnitTopRight, 90f, 2f, 0f, null, null, Color.magenta, GlobalSparksDoer.SparksType.RED_MATTER);
				AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
				GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
				DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition).TimedAddGoopCircle(arg2.sprite.WorldBottomCenter, 1f, 3f, false);
			}

		}

		public void HandleHit(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			GameManager.Instance.StartCoroutine(Bleed(arg2));

		}
		public IEnumerator Bleed(SpeculativeRigidbody arg2)
		{
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield return new WaitForSeconds(0.5f);
			this.Sharp(arg2);
			yield break;

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
					Vector3 position = base.Owner.sprite.WorldTopCenter - new Vector2(0f, 0.2f);
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
			this.Stats();

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

		private void Eat(PlayerController arg1, AIActor arg2)
		{
			if (arg2.healthHaver.GetCurrentHealth() / arg2.healthHaver.GetMaxHealth() < 0.45f && !arg2.healthHaver.IsBoss)
			{

				if (arg2.gameObject.GetComponent<ExplodeOnDeath>())
				{
					Destroy(arg2.gameObject.GetComponent<ExplodeOnDeath>());
				}
				GameManager.Instance.StartCoroutine(HandleEnemySuck(arg2));
				this.NewFloor(arg1);
			
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000E660 File Offset: 0x0000C860
		public override DebrisObject Drop(PlayerController player)
		{
			player.OnRolledIntoEnemy -= this.Eat;
			player.OnReloadPressed -= this.Spit;
			DebrisObject debrisObject = base.Drop(player);
			this.Boost = 0f;
			this.CheckBoost = 0f;
			this.LastCheckBoost = -1f;
			debrisObject.GetComponent<Acrid>().m_pickedUpThisRun = true;
			return debrisObject;
		}
		protected override void OnDestroy()
		{
			PlayerController player = base.Owner;
			if (base.Owner)
			{
				player.OnRolledIntoEnemy -= this.Eat;
				player.OnReloadPressed -= this.Spit;
				DebrisObject debrisObject = base.Drop(player);
				this.Boost = 0f;
				this.CheckBoost = 0f;
				this.LastCheckBoost = -1f;
			}
			base.OnDestroy();
		}
		// Token: 0x060001D7 RID: 471 RVA: 0x0000E6F0 File Offset: 0x0000C8F0
		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			foreach (StatModifier statModifier in this.passiveStatModifiers)
			{
				bool flag = statModifier.statToBoost == statType;
				if (flag)
				{
					return;
				}
			}
			StatModifier statModifier2 = new StatModifier
			{
				amount = amount,
				statToBoost = statType,
				modifyType = method
			};
			bool flag2 = this.passiveStatModifiers == null;
			if (flag2)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier2
				};
				return;
			}
			this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
			{
				statModifier2
			}).ToArray<StatModifier>();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000E788 File Offset: 0x0000C988
		private void RemoveStat(PlayerStats.StatType statType)
		{
			List<StatModifier> list = new List<StatModifier>();
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				bool flag = this.passiveStatModifiers[i].statToBoost != statType;
				if (flag)
				{
					list.Add(this.passiveStatModifiers[i]);
				}
			}
			this.passiveStatModifiers = list.ToArray();
		}

		// Token: 0x040000BA RID: 186
		private float Boost = 0f;
		private DamageTypeModifier m_PoisonImmunity;

		// Token: 0x040000BB RID: 187
		private float LastCheckBoost = -1f;

		// Token: 0x040000BC RID: 188
		private float CheckBoost = 0f;

		// Token: 0x040000BD RID: 189
		private float TrueColorBoost;

		// Token: 0x040000BE RID: 190
		private float ColorBoost;

		// Token: 0x040000BF RID: 191



		// Token: 0x04007521 RID: 29985
		public GameObject VFXHealthBar;
		private RatchetScouterItem healthbar;
		private GameObject gameObjectHB;
	}
}
