using System;
using System.Collections;
using KernellClientUI.VRChat;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.Localization;
using VRC.Ui;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000041 RID: 65
	public class ReMenuButton : UiElement
	{
		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000D681 File Offset: 0x0000B881
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000D689 File Offset: 0x0000B889
		public ImageEx Background { get; private set; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000D692 File Offset: 0x0000B892
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000D69C File Offset: 0x0000B89C
		public bool ThinMode
		{
			get
			{
				return this._thinMode;
			}
			set
			{
				bool flag = this._thinMode != value;
				if (flag)
				{
					this._thinMode = value;
					this.UpdateThinMode();
				}
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000D6CA File Offset: 0x0000B8CA
		// (set) Token: 0x06000296 RID: 662 RVA: 0x0000D6E8 File Offset: 0x0000B8E8
		public string Text
		{
			get
			{
				TextMeshProUGUIEx text = this._text;
				return ((text != null) ? text.text : null) ?? string.Empty;
			}
			set
			{
				bool flag = this._text != null && this._text.text != value;
				if (flag)
				{
					this._text.SetText(value, true);
				}
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000D72A File Offset: 0x0000B92A
		public float ButtonWidth
		{
			get
			{
				RectTransform rectTransform = base.RectTransform;
				return (rectTransform != null) ? rectTransform.sizeDelta.x : 0f;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000D747 File Offset: 0x0000B947
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000D758 File Offset: 0x0000B958
		public string Tooltip
		{
			get
			{
				return this._cachedTooltip ?? string.Empty;
			}
			set
			{
				bool flag = this._cachedTooltip != value;
				if (flag)
				{
					this._cachedTooltip = value;
					this.UpdateTooltip(value);
				}
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000D787 File Offset: 0x0000B987
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000D79C File Offset: 0x0000B99C
		public bool Interactable
		{
			get
			{
				Button button = this._button;
				return button != null && button.interactable;
			}
			set
			{
				bool flag = this._button != null && this._button.interactable != value;
				if (flag)
				{
					this._button.interactable = value;
					StyleElement styleElement = this._styleElement;
					if (styleElement != null)
					{
						styleElement.Method_Private_Void_Boolean_Boolean_0(value, false);
					}
				}
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		public ReMenuButton(string text, string tooltip, UnityAction onClick, Transform parent, Sprite sprite = null, bool resizeTextNoSprite = true, string color = "#ffffff", bool thinMode = false) : base(QMMenuPrefabs.ButtonPrefab, parent, "Button_" + text, true)
		{
			bool flag = base.GameObject == null;
			if (!flag)
			{
				try
				{
					this.CacheComponents();
					this.InitializeText(text, color);
					this.InitializeIcon(sprite, resizeTextNoSprite);
					this.SetupTooltipAndButton(tooltip, onClick);
					this._thinMode = thinMode;
					if (thinMode)
					{
						this.UpdateThinMode();
					}
					this._isInitialized = true;
				}
				catch (Exception arg)
				{
					MelonLogger.Error(string.Format("Failed to initialize ReMenuButton: {0}", arg));
				}
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D89C File Offset: 0x0000BA9C
		private void CacheComponents()
		{
			this._text = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
			bool flag = this._text != null;
			if (flag)
			{
				this._originalTextPosY = this._text.transform.localPosition.y;
				this._originalFontSize = this._text.fontSize;
				this._text.richText = true;
			}
			RectTransform rectTransform = base.RectTransform;
			Transform transform = (rectTransform != null) ? rectTransform.Find("Background") : null;
			this.Background = ((transform != null) ? transform.GetComponent<ImageEx>() : null);
			RectTransform rectTransform2 = base.RectTransform;
			GameObject iconsGameObject;
			if (rectTransform2 == null)
			{
				iconsGameObject = null;
			}
			else
			{
				Transform transform2 = rectTransform2.Find("Icons");
				iconsGameObject = ((transform2 != null) ? transform2.gameObject : null);
			}
			this._iconsGameObject = iconsGameObject;
			RectTransform rectTransform3 = base.RectTransform;
			Transform transform3 = (rectTransform3 != null) ? rectTransform3.Find("Icons/Icon") : null;
			bool flag2 = transform3 != null;
			if (flag2)
			{
				this._iconImage = transform3.GetComponent<ImageEx>();
			}
			this._button = base.GameObject.GetComponent<Button>();
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D99C File Offset: 0x0000BB9C
		private void InitializeText(string text, string color)
		{
			bool flag = this._text == null;
			if (!flag)
			{
				MelonCoroutines.Start(this.ApplyTextColorCoroutine(text, color));
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000D9CC File Offset: 0x0000BBCC
		private void InitializeIcon(Sprite sprite, bool resizeTextNoSprite)
		{
			bool flag = this._iconImage == null;
			if (!flag)
			{
				bool flag2 = sprite == null;
				if (flag2)
				{
					if (resizeTextNoSprite)
					{
						this.SetupTextOnlyButton();
					}
					else
					{
						this.DisableIcon();
					}
				}
				else
				{
					this.SetupIconSprite(sprite);
				}
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000DA1E File Offset: 0x0000BC1E
		private void SetupTooltipAndButton(string tooltip, UnityAction onClick)
		{
			this.CleanupBadges();
			this.SetupTooltip(tooltip);
			this.SetupButtonClick(onClick);
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000DA38 File Offset: 0x0000BC38
		private void SetupTextOnlyButton()
		{
			bool flag = this._text == null;
			if (!flag)
			{
				this._text.fontSize = 35f;
				this._text.enableAutoSizing = true;
				this._text.color = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
				this._text.m_fontColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
				this._text.m_htmlColor = new Color(0.4157f, 0.8902f, 0.9765f, 1f);
				Vector3 localPosition = this._text.transform.localPosition;
				this._text.transform.localPosition = new Vector3(localPosition.x, -30f, localPosition.z);
				bool flag2 = this.Background != null;
				if (flag2)
				{
					this.EnsureLayoutElement(this.Background.gameObject).ignoreLayout = true;
				}
				base.GameObject.AddComponent<LogoutButton>();
				this._styleElement = this._text.GetComponent<StyleElement>();
				bool flag3 = this._styleElement != null;
				if (flag3)
				{
					this._styleElement.field_Public_String_1 = "H1";
				}
				bool flag4 = this._iconsGameObject != null;
				if (flag4)
				{
					Object.DestroyImmediate(this._iconsGameObject);
				}
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000DBAB File Offset: 0x0000BDAB
		private void DisableIcon()
		{
			this._iconImage.sprite = null;
			this._iconImage.overrideSprite = null;
			this._iconImage.enabled = false;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000DBD8 File Offset: 0x0000BDD8
		private void SetupIconSprite(Sprite sprite)
		{
			this._iconImage.sprite = sprite;
			this._iconImage.overrideSprite = sprite;
			this._iconImage.enabled = true;
			this._iconImage.color = Color.white;
			RectTransform rectTransform = this._iconImage.transform as RectTransform;
			bool flag = rectTransform != null;
			if (flag)
			{
				rectTransform.sizeDelta = new Vector2(50f, 50f);
				rectTransform.anchoredPosition = new Vector2(0f, 20f);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000DC68 File Offset: 0x0000BE68
		private void CleanupBadges()
		{
			RectTransform rectTransform = base.RectTransform;
			Transform transform = (rectTransform != null) ? rectTransform.Find("Badge_Close") : null;
			bool flag = transform != null;
			if (flag)
			{
				Object.DestroyImmediate(transform.gameObject);
			}
			RectTransform rectTransform2 = base.RectTransform;
			Transform transform2 = (rectTransform2 != null) ? rectTransform2.Find("Badge_MMJump") : null;
			bool flag2 = transform2 != null;
			if (flag2)
			{
				Object.DestroyImmediate(transform2.gameObject);
			}
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000DCD4 File Offset: 0x0000BED4
		private void SetupTooltip(string tooltip)
		{
			Il2CppArrayBase<ToolTip> components = base.GameObject.GetComponents<ToolTip>();
			bool flag = components != null && components.Length > 0;
			if (flag)
			{
				this._tooltip = components[0];
				for (int i = 1; i < components.Length; i++)
				{
					Object.DestroyImmediate(components[i]);
				}
			}
			this._cachedTooltip = tooltip;
			this.UpdateTooltip(tooltip);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000DD44 File Offset: 0x0000BF44
		private void SetupButtonClick(UnityAction onClick)
		{
			bool flag = this._button != null && onClick != null;
			if (flag)
			{
				this._button.onClick = new Button.ButtonClickedEvent();
				this._button.onClick.AddListener(onClick);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000DD94 File Offset: 0x0000BF94
		private LayoutElement EnsureLayoutElement(GameObject target = null)
		{
			GameObject gameObject = target ?? base.GameObject;
			bool flag = this._layoutElement == null || this._layoutElement.gameObject != gameObject;
			if (flag)
			{
				this._layoutElement = (gameObject.GetComponent<LayoutElement>() ?? gameObject.AddComponent<LayoutElement>());
			}
			return this._layoutElement;
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000DDF8 File Offset: 0x0000BFF8
		private ContentSizeFitter EnsureContentSizeFitter()
		{
			bool flag = this._contentSizeFitter == null;
			if (flag)
			{
				this._contentSizeFitter = (base.RectTransform.GetComponent<ContentSizeFitter>() ?? base.RectTransform.gameObject.AddComponent<ContentSizeFitter>());
			}
			return this._contentSizeFitter;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000DE48 File Offset: 0x0000C048
		private void RemoveContentSizeFitter()
		{
			bool flag = this._contentSizeFitter != null;
			if (flag)
			{
				Object.DestroyImmediate(this._contentSizeFitter);
				this._contentSizeFitter = null;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000DE7C File Offset: 0x0000C07C
		public void SetButtonSize(float width, float height)
		{
			bool flag = base.RectTransform == null;
			if (!flag)
			{
				LayoutElement layoutElement = this.EnsureLayoutElement(null);
				layoutElement.ignoreLayout = true;
				layoutElement.minWidth = width;
				layoutElement.preferredWidth = width;
				layoutElement.flexibleWidth = 0f;
				layoutElement.minHeight = height;
				layoutElement.preferredHeight = height;
				layoutElement.flexibleHeight = 0f;
				this.RemoveContentSizeFitter();
				base.RectTransform.sizeDelta = new Vector2(width, height);
				bool flag2 = this._originalWidth == 0f;
				if (flag2)
				{
					this._originalWidth = width;
				}
			}
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000DF18 File Offset: 0x0000C118
		public void SetButtonWidth(float width)
		{
			bool flag = base.RectTransform == null;
			if (!flag)
			{
				LayoutElement layoutElement = this.EnsureLayoutElement(null);
				layoutElement.ignoreLayout = true;
				layoutElement.minWidth = width;
				layoutElement.preferredWidth = width;
				layoutElement.flexibleWidth = 0f;
				Vector2 sizeDelta = base.RectTransform.sizeDelta;
				base.RectTransform.sizeDelta = new Vector2(width, sizeDelta.y);
				bool flag2 = this._originalWidth == 0f;
				if (flag2)
				{
					this._originalWidth = width;
				}
			}
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000DFA0 File Offset: 0x0000C1A0
		public void SetIconPosition(float x, float y)
		{
			ImageEx iconImage = this._iconImage;
			RectTransform rectTransform = ((iconImage != null) ? iconImage.transform : null) as RectTransform;
			bool flag = rectTransform != null;
			if (flag)
			{
				rectTransform.anchoredPosition = new Vector2(x, y);
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000DFE0 File Offset: 0x0000C1E0
		public void SetIconSize(float width, float height)
		{
			ImageEx iconImage = this._iconImage;
			RectTransform rectTransform = ((iconImage != null) ? iconImage.transform : null) as RectTransform;
			bool flag = rectTransform != null;
			if (flag)
			{
				rectTransform.sizeDelta = new Vector2(width, height);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000E020 File Offset: 0x0000C220
		public void SetSprite(Sprite newSprite)
		{
			bool flag = this._iconImage == null;
			if (!flag)
			{
				bool flag2 = newSprite != null;
				if (flag2)
				{
					this.SetupIconSprite(newSprite);
					bool thinMode = this._thinMode;
					if (thinMode)
					{
						this.ThinMode = false;
					}
				}
				else
				{
					this.DisableIcon();
				}
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000E074 File Offset: 0x0000C274
		public void IncreaseButtonWidth(float deltaWidth)
		{
			bool flag = base.RectTransform == null;
			if (!flag)
			{
				this.SetButtonWidth(base.RectTransform.sizeDelta.x + deltaWidth);
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
		public void DecreaseButtonWidth(float deltaWidth)
		{
			bool flag = base.RectTransform == null;
			if (!flag)
			{
				float buttonWidth = Mathf.Max(base.RectTransform.sizeDelta.x - deltaWidth, 50f);
				this.SetButtonWidth(buttonWidth);
			}
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000E0F8 File Offset: 0x0000C2F8
		public void ResetButtonWidth()
		{
			bool flag = this._originalWidth > 0f;
			if (flag)
			{
				this.SetButtonWidth(this._originalWidth);
			}
		}

		// Token: 0x060002B2 RID: 690 RVA: 0x0000E128 File Offset: 0x0000C328
		private void UpdateThinMode()
		{
			bool flag = !this._isInitialized || base.RectTransform == null;
			if (!flag)
			{
				bool thinMode = this._thinMode;
				if (thinMode)
				{
					this.ApplyThinMode();
				}
				else
				{
					this.RestoreNormalMode();
				}
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.RectTransform);
			}
		}

		// Token: 0x060002B3 RID: 691 RVA: 0x0000E180 File Offset: 0x0000C380
		private void ApplyThinMode()
		{
			bool flag = this._iconsGameObject != null;
			if (flag)
			{
				this._iconsGameObject.SetActive(false);
			}
			bool flag2 = this._text != null;
			if (flag2)
			{
				Vector3 localPosition = this._text.transform.localPosition;
				this._text.transform.localPosition = new Vector3(0f, 0f, localPosition.z);
				this._text.fontSize = this._originalFontSize * 0.9f;
				this._text.enableAutoSizing = true;
				this._text.fontSizeMin = 18f;
				this._text.fontSizeMax = this._originalFontSize;
				this._text.alignment = 514;
			}
			float buttonWidth = (this._originalWidth > 0f) ? (this._originalWidth * 0.6f) : 80f;
			this.SetButtonWidth(buttonWidth);
			ContentSizeFitter contentSizeFitter = this.EnsureContentSizeFitter();
			contentSizeFitter.horizontalFit = 2;
			LayoutElement layoutElement = this.EnsureLayoutElement(null);
			layoutElement.minWidth = 60f;
		}

		// Token: 0x060002B4 RID: 692 RVA: 0x0000E2A0 File Offset: 0x0000C4A0
		private void RestoreNormalMode()
		{
			bool flag = this._iconsGameObject != null;
			if (flag)
			{
				this._iconsGameObject.SetActive(true);
			}
			bool flag2 = this._text != null;
			if (flag2)
			{
				Vector3 localPosition = this._text.transform.localPosition;
				this._text.transform.localPosition = new Vector3(localPosition.x, this._originalTextPosY, localPosition.z);
				this._text.fontSize = this._originalFontSize;
				this._text.alignment = 514;
			}
			this.RemoveContentSizeFitter();
			bool flag3 = this._originalWidth > 0f;
			if (flag3)
			{
				this.SetButtonWidth(this._originalWidth);
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000E360 File Offset: 0x0000C560
		private void UpdateTooltip(string tooltip)
		{
			bool flag = this._tooltip != null && !string.IsNullOrEmpty(tooltip);
			if (flag)
			{
				LocalizableString localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
				this._tooltip._localizableString = localizableString;
				this._tooltip._alternateLocalizableString = localizableString;
			}
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000E3B2 File Offset: 0x0000C5B2
		private IEnumerator ApplyTextColorCoroutine(string text, string color)
		{
			while (!base.GameObject.activeInHierarchy)
			{
				yield return null;
			}
			bool flag = this._text != null;
			if (flag)
			{
				this._text.text = string.Concat(new string[]
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

		// Token: 0x04000111 RID: 273
		private TextMeshProUGUIEx _text;

		// Token: 0x04000112 RID: 274
		private ToolTip _tooltip;

		// Token: 0x04000113 RID: 275
		private StyleElement _styleElement;

		// Token: 0x04000114 RID: 276
		private Button _button;

		// Token: 0x04000115 RID: 277
		private ImageEx _iconImage;

		// Token: 0x04000116 RID: 278
		private GameObject _iconsGameObject;

		// Token: 0x04000117 RID: 279
		private LayoutElement _layoutElement;

		// Token: 0x04000118 RID: 280
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x0400011A RID: 282
		private float _originalWidth;

		// Token: 0x0400011B RID: 283
		private float _originalTextPosY;

		// Token: 0x0400011C RID: 284
		private float _originalFontSize;

		// Token: 0x0400011D RID: 285
		private bool _thinMode;

		// Token: 0x0400011E RID: 286
		private string _cachedTooltip;

		// Token: 0x0400011F RID: 287
		private bool _isInitialized;

		// Token: 0x04000120 RID: 288
		private const float THIN_MODE_MULTIPLIER = 0.6f;

		// Token: 0x04000121 RID: 289
		private const float THIN_MODE_FONT_MULTIPLIER = 0.9f;

		// Token: 0x04000122 RID: 290
		private const float MIN_BUTTON_WIDTH = 50f;

		// Token: 0x04000123 RID: 291
		private const float MIN_THIN_WIDTH = 60f;

		// Token: 0x04000124 RID: 292
		private const float DEFAULT_ICON_SIZE = 50f;

		// Token: 0x04000125 RID: 293
		private const float DEFAULT_ICON_Y_OFFSET = 20f;

		// Token: 0x04000126 RID: 294
		private const float TEXT_ONLY_Y_OFFSET = -30f;

		// Token: 0x04000127 RID: 295
		private const float TEXT_ONLY_FONT_SIZE = 35f;
	}
}
