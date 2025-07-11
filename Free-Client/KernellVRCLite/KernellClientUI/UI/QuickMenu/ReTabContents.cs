using System;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000058 RID: 88
	public class ReTabContents : UiElement
	{
		// Token: 0x060003E0 RID: 992 RVA: 0x0001655C File Offset: 0x0001475C
		public ReTabContents(string title, string color = "#ffffff", Transform parent = null) : base(QMMenuPrefabs.TabContentPrefab, parent, "Contents_" + title, true)
		{
			Object.DestroyImmediate(base.GameObject.GetComponent<MonoBehaviour1PublicBuObGaBuObObObUnique>());
			base.GameObject.GetComponent<Canvas>().enabled = true;
			base.GameObject.GetComponent<GraphicRaycaster>().enabled = true;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00012330 File Offset: 0x00010530
		public ReTabContents(Transform transform) : base(transform)
		{
		}
	}
}
