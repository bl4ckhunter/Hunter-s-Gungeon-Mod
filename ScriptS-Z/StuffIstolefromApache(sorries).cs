using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gungeon.Utilities
{
	// Token: 0x02000023 RID: 35
	public static class ReflectionHelpers
	{
		// Token: 0x06000171 RID: 369 RVA: 0x0001AD98 File Offset: 0x00018F98
		public static IList CreateDynamicList(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type", "Argument cannot be null.");
			}
			foreach (ConstructorInfo constructorInfo in typeof(List<>).MakeGenericType(new Type[]
			{
				type
			}).GetConstructors())
			{
				if (constructorInfo.GetParameters().Length == 0)
				{
					return (IList)constructorInfo.Invoke(null, null);
				}
			}
			throw new ApplicationException("Could not create a new list with type <" + type.ToString() + ">.");
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0001AE20 File Offset: 0x00019020
		public static IDictionary CreateDynamicDictionary(Type typeKey, Type typeValue)
		{
			if (typeKey == null)
			{
				throw new ArgumentNullException("type_key", "Argument cannot be null.");
			}
			if (typeValue == null)
			{
				throw new ArgumentNullException("type_value", "Argument cannot be null.");
			}
			foreach (ConstructorInfo constructorInfo in typeof(Dictionary<,>).MakeGenericType(new Type[]
			{
				typeKey,
				typeValue
			}).GetConstructors())
			{
				if (constructorInfo.GetParameters().Length == 0)
				{
					return (IDictionary)constructorInfo.Invoke(null, null);
				}
			}
			throw new ApplicationException(string.Concat(new string[]
			{
				"Could not create a new dictionary with types <",
				typeKey.ToString(),
				",",
				typeValue.ToString(),
				">."
			}));
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0001AEE2 File Offset: 0x000190E2
		public static T ReflectGetField<T>(Type classType, string fieldName, object o = null)
		{
			return (T)((object)classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static)).GetValue(o));
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0001AF00 File Offset: 0x00019100
		public static void ReflectSetField<T>(Type classType, string fieldName, T value, object o = null)
		{
			classType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static)).SetValue(o, value);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0001AF1F File Offset: 0x0001911F
		public static T ReflectGetProperty<T>(Type classType, string propName, object o = null, object[] indexes = null)
		{
			return (T)((object)classType.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static)).GetValue(o, indexes));
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0001AF3E File Offset: 0x0001913E
		public static void ReflectSetProperty<T>(Type classType, string propName, T value, object o = null, object[] indexes = null)
		{
			classType.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | ((o != null) ? BindingFlags.Instance : BindingFlags.Static)).SetValue(o, value, indexes);
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0001AF5F File Offset: 0x0001915F
		public static MethodInfo ReflectGetMethod(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
		{
			MethodInfo[] array = ReflectionHelpers.ReflectTryGetMethods(classType, methodName, methodArgumentTypes, genericMethodTypes, isStatic);
			if (array.Count<MethodInfo>() == 0)
			{
				throw new MissingMethodException("Cannot reflect method, not found based on input parameters.");
			}
			if (array.Count<MethodInfo>() > 1)
			{
				throw new InvalidOperationException("Cannot reflect method, more than one method matched based on input parameters.");
			}
			return array[0];
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0001AF9C File Offset: 0x0001919C
		public static MethodInfo ReflectTryGetMethod(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
		{
			MethodInfo[] array = ReflectionHelpers.ReflectTryGetMethods(classType, methodName, methodArgumentTypes, genericMethodTypes, isStatic);
			MethodInfo result;
			if (array.Count<MethodInfo>() == 0)
			{
				result = null;
			}
			else if (array.Count<MethodInfo>() > 1)
			{
				result = null;
			}
			else
			{
				result = array[0];
			}
			return result;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0001AFD8 File Offset: 0x000191D8
		public static MethodInfo[] ReflectTryGetMethods(Type classType, string methodName, Type[] methodArgumentTypes = null, Type[] genericMethodTypes = null, bool? isStatic = null)
		{
			BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic;
			if (isStatic == null || isStatic.Value)
			{
				bindingFlags |= BindingFlags.Static;
			}
			if (isStatic == null || !isStatic.Value)
			{
				bindingFlags |= BindingFlags.Instance;
			}
			MethodInfo[] methods = classType.GetMethods(bindingFlags);
			List<MethodInfo> list = new List<MethodInfo>();
			for (int i = 0; i < methods.Length; i++)
			{
				if (!(methods[i].Name != methodName))
				{
					if (methods[i].IsGenericMethodDefinition)
					{
						if (genericMethodTypes == null || genericMethodTypes.Length == 0 || methods[i].GetGenericArguments().Length != genericMethodTypes.Length)
						{
							goto IL_F5;
						}
						methods[i] = methods[i].MakeGenericMethod(genericMethodTypes);
					}
					else if (genericMethodTypes != null && genericMethodTypes.Length != 0)
					{
						goto IL_F5;
					}
					ParameterInfo[] parameters = methods[i].GetParameters();
					if (methodArgumentTypes != null)
					{
						if (parameters.Length != methodArgumentTypes.Length)
						{
							goto IL_F5;
						}
						for (int j = 0; j < parameters.Length; j++)
						{
							if (parameters[j].ParameterType != methodArgumentTypes[j])
							{
								goto IL_F5;
							}
						}
					}
					list.Add(methods[i]);
				}
			IL_F5:;
			}
			return list.ToArray();
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0001B0F0 File Offset: 0x000192F0
		public static object InvokeRefs<T0>(MethodInfo methodInfo, object o, T0 p0)
		{
			object[] parameters = new object[]
			{
				p0
			};
			return methodInfo.Invoke(o, parameters);
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0001B118 File Offset: 0x00019318
		public static object InvokeRefs<T0>(MethodInfo methodInfo, object o, ref T0 p0)
		{
			object[] array = new object[]
			{
				p0
			};
			object result = methodInfo.Invoke(o, array);
			p0 = (T0)((object)array[0]);
			return result;
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0001B150 File Offset: 0x00019350
		public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, T0 p0, T1 p1)
		{
			object[] parameters = new object[]
			{
				p0,
				p1
			};
			return methodInfo.Invoke(o, parameters);
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0001B180 File Offset: 0x00019380
		public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1)
		{
			object[] array = new object[]
			{
				p0,
				p1
			};
			object result = methodInfo.Invoke(o, array);
			p0 = (T0)((object)array[0]);
			return result;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0001B1C4 File Offset: 0x000193C4
		public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1)
		{
			object[] array = new object[]
			{
				p0,
				p1
			};
			object result = methodInfo.Invoke(o, array);
			p1 = (T1)((object)array[1]);
			return result;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0001B208 File Offset: 0x00019408
		public static object InvokeRefs<T0, T1>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1)
		{
			object[] array = new object[]
			{
				p0,
				p1
			};
			object result = methodInfo.Invoke(o, array);
			p0 = (T0)((object)array[0]);
			p1 = (T1)((object)array[1]);
			return result;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0001B25C File Offset: 0x0001945C
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, T1 p1, T2 p2)
		{
			object[] parameters = new object[]
			{
				p0,
				p1,
				p2
			};
			return methodInfo.Invoke(o, parameters);
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0001B294 File Offset: 0x00019494
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1, T2 p2)
		{
			object[] array = new object[]
			{
				p0,
				p1,
				p2
			};
			object result = methodInfo.Invoke(o, array);
			p0 = (T0)((object)array[0]);
			return result;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0001B2E0 File Offset: 0x000194E0
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1, T2 p2)
		{
			object[] array = new object[]
			{
				p0,
				p1,
				p2
			};
			object result = methodInfo.Invoke(o, array);
			p1 = (T1)((object)array[1]);
			return result;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0001B32C File Offset: 0x0001952C
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, T1 p1, ref T2 p2)
		{
			object[] array = new object[]
			{
				p0,
				p1,
				p2
			};
			object result = methodInfo.Invoke(o, array);
			p2 = (T2)((object)array[2]);
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0001B378 File Offset: 0x00019578
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1, T2 p2)
		{
			object[] array = new object[]
			{
				p0,
				p1,
				p2
			};
			object result = methodInfo.Invoke(o, array);
			p0 = (T0)((object)array[0]);
			p1 = (T1)((object)array[1]);
			return result;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0001B3D8 File Offset: 0x000195D8
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, T1 p1, ref T2 p2)
		{
			object[] array = new object[]
			{
				p0,
				p1,
				p2
			};
			object result = methodInfo.Invoke(o, array);
			p0 = (T0)((object)array[0]);
			p2 = (T2)((object)array[2]);
			return result;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0001B438 File Offset: 0x00019638
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, T0 p0, ref T1 p1, ref T2 p2)
		{
			object[] array = new object[]
			{
				p0,
				p1,
				p2
			};
			object result = methodInfo.Invoke(o, array);
			p1 = (T1)((object)array[1]);
			p2 = (T2)((object)array[2]);
			return result;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0001B498 File Offset: 0x00019698
		public static object InvokeRefs<T0, T1, T2>(MethodInfo methodInfo, object o, ref T0 p0, ref T1 p1, ref T2 p2)
		{
			object[] array = new object[]
			{
				p0,
				p1,
				p2
			};
			object result = methodInfo.Invoke(o, array);
			p0 = (T0)((object)array[0]);
			p1 = (T1)((object)array[1]);
			p2 = (T2)((object)array[2]);
			return result;
		}
	}
}
