using System;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using Il2CppSystem.Diagnostics.Tracing;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Udon;

namespace KernelVRC
{
	// Token: 0x020000B1 RID: 177
	public abstract class KernelModuleBase : IKernelModule
	{
		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x00038C76 File Offset: 0x00036E76
		public virtual string ModuleName
		{
			get
			{
				return "GENERIC MODULE";
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x0002EF62 File Offset: 0x0002D162
		public virtual string Version
		{
			get
			{
				return "1.0.0";
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00038C7D File Offset: 0x00036E7D
		public virtual string Author
		{
			get
			{
				return "KernelVRC Team";
			}
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00038C84 File Offset: 0x00036E84
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x00038C8C File Offset: 0x00036E8C
		public virtual bool IsEnabled
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return this._isEnabled;
			}
			set
			{
				this._isEnabled = value;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x000354F5 File Offset: 0x000336F5
		public virtual ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.None;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x00038C95 File Offset: 0x00036E95
		public virtual UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.OnDemand;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x00003315 File Offset: 0x00001515
		public virtual ModulePriority Priority
		{
			get
			{
				return ModulePriority.Normal;
			}
		}

		// Token: 0x0600091C RID: 2332 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnApplicationStart()
		{
		}

		// Token: 0x0600091D RID: 2333 RVA: 0x00038C9C File Offset: 0x00036E9C
		public virtual void OnInitialize()
		{
			this._initialized = true;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00038CA6 File Offset: 0x00036EA6
		public virtual void OnShutdown()
		{
			this._initialized = false;
			this._uiInitialized = false;
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnUserLoggedIn(APIUser user)
		{
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00038CB7 File Offset: 0x00036EB7
		public virtual void OnUiManagerInit()
		{
			this._uiInitialized = true;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnUpdate()
		{
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnLateUpdate()
		{
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnGUI()
		{
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnPlayerJoined(Player player)
		{
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnPlayerLeft(Player player)
		{
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnEnterWorld(ApiWorld world, ApiWorldInstance instance)
		{
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnLeaveWorld()
		{
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnAvatarChanged(Player player, GameObject avatar)
		{
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x000347C3 File Offset: 0x000329C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool OnEventPatch(EventSource.EventData eventData)
		{
			return true;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x000347C3 File Offset: 0x000329C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool OnEventPatchVRC(ref EventSource.EventData eventData)
		{
			return true;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x000347C3 File Offset: 0x000329C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool OnEventSent(byte eventCode, object eventData, RaiseEventOptions options, SendOptions sendOptions)
		{
			return true;
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x000347C3 File Offset: 0x000329C3
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public virtual bool OnUdonPatch(UdonBehaviour instance, string program)
		{
			return true;
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnMenuOpened()
		{
		}

		// Token: 0x0600092F RID: 2351 RVA: 0x000053C4 File Offset: 0x000035C4
		public virtual void OnMenuClosed()
		{
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00038CC1 File Offset: 0x00036EC1
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void Log(string message)
		{
			KernelModuleBase.Logger.Msg("[" + this.ModuleName + "] " + message);
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x00038CE4 File Offset: 0x00036EE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void LogError(string message)
		{
			KernelModuleBase.Logger.Error("[" + this.ModuleName + "] " + message);
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x00038D07 File Offset: 0x00036F07
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void LogWarning(string message)
		{
			KernelModuleBase.Logger.Warning("[" + this.ModuleName + "] " + message);
		}

		// Token: 0x0400048F RID: 1167
		private bool _isEnabled = true;

		// Token: 0x04000490 RID: 1168
		protected bool _initialized;

		// Token: 0x04000491 RID: 1169
		protected bool _uiInitialized;

		// Token: 0x04000492 RID: 1170
		protected static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("KernelVRC");
	}
}
