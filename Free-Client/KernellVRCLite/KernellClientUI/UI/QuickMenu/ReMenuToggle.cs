using System;
using KernellClientUI.VRChat;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200004D RID: 77
	public class ReMenuToggle : UiElement
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000378 RID: 888 RVA: 0x0001233B File Offset: 0x0001053B
		// (set) Token: 0x06000379 RID: 889 RVA: 0x00012344 File Offset: 0x00010544
		public bool ThinMode
		{
			get
			{
				return this._thinMode;
			}
			set
			{
				bool flag = this._thinMode != value;
				if (flag)
				{
					this._thinMode = value;
					this.UpdateThinMode();
					this.UpdateVisuals();
				}
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00012379 File Offset: 0x00010579
		// (set) Token: 0x0600037B RID: 891 RVA: 0x00012381 File Offset: 0x00010581
		public bool Value
		{
			get
			{
				return this._value;
			}
			set
			{
				this.SetValue(value, true);
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600037C RID: 892 RVA: 0x0001238C File Offset: 0x0001058C
		// (set) Token: 0x0600037D RID: 893 RVA: 0x00012394 File Offset: 0x00010594
		public string Text
		{
			get
			{
				return this._baseText;
			}
			set
			{
				this._baseText = value;
				this.UpdateToggleText();
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600037E RID: 894 RVA: 0x000123A5 File Offset: 0x000105A5
		// (set) Token: 0x0600037F RID: 895 RVA: 0x000123BC File Offset: 0x000105BC
		public bool Interactable
		{
			get
			{
				Button button = this._button;
				return button != null && button.interactable;
			}
			set
			{
				bool flag = this._button != null;
				if (flag)
				{
					this._button.interactable = value;
					StyleElement styleElement = this._styleElement;
					if (styleElement != null)
					{
						styleElement.SetInteractableStyle(value);
					}
				}
			}
		}

		// Token: 0x06000380 RID: 896 RVA: 0x000123FC File Offset: 0x000105FC
		public ReMenuToggle(string text, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false, Sprite iconOn = null, Sprite iconOff = null, string color = "#FFFFFF", bool thinMode = false, string onText = "ENABLED", string offText = "DISABLED") : base(QMMenuPrefabs.TogglePrefab, parent, "Toggle_" + text, true)
		{
			bool flag = base.GameObject == null;
			if (flag)
			{
				MelonLogger.Error("ReMenuToggle: Failed to instantiate TogglePrefab for '" + text + "'.");
			}
			else
			{
				if (onToggle == null)
				{
					throw new ArgumentNullException("onToggle");
				}
				this._onToggle = onToggle;
				this._value = defaultValue;
				this._thinMode = thinMode;
				this._baseText = text;
				this._onText = onText;
				this._offText = offText;
				Color textColor;
				bool flag2 = ColorUtility.TryParseHtmlString(color, ref textColor);
				if (flag2)
				{
					this._textColor = textColor;
				}
				this._button = base.GameObject.GetComponent<Button>();
				this._text = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
				this._styleElement = base.GameObject.GetComponent<StyleElement>();
				this._tooltip = base.GameObject.GetComponent<UiToggleTooltip>();
				RectTransform rectTransform = base.RectTransform;
				GameObject iconsContainer;
				if (rectTransform == null)
				{
					iconsContainer = null;
				}
				else
				{
					Transform transform = rectTransform.Find("Icons");
					iconsContainer = ((transform != null) ? transform.gameObject : null);
				}
				this._iconsContainer = iconsContainer;
				bool flag3 = this._iconsContainer != null;
				if (flag3)
				{
					this._iconsContainer.SetActive(false);
				}
				this._iconOn = this.GetIconImage("Icons/Icon_On");
				this._iconOff = this.GetIconImage("Icons/Icon_Off");
				this.AssignButtonListener();
				this.SetupTextAndTooltip(text, tooltip, color);
				this.RemoveInvisibleGraphic();
				this.UpdateThinMode();
				this._initialized = true;
				this.SetValue(this._value, false);
				this.UpdateVisuals();
				this.Interactable = true;
				MelonLogger.Msg(string.Format("ReMenuToggle '{0}' created with initial value: {1}", text, this._value));
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00012618 File Offset: 0x00010818
		private void UpdateToggleText()
		{
			bool flag = this._text == null || !this._initialized;
			if (!flag)
			{
				bool thinMode = this._thinMode;
				string text;
				if (thinMode)
				{
					string str = this._value ? this._onText : this._offText;
					text = this._baseText + ": " + str;
				}
				else
				{
					text = this._baseText;
				}
				Color color = this._value ? this._enabledColor : this._disabledColor;
				this._text.text = string.Concat(new string[]
				{
					"<color=#",
					ColorUtility.ToHtmlStringRGB(color),
					">",
					text,
					"</color>"
				});
			}
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000126E0 File Offset: 0x000108E0
		private void UpdateThinMode()
		{
			bool flag = !this._initialized;
			if (!flag)
			{
				bool flag2 = this._iconsContainer != null;
				if (flag2)
				{
					this._iconsContainer.SetActive(false);
				}
				bool flag3 = this._text != null;
				if (flag3)
				{
					this._text.transform.localPosition = new Vector3(0f, 0f, this._text.transform.localPosition.z);
					this._text.fontSize = 16f;
					this._text.enableAutoSizing = true;
					this._text.fontSizeMin = 12f;
					this._text.fontSizeMax = 16f;
					this._text.alignment = 514;
				}
				this.UpdateToggleText();
				bool flag4 = base.RectTransform != null;
				if (flag4)
				{
					LayoutElement layoutElement = base.RectTransform.GetComponent<LayoutElement>();
					bool flag5 = layoutElement == null;
					if (flag5)
					{
						layoutElement = base.RectTransform.gameObject.AddComponent<LayoutElement>();
					}
					layoutElement.minWidth = 220f;
					layoutElement.preferredWidth = 220f;
					layoutElement.minHeight = 50f;
					layoutElement.preferredHeight = 50f;
					base.RectTransform.sizeDelta = new Vector2(220f, 50f);
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.RectTransform);
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0001285C File Offset: 0x00010A5C
		private Image GetIconImage(string childPath)
		{
			RectTransform rectTransform = base.RectTransform;
			Transform transform = (rectTransform != null) ? rectTransform.Find(childPath) : null;
			bool flag = transform == null;
			Image result;
			if (flag)
			{
				result = null;
			}
			else
			{
				result = transform.GetComponent<Image>();
			}
			return result;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x00012898 File Offset: 0x00010A98
		private void AssignButtonListener()
		{
			bool flag = this._button != null;
			if (flag)
			{
				this._button.onClick.RemoveAllListeners();
				this._button.onClick.AddListener(DelegateSupport.ConvertDelegate<UnityAction>(new Action(this.OnButtonClick)));
			}
			else
			{
				MelonLogger.Error("ReMenuToggle: Button component not found.");
			}
		}

		// Token: 0x06000385 RID: 901 RVA: 0x000128FC File Offset: 0x00010AFC
		private void SetupTextAndTooltip(string text, string tooltip, string color)
		{
			bool flag = this._text != null;
			if (flag)
			{
				this._text.richText = true;
				Color textColor;
				bool flag2 = ColorUtility.TryParseHtmlString(color, ref textColor);
				if (flag2)
				{
					this._textColor = textColor;
				}
			}
			else
			{
				MelonLogger.Warning("ReMenuToggle: Text component not found.");
			}
			bool flag3 = this._tooltip != null && !string.IsNullOrEmpty(tooltip);
			if (flag3)
			{
				LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
				this._tooltip._localizableString = localizableString;
				this._tooltip._alternateLocalizableString = localizableString;
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00012994 File Offset: 0x00010B94
		public void Toggle(bool? value = null, bool triggerCallback = true)
		{
			bool flag = value != null;
			if (flag)
			{
				this._value = value.Value;
			}
			else
			{
				this._value = !this._value;
			}
			this.UpdateVisuals();
			if (triggerCallback)
			{
				Action<bool> onToggle = this._onToggle;
				if (onToggle != null)
				{
					onToggle(this._value);
				}
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x000129F0 File Offset: 0x00010BF0
		private void RemoveInvisibleGraphic()
		{
			UIInvisibleGraphic component = base.GameObject.GetComponent<UIInvisibleGraphic>();
			bool flag = component != null;
			if (flag)
			{
				Object.DestroyImmediate(component);
			}
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00012A1C File Offset: 0x00010C1C
		private void OnButtonClick()
		{
			this.SetValue(!this._value, true);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00012A30 File Offset: 0x00010C30
		private void UpdateVisuals()
		{
			bool flag = !this._initialized;
			if (!flag)
			{
				this.UpdateToggleText();
				Button button = this._button;
				Image image = (button != null) ? button.GetComponent<Image>() : null;
				bool flag2 = image != null;
				if (flag2)
				{
					Color color = this._value ? new Color(0.1f, 0.4f, 0.1f, 0.8f) : new Color(0.4f, 0.1f, 0.1f, 0.8f);
					image.color = color;
				}
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00012ABC File Offset: 0x00010CBC
		public void SetValue(bool value, bool triggerCallback = true)
		{
			bool flag = this._value != value;
			this._value = value;
			this.UpdateVisuals();
			bool flag2 = flag && triggerCallback;
			if (flag2)
			{
				Action<bool> onToggle = this._onToggle;
				if (onToggle != null)
				{
					onToggle(this._value);
				}
			}
		}

		// Token: 0x0400015F RID: 351
		private readonly Button _button;

		// Token: 0x04000160 RID: 352
		private readonly TextMeshProUGUIEx _text;

		// Token: 0x04000161 RID: 353
		private readonly StyleElement _styleElement;

		// Token: 0x04000162 RID: 354
		private readonly UiToggleTooltip _tooltip;

		// Token: 0x04000163 RID: 355
		private readonly Image _iconOn;

		// Token: 0x04000164 RID: 356
		private readonly Image _iconOff;

		// Token: 0x04000165 RID: 357
		private readonly GameObject _iconsContainer;

		// Token: 0x04000166 RID: 358
		private bool _value;

		// Token: 0x04000167 RID: 359
		private bool _thinMode;

		// Token: 0x04000168 RID: 360
		private bool _initialized = false;

		// Token: 0x04000169 RID: 361
		private readonly Action<bool> _onToggle;

		// Token: 0x0400016A RID: 362
		private readonly float _onBrightness = 1f;

		// Token: 0x0400016B RID: 363
		private readonly float _offBrightness = 0.3f;

		// Token: 0x0400016C RID: 364
		private string _onText;

		// Token: 0x0400016D RID: 365
		private string _offText;

		// Token: 0x0400016E RID: 366
		private string _baseText;

		// Token: 0x0400016F RID: 367
		private Color _textColor = Color.white;

		// Token: 0x04000170 RID: 368
		private Color _enabledColor = new Color(0.2f, 0.8f, 0.2f, 1f);

		// Token: 0x04000171 RID: 369
		private Color _disabledColor = new Color(0.8f, 0.2f, 0.2f, 1f);
	}
}
