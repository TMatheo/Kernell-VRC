using System;
using KernellClientUI.UI.MainMenu;
using KernellClientUI.UI.QuickMenu;
using KernellClientUI.VRChat;
using UnityEngine;
using VRC.UI.Elements;

namespace KernellClientUI.Managers
{
	// Token: 0x0200006F RID: 111
	public class UiManager
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000495 RID: 1173 RVA: 0x0001A531 File Offset: 0x00018731
		public IButtonPage QMMenu { get; }

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000496 RID: 1174 RVA: 0x0001A539 File Offset: 0x00018739
		public IButtonPage TargetMenu { get; }

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000497 RID: 1175 RVA: 0x0001A541 File Offset: 0x00018741
		public IButtonPage LaunchPad { get; }

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001A549 File Offset: 0x00018749
		public IButtonPage LaunchPadTargetMenu { get; }

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0001A551 File Offset: 0x00018751
		public ReMMCategory MMenu { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x0600049A RID: 1178 RVA: 0x0001A55C File Offset: 0x0001875C
		// (set) Token: 0x0600049B RID: 1179 RVA: 0x0001A584 File Offset: 0x00018784
		public ButtonLayoutShape QMMenuLayout
		{
			get
			{
				ReMenuPage reMenuPage = this.QMMenu as ReMenuPage;
				return (reMenuPage != null) ? reMenuPage.LayoutShape : ButtonLayoutShape.Grid;
			}
			set
			{
				ReMenuPage reMenuPage = this.QMMenu as ReMenuPage;
				bool flag = reMenuPage != null;
				if (flag)
				{
					reMenuPage.SetButtonLayout(value);
				}
			}
		}

		// Token: 0x0600049C RID: 1180 RVA: 0x0001A5AE File Offset: 0x000187AE
		public void OpenQMMenu()
		{
			GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)").GetComponent<QuickMenu>().Method_Private_Void_1();
		}

