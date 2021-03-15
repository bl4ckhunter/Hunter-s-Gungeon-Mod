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

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class OfficeScroll : PlayerItem
	{
		private string DevilEnemyGuid;

		private GameObject TentacleVFX;

		private Vector2 WarpTarget;
		private bool used;
		private bool flag;


		// Token: 0x060001CF RID: 463 RVA: 0x0000E384 File Offset: 0x0000C584


		public static void Init()
		{
			string name = "Offices escape route";
			string resourceName = "ClassLibrary1/Resources/Offices";
			GameObject gameObject = new GameObject();
			OfficeScroll OfficeScroll = gameObject.AddComponent<OfficeScroll>();
			ItemBuilder.AddSpriteToObject(name, resourceName, gameObject);
			string shortDesc = "To the R&G Department";
			string longDesc = "Use it after killing a boss to descend to the R&G Department from an higher floor.\n\n" + "The Old King knew of chancellor Toadstool's treasonous intents and drafted a few escape plans, though he did share both his and Toadstool's plans with the current Bullet King the latter discarded them thoughtlessly, in time he'll regret doing that. \n" ;
			OfficeScroll.SetupItem(shortDesc, longDesc, "ror");
			OfficeScroll.AddPassiveStatModifier(PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
			OfficeScroll.quality = PickupObject.ItemQuality.EXCLUDED;
		}

		protected override void DoEffect(PlayerController user)
		{
			AkSoundEngine.PostEvent("Play_ENM_blobulord_reform_01", base.gameObject);
			user.RemoveActiveItem(this.PickupObjectId);
			GameManager.Instance.StartCoroutine(DoOrDie(user));
			



		}

		private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
		{
			if (enemy.IsBoss)
				this.flag = true;

		}

		private void Flag(Vector2 obj)
		{
			this.flag = true;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000F6CF File Offset: 0x0000D8CF
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
			player.OnNewFloorLoaded += this.CheckFloor;
		}

		private void CheckFloor(PlayerController obj)
		{
			this.flag = false;
			if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON || GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
			{ obj.RemoveActiveItem(this.PickupObjectId); }
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000F6FC File Offset: 0x0000D8FC

		public override bool CanBeUsed(PlayerController user)
		{
			return user.CurrentRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) <= 0 && this.flag; } 
		
		public IEnumerator DoOrDie(PlayerController user)
		{
			new WaitForSeconds(0.05f);
			GameManager.Instance.LoadCustomLevel("tt_nakatomi");
			yield break;

		}






	}

	}







	


