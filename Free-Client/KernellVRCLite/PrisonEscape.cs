using System;
using System.Linq;
using Il2CppSystem;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

// Token: 0x02000016 RID: 22
internal class PrisonEscape : KernelModuleBase
{
	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060000BC RID: 188 RVA: 0x0000540C File Offset: 0x0000360C
	public override string ModuleName
	{
		get
		{
			return "Prison Escape";
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060000BD RID: 189 RVA: 0x00003304 File Offset: 0x00001504
	public override string Version
	{
		get
		{
			return "2.0.0";
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060000BE RID: 190 RVA: 0x00005413 File Offset: 0x00003613
	public override ModuleCapabilities Capabilities
	{
		get
		{
			return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.WorldEvents | ModuleCapabilities.UdonEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x0000541C File Offset: 0x0000361C
	public override void OnUiManagerInit()
	{
		ReMenuCategory category = MenuSetup._uiManager.QMMenu.GetCategoryPage("Exploits").GetCategory("Game Worlds");
		ReMenuCategory reMenuCategory = category.AddCategoryPage("Prison Escape", "", null, "#ffffff").AddCategory("Prison Escape", true, "#ffffff", false);
		reMenuCategory.AddButton("Force Pickup", "", delegate
		{
			this.ForcePickup();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Get all weapons", "", delegate
		{
			this.GetAllWeapons();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give Pistol", "", delegate
		{
			this.GivePistol();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give ShotGun", "", delegate
		{
			this.GiveShotgun();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give SMG", "", delegate
		{
			this.GiveSMG();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give Sniper", "", delegate
		{
			this.GiveSniper();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give Magnum", "", delegate
		{
			this.GiveMagnum();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give M4A1", "", delegate
		{
			this.GiveM4A1();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give Revolver", "", delegate
		{
			this.GiveRevolver();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give Knife", "", delegate
		{
			this.GiveKnife();
		}, null, "#ffffff");
		reMenuCategory.AddButton("Give KeyCard", "", delegate
		{
			this.GiveKeyCard();
		}, null, "#ffffff");
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x000055F4 File Offset: 0x000037F4
	public void SpecificTarget(string udonEvent, string gameObject)
	{
		foreach (GameObject gameObject2 in Resources.FindObjectsOfTypeAll<GameObject>())
		{
			bool flag = gameObject2.name.Contains(gameObject);
			if (flag)
			{
				gameObject2.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, udonEvent);
			}
		}
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00005660 File Offset: 0x00003860
	public void GetAllWeapons()
	{
		kernelllogger.Msg("Gave yourself all weapons");
		PrisonEscape.MurderGive("ShotGun");
		PrisonEscape.MurderGive("Knife");
		PrisonEscape.MurderGive("Pistol");
		PrisonEscape.MurderGive("SMG");
		PrisonEscape.MurderGive("Sniper");
		PrisonEscape.MurderGive("M4A1");
		PrisonEscape.MurderGive("Magnum");
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x000056C6 File Offset: 0x000038C6
	public void GiveKeyCard()
	{
		kernelllogger.Msg("Gave yourself a KeyCard");
		PrisonEscape.MurderGive("Keycard");
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x000056DF File Offset: 0x000038DF
	public void GiveKnife()
	{
		kernelllogger.Msg("Gave yourself a Knife");
		PrisonEscape.MurderGive("Knife");
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x000056F8 File Offset: 0x000038F8
	public void GivePistol()
	{
		kernelllogger.Msg("Gave yourself a Pistol");
		PrisonEscape.MurderGive("Pistol");
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x00005711 File Offset: 0x00003911
	public void GiveShotgun()
	{
		kernelllogger.Msg("Gave yourself a Shotgun");
		PrisonEscape.MurderGive("Shotgun");
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x0000572A File Offset: 0x0000392A
	public void GiveSMG()
	{
		kernelllogger.Msg("Gave yourself a SMG");
		PrisonEscape.MurderGive("SMG");
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x00005743 File Offset: 0x00003943
	public void GiveSniper()
	{
		kernelllogger.Msg("Gave yourself a Sniper");
		PrisonEscape.MurderGive("Sniper");
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x0000575C File Offset: 0x0000395C
	public void GiveMagnum()
	{
		kernelllogger.Msg("Gave yourself a Magnum");
		PrisonEscape.MurderGive("Magnum");
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005775 File Offset: 0x00003975
	public void GiveM4A1()
	{
		kernelllogger.Msg("Gave yourself a M4A1");
		PrisonEscape.MurderGive("M4A1");
	}

	// Token: 0x060000CA RID: 202 RVA: 0x0000578E File Offset: 0x0000398E
	public void GiveRevolver()
	{
		kernelllogger.Msg("Gave yourself a Revolver");
		PrisonEscape.MurderGive("Revolver");
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000057A7 File Offset: 0x000039A7
	public void ForcePickup()
	{
		PrisonEscape.PickupSteal();
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000057B0 File Offset: 0x000039B0
	public static void MurderCheat(string udonEvent)
	{
		foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
		{
			bool flag = gameObject.name.Contains("Game Logic");
			if (flag)
			{
				gameObject.GetComponent<UdonBehaviour>().SendCustomNetworkEvent(0, udonEvent);
			}
		}
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005820 File Offset: 0x00003A20
	public static void MurderGive(string ObjectName)
	{
		foreach (GameObject gameObject in Resources.FindObjectsOfTypeAll<GameObject>())
		{
			bool flag = gameObject.name.Contains(ObjectName) && !gameObject.name.Contains("Give") && !gameObject.name.Contains("Teleport");
			if (flag)
			{
				Networking.SetOwner(VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_get_VRCPlayerApi_0(), gameObject);
				gameObject.transform.position = ((Component)VRCPlayer.field_Internal_Static_VRCPlayer_0).transform.position + new Vector3(0f, 0.1f, 0f);
			}
		}
	}

	// Token: 0x060000CE RID: 206 RVA: 0x000058F8 File Offset: 0x00003AF8
	public static void PickupSteal()
	{
		VRC_Pickup[] array = Enumerable.ToArray<VRC_Pickup>(Resources.FindObjectsOfTypeAll<VRC_Pickup>());
		for (int i = 0; i < array.Length; i++)
		{
			bool flag = ((Component)array[i]).gameObject;
			if (flag)
			{
				array[i].DisallowTheft = false;
			}
		}
		VRC_Pickup[] array2 = Enumerable.ToArray<VRC_Pickup>(Resources.FindObjectsOfTypeAll<VRC_Pickup>());
		for (int j = 0; j < array2.Length; j++)
		{
			bool flag2 = ((Component)array2[j]).gameObject;
			if (flag2)
			{
				array2[j].DisallowTheft = false;
			}
		}
		VRCPickup[] array3 = Enumerable.ToArray<VRCPickup>(Resources.FindObjectsOfTypeAll<VRCPickup>());
		for (int k = 0; k < array3.Length; k++)
		{
			bool flag3 = ((Component)array3[k]).gameObject;
			if (flag3)
			{
				array3[k].DisallowTheft = false;
			}
		}
	}

	// Token: 0x04000049 RID: 73
	private readonly Object[] SyncKill = new Object[]
	{
		"SyncKill"
	};
}
