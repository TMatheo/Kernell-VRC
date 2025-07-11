using System;
using System.Reflection;
using HarmonyLib;
using Il2CppSystem;
using MelonLoader;
using UnhollowerRuntimeLib.XrefScans;

namespace KernellClientUI
{
	// Token: 0x02000028 RID: 40
	public static class XrefUtils
	{
		// Token: 0x06000192 RID: 402 RVA: 0x00009034 File Offset: 0x00007234
		public static bool CheckMethod(MethodInfo method, string match)
		{
			try
			{
				foreach (XrefInstance xrefInstance in XrefScanner.XrefScan(method))
				{
					bool flag = xrefInstance.Type == null && xrefInstance.ReadAsObject().ToString().Contains(match);
					if (flag)
					{
						return true;
					}
				}
				return false;
			}
			catch
			{
			}
			return false;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000090C0 File Offset: 0x000072C0
		public static bool CheckUsedBy(MethodInfo method, string methodName, Type type = null)
		{
			foreach (XrefInstance xrefInstance in XrefScanner.UsedBy(method))
			{
				bool flag = xrefInstance.Type != 1;
				if (!flag)
				{
					try
					{
						bool flag2 = (type == null || xrefInstance.TryResolve().DeclaringType == type) && xrefInstance.TryResolve().Name.Contains(methodName);
						if (flag2)
						{
							return true;
						}
					}
					catch
					{
					}
				}
			}
			return false;
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009178 File Offset: 0x00007378
		public static bool CheckUsing(MethodInfo method, string methodName, Type type = null)
		{
			foreach (XrefInstance xrefInstance in XrefScanner.XrefScan(method))
			{
				bool flag = xrefInstance.Type != 1;
				if (!flag)
				{
					try
					{
						bool flag2 = (type == null || xrefInstance.TryResolve().DeclaringType == type) && xrefInstance.TryResolve().Name.Contains(methodName);
						if (flag2)
						{
							return true;
						}
					}
					catch
					{
					}
				}
			}
			return false;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009230 File Offset: 0x00007430
		public static void DumpXRefs(Type type)
		{
			MelonLogger.Msg(type.Name + " XRefs:");
			foreach (MethodInfo method in AccessTools.GetDeclaredMethods(type))
			{
				XrefUtils.DumpXRefs(method, 1);
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x000092A0 File Offset: 0x000074A0
		public static void DumpXRefs(MethodInfo method, int depth = 0)
		{
			string text = new string('\t', depth);
			MelonLogger.Msg(text + method.Name + " XRefs:");
			foreach (XrefInstance xrefInstance in XrefScanner.XrefScan(method))
			{
				bool flag = xrefInstance.Type == 0;
				if (flag)
				{
					string str = "\tString = ";
					Object @object = xrefInstance.ReadAsObject();
					MelonLogger.Msg(str + ((@object != null) ? @object.ToString() : null));
				}
				else
				{
					MethodBase methodBase = xrefInstance.TryResolve();
					bool flag2 = methodBase != null;
					if (flag2)
					{
						string[] array = new string[5];
						array[0] = text;
						array[1] = "\tMethod -> ";
						int num = 2;
						Type declaringType = methodBase.DeclaringType;
						array[num] = ((declaringType != null) ? declaringType.Name : null);
						array[3] = ".";
						array[4] = methodBase.Name;
						MelonLogger.Msg(string.Concat(array));
					}
				}
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000093A0 File Offset: 0x000075A0
		internal static bool XRefScanForMethod(MethodBase methodBase, string methodName = null, string reflectedType = null)
		{
			bool flag = false;
			foreach (XrefInstance xrefInstance in XrefScanner.XrefScan(methodBase))
			{
				bool flag2 = xrefInstance.Type != 1;
				if (!flag2)
				{
					MethodBase methodBase2 = xrefInstance.TryResolve();
					bool flag3 = !(methodBase2 == null);
					if (flag3)
					{
						bool flag4 = !string.IsNullOrEmpty(methodName);
						if (flag4)
						{
							flag = (!string.IsNullOrEmpty(methodBase2.Name) && methodBase2.Name.IndexOf(methodName, StringComparison.OrdinalIgnoreCase) >= 0);
						}
						bool flag5 = !string.IsNullOrEmpty(reflectedType);
						if (flag5)
						{
							Type reflectedType2 = methodBase2.ReflectedType;
							flag = (!string.IsNullOrEmpty((reflectedType2 != null) ? reflectedType2.Name : null) && methodBase2.ReflectedType.Name.IndexOf(reflectedType, StringComparison.OrdinalIgnoreCase) >= 0);
						}
						bool flag6 = flag;
						if (flag6)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x000094B8 File Offset: 0x000076B8
		internal static int XRefCount(MethodBase methodBase)
		{
			int num = 0;
			foreach (XrefInstance xrefInstance in XrefScanner.XrefScan(methodBase))
			{
				bool flag = xrefInstance.Type == 1;
				if (flag)
				{
					MethodBase left = xrefInstance.TryResolve();
					bool flag2 = !(left == null);
					if (flag2)
					{
						num++;
					}
				}
			}
			return num;
		}
	}
}
