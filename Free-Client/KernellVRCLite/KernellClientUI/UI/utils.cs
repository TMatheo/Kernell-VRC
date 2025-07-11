using System;
using UnityEngine;

namespace KernellClientUI.UI
{
	// Token: 0x02000036 RID: 54
	internal static class utils
	{
		// Token: 0x06000238 RID: 568 RVA: 0x0000C265 File Offset: 0x0000A465
		public static void DestroyChildren(Transform transform)
		{
			utils.DestroyChildren(transform, null);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x0000C270 File Offset: 0x0000A470
		public static void DestroyChildren(Transform transform, Func<Transform, bool> exclude)
		{
			for (int i = transform.childCount - 1; i >= 0; i--)
			{
				bool flag = exclude == null || exclude(transform.GetChild(i));
				if (flag)
				{
					Object.DestroyImmediate(transform.GetChild(i).gameObject);
				}
			}
		}
	}
}
