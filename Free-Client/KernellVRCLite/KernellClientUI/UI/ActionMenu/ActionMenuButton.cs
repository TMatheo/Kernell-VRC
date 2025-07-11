using System;
using UnityEngine;

namespace KernellClientUI.UI.ActionMenu
{
	// Token: 0x0200006A RID: 106
	public class ActionMenuButton
	{
		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0001A043 File Offset: 0x00018243
		// (set) Token: 0x0600047A RID: 1146 RVA: 0x0001A04B File Offset: 0x0001824B
		public PedalOption currentPedalOption { get; set; }

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001A054 File Offset: 0x00018254
		internal Func<bool> buttonAction { get; }

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0001A05C File Offset: 0x0001825C
		// (set) Token: 0x0600047D RID: 1149 RVA: 0x0001A064 File Offset: 0x00018264
		internal string buttonText { get; private set; }

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x0001A06D File Offset: 0x0001826D
		// (set) Token: 0x0600047F RID: 1151 RVA: 0x0001A075 File Offset: 0x00018275
		internal Texture2D buttonIcon { get; private set; }

		// Token: 0x06000480 RID: 1152 RVA: 0x0001A080 File Offset: 0x00018280
		public ActionMenuButton(string text, Action action, Sprite icon = null)
		{
			this.buttonText = text;
			this.buttonIcon = ((icon != null) ? icon.texture : null);
			this.buttonAction = delegate()
			{
				action();
				return true;
			};
			ActionMenuAPI.mainMenuButtons.Add(this);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x0001A0E4 File Offset: 0x000182E4
		public ActionMenuButton(ActionMenuPage basePage, string text, Action action, Sprite icon = null)
		{
			this.buttonText = text;
			this.buttonIcon = ((icon != null) ? icon.texture : null);
			this.buttonAction = delegate()
			{
				action();
				return true;
			};
			basePage.buttons.Add(this);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x0001A148 File Offset: 0x00018348
		public void SetButtonText(string newText)
		{
			this.buttonText = newText;
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x0001A154 File Offset: 0x00018354
		public void SetIcon(Sprite newTexture)
		{
			this.buttonIcon = newTexture.texture;
			bool flag = this.currentPedalOption != null;
			if (flag)
			{
				this.currentPedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(newTexture.texture);
			}
		}
	}
}
