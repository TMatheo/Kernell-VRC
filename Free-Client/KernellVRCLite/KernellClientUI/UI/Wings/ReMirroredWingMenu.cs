using System;
using KernellClientUI.VRChat;
using UnityEngine;

namespace KernellClientUI.UI.Wings
{
	// Token: 0x02000038 RID: 56
	public class ReMirroredWingMenu
	{
		// Token: 0x17000091 RID: 145
		// (get) Token: 0x0600023C RID: 572 RVA: 0x0000C334 File Offset: 0x0000A534
		// (set) Token: 0x0600023D RID: 573 RVA: 0x0000C361 File Offset: 0x0000A561
		public bool Active
		{
			get
			{
				return this._leftMenu.Active && this._rightMenu.Active;
			}
			set
			{
				this._leftMenu.Active = value;
				this._rightMenu.Active = value;
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000C380 File Offset: 0x0000A580
		public ReMirroredWingMenu(string text, string tooltip, Transform leftParent, Transform rightParent, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			this._leftMenu = new ReWingMenu(text, true);
			this._rightMenu = new ReWingMenu(text, false);
			ReWingButton.Create(text, tooltip, new Action(this._leftMenu.Open), leftParent, sprite, arrow, background, separator);
			ReWingButton.Create(text, tooltip, new Action(this._rightMenu.Open), rightParent, sprite, arrow, background, separator);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		public static ReMirroredWingMenu Create(string text, string tooltip, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			return new ReMirroredWingMenu(text, tooltip, MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), MenuEx.QMRightWing.transform.Find("Container/InnerContainer/WingMenu/ScrollRect/Viewport/VerticalLayoutGroup"), sprite, arrow, background, separator);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x0000C43C File Offset: 0x0000A63C
		public ReMirroredWingButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			bool flag = this._leftMenu == null || this._rightMenu == null;
			if (flag)
			{
				throw new NullReferenceException("This wing menu has been destroyed.");
			}
			return new ReMirroredWingButton(text, tooltip, onClick, this._leftMenu.Container, this._rightMenu.Container, sprite, arrow, background, separator);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000C498 File Offset: 0x0000A698
		public ReMirroredWingToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue)
		{
			bool flag = this._leftMenu == null || this._rightMenu == null;
			if (flag)
			{
				throw new NullReferenceException("This wing menu has been destroyed.");
			}
			return new ReMirroredWingToggle(text, tooltip, onToggle, this._leftMenu.Container, this._rightMenu.Container, defaultValue);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000C4F0 File Offset: 0x0000A6F0
		public ReMirroredWingMenu AddSubMenu(string text, string tooltip, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
		{
			bool flag = this._leftMenu == null || this._rightMenu == null;
			if (flag)
			{
				throw new NullReferenceException("This wing menu has been destroyed.");
			}
			return new ReMirroredWingMenu(text, tooltip, this._leftMenu.Container, this._rightMenu.Container, sprite, arrow, background, separator);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000C54A File Offset: 0x0000A74A
		public void Destroy()
		{
			this._leftMenu.Destroy();
			this._rightMenu.Destroy();
			this._leftMenu = null;
			this._rightMenu = null;
		}

		// Token: 0x040000F6 RID: 246
		private ReWingMenu _leftMenu;

		// Token: 0x040000F7 RID: 247
		private ReWingMenu _rightMenu;
	}
}
