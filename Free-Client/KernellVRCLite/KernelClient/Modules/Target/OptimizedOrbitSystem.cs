using System;
using System.Linq;
using KernellClientUI.UI.QuickMenu;
using KernellClientUI.VRChat;
using KernellVRC;
using KernellVRCLite;
using KernelVRC;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRC.SDKBase;

namespace KernelClient.Modules.Target
{
	// Token: 0x02000020 RID: 32
	internal class OptimizedOrbitSystem : KernelModuleBase
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600015C RID: 348 RVA: 0x00007F31 File Offset: 0x00006131
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600015D RID: 349 RVA: 0x00007F38 File Offset: 0x00006138
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every3Frames;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600015E RID: 350 RVA: 0x00007F3B File Offset: 0x0000613B
		public override string ModuleName
		{
			get
			{
				return "Optimized Orbit System";
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00007F44 File Offset: 0x00006144
		public override void OnUiManagerInit()
		{
			OptimizedOrbitSystem._instance = this;
			this.InitializeUI();
			ToastNotif.Toast("Optimized Orbit", "Circle & Swastika patterns loaded", EmbeddedResourceLoader.LoadEmbeddedSprite("KernelClient.assets.IOI.png", null, 100f), 2f);
			kernelllogger.Msg("Optimized Orbit System initialized");
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00007F98 File Offset: 0x00006198
		private void InitializeUI()
		{
			try
			{
				ReCategoryPage reCategoryPage = MenuSetup._uiManager.TargetMenu.AddCategoryPage("Orbit", "Optimized orbit patterns", null, "#00ff88");
				ReMenuCategory reMenuCategory = reCategoryPage.AddCategory("Patterns", true, "#ffffff", false);
				ReMenuCategory reMenuCategory2 = reCategoryPage.AddCategory("Controls", true, "#ffffff", false);
				reMenuCategory.AddToggle("Circle", "Classic circular orbit", delegate(bool state)
				{
					if (state)
					{
						this.StartOrbit(OrbitMode.Circle);
					}
					else
					{
						this.StopOrbit();
					}
				}, false, "#ffffff");
				reMenuCategory.AddToggle("Swastika", "Swastika pattern", delegate(bool state)
				{
					if (state)
					{
						this.StartOrbit(OrbitMode.Swastika);
					}
					else
					{
						this.StopOrbit();
					}
				}, false, "#ffffff");
				reMenuCategory2.AddButton("Size +", "Increase size", delegate
				{
					this._radius = Mathf.Min(5f, this._radius + 0.5f);
				}, null, "#ffffff");
				reMenuCategory2.AddButton("Size -", "Decrease size", delegate
				{
					this._radius = Mathf.Max(0.5f, this._radius - 0.5f);
				}, null, "#ffffff");
				reMenuCategory2.AddButton("Speed +", "Increase speed", delegate
				{
					this._speed = Mathf.Min(3f, this._speed + 0.2f);
				}, null, "#ffffff");
				reMenuCategory2.AddButton("Speed -", "Decrease speed", delegate
				{
					this._speed = Mathf.Max(0.1f, this._speed - 0.2f);
				}, null, "#ffffff");
				reMenuCategory2.AddToggle("Disable Colliders", "Better performance", delegate(bool val)
				{
					this._disableColliders = val;
				}, true, "#ffffff");
				reMenuCategory2.AddButton("Stop", "Stop orbit", new Action(this.StopOrbit), null, "#ff4444");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("UI setup error: " + ex.Message);
			}
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00008140 File Offset: 0x00006340
		public override void OnUpdate()
		{
			this._updateCounter++;
			bool flag = this._updateCounter % 2 != 0;
			if (!flag)
			{
				bool flag2 = Time.time - this._lastPickupRefresh > 30f;
				if (flag2)
				{
					this.RefreshPickupsOptimized();
				}
				bool flag3 = !this._isActive || this._target == null;
				if (!flag3)
				{
					bool flag4 = !this.IsTargetValid();
					if (flag4)
					{
						this.StopOrbit();
					}
					else
					{
						this._time += Time.fixedDeltaTime * this._speed * 2f;
						this.UpdateOrbitPositions();
					}
				}
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x000081E8 File Offset: 0x000063E8
		private void UpdateOrbitPositions()
		{
			bool flag = this._cachedPickups.Length == 0;
			if (!flag)
			{
				Vector3 targetPosition = this.GetTargetPosition();
				int num = 0;
				for (int i = 0; i < this._cachedPickups.Length; i++)
				{
					this._validPickups[i] = (this._cachedPickups[i] != null && this._cachedPickups[i].gameObject != null);
					bool flag2 = this._validPickups[i];
					if (flag2)
					{
						num++;
					}
				}
				bool flag3 = num == 0;
				if (!flag3)
				{
					OrbitMode currentMode = this._currentMode;
					OrbitMode orbitMode = currentMode;
					if (orbitMode != OrbitMode.Circle)
					{
						if (orbitMode == OrbitMode.Swastika)
						{
							this.CalculateSwastikaPositions(targetPosition, num);
						}
					}
					else
					{
						this.CalculateCirclePositions(targetPosition, num);
					}
					this.ApplyPositionsBatched();
				}
			}
		}

		// Token: 0x06000163 RID: 355 RVA: 0x000082B0 File Offset: 0x000064B0
		private void CalculateCirclePositions(Vector3 center, int count)
		{
			int num = 0;
			float num2 = 360f / (float)count;
			int num3 = 0;
			while (num3 < this._cachedPickups.Length && num < count)
			{
				bool flag = !this._validPickups[num3];
				if (!flag)
				{
					float num4 = (num2 * (float)num + this._time * 90f) * 0.017453292f;
					this._positions[num3] = center + new Vector3(Mathf.Cos(num4) * this._radius, 0f, Mathf.Sin(num4) * this._radius);
					this._rotations[num3] = Quaternion.identity;
					num++;
				}
				num3++;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00008364 File Offset: 0x00006564
		private void CalculateSwastikaPositions(Vector3 center, int count)
		{
			int num = 0;
			float num2 = this._time * 30f;
			int num3 = Mathf.Max(1, count / 8);
			int num4 = 0;
			while (num4 < this._cachedPickups.Length && num < count)
			{
				bool flag = !this._validPickups[num4];
				if (!flag)
				{
					int section = num % 8;
					float num5 = (float)num / 8f / (float)Mathf.Max(1, num3);
					num5 = Mathf.Clamp01(num5);
					Vector3 swastikaArmPosition = this.GetSwastikaArmPosition(section, num5);
					Vector3 vector = Quaternion.Euler(0f, num2, 0f) * swastikaArmPosition;
					this._positions[num4] = center + vector;
					this._rotations[num4] = Quaternion.Euler(0f, num2, 0f);
					num++;
				}
				num4++;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00008440 File Offset: 0x00006640
		private Vector3 GetSwastikaArmPosition(int section, float position)
		{
			Vector3 result;
			switch (section)
			{
			case 0:
				result = new Vector3(0f, position * this._radius, 0f);
				break;
			case 1:
				result = new Vector3(0f, -position * this._radius, 0f);
				break;
			case 2:
				result = new Vector3(position * this._radius, 0f, 0f);
				break;
			case 3:
				result = new Vector3(-position * this._radius, 0f, 0f);
				break;
			case 4:
				result = new Vector3(this._radius, position * this._radius, 0f);
				break;
			case 5:
				result = new Vector3(this._radius, -position * this._radius, 0f);
				break;
			case 6:
				result = new Vector3(-this._radius, -position * this._radius, 0f);
				break;
			case 7:
				result = new Vector3(-this._radius, position * this._radius, 0f);
				break;
			default:
				result = Vector3.zero;
				break;
			}
			return result;
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000856C File Offset: 0x0000676C
		private void ApplyPositionsBatched()
		{
			for (int i = 0; i < this._cachedPickups.Length; i++)
			{
				bool flag = !this._validPickups[i];
				if (!flag)
				{
					VRC_Pickup vrc_Pickup = this._cachedPickups[i];
					try
					{
						bool flag2 = Vector3.Distance(vrc_Pickup.transform.position, this._positions[i]) > 0.2f;
						if (flag2)
						{
							Networking.SetOwner(Networking.LocalPlayer, vrc_Pickup.gameObject);
						}
						vrc_Pickup.transform.SetPositionAndRotation(this._positions[i], this._rotations[i]);
						Rigidbody component = vrc_Pickup.GetComponent<Rigidbody>();
						bool flag3 = component != null;
						if (flag3)
						{
							component.velocity = Vector3.zero;
							component.angularVelocity = Vector3.zero;
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("Position update error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x00008678 File Offset: 0x00006878
		private void RefreshPickupsOptimized()
		{
			try
			{
				this._lastPickupRefresh = Time.time;
				VRC_Pickup[] cachedPickups = Enumerable.ToArray<VRC_Pickup>(Enumerable.Take<VRC_Pickup>(Enumerable.Where<VRC_Pickup>(Object.FindObjectsOfType<VRC_Pickup>(), (VRC_Pickup p) => p != null && p.gameObject != null && p.gameObject.activeInHierarchy), 50));
				this._cachedPickups = cachedPickups;
				this._positions = new Vector3[this._cachedPickups.Length];
				this._rotations = new Quaternion[this._cachedPickups.Length];
				this._validPickups = new bool[this._cachedPickups.Length];
				this.SetCollidersOptimized();
				kernelllogger.Msg(string.Format("Refreshed {0} pickups (optimized)", this._cachedPickups.Length));
			}
			catch (Exception ex)
			{
				kernelllogger.Error("Pickup refresh error: " + ex.Message);
				this._cachedPickups = new VRC_Pickup[0];
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x00008768 File Offset: 0x00006968
		private void SetCollidersOptimized()
		{
			bool flag = !this._disableColliders;
			if (!flag)
			{
				VRC_Pickup[] cachedPickups = this._cachedPickups;
				int i = 0;
				while (i < cachedPickups.Length)
				{
					VRC_Pickup vrc_Pickup = cachedPickups[i];
					try
					{
						bool flag2 = ((vrc_Pickup != null) ? vrc_Pickup.gameObject : null) == null;
						if (!flag2)
						{
							Il2CppArrayBase<Collider> componentsInChildren = vrc_Pickup.GetComponentsInChildren<Collider>();
							foreach (Collider collider in componentsInChildren)
							{
								bool flag3 = collider != null;
								if (flag3)
								{
									collider.enabled = false;
								}
							}
							Rigidbody component = vrc_Pickup.GetComponent<Rigidbody>();
							bool flag4 = component != null;
							if (flag4)
							{
								component.isKinematic = true;
								component.useGravity = false;
							}
						}
					}
					catch
					{
					}
					IL_C6:
					i++;
					continue;
					goto IL_C6;
				}
			}
		}

		// Token: 0x06000169 RID: 361 RVA: 0x00008864 File Offset: 0x00006A64
		private void RestoreCollidersOptimized()
		{
			VRC_Pickup[] cachedPickups = this._cachedPickups;
			int i = 0;
			while (i < cachedPickups.Length)
			{
				VRC_Pickup vrc_Pickup = cachedPickups[i];
				try
				{
					bool flag = ((vrc_Pickup != null) ? vrc_Pickup.gameObject : null) == null;
					if (!flag)
					{
						Il2CppArrayBase<Collider> componentsInChildren = vrc_Pickup.GetComponentsInChildren<Collider>();
						foreach (Collider collider in componentsInChildren)
						{
							bool flag2 = collider != null;
							if (flag2)
							{
								collider.enabled = true;
							}
						}
						Rigidbody component = vrc_Pickup.GetComponent<Rigidbody>();
						bool flag3 = component != null;
						if (flag3)
						{
							component.isKinematic = false;
							component.useGravity = true;
						}
					}
				}
				catch
				{
				}
				IL_AF:
				i++;
				continue;
				goto IL_AF;
			}
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000894C File Offset: 0x00006B4C
		public void StartOrbit(OrbitMode mode)
		{
			Player selectedTarget = this.GetSelectedTarget();
			bool flag = ((selectedTarget != null) ? selectedTarget.field_Private_VRCPlayerApi_0 : null) == null;
			if (flag)
			{
				kernelllogger.Warning("Invalid target for orbit");
			}
			else
			{
				this.StopOrbit();
				this._currentMode = mode;
				this._target = selectedTarget;
				this._isActive = true;
				this._time = 0f;
				this.RefreshPickupsOptimized();
				kernelllogger.Msg(string.Format("Started {0} orbit on {1}", mode, selectedTarget.field_Private_VRCPlayerApi_0.displayName));
			}
		}

		// Token: 0x0600016B RID: 363 RVA: 0x000089D4 File Offset: 0x00006BD4
		public void StopOrbit()
		{
			bool flag = !this._isActive;
			if (!flag)
			{
				this._isActive = false;
				this._currentMode = OrbitMode.None;
				this._target = null;
				this.RestoreCollidersOptimized();
				kernelllogger.Msg("Orbit stopped");
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x00008A18 File Offset: 0x00006C18
		private Player GetSelectedTarget()
		{
			Player result;
			try
			{
				SelectedUserMenuQM qmselectedUserLocal = MenuEx.QMSelectedUserLocal;
				IUser user = (qmselectedUserLocal != null) ? qmselectedUserLocal.field_Private_IUser_0 : null;
				result = ((user != null) ? PlayerExtensions.GetPlayer(user) : null);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("Target selection error: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x00008A78 File Offset: 0x00006C78
		private bool IsTargetValid()
		{
			Player target = this._target;
			bool result;
			if (((target != null) ? target.field_Private_VRCPlayerApi_0 : null) != null)
			{
				VRCPlayer vrcplayer = this._target._vrcplayer;
				result = (((vrcplayer != null) ? vrcplayer.transform : null) != null);
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x00008AC0 File Offset: 0x00006CC0
		private Vector3 GetTargetPosition()
		{
			Player target = this._target;
			Object @object;
			if (target == null)
			{
				@object = null;
			}
			else
			{
				VRCPlayer vrcplayer = target._vrcplayer;
				@object = ((vrcplayer != null) ? vrcplayer.transform : null);
			}
			bool flag = @object != null;
			Vector3 result;
			if (flag)
			{
				result = this._target._vrcplayer.transform.position + Vector3.up;
			}
			else
			{
				result = Vector3.zero;
			}
			return result;
		}

		// Token: 0x0600016F RID: 367 RVA: 0x00008B22 File Offset: 0x00006D22
		public static OptimizedOrbitSystem GetInstance()
		{
			return OptimizedOrbitSystem._instance;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00008B29 File Offset: 0x00006D29
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000171 RID: 369 RVA: 0x00008B31 File Offset: 0x00006D31
		public OrbitMode CurrentMode
		{
			get
			{
				return this._currentMode;
			}
		}

		// Token: 0x0400006F RID: 111
		private static OptimizedOrbitSystem _instance;

		// Token: 0x04000070 RID: 112
		private OrbitMode _currentMode = OrbitMode.None;

		// Token: 0x04000071 RID: 113
		private Player _target;

		// Token: 0x04000072 RID: 114
		private bool _isActive = false;

		// Token: 0x04000073 RID: 115
		private float _time = 0f;

		// Token: 0x04000074 RID: 116
		private VRC_Pickup[] _cachedPickups = new VRC_Pickup[0];

		// Token: 0x04000075 RID: 117
		private float _lastPickupRefresh = 0f;

		// Token: 0x04000076 RID: 118
		private Vector3[] _positions;

		// Token: 0x04000077 RID: 119
		private Quaternion[] _rotations;

		// Token: 0x04000078 RID: 120
		private bool[] _validPickups;

		// Token: 0x04000079 RID: 121
		private float _radius = 2f;

		// Token: 0x0400007A RID: 122
		private float _speed = 1f;

		// Token: 0x0400007B RID: 123
		private bool _disableColliders = true;

		// Token: 0x0400007C RID: 124
		private int _updateCounter = 0;

		// Token: 0x0400007D RID: 125
		private const int UPDATE_INTERVAL = 2;
	}
}
