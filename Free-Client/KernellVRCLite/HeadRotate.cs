using System;
using System.Collections;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;
using MelonLoader;
using UnityEngine;

// Token: 0x02000011 RID: 17
public class HeadRotate : KernelModuleBase
{
	// Token: 0x17000013 RID: 19
	// (get) Token: 0x06000053 RID: 83 RVA: 0x00003699 File Offset: 0x00001899
	public override string ModuleName
	{
		get
		{
			return "Head rotate";
		}
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000054 RID: 84 RVA: 0x00003304 File Offset: 0x00001504
	public override string Version
	{
		get
		{
			return "2.0.0";
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000055 RID: 85 RVA: 0x000036A0 File Offset: 0x000018A0
	public override ModuleCapabilities Capabilities
	{
		get
		{
			return ModuleCapabilities.GUI | ModuleCapabilities.PlayerEvents | ModuleCapabilities.WorldEvents | ModuleCapabilities.NetworkEvents | ModuleCapabilities.MenuEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit | ModuleCapabilities.UserLogin;
		}
	}

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000056 RID: 86 RVA: 0x000036A7 File Offset: 0x000018A7
	public override UpdateFrequency UpdateFrequency
	{
		get
		{
			return UpdateFrequency.Every30Frames;
		}
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000057 RID: 87 RVA: 0x00003315 File Offset: 0x00001515
	public override ModulePriority Priority
	{
		get
		{
			return ModulePriority.Normal;
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x000036AC File Offset: 0x000018AC
	public override void OnUiManagerInit()
	{
		ReMenuCategory category = MenuSetup._uiManager.QMMenu.GetCategoryPage("Movement").GetCategory("Camera");
		category.AddToggle("Manual Rotate", "Enable manual rotation via Ctrl + Scroll Wheel", delegate(bool isEnabled)
		{
			this.manualRotateEnabled = isEnabled;
			bool flag = this.manualRotateEnabled;
			if (flag)
			{
				bool flag2 = this.manualRotateCoroutine == null;
				if (flag2)
				{
					this.manualRotateCoroutine = this.ManualRotationRoutine();
					MelonCoroutines.Start(this.manualRotateCoroutine);
				}
			}
		});
		category.AddButton("Reset Axis", "Reset head rotation to original orientation", new Action(this.ResetAxis), null, "#ff0000");
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0000371C File Offset: 0x0000191C
	public override void OnSceneWasLoaded(int buildIndex, string sceneName)
	{
		this.mainCamera = Camera.main;
		bool flag = this.mainCamera != null;
		if (flag)
		{
			this.originalRotation = this.mainCamera.transform.rotation;
			bool flag2 = this.manualRotateCoroutine == null;
			if (flag2)
			{
				this.manualRotateCoroutine = this.ManualRotationRoutine();
				MelonCoroutines.Start(this.manualRotateCoroutine);
			}
		}
		else
		{
			Debug.LogError("HeadRotate: Main camera not found.");
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003798 File Offset: 0x00001998
	private IEnumerator ManualRotationRoutine()
	{
		for (;;)
		{
			bool flag = this.manualRotateEnabled && this.mainCamera != null;
			if (flag)
			{
				bool key = Input.GetKey(306);
				if (key)
				{
					float scrollInput = Input.GetAxis("Mouse ScrollWheel");
					bool flag2 = Mathf.Abs(scrollInput) >= 0.01f;
					if (flag2)
					{
						float rotationAmount = scrollInput * this.manualRotationSpeed;
						this.mainCamera.transform.Rotate(0f, 0f, rotationAmount, 1);
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x000037A8 File Offset: 0x000019A8
	private void ResetAxis()
	{
		bool flag = this.mainCamera == null;
		if (flag)
		{
			Debug.LogError("HeadRotate: Main camera not found.");
		}
		else
		{
			this.mainCamera.transform.rotation = this.originalRotation;
			this.manualRotateEnabled = false;
			Debug.Log("HeadRotate: Rotation reset to original orientation.");
		}
	}

	// Token: 0x0400001E RID: 30
	private bool manualRotateEnabled = false;

	// Token: 0x0400001F RID: 31
	private readonly float manualRotationSpeed = 100f;

	// Token: 0x04000020 RID: 32
	private Quaternion originalRotation;

	// Token: 0x04000021 RID: 33
	private Camera mainCamera;

	// Token: 0x04000022 RID: 34
	private IEnumerator manualRotateCoroutine;
}
