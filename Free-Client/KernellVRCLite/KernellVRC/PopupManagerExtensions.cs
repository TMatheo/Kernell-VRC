using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Il2CppSystem;
using KernellClientUI.VRChat;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRC.DataModel;
using VRC.Localization;
using VRC.Ui;

namespace KernellVRC
{
	// Token: 0x02000072 RID: 114
	[NullableContext(1)]
	[Nullable(0)]
	public static class PopupManagerExtensions
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x0001BBFD File Offset: 0x00019DFD
		static PopupManagerExtensions()
		{
			MelonCoroutines.Start(PopupManagerExtensions.<.cctor>g__wait|5_0());
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001BC1C File Offset: 0x00019E1C
		public static void Alert1Box(string title, string content, Action middleBtnAction = null, string middleBtnText = "Okay", Sprite icon = null)
		{
			Action middleBtnAction2 = middleBtnAction;
			if (middleBtnAction == null && (middleBtnAction2 = PopupManagerExtensions.<>c.<>9__6_0) == null)
			{
				middleBtnAction2 = (PopupManagerExtensions.<>c.<>9__6_0 = delegate()
				{
				});
			}
			MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction2, null, null, middleBtnText, "", "", icon));
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001BC6C File Offset: 0x00019E6C
		public static void Alert2Box(string title, string content, Action leftBtnAction = null, Action rightBtnAction = null, string leftBtnText = "Yes", string rightBtnText = "No", Sprite icon = null)
		{
			bool flag = rightBtnText == "" && rightBtnAction == null;
			if (flag)
			{
				Action middleBtnAction = leftBtnAction;
				if (leftBtnAction == null && (middleBtnAction = PopupManagerExtensions.<>c.<>9__7_0) == null)
				{
					middleBtnAction = (PopupManagerExtensions.<>c.<>9__7_0 = delegate()
					{
					});
				}
				MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction, null, null, leftBtnText, "", "", icon));
			}
			else
			{
				Action middleBtnAction2 = null;
				Action leftBtnAction2 = leftBtnAction;
				if (leftBtnAction == null && (leftBtnAction2 = PopupManagerExtensions.<>c.<>9__7_1) == null)
				{
					leftBtnAction2 = (PopupManagerExtensions.<>c.<>9__7_1 = delegate()
					{
					});
				}
				Action rightBtnAction2 = rightBtnAction;
				if (rightBtnAction == null && (rightBtnAction2 = PopupManagerExtensions.<>c.<>9__7_2) == null)
				{
					rightBtnAction2 = (PopupManagerExtensions.<>c.<>9__7_2 = delegate()
					{
					});
				}
				MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction2, leftBtnAction2, rightBtnAction2, "", leftBtnText, rightBtnText, icon));
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001BD38 File Offset: 0x00019F38
		public static void Alert3Box(string title, string content, Action leftBtnAction = null, Action middleBtnAction = null, Action rightBtnAction = null, string leftBtnText = "Yes", string middleBtnText = "Maybe", string rightBtnText = "No", Sprite icon = null)
		{
			Action middleBtnAction2 = middleBtnAction;
			if (middleBtnAction == null && (middleBtnAction2 = PopupManagerExtensions.<>c.<>9__8_0) == null)
			{
				middleBtnAction2 = (PopupManagerExtensions.<>c.<>9__8_0 = delegate()
				{
				});
			}
			Action leftBtnAction2 = leftBtnAction;
			if (leftBtnAction == null && (leftBtnAction2 = PopupManagerExtensions.<>c.<>9__8_1) == null)
			{
				leftBtnAction2 = (PopupManagerExtensions.<>c.<>9__8_1 = delegate()
				{
				});
			}
			Action rightBtnAction2 = rightBtnAction;
			if (rightBtnAction == null && (rightBtnAction2 = PopupManagerExtensions.<>c.<>9__8_2) == null)
			{
				rightBtnAction2 = (PopupManagerExtensions.<>c.<>9__8_2 = delegate()
				{
				});
			}
			MelonCoroutines.Start(PopupManagerExtensions.TriggerQMConfirm(title, content, middleBtnAction2, leftBtnAction2, rightBtnAction2, middleBtnText, leftBtnText, rightBtnText, icon));
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001BDC8 File Offset: 0x00019FC8
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
			MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique component = confirmmObj.parent.GetComponent<MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique>();
			int num = 0;
			MelonCoroutines.Start(PopupManagerExtensions.QMConfirmIgnoredWait(confirmmObj, iconObj, component));
			confirmmObj.Find("Buttons").gameObject.SetActive(false);
			PopupManagerExtensions.QMCofirmPopupObj.SetActive(true);
			bool flag = leftBtnAction != null;
			if (flag)
			{
				int num2 = num + 1;
				num = num2;
			}
			bool flag2 = middleBtnAction != null;
			if (flag2)
			{
				int num2 = num + 1;
				num = num2;
			}
			bool flag3 = rightBtnAction != null;
			if (flag3)
			{
				int num2 = num + 1;
				num = num2;
			}
			switch (num)
			{
			case 1:
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, component, PopupManagerExtensions.QMConfirmButtons.Button_Yes, middleBtnText, middleBtnAction);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(false);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_No)).gameObject.SetActive(false);
				break;
			case 2:
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, component, PopupManagerExtensions.QMConfirmButtons.Button_Yes, leftBtnText, leftBtnAction);
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, component, PopupManagerExtensions.QMConfirmButtons.Button_No, rightBtnText, rightBtnAction);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(false);
				break;
			case 3:
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, component, PopupManagerExtensions.QMConfirmButtons.Button_Yes, leftBtnText, leftBtnAction);
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, component, PopupManagerExtensions.QMConfirmButtons.Button_YesAlt, middleBtnText, middleBtnAction);
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, component, PopupManagerExtensions.QMConfirmButtons.Button_No, rightBtnText, rightBtnAction);
				break;
			default:
				middleBtnAction = delegate()
				{
				};
				PopupManagerExtensions.SetUpQMConfirmButton(confirmmObj, iconObj, component, PopupManagerExtensions.QMConfirmButtons.Button_Yes, middleBtnText, middleBtnAction);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_YesAlt)).gameObject.SetActive(false);
				PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), PopupManagerExtensions.QMConfirmButtons.Button_No)).gameObject.SetActive(false);
				break;
			}
			bool flag4 = icon != null;
			if (flag4)
			{
				iconObj.gameObject.SetActive(true);
				iconObj.GetComponent<ImageEx>().overrideSprite = icon;
			}
			confirmmObj.Find("QMHeader_H2/LeftItemContainer/Text_Title").GetComponent<TextMeshProUGUIEx>().text = title;
			confirmmObj.Find("Text_Body").GetComponent<TextMeshProUGUIEx>().text = content;
			yield break;
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001BE1F File Offset: 0x0001A01F
		private static IEnumerator QMConfirmIgnoredWait(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something)
		{
			while (something.isActiveAndEnabled)
			{
				yield return null;
			}
			PopupManagerExtensions.QMConfirmPopupCleanUp(confirmmObj, iconObj, something, false);
			yield break;
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001BE3C File Offset: 0x0001A03C
		private static void SetUpQMConfirmButton(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something, PopupManagerExtensions.QMConfirmButtons qmConfirmButtons, string BtnText, Action BtnAction)
		{
			Transform transform = PopupManagerExtensions.QMCofirmPopupObj.transform.Find(Enum.GetName(typeof(PopupManagerExtensions.QMConfirmButtons), qmConfirmButtons));
			transform.gameObject.SetActive(true);
			TextMeshProUGUIEx component = transform.Find("Text_MM_H3").GetComponent<TextMeshProUGUIEx>();
			Button component2 = transform.GetComponent<Button>();
			component.text = BtnText;
			Action action = delegate()
			{
				PopupManagerExtensions.QMConfirmPopupCleanUp(confirmmObj, iconObj, something, true);
			};
			UnityAction unityAction = DelegateSupport.ConvertDelegate<UnityAction>(action);
			component2.onClick.RemoveListener(unityAction);
			component2.onClick.AddListener(unityAction);
			foreach (UnityAction unityAction2 in PopupManagerExtensions.QMConfirmActionsActive)
			{
				component2.onClick.RemoveListener(unityAction2);
			}
			UnityAction unityAction3 = DelegateSupport.ConvertDelegate<UnityAction>(BtnAction);
			PopupManagerExtensions.QMConfirmActionsActive.Add(unityAction3);
			component2.onClick.AddListener(unityAction3);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001BF64 File Offset: 0x0001A164
		private static void QMConfirmPopupCleanUp(Transform confirmmObj, Transform iconObj, MonoBehaviour1PublicObBuObGaBuToGrBuImTrUnique something, bool closePopup = false)
		{
			if (closePopup)
			{
				something.Method_Private_Void_0();
			}
			confirmmObj.Find("Buttons").gameObject.SetActive(true);
			PopupManagerExtensions.QMCofirmPopupObj.SetActive(false);
			bool flag = !iconObj.gameObject.activeInHierarchy;
			if (!flag)
			{
				iconObj.GetComponent<ImageEx>().overrideSprite = null;
				iconObj.gameObject.SetActive(false);
				PopupManagerExtensions.QMConfirmButtonIsReady = true;
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001BFD8 File Offset: 0x0001A1D8
		public static void InputPopup(string mainText, Action<string> stringAction, PopupManagerExtensions.KeyBoardType keyBoardType = PopupManagerExtensions.KeyBoardType.Standard)
		{
			KeyboardData keyboardData = new KeyboardData();
			keyboardData.Method_Public_KeyboardData_LocalizableString_LocalizableString_String_LocalizableString_LocalizableString_0(LocalizableStringExtensions.Localize(mainText, null, null, null), LocalizableStringExtensions.Localize("Input Text here....", null, null, null), "", LocalizableStringExtensions.Localize("Submit", null, null, null), LocalizableStringExtensions.Localize("Cancel", null, null, null));
			Action<string> action = DelegateSupport.ConvertDelegate<Action<string>>(stringAction);
			keyboardData.Method_Public_KeyboardData_Action_1_String_Action_1_String_Action_Boolean_PDM_0(null, action, null, false);
			keyboardData.Method_Public_KeyboardData_InputPopupType_Boolean_PDM_0(keyBoardType, true);
			keyboardData.Method_Public_KeyboardData_InputType_ContentType_Int32_Boolean_Boolean_InterfacePublicAbstractBoStVoAc1VoAcSt1BoUnique_PDM_0(0, 0, 0, false, false, null);
			keyboardData._isWorldKeyboard = true;
			PopupManagerExtensions._KeyboardComponent._keyboardData = keyboardData;
			PopupManagerExtensions._KeyboardComponent.Method_Private_Void_0();
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001C070 File Offset: 0x0001A270
		[CompilerGenerated]
		internal static IEnumerator <.cctor>g__wait|5_0()
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

		// Token: 0x04000205 RID: 517
		private static MonoBehaviour1PublicTe_p_dTeGa_cKe_kBo_mUnique _KeyboardComponent;

		// Token: 0x04000206 RID: 518
		private static GameObject keyboardGameObject;

		// Token: 0x04000207 RID: 519
		private static GameObject QMCofirmPopupObj;

		// Token: 0x04000208 RID: 520
		private static readonly List<UnityAction> QMConfirmActionsActive = new List<UnityAction>();

		// Token: 0x04000209 RID: 521
		private static bool QMConfirmButtonIsReady = true;

		// Token: 0x0200011B RID: 283
		[NullableContext(0)]
		private enum QMConfirmButtons
		{
			// Token: 0x0400065C RID: 1628
			Button_Yes,
			// Token: 0x0400065D RID: 1629
			Button_YesAlt,
			// Token: 0x0400065E RID: 1630
			Button_No
		}

		// Token: 0x0200011C RID: 284
		[NullableContext(0)]
		public enum KeyBoardType
		{
			// Token: 0x04000660 RID: 1632
			Standard,
			// Token: 0x04000661 RID: 1633
			Numeric,
			// Token: 0x04000662 RID: 1634
			Search
		}
	}
}
