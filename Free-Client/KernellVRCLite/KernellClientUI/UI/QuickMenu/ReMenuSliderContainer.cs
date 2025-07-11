using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200004C RID: 76
	public class ReMenuSliderContainer : UiElement
	{
		// Token: 0x06000376 RID: 886 RVA: 0x00012254 File Offset: 0x00010454
		public ReMenuSliderContainer(string name, Transform parent = null) : base(QMMenuPrefabs.ContainerPrefab, (parent == null) ? QMMenuPrefabs.ContainerPrefab.transform.parent : parent, "Sliders_" + name, true)
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

		// Token: 0x06000377 RID: 887 RVA: 0x00012330 File Offset: 0x00010530
		public ReMenuSliderContainer(Transform transform) : base(transform)
		{
		}
	}
}
