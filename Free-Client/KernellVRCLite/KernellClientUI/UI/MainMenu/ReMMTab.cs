using System;
using KernellClientUI.VRChat;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Elements.Controls;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x02000063 RID: 99
	public class ReMMTab : UiElement
	{
		// Token: 0x06000457 RID: 1111 RVA: 0x00019468 File Offset: 0x00017668
		public ReMMTab(string name, string tooltip, string pageName, Sprite sprite) : base(MMenuPrefabs.MMTabButtonPrefab, MMenuPrefabs.MMTabButtonPrefab.transform.parent, "Page_" + name, true)
		{
			try
			{
				MenuTab component = base.RectTransform.GetComponent<MenuTab>();
				component.name = UiElement.GetCleanName("MainMenuReMod" + pageName);
				component.field_Private_MenuStateController_0 = MenuEx.MMenuStateCtrl;
				Button component4 = base.GameObject.GetComponent<Button>();
				component4.onClick = new Button.ButtonClickedEvent();
				component4.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate()
				{
					component.field_Private_MenuStateController_0.ShowTabContent();
				})));
				LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
				ToolTip component2 = base.GameObject.GetComponent<ToolTip>();
				component2._alternateLocalizableString = localizableString;
				component2._localizableString = localizableString;
				Image component3 = base.RectTransform.Find("Icon").GetComponent<Image>();
				component3.sprite = sprite;
				component3.overrideSprite = sprite;
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("Error creating ReMMTab: {0}", arg));
			}
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x00019594 File Offset: 0x00017794
		public static ReMMTab Create(string name, string tooltip, string pageName, Sprite sprite)
		{
			return new ReMMTab(name, tooltip, pageName, sprite);
		}
	}
}
