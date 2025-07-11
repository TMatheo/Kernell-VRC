using System;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000046 RID: 70
	public class ReMenuHeader : UiElement
	{
		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000F910 File Offset: 0x0000DB10
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000F930 File Offset: 0x0000DB30
		public string Title
		{
			get
			{
				return this.TextComponent.text;
			}
			set
			{
				ReMenuHeader.<>c__DisplayClass3_0 CS$<>8__locals1 = new ReMenuHeader.<>c__DisplayClass3_0();
				CS$<>8__locals1.<>4__this = this;
				CS$<>8__locals1.value = value;
				MelonCoroutines.Start(CS$<>8__locals1.<set_Title>g__Wait|0());
			}
		}

		// Token: 0x06000306 RID: 774 RVA: 0x0000F960 File Offset: 0x0000DB60
		public ReMenuHeader(string title, Transform parent)
		{
			ReMenuHeader.<>c__DisplayClass4_0 CS$<>8__locals1 = new ReMenuHeader.<>c__DisplayClass4_0();
			CS$<>8__locals1.title = title;
			base..ctor(QMMenuPrefabs.MenuCategoryHeaderPrefab, (parent == null) ? QMMenuPrefabs.MenuCategoryHeaderPrefab.transform.parent : parent, "Header_" + CS$<>8__locals1.title, true);
			ReMenuHeader.<>c__DisplayClass4_1 CS$<>8__locals2 = new ReMenuHeader.<>c__DisplayClass4_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMenuHeader = this;
			this.TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|0());
			this.TextComponent.richText = true;
			this.TextComponent.transform.parent.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
		}

		// Token: 0x06000307 RID: 775 RVA: 0x0000FA0E File Offset: 0x0000DC0E
		public ReMenuHeader(Transform transform) : base(transform)
		{
			this.TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
		}

		// Token: 0x06000308 RID: 776 RVA: 0x0000FA2A File Offset: 0x0000DC2A
		protected ReMenuHeader(GameObject original, Transform parent, Vector3 pos, string name, bool defaultState = true) : base(original, parent, pos, name, defaultState)
		{
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0000FA3B File Offset: 0x0000DC3B
		protected ReMenuHeader(GameObject original, Transform parent, string name, bool defaultState = true) : base(original, parent, name, defaultState)
		{
		}

		// Token: 0x04000135 RID: 309
		protected TextMeshProUGUIEx TextComponent;
	}
}
