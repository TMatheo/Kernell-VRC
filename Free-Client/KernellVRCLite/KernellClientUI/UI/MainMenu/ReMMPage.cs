using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.UI.MainMenu.Header;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;
using VRC.UI.Pages.MM;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x0200005F RID: 95
	public class ReMMPage : UiElement
	{
		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x000179EC File Offset: 0x00015BEC
		public UIPage UiPage { get; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000425 RID: 1061 RVA: 0x000179F4 File Offset: 0x00015BF4
		// (set) Token: 0x06000426 RID: 1062 RVA: 0x000179FC File Offset: 0x00015BFC
		public GameObject MenuObject { get; private set; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00017A05 File Offset: 0x00015C05
		// (set) Token: 0x06000428 RID: 1064 RVA: 0x00017A0D File Offset: 0x00015C0D
		internal TextMeshProUGUIEx MenuTitleText { get; private set; }

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06000429 RID: 1065 RVA: 0x00017A18 File Offset: 0x00015C18
		// (remove) Token: 0x0600042A RID: 1066 RVA: 0x00017A50 File Offset: 0x00015C50
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnOpen;

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x0600042B RID: 1067 RVA: 0x00017A88 File Offset: 0x00015C88
		// (remove) Token: 0x0600042C RID: 1068 RVA: 0x00017AC0 File Offset: 0x00015CC0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnClose;

		// Token: 0x0600042D RID: 1069 RVA: 0x00017AF8 File Offset: 0x00015CF8
		internal ReMMPage(string text, Sprite icon, bool isRoot = false, string color = "#ffffff") : base(null, null, "Menu_" + text, false)
		{
			try
			{
				bool flag = MenuEx.MMSettingsMenu == null;
				if (flag)
				{
					Debug.LogError("[KernellClient] MenuEx.MMSettingsMenu is null, cannot create page");
				}
				else
				{
					bool flag2 = MenuEx.QMenuParent == null;
					if (flag2)
					{
						Debug.LogError("[KernellClient] MenuEx.QMenuParent is null, cannot create page");
					}
					else
					{
						bool flag3 = MenuEx.MMenuStateCtrl == null;
						if (flag3)
						{
							Debug.LogError("[KernellClient] MenuEx.MMenuStateCtrl is null, cannot create page");
						}
						else
						{
							base.GameObject = new GameObject("Menu_" + UiElement.GetCleanName(text));
							base.GameObject.transform.SetParent(MenuEx.QMenuParent.transform, false);
							ReMMPage reMMPage = this;
							this._pageIcon = icon;
							string cleanName = UiElement.GetCleanName(text);
							this.MenuObject = Object.Instantiate<GameObject>(MenuEx.MMSettingsMenu, MenuEx.MMSettingsMenu.transform.parent);
							bool flag4 = this.MenuObject == null;
							if (flag4)
							{
								Debug.LogError("[KernellClient] Failed to instantiate MenuObject");
							}
							else
							{
								this.MenuObject.name = "Menu_" + cleanName;
								this.MenuObject.transform.SetSiblingIndex(19);
								Object.Destroy(base.GameObject);
								base.GameObject = this.MenuObject;
								SettingsPage component = this.MenuObject.GetComponent<SettingsPage>();
								bool flag5 = component != null;
								if (flag5)
								{
									Object.DestroyImmediate(component);
								}
								GameObject gameObject = this.FindComponentInObject(this.MenuObject, "Field_MM_TextSearchField");
								bool flag6 = gameObject != null;
								if (flag6)
								{
									Object.DestroyImmediate(gameObject);
								}
								this.UiPage = this.MenuObject.AddComponent<UIPage>();
								this.UiPage.field_Public_String_0 = "MainMenuReMod" + cleanName;
								this.UiPage.field_Private_List_1_UIPage_0 = new List<UIPage>();
								this.UiPage.field_Private_List_1_UIPage_0.Add(this.UiPage);
								Canvas component2 = this.UiPage.GetComponent<Canvas>();
								bool flag7 = component2 != null;
								if (flag7)
								{
									component2.enabled = true;
								}
								CanvasGroup component3 = this.UiPage.GetComponent<CanvasGroup>();
								bool flag8 = component3 != null;
								if (flag8)
								{
									component3.enabled = true;
								}
								this.UiPage.enabled = true;
								GraphicRaycaster component4 = this.UiPage.GetComponent<GraphicRaycaster>();
								bool flag9 = component4 != null;
								if (flag9)
								{
									component4.enabled = true;
								}
								this.UiPage.gameObject.SetActive(false);
								try
								{
									MenuEx.MMenuStateCtrl.field_Private_Dictionary_2_String_UIPage_0.Add(this.UiPage.field_Public_String_0, this.UiPage);
									bool flag10 = MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0 != null;
									if (flag10)
									{
										List<UIPage> list = Enumerable.ToList<UIPage>(MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0);
										list.Add(this.UiPage);
										MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0 = list.ToArray();
									}
									else
									{
										Debug.LogError("[KernellClient] MenuEx.MMenuStateCtrl.field_Public_ArrayOf_UIPage_0 is null");
									}
								}
								catch (Exception ex)
								{
									Debug.LogError("[KernellClient] Failed to register page with menu controller: " + ex.Message);
								}
								EnableDisableListener enableDisableListener = base.GameObject.AddComponent<EnableDisableListener>();
								enableDisableListener.OnEnableEvent += delegate()
								{
									bool flag13 = reMMPage.OnOpen != null;
									if (flag13)
									{
										reMMPage.OnOpen();
									}
								};
								enableDisableListener.OnDisableEvent += delegate()
								{
									bool flag13 = reMMPage.OnClose != null;
									if (flag13)
									{
										reMMPage.OnClose();
									}
								};
								this.CleanupCategoryContainers();
								this.MenuTitleText = this.FindTitleText();
								bool flag11 = this.MenuTitleText != null;
								if (flag11)
								{
									this.MenuTitleText.richText = true;
								}
								this.SetupHeaderButtons();
								GameObject gameObject2 = this.FindComponentInObject(this.MenuObject, "Text_Title");
								bool flag12 = gameObject2 != null;
								if (flag12)
								{
									gameObject2.SetActive(false);
								}
								MelonCoroutines.Start(this.SetTitleWhenActive(text, color));
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Debug.LogError("[KernellClient] Error creating ReMMPage: " + ex2.Message + "\n" + ex2.StackTrace);
			}
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00017F4C File Offset: 0x0001614C
		public ReMMPage(Transform transform) : base(transform)
		{
			try
			{
				bool flag = transform == null;
				if (flag)
				{
					Debug.LogError("[KernellClient] Cannot create ReMMPage from null transform");
				}
				else
				{
					this.UiPage = base.GameObject.GetComponent<UIPage>();
					bool flag2 = MenuEx.QMenuStateCtrl != null && MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0 != null;
					if (flag2)
					{
						this._isRoot = MenuEx.QMenuStateCtrl.field_Public_ArrayOf_UIPage_0.Contains(this.UiPage);
					}
					else
					{
						this._isRoot = false;
					}
					GameObject gameObject = this.FindComponentInObject(base.GameObject, "Scrollrect");
					bool flag3 = gameObject != null;
					if (flag3)
					{
						ScrollRect component = gameObject.GetComponent<ScrollRect>();
						bool flag4 = component != null;
						if (flag4)
						{
							this._container = component.content;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error creating ReMMPage from transform: " + ex.Message);
			}
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00018058 File Offset: 0x00016258
		private string GetFullPath(Transform transform)
		{
			bool flag = transform == null;
			string result;
			if (flag)
			{
				result = "null";
			}
			else
			{
				string text = transform.name;
				Transform parent = transform.parent;
				while (parent != null)
				{
					text = parent.name + "/" + text;
					parent = parent.parent;
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x000180B8 File Offset: 0x000162B8
		private GameObject FindComponentInObject(GameObject parent, string name)
		{
			bool flag = parent == null || string.IsNullOrEmpty(name);
			GameObject result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = parent.name == name;
				if (flag2)
				{
					result = parent;
				}
				else
				{
					Transform transform = parent.transform.Find(name);
					bool flag3 = transform != null;
					if (flag3)
					{
						result = transform.gameObject;
					}
					else
					{
						foreach (Object @object in parent.transform)
						{
							Transform transform2 = (Transform)@object;
							bool flag4 = transform2 == null;
							if (!flag4)
							{
								bool flag5 = transform2.name.Contains("QM") || transform2.name.Contains("QuickMenu");
								if (!flag5)
								{
									GameObject gameObject = this.FindComponentInObject(transform2.gameObject, name);
									bool flag6 = gameObject != null;
									if (flag6)
									{
										return gameObject;
									}
								}
							}
						}
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x000181DC File Offset: 0x000163DC
		private void CleanupCategoryContainers()
		{
			try
			{
				bool flag = this.MenuObject == null;
				if (!flag)
				{
					VerticalLayoutGroup[] array = this.MenuObject.GetComponentsInChildren<VerticalLayoutGroup>(true);
					bool flag2 = array == null;
					if (!flag2)
					{
						foreach (VerticalLayoutGroup verticalLayoutGroup in array)
						{
							bool flag3 = verticalLayoutGroup == null || verticalLayoutGroup.transform == null;
							if (!flag3)
							{
								bool flag4 = verticalLayoutGroup.transform.Find("DynamicSidePanel_Header") != null;
								if (!flag4)
								{
									for (int j = verticalLayoutGroup.transform.childCount - 1; j >= 0; j--)
									{
										Transform child = verticalLayoutGroup.transform.GetChild(j);
										bool flag5 = child != null && child.name != "DynamicSidePanel_Header";
										if (flag5)
										{
											Object.Destroy(child.gameObject);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error cleaning category containers: " + ex.Message);
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x00018334 File Offset: 0x00016534
		private TextMeshProUGUIEx FindTitleText()
		{
			TextMeshProUGUIEx result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					GameObject gameObject = this.FindComponentInObject(this.MenuObject, "Text_Name");
					result = ((gameObject != null) ? gameObject.GetComponent<TextMeshProUGUIEx>() : null);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error finding title text: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x000183B0 File Offset: 0x000165B0
		private void SetupHeaderButtons()
		{
			try
			{
				bool flag = this.MenuTitleText == null;
				if (!flag)
				{
					Transform parent = this.MenuTitleText.transform.parent;
					bool flag2 = parent == null;
					if (!flag2)
					{
						GameObject gameObject = this.FindComponentInObject(parent.gameObject, "Button_Logout");
						bool flag3 = gameObject != null;
						if (flag3)
						{
							Object.Destroy(gameObject);
						}
						GameObject gameObject2 = this.FindComponentInObject(parent.gameObject, "Button_Exit");
						bool flag4 = gameObject2 != null;
						if (flag4)
						{
							Object.Destroy(gameObject2);
						}
						GameObject gameObject3 = this.FindComponentInObject(parent.gameObject, "Separator");
						bool flag5 = gameObject3 != null;
						if (flag5)
						{
							RectTransform component = gameObject3.GetComponent<RectTransform>();
							bool flag6 = component != null;
							if (flag6)
							{
								component.anchoredPosition = new Vector2(0f, -100f);
							}
						}
						LayoutElement component2 = parent.GetComponent<LayoutElement>();
						bool flag7 = component2 != null;
						if (flag7)
						{
							component2.minHeight = 100f;
						}
						GameObject gameObject4 = this.FindComponentInObject(parent.gameObject, "Icon");
						bool flag8 = gameObject4 != null && this._pageIcon != null;
						if (flag8)
						{
							Image component3 = gameObject4.GetComponent<Image>();
							bool flag9 = component3 != null;
							if (flag9)
							{
								component3.sprite = this._pageIcon;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error setting up header buttons: " + ex.Message);
			}
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001855C File Offset: 0x0001675C
		private IEnumerator SetTitleWhenActive(string text, string color)
		{
			while (this.MenuObject != null && !this.MenuObject.activeInHierarchy)
			{
				yield return null;
			}
			try
			{
				bool flag = this.MenuTitleText != null;
				if (flag)
				{
					this.MenuTitleText.text = string.Concat(new string[]
					{
						"<color=",
						color,
						">",
						text,
						"</color>"
					});
				}
				yield break;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				Debug.LogError("[KernellClient] Error setting title text: " + ex.Message);
				yield break;
			}
			yield break;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001857C File Offset: 0x0001677C
		public void Open()
		{
			try
			{
				bool flag = this.UiPage == null;
				if (flag)
				{
					Debug.LogError("[KernellClient] Cannot open page: UiPage is null");
				}
				else
				{
					this.UiPage.gameObject.SetActive(true);
					bool flag2 = MenuEx.QMenuStateCtrl != null;
					if (flag2)
					{
						MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(this.UiPage.field_Public_String_0, null, false, 9058);
					}
					Action onOpen = this.OnOpen;
					if (onOpen != null)
					{
						onOpen();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error opening page: " + ex.Message);
			}
		}

		// Token: 0x06000436 RID: 1078 RVA: 0x00018634 File Offset: 0x00016834
		internal Transform GetCategoryButtonContainer()
		{
			Transform result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					VerticalLayoutGroup[] array = this.MenuObject.GetComponentsInChildren<VerticalLayoutGroup>(true);
					bool flag2 = array != null;
					if (flag2)
					{
						foreach (VerticalLayoutGroup verticalLayoutGroup in array)
						{
							bool flag3 = verticalLayoutGroup == null || verticalLayoutGroup.transform == null || verticalLayoutGroup.transform.parent == null;
							if (!flag3)
							{
								bool flag4 = verticalLayoutGroup.transform.parent.name.Contains("Viewport") && verticalLayoutGroup.transform.parent.name.Contains("Navigation");
								if (flag4)
								{
									return verticalLayoutGroup.transform;
								}
							}
						}
					}
					GameObject gameObject = this.FindComponentInObject(this.MenuObject, "VerticalLayoutGroup");
					bool flag5 = gameObject == null;
					if (flag5)
					{
						Debug.LogError("[KernellClient] Could not find category button container");
					}
					result = ((gameObject != null) ? gameObject.transform : null);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error getting category button container: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x06000437 RID: 1079 RVA: 0x000187A4 File Offset: 0x000169A4
		internal Transform GetCategoryChildContainer()
		{
			Transform result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					VerticalLayoutGroup[] array = this.MenuObject.GetComponentsInChildren<VerticalLayoutGroup>(true);
					bool flag2 = array != null;
					if (flag2)
					{
						foreach (VerticalLayoutGroup verticalLayoutGroup in array)
						{
							bool flag3 = verticalLayoutGroup == null || verticalLayoutGroup.transform == null || verticalLayoutGroup.transform.parent == null;
							if (!flag3)
							{
								bool flag4 = verticalLayoutGroup.transform.parent.name.Contains("Viewport") && !verticalLayoutGroup.transform.parent.name.Contains("Navigation");
								if (flag4)
								{
									return verticalLayoutGroup.transform;
								}
							}
						}
					}
					Transform transform = this.FindContentContainer();
					bool flag5 = transform != null;
					if (flag5)
					{
						result = transform;
					}
					else
					{
						result = this.GetCategoryButtonContainer();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error getting category child container: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x000188F8 File Offset: 0x00016AF8
		private Transform FindContentContainer()
		{
			Transform result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					string[] array = new string[]
					{
						"Content_Header",
						"Content",
						"ScrollRect_Content"
					};
					foreach (string name in array)
					{
						GameObject gameObject = this.FindComponentInObject(this.MenuObject, name);
						bool flag2 = gameObject != null;
						if (flag2)
						{
							return gameObject.transform;
						}
					}
					result = null;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error finding content container: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x000189BC File Offset: 0x00016BBC
		internal TextMeshProUGUIEx GetCategoryTitle()
		{
			TextMeshProUGUIEx result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					GameObject gameObject = this.FindComponentInObject(this.MenuObject, "Text_Title");
					bool flag2 = gameObject == null;
					if (flag2)
					{
						Debug.LogError("[KernellClient] Could not find category title");
						result = null;
					}
					else
					{
						result = gameObject.GetComponent<TextMeshProUGUIEx>();
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error getting category title: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x00018A50 File Offset: 0x00016C50
		internal Transform GetSidePanelHeader()
		{
			Transform result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					GameObject gameObject = this.FindComponentInObject(this.MenuObject, "DynamicSidePanel_Header");
					bool flag2 = gameObject == null;
					if (flag2)
					{
						Debug.LogError("[KernellClient] Could not find side panel header");
						result = null;
					}
					else
					{
						result = gameObject.transform;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error getting side panel header: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00018AE4 File Offset: 0x00016CE4
		public ReMMHeaderButton AddHeaderButton(string tooltip, Action onClick, Sprite icon = null)
		{
			ReMMHeaderButton result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					Debug.LogError("[KernellClient] Cannot add header button: MenuObject is null");
					result = null;
				}
				else
				{
					result = new ReMMHeaderButton(tooltip, icon, this, onClick);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error adding header button: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00018B58 File Offset: 0x00016D58
		public ReMMSidebarHeaderButton AddSidebarHeaderButton(string text, string tooltip, Action onClick, Sprite icon = null, string color = "#ffffff")
		{
			ReMMSidebarHeaderButton result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					Debug.LogError("[KernellClient] Cannot add sidebar header button: MenuObject is null");
					result = null;
				}
				else
				{
					result = new ReMMSidebarHeaderButton(this, text, tooltip, icon, onClick, color);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error adding sidebar header button: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00018BD0 File Offset: 0x00016DD0
		public ReMMPage GetMenuPage(string name)
		{
			ReMMPage result;
			try
			{
				string cleanName = UiElement.GetCleanName("Menu_" + name);
				GameObject gameObject = null;
				bool flag = MenuEx.MMenuParent != null;
				if (flag)
				{
					Transform transform = MenuEx.MMenuParent.transform.Find(cleanName);
					bool flag2 = transform != null;
					if (flag2)
					{
						gameObject = transform.gameObject;
					}
				}
				bool flag3 = gameObject == null && MenuEx.MMSettingsMenu != null;
				if (flag3)
				{
					Transform transform2 = MenuEx.MMSettingsMenu.transform.Find(cleanName);
					bool flag4 = transform2 != null;
					if (flag4)
					{
						gameObject = transform2.gameObject;
					}
				}
				bool flag5 = gameObject == null && MenuEx.MMenuParent != null;
				if (flag5)
				{
					gameObject = this.FindComponentInObject(MenuEx.MMenuParent, cleanName);
				}
				result = ((gameObject != null) ? new ReMMPage(gameObject.transform) : null);
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error getting menu page: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00018CF0 File Offset: 0x00016EF0
		public ReMMCategory AddnGetMenuCategory(string title, string tooltip, Sprite icon = null, string color = "#ffffff")
		{
			ReMMCategory result;
			try
			{
				bool flag = this.MenuObject == null;
				if (flag)
				{
					Debug.LogError("[KernellClient] Cannot add menu category: MenuObject is null");
					result = null;
				}
				else
				{
					result = new ReMMCategory(this, title, tooltip, icon, null, color);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error adding menu category: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00018D68 File Offset: 0x00016F68
		public static ReMMPage Create(string text, Sprite icon, bool isRoot)
		{
			ReMMPage result;
			try
			{
				bool flag = MenuEx.MMSettingsMenu == null;
				if (flag)
				{
					Debug.LogError("[KernellClient] Cannot create page: MenuEx.MMSettingsMenu is null");
					result = null;
				}
				else
				{
					bool flag2 = MenuEx.QMenuParent == null;
					if (flag2)
					{
						Debug.LogError("[KernellClient] Cannot create page: MenuEx.QMenuParent is null");
						result = null;
					}
					else
					{
						bool flag3 = MenuEx.MMenuStateCtrl == null;
						if (flag3)
						{
							Debug.LogError("[KernellClient] Cannot create page: MenuEx.MMenuStateCtrl is null");
							result = null;
						}
						else
						{
							result = new ReMMPage(text, icon, isRoot, "#ffffff");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[KernellClient] Error creating page: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x040001B2 RID: 434
		private readonly bool _isRoot;

		// Token: 0x040001B3 RID: 435
		private readonly Transform _container;

		// Token: 0x040001B4 RID: 436
		private Sprite _pageIcon;
	}
}
