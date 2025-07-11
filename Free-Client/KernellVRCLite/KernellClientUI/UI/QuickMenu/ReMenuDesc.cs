using System;
using System.Collections;
using KernellClientUI.VRChat;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000045 RID: 69
	public class ReMenuDesc : UiElement
	{
		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002EF RID: 751 RVA: 0x0000F244 File Offset: 0x0000D444
		private static GameObject DescriptionPrefab
		{
			get
			{
				bool flag = ReMenuDesc._descPrefab == null;
				if (flag)
				{
					string text = "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Here/ScrollRect/Viewport/VerticalLayoutGroup/QMCell_InstanceDetails/Panel/Info/Text_DetailsHeader";
					GameObject gameObject = GameObject.Find(text);
					Transform transform = (gameObject != null) ? gameObject.transform : null;
					bool flag2 = transform != null;
					if (flag2)
					{
						ReMenuDesc._descPrefab = transform.gameObject;
						MelonLogger.Msg("Found description text prefab (Text_DetailsHeader) successfully");
					}
					else
					{
						string text2 = "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Header_H1/LeftItemContainer/Text_Title";
						GameObject gameObject2 = GameObject.Find(text2);
						transform = ((gameObject2 != null) ? gameObject2.transform : null);
						bool flag3 = transform != null;
						if (flag3)
						{
							ReMenuDesc._descPrefab = transform.gameObject;
							MelonLogger.Msg("Found alternative text prefab (Text_Title) successfully");
						}
						else
						{
							ReMenuDesc._descPrefab = QMMenuPrefabs.MenuCategoryHeaderPrefab;
							MelonLogger.Warning("Could not find text prefabs, using header prefab as fallback");
						}
					}
				}
				return ReMenuDesc._descPrefab;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000F30A File Offset: 0x0000D50A
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000F328 File Offset: 0x0000D528
		public string Text
		{
			get
			{
				TextMeshProUGUIEx textComponent = this._textComponent;
				return ((textComponent != null) ? textComponent.text : null) ?? string.Empty;
			}
			set
			{
				bool flag = this._textComponent != null;
				if (flag)
				{
					MelonCoroutines.Start(this.SetTextWhenActive(value));
				}
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000F355 File Offset: 0x0000D555
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000F370 File Offset: 0x0000D570
		public float FontSize
		{
			get
			{
				TextMeshProUGUIEx textComponent = this._textComponent;
				return (textComponent != null) ? textComponent.fontSize : 12f;
			}
			set
			{
				bool flag = this._textComponent != null;
				if (flag)
				{
					this._textComponent.fontSize = value;
				}
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000F39D File Offset: 0x0000D59D
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000F3B8 File Offset: 0x0000D5B8
		public TextAlignmentOptions TextAlignment
		{
			get
			{
				TextMeshProUGUIEx textComponent = this._textComponent;
				return (textComponent != null) ? textComponent.alignment : 514;
			}
			set
			{
				bool flag = this._textComponent != null;
				if (flag)
				{
					this._textComponent.alignment = value;
				}
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x0000F3E5 File Offset: 0x0000D5E5
		public ReMenuDesc(string text, Transform parent, string color = "#FFFFFF", float fontSize = 14f, TextAlignmentOptions alignment = 513) : base(ReMenuDesc.DescriptionPrefab, parent, "Desc_" + UiElement.GetCleanName(text), true)
		{
			this.Initialize(text, color, fontSize, alignment, false);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000F414 File Offset: 0x0000D614
		public ReMenuDesc(string text, ReMenuCategory category, string color = "#FFFFFF", float fontSize = 14f, TextAlignmentOptions alignment = 513) : base(ReMenuDesc.DescriptionPrefab, category.RectTransform, "Desc_" + UiElement.GetCleanName(text), true)
		{
			this.Initialize(text, color, fontSize, alignment, true);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000F448 File Offset: 0x0000D648
		private void Initialize(string text, string color, float fontSize, TextAlignmentOptions alignment, bool inCategory)
		{
			try
			{
				this._textComponent = (base.GameObject.GetComponent<TextMeshProUGUIEx>() ?? base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>(true));
				bool flag = this._textComponent == null;
				if (flag)
				{
					MelonLogger.Error("Could not find TextMeshProUGUI component for: " + text);
					GameObject gameObject = new GameObject("DescText");
					gameObject.transform.SetParent(base.GameObject.transform, false);
					RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
					rectTransform.anchorMin = new Vector2(0f, 0f);
					rectTransform.anchorMax = new Vector2(1f, 1f);
					rectTransform.sizeDelta = Vector2.zero;
					this._textComponent = gameObject.AddComponent<TextMeshProUGUIEx>();
				}
				bool flag2 = this._textComponent != null;
				if (flag2)
				{
					this._textComponent.richText = true;
					this._textComponent.fontSize = fontSize;
					this._textComponent.alignment = alignment;
					this._textComponent.enableWordWrapping = true;
					this._textComponent.overflowMode = 0;
					MelonCoroutines.Start(this.SetTextWhenActive(string.Concat(new string[]
					{
						"<color=",
						color,
						">",
						text,
						"</color>"
					})));
				}
				this.SetupLayout(inCategory);
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("Error initializing ReMenuDesc: {0}", arg));
			}
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0000F5D8 File Offset: 0x0000D7D8
		private void SetupLayout(bool inCategory)
		{
			LayoutElement layoutElement = base.GameObject.GetComponent<LayoutElement>();
			bool flag = layoutElement == null;
			if (flag)
			{
				layoutElement = base.GameObject.AddComponent<LayoutElement>();
			}
			layoutElement.ignoreLayout = false;
			RectTransform component = base.GameObject.GetComponent<RectTransform>();
			bool flag2 = component != null;
			if (flag2)
			{
				component.anchorMin = new Vector2(0f, 1f);
				component.anchorMax = new Vector2(1f, 1f);
				component.pivot = new Vector2(0.5f, 1f);
				component.offsetMin = Vector2.zero;
				component.offsetMax = Vector2.zero;
			}
			this.RemoveInterferingComponents();
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0000F68C File Offset: 0x0000D88C
		private void RemoveInterferingComponents()
		{
			HorizontalLayoutGroup component = base.GameObject.GetComponent<HorizontalLayoutGroup>();
			bool flag = component != null;
			if (flag)
			{
				component.childAlignment = 0;
				component.childControlWidth = true;
				component.childForceExpandWidth = true;
				component.childControlHeight = true;
				component.childForceExpandHeight = true;
			}
		}

		// Token: 0x060002FB RID: 763 RVA: 0x0000F6DB File Offset: 0x0000D8DB
		private IEnumerator SetTextWhenActive(string text)
		{
			int attempts = 0;
			while (!base.GameObject.activeInHierarchy && attempts < 30)
			{
				int num = attempts;
				attempts = num + 1;
				yield return new WaitForSeconds(0.1f);
			}
			bool flag = this._textComponent != null;
			if (flag)
			{
				this._textComponent.text = text;
			}
			yield break;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x0000F6F1 File Offset: 0x0000D8F1
		public void SetColoredText(string text, string color)
		{
			this.Text = string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				text,
				"</color>"
			});
		}

		// Token: 0x060002FD RID: 765 RVA: 0x0000F728 File Offset: 0x0000D928
		public void SetItalic(bool italic)
		{
			bool flag = this._textComponent != null;
			if (flag)
			{
				FontStyles fontStyles = this._textComponent.fontStyle;
				if (italic)
				{
					fontStyles |= 2;
				}
				else
				{
					fontStyles &= -3;
				}
				this._textComponent.fontStyle = fontStyles;
			}
		}

		// Token: 0x060002FE RID: 766 RVA: 0x0000F774 File Offset: 0x0000D974
		public void SetBold(bool bold)
		{
			bool flag = this._textComponent != null;
			if (flag)
			{
				FontStyles fontStyles = this._textComponent.fontStyle;
				if (bold)
				{
					fontStyles |= 1;
				}
				else
				{
					fontStyles &= -2;
				}
				this._textComponent.fontStyle = fontStyles;
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0000F7C0 File Offset: 0x0000D9C0
		public void SetMargins(int left, int right, int top, int bottom)
		{
			LayoutGroup component = base.GameObject.GetComponent<LayoutGroup>();
			bool flag = component != null;
			if (flag)
			{
				component.padding = new RectOffset(left, right, top, bottom);
			}
		}

		// Token: 0x06000300 RID: 768 RVA: 0x0000F7F8 File Offset: 0x0000D9F8
		public static ReMenuDesc CreateHeader(Transform parent, string text, string color = "#FFAA00")
		{
			ReMenuDesc reMenuDesc = new ReMenuDesc(text, parent, color, 16f, 514);
			reMenuDesc.SetBold(true);
			LayoutElement component = reMenuDesc.GameObject.GetComponent<LayoutElement>();
			bool flag = component != null;
			if (flag)
			{
			}
			return reMenuDesc;
		}

		// Token: 0x06000301 RID: 769 RVA: 0x0000F840 File Offset: 0x0000DA40
		public static ReMenuDesc CreateCategoryHeader(ReMenuCategory category, string text, string color = "#FFAA00")
		{
			ReMenuDesc reMenuDesc = new ReMenuDesc(text, category, color, 16f, 514);
			reMenuDesc.SetBold(true);
			LayoutElement component = reMenuDesc.GameObject.GetComponent<LayoutElement>();
			bool flag = component != null;
			if (flag)
			{
			}
			return reMenuDesc;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x0000F888 File Offset: 0x0000DA88
		public static ReMenuDesc CreateDivider(Transform parent, string color = "#888888")
		{
			ReMenuDesc reMenuDesc = new ReMenuDesc("───────────────────────", parent, color, 12f, 514);
			LayoutElement component = reMenuDesc.GameObject.GetComponent<LayoutElement>();
			bool flag = component != null;
			if (flag)
			{
			}
			return reMenuDesc;
		}

		// Token: 0x06000303 RID: 771 RVA: 0x0000F8CC File Offset: 0x0000DACC
		public static ReMenuDesc CreateCategoryDivider(ReMenuCategory category, string color = "#888888")
		{
			ReMenuDesc reMenuDesc = new ReMenuDesc("───────────────────────", category, color, 12f, 514);
			LayoutElement component = reMenuDesc.GameObject.GetComponent<LayoutElement>();
			bool flag = component != null;
			if (flag)
			{
			}
			return reMenuDesc;
		}

		// Token: 0x04000133 RID: 307
		private TextMeshProUGUIEx _textComponent;

		// Token: 0x04000134 RID: 308
		private static GameObject _descPrefab;
	}
}
