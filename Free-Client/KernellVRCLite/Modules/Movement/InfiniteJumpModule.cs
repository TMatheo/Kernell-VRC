using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;
using MelonLoader;
using UnityEngine;
using VRC.SDKBase;

namespace KernellVRCLite.Modules.Movement
{
	// Token: 0x0200009C RID: 156
	public class InfiniteJumpModule : KernelModuleBase
	{
		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x00030EA6 File Offset: 0x0002F0A6
		public override string ModuleName
		{
			get
			{
				return "InfiniteJump";
			}
		}

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060007F9 RID: 2041 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00030EAD File Offset: 0x0002F0AD
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.MenuEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x060007FB RID: 2043 RVA: 0x00003312 File Offset: 0x00001512
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every2Frames;
			}
		}

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00003315 File Offset: 0x00001515
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.Normal;
			}
		}

		// Token: 0x060007FD RID: 2045 RVA: 0x00030EB4 File Offset: 0x0002F0B4
		public override void OnUiManagerInit()
		{
			try
			{
				ReMenuCategory reMenuCategory = MenuSetup._uiManager.QMMenu.GetCategoryPage("Utility").AddCategory("Movement", true, "#ffffff", false);
				IButtonPage launchPad = MenuSetup._uiManager.LaunchPad;
				launchPad.AddToggle("Enable Infinite Jump", "Allows you to jump upward whenever you hold the jump key (Space).", delegate(bool b)
				{
					this.InfiniteJumpEnabled = b;
					kernelllogger.Msg("[InfiniteJump] LaunchPad Toggle set to: " + b.ToString());
				}, false, "#ffffff");
				reMenuCategory.AddToggle("Enable Infinite Jump", "Allows you to jump upward whenever you hold the jump key (Space).", delegate(bool value)
				{
					this.InfiniteJumpEnabled = value;
					kernelllogger.Msg("[InfiniteJump] Movement Category Toggle set to: " + value.ToString());
				}, this.InfiniteJumpEnabled, "#ffffff");
				this.cancellationTokenSource = new CancellationTokenSource();
				this.InfiniteJumpTask = this.InfiniteJumpLoopAsync(this.cancellationTokenSource.Token);
				kernelllogger.Msg("[InfiniteJump] Module UI initialized successfully with threading");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[InfiniteJump] Error initializing UI: " + ex.Message);
			}
		}

		// Token: 0x060007FE RID: 2046 RVA: 0x00030F9C File Offset: 0x0002F19C
		[DebuggerStepThrough]
		private Task InfiniteJumpLoopAsync(CancellationToken cancellationToken)
		{
			InfiniteJumpModule.<InfiniteJumpLoopAsync>d__18 <InfiniteJumpLoopAsync>d__ = new InfiniteJumpModule.<InfiniteJumpLoopAsync>d__18();
			<InfiniteJumpLoopAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InfiniteJumpLoopAsync>d__.<>4__this = this;
			<InfiniteJumpLoopAsync>d__.cancellationToken = cancellationToken;
			<InfiniteJumpLoopAsync>d__.<>1__state = -1;
			<InfiniteJumpLoopAsync>d__.<>t__builder.Start<InfiniteJumpModule.<InfiniteJumpLoopAsync>d__18>(ref <InfiniteJumpLoopAsync>d__);
			return <InfiniteJumpLoopAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00030FE8 File Offset: 0x0002F1E8
		[DebuggerStepThrough]
		private Task<bool> CheckInputAsync()
		{
			InfiniteJumpModule.<CheckInputAsync>d__19 <CheckInputAsync>d__ = new InfiniteJumpModule.<CheckInputAsync>d__19();
			<CheckInputAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<CheckInputAsync>d__.<>4__this = this;
			<CheckInputAsync>d__.<>1__state = -1;
			<CheckInputAsync>d__.<>t__builder.Start<InfiniteJumpModule.<CheckInputAsync>d__19>(ref <CheckInputAsync>d__);
			return <CheckInputAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x0003102C File Offset: 0x0002F22C
		[DebuggerStepThrough]
		private Task ApplyInfiniteJumpAsync()
		{
			InfiniteJumpModule.<ApplyInfiniteJumpAsync>d__20 <ApplyInfiniteJumpAsync>d__ = new InfiniteJumpModule.<ApplyInfiniteJumpAsync>d__20();
			<ApplyInfiniteJumpAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ApplyInfiniteJumpAsync>d__.<>4__this = this;
			<ApplyInfiniteJumpAsync>d__.<>1__state = -1;
			<ApplyInfiniteJumpAsync>d__.<>t__builder.Start<InfiniteJumpModule.<ApplyInfiniteJumpAsync>d__20>(ref <ApplyInfiniteJumpAsync>d__);
			return <ApplyInfiniteJumpAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x00031070 File Offset: 0x0002F270
		private VRCPlayerApi GetCachedLocalPlayer()
		{
			bool flag = this.cachedLocalPlayer == null || Time.time - this.lastPlayerCacheTime > 1f;
			if (flag)
			{
				try
				{
					this.cachedLocalPlayer = Networking.LocalPlayer;
					this.lastPlayerCacheTime = Time.time;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[InfiniteJump] Error getting local player: " + ex.Message);
					this.cachedLocalPlayer = null;
				}
			}
			return this.cachedLocalPlayer;
		}

		// Token: 0x040003D3 RID: 979
		private volatile bool InfiniteJumpEnabled = false;

		// Token: 0x040003D4 RID: 980
		private volatile float jumpImpulse = 2f;

		// Token: 0x040003D5 RID: 981
		private CancellationTokenSource cancellationTokenSource;

		// Token: 0x040003D6 RID: 982
		private Task InfiniteJumpTask;

		// Token: 0x040003D7 RID: 983
		private VRCPlayerApi cachedLocalPlayer;

		// Token: 0x040003D8 RID: 984
		private float lastPlayerCacheTime = 0f;

		// Token: 0x040003D9 RID: 985
		private const float PLAYER_CACHE_INTERVAL = 1f;

		// Token: 0x02000174 RID: 372
		private static class MainThreadDispatcher
		{
			// Token: 0x06000CE7 RID: 3303 RVA: 0x0004BDF0 File Offset: 0x00049FF0
			public static Task InvokeAsync(Action action)
			{
				TaskCompletionSource<object> taskCompletionSource = new TaskCompletionSource<object>();
				MelonCoroutines.Start(InfiniteJumpModule.MainThreadDispatcher.ExecuteOnMainThread(action, taskCompletionSource));
				return taskCompletionSource.Task;
			}

			// Token: 0x06000CE8 RID: 3304 RVA: 0x0004BE1C File Offset: 0x0004A01C
			public static Task<T> InvokeAsync<T>(Func<T> func)
			{
				TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
				MelonCoroutines.Start(InfiniteJumpModule.MainThreadDispatcher.ExecuteOnMainThread<T>(func, taskCompletionSource));
				return taskCompletionSource.Task;
			}

			// Token: 0x06000CE9 RID: 3305 RVA: 0x0004BE47 File Offset: 0x0004A047
			private static IEnumerator ExecuteOnMainThread(Action action, TaskCompletionSource<object> tcs)
			{
				try
				{
					action();
					tcs.SetResult(null);
					yield break;
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					tcs.SetException(ex);
					yield break;
				}
				yield break;
			}

			// Token: 0x06000CEA RID: 3306 RVA: 0x0004BE5D File Offset: 0x0004A05D
			private static IEnumerator ExecuteOnMainThread<T>(Func<T> func, TaskCompletionSource<T> tcs)
			{
				try
				{
					T result = func();
					tcs.SetResult(result);
					result = default(T);
					yield break;
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					tcs.SetException(ex);
					yield break;
				}
				yield break;
			}
		}
	}
}
