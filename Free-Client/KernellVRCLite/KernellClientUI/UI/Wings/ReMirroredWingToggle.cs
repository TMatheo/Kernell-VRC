using System;
using UnityEngine;

namespace KernellClientUI.UI.Wings
{
	// Token: 0x02000039 RID: 57
	public class ReMirroredWingToggle
	{
		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000C574 File Offset: 0x0000A774
		// (set) Token: 0x06000245 RID: 581 RVA: 0x0000C591 File Offset: 0x0000A791
		public bool Interactable
		{
			get
			{
				return this._leftToggle.Interactable;
			}
			set
			{
				this._leftToggle.Interactable = value;
				this._rightToggle.Interactable = value;
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000C5B0 File Offset: 0x0000A7B0
		public ReMirroredWingToggle(string text, string tooltip, Action<bool> onToggle, Transform leftParent, Transform rightParent, bool defaultValue = false)
		{
			ReMirroredWingToggle.<>c__DisplayClass5_0 CS$<>8__locals1 = new ReMirroredWingToggle.<>c__DisplayClass5_0();
			CS$<>8__locals1.onToggle = onToggle;
			base..ctor();
			this._leftToggle = new ReWingToggle(text, tooltip, delegate(bool b)
			{
				ReWingToggle rightToggle = this._rightToggle;
				if (rightToggle != null)
				{
					rightToggle.Toggle(b, false);
				}
				CS$<>8__locals1.onToggle(b);
			}, leftParent, defaultValue);
			this._rightToggle = new ReWingToggle(text, tooltip, delegate(bool b)
			{
				this._leftToggle.Toggle(b, false);
				CS$<>8__locals1.onToggle(b);
			}, rightParent, defaultValue);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000C620 File Offset: 0x0000A820
		public void Toggle(bool b, bool callback = true)
		{
			this._leftToggle.Toggle(b, callback);
			this._rightToggle.Toggle(b, callback);
		}

		// Token: 0x040000F8 RID: 248
		private readonly ReWingToggle _leftToggle;

		// Token: 0x040000F9 RID: 249
		private readonly ReWingToggle _rightToggle;
	}
}
