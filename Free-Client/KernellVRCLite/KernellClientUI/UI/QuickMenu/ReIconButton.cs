using System;
using System.Diagnostics;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Elements;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000040 RID: 64
	public class ReIconButton
	{
		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000D463 File Offset: 0x0000B663
		public UIPage UiPage { get; }

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600028A RID: 650 RVA: 0x0000D46C File Offset: 0x0000B66C
		// (remove) Token: 0x0600028B RID: 651 RVA: 0x0000D4A4 File Offset: 0x0000B6A4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnOpen;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x0600028C RID: 652 RVA: 0x0000D4DC File Offset: 0x0000B6DC
		// (remove) Token: 0x0600028D RID: 653 RVA: 0x0000D514 File Offset: 0x0000B714
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnClose;

		// Token: 0x0600028E RID: 654 RVA: 0x0000D54C File Offset: 0x0000B74C
		public ReIconButton(ReMenuPage menu, Sprite icon, string parent = "Dashboard")
		{
			Transform transform = MenuEx.QMDashboardMenu.transform.Find("Header_H1/RightItemContainer/Button_QM_Expand");
			Transform transform2 = MenuEx.QMenuParent.transform.Find("Menu_" + parent).transform.Find("Header_H1/RightItemContainer/");
			GameObject gameObject = Object.Instantiate<Transform>(transform, transform2).gameObject;
			gameObject.transform.Find("Icon").GetComponent<Image>().overrideSprite = icon;
			Button component = gameObject.GetComponent<Button>();
			component.onClick.RemoveAllListeners();
			component.onClick.AddListener(new Action(menu.Open));
			EnableDisableListener enableDisableListener = gameObject.AddComponent<EnableDisableListener>();
			enableDisableListener.OnEnableEvent += delegate()
			{
				bool flag = this.OnOpen != null;
				if (flag)
				{
					this.OnOpen();
				}
			};
			enableDisableListener.OnDisableEvent += delegate()
			{
				bool flag = this.OnClose != null;
				if (flag)
				{
					this.OnClose();
				}
			};
		}
	}
}
