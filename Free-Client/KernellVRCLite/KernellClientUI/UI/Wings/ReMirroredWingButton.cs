using System;
using UnityEngine;

namespace KernellClientUI.UI.Wings
{
	// Token: 0x02000037 RID: 55
	public class ReMirroredWingButton
	{
		// Token: 0x0600023A RID: 570 RVA: 0x0000C2C8 File Offset: 0x0000A4C8
		public ReMirroredWingButton(string text, string tooltip, Action onClick, Transform leftParent, Transform rightParent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			this._leftButton = new ReWingButton(text, tooltip, onClick, leftParent, sprite, arrow, background, separator);
			this._rightButton = new ReWingButton(text, tooltip, onClick, rightParent, sprite, arrow, background, separator);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000C30D File Offset: 0x0000A50D
		public void Destroy()
		{
			Object.DestroyImmediate(this._leftButton.GameObject);
			Object.DestroyImmediate(this._rightButton.GameObject);
		}

		// Token: 0x040000F4 RID: 244
		private readonly ReWingButton _leftButton;

		// Token: 0x040000F5 RID: 245
		private readonly ReWingButton _rightButton;
	}
}
