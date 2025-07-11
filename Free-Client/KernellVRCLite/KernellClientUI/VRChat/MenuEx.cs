using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KernellClientUI.Unity;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Ui;
using VRC.UI.Controls;
using VRC.UI.Elements;

namespace KernellClientUI.VRChat
{
	// Token: 0x02000029 RID: 41
	public static class MenuEx
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000199 RID: 409 RVA: 0x0000953C File Offset: 0x0000773C
		public static QuickMenu QMInstance
		{
			get
			{
				bool flag = MenuEx._quickMenuInstance == null;
				if (flag)
				{
					GameObject userInterface = MenuEx.userInterface;
					MenuEx._quickMenuInstance = ((userInterface != null) ? userInterface.GetComponentInChildren<QuickMenu>(true) : null);
				}
				return MenuEx._quickMenuInstance;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000957C File Offset: 0x0000777C
		public static GameObject userInterface
		{
			get
			{
				bool flag = MenuEx._getUserInterface == null;
				if (flag)
				{
					MelonCoroutines.Start(MenuEx.WaitForUI());
				}
				return MenuEx._getUserInterface;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x0600019B RID: 411 RVA: 0x000095B0 File Offset: 0x000077B0
		public static GameObject _application
		{
			get
			{
				bool flag = MenuEx._getApplication == null;
				if (flag)
				{
					MelonCoroutines.Start(MenuEx.WaitForUI());
				}
				return MenuEx._getApplication;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x000095E3 File Offset: 0x000077E3
		private static IEnumerator WaitForUI()
		{
			EnableDisableListener.RegisterSafe();
			while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null)
			{
				yield return null;
			}
			foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
			{
				bool flag = gameObject.name.Contains("UserInterface");
				if (flag)
				{
					MenuEx._getUserInterface = gameObject;
				}
				else
				{
					bool flag2 = gameObject.name.Contains("_Application");
					if (flag2)
					{
						MenuEx._getApplication = gameObject;
					}
				}
				gameObject = null;
			}
			IEnumerator<GameObject> enumerator = null;
			while (MenuEx._getUserInterface == null || MenuEx._getApplication == null)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x0600019D RID: 413 RVA: 0x000095EC File Offset: 0x000077EC
		public static ActionMenuController ActionMenuInstance
		{
			get
			{
				bool flag = MenuEx._getActionMenuInstance == null;
				if (flag)
				{
					MelonCoroutines.Start(MenuEx.WaitForActionMenu());
					MenuEx.Patch();
				}
				return MenuEx._getActionMenuInstance;
			}
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00009625 File Offset: 0x00007825
		private static IEnumerator WaitForActionMenu()
		{
			while (ActionMenuController.field_Public_Static_ActionMenuController_0 == null)
			{
				yield return null;
			}
			MenuEx._getActionMenuInstance = ActionMenuController.field_Public_Static_ActionMenuController_0;
			yield break;
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x0600019F RID: 415 RVA: 0x00009630 File Offset: 0x00007830
		public static MainMenu MMInstance
		{
			get
			{
				bool flag = MenuEx._mainMenuInstance == null;
				if (flag)
				{
					GameObject userInterface = MenuEx.userInterface;
					MenuEx._mainMenuInstance = ((userInterface != null) ? userInterface.GetComponentInChildren<MainMenu>(true) : null);
				}
				return MenuEx._mainMenuInstance;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00009670 File Offset: 0x00007870
		public static GameObject MMenuParent
		{
			get
			{
				bool flag = MenuEx._MmenuParent == null;
				if (flag)
				{
					MainMenu mminstance = MenuEx.MMInstance;
					MenuEx._MmenuParent = ((mminstance != null) ? mminstance.transform.Find("Container/MMParent") : null);
				}
				return MenuEx._MmenuParent.gameObject;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x000096BC File Offset: 0x000078BC
		public static Transform QMenuParent
		{
			get
			{
				bool flag = MenuEx._menuParent == null;
				if (flag)
				{
					QuickMenu qminstance = MenuEx.QMInstance;
					MenuEx._menuParent = ((qminstance != null) ? qminstance.transform.Find("CanvasGroup/Container/Window/QMParent") : null);
				}
				return MenuEx._menuParent;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00009704 File Offset: 0x00007904
		public static Transform QMenuTabs
		{
			get
			{
				bool flag = MenuEx._qmenuTabs == null;
				if (flag)
				{
					QuickMenu qminstance = MenuEx.QMInstance;
					MenuEx._qmenuTabs = ((qminstance != null) ? qminstance.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup") : null);
				}
				return MenuEx._qmenuTabs;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0000974C File Offset: 0x0000794C
		public static Transform MMenuTabs
		{
			get
			{
				bool flag = MenuEx._MmenuTabs == null;
				if (flag)
				{
					MainMenu mminstance = MenuEx.MMInstance;
					MenuEx._MmenuTabs = ((mminstance != null) ? mminstance.transform.Find("Container/PageButtons/HorizontalLayoutGroup") : null);
				}
				return MenuEx._MmenuTabs;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00009794 File Offset: 0x00007994
		public static MenuStateController QMenuStateCtrl
		{
			get
			{
				bool flag = MenuEx._menuStateCtrl == null;
				if (flag)
				{
					QuickMenu qminstance = MenuEx.QMInstance;
					MenuEx._menuStateCtrl = ((qminstance != null) ? qminstance.GetComponent<MenuStateController>() : null);
				}
				return MenuEx._menuStateCtrl;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000097D0 File Offset: 0x000079D0
		public static MenuStateController MMenuStateCtrl
		{
			get
			{
				bool flag = MenuEx._mmenuStateCtrl == null;
				if (flag)
				{
					MainMenu mminstance = MenuEx.MMInstance;
					MenuEx._mmenuStateCtrl = ((mminstance != null) ? mminstance.GetComponent<MenuStateController>() : null);
				}
				return MenuEx._mmenuStateCtrl;
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x0000980C File Offset: 0x00007A0C
		public static SelectedUserMenuQM QMSelectedUserLocal
		{
			get
			{
				bool flag = MenuEx._qmselectedUserLocal == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					SelectedUserMenuQM qmselectedUserLocal;
					if (qmenuParent == null)
					{
						qmselectedUserLocal = null;
					}
					else
					{
						Transform transform = qmenuParent.Find("Menu_SelectedUser_Local");
						qmselectedUserLocal = ((transform != null) ? transform.GetComponent<SelectedUserMenuQM>() : null);
					}
					MenuEx._qmselectedUserLocal = qmselectedUserLocal;
				}
				return MenuEx._qmselectedUserLocal;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000985C File Offset: 0x00007A5C
		public static Transform QMDashboardMenu
		{
			get
			{
				bool flag = MenuEx._qmdashboardMenu == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					MenuEx._qmdashboardMenu = ((qmenuParent != null) ? qmenuParent.Find("Menu_Dashboard") : null);
				}
				return MenuEx._qmdashboardMenu;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x000098A0 File Offset: 0x00007AA0
		public static Transform MMDashboardMenu
		{
			get
			{
				bool flag = MenuEx._MMdashboardMenu == null;
				if (flag)
				{
					GameObject mmenuParent = MenuEx.MMenuParent;
					MenuEx._MMdashboardMenu = ((mmenuParent != null) ? mmenuParent.transform.Find("Menu_Dashboard") : null);
				}
				return MenuEx._MMdashboardMenu;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x000098E8 File Offset: 0x00007AE8
		public static Transform MMTargetboardMenu
		{
			get
			{
				bool flag = MenuEx._MMTargetboardMenu == null;
				if (flag)
				{
					MenuEx._MMTargetboardMenu = MenuEx.MMenuParent.transform.Find("Menu_UserDetail");
				}
				return MenuEx._MMTargetboardMenu;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00009928 File Offset: 0x00007B28
		public static Transform QMNotificationMenu
		{
			get
			{
				bool flag = MenuEx._qmnotificationMenu == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					MenuEx._qmnotificationMenu = ((qmenuParent != null) ? qmenuParent.Find("Menu_Notifications") : null);
				}
				return MenuEx._qmnotificationMenu;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000996C File Offset: 0x00007B6C
		public static Transform QMHereMenu
		{
			get
			{
				bool flag = MenuEx._qmhereMenu == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					MenuEx._qmhereMenu = ((qmenuParent != null) ? qmenuParent.Find("Menu_Here") : null);
				}
				return MenuEx._qmhereMenu;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001AC RID: 428 RVA: 0x000099B0 File Offset: 0x00007BB0
		public static Transform QMCameraMenu
		{
			get
			{
				bool flag = MenuEx._qmcameraMenu == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					MenuEx._qmcameraMenu = ((qmenuParent != null) ? qmenuParent.Find("Menu_Camera") : null);
				}
				return MenuEx._qmcameraMenu;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001AD RID: 429 RVA: 0x000099F4 File Offset: 0x00007BF4
		public static Transform QMAudioSettingsMenu
		{
			get
			{
				bool flag = MenuEx._qmaudiosettingsMenu == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					MenuEx._qmaudiosettingsMenu = ((qmenuParent != null) ? qmenuParent.Find("Menu_QM_AudioSettings") : null);
				}
				return MenuEx._qmaudiosettingsMenu;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00009A38 File Offset: 0x00007C38
		public static Transform QMSettingsMenu
		{
			get
			{
				bool flag = MenuEx._qmsettingsMenu == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					MenuEx._qmsettingsMenu = ((qmenuParent != null) ? qmenuParent.Find("Menu_QM_GeneralSettings") : null);
				}
				return MenuEx._qmsettingsMenu;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001AF RID: 431 RVA: 0x00009A7C File Offset: 0x00007C7C
		public static Transform QMDevToolsMenu
		{
			get
			{
				bool flag = MenuEx._qmdevtoolsMenu == null;
				if (flag)
				{
					Transform qmenuParent = MenuEx.QMenuParent;
					MenuEx._qmdevtoolsMenu = ((qmenuParent != null) ? qmenuParent.Find("Menu_DevTools") : null);
				}
				return MenuEx._qmdevtoolsMenu;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00009AC0 File Offset: 0x00007CC0
		public static GameObject QMLeftWing
		{
			get
			{
				bool flag = MenuEx._qmleftWing == null;
				if (flag)
				{
					QuickMenu qminstance = MenuEx.QMInstance;
					GameObject qmleftWing;
					if (qminstance == null)
					{
						qmleftWing = null;
					}
					else
					{
						Transform transform = qminstance.transform.Find("CanvasGroup/Container/Window/Wing_Left");
						qmleftWing = ((transform != null) ? transform.gameObject : null);
					}
					MenuEx._qmleftWing = qmleftWing;
				}
				return MenuEx._qmleftWing;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x00009B14 File Offset: 0x00007D14
		public static GameObject QMRightWing
		{
			get
			{
				bool flag = MenuEx._qmrightWing == null;
				if (flag)
				{
					QuickMenu qminstance = MenuEx.QMInstance;
					GameObject qmrightWing;
					if (qminstance == null)
					{
						qmrightWing = null;
					}
					else
					{
						Transform transform = qminstance.transform.Find("CanvasGroup/Container/Window/Wing_Right");
						qmrightWing = ((transform != null) ? transform.gameObject : null);
					}
					MenuEx._qmrightWing = qmrightWing;
				}
				return MenuEx._qmrightWing;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00009B68 File Offset: 0x00007D68
		public static Sprite OnIconSprite
		{
			get
			{
				try
				{
					bool flag = MenuEx._onIconSprite == null;
					if (flag)
					{
						MenuEx._onIconSprite = Enumerable.FirstOrDefault<Sprite>(Resources.FindObjectsOfTypeAll<Sprite>(), (Sprite x) => x.name == "Toggle_ON" || x.name == "ON");
						bool flag2 = MenuEx._onIconSprite == null && MenuEx.QMNotificationMenu != null;
						if (flag2)
						{
							Transform transform = MenuEx.QMNotificationMenu.Find("Panel_NoNotifications_Message/Icon");
							bool flag3 = transform != null;
							if (flag3)
							{
								ImageEx component = transform.GetComponent<ImageEx>();
								bool flag4 = component != null;
								if (flag4)
								{
									MenuEx._onIconSprite = ((Image)component).sprite;
								}
							}
						}
						bool flag5 = MenuEx._onIconSprite == null;
						if (flag5)
						{
							Texture2D texture2D = new Texture2D(64, 64);
							Il2CppStructArray<Color> pixels = texture2D.GetPixels();
							for (int i = 0; i < pixels.Length; i++)
							{
								pixels[i] = Color.white;
							}
							texture2D.SetPixels(pixels);
							texture2D.Apply();
							MenuEx._onIconSprite = Sprite.Create(texture2D, new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f));
						}
					}
				}
				catch (Exception arg)
				{
					MelonLogger.Warning(string.Format("Failed to load OnIconSprite: {0}", arg));
				}
				return MenuEx._onIconSprite;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x00009CFC File Offset: 0x00007EFC
		public static Sprite OffIconSprite
		{
			get
			{
				try
				{
					bool flag = MenuEx._offIconSprite == null;
					if (flag)
					{
						MenuEx._offIconSprite = Enumerable.FirstOrDefault<Sprite>(Resources.FindObjectsOfTypeAll<Sprite>(), (Sprite x) => x.name == "Toggle_OFF" || x.name == "OFF");
						bool flag2 = MenuEx._offIconSprite == null && MenuEx.QMNotificationMenu != null;
						if (flag2)
						{
							Transform transform = MenuEx.QMNotificationMenu.Find("Panel_Notification_Tabs/Button_ClearNotifications/Text_FieldContent/Icon");
							bool flag3 = transform != null;
							if (flag3)
							{
								ImageEx component = transform.GetComponent<ImageEx>();
								bool flag4 = component != null;
								if (flag4)
								{
									MenuEx._offIconSprite = ((Image)component).sprite;
								}
							}
						}
						bool flag5 = MenuEx._offIconSprite == null;
						if (flag5)
						{
							Texture2D texture2D = new Texture2D(64, 64);
							Il2CppStructArray<Color> pixels = texture2D.GetPixels();
							for (int i = 0; i < pixels.Length; i++)
							{
								pixels[i] = Color.gray;
							}
							texture2D.SetPixels(pixels);
							texture2D.Apply();
							MenuEx._offIconSprite = Sprite.Create(texture2D, new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f));
						}
					}
				}
				catch (Exception arg)
				{
					MelonLogger.Warning(string.Format("Failed to load OffIconSprite: {0}", arg));
				}
				return MenuEx._offIconSprite;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00009E90 File Offset: 0x00008090
		public static GameObject MMSettingsMenu
		{
			get
			{
				bool flag = MenuEx._MMSettingsMenu == null;
				if (flag)
				{
					GameObject mmenuParent = MenuEx.MMenuParent;
					GameObject mmsettingsMenu;
					if (mmenuParent == null)
					{
						mmsettingsMenu = null;
					}
					else
					{
						Transform transform = mmenuParent.transform.Find("Menu_Settings");
						mmsettingsMenu = ((transform != null) ? transform.gameObject : null);
					}
					MenuEx._MMSettingsMenu = mmsettingsMenu;
				}
				return MenuEx._MMSettingsMenu;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x00009EE4 File Offset: 0x000080E4
		public static GameObject MMReMod
		{
			get
			{
				bool flag = MenuEx._MMReMod == null;
				if (flag)
				{
					GameObject mmenuParent = MenuEx.MMenuParent;
					GameObject mmreMod;
					if (mmenuParent == null)
					{
						mmreMod = null;
					}
					else
					{
						Transform transform = mmenuParent.transform.Find("Menu_ReMod");
						mmreMod = ((transform != null) ? transform.gameObject : null);
					}
					MenuEx._MMReMod = mmreMod;
				}
				return MenuEx._MMReMod;
			}
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x00009F38 File Offset: 0x00008138
		public static IEnumerable<Type> TryGetTypes(Assembly asm)
		{
			IEnumerable<Type> result;
			try
			{
				result = asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				try
				{
					result = asm.GetExportedTypes();
				}
				catch
				{
					result = Enumerable.Where<Type>(ex.Types, (Type t) => t != null);
				}
			}
			catch
			{
				result = Enumerable.Empty<Type>();
			}
			return result;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00009FC0 File Offset: 0x000081C0
		public static void Patch()
		{
			bool flag = !MenuEx.wasPatched;
			if (flag)
			{
				MenuEx.wasPatched = true;
				ReModPatches.Patch();
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x00009FE8 File Offset: 0x000081E8
		public static IEnumerator WaitForUInPatch()
		{
			MenuEx.Patch();
			while (VRCUiManager.field_Private_Static_VRCUiManager_0 == null)
			{
				yield return null;
			}
			while (MenuEx.userInterface == null)
			{
				yield return null;
			}
			while (MenuEx.QMInstance == null)
			{
				yield return null;
			}
			while (MenuEx.MMInstance == null)
			{
				yield return null;
			}
			while (MenuEx.ActionMenuInstance == null)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x00009FF0 File Offset: 0x000081F0
		public static void Initialize()
		{
			MelonCoroutines.Start(MenuEx.InitializeRoutine());
		}

		// Token: 0x060001BA RID: 442 RVA: 0x00009FFE File Offset: 0x000081FE
		private static IEnumerator InitializeRoutine()
		{
			yield return MenuEx.WaitForUInPatch();
			Sprite _ = MenuEx.OnIconSprite;
			Sprite __ = MenuEx.OffIconSprite;
			MelonLogger.Msg("MenuEx initialized successfully");
			yield break;
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000A008 File Offset: 0x00008208
		public static Transform QMWingMenuContent(GameObject qmwing)
		{
			return (qmwing != null) ? qmwing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup") : null;
		}

		// Token: 0x04000087 RID: 135
		private static QuickMenu _quickMenuInstance;

		// Token: 0x04000088 RID: 136
		private static GameObject _getUserInterface;

		// Token: 0x04000089 RID: 137
		private static GameObject _getApplication;

		// Token: 0x0400008A RID: 138
		private static ActionMenuController _getActionMenuInstance;

		// Token: 0x0400008B RID: 139
		private static bool wasPatched;

		// Token: 0x0400008C RID: 140
		private static MainMenu _mainMenuInstance;

		// Token: 0x0400008D RID: 141
		private static Transform _MmenuParent;

		// Token: 0x0400008E RID: 142
		private static Transform _menuParent;

		// Token: 0x0400008F RID: 143
		private static Transform _qmenuTabs;

		// Token: 0x04000090 RID: 144
		private static Transform _MmenuTabs;

		// Token: 0x04000091 RID: 145
		private static MenuStateController _menuStateCtrl;

		// Token: 0x04000092 RID: 146
		private static MenuStateController _mmenuStateCtrl;

		// Token: 0x04000093 RID: 147
		private static SelectedUserMenuQM _qmselectedUserLocal;

		// Token: 0x04000094 RID: 148
		private static Transform _qmdashboardMenu;

		// Token: 0x04000095 RID: 149
		private static Transform _MMdashboardMenu;

		// Token: 0x04000096 RID: 150
		private static Transform _MMTargetboardMenu;

		// Token: 0x04000097 RID: 151
		private static Transform _qmnotificationMenu;

		// Token: 0x04000098 RID: 152
		private static Transform _qmhereMenu;

		// Token: 0x04000099 RID: 153
		private static Transform _qmcameraMenu;

		// Token: 0x0400009A RID: 154
		private static Transform _qmaudiosettingsMenu;

		// Token: 0x0400009B RID: 155
		private static Transform _qmsettingsMenu;

		// Token: 0x0400009C RID: 156
		private static Transform _qmdevtoolsMenu;

		// Token: 0x0400009D RID: 157
		private static GameObject _qmleftWing;

		// Token: 0x0400009E RID: 158
		private static GameObject _qmrightWing;

		// Token: 0x0400009F RID: 159
		private static Sprite _onIconSprite;

		// Token: 0x040000A0 RID: 160
		private static Sprite _offIconSprite;

		// Token: 0x040000A1 RID: 161
		private static GameObject _MMSettingsMenu;

		// Token: 0x040000A2 RID: 162
		private static GameObject _MMReMod;
	}
}
