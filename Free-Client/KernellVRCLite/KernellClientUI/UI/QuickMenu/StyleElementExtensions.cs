using System;
using MelonLoader;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x0200004E RID: 78
	public static class StyleElementExtensions
	{
		// Token: 0x0600038B RID: 907 RVA: 0x00012B04 File Offset: 0x00010D04
		public static void SetInteractableStyle(this StyleElement styleElement, bool isInteractable)
		{
			bool flag = styleElement == null;
			if (flag)
			{
				MelonLogger.Warning("StyleElementExtensions: Provided StyleElement is null.");
			}
			else
			{
				try
				{
					styleElement.Method_Private_Void_Boolean_Boolean_0(isInteractable, isInteractable);
				}
				catch (Exception ex)
				{
					MelonLogger.Warning("StyleElementExtensions: Failed to set interactable style. Exception: " + ex.Message);
				}
			}
		}
	}
}
