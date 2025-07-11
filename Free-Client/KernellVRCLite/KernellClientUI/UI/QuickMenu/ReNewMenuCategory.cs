using System;
using UnityEngine;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200004F RID: 79
	public class ReNewMenuCategory
	{
		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600038C RID: 908 RVA: 0x00012B64 File Offset: 0x00010D64
		// (set) Token: 0x0600038D RID: 909 RVA: 0x00012B81 File Offset: 0x00010D81
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

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00012B94 File Offset: 0x00010D94
		// (set) Token: 0x0600038F RID: 911 RVA: 0x00012BB1 File Offset: 0x00010DB1
		public bool Active
		{
			get
			{
				return this._newContainer.Active;
			}
			set
			{
				this.Header.Active = value;
				this._newContainer.Active = value;
			}
		}

		// Token: 0x06000390 RID: 912 RVA: 0x00012BD0 File Offset: 0x00010DD0
		public ReNewMenuCategory(string title, Transform parent = null, bool collapsible = true, string color = "#ffffff")
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
					bool flag = this._newContainer != null;
					if (flag)
					{
						this._newContainer.GameObject.SetActive(b);
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
			this._newContainer = new ReNewMenuContainer(string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				title,
				"</color>"
			}), parent);
			new ReNewUIBackground(title, this._newContainer.RectTransform);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x00012CD0 File Offset: 0x00010ED0
		public ReNewMenuCategory(ReMenuHeader headerElement, ReNewMenuContainer container)
		{
			this.Header = headerElement;
			this._newContainer = container;
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00012CE8 File Offset: 0x00010EE8
		public ReCategoryToggle AddCategoryToggle(string title, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
		{
			return new ReCategoryToggle(title, tooltip, onToggle, this._newContainer.RectTransform, false, "#ffffff");
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00012D14 File Offset: 0x00010F14
		public ReToggleSlider AddSlider(string text, string tooltip, Action<float> onSlide, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new ReToggleSlider(text, tooltip, onSlide, this._newContainer.RectTransform, defaultValue, minValue, maxValue, color);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00012D44 File Offset: 0x00010F44
		public ReToggleSlider AddSlider(string text, string tooltip, ConfigValue<float> configValue, bool reset = false, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new ReToggleSlider(text, tooltip, new Action<float>(configValue.SetValue), this._newContainer.RectTransform, configValue, minValue, maxValue, color);
		}

		// Token: 0x04000172 RID: 370
		public readonly ReMenuHeader Header;

		// Token: 0x04000173 RID: 371
		private readonly ReNewMenuContainer _newContainer;
	}
}
