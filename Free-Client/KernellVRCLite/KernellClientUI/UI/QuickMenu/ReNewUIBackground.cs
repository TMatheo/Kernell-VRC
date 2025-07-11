using System;
using KernellClientUI.VRChat;
using UnityEngine;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000051 RID: 81
	public class ReNewUIBackground : UiElement
	{
		// Token: 0x06000398 RID: 920 RVA: 0x00012E8C File Offset: 0x0001108C
		public ReNewUIBackground(string name, Transform parent = null) : base(QMMenuPrefabs.NewBackgroundPrefab, (parent == null) ? QMMenuPrefabs.NewBackgroundPrefab.transform.parent : parent, "BackgroundInfo_" + name, true)
		{
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00012330 File Offset: 0x00010530
		public ReNewUIBackground(Transform transform) : base(transform)
		{
		}
	}
}
