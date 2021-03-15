using System;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using System.Collections;
using Dungeonator;
using System.Collections.Generic;
using Gungeon;
using ClassLibrary1;

namespace Mod
{
	//Call this method from the Start() method of your ETGModule extension
	public class Chaos : PassiveItem
	{
		private bool onCooldown;
		private PlayerController m_buffedTarget;
		private StatModifier m_temporaryModifier;
		private bool Picked;

		// Token: 0x06000095 RID: 149 RVA: 0x00006264 File Offset: 0x00004464
		public static void Init()
		{
			string name = "Khaos Shell";
			string resourceName = "ClassLibrary1/Resources/shell"; ;
			GameObject gameObject = new GameObject();
			Chaos Chaos = gameObject.AddComponent<Chaos>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "What could be, will be!";
			string longDesc = "This shell imbues your bullets with the power of possibility itself, but at what consequences?";
			ItemBuilder.AddPassiveStatModifier(Chaos, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(Chaos, PlayerStats.StatType.Health, 1, StatModifier.ModifyMethod.ADDITIVE);
			ItemBuilder.AddPassiveStatModifier(Chaos, PlayerStats.StatType.MovementSpeed, 1, StatModifier.ModifyMethod.ADDITIVE);
			Chaos.SetupItem(shortDesc, longDesc, "ror");
			Chaos.quality = PickupObject.ItemQuality.D;
			Chaos.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			Chaos.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
			Chaos.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
			Chaos.AddToSubShop(ItemBuilder.ShopType.Flynt, 1f);
			Chaos.AddToSubShop(ItemBuilder.ShopType.OldRed, 1f);

		}

		private void PostProcessProjectile1(Projectile sourceProjectile, float effectChanceScalar) { sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		private void PostProcessProjectile(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			if (!this.onCooldown && UnityEngine.Random.value < 0.15f)
			{
				this.onCooldown = true;
				GameManager.Instance.StartCoroutine(StartCooldown());
				List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				if (activeEnemies != null && activeEnemies.Count < 8)
				{
					int count = activeEnemies.Count;
					for (int i = 0; i < count; i++)
					{
						string text;
						string item;
						int index = UnityEngine.Random.Range(0, EnemyGuidDatabase.Entries.Count);
						bool flag = activeEnemies[i].EnemyGuid == "699cd24270af4cd183d671090d8323a1" || activeEnemies[i].EnemyGuid == "a446c626b56d4166915a4e29869737fd";
						text = EnemyGuidDatabase.Entries.Keys.ElementAt(index);
						item = EnemyGuidDatabase.Entries[text];
						if (!flag && activeEnemies[i] && activeEnemies[i].HasBeenEngaged && activeEnemies[i].healthHaver && activeEnemies[i].IsNormalEnemy && !activeEnemies[i].healthHaver.IsDead && !activeEnemies[i].healthHaver.IsBoss && activeEnemies[i].EnemyGuid != "699cd24270af4cd183d671090d8323a1" && activeEnemies[i].EnemyGuid != "a446c626b56d4166915a4e29869737fd")
						{
							activeEnemies[i].Transmogrify(EnemyDatabase.GetOrLoadByGuid(item), (GameObject)ResourceCache.Acquire("Global VFX/VFX_Item_Spawn_Poof"));
						}
					}
				}
			}
		}


		private IEnumerator StartCooldown()
		{
			yield return new WaitForSeconds(3.5f);
			this.onCooldown = false;
			yield break;
		}
		private void PostProcessBeamChanceTick(BeamController beamController) { Projectile sourceProjectile = beamController.projectile; sourceProjectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(sourceProjectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.PostProcessProjectile)); }
		// Token: 0x060000FE RID: 254 RVA: 0x00008E04 File Offset: 0x00007004

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile1;
			player.PostProcessBeamChanceTick += this.PostProcessBeamChanceTick;
			this.Stats(player);
			if (!this.Picked)
			{
				this.Picked = true;
				player.healthHaver.FullHeal();
			}
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile1;
			player.PostProcessBeamChanceTick -= this.PostProcessBeamChanceTick;
			this.RemoveTemporaryBuff();
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

		private void Stats(PlayerController user)
		{
			this.m_buffedTarget = user;
			this.m_temporaryModifier = new StatModifier();
			this.m_temporaryModifier.statToBoost = PlayerStats.StatType.Damage;
			this.m_temporaryModifier.amount = 1.5f;
			this.m_temporaryModifier.modifyType = StatModifier.ModifyMethod.MULTIPLICATIVE;
			user.ownerlessStatModifiers.Add(this.m_temporaryModifier);
			user.stats.RecalculateStats(user, false, false);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000058C4 File Offset: 0x00003AC4
		private void RemoveTemporaryBuff()
		{
			this.m_buffedTarget.ownerlessStatModifiers.Remove(this.m_temporaryModifier);
			this.m_buffedTarget.stats.RecalculateStats(this.m_buffedTarget, false, false);
			this.m_temporaryModifier = null;
			this.m_buffedTarget = null;
		}

	}
 }     