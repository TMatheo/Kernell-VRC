using System;
using KernellClientUI.VRChat;
using TMPro;
using UnityEngine;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000044 RID: 68
	public class ReMenuCategory : IButtonPage
	{
		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x0000E8F8 File Offset: 0x0000CAF8
		// (set) Token: 0x060002C8 RID: 712 RVA: 0x0000E915 File Offset: 0x0000CB15
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

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000E928 File Offset: 0x0000CB28
		// (set) Token: 0x060002CA RID: 714 RVA: 0x0000E94A File Offset: 0x0000CB4A
		public bool Active
		{
			get
			{
				return this._buttonContainer.GameObject.activeInHierarchy;
			}
			set
			{
				this.Header.Active = value;
				this._buttonContainer.Active = value;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000E967 File Offset: 0x0000CB67
		public RectTransform RectTransform
		{
			get
			{
				return this._buttonContainer.RectTransform;
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000E974 File Offset: 0x0000CB74
		public ReMenuCategory(string title, Transform parent = null, bool collapsible = true, string color = "#ffffff", bool skipLayoutGroup = false)
		{
			Transform parent2 = parent;
			bool flag = skipLayoutGroup && parent != null;
			if (flag)
			{
				parent2 = parent.parent;
			}
			if (collapsible)
			{
				ReMenuHeaderCollapsible reMenuHeaderCollapsible = new ReMenuHeaderCollapsible(string.Concat(new string[]
				{
					"<color=",
					color,
					">",
					title,
					"</color>"
				}), parent2);
				reMenuHeaderCollapsible.OnToggle = (Action<bool>)Delegate.Combine(reMenuHeaderCollapsible.OnToggle, new Action<bool>(delegate(bool b)
				{
					this._buttonContainer.GameObject.SetActive(b);
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
				}), parent2);
				this.Header = header;
			}
			this._buttonContainer = new ReMenuButtonContainer(string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				title,
				"</color>"
			}), parent2);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000EA81 File Offset: 0x0000CC81
		public ReMenuCategory(ReMenuHeader headerElement, ReMenuButtonContainer container)
		{
			this.Header = headerElement;
			this._buttonContainer = container;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000EA9C File Offset: 0x0000CC9C
		public ReMenuButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, string color = "#ffffff")
		{
			return new ReMenuButton(text, tooltip, onClick, this._buttonContainer.RectTransform, sprite, true, color, false);
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000EACC File Offset: 0x0000CCCC
		public ReMenuButton AddSpacer(Sprite sprite = null)
		{
			ReMenuButton reMenuButton = this.AddButton(string.Empty, string.Empty, null, sprite, "#ffffff");
			reMenuButton.GameObject.name = "Button_Spacer";
			reMenuButton.Background.gameObject.SetActive(false);
			return reMenuButton;
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000EB1C File Offset: 0x0000CD1C
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, string color = "#ffffff")
		{
			return this.AddToggle(text, tooltip, onToggle, false, color);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x0000EB3C File Offset: 0x0000CD3C
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
		{
			return this.AddToggle(text, tooltip, onToggle, defaultValue, null, null, color);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000EB60 File Offset: 0x0000CD60
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle)
		{
			return this.AddToggle(text, tooltip, onToggle, false, null, null);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x0000EB80 File Offset: 0x0000CD80
		public ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff")
		{
			return this.AddToggle(text, tooltip, configValue, null, null, color);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000EBA0 File Offset: 0x0000CDA0
		public ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue)
		{
			return this.AddToggle(text, tooltip, configValue, null, null);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000EBC0 File Offset: 0x0000CDC0
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff")
		{
			return new ReMenuToggle(text, tooltip, onToggle, this._buttonContainer.RectTransform, defaultValue, iconOn, iconOff, color, false, "ENABLED", "DISABLED");
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x0000EBF8 File Offset: 0x0000CDF8
		public ReRadioTogglePage AddReRadioTogglePage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			return this.AddRadioTogglePage(text, tooltip, sprite, color);
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0000EC18 File Offset: 0x0000CE18
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue, Sprite iconOn, Sprite iconOff)
		{
			return new ReMenuToggle(text, tooltip, onToggle, this._buttonContainer.RectTransform, defaultValue, iconOn, iconOff, "#FFFFFF", false, "ENABLED", "DISABLED");
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000EC54 File Offset: 0x0000CE54
		public ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff")
		{
			return new ReMenuToggle(text, tooltip, new Action<bool>(configValue.SetValue), this._buttonContainer.RectTransform, configValue, iconOn, iconOff, color, false, "ENABLED", "DISABLED");
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000EC9C File Offset: 0x0000CE9C
		public ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn, Sprite iconOff)
		{
			return new ReMenuToggle(text, tooltip, new Action<bool>(configValue.SetValue), this._buttonContainer.RectTransform, configValue, iconOn, iconOff, "#FFFFFF", false, "ENABLED", "DISABLED");
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0000ECE8 File Offset: 0x0000CEE8
		public ReMenuDesc AddDescription(string text, string color = "#FFFFFF", float fontSize = 14f, TextAlignmentOptions alignment = 514)
		{
			return new ReMenuDesc(text, this._buttonContainer.RectTransform, color, fontSize, alignment);
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0000ED10 File Offset: 0x0000CF10
		public ReMenuDesc AddDetailedDescription(string text, string color = "#FFFFFF", float fontSize = 14f, TextAlignmentOptions alignment = 514, bool isBold = false, bool isItalic = false, float width = 0f)
		{
			ReMenuDesc reMenuDesc = new ReMenuDesc(text, this._buttonContainer.RectTransform, color, fontSize, alignment);
			reMenuDesc.SetBold(isBold);
			reMenuDesc.SetItalic(isItalic);
			return reMenuDesc;
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0000ED4C File Offset: 0x0000CF4C
		public ReMenuDesc AddHighlightedDescription(string text, string textColor = "#FFFFFF", string backgroundColor = "#444444", float fontSize = 14f)
		{
			ReMenuDesc reMenuDesc = new ReMenuDesc(text, this._buttonContainer.RectTransform, textColor, fontSize, 513);
			string text2 = string.Concat(new string[]
			{
				"<mark=",
				backgroundColor,
				">",
				text,
				"</mark>"
			});
			reMenuDesc.SetColoredText(text2, textColor);
			return reMenuDesc;
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000EDB0 File Offset: 0x0000CFB0
		public ReMenuPage AddMenuPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReMenuPage menuPage = this.GetMenuPage(text);
			bool flag = menuPage != null;
			ReMenuPage result;
			if (flag)
			{
				result = menuPage;
			}
			else
			{
				ReMenuPage reMenuPage = new ReMenuPage(text, false, color);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip, new Action(reMenuPage.Open), sprite, color);
				result = reMenuPage;
			}
			return result;
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000EE14 File Offset: 0x0000D014
		public ReTabbedPage AddTabbedPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReTabbedPage tabbedPage = this.GetTabbedPage(text);
			bool flag = tabbedPage != null;
			ReTabbedPage result;
			if (flag)
			{
				result = tabbedPage;
			}
			else
			{
				ReTabbedPage reTabbedPage = new ReTabbedPage(text, false, color);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip, new Action(reTabbedPage.Open), sprite, color);
				result = reTabbedPage;
			}
			return result;
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0000EE78 File Offset: 0x0000D078
		public ReMenuPage ToMenuPage(string name, string tooltip = "", Sprite sprite = null)
		{
			ReMenuPage menuPage = this.GetMenuPage(name);
			this.AddButton(name, string.IsNullOrEmpty(tooltip) ? ("Open the " + name + " menu") : tooltip, new Action(menuPage.Open), sprite, "#ffffff");
			return menuPage;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000EEC8 File Offset: 0x0000D0C8
		public ReRadioTogglePage AddRadioTogglePage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReRadioTogglePage radioTogglePage = this.GetRadioTogglePage(text);
			bool flag = radioTogglePage != null;
			ReRadioTogglePage result;
			if (flag)
			{
				result = radioTogglePage;
			}
			else
			{
				ReRadioTogglePage reRadioTogglePage = new ReRadioTogglePage(text);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " radio selection menu") : tooltip, delegate
				{
					reRadioTogglePage.Open(null);
				}, sprite, color);
				result = reRadioTogglePage;
			}
			return result;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000EF38 File Offset: 0x0000D138
		public ReRadioTogglePage GetRadioTogglePage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReRadioTogglePage(name);
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000EF77 File Offset: 0x0000D177
		public void AddRadioTogglePage(string text, string tooltip, Action<ReRadioTogglePage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddRadioTogglePage(text, tooltip, sprite, color));
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000EF90 File Offset: 0x0000D190
		public ReRadioToggleGroup AddRadioToggleGroup(string groupName, Action<object> onSelectionChanged, string color = "#ffffff")
		{
			return new ReRadioToggleGroup(groupName, this._buttonContainer.RectTransform, onSelectionChanged, color);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public ReCategoryPage ToCategoryPage(string name, string tooltip = "", Sprite sprite = null)
		{
			ReCategoryPage categoryPage = this.GetCategoryPage(name);
			this.AddButton(name, string.IsNullOrEmpty(tooltip) ? ("Open the " + name + " menu") : tooltip, new Action(categoryPage.Open), sprite, "#ffffff");
			return categoryPage;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000F008 File Offset: 0x0000D208
		public ReCategoryPage AddCategoryPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReCategoryPage categoryPage = this.GetCategoryPage(text);
			bool flag = categoryPage != null;
			ReCategoryPage result;
			if (flag)
			{
				result = categoryPage;
			}
			else
			{
				ReCategoryPage reCategoryPage = new ReCategoryPage(text, false, color);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip, new Action(reCategoryPage.Open), sprite, color);
				result = reCategoryPage;
			}
			return result;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000F06B File Offset: 0x0000D26B
		public void AddMenuPage(string text, string tooltip, Action<ReMenuPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddMenuPage(text, tooltip, sprite, color));
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000F081 File Offset: 0x0000D281
		public void AddCategoryPage(string text, string tooltip, Action<ReCategoryPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddCategoryPage(text, tooltip, sprite, color));
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000F097 File Offset: 0x0000D297
		public void AddTabbedPage(string text, string tooltip, Action<ReTabbedPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddTabbedPage(text, tooltip, sprite, color));
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0000F0B0 File Offset: 0x0000D2B0
		public ReMenuPage GetMenuPage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReMenuPage(transform);
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0000F0F0 File Offset: 0x0000D2F0
		public ReCategoryPage GetCategoryPage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReCategoryPage(transform);
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0000F130 File Offset: 0x0000D330
		public ReTabbedPage GetTabbedPage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReTabbedPage(transform);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0000F170 File Offset: 0x0000D370
		public void ClearSubCategories()
		{
			for (int i = this._buttonContainer.RectTransform.childCount - 1; i >= 0; i--)
			{
				Transform child = this._buttonContainer.RectTransform.GetChild(i);
				bool flag = child != null;
				if (flag)
				{
					Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0000F1D0 File Offset: 0x0000D3D0
		public void Clear()
		{
			for (int i = this._buttonContainer.RectTransform.childCount - 1; i >= 0; i--)
			{
				Transform child = this._buttonContainer.RectTransform.GetChild(i);
				bool flag = child != null;
				if (flag)
				{
					Object.Destroy(child.gameObject);
				}
			}
		}

		// Token: 0x04000131 RID: 305
		public readonly ReMenuHeader Header;

		// Token: 0x04000132 RID: 306
		private readonly ReMenuButtonContainer _buttonContainer;
	}
}
