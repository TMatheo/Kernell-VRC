using System;
using System.Collections.Generic;
using System.Linq;
using KernellClientUI.VRChat;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000055 RID: 85
	public class ReTab : UiElement
	{
		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x060003BD RID: 957 RVA: 0x00015818 File Offset: 0x00013A18
		// (set) Token: 0x060003BE RID: 958 RVA: 0x00015835 File Offset: 0x00013A35
		public string Title
		{
			get
			{
				return this.TextComponent.text;
			}
			set
			{
				this.TextComponent.text = value;
			}
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00015848 File Offset: 0x00013A48
		public ReTab(string title, string color = "#ffffff", Transform parent = null)
		{
			ReTab.<>c__DisplayClass6_0 CS$<>8__locals1 = new ReTab.<>c__DisplayClass6_0();
			CS$<>8__locals1.title = title;
			base..ctor(QMMenuPrefabs.TabPrefab, parent, "Tab_" + CS$<>8__locals1.title, true);
			Object.DestroyImmediate(base.GameObject.GetComponent<MonoBehaviour1PublicBuToBuToUnique>());
			this.TextComponent = base.GameObject.GetComponentInChildren<TextMeshProUGUIEx>();
			this.TextComponent.text = string.Concat(new string[]
			{
				"<color=",
				color,
				">",
				CS$<>8__locals1.title,
				"</color>"
			});
			this.TextComponent.richText = true;
			ReTabContents tabContents = new ReTabContents(CS$<>8__locals1.title, color, parent.parent.parent.GetChild(2));
			this._container = tabContents.RectTransform;
			tabContents.GameObject.SetActive(false);
			Enumerable.First<ReTabContents>(ReTab.tabcontentslist).GameObject.SetActive(true);
			base.RectTransform.GetChild(1).gameObject.transform.Find("NewBadge").gameObject.SetActive(false);
			base.RectTransform.parent.parent.GetChild(1).gameObject.SetActive(false);
			Button component = base.GameObject.GetComponent<Button>();
			component.onClick = new Button.ButtonClickedEvent();
			component.onClick.AddListener(delegate()
			{
				foreach (ReTabContents reTabContents in ReTab.tabcontentslist)
				{
					reTabContents.GameObject.SetActive(false);
				}
				bool flag = tabContents.GameObject.name.Contains(CS$<>8__locals1.title);
				if (flag)
				{
					tabContents.GameObject.SetActive(true);
				}
			});
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00012330 File Offset: 0x00010530
		public ReTab(Transform transform) : base(transform)
		{
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x000159E4 File Offset: 0x00013BE4
		public ReMenuCategory AddCategory(string title, bool collapsible = true)
		{
			return this.GetCategory(title) ?? new ReMenuCategory(title, this._container.GetChild(0).GetChild(0), collapsible, "#ffffff", false);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00015A20 File Offset: 0x00013C20
		public ReMenuCategory AddCategory(string title)
		{
			return this.GetCategory(title) ?? new ReMenuCategory(title, this._container.GetChild(0).GetChild(0), true, "#ffffff", false);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00015A5C File Offset: 0x00013C5C
		public ReMenuCategory AddCategory(string title, bool collapsible = true, string color = "#ffffff")
		{
			return this.GetCategory(title) ?? new ReMenuCategory(title, this._container.GetChild(0).GetChild(0), collapsible, color, false);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00015A94 File Offset: 0x00013C94
		public ReMenuCategory AddCategory(string title, string color = "#ffffff")
		{
			return this.GetCategory(title) ?? new ReMenuCategory(title, this._container.GetChild(0).GetChild(0), true, color, false);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00015ACC File Offset: 0x00013CCC
		public ReMenuCategory GetCategory(string name)
		{
			Transform transform = this._container.GetChild(0).GetChild(0).Find("Header_" + UiElement.GetCleanName(name));
			bool flag = transform == null;
			ReMenuCategory result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ReMenuHeader headerElement = new ReMenuHeader(transform);
				ReMenuButtonContainer container = new ReMenuButtonContainer(this._container.GetChild(0).GetChild(0).Find("Buttons_" + UiElement.GetCleanName(name)));
				result = new ReMenuCategory(headerElement, container);
			}
			return result;
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00015B54 File Offset: 0x00013D54
		public ReMenuSliderCategory AddSliderCategory(string title)
		{
			return this.AddSliderCategory(title, true);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00015B70 File Offset: 0x00013D70
		public ReMenuSliderCategory AddSliderCategory(string title, bool collapsible = true)
		{
			return this.GetSliderCategory(title) ?? new ReMenuSliderCategory(title, this._container.GetChild(0).GetChild(0), collapsible, "#ffffff");
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00015BAC File Offset: 0x00013DAC
		public ReMenuSliderCategory AddSliderCategory(string title, string color = "#ffffff")
		{
			return this.AddSliderCategory(title, true, color);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x00015BC8 File Offset: 0x00013DC8
		public ReMenuSliderCategory AddSliderCategory(string title, bool collapsible = true, string color = "#ffffff")
		{
			return this.GetSliderCategory(title) ?? new ReMenuSliderCategory(title, this._container.GetChild(0).GetChild(0), collapsible, color);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00015C00 File Offset: 0x00013E00
		public ReMenuSliderCategory GetSliderCategory(string name)
		{
			Transform transform = this._container.GetChild(0).GetChild(0).Find("Header_" + UiElement.GetCleanName(name));
			bool flag = transform == null;
			ReMenuSliderCategory result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ReMenuHeader headerElement = new ReMenuHeader(transform);
				ReMenuSliderContainer container = new ReMenuSliderContainer(this._container.GetChild(0).GetChild(0).Find("Sliders_" + UiElement.GetCleanName(name)));
				result = new ReMenuSliderCategory(headerElement, container);
			}
			return result;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00015C88 File Offset: 0x00013E88
		public ReNewMenuCategory AddNewCategory(string title)
		{
			return this.AddNewCategory(title, true);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00015CA4 File Offset: 0x00013EA4
		public ReNewMenuCategory AddNewCategory(string title, bool collapsible = true)
		{
			return this.GetNewCategory(title) ?? new ReNewMenuCategory(title, this._container.GetChild(0).GetChild(0), collapsible, "#ffffff");
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00015CE0 File Offset: 0x00013EE0
		public ReNewMenuCategory AddNewCategory(string title, string color = "#ffffff")
		{
			return this.AddNewCategory(title, true, color);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00015CFC File Offset: 0x00013EFC
		public ReNewMenuCategory AddNewCategory(string title, bool collapsible = true, string color = "#ffffff")
		{
			return this.GetNewCategory(title) ?? new ReNewMenuCategory(title, this._container.GetChild(0).GetChild(0), collapsible, color);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00015D34 File Offset: 0x00013F34
		public ReNewMenuCategory GetNewCategory(string name)
		{
			Transform transform = this._container.GetChild(0).GetChild(0).Find("Header_" + UiElement.GetCleanName(name));
			bool flag = transform == null;
			ReNewMenuCategory result;
			if (flag)
			{
				result = null;
			}
			else
			{
				ReMenuHeader headerElement = new ReMenuHeader(transform);
				ReNewMenuContainer container = new ReNewMenuContainer(this._container.GetChild(0).GetChild(0).Find("Sliders_" + UiElement.GetCleanName(name)));
				result = new ReNewMenuCategory(headerElement, container);
			}
			return result;
		}

		// Token: 0x0400018D RID: 397
		private readonly Transform _container;

		// Token: 0x0400018E RID: 398
		protected TextMeshProUGUIEx TextComponent;

		// Token: 0x0400018F RID: 399
		public static List<ReTabContents> tabcontentslist = new List<ReTabContents>();
	}
}
