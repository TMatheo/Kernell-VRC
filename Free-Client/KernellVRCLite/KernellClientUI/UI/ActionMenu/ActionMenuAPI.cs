using System;
using System.Collections.Generic;
using Il2CppSystem;
using UnhollowerRuntimeLib;
using VRC.Localization;

namespace KernellClientUI.UI.ActionMenu
{
	// Token: 0x02000069 RID: 105
	public static class ActionMenuAPI
	{
		// Token: 0x06000472 RID: 1138 RVA: 0x00019EC0 File Offset: 0x000180C0
		public static bool IsOpen(ActionMenuOpener actionMenuOpener)
		{
			return actionMenuOpener.field_Private_Boolean_0;
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00019ED8 File Offset: 0x000180D8
		internal static ActionMenuOpener GetActionMenuOpener()
		{
			bool flag = !ActionMenuAPI.IsOpen(ActionMenuAPI.ActionMenuOpener1) && ActionMenuAPI.IsOpen(ActionMenuAPI.ActionMenuOpener2);
			ActionMenuOpener result;
			if (flag)
			{
				result = ActionMenuAPI.ActionMenuOpener2;
			}
			else
			{
				bool flag2 = ActionMenuAPI.IsOpen(ActionMenuAPI.ActionMenuOpener1) && !ActionMenuAPI.IsOpen(ActionMenuAPI.ActionMenuOpener2);
				if (flag2)
				{
					result = ActionMenuAPI.ActionMenuOpener1;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00019F3C File Offset: 0x0001813C
		internal static void OpenMainPage(ActionMenu menu)
		{
			ActionMenuAPI.activeActionMenu = menu;
			foreach (ActionMenuButton actionMenuButton in ActionMenuAPI.mainMenuButtons)
			{
				PedalOption pedalOption = ActionMenuAPI.activeActionMenu.Method_Public_PedalOption_0();
				pedalOption.Method_Public_set_Void_LocalizableString_PDM_0(LocalizableStringExtensions.Localize(actionMenuButton.buttonText, null, null, null));
				pedalOption.field_Public_Func_1_Boolean_0 = DelegateSupport.ConvertDelegate<Func<bool>>(actionMenuButton.buttonAction);
				pedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(actionMenuButton.buttonIcon);
				actionMenuButton.currentPedalOption = pedalOption;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0001A007 File Offset: 0x00018207
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0001A00E File Offset: 0x0001820E
		public static ActionMenu activeActionMenu { get; private set; }

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001A018 File Offset: 0x00018218
		public static bool IsWeelOpen
		{
			get
			{
				return ActionMenuAPI.ActionMenuOpener1.field_Private_Boolean_0 || ActionMenuAPI.ActionMenuOpener2.field_Private_Boolean_0;
			}
		}

		// Token: 0x040001D3 RID: 467
		internal static readonly List<ActionMenuButton> mainMenuButtons = new List<ActionMenuButton>();

		// Token: 0x040001D4 RID: 468
		private static readonly ActionMenuOpener ActionMenuOpener1 = ActionMenuController.field_Public_Static_ActionMenuController_0.field_Public_ActionMenuOpener_0;

		// Token: 0x040001D5 RID: 469
		private static readonly ActionMenuOpener ActionMenuOpener2 = ActionMenuController.field_Public_Static_ActionMenuController_0.field_Public_ActionMenuOpener_1;
	}
}
