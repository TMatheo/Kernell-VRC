using System;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200004A RID: 74
	public class ReMenuSlider : UiElement
	{
		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x06000353 RID: 851 RVA: 0x00011B86 File Offset: 0x0000FD86
		// (set) Token: 0x06000354 RID: 852 RVA: 0x00011BB0 File Offset: 0x0000FDB0
		public string Tooltip
		{
			get
			{
				return (this._tooltip != null) ? this._tooltip._localizableString.Key : "";
			}
			set
			{
				bool flag = this._tooltip != null;
				if (flag)
				{
					LocalizableString localizableString = LocalizableStringExtensions.Localize(value, null, null, null);
					this._tooltip._localizableString = localizableString;
					this._tooltip._alternateLocalizableString = localizableString;
				}
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000355 RID: 853 RVA: 0x00011BF4 File Offset: 0x0000FDF4
		// (set) Token: 0x06000356 RID: 854 RVA: 0x00011C04 File Offset: 0x0000FE04
		public bool Interactable
		{
			get
			{
				return this._sliderComponent.interactable;
			}
			set
			{
				bool flag = this._sliderComponent.interactable == value;
				if (!flag)
				{
					this._sliderComponent.interactable = value;
					StyleElement styleElement = this._styleElement;
					if (styleElement != null)
					{
						styleElement.Method_Private_Void_Boolean_Boolean_0(value, false);
					}
				}
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00011C47 File Offset: 0x0000FE47
		// (set) Token: 0x06000358 RID: 856 RVA: 0x00011C54 File Offset: 0x0000FE54
		public float Value
		{
			get
			{
				return this._sliderComponent.value;
			}
			set
			{
				this.Slide(value, true);
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000359 RID: 857 RVA: 0x00011C5F File Offset: 0x0000FE5F
		// (set) Token: 0x0600035A RID: 858 RVA: 0x00011C6C File Offset: 0x0000FE6C
		public float MinValue
		{
			get
			{
				return this._sliderComponent.minValue;
			}
			set
			{
				this._sliderComponent.minValue = value;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x0600035B RID: 859 RVA: 0x00011C7B File Offset: 0x0000FE7B
		// (set) Token: 0x0600035C RID: 860 RVA: 0x00011C88 File Offset: 0x0000FE88
		public float MaxValue
		{
			get
			{
				return this._sliderComponent.maxValue;
			}
			set
			{
				this._sliderComponent.maxValue = value;
			}
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00011C98 File Offset: 0x0000FE98
		public ReMenuSlider(string text, string tooltip, Action<float> onSlide, Transform parent, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff") : base(QMMenuPrefabs.SliderPrefab, parent, "Slider_" + text, true)
		{
			this._colorHex = color;
			this.SetupLabelText(text);
			this._valueText = this.SetupValueText(defaultValue);
			this.SetupSlider(defaultValue, minValue, maxValue, onSlide);
			this.CleanupUnusedComponents();
			this.SetupTooltip(tooltip);
			this.Slide(defaultValue, false);
			EnableDisableListener.RegisterSafe();
		}

		// Token: 0x0600035E RID: 862 RVA: 0x00011D0C File Offset: 0x0000FF0C
		private void SetupLabelText(string text)
		{
			TextMeshProUGUIEx componentInChildren = base.RectTransform.GetChild(1).GetComponentInChildren<TextMeshProUGUIEx>();
			componentInChildren.text = string.Concat(new string[]
			{
				"<color=",
				this._colorHex,
				">",
				text,
				"</color>"
			});
			componentInChildren.richText = true;
			componentInChildren.enableAutoSizing = true;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00011D74 File Offset: 0x0000FF74
		private TextMeshProUGUIEx SetupValueText(float defaultValue)
		{
			TextMeshProUGUIEx componentInChildren = base.RectTransform.GetChild(0).GetComponentInChildren<TextMeshProUGUIEx>();
			componentInChildren.richText = true;
			componentInChildren.text = this.GetFormattedValueText(defaultValue);
			return componentInChildren;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x00011DB0 File Offset: 0x0000FFB0
		private void SetupSlider(float defaultValue, float minValue, float maxValue, Action<float> onSlide)
		{
			this._sliderComponent = base.GameObject.GetComponentInChildren<Slider>();
			this._styleElement = this._sliderComponent.GetComponent<StyleElement>();
			this._sliderComponent.minValue = minValue;
			this._sliderComponent.maxValue = maxValue;
			this._sliderComponent.value = defaultValue;
			this._sliderComponent.onValueChanged = new Slider.SliderEvent();
			bool flag = onSlide != null;
			if (flag)
			{
				this._sliderComponent.onValueChanged.AddListener(new Action<float>(onSlide.Invoke));
			}
			this._sliderComponent.onValueChanged.AddListener(new Action<float>(this.UpdateValueText));
			this._sliderComponent.m_OnValueChanged = this._sliderComponent.onValueChanged;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00011E80 File Offset: 0x00010080
		private void UpdateValueText(float value)
		{
			bool flag = this._valueText != null;
			if (flag)
			{
				this._valueText.text = this.GetFormattedValueText(value);
			}
		}

		// Token: 0x06000362 RID: 866 RVA: 0x00011EB4 File Offset: 0x000100B4
		private string GetFormattedValueText(float value)
		{
			return string.Format("<color={0}>{1:F}</color>", this._colorHex, value);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00011EDC File Offset: 0x000100DC
		private void CleanupUnusedComponents()
		{
			Transform transform = base.GameObject.transform.Find("RightItemContainer/Cell_MM_ToggleButton");
			bool flag = transform != null;
			if (flag)
			{
				Object.DestroyImmediate(transform.gameObject);
			}
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00011F1C File Offset: 0x0001011C
		private void SetupTooltip(string tooltipText)
		{
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltipText, null, null, null);
			bool flag = this._valueText != null;
			if (flag)
			{
				this._valueText.text = localizableString._fallbackText;
			}
			this._tooltip = this._sliderComponent.GetComponent<ToolTip>();
			bool flag2 = this._tooltip != null;
			if (flag2)
			{
				this._tooltip._localizableString = localizableString;
				this._tooltip._alternateLocalizableString = localizableString;
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00011F95 File Offset: 0x00010195
		public void Slide(float value, bool callback = true)
		{
			this._sliderComponent.Set(value, callback);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00011FA6 File Offset: 0x000101A6
		public void SetNewMaxValue(float value)
		{
			this.MaxValue = value;
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00011FB1 File Offset: 0x000101B1
		public void SetNewMinValue(float value)
		{
			this.MinValue = value;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00011FBC File Offset: 0x000101BC
		public float CurrentValue()
		{
			return this.Value;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00011FD4 File Offset: 0x000101D4
		public ReMenuSlider WithValue(float value)
		{
			this.Value = value;
			return this;
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00011FF0 File Offset: 0x000101F0
		public ReMenuSlider WithMinValue(float value)
		{
			this.MinValue = value;
			return this;
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001200C File Offset: 0x0001020C
		public ReMenuSlider WithMaxValue(float value)
		{
			this.MaxValue = value;
			return this;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00012028 File Offset: 0x00010228
		public ReMenuSlider WithInteractable(bool interactable)
		{
			this.Interactable = interactable;
			return this;
		}

		// Token: 0x04000158 RID: 344
		private Slider _sliderComponent;

		// Token: 0x04000159 RID: 345
		private ToolTip _tooltip;

		// Token: 0x0400015A RID: 346
		private StyleElement _styleElement;

		// Token: 0x0400015B RID: 347
		private readonly TextMeshProUGUIEx _valueText;

		// Token: 0x0400015C RID: 348
		private readonly string _colorHex;
	}
}
