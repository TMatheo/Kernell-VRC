using System;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements;
using VRC.UI.Elements.Controls;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000057 RID: 87
	public class ReTabButton : UiElement
	{
		// Token: 0x060003DE RID: 990 RVA: 0x00016408 File Offset: 0x00014608
		public ReTabButton(string name, string tooltip, string pageName, Sprite sprite, ReMenuPage menus)
		{
			ReTabButton.<>c__DisplayClass0_0 CS$<>8__locals1 = new ReTabButton.<>c__DisplayClass0_0();
			CS$<>8__locals1.menus = menus;
			base..ctor(QMMenuPrefabs.TabButtonPrefab, QMMenuPrefabs.TabButtonPrefab.transform.parent, "Page_" + name, true);
			MenuTab menuTab = base.RectTransform.GetComponent<MenuTab>();
			menuTab.name = UiElement.GetCleanName("Page_" + pageName);
			menuTab._controlName = UiElement.GetCleanName("Page_" + pageName);
			menuTab.field_Private_MenuStateController_0 = MenuEx.QMenuStateCtrl;
			Button component = base.GameObject.GetComponent<Button>();
			component.onClick = new Button.ButtonClickedEvent();
			component.onClick.AddListener(delegate()
			{
				UIPage uipage = menuTab.field_Private_MenuStateController_0.Method_Public_UIPage_String_0(CS$<>8__locals1.menus.UiPage.field_Public_String_0);
				int num = 11;
				int num2 = 0;
				bool flag = uipage != null;
				if (flag)
				{
					foreach (UIPage uipage2 in menuTab.field_Private_MenuStateController_0.field_Public_ArrayOf_UIPage_0)
					{
						bool flag2 = uipage2 != null && uipage2.field_Public_String_0 == uipage.field_Public_String_0;
						if (flag2)
						{
							num = num2;
							break;
						}
						num2++;
					}
				}
				menuTab.field_Private_MenuStateController_0.ShowTabContent(num, false, false);
			});
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
			ToolTip component2 = base.GameObject.GetComponent<ToolTip>();
			component2._localizableString = localizableString;
			component2._alternateLocalizableString = localizableString;
			Image component3 = base.RectTransform.Find("Icon").GetComponent<Image>();
			component3.sprite = sprite;
			component3.overrideSprite = sprite;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001653C File Offset: 0x0001473C
		public static ReTabButton Create(string name, string tooltip, string pageName, Sprite sprite, ReMenuPage menu)
		{
			return new ReTabButton(name, tooltip, pageName, sprite, menu);
		}
	}
}
