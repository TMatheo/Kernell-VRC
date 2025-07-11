using System;
using UnityEngine;

namespace KernellClientUI.UI.MainMenu.Header
{
	// Token: 0x02000068 RID: 104
	public class ReMMHeaderElement
	{
		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00019E56 File Offset: 0x00018056
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x00019E5E File Offset: 0x0001805E
		internal GameObject gameObject { get; private set; }

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x00019E67 File Offset: 0x00018067
		public Transform Container
		{
			get
			{
				return this.gameObject.transform.parent;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x0600046F RID: 1135 RVA: 0x00019E79 File Offset: 0x00018079
		// (set) Token: 0x06000470 RID: 1136 RVA: 0x00019E81 File Offset: 0x00018081
		public ReMMPage Page { get; private set; }

		// Token: 0x06000471 RID: 1137 RVA: 0x00019E8A File Offset: 0x0001808A
		public ReMMHeaderElement(GameObject prefab, ReMMPage page, string tooltip)
		{
			this.gameObject = Object.Instantiate<GameObject>(prefab, page.MenuObject.transform.Find("Menu_MM_DynamicSidePanel/Panel_SectionList/ScrollRect_Navigation/ScrollRect_Content/Header_MM_H2/RightItemContainer"));
			this.Page = page;
		}
	}
}
