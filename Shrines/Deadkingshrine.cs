using System;
using System.Collections;
using System.Collections.Generic;
using GungeonAPI;
using Items;
using UnityEngine;

namespace Mod
{
	// Token: 0x02000035 RID: 53
	public static class DeadkingShrine
	{
		// Token: 0x06000145 RID: 325 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "deadking",
				modID = "ror",
				spritePath = "ClassLibrary1/Resources/BoomSprites/deadking.png",
				acceptText = "<Put him out of his misery>",
				declineText = "<Walk away>",
				OnAccept = new Action<PlayerController, GameObject>(DeadkingShrine.Accept),
				OnDecline = new Action<PlayerController, GameObject>(DeadkingShrine.Decline),
				CanUse = new Func<PlayerController, GameObject, bool>(DeadkingShrine.CanUse),
				offset = new Vector3(-0.5f, 0f, 1f),
				talkPointOffset = new Vector3(0f, 0f, 0f),
				isToggle = false,
				isBreachShrine = true,


				interactableComponent = typeof(DeadkingShrineInteractable)
			};
			GameObject gameObject = shrineFactory.Build();
			DeadkingShrineInteractable component = gameObject.GetComponent<DeadkingShrineInteractable>();
			component.conversation = new List<string>()
			{
				"The unseeing eyes of a dying soldier, doomed to relive his last moments forever, bore into you...."
			};
			gameObject.SetActive(false);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000B5C0 File Offset: 0x000097C0
		private static bool CanUse(PlayerController player, GameObject npc)
		{
			bool flag = player.healthHaver.GetCurrentHealth() > 0.5f && player.stats.GetStatValue(PlayerStats.StatType.Curse) > 1f;
			return flag;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000B5D8 File Offset: 0x000097D8
		public static void Accept(PlayerController player, GameObject npc)
		{
			player.carriedConsumables.Currency += 30;
			player.healthHaver.ApplyDamage(0.5f, Vector2.zero, "deadking");
			DeadkingShrine.teleporter = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>();
			if (player.specRigidbody.UnitCenter != null)
			{
				UnityEngine.Object.Instantiate<GameObject>(teleporter.TelefragVFXPrefab, player.specRigidbody.UnitCenter, Quaternion.identity);
			}
		}


		// Token: 0x06000148 RID: 328 RVA: 0x0000B68F File Offset: 0x0000988F
		public static void Decline(PlayerController player, GameObject npc)
		{
		}

		// Token: 0x06000149 RID: 329 RVA: 0x0000B694 File Offset: 0x00009894



		// Token: 0x0600014B RID: 331 RVA: 0x0000B780 File Offset: 0x00009980



		// Token: 0x04000060 RID: 96
		private static PlayerController storedPlayer;

		// Token: 0x04000061 RID: 97
		public static int Char = 0;

		// Token: 0x04000062 RID: 98
		private static string header = "";

		// Token: 0x04000063 RID: 99
		private static string text = "";

		// Token: 0x04000064 RID: 100
		private static bool hmmYes = true;
		private static float elapsed;
		public static GameObject GameObject;
		private static TeleporterPrototypeItem teleporter;
	}
}