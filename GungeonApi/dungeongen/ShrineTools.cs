using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace GungeonAPI
{
	// Token: 0x02000006 RID: 6
	public static class ShrineTools
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002153 File Offset: 0x00000353
		public static void Init()
		{
			if (File.Exists(ShrineTools.defaultLog))
			{
				File.Delete(ShrineTools.defaultLog);
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00007B4C File Offset: 0x00005D4C
		public static void Print<T>(T obj, string color = "FFFFFF", bool force = false)
		{
			if (ShrineTools.verbose || force)
			{
				string[] array = obj.ToString().Split(new char[]
				{
					'\n'
				});
				foreach (string text in array)
				{
					ShrineTools.LogToConsole(string.Concat(new string[]
					{
						"<color=#",
						color,
						">[",
						ShrineTools.modID,
						"] ",
						text,
						"</color>"
					}));
				}
			}
			ShrineTools.Log<string>(obj.ToString());
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000216B File Offset: 0x0000036B
		public static void PrintRaw<T>(T obj, bool force = false)
		{
			if (ShrineTools.verbose || force)
			{
				ShrineTools.LogToConsole(obj.ToString());
			}
			ShrineTools.Log<string>(obj.ToString());
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00007BE8 File Offset: 0x00005DE8
		public static void PrintError<T>(T obj, string color = "FF0000")
		{
			string[] array = obj.ToString().Split(new char[]
			{
				'\n'
			});
			foreach (string text in array)
			{
				ShrineTools.LogToConsole(string.Concat(new string[]
				{
					"<color=#",
					color,
					">[",
					ShrineTools.modID,
					"] ",
					text,
					"</color>"
				}));
			}
			ShrineTools.Log<string>(obj.ToString());
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00007C7C File Offset: 0x00005E7C
		public static void PrintException(Exception e, string color = "FF0000")
		{
			string text = e.Message + "\n" + e.StackTrace;
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			foreach (string text2 in array)
			{
				ShrineTools.LogToConsole(string.Concat(new string[]
				{
					"<color=#",
					color,
					">[",
					ShrineTools.modID,
					"] ",
					text2,
					"</color>"
				}));
			}
			ShrineTools.Log<string>(e.Message);
			ShrineTools.Log<string>("\t" + e.StackTrace);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00007D28 File Offset: 0x00005F28
		public static void Log<T>(T obj)
		{
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(global::ETGMod.ResourcesDirectory, ShrineTools.defaultLog), true))
			{
				streamWriter.WriteLine(obj.ToString());
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00007D7C File Offset: 0x00005F7C
		public static void Log<T>(T obj, string fileName)
		{
			if (!ShrineTools.verbose)
			{
				return;
			}
			using (StreamWriter streamWriter = new StreamWriter(Path.Combine(global::ETGMod.ResourcesDirectory, fileName), true))
			{
				streamWriter.WriteLine(obj.ToString());
			}
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000219A File Offset: 0x0000039A
		public static void LogToConsole(string message)
		{
			message.Replace("\t", "    ");
			global::ETGModConsole.Log(message, false);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00007DD4 File Offset: 0x00005FD4
		private static void BreakdownComponentsInternal(this GameObject obj, int lvl = 0)
		{
			string text = "";
			for (int i = 0; i < lvl; i++)
			{
				text += "\t";
			}
			ShrineTools.Log<string>(text + obj.name + "...");
			foreach (Component component in obj.GetComponents<Component>())
			{
				string str = text;
				string str2 = "    -";
				Type type = component.GetType();
				ShrineTools.Log<string>(str + str2 + ((type != null) ? type.ToString() : null));
			}
			foreach (Transform transform in obj.GetComponentsInChildren<Transform>())
			{
				if (transform != obj.transform)
				{
					transform.gameObject.BreakdownComponentsInternal(lvl + 1);
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000021B4 File Offset: 0x000003B4
		public static void BreakdownComponents(this GameObject obj)
		{
			obj.BreakdownComponentsInternal(0);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00007E94 File Offset: 0x00006094
		public static void ExportTexture(Texture texture, string folder = "")
		{
			string text = Path.Combine(global::ETGMod.ResourcesDirectory, folder);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			File.WriteAllBytes(Path.Combine(text, texture.name + ".png"), ((Texture2D)texture).EncodeToPNG());
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000021BD File Offset: 0x000003BD
		public static T GetEnumValue<T>(string val) where T : Enum
		{
			return (T)((object)Enum.Parse(typeof(T), val.ToUpper()));
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00007EE4 File Offset: 0x000060E4
		public static void LogPropertiesAndFields<T>(this T obj, string header = "")
		{
			ShrineTools.Log<string>(header);
			ShrineTools.Log<string>("=======================");
			if (obj == null)
			{
				ShrineTools.Log<string>("LogPropertiesAndFields: Null object");
				return;
			}
			Type type = obj.GetType();
			ShrineTools.Log<string>(string.Format("Type: {0}", type));
			PropertyInfo[] properties = type.GetProperties();
			ShrineTools.Log<string>(string.Format("{0} Properties: ", typeof(T)));
			foreach (PropertyInfo propertyInfo in properties)
			{
				try
				{
					object value = propertyInfo.GetValue(obj, null);
					string text = value.ToString();
					bool flag = ((obj != null) ? obj.GetType().GetGenericTypeDefinition() : null) == typeof(List<>);
					if (flag)
					{
						List<object> list = value as List<object>;
						text = string.Format("List[{0}]", list.Count);
						foreach (object obj2 in list)
						{
							text = text + "\n\t\t" + obj2.ToString();
						}
					}
					ShrineTools.Log<string>("\t" + propertyInfo.Name + ": " + text);
				}
				catch
				{
				}
			}
			ShrineTools.Log<string>(string.Format("{0} Fields: ", typeof(T)));
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				ShrineTools.Log<string>(string.Format("\t{0}: {1}", fieldInfo.Name, fieldInfo.GetValue(obj)));
			}
		}

		// Token: 0x0400001D RID: 29
		public static bool verbose = false;

		// Token: 0x0400001E RID: 30
		private static string defaultLog = Path.Combine(global::ETGMod.ResourcesDirectory, "customRooms.txt");

		// Token: 0x0400001F RID: 31
		public static string modID = "CR";

		// Token: 0x04000020 RID: 32
		private static Dictionary<string, float> timers = new Dictionary<string, float>();
	}
}
