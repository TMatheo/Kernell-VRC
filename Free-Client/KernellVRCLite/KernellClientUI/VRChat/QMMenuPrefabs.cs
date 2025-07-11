using System;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.VRChat
{
	// Token: 0x0200002D RID: 45
	public static class QMMenuPrefabs
	{
		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000B008 File Offset: 0x00009208
		public static GameObject TogglePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._togglePrefab == null;
				if (flag)
				{
					QMMenuPrefabs._togglePrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions/SitStandCalibrateButton/Button_SitStand").gameObject;
				}
				return QMMenuPrefabs._togglePrefab;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000B064 File Offset: 0x00009264
		public static GameObject SliderPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._sliderPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._sliderPrefab = MenuEx.QMAudioSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AudioVolume/QM_Settings_Panel/VerticalLayoutGroup/Master").gameObject;
				}
				return QMMenuPrefabs._sliderPrefab;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000B0C0 File Offset: 0x000092C0
		public static GameObject WingButtonPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._wingButtonPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._wingButtonPrefab = MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect").GetComponent<ScrollRect>().content.Find("Button_Profile").gameObject;
				}
				return QMMenuPrefabs._wingButtonPrefab;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001F0 RID: 496 RVA: 0x0000B11C File Offset: 0x0000931C
		public static GameObject WingMenuPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._wingMenuPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._wingMenuPrefab = MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu").gameObject;
				}
				return QMMenuPrefabs._wingMenuPrefab;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000B164 File Offset: 0x00009364
		public static GameObject TabButtonPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._tabButtonPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._tabButtonPrefab = MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/Page_Buttons_QM/HorizontalLayoutGroup/Page_Settings").gameObject;
				}
				return QMMenuPrefabs._tabButtonPrefab;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000B1AC File Offset: 0x000093AC
		public static GameObject ContainerPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._ContainerPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._ContainerPrefab = MenuEx.QMAudioSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.gameObject;
				}
				return QMMenuPrefabs._ContainerPrefab;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001F3 RID: 499 RVA: 0x0000B1FC File Offset: 0x000093FC
		public static GameObject RadioTogglePagePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._radioTogglePagePrefab == null;
				if (flag)
				{
					QMMenuPrefabs._radioTogglePagePrefab = MenuEx.QMenuParent.transform.Find("Menu_ChangeAudioInputDevice").gameObject;
				}
				return QMMenuPrefabs._radioTogglePagePrefab;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000B244 File Offset: 0x00009444
		public static GameObject RadioTogglePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._radioTogglePrefab == null;
				if (flag)
				{
					GameObject gameObject = MenuEx.QMenuParent.transform.Find("Menu_ChangeAudioInputDevice").gameObject;
					MonoBehaviour1PublicBuExVoStVoVoVoVoVoVo0 component = gameObject.GetComponent<MonoBehaviour1PublicBuExVoStVoVoVoVoVoVo0>();
					QMMenuPrefabs._radioTogglePrefab = component.gameObject;
				}
				return QMMenuPrefabs._radioTogglePrefab;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001F5 RID: 501 RVA: 0x0000B298 File Offset: 0x00009498
		public static GameObject MenuPagePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._menuPagePrefab == null;
				if (flag)
				{
					QMMenuPrefabs._menuPagePrefab = MenuEx.QMDevToolsMenu.gameObject;
				}
				return QMMenuPrefabs._menuPagePrefab;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000B2D0 File Offset: 0x000094D0
		public static GameObject CategoryPagePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._categoryPagePrefab == null;
				if (flag)
				{
					QMMenuPrefabs._categoryPagePrefab = MenuEx.QMDashboardMenu.gameObject;
				}
				return QMMenuPrefabs._categoryPagePrefab;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000B308 File Offset: 0x00009508
		public static GameObject TabbedPagePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._tabbedPagePrefab == null;
				if (flag)
				{
					QMMenuPrefabs._tabbedPagePrefab = MenuEx.QMNotificationMenu.gameObject;
				}
				return QMMenuPrefabs._tabbedPagePrefab;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0000B340 File Offset: 0x00009540
		public static GameObject TabPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._tabPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._tabPrefab = MenuEx.QMNotificationMenu.transform.Find("Panel_Notification_Tabs/Tabs/InvitesTab").gameObject;
				}
				return QMMenuPrefabs._tabPrefab;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000B388 File Offset: 0x00009588
		public static GameObject TabContentPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._tabcontentPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._tabcontentPrefab = MenuEx.QMNotificationMenu.transform.Find("Panel_Content/Invites").gameObject;
				}
				return QMMenuPrefabs._tabcontentPrefab;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0000B3D0 File Offset: 0x000095D0
		public static GameObject ButtonPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._buttonPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._buttonPrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions/Button_Respawn").gameObject;
				}
				return QMMenuPrefabs._buttonPrefab;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001FB RID: 507 RVA: 0x0000B42C File Offset: 0x0000962C
		public static GameObject LabelPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._labelPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._labelPrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("Debug/QM_Settings_Panel/VerticalLayoutGroup/Stats/LeftItemContainer/Cell_QM_SettingStat").gameObject;
				}
				return QMMenuPrefabs._labelPrefab;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000B488 File Offset: 0x00009688
		public static GameObject MenuCategoryHeaderPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._henuCategoryHeaderPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._henuCategoryHeaderPrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Header_QuickActions").gameObject;
				}
				return QMMenuPrefabs._henuCategoryHeaderPrefab;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000B4E4 File Offset: 0x000096E4
		public static GameObject MenuCategoryHeaderCollapsiblePrefav
		{
			get
			{
				bool flag = QMMenuPrefabs._menuCategoryHeaderCollapsiblePrefav == null;
				if (flag)
				{
					QMMenuPrefabs._menuCategoryHeaderCollapsiblePrefav = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("UIElements/QM_Foldout").gameObject;
				}
				return QMMenuPrefabs._menuCategoryHeaderCollapsiblePrefav;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0000B540 File Offset: 0x00009740
		public static GameObject MenuCategoryContainerPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._menuCategoryContainerPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._menuCategoryContainerPrefab = MenuEx.QMDashboardMenu.transform.Find("ScrollRect").GetComponent<ScrollRect>().content.Find("Buttons_QuickActions").gameObject;
				}
				return QMMenuPrefabs._menuCategoryContainerPrefab;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000B59C File Offset: 0x0000979C
		public static GameObject NewContainerPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._newContainerPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._newContainerPrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AdvancedOptions/QM_Settings_Panel/VerticalLayoutGroup/").gameObject;
				}
				return QMMenuPrefabs._newContainerPrefab;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000200 RID: 512 RVA: 0x0000B5F8 File Offset: 0x000097F8
		public static GameObject NewBackgroundPrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._newBackgroundPrefab == null;
				if (flag)
				{
					QMMenuPrefabs._newBackgroundPrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AdvancedOptions/QM_Settings_Panel/VerticalLayoutGroup/Background_Info").gameObject;
				}
				return QMMenuPrefabs._newBackgroundPrefab;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0000B654 File Offset: 0x00009854
		public static GameObject MenuCategoryTogglePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._menuCategoryTogglePrefab == null;
				if (flag)
				{
					QMMenuPrefabs._menuCategoryTogglePrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AdvancedOptions/QM_Settings_Panel/VerticalLayoutGroup/AllowHorizonAdjust").gameObject;
				}
				return QMMenuPrefabs._menuCategoryTogglePrefab;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000202 RID: 514 RVA: 0x0000B6B0 File Offset: 0x000098B0
		public static GameObject SliderTogglePrefab
		{
			get
			{
				bool flag = QMMenuPrefabs._sliderTogglePrefab == null;
				if (flag)
				{
					QMMenuPrefabs._sliderTogglePrefab = MenuEx.QMSettingsMenu.transform.Find("Panel_QM_ScrollRect").GetComponent<ScrollRect>().content.Find("AvatarCulling/QM_Settings_Panel/VerticalLayoutGroup/HideBeyond").gameObject;
				}
				return QMMenuPrefabs._sliderTogglePrefab;
			}
		}

		// Token: 0x040000BB RID: 187
		private static GameObject _togglePrefab;

		// Token: 0x040000BC RID: 188
		private static GameObject _sliderPrefab;

		// Token: 0x040000BD RID: 189
		private static GameObject _wingButtonPrefab;

		// Token: 0x040000BE RID: 190
		private static GameObject _wingMenuPrefab;

		// Token: 0x040000BF RID: 191
		private static GameObject _tabButtonPrefab;

		// Token: 0x040000C0 RID: 192
		private static GameObject _ContainerPrefab;

		// Token: 0x040000C1 RID: 193
		private static GameObject _radioTogglePagePrefab;

		// Token: 0x040000C2 RID: 194
		private static GameObject _radioTogglePrefab;

		// Token: 0x040000C3 RID: 195
		private static GameObject _menuPagePrefab;

		// Token: 0x040000C4 RID: 196
		private static GameObject _categoryPagePrefab;

		// Token: 0x040000C5 RID: 197
		private static GameObject _tabbedPagePrefab;

		// Token: 0x040000C6 RID: 198
		private static GameObject _tabPrefab;

		// Token: 0x040000C7 RID: 199
		private static GameObject _tabcontentPrefab;

		// Token: 0x040000C8 RID: 200
		private static GameObject _buttonPrefab;

		// Token: 0x040000C9 RID: 201
		private static GameObject _labelPrefab;

		// Token: 0x040000CA RID: 202
		private static GameObject _henuCategoryHeaderPrefab;

		// Token: 0x040000CB RID: 203
		private static GameObject _menuCategoryHeaderCollapsiblePrefav;

		// Token: 0x040000CC RID: 204
		private static GameObject _menuCategoryContainerPrefab;

		// Token: 0x040000CD RID: 205
		private static GameObject _newContainerPrefab;

		// Token: 0x040000CE RID: 206
		private static GameObject _newBackgroundPrefab;

		// Token: 0x040000CF RID: 207
		private static GameObject _menuCategoryTogglePrefab;

		// Token: 0x040000D0 RID: 208
		private static GameObject _sliderTogglePrefab;
	}
}
