using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Dungeonator;
using GungeonAPI;
using HutongGames.PlayMaker.Actions;
using Items;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace Mod
{
	// Token: 0x02000035 RID: 53
	public static class ModManShrine
	{


		// Token: 0x06000145 RID: 325 RVA: 0x0000B4C8 File Offset: 0x000096C8
		public static void Add()
		{
			ShrineFactory shrineFactory = new ShrineFactory
			{
				name = "Mod Man",
				modID = "ror",
				spritePath = "ClassLibrary1/Resources/BoomSprites/Wizkid/wizkid_idle_001.png",
				acceptText = "<Put him out of his misery>",
				declineText = "<Walk away>",
				OnAccept = new Action<PlayerController, GameObject>(ModManShrine.Accept),
				OnDecline = new Action<PlayerController, GameObject>(ModManShrine.Decline),
				CanUse = new Func<PlayerController, GameObject, bool>(ModManShrine.CanUse),
				offset = new Vector3(-0.5f, 0f, 1f),
				talkPointOffset = new Vector3(0f, 0f, 0f),
				isToggle = false,
				isBreachShrine = true,


				interactableComponent = typeof(ModManInteractable)
			};
			GameObject gameObject = shrineFactory.Build();
			ModManInteractable component = gameObject.GetComponent<ModManInteractable>();
			gameObject.AddAnimation("idle", "ClassLibrary1/Resources/BoomSprites/", 5, NPCBuilder.AnimationType.Idle, DirectionalAnimation.DirectionType.None, DirectionalAnimation.FlipType.None);
			gameObject.SetActive(false);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x0000B5C0 File Offset: 0x000097C0
		private static bool CanUse(PlayerController player, GameObject npc)
		{
			return !npc.GetComponent<ModManInteractable>().successful;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x0000B5D8 File Offset: 0x000097D8
		public static void Accept(PlayerController user, GameObject npc)
		{

			List<PickupObject> Sguns = new List<PickupObject>();
		    List<PickupObject> Spassives = new List<PickupObject>();
			List<PickupObject> Sactives = new List<PickupObject>();
			List<PickupObject> Aguns = new List<PickupObject>();
			List<PickupObject> Apassives = new List<PickupObject>();
			List<PickupObject> Aactives = new List<PickupObject>();
			List<PickupObject> Bguns = new List<PickupObject>();
			List<PickupObject> Bpassives = new List<PickupObject>();
			List<PickupObject> Bactives = new List<PickupObject>();
			List<PickupObject> Cguns = new List<PickupObject>();
			List<PickupObject> Cpassives = new List<PickupObject>();
			List<PickupObject> Cactives = new List<PickupObject>();
			List<PickupObject> Dguns = new List<PickupObject>();
			List<PickupObject> Dpassives = new List<PickupObject>();
			List<PickupObject> Dactives = new List<PickupObject>();


			todestroy = new List<PickupObject>();
			try
			{
				foreach (PickupObject p in PickupObjectDatabase.Instance.Objects)
				{
					if (p != null && p.PickupObjectId > 824) 
					{
						if (p is Gun)
						{
							if (p.quality == PickupObject.ItemQuality.S)
							{ Sguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.A)
							{ Aguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.B)
							{ Bguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.C)
							{ Cguns.Add(p); }
							if (p.quality == PickupObject.ItemQuality.D)
							{ Dguns.Add(p); }
						}
						if (p is PassiveItem)
						{
							if (p.quality == PickupObject.ItemQuality.S)
							{ Spassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.A)
							{ Apassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.B)
							{ Bpassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.C)
							{ Cpassives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.D)
							{ Dpassives.Add(p); }
						}
						if (p is PlayerItem)
						{
							if (p.quality == PickupObject.ItemQuality.S)
							{ Sactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.A)
							{ Aactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.B)
							{ Bactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.C)
							{ Cactives.Add(p); }
							if (p.quality == PickupObject.ItemQuality.D)
							{ Dactives.Add(p); }
						}

					}
				} 
			}
			catch
			{ ETGModConsole.Log("---------------------ouch-------------------"); }
			try { 
			List<IPlayerInteractable> nearestInteractable1 = RoomHandler.unassignedInteractableObjects;
			foreach (IPlayerInteractable item in nearestInteractable1)
				{
					try
					{
						bool flag = (item as PickupObject).quality == PickupObject.ItemQuality.S || (item as PickupObject).quality == PickupObject.ItemQuality.A || (item as PickupObject).quality == PickupObject.ItemQuality.B || (item as PickupObject).quality == PickupObject.ItemQuality.C || (item as PickupObject).quality == PickupObject.ItemQuality.D;
						if (flag)
						{
							todestroy.Add(item as PickupObject);
						}
					}catch{ ETGModConsole.Log("---------------------ugh------------------"); }
			}
			if(todestroy.Count > 0) 
				{ 
			        int pickupidreplacement = new int();
					foreach (PickupObject p in todestroy)
					{
						try
						{
							if (p is Gun)
							{
								if (p.quality == PickupObject.ItemQuality.S)
								{ pickupidreplacement = Sguns[UnityEngine.Random.Range(0, Sguns.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.A)
								{ pickupidreplacement = Aguns[UnityEngine.Random.Range(0, Aguns.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.B)
								{ pickupidreplacement = Bguns[UnityEngine.Random.Range(0, Bguns.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.C)
								{ pickupidreplacement = Cguns[UnityEngine.Random.Range(0, Cguns.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.D)
								{ pickupidreplacement = Dguns[UnityEngine.Random.Range(0, Dguns.Count)].PickupObjectId; }

								LootEngine.SpawnItem(PickupObjectDatabase.GetById(pickupidreplacement).gameObject, p.transform.position, new Vector2(0, 2), 2f);
								UnityEngine.Object.Destroy(p.gameObject);


							}
							if (p is PassiveItem)
							{
								if (p.quality == PickupObject.ItemQuality.S)
								{ pickupidreplacement = Spassives[UnityEngine.Random.Range(0, Spassives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.A)
								{ pickupidreplacement = Apassives[UnityEngine.Random.Range(0, Apassives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.B)
								{ pickupidreplacement = Bpassives[UnityEngine.Random.Range(0, Bpassives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.C)
								{ pickupidreplacement = Cpassives[UnityEngine.Random.Range(0, Cpassives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.D)
								{ pickupidreplacement = Dpassives[UnityEngine.Random.Range(0, Dpassives.Count)].PickupObjectId; }

								LootEngine.SpawnItem(PickupObjectDatabase.GetById(pickupidreplacement).gameObject, p.transform.position, new Vector2(0, 2), 2f);
								UnityEngine.Object.Destroy(p.gameObject);
							}
							if (p is PlayerItem)
							{
								if (p.quality == PickupObject.ItemQuality.S)
								{ pickupidreplacement = Sactives[UnityEngine.Random.Range(0, Sactives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.A)
								{ pickupidreplacement = Aactives[UnityEngine.Random.Range(0, Aactives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.B)
								{ pickupidreplacement = Bactives[UnityEngine.Random.Range(0, Bactives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.C)
								{ pickupidreplacement = Cactives[UnityEngine.Random.Range(0, Cactives.Count)].PickupObjectId; }
								if (p.quality == PickupObject.ItemQuality.D)
								{ pickupidreplacement = Dactives[UnityEngine.Random.Range(0, Dactives.Count)].PickupObjectId; }

								LootEngine.SpawnItem(PickupObjectDatabase.GetById(pickupidreplacement).gameObject, p.transform.position, new Vector2(0, 2), 2f);
								UnityEngine.Object.Destroy(p.gameObject);

							}
						}
						catch{ }
					}
					npc.GetComponent<ModManInteractable>().successful = true;
				}
			else
				{
					TextBoxManager.ShowTextBox(npc.GetComponent<ModManInteractable>().talkPoint.position, npc.GetComponent<ModManInteractable>().talkPoint, 2.5f, $"Can't transmute the air can we, \n  drop some items or leave me be!", user.characterAudioSpeechTag, false, TextBoxManager.BoxSlideOrientation.NO_ADJUSTMENT, false, false);
					npc.GetComponent<ModManInteractable>().successful = false;
				}
			    




			}
			catch{ ETGModConsole.Log("---------------------yikes------------------"); }




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
		private static float elapsed;
		public static GameObject GameObject;
		private static List<IPlayerInteractable> nearestInteractable;
		private static List<PickupObject> Sguns;
		private static List<PickupObject> Spassives;
		private static List<PickupObject> Sactives;
		private static List<PickupObject> Aguns;
		private static List<PickupObject> Apassives;
		private static List<PickupObject> Aactives;
		private static List<PickupObject> Bguns;
		private static List<PickupObject> Bpassives;
		private static List<PickupObject> Bactives;
		private static List<PickupObject> Cguns;
		private static List<PickupObject> Cpassives;
		private static List<PickupObject> Cactives;
		private static List<PickupObject> Dguns;
		private static List<PickupObject> Dpassives;
		private static List<PickupObject> Dactives;
		private static List<PickupObject> todestroy;
	}
}