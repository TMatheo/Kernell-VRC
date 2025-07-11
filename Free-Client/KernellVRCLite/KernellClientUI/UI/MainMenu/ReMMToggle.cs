using System;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.Ui;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x02000065 RID: 101
	public class ReMMToggle
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00019738 File Offset: 0x00017938
		// (set) Token: 0x0600045D RID: 1117 RVA: 0x00019750 File Offset: 0x00017950
		public bool Value
		{
			get
			{
				return this._valueHolder;
			}
			set
			{
				this.Toggle(value, true);
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0001975C File Offset: 0x0001795C
		// (set) Token: 0x0600045F RID: 1119 RVA: 0x00019779 File Offset: 0x00017979
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

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x0001978C File Offset: 0x0001798C
		// (set) Token: 0x06000461 RID: 1121 RVA: 0x000197C4 File Offset: 0x000179C4
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00019800 File Offset: 0x00017A00
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x0001981D File Offset: 0x00017A1D
		public bool Interactable
		{
			get
			{
				return this._toggleComponent.interactable;
			}
			set
			{
				this._toggleComponent.interactable = value;
				this._toggleStyleElement.OnEnable();
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x00019839 File Offset: 0x00017A39
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00019841 File Offset: 0x00017A41
		public GameObject toggleObj { get; protected set; }

		// Token: 0x06000466 RID: 1126 RVA: 0x0001984C File Offset: 0x00017A4C
		public ReMMToggle(string title, string tooltip, Action<bool> onToggle, bool defaultState = false, Transform parent = null, bool separator = true, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			ReMMToggle.<>c__DisplayClass25_0 CS$<>8__locals1 = new ReMMToggle.<>c__DisplayClass25_0();
			CS$<>8__locals1.defaultState = defaultState;
			base..ctor();
			this.toggleObj = Object.Instantiate<GameObject>(MMenuPrefabs.MMTogglePrefab, parent);
			this._toggleComponent = this.toggleObj.GetComponent<Toggle>();
			this._defaultState = CS$<>8__locals1.defaultState;
			this._textComponent = this.toggleObj.transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUIEx>();
			this._textComponent.richText = true;
			this.Text = string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				title,
				"</color>"
			});
			this._toggleOnImage = this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/On_Container").GetComponent<ImageEx>();
			((Image)this._toggleOnImage).overrideSprite = (iconOn ? iconOn : ((Image)this._toggleOnImage).sprite);
			this._toggleOffImage = this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/Off_Container").GetComponent<ImageEx>();
			((Image)this._toggleOffImage).overrideSprite = (iconOff ? iconOff : ((Image)this._toggleOffImage).sprite);
			this._toggleStyleElement = this.toggleObj.GetComponent<StyleElement>();
			this._toggleComponent.onValueChanged = new Toggle.ToggleEvent();
			RectTransform handle = this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/Handle").GetComponent<RectTransform>();
			this.handleWidth = handle.rect.width;
			this._toggleComponent.onValueChanged.AddListener(delegate(bool b)
			{
				this._valueHolder = b;
				this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/On_Container").gameObject.SetActive(b);
				this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/Off_Container").gameObject.SetActive(!b);
				this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/On_Container/On_Text").gameObject.SetActive(b);
				this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/Off_Container/Off_Text").gameObject.SetActive(!b);
				handle.localPosition += new Vector3(b ? (this.handleWidth * 2f) : ((0f - this.handleWidth) * 2f), 0f, 0f);
			});
			this._toggleComponent.onValueChanged.AddListener(new Action<bool>(onToggle.Invoke));
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
			this._tooltip = this.toggleObj.GetComponent<UiToggleTooltip>();
			this._tooltip._alternateLocalizableString = localizableString;
			this._tooltip._localizableString = localizableString;
			if (separator)
			{
				Object.Instantiate<GameObject>(MMenuPrefabs.MMSeparatorprefab, parent);
			}
			ReModPatches.isOpenForFirstTime += delegate()
			{
				this.Toggle(CS$<>8__locals1.defaultState, false);
			};
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00019AB4 File Offset: 0x00017CB4
		public void Toggle(bool value, bool callback = true)
		{
			this._valueHolder = value;
			this._toggleComponent.Set(value, callback);
			this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/On_Container").gameObject.SetActive(value);
			this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/Off_Container").gameObject.SetActive(!value);
			this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/On_Container/On_Text").gameObject.SetActive(value);
			this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/Off_Container/Off_Text").gameObject.SetActive(!value);
			RectTransform component = this.toggleObj.transform.Find("RightItemContainer/Cell_MM_OnOffSwitch/Handle").GetComponent<RectTransform>();
			bool flag = !callback && (this._defaultState || value);
			if (flag)
			{
				component.localPosition += new Vector3(value ? (this.handleWidth * 2f) : ((0f - this.handleWidth) * 2f), 0f, 0f);
			}
		}

		// Token: 0x040001C4 RID: 452
		private readonly Toggle _toggleComponent;

		// Token: 0x040001C5 RID: 453
		private bool _valueHolder;

		// Token: 0x040001C6 RID: 454
		private StyleElement _toggleStyleElement;

		// Token: 0x040001C7 RID: 455
		private TextMeshProUGUIEx _textComponent;

		// Token: 0x040001C8 RID: 456
		private UiToggleTooltip _tooltip;

		// Token: 0x040001C9 RID: 457
		private readonly ImageEx _toggleOnImage;

		// Token: 0x040001CA RID: 458
		private readonly ImageEx _toggleOffImage;

		// Token: 0x040001CB RID: 459
		private float handleWidth;

		// Token: 0x040001CC RID: 460
		private bool _defaultState;
	}
}
