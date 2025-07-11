using System;
using Il2CppSystem;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace KernellVRCLite.API
{
	// Token: 0x020000A9 RID: 169
	internal static class APIUtils
	{
		// Token: 0x060008B3 RID: 2227 RVA: 0x00035530 File Offset: 0x00033730
		public static void Initialize()
		{
			bool initialized = APIUtils._initialized;
			if (!initialized)
			{
				try
				{
					Il2CppArrayBase<QuickMenu> il2CppArrayBase = Object.FindObjectsOfType<QuickMenu>();
					bool flag = il2CppArrayBase != null && il2CppArrayBase.Length > 0;
					if (flag)
					{
						APIUtils._quickMenu = il2CppArrayBase[0];
					}
					Il2CppArrayBase<MainMenu> il2CppArrayBase2 = Object.FindObjectsOfType<MainMenu>();
					bool flag2 = il2CppArrayBase2 != null && il2CppArrayBase2.Length > 0;
					if (flag2)
					{
						APIUtils._socialMenu = il2CppArrayBase2[0];
					}
					Il2CppArrayBase<UserInterface> il2CppArrayBase3 = Object.FindObjectsOfType<UserInterface>();
					bool flag3 = il2CppArrayBase3 != null && il2CppArrayBase3.Length > 0;
					if (flag3)
					{
						APIUtils._userInterface = il2CppArrayBase3[0];
					}
					bool flag4 = APIUtils._quickMenu != null;
					if (flag4)
					{
						Transform transform = APIUtils._quickMenu.transform;
						Transform transform2 = transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard");
						bool flag5 = transform2 != null;
						if (flag5)
						{
							APIUtils._qmMenuTemplate = transform2.gameObject;
						}
						Transform transform3 = transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings");
						bool flag6 = transform3 != null;
						if (flag6)
						{
							APIUtils._qmTabTemplate = transform3.gameObject;
						}
						Transform transform4 = transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks/Button_Worlds");
						bool flag7 = transform4 != null;
						if (flag7)
						{
							APIUtils._qmButtonTemplate = transform4.gameObject;
						}
						Transform transform5 = transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon");
						bool flag8 = transform5 != null;
						if (flag8)
						{
							Image component = transform5.GetComponent<Image>();
							bool flag9 = component != null;
							if (flag9)
							{
								APIUtils._onSprite = component.sprite;
							}
						}
						Transform transform6 = transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Notifications/Panel_Notification_Tabs/Button_ClearNotifications/Text_FieldContent/Icon");
						bool flag10 = transform6 != null;
						if (flag10)
						{
							Image component2 = transform6.GetComponent<Image>();
							bool flag11 = component2 != null;
							if (flag11)
							{
								APIUtils._offSprite = component2.sprite;
							}
						}
					}
					bool flag12 = APIUtils._socialMenu != null;
					if (flag12)
					{
						Transform transform7 = APIUtils._socialMenu.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/AboutText/Panel_Description_Expandable");
						bool flag13 = transform7 != null;
						if (flag13)
						{
							APIUtils._qmInfoPanelTemplate = transform7.gameObject;
						}
					}
					APIUtils._initialized = true;
				}
				catch (Exception arg)
				{
					Debug.LogError(string.Format("[APIUtils] Failed to initialize: {0}", arg));
				}
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x00035764 File Offset: 0x00033964
		public static UserInterface UserInterface
		{
			get
			{
				bool flag = APIUtils._userInterface == null;
				if (flag)
				{
					Il2CppArrayBase<UserInterface> il2CppArrayBase = Object.FindObjectsOfType<UserInterface>();
					bool flag2 = il2CppArrayBase != null && il2CppArrayBase.Length > 0;
					if (flag2)
					{
						APIUtils._userInterface = il2CppArrayBase[0];
					}
				}
				return APIUtils._userInterface;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x000357B4 File Offset: 0x000339B4
		public static QuickMenu QuickMenuInstance
		{
			get
			{
				bool flag = APIUtils._quickMenu == null;
				if (flag)
				{
					Il2CppArrayBase<QuickMenu> il2CppArrayBase = Object.FindObjectsOfType<QuickMenu>();
					bool flag2 = il2CppArrayBase != null && il2CppArrayBase.Length > 0;
					if (flag2)
					{
						APIUtils._quickMenu = il2CppArrayBase[0];
					}
				}
				return APIUtils._quickMenu;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x00035804 File Offset: 0x00033A04
		public static MainMenu SocialMenuInstance
		{
			get
			{
				bool flag = APIUtils._socialMenu == null;
				if (flag)
				{
					Il2CppArrayBase<MainMenu> il2CppArrayBase = Object.FindObjectsOfType<MainMenu>();
					bool flag2 = il2CppArrayBase != null && il2CppArrayBase.Length > 0;
					if (flag2)
					{
						APIUtils._socialMenu = il2CppArrayBase[0];
					}
				}
				return APIUtils._socialMenu;
			}
		}

		// Token: 0x060008B7 RID: 2231 RVA: 0x00035854 File Offset: 0x00033A54
		public static GameObject GetQMMenuTemplate()
		{
			bool flag = APIUtils._qmMenuTemplate == null && APIUtils.QuickMenuInstance != null;
			if (flag)
			{
				Transform transform = APIUtils.QuickMenuInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard");
				bool flag2 = transform != null;
				if (flag2)
				{
					APIUtils._qmMenuTemplate = transform.gameObject;
				}
			}
			return APIUtils._qmMenuTemplate;
		}

		// Token: 0x060008B8 RID: 2232 RVA: 0x000358B8 File Offset: 0x00033AB8
		public static GameObject GetQMTabButtonTemplate()
		{
			bool flag = APIUtils._qmTabTemplate == null && APIUtils.QuickMenuInstance != null;
			if (flag)
			{
				Transform transform = APIUtils.QuickMenuInstance.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings");
				bool flag2 = transform != null;
				if (flag2)
				{
					APIUtils._qmTabTemplate = transform.gameObject;
				}
			}
			return APIUtils._qmTabTemplate;
		}

		// Token: 0x060008B9 RID: 2233 RVA: 0x0003591C File Offset: 0x00033B1C
		public static GameObject GetQMButtonTemplate()
		{
			bool flag = APIUtils._qmButtonTemplate == null && APIUtils.QuickMenuInstance != null;
			if (flag)
			{
				Transform transform = APIUtils.QuickMenuInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks/Button_Worlds");
				bool flag2 = transform != null;
				if (flag2)
				{
					APIUtils._qmButtonTemplate = transform.gameObject;
				}
			}
			return APIUtils._qmButtonTemplate;
		}

		// Token: 0x060008BA RID: 2234 RVA: 0x00035980 File Offset: 0x00033B80
		public static GameObject GetQMInfoPanelTemplate()
		{
			bool flag = APIUtils._qmInfoPanelTemplate == null && APIUtils.SocialMenuInstance != null;
			if (flag)
			{
				Transform transform = APIUtils.SocialMenuInstance.transform.Find("Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/AboutText/Panel_Description_Expandable");
				bool flag2 = transform != null;
				if (flag2)
				{
					APIUtils._qmInfoPanelTemplate = transform.gameObject;
				}
			}
			return APIUtils._qmInfoPanelTemplate;
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x000359E4 File Offset: 0x00033BE4
		public static Sprite OnIconSprite()
		{
			bool flag = APIUtils._onSprite == null && APIUtils.QuickMenuInstance != null;
			if (flag)
			{
				Transform transform = APIUtils.QuickMenuInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon");
				bool flag2 = transform != null;
				if (flag2)
				{
					Image component = transform.GetComponent<Image>();
					bool flag3 = component != null;
					if (flag3)
					{
						APIUtils._onSprite = component.sprite;
					}
				}
			}
			return APIUtils._onSprite;
		}

		// Token: 0x060008BC RID: 2236 RVA: 0x00035A60 File Offset: 0x00033C60
		public static Sprite OffIconSprite()
		{
			bool flag = APIUtils._offSprite == null && APIUtils.QuickMenuInstance != null;
			if (flag)
			{
				Transform transform = APIUtils.QuickMenuInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Notifications/Panel_Notification_Tabs/Button_ClearNotifications/Text_FieldContent/Icon");
				bool flag2 = transform != null;
				if (flag2)
				{
					Image component = transform.GetComponent<Image>();
					bool flag3 = component != null;
					if (flag3)
					{
						APIUtils._offSprite = component.sprite;
					}
				}
			}
			return APIUtils._offSprite;
		}

		// Token: 0x060008BD RID: 2237 RVA: 0x00035ADC File Offset: 0x00033CDC
		public static int RandomNumbers()
		{
			return APIUtils.rnd.Next(100000, 999999);
		}

		// Token: 0x060008BE RID: 2238 RVA: 0x00035B04 File Offset: 0x00033D04
		public static void DestroyChildren(Transform transform)
		{
			bool flag = transform == null;
			if (!flag)
			{
				APIUtils.DestroyChildren(transform, null);
			}
		}

		// Token: 0x060008BF RID: 2239 RVA: 0x00035B28 File Offset: 0x00033D28
		public static void DestroyChildren(Transform transform, Func<Transform, bool> exclude)
		{
			bool flag = transform == null;
			if (!flag)
			{
				int childCount = transform.childCount;
				for (int i = childCount - 1; i >= 0; i--)
				{
					Transform child = transform.GetChild(i);
					bool flag2 = exclude == null || exclude(child);
					if (flag2)
					{
						Object.DestroyImmediate(child.gameObject);
					}
				}
			}
		}

		// Token: 0x04000423 RID: 1059
		public const string Identifier = "Kernell";

		// Token: 0x04000424 RID: 1060
		private static readonly Random rnd = new Random();

		// Token: 0x04000425 RID: 1061
		private static QuickMenu _quickMenu;

		// Token: 0x04000426 RID: 1062
		private static MainMenu _socialMenu;

		// Token: 0x04000427 RID: 1063
		private static UserInterface _userInterface;

		// Token: 0x04000428 RID: 1064
		private static GameObject _qmMenuTemplate;

		// Token: 0x04000429 RID: 1065
		private static GameObject _qmTabTemplate;

		// Token: 0x0400042A RID: 1066
		private static GameObject _qmButtonTemplate;

		// Token: 0x0400042B RID: 1067
		private static GameObject _qmInfoPanelTemplate;

		// Token: 0x0400042C RID: 1068
		private static Sprite _onSprite;

		// Token: 0x0400042D RID: 1069
		private static Sprite _offSprite;

		// Token: 0x0400042E RID: 1070
		private const string QM_MENU_PATH = "CanvasGroup/Container/Window/QMParent/Menu_Dashboard";

		// Token: 0x0400042F RID: 1071
		private const string QM_TAB_PATH = "CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings";

		// Token: 0x04000430 RID: 1072
		private const string QM_BUTTON_PATH = "CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_QuickLinks/Button_Worlds";

		// Token: 0x04000431 RID: 1073
		private const string QM_INFO_PANEL_PATH = "Container/MMParent/Menu_WorldDetail/ScrollRect/Viewport/VerticalLayoutGroup/AboutText/Panel_Description_Expandable";

		// Token: 0x04000432 RID: 1074
		private const string ON_SPRITE_PATH = "CanvasGroup/Container/Window/QMParent/Menu_Notifications/Panel_NoNotifications_Message/Icon";

		// Token: 0x04000433 RID: 1075
		private const string OFF_SPRITE_PATH = "CanvasGroup/Container/Window/QMParent/Menu_Notifications/Panel_Notification_Tabs/Button_ClearNotifications/Text_FieldContent/Icon";

		// Token: 0x04000434 RID: 1076
		private static bool _initialized = false;
	}
}
