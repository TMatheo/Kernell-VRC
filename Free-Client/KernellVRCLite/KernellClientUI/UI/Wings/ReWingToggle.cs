using System;
using KernellClientUI.VRChat;
using UnityEngine;

namespace KernellClientUI.UI.Wings
{
	// Token: 0x0200003B RID: 59
	public class ReWingToggle
	{
		// Token: 0x17000096 RID: 150
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000C918 File Offset: 0x0000AB18
		// (set) Token: 0x06000252 RID: 594 RVA: 0x0000C935 File Offset: 0x0000AB35
		public bool Interactable
		{
			get
			{
				return this._button.Interactable;
			}
			set
			{
				this._button.Interactable = value;
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000C948 File Offset: 0x0000AB48
		public ReWingToggle(string text, string tooltip, Action<bool> onToggle, Transform parent, bool defaultValue = false)
		{
			this._onToggle = onToggle;
			this._button = new ReWingButton(text, tooltip, delegate()
			{
				this.Toggle(!this._state, true);
			}, parent, this.GetCurrentIcon(), false, true, false);
			this.Toggle(defaultValue, true);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000C994 File Offset: 0x0000AB94
		private Sprite GetCurrentIcon()
		{
			return this._state ? MenuEx.OnIconSprite : MenuEx.OffIconSprite;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000C9BC File Offset: 0x0000ABBC
		public void Toggle(bool b, bool callback = true)
		{
			bool flag = this._state != b;
			if (flag)
			{
				this._state = b;
				this._button.Sprite = this.GetCurrentIcon();
				if (callback)
				{
					this._onToggle(this._state);
				}
			}
		}

		// Token: 0x040000FE RID: 254
		private readonly ReWingButton _button;

		// Token: 0x040000FF RID: 255
		private readonly Action<bool> _onToggle;

		// Token: 0x04000100 RID: 256
		private bool _state;
	}
}
