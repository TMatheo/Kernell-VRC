using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KernellClientUI.VRChat;
using MelonLoader;
using TMPro;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x0200005E RID: 94
	public class ReMMOptionSelector : ReMMSectionElement
	{
		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000418 RID: 1048 RVA: 0x0001760C File Offset: 0x0001580C
		// (set) Token: 0x06000419 RID: 1049 RVA: 0x00017614 File Offset: 0x00015814
		public ReMMCategorySection Section { get; private set; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x0600041A RID: 1050 RVA: 0x0001761D File Offset: 0x0001581D
		// (set) Token: 0x0600041B RID: 1051 RVA: 0x00017625 File Offset: 0x00015825
		public string CurrentOption { get; private set; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600041C RID: 1052 RVA: 0x0001762E File Offset: 0x0001582E
		// (set) Token: 0x0600041D RID: 1053 RVA: 0x00017636 File Offset: 0x00015836
		public int CurrentOptionIndex { get; private set; }

		// Token: 0x0600041E RID: 1054 RVA: 0x00017640 File Offset: 0x00015840
		public ReMMOptionSelector(string title, string tooltipForward = "Next", string tooltipBackward = "Back", uint defaultOptionIndex = 0U, Transform parent = null, bool separator = true, string color = "#ffffff", ReMMCategorySection section = null)
		{
			ReMMOptionSelector.<>c__DisplayClass17_0 CS$<>8__locals1 = new ReMMOptionSelector.<>c__DisplayClass17_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.title = title;
			base..ctor(MMenuPrefabs.MMSelectorPrefab, parent, true, separator);
			ReMMOptionSelector.<>c__DisplayClass17_1 CS$<>8__locals2 = new ReMMOptionSelector.<>c__DisplayClass17_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMMOptionSelector = this;
			CS$<>8__locals2._textComponent = base.LeftItemContainer.Find("Title").GetComponent<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|3());
			CS$<>8__locals2._textComponent.richText = true;
			base.StyleElement = base.RightItemContainer.GetComponent<StyleElement>();
			bool flag = section != null;
			if (flag)
			{
				this.Section = section;
			}
			this.buttonBack = base.RightItemContainer.Find("ButtonLeft").GetComponent<Button>();
			this.buttonForward = base.RightItemContainer.Find("ButtonRight").GetComponent<Button>();
			this.OptionTextComponent = base.RightItemContainer.Find("OptionSelectionBox/Text_MM_H3").GetComponent<TextMeshProUGUIEx>();
			this.OptionTextComponent.text = "";
			this.buttonBack.onClick.AddListener(delegate()
			{
				bool flag7 = CS$<>8__locals2.reMMOptionSelector.CurrentOptionIndex == 0;
				if (flag7)
				{
					CS$<>8__locals2.reMMOptionSelector.Set((uint)(CS$<>8__locals2.reMMOptionSelector.Options.Count - 1), true);
				}
				else
				{
					CS$<>8__locals2.reMMOptionSelector.Set((uint)(CS$<>8__locals2.reMMOptionSelector.CurrentOptionIndex - 1), true);
				}
			});
			this.buttonForward.onClick.AddListener(delegate()
			{
				bool flag7 = CS$<>8__locals2.reMMOptionSelector.Options.Count >= CS$<>8__locals2.reMMOptionSelector.CurrentOptionIndex + 1;
				if (flag7)
				{
					CS$<>8__locals2.reMMOptionSelector.Set((uint)(CS$<>8__locals2.reMMOptionSelector.CurrentOptionIndex + 1), true);
				}
			});
			LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltipForward, null, null, null);
			LocalizableString localizableString2 = LocalizableStringExtensions.Localize(tooltipForward, null, null, null);
			ToolTip toolTip = null;
			Il2CppArrayBase<ToolTip> components = this.buttonBack.GetComponents<ToolTip>();
			bool flag2 = components.Length > 0;
			if (flag2)
			{
				toolTip = components[0];
				for (int i = 1; i < components.Length; i++)
				{
					Object.DestroyImmediate(components[i]);
				}
			}
			bool flag3 = toolTip != null;
			if (flag3)
			{
				toolTip._localizableString = localizableString2;
				toolTip._alternateLocalizableString = localizableString2;
			}
			ToolTip toolTip2 = null;
			Il2CppArrayBase<ToolTip> components2 = this.buttonForward.GetComponents<ToolTip>();
			bool flag4 = components2.Length > 0;
			if (flag4)
			{
				toolTip2 = components2[0];
				for (int j = 1; j < components2.Length; j++)
				{
					Object.DestroyImmediate(components2[j]);
				}
			}
			bool flag5 = toolTip2 != null;
			if (flag5)
			{
				toolTip2._localizableString = localizableString;
				toolTip2._alternateLocalizableString = localizableString;
			}
			bool flag6 = this.Section != null;
			if (flag6)
			{
				this.Section.Category.ButtonObj.GetComponent<Button>().onClick.AddListener(delegate()
				{
					MelonCoroutines.Start(CS$<>8__locals2.reMMOptionSelector.fix1());
				});
			}
			this._defaultOptionIndex = defaultOptionIndex;
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x000178EA File Offset: 0x00015AEA
		private IEnumerator fix1()
		{
			yield return new WaitForSeconds(0.025f);
			try
			{
				this.Refresh();
				yield break;
			}
			catch (Exception)
			{
				yield break;
			}
			yield break;
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x000178F9 File Offset: 0x00015AF9
		public void Set(uint index, bool callBack = false)
		{
			this.SetCurrectStringOption(index, callBack);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00017908 File Offset: 0x00015B08
		public void AddOption(string name, Action Option)
		{
			bool flag = !this.Options.ContainsKey(name);
			if (flag)
			{
				this.Options.Add(name, Option);
				bool flag2 = this.Options.Count == 1;
				if (flag2)
				{
					this.Set(this._defaultOptionIndex, false);
				}
			}
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x0001795B File Offset: 0x00015B5B
		private void Refresh()
		{
			this.SetCurrectStringOption((uint)this.CurrentOptionIndex, false);
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x0001796C File Offset: 0x00015B6C
		private void SetCurrectStringOption(uint index, bool callBack = false)
		{
			bool flag = this.Options.Count != 0;
			if (flag)
			{
				int num = this.CurrentOptionIndex = (int)((ulong)index % (ulong)((long)this.Options.Count));
				KeyValuePair<string, Action> keyValuePair = Enumerable.ElementAt<KeyValuePair<string, Action>>(this.Options, num);
				this.CurrentOption = keyValuePair.Key;
				this.OptionTextComponent.text = this.CurrentOption;
				if (callBack)
				{
					keyValuePair.Value();
				}
			}
		}

		// Token: 0x040001AA RID: 426
		private Button buttonBack;

		// Token: 0x040001AB RID: 427
		private Button buttonForward;

		// Token: 0x040001AC RID: 428
		private TextMeshProUGUI OptionTextComponent;

		// Token: 0x040001AD RID: 429
		private readonly Dictionary<string, Action> Options = new Dictionary<string, Action>();

		// Token: 0x040001AE RID: 430
		private readonly uint _defaultOptionIndex;
	}
}
