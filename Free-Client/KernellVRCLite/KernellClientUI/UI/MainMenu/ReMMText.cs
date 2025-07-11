using System;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x02000064 RID: 100
	public class ReMMText : ReMMSectionElement
	{
		// Token: 0x170000DD RID: 221
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x000195B0 File Offset: 0x000177B0
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x000195CD File Offset: 0x000177CD
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

		// Token: 0x0600045B RID: 1115 RVA: 0x000195E0 File Offset: 0x000177E0
		public ReMMText(string title, string menutext, string tooltip, Transform parent = null, int fontSize = 30, bool separator = true, string color = "#ffffff")
		{
			ReMMText.<>c__DisplayClass4_0 CS$<>8__locals1 = new ReMMText.<>c__DisplayClass4_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.title = title;
			base..ctor(MMenuPrefabs.MMLabelPrefab, parent, true, separator);
			ReMMText.<>c__DisplayClass4_1 CS$<>8__locals2 = new ReMMText.<>c__DisplayClass4_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMMText = this;
			Object.Destroy(base.gameObject.GetComponent<Toggle>());
			Object.Destroy(base.RightItemContainer.Find("Cell_MM_OnOffSwitch").gameObject);
			this._textComponent = base.LeftItemContainer.Find("Title").GetComponent<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|0());
			this._textComponent.fontSize = (float)fontSize;
			this._textComponent.richText = true;
			GameObject gameObject = Object.Instantiate<GameObject>(MMenuPrefabs.MMLabelTextPrefab, base.RightItemContainer);
			TextMeshProUGUIEx component = gameObject.GetComponent<TextMeshProUGUIEx>();
			component.richText = true;
			component.text = string.Concat(new string[]
			{
				"<color=",
				CS$<>8__locals2.CS$<>8__locals1.color,
				">",
				menutext,
				"</color>"
			});
			component.alignment = 516;
			component.fontSize = (float)fontSize;
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
			ToolTip component2 = base.gameObject.GetComponent<ToolTip>();
			component2._localizableString = localizableString;
			component2._alternateLocalizableString = localizableString;
		}

		// Token: 0x040001C3 RID: 451
		private TextMeshProUGUIEx _textComponent;
	}
}
