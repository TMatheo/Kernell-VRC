using System;
using KernellClientUI.VRChat;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x02000066 RID: 102
	public class ReMMUserButton
	{
		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00019BD4 File Offset: 0x00017DD4
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x00019C0C File Offset: 0x00017E0C
		public string Tooltip
		{
			get
			{
				return (this._tooltip != null) ? this._tooltip._localizableString.Key : "";
			}
			set
			{
				bool flag = !(this._tooltip == null);
				if (flag)
				{
					LocalizableString localizableString = LocalizableStringExtensions.Localize(value, null, null, null);
					this._tooltip._localizableString = localizableString;
				}
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x00019C48 File Offset: 0x00017E48
		public ReMMUserButton(string name, string tooltip, Action onClick, Sprite icon, Transform parent)
		{
			ReMMUserButton.<>c__DisplayClass4_0 CS$<>8__locals1 = new ReMMUserButton.<>c__DisplayClass4_0();
			CS$<>8__locals1.name = name;
			base..ctor();
			ReMMUserButton.<>c__DisplayClass4_1 CS$<>8__locals2 = new ReMMUserButton.<>c__DisplayClass4_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			GameObject gameObject = Object.Instantiate<Transform>(MMenuPrefabs.MMUserDetailButton.transform, parent).gameObject;
			gameObject.name = "MMUButton_" + CS$<>8__locals2.CS$<>8__locals1.name;
			CS$<>8__locals2.txt = gameObject.transform.Find("Text_ButtonName").GetComponent<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|0());
			CS$<>8__locals2.txt.richText = true;
			CS$<>8__locals2.txt.text = CS$<>8__locals2.CS$<>8__locals1.name;
			Il2CppArrayBase<ToolTip> components = gameObject.GetComponents<ToolTip>();
			bool flag = components.Length > 0;
			if (flag)
			{
				this._tooltip = components[0];
				for (int i = 1; i < components.Length; i++)
				{
					Object.DestroyImmediate(components[i]);
				}
			}
			bool flag2 = this._tooltip != null;
			if (flag2)
			{
				LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
				this._tooltip._localizableString = localizableString;
				this._tooltip._alternateLocalizableString = localizableString;
			}
			gameObject.transform.Find("Text_ButtonName/Icon").GetComponent<Image>().overrideSprite = icon;
			Button component = gameObject.GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(new Action(onClick.Invoke));
		}

		// Token: 0x040001CE RID: 462
		private ToolTip _tooltip;
	}
}
