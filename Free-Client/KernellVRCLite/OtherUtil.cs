using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Il2CppSystem;
using KernellClientUI.VRChat;
using KernellVRCLite.API;
using UnhollowerRuntimeLib.XrefScans;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// Token: 0x0200000E RID: 14
public static class OtherUtil
{
	// Token: 0x06000035 RID: 53 RVA: 0x00002C48 File Offset: 0x00000E48
	public static bool XRefScanForMethod(MethodBase methodBase, string methodName = null, string reflectedType = null)
	{
		bool flag = false;
		foreach (XrefInstance xrefInstance in XrefScanner.XrefScan(methodBase))
		{
			bool flag2 = xrefInstance.Type != 1;
			if (!flag2)
			{
				MethodBase methodBase2 = xrefInstance.TryResolve();
				bool flag3 = !(methodBase2 == null);
				if (flag3)
				{
					Type reflectedType2 = methodBase2.ReflectedType;
					Console.WriteLine((reflectedType2 != null) ? reflectedType2.Name : null);
					bool flag4 = !string.IsNullOrEmpty(methodName);
					if (flag4)
					{
						flag = (methodBase2.Name != null && methodBase2.Name.IndexOf(methodName, StringComparison.OrdinalIgnoreCase) >= 0);
					}
					bool flag5 = !string.IsNullOrEmpty(reflectedType);
					if (flag5)
					{
						Type reflectedType3 = methodBase2.ReflectedType;
						flag = (((reflectedType3 != null) ? reflectedType3.Name : null) != null && methodBase2.ReflectedType.Name.IndexOf(reflectedType, StringComparison.OrdinalIgnoreCase) >= 0);
					}
					bool flag6 = flag;
					if (flag6)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002D70 File Offset: 0x00000F70
	public static string ToAgeString(DateTime dob)
	{
		DateTime today = DateTime.Today;
		int num = today.Month - dob.Month;
		int num2 = today.Year - dob.Year;
		bool flag = today.Day < dob.Day;
		if (flag)
		{
			num--;
		}
		bool flag2 = num < 0;
		if (flag2)
		{
			num2--;
			num += 12;
		}
		int days = (today - dob.AddMonths(num2 * 12 + num)).Days;
		return string.Format("{0} Year{1}, {2} Month{3}, {4} Day{5}", new object[]
		{
			num2,
			(num2 == 1) ? "" : "s",
			num,
			(num == 1) ? "" : "s",
			days,
			(days == 1) ? "" : "s"
		});
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00002E5C File Offset: 0x0000105C
	private static void CloseInputPopupWrapper(string _)
	{
		OtherUtil.CloseInputPopup();
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00002E68 File Offset: 0x00001068
	public static MainMenuSelectedUser GetMainMenuSelectedUser()
	{
		bool flag = OtherUtil._mainMenuSelectedUser == null;
		if (flag)
		{
			OtherUtil._mainMenuSelectedUser = MenuEx.MMenuParent.transform.Find("Menu_MM_Profile").GetComponent<MainMenuSelectedUser>();
		}
		return OtherUtil._mainMenuSelectedUser;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00002EAE File Offset: 0x000010AE
	public static IEnumerator LoadAudio(AudioSource audioSource, string path)
	{
		UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, 20);
		yield return www.SendWebRequest();
		bool flag = www.result != 2 && www.result != 3;
		if (flag)
		{
			audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
		}
		www.Dispose();
		yield break;
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002EC4 File Offset: 0x000010C4
	public static void CloseInputPopup()
	{
		bool flag = OtherUtil._closeKeyboardObject == null;
		if (flag)
		{
			OtherUtil._closeKeyboardObject = APIUtils.SocialMenuInstance.transform.Find("Container/MMParent/Modal_MM_Keyboard/MenuPanel/Header_Modal_H1_Centered/RightItemContainer/Button_Close").gameObject;
		}
		bool flag2 = !(OtherUtil._closeKeyboardObject == null);
		if (flag2)
		{
			Button component = OtherUtil._closeKeyboardObject.GetComponent<Button>();
			bool flag3 = component != null;
			if (flag3)
			{
				component.onClick.Invoke();
			}
		}
	}

	// Token: 0x04000010 RID: 16
	private static MainMenuSelectedUser _mainMenuSelectedUser;

	// Token: 0x04000011 RID: 17
	private static VRCInputField _keyboardComponent;

	// Token: 0x04000012 RID: 18
	private static GameObject _closeKeyboardObject;

	// Token: 0x04000013 RID: 19
	private static GameObject _container;

	// Token: 0x04000014 RID: 20
	public static List<string> totalHudLogs = new List<string>();

	// Token: 0x04000015 RID: 21
	public static List<string> processStrings = new List<string>();
}
