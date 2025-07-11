using System;
using System.Collections.Generic;
using Il2CppSystem;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC.Localization;

namespace KernellClientUI.UI.ActionMenu
{
	// Token: 0x0200006B RID: 107
	public class ActionMenuPage
	{
		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0001A193 File Offset: 0x00018393
		public ActionMenuPage previousPage { get; }

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000485 RID: 1157 RVA: 0x0001A19B File Offset: 0x0001839B
		public ActionMenuButton menuEntryButton { get; }

		// Token: 0x06000486 RID: 1158 RVA: 0x0001A1A3 File Offset: 0x000183A3
		public ActionMenuPage(string buttonText, Sprite buttonIcon = null)
		{
			this.menuEntryButton = new ActionMenuButton(buttonText, new Action(this.OpenMenu), buttonIcon);
		}

		// Token: 0x06000487 RID: 1159 RVA: 0x0001A1D1 File Offset: 0x000183D1
		public ActionMenuPage(ActionMenuPage basePage, string buttonText, Sprite buttonIcon = null)
		{
			this.previousPage = basePage;
			this.menuEntryButton = new ActionMenuButton(this.previousPage, buttonText, new Action(this.OpenMenu), buttonIcon);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x0001A20C File Offset: 0x0001840C
		internal void OpenMenu()
		{
			bool flag = ActionMenuAPI.GetActionMenuOpener() == null;
			if (!flag)
			{
				using (List<ActionMenuButton>.Enumerator enumerator = this.buttons.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActionMenuButton button = enumerator.Current;
						ActionMenuAPI.GetActionMenuOpener().field_Public_ActionMenu_0.Method_Public_ObjectNPublicAcGaAcFu1BoUnique_Action_Action_0(delegate()
						{
							PedalOption pedalOption = ActionMenuAPI.GetActionMenuOpener().field_Public_ActionMenu_0.Method_Public_PedalOption_0();
							pedalOption.Method_Public_set_Void_LocalizableString_PDM_0(LocalizableStringExtensions.Localize(button.buttonText, null, null, null));
							pedalOption.field_Public_ActionMenu_0.field_Private_LocalizableString_1 = LocalizableStringExtensions.Localize(button.buttonText, null, null, null);
							pedalOption.field_Public_Func_1_Boolean_0 = DelegateSupport.ConvertDelegate<Func<bool>>(button.buttonAction);
							bool flag2 = button.buttonIcon != null;
							if (flag2)
							{
								pedalOption.Method_Public_Virtual_Final_New_Void_Texture2D_0(button.buttonIcon);
							}
							button.currentPedalOption = pedalOption;
						}, null);
					}
				}
			}
		}

		// Token: 0x040001DA RID: 474
		internal readonly List<ActionMenuButton> buttons = new List<ActionMenuButton>();
	}
}
