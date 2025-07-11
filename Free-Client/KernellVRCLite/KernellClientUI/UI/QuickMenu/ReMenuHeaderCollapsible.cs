using System;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000047 RID: 71
	public class ReMenuHeaderCollapsible : ReMenuHeader
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0000FA4C File Offset: 0x0000DC4C
		public ReMenuHeaderCollapsible(string title, Transform parent)
		{
			ReMenuHeaderCollapsible.<>c__DisplayClass1_0 CS$<>8__locals1 = new ReMenuHeaderCollapsible.<>c__DisplayClass1_0();
			CS$<>8__locals1.title = title;
			base..ctor(QMMenuPrefabs.MenuCategoryHeaderCollapsiblePrefav, (parent == null) ? QMMenuPrefabs.MenuCategoryHeaderCollapsiblePrefav.transform.parent : parent, "Header_" + CS$<>8__locals1.title, true);
			ReMenuHeaderCollapsible.<>c__DisplayClass1_1 CS$<>8__locals2 = new ReMenuHeaderCollapsible.<>c__DisplayClass1_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMenuHeaderCollapsible = this;
			this.TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|1());
			this.TextComponent.richText = true;
			FoldoutToggle component = base.GameObject.GetComponent<FoldoutToggle>();
			component.Method_Public_Void_String_Boolean_0("UI.ReMod." + UiElement.GetCleanName(CS$<>8__locals2.CS$<>8__locals1.title), false);
			component.Method_Public_Void_UnityAction_1_Boolean_0(delegate(bool b)
			{
				bool flag = CS$<>8__locals2.reMenuHeaderCollapsible.OnToggle != null;
				if (flag)
				{
					CS$<>8__locals2.reMenuHeaderCollapsible.OnToggle(b);
				}
			});
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000FB24 File Offset: 0x0000DD24
		public ReMenuHeaderCollapsible(Transform transform) : base(transform)
		{
			this.TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
		}

		// Token: 0x04000136 RID: 310
		public Action<bool> OnToggle;
	}
}
