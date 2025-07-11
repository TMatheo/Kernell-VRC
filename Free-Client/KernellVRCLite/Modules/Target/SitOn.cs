using System;
using System.Collections;
using System.Collections.Generic;
using KernellClientUI.UI.QuickMenu;
using KernellClientUI.VRChat;
using KernelVRC;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Core;

namespace KernellVRCLite.Modules.Target
{
	// Token: 0x02000098 RID: 152
	internal class SitOn : KernelModuleBase
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060007A8 RID: 1960 RVA: 0x0002EF5B File Offset: 0x0002D15B
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.WorldEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x000036A7 File Offset: 0x000018A7
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every30Frames;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060007AA RID: 1962 RVA: 0x0002F5E7 File Offset: 0x0002D7E7
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.Low;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060007AB RID: 1963 RVA: 0x0002F5EB File Offset: 0x0002D7EB
		public override string ModuleName
		{
			get
			{
				return "sit on player";
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0002F5F2 File Offset: 0x0002D7F2
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0002F5FA File Offset: 0x0002D7FA
		public bool OrbitTarget { get; set; } = false;

		// Token: 0x060007AF RID: 1967 RVA: 0x0002F604 File Offset: 0x0002D804
		public override void OnUiManagerInit()
		{
			ReCategoryPage reCategoryPage = new ReCategoryPage(MenuEx.QMSelectedUserLocal.transform);
			ReMenuCategory reMenuCategory = reCategoryPage.AddCategory("Sit On Menu", true, "#ffffff", false);
			reMenuCategory.AddButton("Sit On Head", "Sit on target (press jump to stop).", delegate
			{
				this.StartSitOn("Head");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Sit On Left Hand", "Sit on target (press jump to stop).", delegate
			{
				this.StartSitOn("LeftHand");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Sit On Right Hand", "Sit on target (press jump to stop).", delegate
			{
				this.StartSitOn("RightHand");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Sit On Right Leg", "Sit on target (press jump to stop).", delegate
			{
				this.StartSitOn("RightLeg");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Sit On Left Leg", "Sit on target (press jump to stop).", delegate
			{
				this.StartSitOn("LeftLeg");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Sit On Hips", "Sit on target (press jump to stop).", delegate
			{
				this.StartSitOn("Hips");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Stop Sit", "Stop sitting on people.", new Action(this.StopSit), null, "#ffffff");
		}

		// Token: 0x060007B0 RID: 1968 RVA: 0x0002F72A File Offset: 0x0002D92A
		private void StartSitOn(string bodyPart)
		{
			this._bodyPart = bodyPart;
			this.StandardSetup();
		}

		// Token: 0x060007B1 RID: 1969 RVA: 0x0002F73C File Offset: 0x0002D93C
		private void StandardSetup()
		{
			SelectedUserMenuQM qmselectedUserLocal = MenuEx.QMSelectedUserLocal;
			object obj;
			if (qmselectedUserLocal == null)
			{
				obj = null;
			}
			else
			{
				IUser field_Private_IUser_ = qmselectedUserLocal.field_Private_IUser_0;
				obj = ((field_Private_IUser_ != null) ? field_Private_IUser_.Method_Public_Abstract_Virtual_New_get_String_0() : null);
			}
			bool flag = obj == null;
			if (flag)
			{
				kernelllogger.Warning("[SitOn] No user selected.");
			}
			else
			{
				Player player = PlayerExtensions.GetPlayer(MenuEx.QMSelectedUserLocal.field_Private_IUser_0.Method_Public_Abstract_Virtual_New_get_String_0());
				bool flag2 = ((player != null) ? player.Method_Public_get_VRCPlayerApi_0() : null) == null;
				if (flag2)
				{
					kernelllogger.Warning("[SitOn] Could not find target player.");
				}
				else
				{
					this._target = player;
					this.CacheComponents();
					bool flag3 = this._cachedTargetAnimator == null;
					if (flag3)
					{
						kernelllogger.Warning("[SitOn] Target player has no animator.");
						this.StopSit();
					}
					else
					{
						this.SetGravity();
						this.TeleportToIUser(player);
						bool flag4 = this.sitCoroutine != null;
						if (flag4)
						{
							MelonCoroutines.Stop(this.sitCoroutine);
						}
						this.sitCoroutine = (Coroutine)MelonCoroutines.Start(this.SitCoroutineOptimized());
						string str = "[SitOn] Started sitting on ";
						APIUser apiuser = this._target.Method_Internal_get_APIUser_0();
						kernelllogger.Msg(str + ((apiuser != null) ? apiuser.displayName : null) + "'s " + this._bodyPart);
					}
				}
			}
		}

		// Token: 0x060007B2 RID: 1970 RVA: 0x0002F860 File Offset: 0x0002DA60
		private void CacheComponents()
		{
			this._cachedLocalPlayer = VRCPlayer.field_Internal_Static_VRCPlayer_0;
			bool flag = this._cachedLocalPlayer != null;
			if (flag)
			{
				this._cachedLocalTransform = this._cachedLocalPlayer.transform;
			}
			Player target = this._target;
			bool flag2 = ((target != null) ? target._vrcplayer : null) != null;
			if (flag2)
			{
				this._cachedTargetAnimator = this._target._vrcplayer.field_Internal_Animator_0;
				HumanBodyBones humanBodyBones;
				bool flag3 = this._cachedTargetAnimator != null && SitOn.BodyPartToBone.TryGetValue(this._bodyPart, out humanBodyBones);
				if (flag3)
				{
					this._cachedTargetBone = this._cachedTargetAnimator.GetBoneTransform(humanBodyBones);
				}
			}
		}

		// Token: 0x060007B3 RID: 1971 RVA: 0x0002F90B File Offset: 0x0002DB0B
		private void ClearCache()
		{
			this._cachedLocalPlayer = null;
			this._cachedLocalTransform = null;
			this._cachedTargetAnimator = null;
			this._cachedTargetBone = null;
		}

		// Token: 0x060007B4 RID: 1972 RVA: 0x0002F92C File Offset: 0x0002DB2C
		private void StopSit()
		{
			bool flag = this._target != null;
			if (flag)
			{
				kernelllogger.Msg("[SitOn] Stopped sitting.");
			}
			this._target = null;
			bool flag2 = this.sitCoroutine != null;
			if (flag2)
			{
				MelonCoroutines.Stop(this.sitCoroutine);
				this.sitCoroutine = null;
			}
			this.RemoveSetGravity();
			this.ClearCache();
		}

		// Token: 0x060007B5 RID: 1973 RVA: 0x0002F990 File Offset: 0x0002DB90
		private void SetGravity()
		{
			bool flag = Physics.gravity != Vector3.zero;
			if (flag)
			{
				this._originalGravity = Physics.gravity;
				Physics.gravity = Vector3.zero;
			}
		}

		// Token: 0x060007B6 RID: 1974 RVA: 0x0002F9CC File Offset: 0x0002DBCC
		private void RemoveSetGravity()
		{
			bool flag = this._originalGravity != Vector3.zero;
			if (flag)
			{
				Physics.gravity = this._originalGravity;
				this._originalGravity = Vector3.zero;
			}
		}

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002FA08 File Offset: 0x0002DC08
		private bool TeleportToIUser(Player user)
		{
			bool flag = user == null || this._cachedLocalTransform == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				try
				{
					bool flag2 = this._cachedTargetBone != null;
					Vector3 vector;
					if (flag2)
					{
						vector = this._cachedTargetBone.position + this._tempOffset;
					}
					else
					{
						HumanBodyBones humanBodyBones;
						bool flag3 = !SitOn.BodyPartToBone.TryGetValue(this._bodyPart, out humanBodyBones);
						if (flag3)
						{
							return false;
						}
						bool flag4 = this._cachedTargetAnimator == null;
						if (flag4)
						{
							return false;
						}
						Transform boneTransform = this._cachedTargetAnimator.GetBoneTransform(humanBodyBones);
						bool flag5 = boneTransform == null;
						if (flag5)
						{
							return false;
						}
						vector = boneTransform.position + this._tempOffset;
						this._cachedTargetBone = boneTransform;
					}
					bool flag6 = Vector3.Distance(this._playerLastPos, vector) > 0.001f;
					if (flag6)
					{
						this._cachedLocalTransform.position = vector;
						this._playerLastPos = vector;
					}
					result = true;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[SitOn] Error in TeleportToIUser: " + ex.Message);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002FB44 File Offset: 0x0002DD44
		private IEnumerator SitCoroutineOptimized()
		{
			this._lastUpdateTime = Time.time;
			this._lastInputCheckTime = Time.time;
			while (this._target != null)
			{
				float currentTime = Time.time;
				bool flag = currentTime - this._lastInputCheckTime >= 0.1f;
				if (flag)
				{
					this._lastInputCheckTime = currentTime;
					bool flag2 = this.CheckStopInput();
					if (flag2)
					{
						kernelllogger.Msg("[SitOn] Jump input detected, stopping sit.");
						break;
					}
				}
				bool flag3 = currentTime - this._lastUpdateTime >= 0.016f;
				if (flag3)
				{
					this._lastUpdateTime = currentTime;
					bool flag4 = !this.TeleportToIUser(this._target);
					if (flag4)
					{
						kernelllogger.Warning("[SitOn] Failed to teleport to target, stopping sit.");
						break;
					}
				}
				yield return this._waitForFrame;
			}
			this.StopSit();
			yield break;
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0002FB54 File Offset: 0x0002DD54
		private bool CheckStopInput()
		{
			return Input.GetKeyDown(32) || Input.GetButton("Oculus_CrossPlatform_Button1");
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0002FB7C File Offset: 0x0002DD7C
		public override void OnPlayerLeft(Player player)
		{
			bool flag = this._target == player;
			if (flag)
			{
				kernelllogger.Msg("[SitOn] Target player left, stopping sit.");
				this.StopSit();
			}
		}

		// Token: 0x060007BB RID: 1979 RVA: 0x0002FBB0 File Offset: 0x0002DDB0
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			bool flag = this._target != null;
			if (flag)
			{
				kernelllogger.Msg("[SitOn] World changed, stopping sit.");
				this.StopSit();
			}
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x0002FBE4 File Offset: 0x0002DDE4
		public override void OnUpdate()
		{
			bool flag = this._target != null && (this._target._vrcplayer == null || this._cachedTargetAnimator == null);
			if (flag)
			{
				kernelllogger.Warning("[SitOn] Target became invalid, stopping sit.");
				this.StopSit();
			}
		}

		// Token: 0x040003B1 RID: 945
		private Player _target;

		// Token: 0x040003B3 RID: 947
		private string _bodyPart;

		// Token: 0x040003B4 RID: 948
		private Vector3 _originalGravity;

		// Token: 0x040003B5 RID: 949
		private Vector3 _playerLastPos;

		// Token: 0x040003B6 RID: 950
		private Coroutine sitCoroutine;

		// Token: 0x040003B7 RID: 951
		private readonly WaitForEndOfFrame _waitForFrame = new WaitForEndOfFrame();

		// Token: 0x040003B8 RID: 952
		private const float UPDATE_INTERVAL = 0.016f;

		// Token: 0x040003B9 RID: 953
		private float _lastUpdateTime;

		// Token: 0x040003BA RID: 954
		private float _lastInputCheckTime;

		// Token: 0x040003BB RID: 955
		private const float INPUT_CHECK_INTERVAL = 0.1f;

		// Token: 0x040003BC RID: 956
		private VRCPlayer _cachedLocalPlayer;

		// Token: 0x040003BD RID: 957
		private Transform _cachedLocalTransform;

		// Token: 0x040003BE RID: 958
		private Animator _cachedTargetAnimator;

		// Token: 0x040003BF RID: 959
		private Transform _cachedTargetBone;

		// Token: 0x040003C0 RID: 960
		private readonly Vector3 _tempOffset = new Vector3(0f, 0.1f, 0f);

		// Token: 0x040003C1 RID: 961
		private static readonly Dictionary<string, HumanBodyBones> BodyPartToBone = new Dictionary<string, HumanBodyBones>
		{
			{
				"Head",
				10
			},
			{
				"LeftHand",
				27
			},
			{
				"RightHand",
				42
			},
			{
				"RightLeg",
				6
			},
			{
				"LeftLeg",
				5
			},
			{
				"Hips",
				0
			}
		};
	}
}
