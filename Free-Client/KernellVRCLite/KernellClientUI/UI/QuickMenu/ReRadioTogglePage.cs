using System;
using System.Collections.Generic;
using System.Diagnostics;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000054 RID: 84
	public class ReRadioTogglePage : UiElement
	{
		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x000143C8 File Offset: 0x000125C8
		private static GameObject MenuPrefab
		{
			get
			{
				bool flag = ReRadioTogglePage._menuPrefab == null;
				if (flag)
				{
					try
					{
						Transform transform = MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_ChangeAudioInputDevice");
						bool flag2 = transform != null;
						if (flag2)
						{
							ReRadioTogglePage._menuPrefab = transform.gameObject;
							Debug.Log("[ReRadioTogglePage] Found audio device menu prefab");
						}
						else
						{
							Transform transform2 = MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard");
							bool flag3 = transform2 != null;
							if (flag3)
							{
								ReRadioTogglePage._menuPrefab = transform2.gameObject;
								Debug.Log("[ReRadioTogglePage] Using dashboard menu as prefab");
							}
							else
							{
								Debug.LogError("[ReRadioTogglePage] Failed to find menu prefab, creating fallback");
								ReRadioTogglePage._menuPrefab = ReRadioTogglePage.CreateFallbackMenu();
							}
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("[ReRadioTogglePage] Error finding menu prefab: " + ex.Message);
						ReRadioTogglePage._menuPrefab = ReRadioTogglePage.CreateFallbackMenu();
					}
				}
				return ReRadioTogglePage._menuPrefab;
			}
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000144D4 File Offset: 0x000126D4
		private static GameObject CreateFallbackMenu()
		{
			GameObject gameObject = new GameObject("FallbackRadioMenu");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(1000f, 700f);
			UIPage uipage = gameObject.AddComponent<UIPage>();
			GameObject gameObject2 = new GameObject("Header");
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
			rectTransform2.anchorMin = new Vector2(0f, 1f);
			rectTransform2.anchorMax = new Vector2(1f, 1f);
			rectTransform2.pivot = new Vector2(0.5f, 1f);
			rectTransform2.sizeDelta = new Vector2(0f, 80f);
			GameObject gameObject3 = new GameObject("LeftItemContainer");
			gameObject3.transform.SetParent(gameObject2.transform, false);
			RectTransform rectTransform3 = gameObject3.AddComponent<RectTransform>();
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(0f, 1f);
			rectTransform3.pivot = new Vector2(0f, 0.5f);
			rectTransform3.sizeDelta = new Vector2(100f, 0f);
			GameObject gameObject4 = new GameObject("Button_Back");
			gameObject4.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform4 = gameObject4.AddComponent<RectTransform>();
			rectTransform4.anchorMin = new Vector2(0f, 0.5f);
			rectTransform4.anchorMax = new Vector2(0f, 0.5f);
			rectTransform4.pivot = new Vector2(0.5f, 0.5f);
			rectTransform4.sizeDelta = new Vector2(50f, 50f);
			rectTransform4.anchoredPosition = new Vector2(40f, 0f);
			Image image = gameObject4.AddComponent<Image>();
			image.color = new Color(0.3f, 0.3f, 0.8f);
			Button button = gameObject4.AddComponent<Button>();
			GameObject gameObject5 = new GameObject("Text_Title");
			gameObject5.transform.SetParent(gameObject2.transform, false);
			RectTransform rectTransform5 = gameObject5.AddComponent<RectTransform>();
			rectTransform5.anchorMin = new Vector2(0f, 0f);
			rectTransform5.anchorMax = new Vector2(1f, 1f);
			rectTransform5.offsetMin = new Vector2(100f, 0f);
			rectTransform5.offsetMax = new Vector2(-100f, 0f);
			TextMeshProUGUIEx textMeshProUGUIEx = gameObject5.AddComponent<TextMeshProUGUIEx>();
			textMeshProUGUIEx.text = "Radio Toggle Page";
			textMeshProUGUIEx.fontSize = 32f;
			textMeshProUGUIEx.alignment = 514;
			textMeshProUGUIEx.verticalAlignment = 512;
			textMeshProUGUIEx.color = Color.white;
			GameObject gameObject6 = new GameObject("ScrollRect");
			gameObject6.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform6 = gameObject6.AddComponent<RectTransform>();
			rectTransform6.anchorMin = new Vector2(0f, 0f);
			rectTransform6.anchorMax = new Vector2(1f, 1f);
			rectTransform6.offsetMin = new Vector2(0f, 0f);
			rectTransform6.offsetMax = new Vector2(0f, -80f);
			ScrollRect scrollRect = gameObject6.AddComponent<ScrollRect>();
			GameObject gameObject7 = new GameObject("Viewport");
			gameObject7.transform.SetParent(gameObject6.transform, false);
			RectTransform rectTransform7 = gameObject7.AddComponent<RectTransform>();
			rectTransform7.anchorMin = Vector2.zero;
			rectTransform7.anchorMax = Vector2.one;
			rectTransform7.offsetMin = Vector2.zero;
			rectTransform7.offsetMax = Vector2.zero;
			Image image2 = gameObject7.AddComponent<Image>();
			image2.color = new Color(0f, 0f, 0f, 0f);
			Mask mask = gameObject7.AddComponent<Mask>();
			mask.showMaskGraphic = false;
			GameObject gameObject8 = new GameObject("VerticalLayoutGroup");
			gameObject8.transform.SetParent(gameObject7.transform, false);
			RectTransform rectTransform8 = gameObject8.AddComponent<RectTransform>();
			rectTransform8.anchorMin = new Vector2(0f, 1f);
			rectTransform8.anchorMax = new Vector2(1f, 1f);
			rectTransform8.pivot = new Vector2(0.5f, 1f);
			rectTransform8.sizeDelta = new Vector2(0f, 1000f);
			VerticalLayoutGroup verticalLayoutGroup = gameObject8.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup.childAlignment = 1;
			verticalLayoutGroup.spacing = 10f;
			verticalLayoutGroup.padding = new RectOffset(20, 20, 20, 20);
			verticalLayoutGroup.childControlHeight = false;
			verticalLayoutGroup.childForceExpandHeight = false;
			ContentSizeFitter contentSizeFitter = gameObject8.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = 2;
			scrollRect.content = rectTransform8;
			scrollRect.viewport = rectTransform7;
			scrollRect.vertical = true;
			scrollRect.horizontal = false;
			GameObject gameObject9 = new GameObject("ToggleGroup");
			gameObject9.transform.SetParent(gameObject8.transform, false);
			RectTransform rectTransform9 = gameObject9.AddComponent<RectTransform>();
			rectTransform9.anchorMin = new Vector2(0f, 1f);
			rectTransform9.anchorMax = new Vector2(1f, 1f);
			rectTransform9.pivot = new Vector2(0.5f, 1f);
			rectTransform9.sizeDelta = new Vector2(0f, 500f);
			VerticalLayoutGroup verticalLayoutGroup2 = gameObject9.AddComponent<VerticalLayoutGroup>();
			verticalLayoutGroup2.childAlignment = 1;
			verticalLayoutGroup2.spacing = 10f;
			verticalLayoutGroup2.childControlHeight = false;
			verticalLayoutGroup2.childForceExpandHeight = false;
			ContentSizeFitter contentSizeFitter2 = gameObject9.AddComponent<ContentSizeFitter>();
			contentSizeFitter2.verticalFit = 2;
			return gameObject;
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003AB RID: 939 RVA: 0x00014AAE File Offset: 0x00012CAE
		private static int SiblingIndex
		{
			get
			{
				return MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Modal_AddMessage").GetSiblingIndex();
			}
		}

		// Token: 0x170000C2 RID: 194
		// (set) Token: 0x060003AC RID: 940 RVA: 0x00014ACC File Offset: 0x00012CCC
		public string TitleText
		{
			set
			{
				bool flag = this._titleText != null;
				if (flag)
				{
					this._titleText.text = value;
				}
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x060003AD RID: 941 RVA: 0x00014AF9 File Offset: 0x00012CF9
		// (set) Token: 0x060003AE RID: 942 RVA: 0x00014B01 File Offset: 0x00012D01
		public UIPage UiPage { get; private set; }

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060003AF RID: 943 RVA: 0x00014B0C File Offset: 0x00012D0C
		// (remove) Token: 0x060003B0 RID: 944 RVA: 0x00014B44 File Offset: 0x00012D44
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnOpen;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060003B1 RID: 945 RVA: 0x00014B7C File Offset: 0x00012D7C
		// (remove) Token: 0x060003B2 RID: 946 RVA: 0x00014BB4 File Offset: 0x00012DB4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnClose;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x060003B3 RID: 947 RVA: 0x00014BEC File Offset: 0x00012DEC
		// (remove) Token: 0x060003B4 RID: 948 RVA: 0x00014C24 File Offset: 0x00012E24
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<object> OnSelect;

		// Token: 0x060003B5 RID: 949 RVA: 0x00014C5C File Offset: 0x00012E5C
		public ReRadioTogglePage(string name) : base(ReRadioTogglePage.MenuPrefab, MenuEx.QMenuParent, "Menu_" + name, false)
		{
			try
			{
				Transform transform = base.RectTransform.Find("Header_H1") ?? base.RectTransform.GetChild(0);
				bool flag = transform != null;
				if (flag)
				{
					this._titleText = transform.GetComponentInChildren<TextMeshProUGUIEx>();
					bool flag2 = this._titleText != null;
					if (flag2)
					{
						this._titleText.text = name;
						this._titleText.richText = true;
					}
					Transform transform2 = transform.Find("LeftItemContainer/Button_Back");
					bool flag3 = transform2 != null;
					if (flag3)
					{
						transform2.gameObject.SetActive(true);
					}
				}
				ScrollRect scrollRect = base.GameObject.GetComponentInChildren<ScrollRect>();
				bool flag4 = scrollRect == null;
				if (flag4)
				{
					GameObject gameObject = new GameObject("ScrollRect");
					gameObject.transform.SetParent(base.GameObject.transform, false);
					RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
					rectTransform.anchorMin = new Vector2(0f, 0f);
					rectTransform.anchorMax = new Vector2(1f, 1f);
					rectTransform.offsetMin = new Vector2(0f, 0f);
					rectTransform.offsetMax = new Vector2(0f, -80f);
					scrollRect = gameObject.AddComponent<ScrollRect>();
					GameObject gameObject2 = new GameObject("Viewport");
					gameObject2.transform.SetParent(gameObject.transform, false);
					RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
					rectTransform2.anchorMin = Vector2.zero;
					rectTransform2.anchorMax = Vector2.one;
					rectTransform2.offsetMin = Vector2.zero;
					rectTransform2.offsetMax = Vector2.zero;
					Image image = gameObject2.AddComponent<Image>();
					image.color = new Color(0f, 0f, 0f, 0f);
					Mask mask = gameObject2.AddComponent<Mask>();
					mask.showMaskGraphic = false;
					GameObject gameObject3 = new GameObject("VerticalLayoutGroup");
					gameObject3.transform.SetParent(gameObject2.transform, false);
					RectTransform rectTransform3 = gameObject3.AddComponent<RectTransform>();
					rectTransform3.anchorMin = new Vector2(0f, 1f);
					rectTransform3.anchorMax = new Vector2(1f, 1f);
					rectTransform3.pivot = new Vector2(0.5f, 1f);
					rectTransform3.sizeDelta = new Vector2(0f, 1000f);
					scrollRect.content = rectTransform3;
					scrollRect.viewport = rectTransform2;
					scrollRect.vertical = true;
					scrollRect.horizontal = false;
				}
				Transform transform3 = scrollRect.content;
				bool flag5 = transform3 == null;
				if (flag5)
				{
					Debug.LogError("[ReRadioTogglePage] Content transform is null, creating one");
					GameObject gameObject4 = new GameObject("Content");
					gameObject4.transform.SetParent(scrollRect.transform.Find("Viewport") ?? scrollRect.transform, false);
					RectTransform rectTransform4 = gameObject4.AddComponent<RectTransform>();
					rectTransform4.anchorMin = new Vector2(0f, 1f);
					rectTransform4.anchorMax = new Vector2(1f, 1f);
					rectTransform4.pivot = new Vector2(0.5f, 1f);
					rectTransform4.sizeDelta = new Vector2(0f, 1000f);
					transform3 = gameObject4.transform;
					scrollRect.content = rectTransform4;
				}
				this._verticalLayoutGroup = transform3.GetComponent<VerticalLayoutGroup>();
				bool flag6 = this._verticalLayoutGroup == null;
				if (flag6)
				{
					this._verticalLayoutGroup = transform3.gameObject.AddComponent<VerticalLayoutGroup>();
					this._verticalLayoutGroup.childAlignment = 1;
					this._verticalLayoutGroup.spacing = 10f;
					this._verticalLayoutGroup.padding = new RectOffset(20, 20, 20, 20);
					this._verticalLayoutGroup.childControlHeight = false;
					this._verticalLayoutGroup.childForceExpandHeight = false;
					ContentSizeFitter contentSizeFitter = transform3.gameObject.AddComponent<ContentSizeFitter>();
					contentSizeFitter.verticalFit = 2;
				}
				this._toggleGroupRoot = new GameObject("ToggleGroup");
				this._toggleGroupRoot.transform.SetParent(transform3, false);
				RectTransform rectTransform5 = this._toggleGroupRoot.GetComponent<RectTransform>();
				bool flag7 = rectTransform5 == null;
				if (flag7)
				{
					rectTransform5 = this._toggleGroupRoot.AddComponent<RectTransform>();
				}
				rectTransform5.anchorMin = new Vector2(0f, 1f);
				rectTransform5.anchorMax = new Vector2(1f, 1f);
				rectTransform5.pivot = new Vector2(0.5f, 1f);
				rectTransform5.sizeDelta = new Vector2(0f, 500f);
				VerticalLayoutGroup verticalLayoutGroup = this._toggleGroupRoot.GetComponent<VerticalLayoutGroup>();
				bool flag8 = verticalLayoutGroup == null;
				if (flag8)
				{
					verticalLayoutGroup = this._toggleGroupRoot.AddComponent<VerticalLayoutGroup>();
					verticalLayoutGroup.childAlignment = 1;
					verticalLayoutGroup.spacing = 10f;
					verticalLayoutGroup.childControlHeight = false;
					verticalLayoutGroup.childForceExpandHeight = false;
					ContentSizeFitter contentSizeFitter2 = this._toggleGroupRoot.AddComponent<ContentSizeFitter>();
					contentSizeFitter2.verticalFit = 2;
				}
				this._container = this._toggleGroupRoot.transform;
				this.UiPage = base.GameObject.GetComponent<UIPage>();
				bool flag9 = this.UiPage == null;
				if (flag9)
				{
					this.UiPage = base.GameObject.AddComponent<UIPage>();
				}
				this.UiPage.field_Public_String_0 = "QuickMenuReMod" + UiElement.GetCleanName(name);
				this.UiPage.field_Private_List_1_UIPage_0 = new List<UIPage>();
				this.UiPage.field_Private_List_1_UIPage_0.Add(this.UiPage);
				try
				{
					bool flag10 = !MenuEx.QMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.ContainsKey(this.UiPage.field_Public_String_0);
					if (flag10)
					{
						MenuEx.QMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(this.UiPage.field_Public_String_0, this.UiPage);
					}
				}
				catch (Exception ex)
				{
					Debug.LogError("[ReRadioTogglePage] Error registering with menu controller: " + ex.Message);
				}
				EnableDisableListener enableDisableListener = base.GameObject.AddComponent<EnableDisableListener>();
				enableDisableListener.OnEnableEvent += delegate()
				{
					Action onOpen = this.OnOpen;
					if (onOpen != null)
					{
						onOpen();
					}
				};
				enableDisableListener.OnDisableEvent += delegate()
				{
					Action onClose = this.OnClose;
					if (onClose != null)
					{
						onClose();
					}
				};
				Debug.Log("[ReRadioTogglePage] Successfully created radio toggle page: " + name);
			}
			catch (Exception ex2)
			{
				Debug.LogError("[ReRadioTogglePage] Error in constructor: " + ex2.Message + "\n" + ex2.StackTrace);
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001534C File Offset: 0x0001354C
		public void Open(object selected = null)
		{
			try
			{
				bool flag = this.UiPage != null && MenuEx.QMenuStateCtrl != null;
				if (flag)
				{
					this.UiPage.gameObject.SetActive(true);
					MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(this.UiPage.field_Public_String_0, null, false, 0);
				}
				this.UpdateToggleElements();
				bool flag2 = selected != null;
				if (flag2)
				{
					foreach (ReRadioToggle reRadioToggle in this._radioElements)
					{
						bool flag3 = reRadioToggle != null;
						if (flag3)
						{
							reRadioToggle.SetToggle(reRadioToggle.ToggleData.Equals(selected));
						}
					}
				}
				Debug.Log(string.Format("[ReRadioTogglePage] Opened page with {0} toggles", this._radioElements.Count));
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioTogglePage] Error in Open: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00015480 File Offset: 0x00013680
		private void UpdateToggleElements()
		{
			bool isUpdated = this._isUpdated;
			if (isUpdated)
			{
				try
				{
					this._isUpdated = false;
					foreach (ReRadioToggle reRadioToggle in this._radioElements)
					{
						bool flag = ((reRadioToggle != null) ? reRadioToggle.GameObject : null) != null;
						if (flag)
						{
							Object.DestroyImmediate(reRadioToggle.GameObject);
						}
					}
					this._radioElements.Clear();
					foreach (Tuple<string, object> tuple in this._radioElementSource)
					{
						bool flag2 = this._toggleGroupRoot != null;
						if (flag2)
						{
							try
							{
								ReRadioToggle reRadioToggle2 = new ReRadioToggle(this._toggleGroupRoot.transform, tuple.Item1, tuple.Item1, tuple.Item2, false);
								reRadioToggle2.ToggleStateUpdated = (Action<ReRadioToggle, bool>)Delegate.Combine(reRadioToggle2.ToggleStateUpdated, new Action<ReRadioToggle, bool>(this.OnToggleSelect));
								this._radioElements.Add(reRadioToggle2);
								Debug.Log("[ReRadioTogglePage] Added toggle: " + tuple.Item1);
							}
							catch (Exception ex)
							{
								Debug.LogError("[ReRadioTogglePage] Error creating toggle for " + tuple.Item1 + ": " + ex.Message);
							}
						}
					}
					bool flag3 = this._verticalLayoutGroup != null;
					if (flag3)
					{
						Canvas.ForceUpdateCanvases();
						LayoutRebuilder.ForceRebuildLayoutImmediate(this._verticalLayoutGroup.GetComponent<RectTransform>());
					}
				}
				catch (Exception ex2)
				{
					Debug.LogError("[ReRadioTogglePage] Error updating toggle elements: " + ex2.Message);
				}
			}
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x000156B8 File Offset: 0x000138B8
		private void OnToggleSelect(ReRadioToggle toggle, bool state)
		{
			try
			{
				if (state)
				{
					foreach (ReRadioToggle reRadioToggle in this._radioElements)
					{
						bool flag = reRadioToggle != null && reRadioToggle != toggle;
						if (flag)
						{
							reRadioToggle.SetToggle(false);
						}
					}
					Action<object> onSelect = this.OnSelect;
					if (onSelect != null)
					{
						onSelect(toggle.ToggleData);
					}
					Debug.Log(string.Format("[ReRadioTogglePage] Selected: {0}", toggle.ToggleData));
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioTogglePage] Error in OnToggleSelect: " + ex.Message);
			}
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00015794 File Offset: 0x00013994
		public void AddItem(string name, object obj)
		{
			this._radioElementSource.Add(new Tuple<string, object>(name, obj));
			this._isUpdated = true;
			Debug.Log("[ReRadioTogglePage] Added item: " + name);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x000157C7 File Offset: 0x000139C7
		public void ClearItems()
		{
			this._radioElementSource.Clear();
			this._isUpdated = true;
			Debug.Log("[ReRadioTogglePage] Cleared all items");
		}

		// Token: 0x04000181 RID: 385
		private static GameObject _menuPrefab;

		// Token: 0x04000182 RID: 386
		private TextMeshProUGUIEx _titleText;

		// Token: 0x04000183 RID: 387
		private GameObject _toggleGroupRoot;

		// Token: 0x04000184 RID: 388
		private List<Tuple<string, object>> _radioElementSource = new List<Tuple<string, object>>();

		// Token: 0x04000185 RID: 389
		private List<ReRadioToggle> _radioElements = new List<ReRadioToggle>();

		// Token: 0x04000186 RID: 390
		private bool _isUpdated = true;

		// Token: 0x04000187 RID: 391
		private readonly Transform _container;

		// Token: 0x04000188 RID: 392
		private VerticalLayoutGroup _verticalLayoutGroup;
	}
}
