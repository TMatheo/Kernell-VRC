using System;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;
using UnityEngine;
using UnityEngine.InputSystem;

// Token: 0x02000010 RID: 16
public class clicktp : KernelModuleBase
{
	// Token: 0x1700000E RID: 14
	// (get) Token: 0x06000044 RID: 68 RVA: 0x000032FD File Offset: 0x000014FD
	public override string ModuleName
	{
		get
		{
			return "ClickTP";
		}
	}

	// Token: 0x1700000F RID: 15
	// (get) Token: 0x06000045 RID: 69 RVA: 0x00003304 File Offset: 0x00001504
	public override string Version
	{
		get
		{
			return "2.0.0";
		}
	}

	// Token: 0x17000010 RID: 16
	// (get) Token: 0x06000046 RID: 70 RVA: 0x0000330B File Offset: 0x0000150B
	public override ModuleCapabilities Capabilities
	{
		get
		{
			return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.UIInit;
		}
	}

	// Token: 0x17000011 RID: 17
	// (get) Token: 0x06000047 RID: 71 RVA: 0x00003312 File Offset: 0x00001512
	public override UpdateFrequency UpdateFrequency
	{
		get
		{
			return UpdateFrequency.Every2Frames;
		}
	}

	// Token: 0x17000012 RID: 18
	// (get) Token: 0x06000048 RID: 72 RVA: 0x00003315 File Offset: 0x00001515
	public override ModulePriority Priority
	{
		get
		{
			return ModulePriority.Normal;
		}
	}

	// Token: 0x06000049 RID: 73 RVA: 0x0000331C File Offset: 0x0000151C
	public override void OnUiManagerInit()
	{
		try
		{
			ReMenuCategory category = MenuSetup._uiManager.QMMenu.GetCategoryPage("Utility").GetCategory("Movement");
			category.AddToggle("Click TP", "Teleport with a specific input", delegate(bool isEnabled)
			{
				this.clicktpEnabled = isEnabled;
			});
			category.AddToggle("Laptop Mode", "Ctrl+Space to teleport", delegate(bool isEnabled)
			{
				this.laptopMode = isEnabled;
			});
			category.AddToggle("VR Mode", "Both thumbsticks to teleport", delegate(bool isEnabled)
			{
				this.vrModeEnabled = isEnabled;
			});
		}
		catch (Exception ex)
		{
			kernelllogger.Error("ClickTP UI setup failed: " + ex.Message);
		}
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000033D0 File Offset: 0x000015D0
	public override void OnLateUpdate()
	{
		bool flag = !this.clicktpEnabled;
		if (!flag)
		{
			try
			{
				bool flag2 = this.laptopMode && this.CheckLaptopInput();
				if (flag2)
				{
					this.DoTeleport();
				}
				else
				{
					bool flag3 = this.vrModeEnabled && this.CheckVRInput();
					if (flag3)
					{
						this.DoTeleport();
					}
					else
					{
						bool flag4 = !this.laptopMode && !this.vrModeEnabled && this.CheckMouseInput();
						if (flag4)
						{
							this.DoTeleport();
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("ClickTP update error: " + ex.Message);
			}
		}
	}

	// Token: 0x0600004B RID: 75 RVA: 0x00003488 File Offset: 0x00001688
	private bool CheckLaptopInput()
	{
		Keyboard current = Keyboard.current;
		return current != null && current.spaceKey.wasPressedThisFrame && (current.leftCtrlKey.isPressed || current.rightCtrlKey.isPressed);
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000034D0 File Offset: 0x000016D0
	private bool CheckVRInput()
	{
		Gamepad current = Gamepad.current;
		return current != null && current.leftStickButton.wasPressedThisFrame && current.rightStickButton.isPressed;
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00003508 File Offset: 0x00001708
	private bool CheckMouseInput()
	{
		Mouse current = Mouse.current;
		return current != null && current.forwardButton.wasPressedThisFrame;
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00003530 File Offset: 0x00001730
	private void DoTeleport()
	{
		try
		{
			bool flag = Camera.main == null;
			if (flag)
			{
				kernelllogger.Warning("No main camera found for teleport");
			}
			else
			{
				Mouse current = Mouse.current;
				Vector2 vector = (current != null) ? current.position.ReadValue() : new Vector2((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
				Ray ray = Camera.main.ScreenPointToRay(vector);
				RaycastHit raycastHit;
				bool flag2 = Physics.Raycast(ray, ref raycastHit, 1000f);
				if (flag2)
				{
					VRCPlayer field_Internal_Static_VRCPlayer_ = VRCPlayer.field_Internal_Static_VRCPlayer_0;
					bool flag3 = ((field_Internal_Static_VRCPlayer_ != null) ? field_Internal_Static_VRCPlayer_.transform : null) != null;
					if (flag3)
					{
						field_Internal_Static_VRCPlayer_.transform.position = raycastHit.point + Vector3.up * 0.1f;
						kernelllogger.Msg(string.Format("Teleported to {0}", raycastHit.point));
					}
					else
					{
						kernelllogger.Warning("VRCPlayer not found for teleport");
					}
				}
			}
		}
		catch (Exception ex)
		{
			kernelllogger.Error("Teleport failed: " + ex.Message);
		}
	}

	// Token: 0x0400001B RID: 27
	private bool clicktpEnabled = false;

	// Token: 0x0400001C RID: 28
	private bool laptopMode = false;

	// Token: 0x0400001D RID: 29
	private bool vrModeEnabled = false;
}