		// Token: 0x0600049D RID: 1181 RVA: 0x0001A5C8 File Offset: 0x000187C8
		public VRCUiPage getcurrentopenpage()
		{
			return null;
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600049E RID: 1182 RVA: 0x0001A5DC File Offset: 0x000187DC
		// (set) Token: 0x0600049F RID: 1183 RVA: 0x0001A604 File Offset: 0x00018804
		public bool UseThinButtons
		{
			get
			{
				ReMenuPage reMenuPage = this.QMMenu as ReMenuPage;
				return reMenuPage != null && reMenuPage.UseThinButtons;
			}
			set
			{
				ReMenuPage reMenuPage = this.QMMenu as ReMenuPage;
				bool flag = reMenuPage != null;
				if (flag)
				{
					reMenuPage.SetThinButtons(value);
				}
				ReMenuPage reMenuPage2 = this.TargetMenu as ReMenuPage;
				bool flag2 = reMenuPage2 != null;
				if (flag2)
				{
					reMenuPage2.SetThinButtons(value);
				}
				ReMenuPage reMenuPage3 = this.LaunchPad as ReMenuPage;
				bool flag3 = reMenuPage3 != null;
				if (flag3)
				{
					reMenuPage3.SetThinButtons(value);
				}
			}
		}

		// Token: 0x060004A0 RID: 1184 RVA: 0x0001A66C File Offset: 0x0001886C
		public UiManager(string menuName, Sprite menuSprite, bool createQMTargets = true, bool createLaunchPadMenu = true, bool createMainMenu = false, string color = "#ffffff", ButtonLayoutShape initialLayout = ButtonLayoutShape.Grid, bool useThinButtons = false)
		{
			ReMenuPage reMenuPage = new ReMenuPage(menuName, true, color);
			this.QMMenu = reMenuPage;
			reMenuPage.SetButtonLayout(initialLayout);
			reMenuPage.SetThinButtons(useThinButtons);
			ReMenuPage menu = reMenuPage;
			ReTabButton.Create(menuName, "Open the " + menuName + " menu.", menuName, menuSprite, menu);
			if (createQMTargets)
			{
				ReCategoryPage reCategoryPage = new ReCategoryPage(MenuEx.QMSelectedUserLocal.transform);
				this.TargetMenu = reCategoryPage.AddCategory(menuName ?? "", color);
				ReMenuPage reMenuPage2 = this.TargetMenu as ReMenuPage;
				bool flag = reMenuPage2 != null && useThinButtons;
				if (flag)
				{
					reMenuPage2.SetThinButtons(true);
				}
			}
			if (createLaunchPadMenu)
			{
				ReCategoryPage reCategoryPage2 = new ReCategoryPage(MenuEx.QMDashboardMenu.transform);
				this.LaunchPad = reCategoryPage2.AddCategory(menuName ?? "", color);
				ReMenuPage reMenuPage3 = this.LaunchPad as ReMenuPage;
				bool flag2 = reMenuPage3 != null && useThinButtons;
				if (flag2)
				{
					reMenuPage3.SetThinButtons(true);
				}
			}
			if (createMainMenu)
			{
				bool flag3 = !UiManager.isMMPageCreated;
				if (flag3)
				{
					UiManager.mmpage = new ReMMPage("ReMod", null, true, "#ffffff");
					ReMMTab.Create("ReMod", "Open the ReMod menu.", "ReMod", menuSprite);
					UiManager.isMMPageCreated = true;
				}
				bool flag4 = UiManager.mmpage != null;
				if (flag4)
				{
					this.MMenu = UiManager.mmpage.AddnGetMenuCategory(menuName, menuName, menuSprite, color);
				}
			}
		}

		// Token: 0x060004A1 RID: 1185 RVA: 0x0001A7DC File Offset: 0x000189DC
		public void SetQMButtonLayout(ButtonLayoutShape layoutShape)
		{
			this.QMMenuLayout = layoutShape;
		}

		// Token: 0x060004A2 RID: 1186 RVA: 0x0001A7E8 File Offset: 0x000189E8
		public void SetAllButtonLayouts(ButtonLayoutShape layoutShape)
		{
			ReMenuPage reMenuPage = this.QMMenu as ReMenuPage;
			bool flag = reMenuPage != null;
			if (flag)
			{
				reMenuPage.SetButtonLayout(layoutShape);
			}
			ReMenuPage reMenuPage2 = this.TargetMenu as ReMenuPage;
			bool flag2 = reMenuPage2 != null;
			if (flag2)
			{
				reMenuPage2.SetButtonLayout(layoutShape);
			}
			ReMenuPage reMenuPage3 = this.LaunchPad as ReMenuPage;
			bool flag3 = reMenuPage3 != null;
			if (flag3)
			{
				reMenuPage3.SetButtonLayout(layoutShape);
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x0001A84E File Offset: 0x00018A4E
		public void SetThinButtonsForAll(bool useThin)
		{
			this.UseThinButtons = useThin;
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x0001A85C File Offset: 0x00018A5C
		public ReMenuButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, string color = "#ffffff")
		{
			ReMenuPage reMenuPage = this.QMMenu as ReMenuPage;
			bool flag = reMenuPage != null;
			ReMenuButton result;
			if (flag)
			{
				result = reMenuPage.AddButton(text, tooltip, onClick, sprite, color);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x0001A894 File Offset: 0x00018A94
		public ReMenuToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false, string color = "#ffffff")
		{
			ReMenuPage reMenuPage = this.QMMenu as ReMenuPage;
			bool flag = reMenuPage != null;
			ReMenuToggle result;
			if (flag)
			{
				result = reMenuPage.AddToggle(text, tooltip, onToggle, defaultValue, color);
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x040001E4 RID: 484
		private static bool isMMPageCreated;

		// Token: 0x040001E5 RID: 485
		private static ReMMPage mmpage;
	}
}
