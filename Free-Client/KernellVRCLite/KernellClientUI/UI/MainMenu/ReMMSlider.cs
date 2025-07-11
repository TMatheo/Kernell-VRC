using System;
using System.Collections;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x02000062 RID: 98
	public class ReMMSlider : ReMMSectionElement
	{
		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x0001914C File Offset: 0x0001734C
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x00019169 File Offset: 0x00017369
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00019179 File Offset: 0x00017379
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x00019181 File Offset: 0x00017381
		public ReMMCategorySection Section { get; private set; }

		// Token: 0x06000451 RID: 1105 RVA: 0x0001918C File Offset: 0x0001738C
		public ReMMSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, bool separator = true, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff", ReMMCategorySection section = null)
		{
			ReMMSlider.<>c__DisplayClass10_0 CS$<>8__locals1 = new ReMMSlider.<>c__DisplayClass10_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.title = title;
			base..ctor(MMenuPrefabs.MMSliderPrefab, parent, false, separator);
			ReMMSlider.<>c__DisplayClass10_1 CS$<>8__locals2 = new ReMMSlider.<>c__DisplayClass10_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMMSlider = this;
			this._textComponent = base.gameObject.transform.Find("LeftItemContainer/Title").GetComponent<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|2());
			this._textComponent.richText = true;
			this._color = CS$<>8__locals2.CS$<>8__locals1.color;
			bool flag = section != null;
			if (flag)
			{
				this.Section = section;
			}
			this._sliderComponent = base.gameObject.transform.Find("RightItemContainer/Slider").GetComponent<SnapSliderExtendedCallbacks>();
			CS$<>8__locals2.value = base.gameObject.transform.Find("RightItemContainer/Text_MM_H3").GetComponent<TextMeshProUGUIEx>();
			CS$<>8__locals2.value.richText = true;
			CS$<>8__locals2.value.text = string.Concat(new string[]
			{
				"<color=",
				CS$<>8__locals2.CS$<>8__locals1.color,
				">",
				defaultValue.ToString("F"),
				"</color>"
			});
			this._sliderComponent.minValue = minValue;
			this._sliderComponent.maxValue = maxValue;
			this._sliderComponent.value = defaultValue;
			this._sliderComponent.onValueChanged = new Slider.SliderEvent();
			this._sliderComponent.onValueChanged.AddListener(new Action<float>(onSlide.Invoke));
			this._sliderComponent.onValueChanged.AddListener(delegate(float val)
			{
				CS$<>8__locals2.value.text = string.Concat(new string[]
				{
					"<color=",
					CS$<>8__locals2.CS$<>8__locals1.color,
					">",
					val.ToString("F"),
					"</color>"
				});
			});
			this._sliderComponent.m_OnValueChanged = this._sliderComponent.onValueChanged;
			ToolTip component = this._sliderComponent.GetComponent<ToolTip>();
			bool flag2 = component != null;
			if (flag2)
			{
				component._localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
				component._alternateLocalizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
			}
			if (separator)
			{
				Object.Instantiate<GameObject>(MMenuPrefabs.MMSeparatorprefab, parent);
			}
			this.Slide(defaultValue, false);
			EnableDisableListener.RegisterSafe();
			bool flag3 = this.Section != null;
			if (flag3)
			{
				this.Section.Category.ButtonObj.GetComponent<Button>().onClick.AddListener(delegate()
				{
					MelonCoroutines.Start(CS$<>8__locals2.reMMSlider.fix1());
				});
			}
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x00019406 File Offset: 0x00017606
		private IEnumerator fix1()
		{
			yield return new WaitForSeconds(0.025f);
			TextMeshProUGUIEx value = base.gameObject.transform.Find("RightItemContainer/Text_MM_H3").GetComponent<TextMeshProUGUIEx>();
			value.text = string.Concat(new string[]
			{
				"<color=",
				this._color,
				">",
				this.CurrentValue().ToString("F"),
				"</color>"
			});
			yield break;
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x00019415 File Offset: 0x00017615
		public void Slide(float value, bool callback = true)
		{
			this._sliderComponent.Set(value, callback);
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x00019426 File Offset: 0x00017626
		public void SetNewMaxValue(float value)
		{
			this._sliderComponent.maxValue = value;
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x00019436 File Offset: 0x00017636
		public void SetNewMinValue(float value)
		{
			this._sliderComponent.minValue = value;
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x00019448 File Offset: 0x00017648
		public float CurrentValue()
		{
			return this._sliderComponent.value;
		}

		// Token: 0x040001BF RID: 447
		private TextMeshProUGUIEx _textComponent;

		// Token: 0x040001C0 RID: 448
		private SnapSliderExtendedCallbacks _sliderComponent;

		// Token: 0x040001C1 RID: 449
		private string _color;
	}
}
