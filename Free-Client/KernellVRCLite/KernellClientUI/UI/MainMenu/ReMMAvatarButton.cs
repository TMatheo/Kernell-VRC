using System;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC.Ui;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x0200005A RID: 90
	public class ReMMAvatarButton
	{
		// Token: 0x060003E9 RID: 1001 RVA: 0x00016810 File Offset: 0x00014A10
		public ReMMAvatarButton(string name, string tooltip, Action onClick, Sprite icon, Transform parent, string color = "#ffffff")
		{
			ReMMAvatarButton.<>c__DisplayClass2_0 CS$<>8__locals1 = new ReMMAvatarButton.<>c__DisplayClass2_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.name = name;
			base..ctor();
			ReMMAvatarButton.<>c__DisplayClass2_1 CS$<>8__locals2 = new ReMMAvatarButton.<>c__DisplayClass2_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			GameObject gameObject = Object.Instantiate<Transform>(MMenuPrefabs.MMAvatarButton.transform, parent).gameObject;
			gameObject.name = "MMAviButton_" + UiElement.GetCleanName(CS$<>8__locals2.CS$<>8__locals1.name);
			CS$<>8__locals2.txt = gameObject.transform.Find("Text_ButtonName").GetComponent<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|0());
			CS$<>8__locals2.txt.richText = true;
			this.background = gameObject.transform.Find("Background_Button").GetComponent<ImageEx>();
			HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup.childAlignment = 5;
			ContentSizeFitter contentSizeFitter = gameObject.transform.Find("Background_Button").gameObject.AddComponent<ContentSizeFitter>();
			contentSizeFitter.horizontalFit = 2;
			contentSizeFitter.verticalFit = 2;
			ContentSizeFitter contentSizeFitter2 = gameObject.AddComponent<ContentSizeFitter>();
			HorizontalLayoutGroup horizontalLayoutGroup2 = gameObject.transform.Find("Background_Button").gameObject.AddComponent<HorizontalLayoutGroup>();
			horizontalLayoutGroup2.childControlWidth = true;
			horizontalLayoutGroup2.childControlHeight = true;
			horizontalLayoutGroup2.childAlignment = 4;
			horizontalLayoutGroup2.padding = new RectOffset(25, 25, 17, 17);
			contentSizeFitter2.horizontalFit = 2;
			contentSizeFitter2.verticalFit = 2;
			gameObject.transform.Find("Text_ButtonName/Icon").gameObject.SetActive(icon != null);
			Object.Instantiate<GameObject>(gameObject.transform.Find("Text_ButtonName").gameObject, gameObject.transform.Find("Background_Button"));
			Object.Destroy(gameObject.transform.Find("Text_ButtonName").gameObject);
			this.btn = gameObject.GetComponent<Button>();
			this.btn.onClick.RemoveAllListeners();
			this.btn.onClick.AddListener(new Action(onClick.Invoke));
		}

		// Token: 0x04000199 RID: 409
		public Button btn;

		// Token: 0x0400019A RID: 410
		public ImageEx background;
	}
}
