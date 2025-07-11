using System;
using System.Diagnostics;
using System.Reflection;
using HarmonyLib;
using KernellClientUI.UI.ActionMenu;
using MelonLoader;
using VRC.UI.Elements;

namespace KernellClientUI.VRChat
{
	// Token: 0x0200002E RID: 46
	public class ReModPatches
	{
		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0000B70A File Offset: 0x0000990A
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0000B711 File Offset: 0x00009911
		public static bool firstTimeQMOpened { get; private set; }

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000205 RID: 517 RVA: 0x0000B71C File Offset: 0x0000991C
		// (remove) Token: 0x06000206 RID: 518 RVA: 0x0000B750 File Offset: 0x00009950
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event ReModPatches.qmTriggered isOpen;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000207 RID: 519 RVA: 0x0000B784 File Offset: 0x00009984
		// (remove) Token: 0x06000208 RID: 520 RVA: 0x0000B7B8 File Offset: 0x000099B8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event ReModPatches.qmTriggeredFirstTime isOpenForFirstTime;

		// Token: 0x06000209 RID: 521 RVA: 0x0000B7EB File Offset: 0x000099EB
		internal static void OnActionMenu(ActionMenu __instance)
		{
			ActionMenuAPI.OpenMainPage(__instance);
		}

		// Token: 0x0600020A RID: 522 RVA: 0x0000B7F8 File Offset: 0x000099F8
		private static void qmEnable()
		{
			bool flag = !ReModPatches.firstTimeQMOpened;
			if (flag)
			{
				ReModPatches.firstTimeQMOpened = true;
				bool flag2 = ReModPatches.isOpenForFirstTime != null;
				if (flag2)
				{
					ReModPatches.isOpenForFirstTime();
				}
			}
			bool flag3 = ReModPatches.isOpen != null;
			if (flag3)
			{
				ReModPatches.isOpen(true);
			}
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0000B850 File Offset: 0x00009A50
		private static void qmDisable()
		{
			bool flag = ReModPatches.isOpen != null;
			if (flag)
			{
				ReModPatches.isOpen(false);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0000B878 File Offset: 0x00009A78
		public static void Patch()
		{
			try
			{
				Harmony harmony = new Harmony(Assembly.GetExecutingAssembly().FullName);
				harmony.Patch(typeof(QuickMenu).GetMethod("OnEnable"), new HarmonyMethod(typeof(ReModPatches).GetMethod("qmEnable", BindingFlags.Static | BindingFlags.NonPublic)), null, null, null, null);
				harmony.Patch(typeof(QuickMenu).GetMethod("OnDisable"), new HarmonyMethod(typeof(ReModPatches).GetMethod("qmDisable", BindingFlags.Static | BindingFlags.NonPublic)), null, null, null, null);
				harmony.Patch(typeof(ActionMenu).GetMethod("Method_Public_Void_PDM_0"), null, new HarmonyMethod(typeof(ReModPatches).GetMethod("OnActionMenu", BindingFlags.Static | BindingFlags.NonPublic)), null, null, null);
				MethodInfo method = typeof(ActionMenu).GetMethod("Method_Public_Void_PDM_8");
				harmony.Patch(method, null, new HarmonyMethod(typeof(ReModPatches).GetMethod("OnActionMenu", BindingFlags.Static | BindingFlags.NonPublic)), null, null, null);
			}
			catch (Exception ex)
			{
				MelonLogger.Msg(string.Concat(new string[]
				{
					"Failed Patching. ",
					ex.Message,
					" ",
					ex.StackTrace,
					" ",
					ex.Source
				}));
			}
		}

		// Token: 0x020000DD RID: 221
		// (Invoke) Token: 0x06000AA1 RID: 2721
		public delegate void qmTriggered(bool triggered);

		// Token: 0x020000DE RID: 222
		// (Invoke) Token: 0x06000AA5 RID: 2725
		public delegate void qmTriggeredFirstTime();
	}
}
