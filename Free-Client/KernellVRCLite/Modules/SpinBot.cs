using System;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernellVRCLite.Utils;
using KernelVRC;
using RootMotion.FinalIK;
using UnityEngine;
using VRC;

namespace KernellVRCLite.modules
{
	// Token: 0x020000A0 RID: 160
	public class SpinBot : KernelModuleBase
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600083D RID: 2109 RVA: 0x0003358F File Offset: 0x0003178F
		public override string ModuleName
		{
			get
			{
				return "spinbot";
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00033596 File Offset: 0x00031796
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.PlayerEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000335A0 File Offset: 0x000317A0
		public override void OnUiManagerInit()
		{
			ReMenuCategory reMenuCategory = MenuSetup._uiManager.QMMenu.GetCategoryPage("Utility").AddCategory("SpinBot", true, "#ffffff", false);
			reMenuCategory.AddToggle("Spin Y-Axis", "Spin horizontally (default)", delegate(bool state)
			{
				this.spinYAxis = state;
				this.UpdateSpinState();
			});
			reMenuCategory.AddToggle("Spin X-Axis", "Spin forward/backward", delegate(bool state)
			{
				this.spinXAxis = state;
				this.UpdateSpinState();
			});
			reMenuCategory.AddToggle("Spin Z-Axis", "Spin sideways", delegate(bool state)
			{
				this.spinZAxis = state;
				this.UpdateSpinState();
			});
			reMenuCategory.AddToggle("Reverse Y", "Reverse horizontal spin direction", delegate(bool state)
			{
				this.reverseY = state;
			});
			reMenuCategory.AddToggle("Reverse X", "Reverse forward/backward spin direction", delegate(bool state)
			{
				this.reverseX = state;
			});
			reMenuCategory.AddToggle("Reverse Z", "Reverse sideways spin direction", delegate(bool state)
			{
				this.reverseZ = state;
			});
			reMenuCategory.AddToggle("Disable Animator", "Disables animator (WARNING: Will cause T-pose)", delegate(bool state)
			{
				this.disableAnimator = state;
				this.UpdateAnimatorState();
			});
			reMenuCategory.AddCategoryPage("Spinbot settings", "", null, "#ffffff").AddSliderCategory("Spinbot settins", true, "#ffffff", false).AddSlider("Spin Speed", "Adjust overall spin speed multiplier", delegate(float value)
			{
				this.spinSpeedMultiplier = value;
			}, 1f, 0.1f, 20f, "#ffffff");
			reMenuCategory.AddButton("Stop All Spins", "Reset all spinning to normal", delegate
			{
				this.spinYAxis = false;
				this.spinXAxis = false;
				this.spinZAxis = false;
				this.disableAnimator = false;
				this.ResetSpinState();
			}, null, "#ffffff");
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x0003371C File Offset: 0x0003191C
		private void UpdateSpinState()
		{
			bool flag = this.spinYAxis || this.spinXAxis || this.spinZAxis;
			bool flag2 = !flag;
			if (flag2)
			{
				this.ResetSpinState();
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x00033758 File Offset: 0x00031958
		private void UpdateAnimatorState()
		{
			bool flag = this.animator != null;
			if (flag)
			{
				this.animator.enabled = !this.disableAnimator;
			}
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x00033790 File Offset: 0x00031990
		public override void OnLateUpdate()
		{
			bool flag = this.spinYAxis || this.spinXAxis || this.spinZAxis;
			bool flag2 = !flag;
			if (flag2)
			{
				bool flag3 = this.spinInit;
				if (flag3)
				{
					this.EnableAnimatorAndIK(true);
					this.spinInit = false;
				}
			}
			else
			{
				bool flag4 = !this.spinInit;
				if (flag4)
				{
					this.InitializeSpin();
				}
				bool flag5 = this.avatar == null;
				if (!flag5)
				{
					bool flag6 = this.vrik != null;
					if (flag6)
					{
						this.vrik.enabled = false;
					}
					bool flag7 = this.spinYAxis;
					if (flag7)
					{
						this.currentRotation.y = this.currentRotation.y + (float)(this.reverseY ? -1 : 1) * this.spinSpeedY * this.spinSpeedMultiplier * Time.deltaTime;
						bool flag8 = this.currentRotation.y > 360f;
						if (flag8)
						{
							this.currentRotation.y = this.currentRotation.y - 360f;
						}
						else
						{
							bool flag9 = this.currentRotation.y < 0f;
							if (flag9)
							{
								this.currentRotation.y = this.currentRotation.y + 360f;
							}
						}
					}
					bool flag10 = this.spinXAxis;
					if (flag10)
					{
						this.currentRotation.x = this.currentRotation.x + (float)(this.reverseX ? -1 : 1) * this.spinSpeedX * this.spinSpeedMultiplier * Time.deltaTime;
						bool flag11 = this.currentRotation.x > 360f;
						if (flag11)
						{
							this.currentRotation.x = this.currentRotation.x - 360f;
						}
						else
						{
							bool flag12 = this.currentRotation.x < 0f;
							if (flag12)
							{
								this.currentRotation.x = this.currentRotation.x + 360f;
							}
						}
					}
					bool flag13 = this.spinZAxis;
					if (flag13)
					{
						this.currentRotation.z = this.currentRotation.z + (float)(this.reverseZ ? -1 : 1) * this.spinSpeedZ * this.spinSpeedMultiplier * Time.deltaTime;
						bool flag14 = this.currentRotation.z > 360f;
						if (flag14)
						{
							this.currentRotation.z = this.currentRotation.z - 360f;
						}
						else
						{
							bool flag15 = this.currentRotation.z < 0f;
							if (flag15)
							{
								this.currentRotation.z = this.currentRotation.z + 360f;
							}
						}
					}
					this.avatar.localRotation = Quaternion.Euler(this.currentRotation);
					bool flag16 = this.vrik != null;
					if (flag16)
					{
						this.vrik.enabled = false;
					}
				}
			}
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00033A34 File Offset: 0x00031C34
		private void InitializeSpin()
		{
			Player player = PlayerUtil.LocalPlayer();
			this.playerTf = ((player != null) ? player.transform : null);
			bool flag = this.playerTf == null;
			if (!flag)
			{
				this.avatar = this.playerTf.Find("ForwardDirection/Avatar");
				bool flag2 = this.avatar == null;
				if (!flag2)
				{
					this.animCtrl = this.avatar.Find("AnimationController/HeadAndHandIK");
					bool flag3 = this.animCtrl != null;
					if (flag3)
					{
						this.vrik = this.animCtrl.GetComponent<VRIK>();
						bool flag4 = this.vrik != null;
						if (flag4)
						{
							this.vrik.enabled = false;
						}
					}
					this.animator = this.avatar.GetComponent<Animator>();
					bool flag5 = this.animator != null;
					if (flag5)
					{
						this.animator.enabled = !this.disableAnimator;
					}
					this.currentRotation = this.avatar.localEulerAngles;
					this.spinInit = true;
				}
			}
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00033B44 File Offset: 0x00031D44
		private void EnableAnimatorAndIK(bool enable)
		{
			bool flag = this.animator != null;
			if (flag)
			{
				this.animator.enabled = (enable && !this.disableAnimator);
			}
			bool flag2 = this.vrik != null;
			if (flag2)
			{
				this.vrik.enabled = enable;
			}
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x00033B9C File Offset: 0x00031D9C
		private void ResetSpinState()
		{
			this.EnableAnimatorAndIK(true);
			this.spinInit = false;
		}

		// Token: 0x040003F2 RID: 1010
		private bool spinYAxis = false;

		// Token: 0x040003F3 RID: 1011
		private bool spinXAxis = false;

		// Token: 0x040003F4 RID: 1012
		private bool spinZAxis = false;

		// Token: 0x040003F5 RID: 1013
		private bool spinInit = false;

		// Token: 0x040003F6 RID: 1014
		private bool disableAnimator = false;

		// Token: 0x040003F7 RID: 1015
		private Transform avatar;

		// Token: 0x040003F8 RID: 1016
		private Transform animCtrl;

		// Token: 0x040003F9 RID: 1017
		private VRIK vrik;

		// Token: 0x040003FA RID: 1018
		private Animator animator;

		// Token: 0x040003FB RID: 1019
		private Transform playerTf;

		// Token: 0x040003FC RID: 1020
		private Vector3 currentRotation = Vector3.zero;

		// Token: 0x040003FD RID: 1021
		private float spinSpeedY = 1080f;

		// Token: 0x040003FE RID: 1022
		private float spinSpeedX = 720f;

		// Token: 0x040003FF RID: 1023
		private float spinSpeedZ = 720f;

		// Token: 0x04000400 RID: 1024
		private bool reverseY = false;

		// Token: 0x04000401 RID: 1025
		private bool reverseX = false;

		// Token: 0x04000402 RID: 1026
		private bool reverseZ = false;

		// Token: 0x04000403 RID: 1027
		private float spinSpeedMultiplier = 1f;
	}
}
