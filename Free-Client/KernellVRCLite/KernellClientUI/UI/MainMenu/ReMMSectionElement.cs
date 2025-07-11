using System;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x02000060 RID: 96
	public class ReMMSectionElement
	{
		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000440 RID: 1088 RVA: 0x00018E28 File Offset: 0x00017028
		// (set) Token: 0x06000441 RID: 1089 RVA: 0x00018E30 File Offset: 0x00017030
		public Transform LeftItemContainer { get; protected set; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000442 RID: 1090 RVA: 0x00018E39 File Offset: 0x00017039
		// (set) Token: 0x06000443 RID: 1091 RVA: 0x00018E41 File Offset: 0x00017041
		public Transform RightItemContainer { get; protected set; }

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000444 RID: 1092 RVA: 0x00018E4A File Offset: 0x0001704A
		// (set) Token: 0x06000445 RID: 1093 RVA: 0x00018E52 File Offset: 0x00017052
		public GameObject gameObject { get; protected set; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000446 RID: 1094 RVA: 0x00018E5B File Offset: 0x0001705B
		// (set) Token: 0x06000447 RID: 1095 RVA: 0x00018E63 File Offset: 0x00017063
		public StyleElement StyleElement { get; protected set; }

		// Token: 0x06000448 RID: 1096 RVA: 0x00018E6C File Offset: 0x0001706C
		public ReMMSectionElement(GameObject prefab, Transform container, bool sizefitter = true, bool separator = true)
		{
			this.gameObject = Object.Instantiate<GameObject>(prefab, container);
			this.LeftItemContainer = (this.gameObject.transform.Find("LeftItemContainer") ?? null);
			this.RightItemContainer = (this.gameObject.transform.Find("RightItemContainer") ?? null);
			this.StyleElement = (this.gameObject.GetComponent<StyleElement>() ?? null);
			bool flag = sizefitter && this.RightItemContainer != null;
			if (flag)
			{
				ContentSizeFitter contentSizeFitter = this.RightItemContainer.gameObject.AddComponent<ContentSizeFitter>();
				contentSizeFitter.horizontalFit = 2;
				contentSizeFitter.verticalFit = 2;
			}
			if (separator)
			{
				Object.Instantiate<GameObject>(MMenuPrefabs.MMSeparatorprefab, container);
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00018F35 File Offset: 0x00017135
		public ReMMSectionElement(GameObject prefab, ReMMCategorySection section, bool sizefitter = true) : this(prefab, section.ContentArea, sizefitter, true)
		{
		}
	}
}
