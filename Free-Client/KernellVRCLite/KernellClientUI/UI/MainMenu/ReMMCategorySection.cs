using System;
using KernellClientUI.VRChat;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x0200005D RID: 93
	public class ReMMCategorySection : UiElement
	{
		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x00017058 File Offset: 0x00015258
		// (set) Token: 0x060003FF RID: 1023 RVA: 0x00017060 File Offset: 0x00015260
		public Transform ContentArea { get; protected set; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000400 RID: 1024 RVA: 0x00017069 File Offset: 0x00015269
		// (set) Token: 0x06000401 RID: 1025 RVA: 0x00017071 File Offset: 0x00015271
		internal ReMMCategory Category { get; private set; }

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000402 RID: 1026 RVA: 0x0001707C File Offset: 0x0001527C
		// (set) Token: 0x06000403 RID: 1027 RVA: 0x00017099 File Offset: 0x00015299
		internal string Title
		{
			get
			{
				return this.TitleText.text;
			}
			set
			{
				this.TitleText.text = value;
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000170AC File Offset: 0x000152AC
		public ReMMCategorySection(string title, bool collapsible = false, Transform parent = null, ReMMCategory category = null, string color = "#ffffff")
		{
			ReMMCategorySection.<>c__DisplayClass13_0 CS$<>8__locals1 = new ReMMCategorySection.<>c__DisplayClass13_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.title = title;
			base..ctor(MMenuPrefabs.MMCategorySectionPrefab, parent ?? MMenuPrefabs.MMCategorySectionPrefab.transform.parent, "Section-" + CS$<>8__locals1.title, true);
			ReMMCategorySection.<>c__DisplayClass13_1 CS$<>8__locals2 = new ReMMCategorySection.<>c__DisplayClass13_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMMCategorySection = this;
			this.TitleContainer = base.GameObject.transform.Find("MM_Foldout/Label").gameObject;
			this.TitleText = this.TitleContainer.GetComponent<TextMeshProUGUI>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|0());
			this.TitleText.richText = true;
			bool flag = category != null;
			if (flag)
			{
				this.Category = category;
			}
			bool flag2 = !collapsible;
			if (flag2)
			{
				this.TitleContainer.transform.SetAsFirstSibling();
			}
			base.GameObject.transform.Find("MM_Foldout/Background_Button").GetComponent<Toggle>().enabled = collapsible;
			base.GameObject.transform.Find("MM_Foldout/Arrow").gameObject.SetActive(collapsible);
			this.ContentArea = base.GameObject.transform.Find("Settings_Panel_1/VerticalLayoutGroup");
			for (int i = this.ContentArea.childCount - 1; i >= 0; i--)
			{
				Object.Destroy(this.ContentArea.GetChild(i).gameObject);
			}
			Object.Instantiate<GameObject>(MMenuPrefabs.MMCategorySectionBackGroundPrefab, this.ContentArea);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x0001723C File Offset: 0x0001543C
		public ReMMSlider AddSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, bool separator = true, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new ReMMSlider(title, tooltip, onSlide, parent ?? this.ContentArea, separator, defaultValue, minValue, maxValue, color, this);
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00017270 File Offset: 0x00015470
		public ReMMSlider AddSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, bool separator = true, string color = "#ffffff")
		{
			return new ReMMSlider(title, tooltip, onSlide, parent ?? this.ContentArea, separator, 0f, 0f, 10f, color, this);
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x000172AC File Offset: 0x000154AC
		public ReMMSlider AddSlider(string title, string tooltip, Action<float> onSlide, Transform parent = null, string color = "#ffffff")
		{
			return new ReMMSlider(title, tooltip, onSlide, parent ?? this.ContentArea, true, 0f, 0f, 10f, color, this);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000172E8 File Offset: 0x000154E8
		public ReMMSlider AddSlider(string title, string tooltip, ConfigValue<float> configValue, Transform parent = null, bool separator = true, float defaultValue = 0f, float minValue = 0f, float maxValue = 10f, string color = "#ffffff")
		{
			return new ReMMSlider(title, tooltip, new Action<float>(configValue.SetValue), parent ?? this.ContentArea, separator, configValue, minValue, maxValue, color, this);
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00017328 File Offset: 0x00015528
		public ReMMSlider AddSlider(string title, string tooltip, ConfigValue<float> configValue, Transform parent = null, bool separator = true, string color = "#ffffff")
		{
			return new ReMMSlider(title, tooltip, new Action<float>(configValue.SetValue), parent ?? this.ContentArea, separator, configValue, 0f, 10f, color, this);
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00017370 File Offset: 0x00015570
		public ReMMSlider AddSlider(string title, string tooltip, ConfigValue<float> configValue, Transform parent = null, string color = "#ffffff")
		{
			return new ReMMSlider(title, tooltip, new Action<float>(configValue.SetValue), parent ?? this.ContentArea, true, configValue, 0f, 10f, color, this);
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x000173B8 File Offset: 0x000155B8
		public ReMMToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, bool separator = true, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new ReMMToggle(text, tooltip, onToggle, defaultValue, this.ContentArea, separator, iconOn, iconOff, color);
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x000173E4 File Offset: 0x000155E4
		public ReMMToggle AddToggle(string text, string tooltip, Action<bool> onToggle, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new ReMMToggle(text, tooltip, onToggle, false, this.ContentArea, true, iconOn, iconOff, color);
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001740C File Offset: 0x0001560C
		public ReMMToggle AddToggle(string text, string tooltip, Action<bool> onToggle, string color = "#ffffff")
		{
			return new ReMMToggle(text, tooltip, onToggle, false, this.ContentArea, true, null, null, color);
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x00017434 File Offset: 0x00015634
		public ReMMToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, bool defaultValue = false, bool separator = true, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new ReMMToggle(text, tooltip, new Action<bool>(configValue.SetValue), configValue, this.ContentArea, separator, iconOn, iconOff, color);
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x00017470 File Offset: 0x00015670
		public ReMMToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn = null, Sprite iconOff = null, string color = "#ffffff")
		{
			return new ReMMToggle(text, tooltip, new Action<bool>(configValue.SetValue), configValue, this.ContentArea, true, iconOn, iconOff, color);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000174A8 File Offset: 0x000156A8
		public ReMMToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff")
		{
			return new ReMMToggle(text, tooltip, new Action<bool>(configValue.SetValue), configValue, this.ContentArea, true, null, null, color);
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x000174E0 File Offset: 0x000156E0
		public ReMMButton AddButton(string title, string buttontext, string tooltip, Action onClick, bool separator = true, string color = "#ffffff")
		{
			return new ReMMButton(title, buttontext, tooltip, onClick, this.ContentArea, separator, color, false);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00017508 File Offset: 0x00015708
		public ReMMButton AddButton(string title, string tooltip, Action onClick, bool separator = true, string color = "#ffffff")
		{
			return new ReMMButton(title, "ㅤㅤㅤ", tooltip, onClick, this.ContentArea, separator, color, false);
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00017534 File Offset: 0x00015734
		public ReMMText AddLabel(string leftText, string rightText, string tooltip, int fontSize, bool separator = true, string color = "#ffffff")
		{
			return new ReMMText(leftText, rightText, tooltip, this.ContentArea, fontSize, separator, color);
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0001755C File Offset: 0x0001575C
		public ReMMText AddLabel(string text, string tooltip, int fontSize, bool separator = true, string color = "#ffffff")
		{
			return new ReMMText(text, "", tooltip, this.ContentArea, fontSize, separator, color);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00017588 File Offset: 0x00015788
		public ReMMOptionSelector AddnGetOptionSelector(string title, string tooltipForward = "Next", string tooltipBackward = "Back", uint defaultOptionIndex = 0U, bool separator = true, string color = "#ffffff")
		{
			return new ReMMOptionSelector(title, tooltipForward, tooltipBackward, defaultOptionIndex, this.ContentArea, separator, color, this);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000175B0 File Offset: 0x000157B0
		public ReMMOptionSelector AddnGetOptionSelector(string title, uint defaultOptionIndex = 0U, bool separator = true, string color = "#ffffff")
		{
			return new ReMMOptionSelector(title, "Next", "Back", defaultOptionIndex, this.ContentArea, separator, color, this);
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000175E0 File Offset: 0x000157E0
		public ReMMOptionSelector AddnGetOptionSelector(string title, bool separator = true, string color = "#ffffff")
		{
			return new ReMMOptionSelector(title, "Next", "Back", 0U, this.ContentArea, separator, color, this);
		}

		// Token: 0x040001A6 RID: 422
		protected GameObject TitleContainer;

		// Token: 0x040001A7 RID: 423
		protected TextMeshProUGUI TitleText;
	}
}
