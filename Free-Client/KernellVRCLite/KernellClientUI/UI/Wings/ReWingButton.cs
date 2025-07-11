using System;
using KernellClientUI.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.Wings
{
	// Token: 0x0200003A RID: 58
	public class ReWingButton : UiElement
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000C640 File Offset: 0x0000A840
		private static GameObject WingButtonPrefab
		{
			get
			{
				bool flag = ReWingButton._wingButtonPrefab == null;
				if (flag)
				{
					ReWingButton._wingButtonPrefab = MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup/Button_Profile").gameObject;
				}
				return ReWingButton._wingButtonPrefab;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000C688 File Offset: 0x0000A888
		// (set) Token: 0x0600024A RID: 586 RVA: 0x0000C6A8 File Offset: 0x0000A8A8
		public Sprite Sprite
		{
			get
			{
				return this._iconImage.sprite;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					this._iconImage.sprite = value;
					this._iconImage.overrideSprite = value;
				}
				this._iconImage.gameObject.SetActive(value != null);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000C6F8 File Offset: 0x0000A8F8
		// (set) Token: 0x0600024C RID: 588 RVA: 0x0000C715 File Offset: 0x0000A915
		public bool Interactable
		{
			get
			{
				return this._button.interactable;
			}
			set
			{
				this._button.interactable = value;
				this._styleElement.Method_Private_Void_Boolean_Boolean_0(value, false);
			}
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000C734 File Offset: 0x0000A934
		public ReWingButton(string text, string tooltip, Action onClick, Sprite sprite = null, bool left = true, bool arrow = true, bool background = true, bool separator = false) : this(text, tooltip, onClick, (left ? MenuEx.QMLeftWing : MenuEx.QMRightWing).transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), sprite, arrow, background, separator)
		{
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000C774 File Offset: 0x0000A974
		public ReWingButton(string text, string tooltip, Action onClick, Transform parent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false) : base(ReWingButton.WingButtonPrefab, parent, "Button_" + text, true)
		{
			Transform transform = base.RectTransform.Find("Container").transform;
			transform.Find("Background").gameObject.SetActive(background);
			transform.Find("Icon_Arrow").gameObject.SetActive(arrow);
			base.RectTransform.Find("Separator").gameObject.SetActive(separator);
			this._iconImage = transform.Find("Icon").GetComponent<Image>();
			this.Sprite = sprite;
			TextMeshProUGUI componentInChildren = transform.GetComponentInChildren<TextMeshProUGUI>();
			componentInChildren.text = text;
			componentInChildren.richText = true;
			this._styleElement = base.GameObject.GetComponent<StyleElement>();
			this._button = base.GameObject.GetComponent<Button>();
			this._button.onClick = new Button.ButtonClickedEvent();
			this._button.onClick.AddListener(new Action(onClick.Invoke));
			bool flag = sprite == null && !arrow;
			if (flag)
			{
				transform.gameObject.AddComponent<HorizontalLayoutGroup>();
				componentInChildren.enableAutoSizing = true;
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000C8B8 File Offset: 0x0000AAB8
		public static void Create(string text, string tooltip, Action onClick, Sprite sprite = null, WingSide wingSide = WingSide.Both, bool arrow = true, bool background = true, bool separator = true)
		{
			bool flag = (wingSide & WingSide.Left) == WingSide.Left;
			if (flag)
			{
				new ReWingButton(text, tooltip, onClick, sprite, true, arrow, background, separator);
			}
			bool flag2 = (wingSide & WingSide.Right) == WingSide.Right;
			if (flag2)
			{
				new ReWingButton(text, tooltip, onClick, sprite, false, arrow, background, separator);
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000C902 File Offset: 0x0000AB02
		public static void Create(string text, string tooltip, Action onClick, Transform parent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = true)
		{
			new ReWingButton(text, tooltip, onClick, parent, sprite, arrow, background, separator);
		}

		// Token: 0x040000FA RID: 250
		private static GameObject _wingButtonPrefab;

		// Token: 0x040000FB RID: 251
		private readonly Image _iconImage;

		// Token: 0x040000FC RID: 252
		private readonly StyleElement _styleElement;

		// Token: 0x040000FD RID: 253
		private readonly Button _button;
	}
}
