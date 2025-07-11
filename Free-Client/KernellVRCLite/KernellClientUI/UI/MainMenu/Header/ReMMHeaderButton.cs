using System;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.MainMenu.Header
{
	// Token: 0x02000067 RID: 103
	public class ReMMHeaderButton : ReMMHeaderElement
	{
		// Token: 0x0600046B RID: 1131 RVA: 0x00019DD4 File Offset: 0x00017FD4
		public ReMMHeaderButton(string tooltip, Sprite icon, ReMMPage page, Action onClick) : base(MMenuPrefabs.MMHeaderButtonPrefab, page, tooltip)
		{
			base.gameObject.name = "Button_header";
			base.gameObject.transform.Find("Icon").GetComponent<Image>().overrideSprite = icon;
			this.buttonComponent = base.gameObject.GetComponent<Button>();
			this.buttonComponent.onClick.AddListener(new Action(onClick.Invoke));
		}

		// Token: 0x040001CF RID: 463
		private Button buttonComponent;
	}
}
