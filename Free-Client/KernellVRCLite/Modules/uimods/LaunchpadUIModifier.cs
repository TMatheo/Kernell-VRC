using System;
using System.Collections;
using MelonLoader;
using UnityEngine;

namespace KernellVRCLite.modules.uimods
{
	// Token: 0x020000A2 RID: 162
	public class LaunchpadUIModifier : MonoBehaviour
	{
		// Token: 0x0600085D RID: 2141 RVA: 0x00034280 File Offset: 0x00032480
		public static LaunchpadUIModifier GetInstance()
		{
			bool flag = LaunchpadUIModifier.instance == null;
			if (flag)
			{
				GameObject gameObject = new GameObject("LaunchpadUIModifierHolder");
				Object.DontDestroyOnLoad(gameObject);
				LaunchpadUIModifier.instance = gameObject.AddComponent<LaunchpadUIModifier>();
			}
			return LaunchpadUIModifier.instance;
		}

		// Token: 0x0600085E RID: 2142 RVA: 0x000342C8 File Offset: 0x000324C8
		public void ApplyUIModifications()
		{
			bool flag = this.isModifying;
			if (!flag)
			{
				MelonCoroutines.Start(this.ApplyUIModificationsDelayed());
			}
		}

		// Token: 0x0600085F RID: 2143 RVA: 0x000342EE File Offset: 0x000324EE
		private IEnumerator ApplyUIModificationsDelayed()
		{
			this.isModifying = true;
			yield return new WaitForEndOfFrame();
			yield return new WaitForSeconds(0.5f);
			bool success = true;
			foreach (LaunchpadUIModifier.UIElementConfig element in this.elementsToHide)
			{
				bool elementSuccess = false;
				IEnumerator hideCoroutine = this.HideGameObjectDelayed(element.Name, element.Path);
				while (hideCoroutine.MoveNext())
				{
					object obj = hideCoroutine.Current;
					bool result;
					bool flag;
					if (obj is bool)
					{
						result = (bool)obj;
						flag = true;
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					if (flag2)
					{
						elementSuccess = result;
						break;
					}
					yield return hideCoroutine.Current;
				}
				bool flag3 = !elementSuccess;
				if (flag3)
				{
					success = false;
				}
				hideCoroutine = null;
				element = null;
			}
			LaunchpadUIModifier.UIElementConfig[] array = null;
			foreach (LaunchpadUIModifier.SiblingIndexConfig config in this.siblingIndexChanges)
			{
				bool elementSuccess2 = false;
				IEnumerator reorderCoroutine = this.SetSiblingIndexDelayed(config.Name, config.Path, config.Index);
				while (reorderCoroutine.MoveNext())
				{
					object obj = reorderCoroutine.Current;
					bool result2;
					bool flag4;
					if (obj is bool)
					{
						result2 = (bool)obj;
						flag4 = true;
					}
					else
					{
						flag4 = false;
					}
					bool flag5 = flag4;
					if (flag5)
					{
						elementSuccess2 = result2;
						break;
					}
					yield return reorderCoroutine.Current;
				}
				bool flag6 = !elementSuccess2;
				if (flag6)
				{
					success = false;
				}
				reorderCoroutine = null;
				config = null;
			}
			LaunchpadUIModifier.SiblingIndexConfig[] array2 = null;
			bool flag7 = success;
			if (flag7)
			{
				Debug.Log("[LaunchpadUIModifier] UI modifications completed successfully");
			}
			else
			{
				Debug.LogWarning("[LaunchpadUIModifier] Some UI modifications failed");
			}
			this.isModifying = false;
			yield break;
		}

		// Token: 0x06000860 RID: 2144 RVA: 0x000342FD File Offset: 0x000324FD
		private IEnumerator HideGameObjectDelayed(string elementName, string path)
		{
			int attempts = 0;
			while (attempts < 10)
			{
				GameObject obj = null;
				try
				{
					obj = GameObject.Find(path);
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("[LaunchpadUIModifier] Exception while finding " + elementName + ": " + e.Message);
					yield break;
				}
				bool flag = obj != null;
				if (flag)
				{
					try
					{
						obj.SetActive(false);
						Debug.Log("[LaunchpadUIModifier] Successfully hid " + elementName);
						yield break;
					}
					catch (Exception ex)
					{
						Exception e2 = ex;
						Debug.LogError("[LaunchpadUIModifier] Exception while hiding " + elementName + ": " + e2.Message);
						yield break;
					}
				}
				int num = attempts;
				attempts = num + 1;
				yield return new WaitForSeconds(0.1f);
				obj = null;
			}
			Debug.LogWarning(string.Format("[LaunchpadUIModifier] {0} not found after {1} attempts at path: {2}", elementName, 10, path));
			yield return false;
			yield break;
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x0003431A File Offset: 0x0003251A
		private IEnumerator SetSiblingIndexDelayed(string elementName, string path, int index)
		{
			int attempts = 0;
			while (attempts < 10)
			{
				GameObject obj = null;
				try
				{
					obj = GameObject.Find(path);
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("[LaunchpadUIModifier] Exception while finding " + elementName + ": " + e.Message);
					yield break;
				}
				bool flag = obj != null && obj.transform != null;
				if (flag)
				{
					try
					{
						obj.transform.SetSiblingIndex(index);
						Debug.Log("[LaunchpadUIModifier] Successfully reordered " + elementName);
						yield break;
					}
					catch (Exception ex)
					{
						Exception e2 = ex;
						Debug.LogError("[LaunchpadUIModifier] Exception while reordering " + elementName + ": " + e2.Message);
						yield break;
					}
				}
				int num = attempts;
				attempts = num + 1;
				yield return new WaitForSeconds(0.1f);
				obj = null;
			}
			Debug.LogWarning(string.Format("[LaunchpadUIModifier] {0} not found after {1} attempts at path: {2}", elementName, 10, path));
			yield return false;
			yield break;
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x0003433E File Offset: 0x0003253E
		private void OnDestroy()
		{
			base.StopAllCoroutines();
			this.isModifying = false;
		}

		// Token: 0x0400040D RID: 1037
		private readonly LaunchpadUIModifier.UIElementConfig[] elementsToHide = new LaunchpadUIModifier.UIElementConfig[]
		{
			new LaunchpadUIModifier.UIElementConfig("Report Button", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/Header_H1/RightItemContainer/Button_QM_Report"),
			new LaunchpadUIModifier.UIElementConfig("Header Title Text", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickLinks/LeftItemContainer/Text_Title"),
			new LaunchpadUIModifier.UIElementConfig("Quick Actions Title", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions/LeftItemContainer/Text_Title"),
			new LaunchpadUIModifier.UIElementConfig("KernellNet Label", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_KernellNetLite/Label"),
			new LaunchpadUIModifier.UIElementConfig("KernellNet Arrow", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_KernellNetLite/Arrow")
		};

		// Token: 0x0400040E RID: 1038
		private readonly LaunchpadUIModifier.SiblingIndexConfig[] siblingIndexChanges = new LaunchpadUIModifier.SiblingIndexConfig[]
		{
			new LaunchpadUIModifier.SiblingIndexConfig("KernellNet Header", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_KernellNetLite", 4),
			new LaunchpadUIModifier.SiblingIndexConfig("QuickActions Header", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_QuickActions", 6),
			new LaunchpadUIModifier.SiblingIndexConfig("KernellNet Buttons", "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Buttons_KernellNetLite", 5)
		};

		// Token: 0x0400040F RID: 1039
		private static LaunchpadUIModifier instance;

		// Token: 0x04000410 RID: 1040
		private bool isModifying = false;

		// Token: 0x02000183 RID: 387
		private class UIElementConfig
		{
			// Token: 0x1700026D RID: 621
			// (get) Token: 0x06000D39 RID: 3385 RVA: 0x0004D098 File Offset: 0x0004B298
			public string Name { get; }

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x06000D3A RID: 3386 RVA: 0x0004D0A0 File Offset: 0x0004B2A0
			public string Path { get; }

			// Token: 0x06000D3B RID: 3387 RVA: 0x0004D0A8 File Offset: 0x0004B2A8
			public UIElementConfig(string name, string path)
			{
				this.Name = name;
				this.Path = path;
			}
		}

		// Token: 0x02000184 RID: 388
		private class SiblingIndexConfig
		{
			// Token: 0x1700026F RID: 623
			// (get) Token: 0x06000D3C RID: 3388 RVA: 0x0004D0C0 File Offset: 0x0004B2C0
			public string Name { get; }

			// Token: 0x17000270 RID: 624
			// (get) Token: 0x06000D3D RID: 3389 RVA: 0x0004D0C8 File Offset: 0x0004B2C8
			public string Path { get; }

			// Token: 0x17000271 RID: 625
			// (get) Token: 0x06000D3E RID: 3390 RVA: 0x0004D0D0 File Offset: 0x0004B2D0
			public int Index { get; }

			// Token: 0x06000D3F RID: 3391 RVA: 0x0004D0D8 File Offset: 0x0004B2D8
			public SiblingIndexConfig(string name, string path, int index)
			{
				this.Name = name;
				this.Path = path;
				this.Index = index;
			}
		}
	}
}
