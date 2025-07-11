using System;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000059 RID: 89
	public class ReToggleSlider : UiElement
	{
		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003E2 RID: 994 RVA: 0x000165B8 File Offset: 0x000147B8
		// (set) Token: 0x060003E3 RID: 995 RVA: 0x000165D5 File Offset: 0x000147D5
		public bool Interactable
		{
			get
			{
				return this._sliderComponent.interactable;
			}
			set
			{
				this._sliderComponent.interactable = value;
				this._styleElement.Method_Private_Void_Boolean_Boolean_0(value, false);
			}
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000165F4 File Offset: 0x000147F4
		public ReToggleSlider(string title, string tooltip, Action<float> onSlide, Transform parent, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			ReToggleSlider.<>c__DisplayClass5_0 CS$<>8__locals1 = new ReToggleSlider.<>c__DisplayClass5_0();
			CS$<>8__locals1.color = color;
			base..ctor(QMMenuPrefabs.SliderTogglePrefab, parent, "SliderToggle_" + title, true);
			Transform child = base.RectTransform.GetChild(1);
			TextMeshProUGUI componentInChildren = child.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren.text = string.Concat(new string[]
			{
				"<color=",
				CS$<>8__locals1.color,
				">",
				title,
				"</color>"
			});
			componentInChildren.richText = true;
			TextMeshProUGUI value = base.RectTransform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
			value.richText = true;
			value.text = string.Concat(new string[]
			{
				"<color=",
				CS$<>8__locals1.color,
				">",
				defaultValue.ToString("F"),
				"</color>"
			});
			this._sliderComponent = base.GameObject.GetComponentInChildren<Slider>();
			this._sliderComponent.minValue = minValue;
			this._sliderComponent.maxValue = maxValue;
			this._sliderComponent.value = defaultValue;
			this._sliderComponent.onValueChanged = new Slider.SliderEvent();
			this._sliderComponent.onValueChanged.AddListener(new Action<float>(onSlide.Invoke));
			this._sliderComponent.onValueChanged.AddListener(delegate(float val)
			{
				value.text = string.Concat(new string[]
				{
					"<color=",
					CS$<>8__locals1.color,
					">",
					val.ToString("F"),
					"</color>"
				});
			});
			this._sliderComponent.m_OnValueChanged = this._sliderComponent.onValueChanged;
			this.Slide(defaultValue, false);
			EnableDisableListener.RegisterSafe();
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x000167BD File Offset: 0x000149BD
		public void Slide(float value, bool callback = true)
		{
			this._sliderComponent.Set(value, callback);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x000167CE File Offset: 0x000149CE
		public void SetNewMaxValue(float value)
		{
			this._sliderComponent.maxValue = value;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x000167DE File Offset: 0x000149DE
		public void SetNewMinValue(float value)
		{
			this._sliderComponent.minValue = value;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x000167F0 File Offset: 0x000149F0
		public float CurrentValue()
		{
			return this._sliderComponent.value;
		}

		// Token: 0x04000197 RID: 407
		private readonly Slider _sliderComponent;

		// Token: 0x04000198 RID: 408
		private readonly StyleElement _styleElement = null;
	}
}
