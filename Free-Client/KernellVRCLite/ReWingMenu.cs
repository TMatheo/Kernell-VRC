using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.UI;
using KernellClientUI.UI.Wings;
using KernellClientUI.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Controls;
using VRC.UI.Elements;

// Token: 0x0200000F RID: 15
public class ReWingMenu : UiElement
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600003C RID: 60 RVA: 0x00002F50 File Offset: 0x00001150
	private static GameObject WingMenuPrefab
	{
		get
		{
			bool flag = ReWingMenu._wingMenuPrefab == null;
			if (flag)
			{
				ReWingMenu._wingMenuPrefab = MenuEx.QMLeftWing.transform.Find("Container/InnerContainer/WingMenu").gameObject;
			}
			return ReWingMenu._wingMenuPrefab;
		}
	}

	// Token: 0x1700000D RID: 13
	// (get) Token: 0x0600003D RID: 61 RVA: 0x00002F96 File Offset: 0x00001196
	public Transform Container { get; }

	// Token: 0x0600003E RID: 62 RVA: 0x00002FA0 File Offset: 0x000011A0
	public ReWingMenu(string text, bool left = true) : base(ReWingMenu.WingMenuPrefab, (left ? MenuEx.QMLeftWing : MenuEx.QMRightWing).transform.Find("Container/InnerContainer/"), text, false)
	{
		this._menuName = UiElement.GetCleanName(text);
		this.WingType = left;
		this._wing = (this.WingType ? MenuEx.QMLeftWing : MenuEx.QMRightWing);
		Transform child = base.RectTransform.GetChild(0);
		TextMeshProUGUI componentInChildren = child.GetComponentInChildren<TextMeshProUGUI>();
		componentInChildren.text = text;
		componentInChildren.richText = true;
		Button componentInChildren2 = child.GetComponentInChildren<Button>(true);
		componentInChildren2.gameObject.SetActive(true);
		Transform transform = componentInChildren2.transform.Find("Icon");
		transform.gameObject.SetActive(true);
		List<Behaviour> list = new List<Behaviour>();
		componentInChildren2.GetComponents<Behaviour>(list);
		List<Behaviour>.Enumerator enumerator = list.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Behaviour current = enumerator._current;
			current.enabled = true;
		}
		RectTransform content = base.RectTransform.GetComponentInChildren<ScrollRect>().content;
		IEnumerator enumerator2 = content.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				Object @object = enumerator2.Current;
				Transform transform2 = @object.Cast<Transform>();
				bool flag = !(transform2 == null);
				if (flag)
				{
					Object.Destroy(transform2.gameObject);
				}
			}
		}
		finally
		{
			IDisposable disposable = enumerator2 as IDisposable;
			bool flag2 = disposable != null;
			if (flag2)
			{
				disposable.Dispose();
			}
		}
		this.Container = content;
		MenuStateController component = this._wing.GetComponent<MenuStateController>();
		UIPage component2 = base.GameObject.GetComponent<UIPage>();
		component2.field_Public_String_0 = this._menuName;
		component2.field_Private_List_1_UIPage_0 = new List<UIPage>();
		List<MenuStateController> list2 = new List<MenuStateController>();
		list2.Add(MenuEx.QMenuStateCtrl);
	}

	// Token: 0x0600003F RID: 63 RVA: 0x00003180 File Offset: 0x00001380
	public void Open()
	{
		bool flag = !base.GameObject;
		if (flag)
		{
			throw new NullReferenceException("This wing menu has been destroyed.");
		}
		this._wing.GetComponent<MenuStateController>().Method_Public_Void_String_UIContext_Boolean_EnumNPublicSealedvaNoLeRiBoIn6vUnique_0(this._menuName, null, false, 0);
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000031C8 File Offset: 0x000013C8
	public ReWingButton AddButton(string text, string tooltip, Action onClick, Sprite sprite = null, bool arrow = true, bool background = true, bool separator = false)
	{
		bool flag = !base.GameObject;
		if (flag)
		{
			throw new NullReferenceException("This wing menu has been destroyed.");
		}
		return new ReWingButton(text, tooltip, onClick, this.Container, sprite, arrow, background, separator);
	}

	// Token: 0x06000041 RID: 65 RVA: 0x00003210 File Offset: 0x00001410
	public ReWingToggle AddToggle(string text, string tooltip, Action<bool> onToggle, bool defaultValue = false)
	{
		bool flag = !base.GameObject;
		if (flag)
		{
			throw new NullReferenceException("This wing menu has been destroyed.");
		}
		return new ReWingToggle(text, tooltip, onToggle, this.Container, defaultValue);
	}

	// Token: 0x06000042 RID: 66 RVA: 0x00003250 File Offset: 0x00001450
	public ReWingMenu AddSubMenu(string text, string tooltip)
	{
		bool flag = !base.GameObject;
		if (flag)
		{
			throw new NullReferenceException("This wing menu has been destroyed.");
		}
		ReWingMenu reWingMenu = new ReWingMenu(text, this.WingType);
		this.AddButton(text, tooltip, new Action(reWingMenu.Open), null, true, true, false);
		return reWingMenu;
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000032A8 File Offset: 0x000014A8
	public override void Destroy()
	{
		bool flag = base.GameObject;
		if (flag)
		{
			MenuStateController component = this._wing.GetComponent<MenuStateController>();
			UIPage component2 = base.GameObject.GetComponent<UIPage>();
			component.field_Private_Dictionary_2_String_UIPage_0.Remove(component2.field_Public_String_0);
			Object.Destroy(base.GameObject);
		}
	}

	// Token: 0x04000016 RID: 22
	private static GameObject _wingMenuPrefab;

	// Token: 0x04000017 RID: 23
	private readonly bool WingType;

	// Token: 0x04000018 RID: 24
	private readonly GameObject _wing;

	// Token: 0x04000019 RID: 25
	private readonly string _menuName;
}
