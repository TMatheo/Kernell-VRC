using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000056 RID: 86
	public class ReTabbedPage : UiElement
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003D1 RID: 977 RVA: 0x0000CA22 File Offset: 0x0000AC22
		private static int SiblingIndex
		{
			get
			{
				return MenuEx.QMenuParent.transform.Find("Modal_AddMessage").GetSiblingIndex();
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003D2 RID: 978 RVA: 0x00015DC8 File Offset: 0x00013FC8
		public UIPage UiPage { get; }

		// Token: 0x14000010 RID: 16
		// (add) Token: 0x060003D3 RID: 979 RVA: 0x00015DD0 File Offset: 0x00013FD0
		// (remove) Token: 0x060003D4 RID: 980 RVA: 0x00015E08 File Offset: 0x00014008
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnOpen;

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060003D5 RID: 981 RVA: 0x00015E40 File Offset: 0x00014040
		// (remove) Token: 0x060003D6 RID: 982 RVA: 0x00015E78 File Offset: 0x00014078
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnClose;

		// Token: 0x060003D7 RID: 983 RVA: 0x00015EB0 File Offset: 0x000140B0
		public ReTabbedPage(string text, bool isRoot = false, string color = "#ffffff")
		{
			ReTabbedPage.<>c__DisplayClass15_0 CS$<>8__locals1 = new ReTabbedPage.<>c__DisplayClass15_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.text = text;
			base..ctor(QMMenuPrefabs.TabbedPagePrefab, MenuEx.QMenuParent, "Menu_" + CS$<>8__locals1.text, false);
			ReTabbedPage.<>c__DisplayClass15_1 CS$<>8__locals2 = new ReTabbedPage.<>c__DisplayClass15_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reTabbedPage = this;
			Object.DestroyImmediate(base.GameObject.GetComponent<MonoBehaviour1PublicBuToBuToUnique>());
			base.RectTransform.SetSiblingIndex(ReTabbedPage.SiblingIndex);
			string cleanName = UiElement.GetCleanName(CS$<>8__locals2.CS$<>8__locals1.text);
			this._isRoot = isRoot;
			Transform child = base.RectTransform.GetChild(0);
			CS$<>8__locals2.titleText = child.GetComponentInChildren<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|2());
			CS$<>8__locals2.titleText.richText = true;
			child.transform.Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);
			this.tabContainer = base.RectTransform.Find("Panel_Notification_Tabs/Tabs");
			IEnumerator enumerator = this.tabContainer.GetEnumerator();
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
			this.contentContainer = base.RectTransform.Find("Panel_Content/");
			IEnumerator enumerator2 = this.contentContainer.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Object object2 = enumerator2.Current;
					Transform transform2 = object2.Cast<Transform>();
					bool flag3 = !(transform2 == null);
					if (flag3)
					{
						Object.Destroy(transform2.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable2 = enumerator2 as IDisposable;
				bool flag4 = disposable2 != null;
				if (flag4)
				{
					disposable2.Dispose();
				}
			}
			this.contentContainer.GetChild(0).gameObject.SetActive(true);
			Object.DestroyImmediate(base.RectTransform.Find("Panel_NoNotifications_Message").gameObject);
			bool flag5 = !this._isRoot;
			if (flag5)
			{
				Button componentInChildren = child.GetComponentInChildren<Button>(true);
				componentInChildren.gameObject.SetActive(true);
			}
			this.UiPage = base.GameObject.AddComponent<UIPage>();
			this.UiPage.field_Public_String_0 = "QuickMenuReMod" + UiElement.GetCleanName(CS$<>8__locals2.CS$<>8__locals1.text);
			this.UiPage.field_Private_List_1_UIPage_0 = new List<UIPage>();
			this.UiPage.field_Private_List_1_UIPage_0.Add(this.UiPage);
			this.UiPage.GetComponent<Canvas>().enabled = true;
			this.UiPage.GetComponent<CanvasGroup>().enabled = true;
			this.UiPage.GetComponent<UIPage>().enabled = true;
			this.UiPage.GetComponent<GraphicRaycaster>().enabled = true;
			this.UiPage.gameObject.active = false;
			MenuEx.QMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(this.UiPage.field_Public_String_0, this.UiPage);
			if (isRoot)
			{
				List<UIPage> list = Enumerable.ToList<UIPage>(MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0);
				list.Add(this.UiPage);
				MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0 = list.ToArray();
			}
			EnableDisableListener enableDisableListener = base.GameObject.AddComponent<EnableDisableListener>();
			enableDisableListener.OnEnableEvent += delegate()
			{
				bool flag6 = CS$<>8__locals2.reTabbedPage.OnOpen != null;
				if (flag6)
				{
					CS$<>8__locals2.reTabbedPage.OnOpen();
				}
			};
			enableDisableListener.OnDisableEvent += delegate()
			{
				bool flag6 = CS$<>8__locals2.reTabbedPage.OnClose != null;
				if (flag6)
				{
					CS$<>8__locals2.reTabbedPage.OnClose();
				}
			};
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001626C File Offset: 0x0001446C
		public static ReTabbedPage Create(string text, bool isRoot, string color = "#ffffff")
		{
			return new ReTabbedPage(text, isRoot, color);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00016288 File Offset: 0x00014488
		public ReTabbedPage(Transform transform) : base(transform)
		{
			this.UiPage = base.GameObject.GetComponent<UIPage>();
			this._isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(this.UiPage);
			ScrollRect component = base.RectTransform.Find("Scrollrect").GetComponent<ScrollRect>();
			this._container = component.content;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x000162EC File Offset: 0x000144EC
		public void Open()
		{
			this.UiPage.gameObject.active = true;
			this.contentContainer.GetChild(0).gameObject.SetActive(true);
			MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(this.UiPage.field_Public_String_0, null, false, 0);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00016340 File Offset: 0x00014540
		public ReTab AddTab(string title, string color = "#ffffff")
		{
			return new ReTab(title, color, this.tabContainer);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x00016360 File Offset: 0x00014560
		public ReTabbedPage GetTabbedPage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReTabbedPage(transform);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x000163A0 File Offset: 0x000145A0
		private static void FixLaunchpadScrolling()
		{
			UIPage component = MenuEx.QMDashboardMenu.GetComponent<UIPage>();
			ScrollRect componentInChildren = component.GetComponentInChildren<ScrollRect>();
			componentInChildren.content.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
			componentInChildren.enabled = true;
			componentInChildren.verticalScrollbar = componentInChildren.transform.Find("Scrollbar").GetComponent<Scrollbar>();
			componentInChildren.viewport.GetComponent<RectMask2D>().enabled = true;
		}

		// Token: 0x04000190 RID: 400
		private readonly bool _isRoot;

		// Token: 0x04000191 RID: 401
		private readonly Transform _container;

		// Token: 0x04000192 RID: 402
		public Transform tabContainer;

		// Token: 0x04000193 RID: 403
		public Transform contentContainer;
	}
}
