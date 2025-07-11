using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MelonLoader;
using TMPro;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.DataModel;
using VRC.Localization;
using VRC.Ui;

namespace KernellClientUI.VRChat
{
	// Token: 0x0200002C RID: 44
	public static class PopupManagerExtensions
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x0000AB1E File Offset: 0x00008D1E
		static PopupManagerExtensions()
		{
			MelonCoroutines.Start(PopupManagerExtensions.<.cctor>g__wait|7_0());
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000AB40 File Offset: 0x00008D40
		public static void Alert1Box(string title, string content, Action middleBtnAction = null, string middleBtnText = "Okay", Sprite icon = null)
		{
			Action middleBtnAction2 = middleBtnAction;
			if (middleBtnAction == null && (middleBtnAction2 = PopupManagerExtensions.<>c.<>9__8_0) == null)
			{
				middleBtnAction2 = (PopupManagerExtensions.<>c.<>9__8_0 = delegate()
				{
				});
			}
			MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction2, null, null, middleBtnText, "", "", icon));
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000AB90 File Offset: 0x00008D90
		public static void Alert2Box(string title, string content, Action leftBtnAction = null, Action rightBtnAction = null, string leftBtnText = "Yes", string rightBtnText = "No", Sprite icon = null)
		{
			bool flag = rightBtnText == "" && rightBtnAction == null;
			if (flag)
			{
				Action middleBtnAction = leftBtnAction;
				if (leftBtnAction == null && (middleBtnAction = PopupManagerExtensions.<>c.<>9__9_0) == null)
				{
					middleBtnAction = (PopupManagerExtensions.<>c.<>9__9_0 = delegate()
					{
					});
				}
				MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction, null, null, leftBtnText, "", "", icon));
			}
			else
			{
				Action middleBtnAction2 = null;
				Action leftBtnAction2 = leftBtnAction;
				if (leftBtnAction == null && (leftBtnAction2 = PopupManagerExtensions.<>c.<>9__9_1) == null)
				{
					leftBtnAction2 = (PopupManagerExtensions.<>c.<>9__9_1 = delegate()
					{
					});
				}
				Action rightBtnAction2 = rightBtnAction;
				if (rightBtnAction == null && (rightBtnAction2 = PopupManagerExtensions.<>c.<>9__9_2) == null)
				{
					rightBtnAction2 = (PopupManagerExtensions.<>c.<>9__9_2 = delegate()
					{
					});
				}
				MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction2, leftBtnAction2, rightBtnAction2, "", leftBtnText, rightBtnText, icon));
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000AC60 File Offset: 0x00008E60
		public static void Alert3Box(string title, string content, Action leftBtnAction = null, Action middleBtnAction = null, Action rightBtnAction = null, string leftBtnText = "Yes", string middleBtnText = "Maybe", string rightBtnText = "No", Sprite icon = null)
		{
			Action middleBtnAction2 = middleBtnAction;
			if (middleBtnAction == null && (middleBtnAction2 = PopupManagerExtensions.<>c.<>9__10_0) == null)
			{
				middleBtnAction2 = (PopupManagerExtensions.<>c.<>9__10_0 = delegate()
				{
				});
			}
			Action leftBtnAction2 = leftBtnAction;
			if (leftBtnAction == null && (leftBtnAction2 = PopupManagerExtensions.<>c.<>9__10_1) == null)
			{
				leftBtnAction2 = (PopupManagerExtensions.<>c.<>9__10_1 = delegate()
				{
				});
			}
			Action rightBtnAction2 = rightBtnAction;
			if (rightBtnAction == null && (rightBtnAction2 = PopupManagerExtensions.<>c.<>9__10_2) == null)
			{
				rightBtnAction2 = (PopupManagerExtensions.<>c.<>9__10_2 = delegate()
				{
				});
			}
			MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction2, leftBtnAction2, rightBtnAction2, middleBtnText, leftBtnText, rightBtnText, icon));
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000ACF0 File Offset: 0x00008EF0
		private static IEnumerator TriggerQMConfirm(string title, string content, Action middleBtnAction = null, Action leftBtnAction = null, Action rightBtnAction = null, string middleBtnText = "Maybe", string leftBtnText = "Yes", string rightBtnText = "No", Sprite icon = null)
		{
			while (object.Equals(PopupManagerExtensions.QMConfirmButtonIsReady, false))
			{
				yield return null;
			}
			yield return new WaitForSeconds(0.3f);
			MenuEx.QMenuStateCtrl.Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0("ConfirmDialog", null, false, 0);
			Transform confirmmObj = MenuEx.QMenuParent.Find("Modal_ConfirmDialog/MenuPanel");
			Transform iconObj = confirmmObj.Find("QMHeader_H2/LeftItemContainer/Icon");
			MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something = confirmmObj.parent.GetComponent<MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique>();
			int buttons = 0;
			MelonCoroutines.Start(PopupManagerExtensions.QMConfirmIgnoredWait(confirmmObj, iconObj, something));
			confirmmObj.Find("Buttons").gameObject.SetActive(false);
			PopupManagerExtensions.QMCofirmPopupObj.SetActive(true);
			bool flag = leftBtnAction != null;
			if (flag)
			{
				int num = buttons;
				buttons = num + 1;
			}
			bool flag2 = middleBtnAction != null;
			if (flag2)
			{
				int num = buttons;
				buttons = num + 1;
			}
			bool flag3 = rightBtnAction != null;
			if (flag3)
			{
				int num = buttons;
				buttons = num + 1;
			}
			switch (buttons)
			{
			case 1:
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, something, PopupManagerExtensions.QMConfirmButtons.Button_Yes, middleBtnText, middleBtnAction);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(false);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_No)).gameObject.SetActive(false);
				break;
			case 2:
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, something, PopupManagerExtensions.QMConfirmButtons.Button_Yes, leftBtnText, leftBtnAction);
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, something, PopupManagerExtensions.QMConfirmButtons.Button_No, rightBtnText, rightBtnAction);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(false);
				break;
			case 3:
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, something, PopupManagerExtensions.QMConfirmButtons.Button_Yes, leftBtnText, leftBtnAction);
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, something, PopupManagerExtensions.QMConfirmButtons.Button_YesAlt, middleBtnText, middleBtnAction);
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, something, PopupManagerExtensions.QMConfirmButtons.Button_No, rightBtnText, rightBtnAction);
				break;
			default:
				middleBtnAction = delegate()
				{
				};
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, something, PopupManagerExtensions.QMConfirmButtons.Button_Yes, middleBtnText, middleBtnAction);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(false);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_No)).gameObject.SetActive(false);
				break;
			}
			bool flag4 = icon != null;
			if (flag4)
			{
				iconObj.gameObject.SetActive(true);
				ImageEx iconCom = iconObj.GetComponent<ImageEx>();
				((Image)iconCom).overrideSprite = icon;
				iconCom = null;
			}
			Transform titleObj = confirmmObj.Find("QMHeader_H2/LeftItemContainer/Text_Title");
			TextMeshProUGUI titleCom = titleObj.GetComponent<TextMeshProUGUI>();
			titleCom.text = title;
			Transform contentObj = confirmmObj.Find("Text_Body");
			TextMeshProUGUI contentCom = contentObj.GetComponent<TextMeshProUGUI>();
			contentCom.text = content;
			yield break;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000AD47 File Offset: 0x00008F47
		private static IEnumerator QMConfirmIgnoredWait(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something)
		{
			while (object.Equals(something.Method_Public_UIPage_0().Method_Public_get_Boolean_1(), true))
			{
				yield return null;
			}
			PopupManagerExtensions.QMConfirmPopupCleanUp(confirmmObj, iconObj, something, false);
			yield break;
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000AD64 File Offset: 0x00008F64
		private static void SetUpQMConfirmButton(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something, PopupManagerExtensions.QMConfirmButtons qmConfirmButtons, string BtnText, Action BtnAction)
		{
			Transform transform = PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), qmConfirmButtons));
			transform.gameObject.SetActive(true);
			Transform transform2 = transform.Find("Text_MM_H3");
			TextMeshProUGUI component = transform2.GetComponent<TextMeshProUGUI>();
			Button component2 = transform.GetComponent<Button>();
			component.text = BtnText;
			UnityAction unityAction = DelegateSupport.ConvertDelegate<UnityAction>(new Action(delegate()
			{
				PopupManagerExtensions.QMConfirmPopupCleanUp(confirmmObj, iconObj, something, true);
			}));
			component2.onClick.RemoveListener(unityAction);
			component2.onClick.AddListener(unityAction);
			foreach (UnityAction unityAction2 in PopupManagerExtensions.QMConfirmActionsActive)
			{
				component2.onClick.RemoveListener(unityAction2);
			}
			PopupManagerExtensions.QMConfirmActionsActive.Add(BtnAction);
			component2.onClick.AddListener(BtnAction);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000AE90 File Offset: 0x00009090
		private static void QMConfirmPopupCleanUp(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something, bool closePopup = false)
		{
			if (closePopup)
			{
				something.Method_Private_Void_0();
			}
			confirmmObj.Find("Buttons").gameObject.SetActive(true);
			PopupManagerExtensions.QMCofirmPopupObj.SetActive(false);
			bool activeInHierarchy = iconObj.gameObject.activeInHierarchy;
			if (activeInHierarchy)
			{
				ImageEx component = iconObj.GetComponent<ImageEx>();
				((Image)component).overrideSprite = null;
				iconObj.gameObject.SetActive(false);
				PopupManagerExtensions.QMConfirmButtonIsReady = true;
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000AF08 File Offset: 0x00009108
		public static void InputPopup(string Title, Action<string> EndString, Action<string> RealTimeString = null, Action OnClose = null, PopupManagerExtensions.KeyBoardType keyBoardType = PopupManagerExtensions.KeyBoardType.Standard, string Placeholder = "Enter Text", string OkButton = "OK", string CancelButton = "Cancel", bool MultiLine = true, int CharLimit = 0, bool KeepOpen = false, bool ReadOnly = false)
		{
			bool flag = PopupManagerExtensions._KeyboardComponent == null;
			if (flag)
			{
				PopupManagerExtensions.keyboardGameObject = new GameObject("KernellClientUI_KeyBoard");
				Object.DontDestroyOnLoad(PopupManagerExtensions.keyboardGameObject);
				PopupManagerExtensions._KeyboardComponent = PopupManagerExtensions.keyboardGameObject.AddComponent<VRCInputField>();
			}
			try
			{
				KeyboardData keyboardData = new KeyboardData();
				KeyboardData keyboardData2 = keyboardData.Method_Public_KeyboardData_LocalizableString_LocalizableString_String_LocalizableString_LocalizableString_0(LocalizableStringExtensions.Localize(Title, null, null, null), LocalizableStringExtensions.Localize(Placeholder, null, null, null), "", LocalizableStringExtensions.Localize(OkButton, null, null, null), LocalizableStringExtensions.Localize(CancelButton, null, null, null));
				KeyboardData keyboardData3 = keyboardData2.Method_Public_KeyboardData_Action_1_String_Action_1_String_Action_Boolean_PDM_0(RealTimeString, EndString, OnClose, KeepOpen);
				KeyboardData keyboardData4 = keyboardData3.Method_Public_KeyboardData_InputPopupType_Boolean_PDM_0(keyBoardType, true);
				KeyboardData keyboardData5 = keyboardData4.Method_Public_KeyboardData_InputType_ContentType_Int32_Boolean_Boolean_InterfacePublicAbstractBoStVoAc1VoAcSt1BoUnique_PDM_0(0, 0, CharLimit, MultiLine, ReadOnly, null);
				keyboardData5._isWorldKeyboard = true;
				PopupManagerExtensions._KeyboardComponent.Method_Private_Void_0();
			}
			catch
			{
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000AFF0 File Offset: 0x000091F0
		[CompilerGenerated]
		internal static IEnumerator <.cctor>g__wait|7_0()
		{
			while (MenuEx.QMenuParent == null)
			{
				yield return null;
			}
			Transform originalButtons = MenuEx.QMenuParent.Find("Modal_ConfirmDialog/MenuPanel/Buttons");
			while (originalButtons == null)
			{
				yield return null;
			}
			PopupManagerExtensions.QMCofirmPopupObj = Object.Instantiate<GameObject>(originalButtons.gameObject, originalButtons.parent);
			PopupManagerExtensions.QMCofirmPopupObj.gameObject.SetActive(false);
			yield break;
		}

		// Token: 0x040000B6 RID: 182
		private static VRCInputField _KeyboardComponent;

		// Token: 0x040000B7 RID: 183
		private static GameObject keyboardGameObject;

		// Token: 0x040000B8 RID: 184
		private static GameObject QMCofirmPopupObj;

		// Token: 0x040000B9 RID: 185
		private static readonly List<UnityAction> QMConfirmActionsActive = new List<UnityAction>();

		// Token: 0x040000BA RID: 186
		private static bool QMConfirmButtonIsReady = true;

		// Token: 0x020000D6 RID: 214
		private enum QMConfirmButtons
		{
			// Token: 0x0400059A RID: 1434
			Button_Yes,
			// Token: 0x0400059B RID: 1435
			Button_YesAlt,
			// Token: 0x0400059C RID: 1436
			Button_No
		}

		// Token: 0x020000D7 RID: 215
		public enum KeyBoardType
		{
			// Token: 0x0400059E RID: 1438
			Standard,
			// Token: 0x0400059F RID: 1439
			Numeric,
			// Token: 0x040005A0 RID: 1440
			Search
		}
	}
}
