using System;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200003F RID: 63
	public class ReCategoryToggle : UiElement
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000D32C File Offset: 0x0000B52C
		// (set) Token: 0x06000284 RID: 644 RVA: 0x0000D349 File Offset: 0x0000B549
		public string Text
		{
			get
			{
				return this._text.text;
			}
			set
			{
				this._text.SetText(value, true);
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000285 RID: 645 RVA: 0x0000D35C File Offset: 0x0000B55C
		// (set) Token: 0x06000286 RID: 646 RVA: 0x0000D37C File Offset: 0x0000B57C
		public bool Interactable
		{
			get
			{
				return this._toggleComponent.interactable;
			}
			set
			{
				this._toggleComponent.interactable = value;
				bool flag = this._toggleStyleElement != null;
				if (flag)
				{
					this._toggleStyleElement.OnEnable();
				}
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x0000D3B8 File Offset: 0x0000B5B8
		public ReCategoryToggle(string title, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false, string color = "#ffffff") : base(QMMenuPrefabs.MenuCategoryTogglePrefab, parent, "Toggle_" + title, true)
		{
			this._text = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
			this._text.text = string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				title,
				"</color>"
			});
			this._text.richText = true;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000D444 File Offset: 0x0000B644
		public static ReCategoryToggle Create(string title, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false, string color = "#ffffff")
		{
			return new ReCategoryToggle(title, tooltip, onToggle, parent, defaultValue, color);
		}

		// Token: 0x0400010B RID: 267
		private readonly TextMeshProUGUIEx _text;

		// Token: 0x0400010C RID: 268
		private readonly Toggle _toggleComponent = null;

		// Token: 0x0400010D RID: 269
		private StyleElement _toggleStyleElement = null;
	}
}
