using System;
using System.Collections;
using System.Collections.Generic;
using GungeonAPI;
using Items;
using SaveAPI;
using UnityEngine;

namespace Mod
{
	// Token: 0x02000035 RID: 53
	public static class MobiusDonationCoffer
	{
		// Token: 0x06000145 RID: 325 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "mobiusdonationcoffer",
				modID = "ror",
				spritePath = "ClassLibrary1/Resources/BoomSprites/mobiuscoffer/coffer_001.png",
				acceptText = "<Put him out of his misery>",
				declineText = "<Walk away>",
				OnAccept = new Action<PlayerController, GameObject>(MobiusDonationCoffer.Accept),
				OnDecline = new Action<PlayerController, GameObject>(MobiusDonationCoffer.Decline),
				CanUse = new Func<PlayerController, GameObject, bool>(MobiusDonationCoffer.CanUse),
				offset = new Vector3(-0.5f, 0f, 1f),
				talkPointOffset = new Vector3(0f, 0f, 0f),
				isToggle = false,
				isBreachShrine = true,


				interactableComponent = typeof(MobiusDonationInteractible)
			};
			GameObject gameObject = shrineFactory.Build();
			MobiusDonationInteractible component = gameObject.GetComponent<MobiusDonationInteractible>();
			component.conversation = new List<string>()
			{
				"The unseeing eyes of a dying soldier, doomed to relive his last moments forever, bore into you...."
			};
			gameObject.SetActive(false);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000B5C0 File Offset: 0x000097C0
		private static bool CanUse(PlayerController player, GameObject npc)
		{
			bool flag = player.carriedConsumables.Currency > 0;
			return flag;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000B5D8 File Offset: 0x000097D8
		public static void Accept(PlayerController player, GameObject npc)
		{ float floaty = UnityEngine.Random.Range(0.55f, 0.85f);
			SaveAPIManager.RegisterStatChange(CustomTrackedStats.MOBIUS_CHEST_MONEY, Mathf.CeilToInt(player.carriedConsumables.Currency * floaty));
			SaveAPIManager.RegisterStatChange(CustomTrackedStats.MOBIUS_CHEST_ALLTIME_MONEY, Mathf.CeilToInt(player.carriedConsumables.Currency * floaty));
			player.carriedConsumables.Currency = 0;
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
	}
}