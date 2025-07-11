using System;
using UnityEngine;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200004B RID: 75
	public class ReMenuSliderCategory
	{
		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00012044 File Offset: 0x00010244
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00012061 File Offset: 0x00010261
		public string Title
		{
			get
			{
				return this.Header.Title;
			}
			set
			{
				this.Header.Title = value;
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00012074 File Offset: 0x00010274
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00012091 File Offset: 0x00010291
		public bool Active
		{
			get
			{
				return this._sliderContainer.Active;
			}
			set
			{
				this.Header.Active = value;
				this._sliderContainer.Active = value;
			}
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000120B0 File Offset: 0x000102B0
		public ReMenuSliderCategory(string title, Transform parent = null, bool collapsible = true, string color = "#ffffff")
		{
			if (collapsible)
			{
				ReMenuHeaderCollapsible reMenuHeaderCollapsible = new ReMenuHeaderCollapsible(string.Concat(new string[]
				{
					"<color=",
					color,
					">",
					title,
					"</color>"
				}), parent);
				reMenuHeaderCollapsible.OnToggle = (Action<bool>)Delegate.Combine(reMenuHeaderCollapsible.OnToggle, new Action<bool>(delegate(bool b)
				{
					bool flag = this._sliderContainer != null;
					if (flag)
					{
						this._sliderContainer.GameObject.SetActive(b);
					}
				}));
				this.Header = reMenuHeaderCollapsible;
			}
			else
			{
				ReMenuHeader header = new ReMenuHeader(string.Concat(new string[]
				{
					"<color=",
					color,
					">",
					title,
					"</color>"
				}), parent);
				this.Header = header;
			}
			this._sliderContainer = new ReMenuSliderContainer(string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				title,
				"</color>"
			}), parent);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0001219E File Offset: 0x0001039E
		public ReMenuSliderCategory(ReMenuHeader headerElement, ReMenuSliderContainer container)
		{
			this.Header = headerElement;
			this._sliderContainer = container;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000121B8 File Offset: 0x000103B8
		public ReMenuSlider AddSlider(string text, string tooltip, Action<float> onSlide, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new ReMenuSlider(text, tooltip, onSlide, this._sliderContainer.RectTransform, defaultValue, minValue, maxValue, color);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000121E8 File Offset: 0x000103E8
		public ReMenuSlider AddSlider(string text, string tooltip, ConfigValue<float> configValue, bool reset = false, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new ReMenuSlider(text, tooltip, new Action<float>(configValue.SetValue), this._sliderContainer.RectTransform, configValue, minValue, maxValue, color);
		}

		// Token: 0x0400015D RID: 349
		public readonly ReMenuHeader Header;

		// Token: 0x0400015E RID: 350
		private readonly ReMenuSliderContainer _sliderContainer;
	}
}
