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
	// Token: 0x02000049 RID: 73
	public class ReMenuPage : UiElement, IButtonPage
	{
		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0000CA22 File Offset: 0x0000AC22
		private static int SiblingIndex
		{
			get
			{
				return MenuEx.QMenuParent.transform.Find("Modal_AddMessage").GetSiblingIndex();
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600030D RID: 781 RVA: 0x0000FB40 File Offset: 0x0000DD40
		public UIPage UiPage { get; }

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x0600030E RID: 782 RVA: 0x0000FB48 File Offset: 0x0000DD48
		// (remove) Token: 0x0600030F RID: 783 RVA: 0x0000FB80 File Offset: 0x0000DD80
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnOpen;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x06000310 RID: 784 RVA: 0x0000FBB8 File Offset: 0x0000DDB8
		// (remove) Token: 0x06000311 RID: 785 RVA: 0x0000FBF0 File Offset: 0x0000DDF0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnClose;

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x06000312 RID: 786 RVA: 0x0000FC25 File Offset: 0x0000DE25
		// (set) Token: 0x06000313 RID: 787 RVA: 0x0000FC30 File Offset: 0x0000DE30
		public ButtonLayoutShape LayoutShape
		{
			get
			{
				return this._currentLayoutShape;
			}
			set
			{
				bool flag = this._currentLayoutShape != value;
				if (flag)
				{
					this._currentLayoutShape = value;
					this.UpdateLayout();
				}
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000FC5E File Offset: 0x0000DE5E
		// (set) Token: 0x06000315 RID: 789 RVA: 0x0000FC68 File Offset: 0x0000DE68
		public bool UseThinButtons
		{
			get
			{
				return this._useThinButtons;
			}
			set
			{
				bool flag = this._useThinButtons != value;
				if (flag)
				{
					this._useThinButtons = value;
					this.UpdateExistingButtonsThinMode();
					this.UpdateLayout();
				}
			}
		}

		// Token: 0x06000316 RID: 790 RVA: 0x0000FCA0 File Offset: 0x0000DEA0
		public ReMenuPage(string text, bool isRoot = false, string color = "#ffffff")
		{
			ReMenuPage.<>c__DisplayClass36_0 CS$<>8__locals1 = new ReMenuPage.<>c__DisplayClass36_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.text = text;
			base..ctor(QMMenuPrefabs.MenuPagePrefab, MenuEx.QMenuParent, "Menu_" + CS$<>8__locals1.text, false);
			CS$<>8__locals1.<>4__this = this;
			ReMenuPage.<>c__DisplayClass36_1 CS$<>8__locals2 = new ReMenuPage.<>c__DisplayClass36_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMenuPage = this;
			Object.DestroyImmediate(base.GameObject.GetComponent<MonoBehaviour1PublicBuToBuToUnique>());
			base.RectTransform.SetSiblingIndex(ReMenuPage.SiblingIndex);
			string cleanName = UiElement.GetCleanName(CS$<>8__locals2.CS$<>8__locals1.text);
			this._isRoot = isRoot;
			Transform child = base.RectTransform.GetChild(0);
			CS$<>8__locals2.titleText = child.GetComponentInChildren<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__WaitForActive|2());
			CS$<>8__locals2.titleText.richText = true;
			bool flag = !this._isRoot;
			if (flag)
			{
				child.Find("LeftItemContainer/Button_Back").gameObject.SetActive(true);
			}
			child.name = "Header_H1";
			this.CleanExistingLayout();
			this.UiPage = base.GameObject.AddComponent<UIPage>();
			this.UiPage.field_Public_String_0 = "Page_" + cleanName;
			this.UiPage.field_Private_List_1_UIPage_0 = new List<UIPage>();
			this.UiPage.field_Private_List_1_UIPage_0.Add(this.UiPage);
			this.UiPage.GetComponent<Canvas>().enabled = true;
			this.UiPage.GetComponent<CanvasGroup>().enabled = true;
			this.UiPage.GetComponent<UIPage>().enabled = true;
			this.UiPage.GetComponent<GraphicRaycaster>().enabled = true;
			this.UiPage.gameObject.active = false;
			CS$<>8__locals2.scrollRect = base.RectTransform.Find("Scrollrect").GetComponent<VRCScrollRect>();
			this._container = CS$<>8__locals2.scrollRect.content;
			this.ConfigureContainer(CS$<>8__locals2.scrollRect);
			this.ConfigureScrolling(CS$<>8__locals2.scrollRect);
			this.RegisterWithMenuController(isRoot);
			EnableDisableListener enableDisableListener = base.GameObject.AddComponent<EnableDisableListener>();
			enableDisableListener.OnEnableEvent += delegate()
			{
				Action onOpen = CS$<>8__locals2.CS$<>8__locals1.<>4__this.OnOpen;
				if (onOpen != null)
				{
					onOpen();
				}
				MelonCoroutines.Start(base.<.ctor>g__MeasureViewport|3());
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

		// Token: 0x06000317 RID: 791 RVA: 0x0000FF90 File Offset: 0x0000E190
		public ReMenuPage(Transform transform) : base(transform)
		{
			this.UiPage = base.GameObject.GetComponent<UIPage>();
			this._isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(this.UiPage);
			ScrollRect component = base.RectTransform.Find("Scrollrect").GetComponent<ScrollRect>();
			this._container = component.content;
			RectTransform rectTransform = this._container as RectTransform;
			bool flag = rectTransform != null;
			if (flag)
			{
				rectTransform.pivot = new Vector2(0f, 1f);
			}
			RectTransform viewport = component.viewport;
			bool flag2 = viewport != null;
			if (flag2)
			{
				Rect rect = viewport.rect;
				this._viewportWidth = rect.width;
				this._viewportHeight = rect.height;
			}
		}

		// Token: 0x06000318 RID: 792 RVA: 0x00010110 File Offset: 0x0000E310
		private void CleanExistingLayout()
		{
			Transform transform = base.RectTransform.Find("Scrollrect/Viewport/VerticalLayoutGroup/Buttons");
			bool flag = transform != null;
			if (flag)
			{
				IEnumerator enumerator = transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Object @object = enumerator.Current;
						Transform transform2 = @object as Transform;
						bool flag2 = transform2 != null;
						if (flag2)
						{
							Object.Destroy(transform2.gameObject);
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					bool flag3 = disposable != null;
					if (flag3)
					{
						disposable.Dispose();
					}
				}
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x000101AC File Offset: 0x0000E3AC
		private void ConfigureContainer(VRCScrollRect scrollRect)
		{
			Transform transform = base.GameObject.transform.Find("Scrollrect/Viewport/VerticalLayoutGroup");
			bool flag = transform != null;
			if (flag)
			{
				VerticalLayoutGroup component = transform.GetComponent<VerticalLayoutGroup>();
				bool flag2 = component != null;
				if (flag2)
				{
					Object.DestroyImmediate(component);
				}
			}
			Transform transform2 = this._container.Find("Buttons");
			bool flag3 = transform2 != null;
			if (flag3)
			{
				Object.DestroyImmediate(transform2.gameObject);
			}
			Transform transform3 = this._container.Find("Spacer_8pt");
			bool flag4 = transform3 != null;
			if (flag4)
			{
				Object.DestroyImmediate(transform3.gameObject);
			}
			GameObject gameObject = new GameObject("CustomButtonContainer");
			gameObject.transform.SetParent(this._container);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			ContentSizeFitter contentSizeFitter = gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = 2;
			contentSizeFitter.verticalFit = 2;
			RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
			bool flag5 = rectTransform == null;
			if (flag5)
			{
				rectTransform = gameObject.AddComponent<RectTransform>();
			}
			rectTransform.anchorMin = new Vector2(0f, 1f);
			rectTransform.anchorMax = new Vector2(0f, 1f);
			rectTransform.pivot = new Vector2(0f, 1f);
			this._container = gameObject.transform;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x00010334 File Offset: 0x0000E534
		private void ConfigureScrolling(VRCScrollRect scrollRect)
		{
			Transform transform = scrollRect.transform.Find("Scrollbar");
			transform.gameObject.SetActive(true);
			scrollRect.field_Public_Boolean_0 = true;
			scrollRect.enabled = true;
			scrollRect.verticalScrollbar = transform.GetComponent<Scrollbar>();
			scrollRect.verticalScrollbarVisibility = 0;
			VRCRectMask2D component = scrollRect.viewport.GetComponent<VRCRectMask2D>();
			component.enabled = true;
			component.Method_Public_set_Void_Boolean_0(true);
		}

		// Token: 0x0600031B RID: 795 RVA: 0x000103A4 File Offset: 0x0000E5A4
		private void RegisterWithMenuController(bool isRoot)
		{
			MenuEx.QMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(this.UiPage.field_Public_String_0, this.UiPage);
			if (isRoot)
			{
				List<UIPage> list = Enumerable.ToList<UIPage>(MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0);
				list.Add(this.UiPage);
				MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0 = list.ToArray();
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0001040D File Offset: 0x0000E60D
		public void Open()
		{
			this.UiPage.gameObject.active = true;
			MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(this.UiPage.field_Public_String_0, null, false, 0);
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0001043C File Offset: 0x0000E63C
		private void UpdateLayout()
		{
			bool flag = this._buttons.Count == 0;
			if (!flag)
			{
				this._directPositions.Clear();
				switch (this._currentLayoutShape)
				{
				case ButtonLayoutShape.Grid:
					this.CalculateGridLayout();
					break;
				case ButtonLayoutShape.Horizontal:
					this.CalculateHorizontalLayout();
					break;
				case ButtonLayoutShape.Vertical:
					this.CalculateVerticalLayout();
					break;
				case ButtonLayoutShape.CircleTop:
					this.CalculateCircleTopLayout();
					break;
				case ButtonLayoutShape.CircleBottom:
					this.CalculateCircleBottomLayout();
					break;
				case ButtonLayoutShape.CircleFull:
					this.CalculateCircleFullLayout();
					break;
				case ButtonLayoutShape.DiagonalRight:
					this.CalculateDiagonalRightLayout();
					break;
				case ButtonLayoutShape.DiagonalLeft:
					this.CalculateDiagonalLeftLayout();
					break;
				case ButtonLayoutShape.SidesAndBottom:
					this.CalculateSidesAndBottomLayout();
					break;
				case ButtonLayoutShape.FivePanelLayout:
					this.CalculateFivePanelLayout();
					break;
				}
				this.ApplyButtonPositions();
				this.UpdateContentSize();
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00010510 File Offset: 0x0000E710
		private void CalculateGridLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			int num2 = Mathf.Max(1, Mathf.FloorToInt((this._viewportWidth - this._leftSafetyMargin - this._rightSafetyMargin - this._horizontalOffset + this._horizontalSpacing) / (this._buttonWidth + this._horizontalSpacing)));
			for (int i = 0; i < this._buttons.Count; i++)
			{
				int num3 = i / num2;
				int num4 = i % num2;
				float num5 = this._leftSafetyMargin + this._horizontalOffset + (float)num4 * (this._buttonWidth + this._horizontalSpacing);
				float num6 = -(this._topSafetyMargin + this._verticalOffset + (float)num3 * this._verticalSpacing);
				this._directPositions[i] = new Vector3(num5, num6, 0f);
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x000105F0 File Offset: 0x0000E7F0
		private void CalculateHorizontalLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			float num2 = -(this._topSafetyMargin + this._verticalOffset);
			for (int i = 0; i < this._buttons.Count; i++)
			{
				float num3 = this._leftSafetyMargin + this._horizontalOffset + (float)i * (this._buttonWidth + this._horizontalSpacing);
				this._directPositions[i] = new Vector3(num3, num2, 0f);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0001067C File Offset: 0x0000E87C
		private void CalculateVerticalLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			float num2 = this._leftSafetyMargin + this._horizontalOffset + (this._viewportWidth - this._leftSafetyMargin - this._rightSafetyMargin - this._buttonWidth) / 2f;
			for (int i = 0; i < this._buttons.Count; i++)
			{
				float num3 = -(this._topSafetyMargin + this._verticalOffset + (float)i * this._verticalSpacing);
				this._directPositions[i] = new Vector3(num2, num3, 0f);
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00010724 File Offset: 0x0000E924
		private void CalculateCircleTopLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			float num2 = Mathf.Min(this._viewportWidth, this._viewportHeight) * 0.35f;
			float num3 = this._viewportWidth / 2f + this._horizontalOffset;
			float num4 = -(this._topSafetyMargin + this._verticalOffset + num2);
			float num5 = 180f;
			float num6 = 360f;
			for (int i = 0; i < this._buttons.Count; i++)
			{
				float num7 = (this._buttons.Count <= 1) ? 0.5f : ((float)i / (float)(this._buttons.Count - 1));
				float num8 = Mathf.Lerp(num5, num6, num7) * 0.017453292f;
				float num9 = num3 + Mathf.Cos(num8) * num2;
				float num10 = num4 + Mathf.Sin(num8) * num2;
				this._directPositions[i] = new Vector3(num9, num10, 0f);
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0001082C File Offset: 0x0000EA2C
		private void CalculateCircleBottomLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			float num2 = Mathf.Min(this._viewportWidth, this._viewportHeight) * 0.35f;
			float num3 = this._viewportWidth / 2f + this._horizontalOffset;
			float num4 = -(this._topSafetyMargin + this._verticalOffset + num2);
			float num5 = 0f;
			float num6 = 180f;
			for (int i = 0; i < this._buttons.Count; i++)
			{
				float num7 = (this._buttons.Count <= 1) ? 0.5f : ((float)i / (float)(this._buttons.Count - 1));
				float num8 = Mathf.Lerp(num5, num6, num7) * 0.017453292f;
				float num9 = num3 + Mathf.Cos(num8) * num2;
				float num10 = num4 + Mathf.Sin(num8) * num2;
				this._directPositions[i] = new Vector3(num9, num10, 0f);
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00010934 File Offset: 0x0000EB34
		private void CalculateCircleFullLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			float num2 = Mathf.Min(this._viewportWidth, this._viewportHeight) * 0.35f;
			float num3 = this._viewportWidth / 2f + this._horizontalOffset;
			float num4 = -(this._topSafetyMargin + this._verticalOffset + num2);
			float num5 = 360f / (float)this._buttons.Count;
			for (int i = 0; i < this._buttons.Count; i++)
			{
				float num6 = (float)i * num5 * 0.017453292f;
				float num7 = num3 + Mathf.Cos(num6) * num2;
				float num8 = num4 + Mathf.Sin(num6) * num2;
				this._directPositions[i] = new Vector3(num7, num8, 0f);
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00010A10 File Offset: 0x0000EC10
		private void CalculateDiagonalRightLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			float num2 = Mathf.Min(this._viewportWidth - this._leftSafetyMargin - this._rightSafetyMargin, this._viewportHeight - this._topSafetyMargin - this._bottomSafetyMargin);
			for (int i = 0; i < this._buttons.Count; i++)
			{
				float num3 = (this._buttons.Count <= 1) ? 0.5f : ((float)i / (float)(this._buttons.Count - 1));
				float num4 = this._leftSafetyMargin + this._horizontalOffset + num3 * num2;
				float num5 = -(this._topSafetyMargin + this._verticalOffset + num3 * num2);
				this._directPositions[i] = new Vector3(num4, num5, 0f);
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00010AF0 File Offset: 0x0000ECF0
		private void CalculateDiagonalLeftLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			float num2 = Mathf.Min(this._viewportWidth - this._leftSafetyMargin - this._rightSafetyMargin, this._viewportHeight - this._topSafetyMargin - this._bottomSafetyMargin);
			for (int i = 0; i < this._buttons.Count; i++)
			{
				float num3 = (this._buttons.Count <= 1) ? 0.5f : ((float)i / (float)(this._buttons.Count - 1));
				float num4 = this._viewportWidth - this._rightSafetyMargin + this._horizontalOffset - num3 * num2;
				float num5 = -(this._topSafetyMargin + this._verticalOffset + num3 * num2);
				this._directPositions[i] = new Vector3(num4, num5, 0f);
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00010BD4 File Offset: 0x0000EDD4
		private void CalculateSidesAndBottomLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			bool flag = this._buttons.Count == 0;
			if (!flag)
			{
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				bool flag2 = this._buttons.Count == 1;
				if (flag2)
				{
					num4 = 1;
				}
				else
				{
					bool flag3 = this._buttons.Count == 2;
					if (flag3)
					{
						num2 = 1;
						num3 = 1;
					}
					else
					{
						num4 = this._buttons.Count % 2;
						int num5 = (this._buttons.Count - num4) / 2;
						num2 = num5;
						num3 = num5;
					}
				}
				float num6 = this._leftSafetyMargin + this._horizontalOffset;
				float num7 = this._viewportWidth - this._rightSafetyMargin - this._buttonWidth + this._horizontalOffset;
				float num8 = -(this._viewportHeight - this._bottomSafetyMargin - num - this._verticalOffset);
				int num9 = 0;
				for (int i = 0; i < num2; i++)
				{
					float num10 = -(this._topSafetyMargin + this._verticalOffset + (float)i * this._verticalSpacing);
					this._directPositions[num9++] = new Vector3(num6, num10, 0f);
				}
				for (int j = 0; j < num3; j++)
				{
					float num11 = -(this._topSafetyMargin + this._verticalOffset + (float)j * this._verticalSpacing);
					this._directPositions[num9++] = new Vector3(num7, num11, 0f);
				}
				for (int k = 0; k < num4; k++)
				{
					float num12 = (this._viewportWidth - this._buttonWidth) / 2f + this._horizontalOffset;
					this._directPositions[num9++] = new Vector3(num12, num8, 0f);
				}
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x00010DBC File Offset: 0x0000EFBC
		private void CalculateFivePanelLayout()
		{
			float num = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
			bool flag = this._buttons.Count == 0;
			if (!flag)
			{
				float num2 = (this._viewportWidth - this._buttonWidth) / 2f + this._horizontalOffset;
				float num3 = -((this._viewportHeight - num) / 2f + this._verticalOffset);
				float num4 = this._buttonWidth + this._horizontalSpacing;
				float verticalSpacing = this._verticalSpacing;
				Vector3[] array = new Vector3[]
				{
					new Vector3(num2, num3, 0f),
					new Vector3(num2 - num4, num3 - verticalSpacing, 0f),
					new Vector3(num2 + num4, num3 - verticalSpacing, 0f),
					new Vector3(num2 - num4, num3 + verticalSpacing, 0f),
					new Vector3(num2 + num4, num3 + verticalSpacing, 0f)
				};
				for (int i = 0; i < Mathf.Min(this._buttons.Count, array.Length); i++)
				{
					this._directPositions[i] = array[i];
				}
				bool flag2 = this._buttons.Count > array.Length;
				if (flag2)
				{
					int num5 = this._buttons.Count - array.Length;
					int num6 = 3;
					float num7 = num3 - verticalSpacing * 2f - this._horizontalSpacing;
					float num8 = num2 - (float)num6 * (this._buttonWidth + this._horizontalSpacing) / 2f + this._buttonWidth / 2f;
					for (int j = 0; j < num5; j++)
					{
						int num9 = j / num6;
						int num10 = j % num6;
						float num11 = num8 + (float)num10 * (this._buttonWidth + this._horizontalSpacing);
						float num12 = num7 - (float)num9 * this._verticalSpacing;
						this._directPositions[array.Length + j] = new Vector3(num11, num12, 0f);
					}
				}
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00010FD8 File Offset: 0x0000F1D8
		private void ApplyButtonPositions()
		{
			for (int i = 0; i < this._buttons.Count; i++)
			{
				UiElement uiElement = this._buttons[i];
				bool flag = ((uiElement != null) ? uiElement.RectTransform : null) == null;
				if (!flag)
				{
					Vector3 localPosition;
					bool flag2 = this._directPositions.TryGetValue(i, out localPosition);
					if (flag2)
					{
						this._buttons[i].RectTransform.localPosition = localPosition;
						this.EnsureButtonIgnoresLayout(this._buttons[i]);
					}
				}
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00011068 File Offset: 0x0000F268
		private void EnsureButtonIgnoresLayout(UiElement button)
		{
			bool flag = ((button != null) ? button.RectTransform : null) == null;
			if (!flag)
			{
				LayoutElement layoutElement = button.RectTransform.GetComponent<LayoutElement>();
				bool flag2 = layoutElement == null;
				if (flag2)
				{
					layoutElement = button.RectTransform.gameObject.AddComponent<LayoutElement>();
				}
				layoutElement.ignoreLayout = true;
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x000110C0 File Offset: 0x0000F2C0
		private void UpdateContentSize()
		{
			bool flag = this._buttons.Count == 0;
			if (!flag)
			{
				float num = float.MaxValue;
				float num2 = float.MinValue;
				float num3 = float.MaxValue;
				float num4 = float.MinValue;
				float num5 = this._useThinButtons ? this._thinButtonHeight : this._buttonHeight;
				foreach (KeyValuePair<int, Vector3> keyValuePair in this._directPositions)
				{
					Vector3 value = keyValuePair.Value;
					num = Mathf.Min(num, value.x);
					num2 = Mathf.Max(num2, value.x + this._buttonWidth);
					num3 = Mathf.Min(num3, value.y - num5);
					num4 = Mathf.Max(num4, value.y);
				}
				num -= this._leftSafetyMargin;
				num2 += this._rightSafetyMargin;
				num3 -= this._bottomSafetyMargin;
				num4 += this._topSafetyMargin;
				float num6 = num2 - num;
				float num7 = Mathf.Abs(num4 - num3);
				num6 = Mathf.Max(num6, this._viewportWidth);
				num7 = Mathf.Max(num7, this._viewportHeight);
				RectTransform rectTransform = this._container as RectTransform;
				bool flag2 = rectTransform != null;
				if (flag2)
				{
					rectTransform.sizeDelta = new Vector2(num6, num7);
					LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
				}
			}
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00011230 File Offset: 0x0000F430
		private void UpdateExistingButtonsThinMode()
		{
			foreach (UiElement uiElement in this._buttons)
			{
				ReMenuButton reMenuButton = uiElement as ReMenuButton;
				bool flag = reMenuButton != null;
				if (flag)
				{
					reMenuButton.ThinMode = this._useThinButtons;
				}
				else
				{
					ReMenuToggle reMenuToggle = uiElement as ReMenuToggle;
					bool flag2 = reMenuToggle != null;
					if (flag2)
					{
						reMenuToggle.ThinMode = this._useThinButtons;
					}
				}
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000112C4 File Offset: 0x0000F4C4
		public ReMenuButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, string color = "#ffffff")
		{
			ReMenuButton reMenuButton = new ReMenuButton(text, tooltip, onClick, this._container, sprite, true, color, this._useThinButtons);
			this.EnsureButtonIgnoresLayout(reMenuButton);
			this._buttons.Add(reMenuButton);
			this.UpdateLayout();
			return reMenuButton;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x00011314 File Offset: 0x0000F514
		public ReMenuButton AddSpacer(Sprite sprite = null)
		{
			ReMenuButton reMenuButton = this.AddButton(string.Empty, string.Empty, null, sprite, "#ffffff");
			reMenuButton.GameObject.name = "Button_Spacer";
			reMenuButton.Background.gameObject.SetActive(false);
			return reMenuButton;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x00011364 File Offset: 0x0000F564
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
		{
			return this.AddToggle(text, tooltip, onToggle, defaultValue, null, null, color);
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00011388 File Offset: 0x0000F588
		public ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, string color = "#ffffff")
		{
			return this.AddToggle(text, tooltip, configValue, null, null, color);
		}

		// Token: 0x06000330 RID: 816 RVA: 0x000113A8 File Offset: 0x0000F5A8
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff")
		{
			ReMenuToggle reMenuToggle = new ReMenuToggle(text, tooltip, onToggle, this._container, defaultValue, iconOn, iconOff, color, this._useThinButtons, "ENABLED", "DISABLED");
			this.EnsureButtonIgnoresLayout(reMenuToggle);
			this._buttons.Add(reMenuToggle);
			this.UpdateLayout();
			return reMenuToggle;
		}

		// Token: 0x06000331 RID: 817 RVA: 0x00011400 File Offset: 0x0000F600
		public ReMenuToggle AddToggle(string text, string tooltip, ConfigValue<bool> configValue, Sprite iconOn, Sprite iconOff, string color = "#ffffff")
		{
			ReMenuToggle reMenuToggle = new ReMenuToggle(text, tooltip, new Action<bool>(configValue.SetValue), this._container, configValue, iconOn, iconOff, color, this._useThinButtons, "ENABLED", "DISABLED");
			this.EnsureButtonIgnoresLayout(reMenuToggle);
			this._buttons.Add(reMenuToggle);
			this.UpdateLayout();
			return reMenuToggle;
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00011468 File Offset: 0x0000F668
		public ReMenuPage AddMenuPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReMenuPage menuPage = this.GetMenuPage(text);
			bool flag = menuPage != null;
			ReMenuPage result;
			if (flag)
			{
				result = menuPage;
			}
			else
			{
				ReMenuPage reMenuPage = new ReMenuPage(text, false, color);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip, new Action(reMenuPage.Open), sprite, color);
				result = reMenuPage;
			}
			return result;
		}

		// Token: 0x06000333 RID: 819 RVA: 0x000114CC File Offset: 0x0000F6CC
		public ReCategoryPage AddCategoryPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReCategoryPage categoryPage = this.GetCategoryPage(text);
			bool flag = categoryPage != null;
			ReCategoryPage result;
			if (flag)
			{
				result = categoryPage;
			}
			else
			{
				ReCategoryPage reCategoryPage = new ReCategoryPage(text, false, color);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip, new Action(reCategoryPage.Open), sprite, color);
				result = reCategoryPage;
			}
			return result;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0001152F File Offset: 0x0000F72F
		public void AddMenuPage(string text, string tooltip, Action<ReMenuPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddMenuPage(text, tooltip, sprite, color));
		}

		// Token: 0x06000335 RID: 821 RVA: 0x00011545 File Offset: 0x0000F745
		public void AddCategoryPage(string text, string tooltip, Action<ReCategoryPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddCategoryPage(text, tooltip, sprite, color));
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0001155B File Offset: 0x0000F75B
		public void AddTabbedPage(string text, string tooltip, Action<ReTabbedPage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddTabbedPage(text, tooltip, sprite, color));
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00011574 File Offset: 0x0000F774
		public ReTabbedPage AddTabbedPage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReTabbedPage tabbedPage = this.GetTabbedPage(text);
			bool flag = tabbedPage != null;
			ReTabbedPage result;
			if (flag)
			{
				result = tabbedPage;
			}
			else
			{
				ReTabbedPage reTabbedPage = new ReTabbedPage(text, false, color);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " menu") : tooltip, new Action(reTabbedPage.Open), sprite, color);
				result = reTabbedPage;
			}
			return result;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000115D8 File Offset: 0x0000F7D8
		public ReRadioTogglePage AddRadioTogglePage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			ReRadioTogglePage radioTogglePage = this.GetRadioTogglePage(text);
			bool flag = radioTogglePage != null;
			ReRadioTogglePage result;
			if (flag)
			{
				result = radioTogglePage;
			}
			else
			{
				ReRadioTogglePage reRadioTogglePage = new ReRadioTogglePage(text);
				this.AddButton(text, string.IsNullOrEmpty(tooltip) ? ("Open the " + text + " radio selection menu") : tooltip, delegate
				{
					reRadioTogglePage.Open(null);
				}, sprite, color);
				result = reRadioTogglePage;
			}
			return result;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00011648 File Offset: 0x0000F848
		public ReRadioTogglePage GetRadioTogglePage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReRadioTogglePage(name);
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00011687 File Offset: 0x0000F887
		public void AddRadioTogglePage(string text, string tooltip, Action<ReRadioTogglePage> onPageBuilt, Sprite sprite = null, string color = "#ffffff")
		{
			onPageBuilt(this.AddRadioTogglePage(text, tooltip, sprite, color));
		}

		// Token: 0x0600033B RID: 827 RVA: 0x000116A0 File Offset: 0x0000F8A0
		public ReRadioToggleGroup AddRadioToggleGroup(string groupName, Action<object> onSelectionChanged, string color = "#ffffff")
		{
			return new ReRadioToggleGroup(groupName, this._container, onSelectionChanged, color);
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000116C0 File Offset: 0x0000F8C0
		public ReRadioTogglePage AddReRadioTogglePage(string text, string tooltip = "", Sprite sprite = null, string color = "#ffffff")
		{
			return this.AddRadioTogglePage(text, tooltip, sprite, color);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000116E0 File Offset: 0x0000F8E0
		public ReMenuPage GetMenuPage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReMenuPage(transform);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x00011720 File Offset: 0x0000F920
		public ReCategoryPage GetCategoryPage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReCategoryPage(transform);
		}

		// Token: 0x0600033F RID: 831 RVA: 0x00011760 File Offset: 0x0000F960
		public ReTabbedPage GetTabbedPage(string name)
		{
			Transform transform = MenuEx.QMenuParent.Find(UiElement.GetCleanName("Menu_" + name));
			return (transform == null) ? null : new ReTabbedPage(transform);
		}

		// Token: 0x06000340 RID: 832 RVA: 0x000117A0 File Offset: 0x0000F9A0
		public ReMenuPage ToMenuPage(string name, string tooltip = "", Sprite sprite = null)
		{
			ReMenuPage menuPage = this.GetMenuPage(name);
			this.AddButton(name, string.IsNullOrEmpty(tooltip) ? ("Open the " + name + " menu") : tooltip, new Action(menuPage.Open), sprite, "#ffffff");
			return menuPage;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x000117F0 File Offset: 0x0000F9F0
		public ReCategoryPage ToCategoryPage(string name, string tooltip = "", Sprite sprite = null)
		{
			ReCategoryPage categoryPage = this.GetCategoryPage(name);
			this.AddButton(name, string.IsNullOrEmpty(tooltip) ? ("Open the " + name + " menu") : tooltip, new Action(categoryPage.Open), sprite, "#ffffff");
			return categoryPage;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00011840 File Offset: 0x0000FA40
		public void SetButtonLayout(ButtonLayoutShape layoutShape)
		{
			this.LayoutShape = layoutShape;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0001184B File Offset: 0x0000FA4B
		public void SetThinButtons(bool useThin)
		{
			this.UseThinButtons = useThin;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x00011856 File Offset: 0x0000FA56
		public void SetSafetyMargins(float top, float right, float bottom, float left)
		{
			this._topSafetyMargin = top;
			this._rightSafetyMargin = right;
			this._bottomSafetyMargin = bottom;
			this._leftSafetyMargin = left;
			this.UpdateLayout();
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0001187D File Offset: 0x0000FA7D
		public void SetHorizontalOffset(float offset)
		{
			this._horizontalOffset = offset;
			this.UpdateLayout();
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0001188E File Offset: 0x0000FA8E
		public void SetVerticalOffset(float offset)
		{
			this._verticalOffset = offset;
			this.UpdateLayout();
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0001189F File Offset: 0x0000FA9F
		public void SetVerticalSpacing(float spacing)
		{
			this._verticalSpacing = spacing;
			this.UpdateLayout();
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000118B0 File Offset: 0x0000FAB0
		public void SetHorizontalSpacing(float spacing)
		{
			this._horizontalSpacing = spacing;
			this.UpdateLayout();
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000118C4 File Offset: 0x0000FAC4
		public void SetButtonPosition(int buttonIndex, Vector3 position)
		{
			bool flag = buttonIndex < 0 || buttonIndex >= this._buttons.Count;
			if (!flag)
			{
				this._directPositions[buttonIndex] = position;
				UiElement uiElement = this._buttons[buttonIndex];
				bool flag2 = ((uiElement != null) ? uiElement.RectTransform : null) != null;
				if (flag2)
				{
					this._buttons[buttonIndex].RectTransform.localPosition = position;
					this.EnsureButtonIgnoresLayout(this._buttons[buttonIndex]);
				}
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00011950 File Offset: 0x0000FB50
		public void SetButtonPosition(UiElement button, Vector3 position)
		{
			int num = this._buttons.IndexOf(button);
			bool flag = num >= 0;
			if (flag)
			{
				this.SetButtonPosition(num, position);
			}
			else
			{
				bool flag2 = ((button != null) ? button.RectTransform : null) != null;
				if (flag2)
				{
					button.RectTransform.localPosition = position;
					this.EnsureButtonIgnoresLayout(button);
				}
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000119B0 File Offset: 0x0000FBB0
		public void ClearButtons()
		{
			foreach (UiElement uiElement in this._buttons)
			{
				bool flag = ((uiElement != null) ? uiElement.GameObject : null) != null;
				if (flag)
				{
					Object.Destroy(uiElement.GameObject);
				}
			}
			this._buttons.Clear();
			this._directPositions.Clear();
			RectTransform rectTransform = this._container as RectTransform;
			bool flag2 = rectTransform != null;
			if (flag2)
			{
				rectTransform.sizeDelta = new Vector2(this._viewportWidth, this._viewportHeight);
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00011A70 File Offset: 0x0000FC70
		public void RefreshLayout()
		{
			this.UpdateLayout();
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00011A7C File Offset: 0x0000FC7C
		public UiElement GetButton(int index)
		{
			bool flag = index >= 0 && index < this._buttons.Count;
			UiElement result;
			if (flag)
			{
				result = this._buttons[index];
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00011AB8 File Offset: 0x0000FCB8
		public UiElement GetButtonByName(string name)
		{
			return Enumerable.FirstOrDefault<UiElement>(this._buttons, delegate(UiElement button)
			{
				string a;
				if (button == null)
				{
					a = null;
				}
				else
				{
					GameObject gameObject = button.GameObject;
					a = ((gameObject != null) ? gameObject.name : null);
				}
				bool result;
				if (!(a == name))
				{
					string a2;
					if (button == null)
					{
						a2 = null;
					}
					else
					{
						GameObject gameObject2 = button.GameObject;
						a2 = ((gameObject2 != null) ? gameObject2.name : null);
					}
					result = (a2 == "Button_" + name);
				}
				else
				{
					result = true;
				}
				return result;
			});
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600034F RID: 847 RVA: 0x00011AEE File Offset: 0x0000FCEE
		public int ButtonCount
		{
			get
			{
				return this._buttons.Count;
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00011AFC File Offset: 0x0000FCFC
		public void SetButtonWidth(float width)
		{
			bool flag = width <= 0f;
			if (!flag)
			{
				this._buttonWidth = width;
				this.UpdateLayout();
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00011B2C File Offset: 0x0000FD2C
		public void SetButtonHeight(float normalHeight, float thinHeight)
		{
			bool flag = normalHeight <= 0f || thinHeight <= 0f;
			if (!flag)
			{
				this._buttonHeight = normalHeight;
				this._thinButtonHeight = thinHeight;
				this.UpdateLayout();
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00011B6C File Offset: 0x0000FD6C
		public static ReMenuPage Create(string text, bool isRoot, string color = "#ffffff")
		{
			return new ReMenuPage(text, isRoot, color);
		}

		// Token: 0x04000142 RID: 322
		private readonly bool _isRoot;

		// Token: 0x04000143 RID: 323
		private Transform _container;

		// Token: 0x04000144 RID: 324
		private ButtonLayoutShape _currentLayoutShape = ButtonLayoutShape.Grid;

		// Token: 0x04000145 RID: 325
		private bool _useThinButtons = false;

		// Token: 0x04000149 RID: 329
		private readonly List<UiElement> _buttons = new List<UiElement>();

		// Token: 0x0400014A RID: 330
		private readonly Dictionary<int, Vector3> _directPositions = new Dictionary<int, Vector3>();

		// Token: 0x0400014B RID: 331
		private float _viewportWidth = 900f;

		// Token: 0x0400014C RID: 332
		private float _viewportHeight = 600f;

		// Token: 0x0400014D RID: 333
		private float _buttonWidth = 210f;

		// Token: 0x0400014E RID: 334
		private float _buttonHeight = 90f;

		// Token: 0x0400014F RID: 335
		private float _thinButtonHeight = 70f;

		// Token: 0x04000150 RID: 336
		public float _horizontalOffset = 100f;

		// Token: 0x04000151 RID: 337
		public float _verticalOffset = -30f;

		// Token: 0x04000152 RID: 338
		public float _verticalSpacing = 180f;

		// Token: 0x04000153 RID: 339
		public float _horizontalSpacing = 20f;

		// Token: 0x04000154 RID: 340
		public float _topSafetyMargin = 100f;

		// Token: 0x04000155 RID: 341
		public float _bottomSafetyMargin = 90f;

		// Token: 0x04000156 RID: 342
		public float _leftSafetyMargin = 80f;

		// Token: 0x04000157 RID: 343
		public float _rightSafetyMargin = 80f;
	}
}
