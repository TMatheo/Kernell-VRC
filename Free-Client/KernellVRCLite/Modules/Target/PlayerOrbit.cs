using System;
using System.Collections;
using KernellClientUI.UI.QuickMenu;
using KernellClientUI.VRChat;
using KernellVRC;
using KernelVRC;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace KernellVRCLite.Modules.Target
{
	// Token: 0x02000097 RID: 151
	internal class PlayerOrbit : KernelModuleBase
	{
		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0002EF54 File Offset: 0x0002D154
		public override string ModuleName
		{
			get
			{
				return "PlayerOrbit";
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0002EF5B File Offset: 0x0002D15B
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.WorldEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0002EF62 File Offset: 0x0002D162
		public override string Version
		{
			get
			{
				return "1.0.0";
			}
		}

		// Token: 0x06000796 RID: 1942 RVA: 0x0002EF6C File Offset: 0x0002D16C
		public override void OnUiManagerInit()
		{
			ReCategoryPage reCategoryPage = MenuSetup._uiManager.TargetMenu.AddCategoryPage("Player Orbit", "", null, "#ffffff");
			ReMenuCategory reMenuCategory = reCategoryPage.AddCategory("Controls", true, "#ffffff", false);
			reMenuCategory.AddButton("Start Orbit", "Begin orbiting target", new Action(this.ToggleOrbit), null, "#ffffff");
			reMenuCategory.AddButton("Stop Orbit", "Stop orbiting target", new Action(this.StopOrbit), null, "#ffffff");
			ReMenuCategory reMenuCategory2 = reCategoryPage.AddCategory("Settings", true, "#ffffff", false);
			reMenuCategory2.AddButton("Speed +", "Faster", delegate
			{
				this._orbitSpeed += 10f;
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Speed -", "Slower", delegate
			{
				this._orbitSpeed = Math.Max(10f, this._orbitSpeed - 10f);
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Radius +", "Further", delegate
			{
				this._orbitRadius += 0.5f;
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Radius -", "Closer", delegate
			{
				this._orbitRadius = Math.Max(0.5f, this._orbitRadius - 0.5f);
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Height +", "Higher", delegate
			{
				this._orbitHeight += 0.5f;
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Height -", "Lower", delegate
			{
				this._orbitHeight -= 0.5f;
			}, null, "#ffffff");
			reMenuCategory2.AddToggle("Free Look", "Look freely while orbiting", delegate(bool state)
			{
				this._allowFreeLook = state;
			}, this._allowFreeLook, "#ffffff");
			reMenuCategory2.AddToggle("Stop On Movement", "Stop orbiting if you try to move", delegate(bool state)
			{
				this._stopOnMovement = state;
			}, this._stopOnMovement, "#ffffff");
		}

		// Token: 0x06000797 RID: 1943 RVA: 0x0002F128 File Offset: 0x0002D328
		private void ToggleOrbit()
		{
			bool isOrbiting = this._isOrbiting;
			if (isOrbiting)
			{
				this.StopOrbit();
			}
			else
			{
				SelectedUserMenuQM qmselectedUserLocal = MenuEx.QMSelectedUserLocal;
				bool flag = ((qmselectedUserLocal != null) ? qmselectedUserLocal.field_Private_IUser_0 : null) == null;
				if (flag)
				{
					kernelllogger.Warning("[Orbit] No target selected.");
				}
				else
				{
					try
					{
						this._target = PlayerExtensions.GetPlayer(MenuEx.QMSelectedUserLocal.field_Private_IUser_0);
						Player target = this._target;
						bool flag2 = ((target != null) ? target.Method_Public_get_VRCPlayerApi_0() : null) == null;
						if (flag2)
						{
							kernelllogger.Warning("[Orbit] Could not find target player.");
						}
						else
						{
							this.StartOrbit();
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[Orbit] Error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x06000798 RID: 1944 RVA: 0x0002F1E8 File Offset: 0x0002D3E8
		private void StartOrbit()
		{
			Player target = this._target;
			bool flag = ((target != null) ? target.Method_Public_get_VRCPlayerApi_0() : null) == null;
			if (!flag)
			{
				this._localPlayer = VRCPlayer.field_Internal_Static_VRCPlayer_0;
				VRCPlayer localPlayer = this._localPlayer;
				this._localPlayerApi = ((localPlayer != null) ? localPlayer.Method_Public_get_VRCPlayerApi_0() : null);
				VRCPlayer localPlayer2 = this._localPlayer;
				this._localTransform = ((localPlayer2 != null) ? localPlayer2.transform : null);
				this._targetTransform = this._target.Method_Public_get_VRCPlayerApi_0().gameObject.transform;
				bool flag2 = this._localPlayerApi == null || this._targetTransform == null || this._localTransform == null;
				if (flag2)
				{
					kernelllogger.Warning("[Orbit] Failed to get required components.");
				}
				else
				{
					Vector3 vector = this._localTransform.position - this._targetTransform.position;
					this._currentAngle = Mathf.Atan2(vector.z, vector.x) * 57.29578f;
					this._isOrbiting = true;
					this._initialPositionSet = false;
					bool flag3 = this._orbitCoroutine != null;
					if (flag3)
					{
						MelonCoroutines.Stop(this._orbitCoroutine);
					}
					this._orbitCoroutine = MelonCoroutines.Start(this.OrbitRoutine());
					string format = "[Orbit] Started orbiting {0} at radius {1}m, speed {2}°/s";
					APIUser apiuser = this._target.Method_Internal_get_APIUser_0();
					kernelllogger.Msg(string.Format(format, (apiuser != null) ? apiuser.displayName : null, this._orbitRadius, this._orbitSpeed));
				}
			}
		}

		// Token: 0x06000799 RID: 1945 RVA: 0x0002F354 File Offset: 0x0002D554
		private void StopOrbit()
		{
			this._isOrbiting = false;
			this._target = null;
			this._initialPositionSet = false;
			bool flag = this._orbitCoroutine != null;
			if (flag)
			{
				MelonCoroutines.Stop(this._orbitCoroutine);
				this._orbitCoroutine = null;
			}
			kernelllogger.Msg("[Orbit] Stopped orbit.");
		}

		// Token: 0x0600079A RID: 1946 RVA: 0x0002F3A4 File Offset: 0x0002D5A4
		private IEnumerator OrbitRoutine()
		{
			WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();
			for (;;)
			{
				bool flag;
				if (this._isOrbiting)
				{
					Player target = this._target;
					flag = (((target != null) ? target.Method_Public_get_VRCPlayerApi_0() : null) != null);
				}
				else
				{
					flag = false;
				}
				if (!flag)
				{
					break;
				}
				try
				{
					this._currentAngle += this._orbitSpeed * Time.deltaTime;
					this._currentAngle %= 360f;
					float radians = this._currentAngle * 0.017453292f;
					Vector3 targetPos = this._targetTransform.position;
					Vector3 newPos = targetPos + new Vector3(Mathf.Cos(radians) * this._orbitRadius, this._orbitHeight, Mathf.Sin(radians) * this._orbitRadius);
					this._lastCalculatedPosition = newPos;
					Quaternion rotation = this._allowFreeLook ? this._localTransform.rotation : Quaternion.LookRotation(targetPos - newPos);
					this._localPlayerApi.TeleportTo(newPos, rotation);
					this._initialPositionSet = true;
					targetPos = default(Vector3);
					newPos = default(Vector3);
					rotation = default(Quaternion);
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					kernelllogger.Error("[Orbit] Error: " + ex.Message);
					this.StopOrbit();
					break;
				}
				yield return waitForFrame;
			}
			bool isOrbiting = this._isOrbiting;
			if (isOrbiting)
			{
				this.StopOrbit();
			}
			yield break;
		}

		// Token: 0x0600079B RID: 1947 RVA: 0x0002F3B4 File Offset: 0x0002D5B4
		public override void OnUpdate()
		{
			bool flag = !this._isOrbiting || !this._stopOnMovement || !this._initialPositionSet;
			if (!flag)
			{
				bool flag2 = this.CheckForPlayerMovementInput();
				if (flag2)
				{
					kernelllogger.Msg("[Orbit] User movement input detected. Stopping orbit.");
					this.StopOrbit();
				}
			}
		}

		// Token: 0x0600079C RID: 1948 RVA: 0x0002F404 File Offset: 0x0002D604
		private bool CheckForPlayerMovementInput()
		{
			return Input.GetKey(119) || Input.GetKey(97) || Input.GetKey(115) || Input.GetKey(100) || Input.GetKey(113) || Input.GetKey(101) || Input.GetKey(276) || Input.GetKey(275) || Input.GetKey(273) || Input.GetKey(274) || Input.GetKey(32);
		}

		// Token: 0x0600079D RID: 1949 RVA: 0x0002F488 File Offset: 0x0002D688
		public override void OnPlayerLeft(Player player)
		{
			bool flag = this._isOrbiting && player == this._target;
			if (flag)
			{
				kernelllogger.Msg("[Orbit] Target left. Stopping orbit.");
				this.StopOrbit();
			}
		}

		// Token: 0x0600079E RID: 1950 RVA: 0x0002F4C8 File Offset: 0x0002D6C8
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			bool isOrbiting = this._isOrbiting;
			if (isOrbiting)
			{
				kernelllogger.Msg("[Orbit] World changed. Stopping orbit.");
				this.StopOrbit();
			}
		}

		// Token: 0x040003A2 RID: 930
		private Player _target;

		// Token: 0x040003A3 RID: 931
		private bool _isOrbiting;

		// Token: 0x040003A4 RID: 932
		private float _orbitSpeed = 60f;

		// Token: 0x040003A5 RID: 933
		private float _orbitRadius = 1f;

		// Token: 0x040003A6 RID: 934
		private float _orbitHeight = 0f;

		// Token: 0x040003A7 RID: 935
		private float _currentAngle = 0f;

		// Token: 0x040003A8 RID: 936
		private bool _allowFreeLook = true;

		// Token: 0x040003A9 RID: 937
		private object _orbitCoroutine;

		// Token: 0x040003AA RID: 938
		private VRCPlayer _localPlayer;

		// Token: 0x040003AB RID: 939
		private VRCPlayerApi _localPlayerApi;

		// Token: 0x040003AC RID: 940
		private Transform _targetTransform;

		// Token: 0x040003AD RID: 941
		private Transform _localTransform;

		// Token: 0x040003AE RID: 942
		private Vector3 _lastCalculatedPosition;

		// Token: 0x040003AF RID: 943
		private bool _initialPositionSet = false;

		// Token: 0x040003B0 RID: 944
		private bool _stopOnMovement = true;
	}
}
