using System; 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using GungeonAPI;
using HutongGames.PlayMaker.Actions;
using ItemAPI;
using Mod;
using MultiplayerBasicExample;
using Pathfinding;
using UnityEngine;

namespace Items
{
	// Token: 0x0200005C RID: 92
	internal class SpawndeadmarineCard : PlayerItem
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
			string name = "ShrineCard";
			string resourceName = "ClassLibrary1/Resources/card7";
			GameObject gameObject = new GameObject();
			SpawndeadmarineCard ChariotCard = gameObject.AddComponent<SpawndeadmarineCard>();
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
			ShrineFactory.builtShrines.TryGetValue("ror:entryway", out GameObject gabject);
			GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, base.LastOwner.transform.position, Quaternion.identity);
			IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
			IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
			RoomHandler absoluteRoom = base.LastOwner.transform.position.GetAbsoluteRoom();
			for (int i = 0; i < interfaces.Length; i++)
			{
				absoluteRoom.RegisterInteractable(interfaces[i]);
			}
			for (int j = 0; j < interfaces2.Length; j++)
			{
				interfaces2[j].ConfigureOnPlacement(absoluteRoom);
			}
			DonationCoffer();

		}

		private void DonationCoffer()
		{
			ShrineFactory.builtShrines.TryGetValue("ror:mobiusdonationcoffer", out GameObject gabject);
			GameObject gObj = UnityEngine.Object.Instantiate<GameObject>(gabject, base.LastOwner.transform.position + new Vector3(2,2,0), Quaternion.identity);
			IPlayerInteractable[] interfaces = gObj.GetInterfaces<IPlayerInteractable>();
			IPlaceConfigurable[] interfaces2 = gObj.GetInterfaces<IPlaceConfigurable>();
			RoomHandler absoluteRoom = base.LastOwner.transform.position.GetAbsoluteRoom();
			for (int i = 0; i < interfaces.Length; i++)
			{
				absoluteRoom.RegisterInteractable(interfaces[i]);
			}
			for (int j = 0; j < interfaces2.Length; j++)
			{
				interfaces2[j].ConfigureOnPlacement(absoluteRoom);
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
	

	







	


