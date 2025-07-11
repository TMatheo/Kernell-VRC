using System;
using System.Collections;
using KernelVRC;
using UnityEngine;
using VRC.Localization;

namespace KernellVRCLite
{
	// Token: 0x0200007A RID: 122
	internal class ToastNotif
	{
		// Token: 0x06000580 RID: 1408 RVA: 0x000215A8 File Offset: 0x0001F7A8
		public static void Toast(string content, string description = null, Sprite icon = null, float duration = 5f)
		{
			try
			{
				bool flag = VRCUiManager.field_Private_Static_VRCUiManager_0 == null || VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0 == null;
				if (flag)
				{
					kernelllogger.Warning("[ToastNotif] Cannot show toast, UI not initialized: " + content);
					kernelllogger.Msg(string.Concat(new string[]
					{
						"\n",
						content,
						"\n",
						description,
						"\n\n"
					}));
				}
				else
				{
					LocalizableString localizableString = LocalizableStringExtensions.Localize(content, null, null, null);
					LocalizableString localizableString2 = LocalizableStringExtensions.Localize(description, null, null, null);
					VRCUiManager.field_Private_Static_VRCUiManager_0.field_Private_HudController_0.notification.Method_Public_Void_Sprite_LocalizableString_LocalizableString_Single_Object1PublicTBoTUnique_1_Boolean_0(icon, localizableString, localizableString2, duration, null);
					kernelllogger.Msg(string.Concat(new string[]
					{
						"\n",
						content,
						"\n",
						description,
						"\n\n"
					}));
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ToastNotif] Error showing toast: " + ex.Message);
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000216B0 File Offset: 0x0001F8B0
		public static IEnumerator DelayToast(float delay, string content, string description = null, Sprite icon = null, float duration = 5f)
		{
			float startTime = Time.time;
			while (Time.time < startTime + delay)
			{
				yield return null;
			}
			ToastNotif.Toast(content, description, icon, duration);
			yield break;
		}
	}
}
