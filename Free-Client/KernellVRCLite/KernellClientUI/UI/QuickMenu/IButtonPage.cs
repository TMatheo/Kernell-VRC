using System;
using UnityEngine;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200003D RID: 61
	public interface IButtonPage
	{
		// Token: 0x06000257 RID: 599
		ReMenuButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, string color = "#ffffff");

		// Token: 0x06000258 RID: 600
		ReMenuButton AddSpacer(Sprite sprite = null);

		// Token: 0x06000259 RID: 601
		ReMenuPage AddMenuPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		// Token: 0x0600025A RID: 602
		ReCategoryPage AddCategoryPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		// Token: 0x0600025B RID: 603
		ReTabbedPage AddTabbedPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		// Token: 0x0600025C RID: 604
		ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff");

		// Token: 0x0600025D RID: 605
		ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff");

		// Token: 0x0600025E RID: 606
		ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff");

		// Token: 0x0600025F RID: 607
		ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff");

		// Token: 0x06000260 RID: 608
		ReMenuPage GetMenuPage(string name);

		// Token: 0x06000261 RID: 609
		ReCategoryPage GetCategoryPage(string name);

		// Token: 0x06000262 RID: 610
		ReTabbedPage GetTabbedPage(string name);

		// Token: 0x06000263 RID: 611
		ReMenuPage ToMenuPage(string name, string tooltip = "", Sprite sprite = null);

		// Token: 0x06000264 RID: 612
		ReRadioTogglePage AddReRadioTogglePage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		// Token: 0x06000265 RID: 613
		ReCategoryPage ToCategoryPage(string name, string tooltip = "", Sprite sprite = null);

		// Token: 0x06000266 RID: 614
		void AddCategoryPage(string text, string tooltip, Action<ReCategoryPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff");

		// Token: 0x06000267 RID: 615
		void AddMenuPage(string text, string tooltip, Action<ReMenuPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff");

		// Token: 0x06000268 RID: 616
		void AddTabbedPage(string text, string tooltip, Action<ReTabbedPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff");

		// Token: 0x06000269 RID: 617
		ReRadioTogglePage AddRadioTogglePage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff");

		// Token: 0x0600026A RID: 618
		ReRadioTogglePage GetRadioTogglePage(string name);

		// Token: 0x0600026B RID: 619
		void AddRadioTogglePage(string text, string tooltip, Action<ReRadioTogglePage> onPageBuilt, Sprite sprite = null, string color = "#ffffff");

		// Token: 0x0600026C RID: 620
		ReRadioToggleGroup AddRadioToggleGroup(string groupName, Action<object> onSelectionChanged, string color = "#ffffff");
	}
}
