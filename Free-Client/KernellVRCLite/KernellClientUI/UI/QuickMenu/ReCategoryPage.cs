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
	// Token: 0x0200003E RID: 62
	public class ReCategoryPage : UiElement
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600026D RID: 621 RVA: 0x0000CA22 File Offset: 0x0000AC22
		private static int SiblingIndex
		{
			get
			{
				return MenuEx.QMenuParent.transform.Find("Modal_AddMessage").GetSiblingIndex();
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000CA3D File Offset: 0x0000AC3D
		public UIPage UiPage { get; }

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600026F RID: 623 RVA: 0x0000CA48 File Offset: 0x0000AC48
		// (remove) Token: 0x06000270 RID: 624 RVA: 0x0000CA80 File Offset: 0x0000AC80
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnOpen;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000271 RID: 625 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		// (remove) Token: 0x06000272 RID: 626 RVA: 0x0000CAF0 File Offset: 0x0000ACF0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnClose;

		// Token: 0x06000273 RID: 627 RVA: 0x0000CB28 File Offset: 0x0000AD28
		public ReCategoryPage(string text, bool isRoot = false, string color = "#ffffff")
		{
			ReCategoryPage.<>c__DisplayClass14_0 CS$<>8__locals1 = new ReCategoryPage.<>c__DisplayClass14_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.text = text;
			base..ctor(QMMenuPrefabs.CategoryPagePrefab, MenuEx.QMenuParent, "Menu_" + CS$<>8__locals1.text, false);
			CS$<>8__locals1.<>4__this = this;
			ReCategoryPage.<>c__DisplayClass14_1 CS$<>8__locals2 = new ReCategoryPage.<>c__DisplayClass14_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reCategoryPage = this;
			bool flag = !ReCategoryPage._fixedLaunchpad;
			if (flag)
			{
				ReCategoryPage.FixLaunchpadScrolling();
				ReCategoryPage._fixedLaunchpad = true;
			}
			VRCScrollRect componentInChildren = base.RectTransform.GetComponentInChildren<VRCScrollRect>();
			componentInChildren.content.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
			componentInChildren.field_Public_Boolean_0 = true;
			componentInChildren.verticalScrollbarVisibility = 0;
			componentInChildren.m_VerticalScrollbarVisibility = 0;
			componentInChildren.enabled = true;
			componentInChildren.verticalScrollbar = componentInChildren.transform.Find("Scrollbar").GetComponent<Scrollbar>();
			VRCRectMask2D component = componentInChildren.viewport.GetComponent<VRCRectMask2D>();
			component.enabled = true;
			component.Method_Public_set_Void_Boolean_0(true);
			Object.DestroyImmediate(base.GameObject.GetComponent<UIPage>());
			base.RectTransform.SetSiblingIndex(ReCategoryPage.SiblingIndex);
			this._isRoot = isRoot;
			Transform child = base.RectTransform.GetChild(0);
			child.transform.Find("RightItemContainer/Button_QM_Expand").gameObject.SetActive(false);
			CS$<>8__locals2.titleText = child.GetComponentInChildren<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__WaitForActive|2());
			CS$<>8__locals2.titleText.text = string.Concat(new string[]
			{
				"<color=",
				CS$<>8__locals2.CS$<>8__locals1.color,
				">",
				CS$<>8__locals2.CS$<>8__locals1.text,
				"</color>"
			});
			CS$<>8__locals2.titleText.richText = true;
			bool flag2 = !this._isRoot;
			if (flag2)
			{
				child.Find("LeftItemContainer/Button_Back").gameObject.SetActive(true);
			}
			this._container = componentInChildren.content;
			IEnumerator enumerator = this._container.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Object @object = enumerator.Current;
					Transform transform = @object.Cast<Transform>();
					bool flag3 = transform != null;
					if (flag3)
					{
						Object.Destroy(transform.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				bool flag4 = disposable != null;
				if (flag4)
				{
					disposable.Dispose();
				}
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
				Action onOpen = CS$<>8__locals2.CS$<>8__locals1.<>4__this.OnOpen;
				if (onOpen != null)
				{
					onOpen();
				}
			};
			enableDisableListener.OnDisableEvent += delegate()
			{
				Action onClose = CS$<>8__locals2.CS$<>8__locals1.<>4__this.OnClose;
				if (onClose != null)
				{
					onClose();
				}
			};
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000CEF0 File Offset: 0x0000B0F0
		public ReCategoryPage(Transform transform) : base(transform)
		{
			this.UiPage = base.GameObject.GetComponent<UIPage>();
			this._isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(this.UiPage);
			this._container = base.RectTransform.GetComponentInChildren<VRCScrollRect>().content;
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000CF48 File Offset: 0x0000B148
		public void Open()
		{
			this.UiPage.gameObject.active = true;
			try
			{
				MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(this.UiPage.field_Public_String_0, null, false, 0);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000CF9C File Offset: 0x0000B19C
		public ReMenuCategory AddCategory(string title, bool collapsible = true, string color = "#ffffff", bool skipLayoutGroup = false)
		{
			ReMenuCategory category = this.GetCategory(title);
			bool flag = category != null;
			ReMenuCategory result;
			if (flag)
			{
				result = category;
			}
			else
			{
				Transform parent = skipLayoutGroup ? this._container.parent : this._container;
				result = new ReMenuCategory(title, parent, collapsible, color, false);
			}
			return result;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000CFE4 File Offset: 0x0000B1E4
		public ReMenuCategory AddCategory(string title, string color)
		{
			return this.AddCategory(title, true, color, false);
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000D000 File Offset: 0x0000B200
		public void ClearCategories()
		{
			for (int i = this._container.childCount - 1; i >= 0; i--)
			{
				Transform child = this._container.GetChild(i);
				Object.Destroy(child.gameObject);
			}
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000D048 File Offset: 0x0000B248
		public ReMenuCategory GetCategory(string name)
		{
			Transform transform = this._container.Find("Header_" + UiElement.GetCleanName(name));
			bool flag = transform == null;
			ReMenuCategory result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ReMenuHeader headerElement = new ReMenuHeader(transform);
				ReMenuButtonContainer container = new ReMenuButtonContainer(this._container.Find("Buttons_" + UiElement.GetCleanName(name)));
				result = new ReMenuCategory(headerElement, container);
			}
			return result;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000D0B8 File Offset: 0x0000B2B8
		public ReMenuSliderCategory AddSliderCategory(string title, bool collapsible = true, string color = "#ffffff", bool skipLayoutGroup = false)
		{
			ReMenuSliderCategory sliderCategory = this.GetSliderCategory(title);
			bool flag = sliderCategory != null;
			ReMenuSliderCategory result;
			if (flag)
			{
				result = sliderCategory;
			}
			else
			{
				Transform parent = skipLayoutGroup ? this._container.parent : this._container;
				result = new ReMenuSliderCategory(title, parent, collapsible, color);
			}
			return result;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000D100 File Offset: 0x0000B300
		public ReMenuSliderCategory AddSliderCategory(string title, string color)
		{
			return this.AddSliderCategory(title, true, color, false);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000D11C File Offset: 0x0000B31C
		public ReMenuSliderCategory GetSliderCategory(string name)
		{
			Transform transform = this._container.Find("Header_" + UiElement.GetCleanName(name));
			bool flag = transform == null;
			ReMenuSliderCategory result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ReMenuHeader headerElement = new ReMenuHeader(transform);
				ReMenuSliderContainer container = new ReMenuSliderContainer(this._container.Find("Sliders_" + UiElement.GetCleanName(name)));
				result = new ReMenuSliderCategory(headerElement, container);
			}
			return result;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000D18C File Offset: 0x0000B38C
		public ReNewMenuCategory AddNewCategory(string title, bool collapsible = true, string color = "#ffffff", bool skipLayoutGroup = false)
		{
			ReNewMenuCategory newCategory = this.GetNewCategory(title);
			bool flag = newCategory != null;
			ReNewMenuCategory result;
			if (flag)
			{
				result = newCategory;
			}
			else
			{
				Transform parent = skipLayoutGroup ? this._container.parent : this._container;
				result = new ReNewMenuCategory(title, parent, collapsible, color);
			}
			return result;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		public ReNewMenuCategory AddNewCategory(string title, string color)
		{
			return this.AddNewCategory(title, true, color, false);
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000D1F0 File Offset: 0x0000B3F0
		public ReNewMenuCategory GetNewCategory(string name)
		{
			Transform transform = this._container.Find("Header_" + UiElement.GetCleanName(name));
			bool flag = transform == null;
			ReNewMenuCategory result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ReMenuHeader headerElement = new ReMenuHeader(transform);
				ReNewMenuContainer container = new ReNewMenuContainer(this._container.Find("Sliders_" + UiElement.GetCleanName(name)));
				result = new ReNewMenuCategory(headerElement, container);
			}
			return result;
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D260 File Offset: 0x0000B460
		public static ReCategoryPage Create(string text, bool isRoot, string color = "#ffffff")
		{
			return new ReCategoryPage(text, isRoot, color);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000D27C File Offset: 0x0000B47C
		public void ClearSubCategories()
		{
			for (int i = this._container.childCount - 1; i >= 0; i--)
			{
				Transform child = this._container.GetChild(i);
				Object.Destroy(child.gameObject);
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		private static void FixLaunchpadScrolling()
		{
			UIPage component = MenuEx.QMDashboardMenu.GetComponent<UIPage>();
			VRCScrollRect componentInChildren = component.GetComponentInChildren<VRCScrollRect>();
			componentInChildren.content.GetComponent<VerticalLayoutGroup>().childControlHeight = true;
			componentInChildren.enabled = true;
			componentInChildren.verticalScrollbar = componentInChildren.transform.Find("Scrollbar").GetComponent<Scrollbar>();
			componentInChildren.viewport.GetComponent<RectMask2D>().enabled = true;
		}

		// Token: 0x04000105 RID: 261
		private readonly bool _isRoot;

		// Token: 0x04000106 RID: 262
		private readonly Transform _container;

		// Token: 0x04000107 RID: 263
		private static bool _fixedLaunchpad;
	}
}
