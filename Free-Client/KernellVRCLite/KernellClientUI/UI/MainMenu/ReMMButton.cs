using System;
using KernellClientUI.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Ui;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x0200005B RID: 91
	public class ReMMButton : ReMMSectionElement
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x00016A1C File Offset: 0x00014C1C
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00016A34 File Offset: 0x00014C34
		public bool ThinMode
		{
			get
			{
				return this._thinMode;
			}
			set
			{
				this._thinMode = value;
				this.UpdateThinMode();
			}
		}

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x00016A48 File Offset: 0x00014C48
		// (set) Token: 0x060003ED RID: 1005 RVA: 0x00016A65 File Offset: 0x00014C65
		public string Text
		{
			get
			{
				return this._textComponent.text;
			}
			set
			{
				this._textComponent.text = value;
			}
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x060003EE RID: 1006 RVA: 0x00016A78 File Offset: 0x00014C78
		// (set) Token: 0x060003EF RID: 1007 RVA: 0x00016A95 File Offset: 0x00014C95
		public bool Interactable
		{
			get
			{
				return this._buttonComponent.interactable;
			}
			set
			{
				this._buttonComponent.interactable = value;
				base.StyleElement.OnEnable();
			}
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x00016AB4 File Offset: 0x00014CB4
		public ReMMButton(string title, string buttontext, string tooltip, Action onClick, Transform parent = null, bool separator = true, string color = "#ffffff", bool thinMode = false) : base(MMenuPrefabs.MMTogglePrefab, parent, true, separator)
		{
			Object.Destroy(base.gameObject.GetComponent<Toggle>());
			Object.Destroy(base.RightItemContainer.Find("Cell_MM_OnOffSwitch").gameObject);
			ReMMAvatarButton reMMAvatarButton = new ReMMAvatarButton(buttontext, tooltip, onClick, null, base.RightItemContainer, color);
			this._buttonComponent = reMMAvatarButton.btn;
			this._buttonBackGround = reMMAvatarButton.background;
			HorizontalLayoutGroup component = base.RightItemContainer.gameObject.GetComponent<HorizontalLayoutGroup>();
			component.childAlignment = 5;
			component.childControlWidth = true;
			this._textComponent = base.LeftItemContainer.Find("Title").GetComponent<TextMeshProUGUIEx>();
			this._textComponent.richText = true;
			bool flag = buttontext != null;
			if (flag)
			{
				this.Text = string.Concat(new string[]
				{
					"<color=",
					color,
					">",
					title,
					"</color>"
				});
			}
			Transform transform = this._buttonComponent.transform.Find("Icon");
			this._iconObject = ((transform != null) ? transform.gameObject : null);
			this._thinMode = thinMode;
			this.UpdateThinMode();
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00016BE8 File Offset: 0x00014DE8
		private void UpdateThinMode()
		{
			bool flag = this._iconObject != null;
			if (flag)
			{
				this._iconObject.SetActive(!this._thinMode);
			}
			bool thinMode = this._thinMode;
			if (thinMode)
			{
				ContentSizeFitter contentSizeFitter = this._buttonComponent.gameObject.GetComponent<ContentSizeFitter>();
				bool flag2 = contentSizeFitter == null;
				if (flag2)
				{
					contentSizeFitter = this._buttonComponent.gameObject.AddComponent<ContentSizeFitter>();
				}
				contentSizeFitter.horizontalFit = 2;
				LayoutElement component = this._buttonComponent.gameObject.GetComponent<LayoutElement>();
				bool flag3 = component != null;
				if (flag3)
				{
					component.minWidth = 40f;
				}
			}
			else
			{
				ContentSizeFitter component2 = this._buttonComponent.gameObject.GetComponent<ContentSizeFitter>();
				bool flag4 = component2 != null;
				if (flag4)
				{
					component2.horizontalFit = 0;
				}
				LayoutElement component3 = this._buttonComponent.gameObject.GetComponent<LayoutElement>();
				bool flag5 = component3 != null;
				if (flag5)
				{
					component3.minWidth = 80f;
				}
			}
			LayoutRebuilder.ForceRebuildLayoutImmediate(this._buttonComponent.transform.parent as RectTransform);
		}

		// Token: 0x0400019B RID: 411
		private readonly Button _buttonComponent;

		// Token: 0x0400019C RID: 412
		public readonly ImageEx _buttonBackGround;

		// Token: 0x0400019D RID: 413
		private TextMeshProUGUI _textComponent;

		// Token: 0x0400019E RID: 414
		private GameObject _iconObject;

		// Token: 0x0400019F RID: 415
		private bool _thinMode;
	}
}
