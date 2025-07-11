using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000050 RID: 80
	public class ReNewMenuContainer : UiElement
	{
		// Token: 0x06000396 RID: 918 RVA: 0x00012DB0 File Offset: 0x00010FB0
		public ReNewMenuContainer(string name, Transform parent = null) : base(QMMenuPrefabs.NewContainerPrefab, (parent == null) ? QMMenuPrefabs.NewContainerPrefab.transform.parent : parent, "NewUI_" + name, true)
		{
			IEnumerator enumerator = base.RectTransform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Object @object = enumerator.Current;
					Transform transform = @object.Cast<Transform>();
					bool flag = !(transform == null);
					if (flag)
					{
						Object.Destroy(transform.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				bool flag2 = disposable != null;
				if (flag2)
				{
					disposable.Dispose();
				}
			}
			VerticalLayoutGroup component = base.GameObject.GetComponent<VerticalLayoutGroup>();
			component.m_Padding = new RectOffset(64, 64, 0, 0);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00012330 File Offset: 0x00010530
		public ReNewMenuContainer(Transform transform) : base(transform)
		{
		}
	}
}
