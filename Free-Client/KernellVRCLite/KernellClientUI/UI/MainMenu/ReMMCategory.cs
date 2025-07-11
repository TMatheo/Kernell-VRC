using System;
using System.Diagnostics;
using KernellClientUI.Unity;
using KernellClientUI.VRChat;
using MelonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Localization;
using VRC.Ui;

namespace KernellClientUI.UI.MainMenu
{
	// Token: 0x0200005C RID: 92
	public class ReMMCategory : UiElement
	{
		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060003F2 RID: 1010 RVA: 0x00016D07 File Offset: 0x00014F07
		// (set) Token: 0x060003F3 RID: 1011 RVA: 0x00016D0F File Offset: 0x00014F0F
		public GameObject ButtonObj { get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060003F4 RID: 1012 RVA: 0x00016D18 File Offset: 0x00014F18
		// (set) Token: 0x060003F5 RID: 1013 RVA: 0x00016D20 File Offset: 0x00014F20
		public GameObject ContainerObj { get; protected set; }

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060003F6 RID: 1014 RVA: 0x00016D2C File Offset: 0x00014F2C
		// (remove) Token: 0x060003F7 RID: 1015 RVA: 0x00016D64 File Offset: 0x00014F64
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action onOpen;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060003F8 RID: 1016 RVA: 0x00016D9C File Offset: 0x00014F9C
		// (remove) Token: 0x060003F9 RID: 1017 RVA: 0x00016DD4 File Offset: 0x00014FD4
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action onClose;

		// Token: 0x060003FA RID: 1018 RVA: 0x00016E0C File Offset: 0x0001500C
		public ReMMCategory(ReMMPage menu, string btnText, string tooltip, Sprite Icon = null, Transform parent = null, string color = "#ffffff")
		{
			ReMMCategory.<>c__DisplayClass16_0 CS$<>8__locals1 = new ReMMCategory.<>c__DisplayClass16_0();
			CS$<>8__locals1.color = color;
			CS$<>8__locals1.btnText = btnText;
			base..ctor(MMenuPrefabs.MMCategoryButtonPrefab, parent ?? menu.GetCategoryButtonContainer(), CS$<>8__locals1.btnText, true);
			ReMMCategory.<>c__DisplayClass16_1 CS$<>8__locals2 = new ReMMCategory.<>c__DisplayClass16_1();
			CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
			CS$<>8__locals2.reMMCategory = this;
			this.Menu = menu;
			this.ButtonObj = base.GameObject;
			this.ButtonObj.name = "CategoryBtn-" + UiElement.GetCleanName(CS$<>8__locals2.CS$<>8__locals1.btnText);
			((Image)this.ButtonObj.transform.Find("Icon").GetComponent<ImageEx>()).overrideSprite = Icon;
			this.ButtonText = this.ButtonObj.transform.Find("Mask/Text_Name").GetComponent<TextMeshProUGUIEx>();
			MelonCoroutines.Start(CS$<>8__locals2.<.ctor>g__Wait|2());
			this.ButtonText.richText = true;
			this.ContainerObj = Object.Instantiate<GameObject>(MMenuPrefabs.MMCategoryContainerPrefab, menu.GetCategoryChildContainer());
			this.ContainerObj.name = "CatetgoryChild-" + UiElement.GetCleanName(CS$<>8__locals2.CS$<>8__locals1.btnText);
			for (int i = this.ContainerObj.transform.childCount - 1; i >= 0; i--)
			{
				Object.Destroy(this.ContainerObj.transform.GetChild(i).gameObject);
			}
			EnableDisableListener enableDisableListener = this.ContainerObj.AddComponent<EnableDisableListener>();
			enableDisableListener.OnEnableEvent += delegate()
			{
				bool flag = CS$<>8__locals2.reMMCategory.onOpen != null;
				if (flag)
				{
					CS$<>8__locals2.reMMCategory.onOpen();
				}
			};
			enableDisableListener.OnDisableEvent += delegate()
			{
				bool flag = CS$<>8__locals2.reMMCategory.onClose != null;
				if (flag)
				{
					CS$<>8__locals2.reMMCategory.onClose();
				}
			};
			ToolTip component = this.ButtonObj.GetComponent<ToolTip>();
			component._alternateLocalizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
			component._localizableString = LocalizableStringExtensions.Localize(tooltip, null, null, null);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00016FE1 File Offset: 0x000151E1
		public ReMMCategory(Transform transform) : base(transform)
		{
			this.Menu = new ReMMPage(transform.parent.parent.parent.parent.parent.parent);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00017018 File Offset: 0x00015218
		public ReMMCategorySection AddnGetSection(string title, bool collapsible = false, string color = "#ffffff")
		{
			return new ReMMCategorySection(title, collapsible, this.ContainerObj.transform, this, color);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00017040 File Offset: 0x00015240
		public ReMMPage GetMenu()
		{
			return this.Menu;
		}

		// Token: 0x040001A0 RID: 416
		protected TextMeshProUGUI ButtonText;

		// Token: 0x040001A1 RID: 417
		protected ReMMPage Menu;
	}
}
