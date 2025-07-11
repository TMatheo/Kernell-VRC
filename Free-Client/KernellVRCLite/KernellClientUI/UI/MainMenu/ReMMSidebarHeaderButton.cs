using System;
using KernellClientUI.VRChat;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x02000061 RID: 97
	public class ReMMSidebarHeaderButton : UiElement
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600044A RID: 1098 RVA: 0x00018F48 File Offset: 0x00017148
		// (set) Token: 0x0600044B RID: 1099 RVA: 0x00018F65 File Offset: 0x00017165
		public string Text
		{
			get
			{
				return this._textComponent.text;
			}
			set
			{
				this._textComponent.text = value;
			}
		}

		// Token: 0x0600044C RID: 1100 RVA: 0x00018F78 File Offset: 0x00017178
		public ReMMSidebarHeaderButton(ReMMPage menu, string text, string tooltip, Sprite icon, Action onClick, string color = "#ffffff")
		{
			ReMMSidebarHeaderButton.<>c__DisplayClass4_0 CS$<>8__locals1 = new ReMMSidebarHeaderButton.<>c__DisplayClass4_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.text = text;
			base..ctor(MMenuPrefabs.MMSideBarHeaderButtonPrefab, menu.GetSidePanelHeader(), "sb_btn_" + CS$<>8__locals1.text, true);
			ReMMSidebarHeaderButton.<>c__DisplayClass4_1 CS$<>8__locals2 = new ReMMSidebarHeaderButton.<>c__DisplayClass4_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMMSidebarHeaderButton = this;
			Object.Destroy(base.GameObject.GetComponent<LogoutButton>());
			this._textComponent = base.GameObject.transform.Find("Background_Field/Text_FieldContent").GetComponent<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|0());
			this._textComponent.richText = true;
			bool flag = icon == null;
			if (flag)
			{
				base.GameObject.transform.Find("Background_Field/Text_FieldContent").SetAsFirstSibling();
			}
			base.GameObject.transform.Find("Background_Field/Icon").GetComponent<Image>().overrideSprite = icon;
			base.GameObject.transform.Find("Background_Field/Icon").gameObject.SetActive(icon != null);
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
			ToolTip component = base.GameObject.GetComponent<ToolTip>();
			bool flag2 = component != null;
			if (flag2)
			{
				component._alternateLocalizableString = localizableString;
				component._localizableString = localizableString;
			}
			base.GameObject.GetComponent<Button>().onClick.AddListener(new Action(onClick.Invoke));
			Transform parent = menu.MenuTitleText.transform.parent;
			parent.GetComponent<LayoutElement>().minHeight += 95f;
			parent.Find("Separator").GetComponent<RectTransform>().anchoredPosition -= new Vector2(0f, 95f);
		}

		// Token: 0x040001BE RID: 446
		private TextMeshProUGUI _textComponent;
	}
}
