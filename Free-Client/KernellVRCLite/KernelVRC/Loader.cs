using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using KernellVRC;
using KernellVRCLite.Core.Mono;
using KernellVRCLite.Network;
using MelonLoader;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.Core;
using VRC.UI.Core;

namespace KernelVRC
{
	// Token: 0x020000B5 RID: 181
	public static class Loader
	{
		// Token: 0x14000025 RID: 37
		// (add) Token: 0x06000957 RID: 2391 RVA: 0x000398AC File Offset: 0x00037AAC
		// (remove) Token: 0x06000958 RID: 2392 RVA: 0x000398E0 File Offset: 0x00037AE0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnUIReady;

		// Token: 0x14000026 RID: 38
		// (add) Token: 0x06000959 RID: 2393 RVA: 0x00039914 File Offset: 0x00037B14
		// (remove) Token: 0x0600095A RID: 2394 RVA: 0x00039948 File Offset: 0x00037B48
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<APIUser> OnUserLogin;

		// Token: 0x14000027 RID: 39
		// (add) Token: 0x0600095B RID: 2395 RVA: 0x0003997C File Offset: 0x00037B7C
		// (remove) Token: 0x0600095C RID: 2396 RVA: 0x000399B0 File Offset: 0x00037BB0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<string> OnWorldLoaded;

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x000399E3 File Offset: 0x00037BE3
		public static bool IsInitialized
		{
			get
			{
				return Loader._isInitialized;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x000399EC File Offset: 0x00037BEC
		public static bool IsUserLoggedIn
		{
			get
			{
				return Loader._userLoggedIn && Loader._currentUser != null;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x00039A02 File Offset: 0x00037C02
		public static bool IsWorldLoaded
		{
			get
			{
				return Loader._worldLoaded;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x00039A0B File Offset: 0x00037C0B
		public static bool IsUIReady
		{
			get
			{
				return Loader._uiSystemsReady && Loader._vrcUiManager != null && Loader._unityUiManager != null;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x00039A31 File Offset: 0x00037C31
		public static bool IsCanvasReady
		{
			get
			{
				return Loader._canvasObjectsReady;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000962 RID: 2402 RVA: 0x00039A3A File Offset: 0x00037C3A
		public static bool IsNetworkInitialized
		{
			get
			{
				return Loader._networkInitialized;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000963 RID: 2403 RVA: 0x00039A43 File Offset: 0x00037C43
		public static bool IsFullyReady
		{
			get
			{
				return Loader._isInitialized && Loader._userLoggedIn && Loader._worldLoaded && Loader._uiSystemsReady && Loader._canvasObjectsReady && Loader._modulesInitialized;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000964 RID: 2404 RVA: 0x00039A7C File Offset: 0x00037C7C
		public static APIUser CurrentUser
		{
			get
			{
				return Loader._currentUser;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000965 RID: 2405 RVA: 0x00039A83 File Offset: 0x00037C83
		public static string CurrentUserId
		{
			get
			{
				APIUser currentUser = Loader._currentUser;
				return (currentUser != null) ? currentUser.id : null;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000966 RID: 2406 RVA: 0x00039A96 File Offset: 0x00037C96
		public static string CurrentUserDisplayName
		{
			get
			{
				APIUser currentUser = Loader._currentUser;
				return (currentUser != null) ? currentUser.displayName : null;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000967 RID: 2407 RVA: 0x00039AA9 File Offset: 0x00037CA9
		public static VRCUiManager VRCUIManager
		{
			get
			{
				return Loader._vrcUiManager;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000968 RID: 2408 RVA: 0x00039AB0 File Offset: 0x00037CB0
		public static UIManager UnityUIManager
		{
			get
			{
				return Loader._unityUiManager;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000969 RID: 2409 RVA: 0x00039AB7 File Offset: 0x00037CB7
		public static string ApplicationPath
		{
			get
			{
				return Loader._applicationPath;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x0600096A RID: 2410 RVA: 0x00039ABE File Offset: 0x00037CBE
		public static string CurrentWorldName
		{
			get
			{
				return Loader._currentWorldName;
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x00039AC5 File Offset: 0x00037CC5
		public static string CurrentWorldId
		{
			get
			{
				return Loader._currentWorldId;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x0600096C RID: 2412 RVA: 0x00039ACC File Offset: 0x00037CCC
		public static TimeSpan InitializationTime
		{
			get
			{
				return DateTime.Now - Loader._initStartTime;
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x00039ADD File Offset: 0x00037CDD
		public static Dictionary<string, bool> InitializationStatus
		{
			get
			{
				return new Dictionary<string, bool>(Loader._initializationSteps);
			}
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00039AEC File Offset: 0x00037CEC
		private static void InvalidateModuleCache()
		{
			object moduleCacheLock = Loader._moduleCacheLock;
			lock (moduleCacheLock)
			{
				Loader._cachedModules = null;
				Loader._cachedEnabledModules = null;
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x00039B38 File Offset: 0x00037D38
		private static IKernelModule[] GetCachedModules()
		{
			object moduleCacheLock = Loader._moduleCacheLock;
			IKernelModule[] cachedModules;
			lock (moduleCacheLock)
			{
				bool flag2 = Loader._cachedModules == null;
				if (flag2)
				{
					Loader._cachedModules = Plugin.GetAllModules();
				}
				cachedModules = Loader._cachedModules;
			}
			return cachedModules;
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00039B94 File Offset: 0x00037D94
		private static IKernelModule[] GetCachedEnabledModules()
		{
			object moduleCacheLock = Loader._moduleCacheLock;
			IKernelModule[] cachedEnabledModules;
			lock (moduleCacheLock)
			{
				bool flag2 = Loader._cachedEnabledModules == null;
				if (flag2)
				{
					Loader._cachedEnabledModules = Enumerable.ToArray<IKernelModule>(Enumerable.Where<IKernelModule>(Plugin.GetAllModules(), (IKernelModule m) => m != null && m.IsEnabled));
				}
				cachedEnabledModules = Loader._cachedEnabledModules;
			}
			return cachedEnabledModules;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00039C1C File Offset: 0x00037E1C
		public static void Initialize()
		{
			bool flag = Loader._isInitialized || Loader._initializationInProgress;
			if (flag)
			{
				kernelllogger.Warning("[Loader] Already initialized or initialization in progress");
			}
			else
			{
				Loader._initializationInProgress = true;
				Loader._initStartTime = DateTime.Now;
				Loader._initErrors.Clear();
				Loader._initWarnings.Clear();
				kernelllogger.Msg("[Loader] ===== Starting KernelVRC Initialization =====");
				Loader.PrintStartupBanner();
				bool flag2 = Loader.InitializeCoreSystemsSync();
				bool flag3 = flag2;
				if (flag3)
				{
					MelonCoroutines.Start(Loader.MainInitializationCoroutine());
				}
				else
				{
					kernelllogger.Error("[Loader] Core systems failed, aborting initialization");
					Loader.CompleteInitialization(false);
				}
			}
		}

		// Token: 0x06000972 RID: 2418 RVA: 0x00039CB8 File Offset: 0x00037EB8
		public static void RegisterIl2CppTypes()
		{
			try
			{
				kernelllogger.Msg("[KernellVRCLite] Registering Il2Cpp types...");
				ClassInjector.RegisterTypeInIl2Cpp<CustomNameplate>();
				ClassInjector.RegisterTypeInIl2Cpp<CustomNameplateAccountAge>();
				ClassInjector.RegisterTypeInIl2Cpp<CustomNameplateTags>();
			}
			catch (Exception ex)
			{
				kernelllogger.Error(ex.ToString());
			}
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x00039D08 File Offset: 0x00037F08
		private static bool InitializeCoreSystemsSync()
		{
			kernelllogger.Msg("[Loader] Initializing core systems...");
			bool flag = true;
			flag &= Loader.SafeExecute(delegate
			{
				EmbeddedResourceLoader.Initialize();
				kernelllogger.Msg("[Loader] ✓ Embedded resources initialized");
				return true;
			}, "Embedded resources initialization", "EmbeddedResources");
			flag &= Loader.SafeExecute(delegate
			{
				string path = Path.Combine(Environment.CurrentDirectory, "KernelVRC");
				bool flag3 = !Directory.Exists(path);
				if (flag3)
				{
					Directory.CreateDirectory(path);
				}
				kernelllogger.Msg("[Loader] ✓ Configuration system initialized");
				return true;
			}, "Configuration initialization", "Configuration");
			flag &= Loader.SafeExecute(delegate
			{
				Loader.RegisterIl2CppTypes();
				kernelllogger.Msg("[Loader] ✓ IL2CPP types registered");
				return true;
			}, "IL2CPP type registration", "IL2CPPTypes");
			Loader.SetInitStep("CoreSystems", flag);
			bool flag2 = flag;
			if (flag2)
			{
				kernelllogger.Msg("[Loader] ✓ Core systems initialized successfully");
			}
			else
			{
				kernelllogger.Error("[Loader] ✗ Core systems initialization failed");
			}
			return flag;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x00039DE8 File Offset: 0x00037FE8
		private static IEnumerator MainInitializationCoroutine()
		{
			kernelllogger.Msg("[Loader] Starting main initialization coroutine...");
			kernelllogger.Msg("[Loader] Phase 1: Waiting for user login...");
			float loginWaitTime = 0f;
			while (!Loader._userLoggedIn && loginWaitTime < 120f)
			{
				Loader.CheckUserLogin();
				bool flag = !Loader._userLoggedIn;
				if (flag)
				{
					bool flag2 = loginWaitTime % 10f < 1f;
					if (flag2)
					{
						kernelllogger.Msg(string.Format("[Loader] Waiting for user login... ({0:F0}s)", loginWaitTime));
					}
					yield return new WaitForSeconds(1f);
					loginWaitTime += 1f;
				}
			}
			bool flag3 = !Loader._userLoggedIn;
			if (flag3)
			{
				kernelllogger.Warning("[Loader] User login timeout, continuing anyway");
			}
			else
			{
				kernelllogger.Msg(string.Format("[Loader] ✓ User logged in after {0:F0}s: {1} (ID: {2})", loginWaitTime, Loader._currentUser.displayName, Loader._currentUser.id));
				Loader.InitializeNetworkClient();
			}
			kernelllogger.Msg("[Loader] Phase 2: Waiting for world to load...");
			float worldWaitTime = 0f;
			while (!Loader._worldLoaded && worldWaitTime < 120f)
			{
				Loader.CheckWorldLoaded();
				bool flag4 = !Loader._worldLoaded;
				if (flag4)
				{
					bool flag5 = worldWaitTime % 10f < 1f;
					if (flag5)
					{
					}
					yield return new WaitForSeconds(1f);
					worldWaitTime += 1f;
				}
			}
			bool flag6 = !Loader._worldLoaded;
			if (flag6)
			{
				kernelllogger.Warning("[Loader] World load timeout, continuing anyway");
			}
			else
			{
				kernelllogger.Msg(string.Format("[Loader] ✓ World loaded after {0:F0}s: {1} (ID: {2})", worldWaitTime, Loader._currentWorldName, Loader._currentWorldId));
				bool flag7 = Loader._networkInitialized && !string.IsNullOrEmpty(Loader._currentWorldId);
				if (flag7)
				{
					MelonCoroutines.Start(Loader.DelayedNetworkWorldUpdate());
				}
			}
			kernelllogger.Msg(string.Format("[Loader] Phase 3: Waiting {0}s for game to fully initialize...", 60f));
			for (float stabilizationTime = 0f; stabilizationTime < 60f; stabilizationTime += 1f)
			{
				bool flag8 = stabilizationTime % 10f < 1f;
				if (flag8)
				{
					kernelllogger.Msg(string.Format("[Loader] Stabilization wait... ({0:F0}/{1}s)", stabilizationTime, 60f));
				}
				yield return new WaitForSeconds(1f);
			}
			kernelllogger.Msg("[Loader] ✓ Stabilization period complete");
			kernelllogger.Msg("[Loader] Phase 4: Checking for UI systems...");
			bool uiSystemsSuccess = false;
			float uiWaitTime = 0f;
			while (!uiSystemsSuccess && uiWaitTime < 120f)
			{
				uiSystemsSuccess = Loader.CheckUISystemsAndQuickMenu();
				bool flag9 = !uiSystemsSuccess;
				if (flag9)
				{
					bool flag10 = uiWaitTime % 10f < 1f;
					if (flag10)
					{
						kernelllogger.Msg(string.Format("[Loader] Waiting for UI systems... ({0:F0}s)", uiWaitTime));
						Loader.LogCurrentUIStatus();
					}
					yield return new WaitForSeconds(1f);
					uiWaitTime += 1f;
				}
			}
			bool flag11 = !uiSystemsSuccess;
			if (flag11)
			{
				kernelllogger.Warning("[Loader] UI systems not fully ready, proceeding with partial initialization");
			}
			else
			{
				kernelllogger.Msg(string.Format("[Loader] ✓ UI systems ready after {0:F0}s", uiWaitTime));
				try
				{
					Action onUIReady = Loader.OnUIReady;
					if (onUIReady != null)
					{
						onUIReady();
					}
					kernelllogger.Msg("[Loader] UI ready event fired");
				}
				catch (Exception ex3)
				{
					Exception ex = ex3;
					kernelllogger.Error("[Loader] Error firing UI ready event: " + ex.Message);
				}
			}
			kernelllogger.Msg(string.Format("[Loader] Phase 5: UI stabilization ({0}s)...", 5f));
			yield return new WaitForSeconds(5f);
			kernelllogger.Msg("[Loader] Phase 6: Initializing Menu System");
			yield return new WaitForSeconds(3f);
			bool menuSuccess = false;
			try
			{
				MenuSetup.Initialize();
				kernelllogger.Msg("[Loader] MenuSetup.Initialize() called");
			}
			catch (Exception ex3)
			{
				Exception ex2 = ex3;
				kernelllogger.Error("[Loader] MenuSetup initialization error: " + ex2.Message);
			}
			float menuWaitTime = 0f;
			while (!MenuSetup.IsReady && menuWaitTime < 170f)
			{
				yield return new WaitForSeconds(0.5f);
				menuWaitTime += 0.5f;
				bool flag12 = menuWaitTime % 10f < 0.5f;
				if (flag12)
				{
					kernelllogger.Msg(string.Format("[Loader] Waiting for MenuSetup... ({0:F0}s)", menuWaitTime));
				}
			}
			bool isReady = MenuSetup.IsReady;
			if (isReady)
			{
				menuSuccess = true;
				kernelllogger.Msg("[Loader] ✓ MenuSetup completed successfully");
			}
			else
			{
				bool isInitialized = MenuSetup.IsInitialized;
				if (isInitialized)
				{
					menuSuccess = true;
					kernelllogger.Warning("[Loader] ⚠ MenuSetup partially completed");
				}
				else
				{
					kernelllogger.Error("[Loader] ✗ MenuSetup failed to complete");
				}
			}
			Loader.SetInitStep("MenuSetup", menuSuccess);
			kernelllogger.Msg(string.Format("[Loader] Phase 7: Module initialization (delay {0}s)...", 2f));
			yield return new WaitForSeconds(2f);
			bool modulesSuccess = Loader.InitializeModulesAsync();
			bool overallSuccess = Loader._userLoggedIn && Loader._worldLoaded && modulesSuccess;
			Loader.CompleteInitialization(overallSuccess);
			yield break;
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00039DF0 File Offset: 0x00037FF0
		private static bool CheckUserLogin()
		{
			return Loader.SafeExecute(delegate
			{
				bool flag = APIUser.CurrentUser != null;
				bool result;
				if (flag)
				{
					Loader._currentUser = APIUser.CurrentUser;
					Loader._userLoggedIn = true;
					try
					{
						Action<APIUser> onUserLogin = Loader.OnUserLogin;
						if (onUserLogin != null)
						{
							onUserLogin(Loader._currentUser);
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[Loader] Error firing user login event: " + ex.Message);
					}
					Loader.NotifyModulesUserLogin();
					Loader.SetInitStep("UserLogin", true);
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}, "User login check", null);
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x00039E2C File Offset: 0x0003802C
		private static void InitializeNetworkClient()
		{
			bool flag = Loader._networkInitialized || !Loader._userLoggedIn || Loader._currentUser == null;
			if (!flag)
			{
				Loader._networkInitAttempts++;
				Loader.SafeExecute(delegate
				{
					kernelllogger.Msg(string.Format("[Loader] Initializing KernellNetworkClient (Attempt {0}/{1})...", Loader._networkInitAttempts, 3));
					bool result;
					try
					{
						KernellNetworkIntegration.Initialize();
						Loader._networkInitialized = true;
						Loader.SetInitStep("NetworkClient", true);
						kernelllogger.Msg("[Loader] ✓ KernellNetworkClient initialized successfully with user ID: " + Loader._currentUser.id);
						result = true;
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[Loader] KernellNetworkClient initialization failed: " + ex.Message);
						bool flag2 = Loader._networkInitAttempts < 3;
						if (flag2)
						{
							kernelllogger.Msg(string.Format("[Loader] Will retry network initialization in {0} seconds...", 10f));
							MelonCoroutines.Start(Loader.DelayedNetworkRetry());
						}
						else
						{
							Loader.SetInitStep("NetworkClient", false);
							kernelllogger.Error(string.Format("[Loader] Failed to initialize network client after {0} attempts", 3));
						}
						result = false;
					}
					return result;
				}, "Network client initialization", null);
			}
		}

		// Token: 0x06000977 RID: 2423 RVA: 0x00039E94 File Offset: 0x00038094
		private static IEnumerator DelayedNetworkRetry()
		{
			yield return new WaitForSeconds(10f);
			Loader.InitializeNetworkClient();
			yield break;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x00039E9C File Offset: 0x0003809C
		private static IEnumerator DelayedNetworkWorldUpdate()
		{
			yield return new WaitForSeconds(3f);
			bool flag = Loader._networkInitialized && !string.IsNullOrEmpty(Loader._currentWorldId);
			if (flag)
			{
				Loader.UpdateNetworkWorldInfo();
			}
			yield break;
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00039EA4 File Offset: 0x000380A4
		private static void UpdateNetworkWorldInfo()
		{
			bool flag = !Loader._networkInitialized || string.IsNullOrEmpty(Loader._currentWorldId);
			if (!flag)
			{
				Loader.SafeExecute(delegate
				{
					bool result;
					try
					{
						KernellNetworkIntegration.UpdateWorld(Loader._currentWorldId);
						kernelllogger.Msg("[Loader] ✓ Updated network with world ID: " + Loader._currentWorldId);
						result = true;
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[Loader] Failed to update network world info: " + ex.Message);
						result = false;
					}
					return result;
				}, "Network world update", null);
			}
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x00039EFC File Offset: 0x000380FC
		private static void NotifyModulesUserLogin()
		{
			bool flag = Loader._currentUser == null;
			if (!flag)
			{
				IKernelModule[] cachedEnabledModules = Loader.GetCachedEnabledModules();
				IKernelModule[] array = cachedEnabledModules;
				for (int i = 0; i < array.Length; i++)
				{
					IKernelModule module = array[i];
					Loader.SafeExecute(delegate
					{
						module.OnUserLoggedIn(Loader._currentUser);
						return true;
					}, "Module " + module.ModuleName + " user login notification", null);
				}
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00039F74 File Offset: 0x00038174
		private static bool CheckWorldLoaded()
		{
			return Loader.SafeExecute(delegate
			{
				bool flag = RoomManager.field_Internal_Static_ApiWorld_0 != null && RoomManager.field_Private_Static_ApiWorldInstance_0 != null;
				bool result;
				if (flag)
				{
					ApiWorldInstance field_Private_Static_ApiWorldInstance_ = RoomManager.field_Private_Static_ApiWorldInstance_1;
					Loader._currentWorldName = field_Private_Static_ApiWorldInstance_.name;
					Loader._currentWorldId = field_Private_Static_ApiWorldInstance_.id;
					Loader._worldLoaded = true;
					try
					{
						Action<string> onWorldLoaded = Loader.OnWorldLoaded;
						if (onWorldLoaded != null)
						{
							onWorldLoaded(Loader._currentWorldId);
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[Loader] Error firing world loaded event: " + ex.Message);
					}
					Loader.SetInitStep("WorldLoaded", true);
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}, "World load check", null);
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00039FB0 File Offset: 0x000381B0
		private static bool CheckUISystemsAndQuickMenu()
		{
			bool vrcUiReady = false;
			bool unityUiReady = false;
			bool quickMenuReady = false;
			Loader.SafeExecute(delegate
			{
				bool flag4 = VRCUiManager.field_Private_Static_VRCUiManager_0 != null;
				if (flag4)
				{
					Loader._vrcUiManager = VRCUiManager.field_Private_Static_VRCUiManager_0;
					vrcUiReady = true;
				}
				return true;
			}, "VRC UI Manager check", null);
			Loader.SafeExecute(delegate
			{
				bool flag4 = UIManager.Method_Public_Static_get_UIManager_PDM_0() != null;
				if (flag4)
				{
					Loader._unityUiManager = UIManager.Method_Public_Static_get_UIManager_PDM_0();
					unityUiReady = true;
				}
				return true;
			}, "Unity UI Manager check", null);
			Loader.SafeExecute(delegate
			{
				quickMenuReady = Loader.CheckForCanvasQuickMenu();
				return true;
			}, "Canvas_QuickMenu check", null);
			bool flag = vrcUiReady & unityUiReady;
			bool flag2 = flag && !Loader._uiSystemsReady;
			if (flag2)
			{
				Loader._uiSystemsReady = true;
				Loader.SetInitStep("UIReady", true);
				kernelllogger.Msg("[Loader] ✓ UI systems ready");
			}
			bool flag3 = quickMenuReady && !Loader._canvasObjectsReady;
			if (flag3)
			{
				Loader._canvasObjectsReady = true;
				Loader.SetInitStep("CanvasReady", true);
				kernelllogger.Msg("[Loader] ✓ Canvas_QuickMenu ready");
			}
			return flag;
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x0003A0A8 File Offset: 0x000382A8
		private static bool CheckForCanvasQuickMenu()
		{
			bool result;
			try
			{
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					bool flag = !sceneAt.isLoaded;
					if (!flag)
					{
						Il2CppReferenceArray<GameObject> rootGameObjects = sceneAt.GetRootGameObjects();
						foreach (GameObject gameObject in rootGameObjects)
						{
							bool flag2 = gameObject.name.StartsWith("_Application") && gameObject.name.Contains("-");
							if (flag2)
							{
								Transform transform = Loader.FindQuickMenuInHierarchy(gameObject.transform);
								bool flag3 = transform != null;
								if (flag3)
								{
									Loader._applicationPath = gameObject.name;
									kernelllogger.Msg("[Loader] Found QuickMenu in scene " + sceneAt.name);
									return true;
								}
							}
						}
					}
				}
				GameObject gameObject2 = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)");
				bool flag4 = gameObject2 != null;
				if (flag4)
				{
					kernelllogger.Msg("[Loader] Found QuickMenu via direct GameObject.Find");
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Warning("[Loader] Error checking for Canvas_QuickMenu: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x0003A228 File Offset: 0x00038428
		private static Transform FindQuickMenuInHierarchy(Transform root)
		{
			Transform transform = root.Find("UserInterface");
			bool flag = transform == null;
			Transform result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string[] array = new string[]
				{
					"Canvas_QuickMenu(Clone)",
					"Canvas_QuickMenu",
					"QuickMenu",
					"QuickMenuNew",
					"QuickMenuManager"
				};
				foreach (string text in array)
				{
					Transform transform2 = transform.Find(text);
					bool flag2 = transform2 != null;
					if (flag2)
					{
						return transform2;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0003A2C4 File Offset: 0x000384C4
		private static void LogCurrentUIStatus()
		{
			bool flag = Loader._vrcUiManager != null;
			bool flag2 = Loader._unityUiManager != null;
			bool canvasObjectsReady = Loader._canvasObjectsReady;
			kernelllogger.Msg(string.Format("[Loader] UI Status: VRC={0}, Unity={1}, Canvas={2}", flag, flag2, canvasObjectsReady));
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0003A314 File Offset: 0x00038514
		private static void LogAvailableScenes()
		{
			try
			{
				kernelllogger.Msg(string.Format("[Loader] Active scenes: {0}", SceneManager.sceneCount));
				for (int i = 0; i < SceneManager.sceneCount; i++)
				{
					Scene sceneAt = SceneManager.GetSceneAt(i);
					kernelllogger.Msg(string.Format("  - {0} (Loaded: {1}, Objects: {2})", sceneAt.name, sceneAt.isLoaded, sceneAt.rootCount));
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Warning("[Loader] Error logging scenes: " + ex.Message);
			}
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0003A3B8 File Offset: 0x000385B8
		private static bool InitializeModulesAsync()
		{
			return Loader.InitializeModulesSync();
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0003A3D0 File Offset: 0x000385D0
		private static bool InitializeModulesSync()
		{
			kernelllogger.Msg("[Loader] ===== Initializing Modules =====");
			return Loader.SafeExecute(delegate
			{
				IKernelModule[] cachedModules = Loader.GetCachedModules();
				bool flag = cachedModules == null || cachedModules.Length == 0;
				bool result;
				if (flag)
				{
					kernelllogger.Warning("[Loader] No modules found");
					result = true;
				}
				else
				{
					int num = 0;
					IKernelModule[] array = Enumerable.ToArray<IKernelModule>(Enumerable.Where<IKernelModule>(cachedModules, (IKernelModule m) => m != null && m.IsEnabled));
					IKernelModule[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						IKernelModule module = array2[i];
						bool flag2 = Loader.SafeExecute(delegate
						{
							module.OnInitialize();
							bool flag4 = Loader._userLoggedIn && Loader._currentUser != null;
							if (flag4)
							{
								module.OnUserLoggedIn(Loader._currentUser);
							}
							kernelllogger.Msg("[Loader] ✓ Initialized module: " + module.ModuleName);
							return true;
						}, "Module " + module.ModuleName + " initialization", null);
						bool flag3 = flag2;
						if (flag3)
						{
							num++;
						}
						Thread.Sleep(50);
					}
					Loader._modulesInitialized = true;
					Loader.InvalidateModuleCache();
					kernelllogger.Msg(string.Format("[Loader] Module initialization complete: {0}/{1} successful", num, array.Length));
					result = (num > 0 || cachedModules.Length == 0);
				}
				return result;
			}, "Modules initialization", "ModulesInit");
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x0003A41C File Offset: 0x0003861C
		public static void OnSceneLoaded(int buildIndex, string sceneName)
		{
			bool flag = !Loader._isInitialized;
			if (!flag)
			{
				Loader.SafeExecute(delegate
				{
					kernelllogger.Msg(string.Format("[Loader] Scene loaded: {0} (Index: {1})", sceneName, buildIndex));
					bool flag2 = false;
					bool flag3 = !Loader._worldLoaded;
					if (flag3)
					{
						flag2 = Loader.CheckWorldLoaded();
					}
					bool flag4 = flag2 && !sceneName.Equals("empty", StringComparison.OrdinalIgnoreCase) && Loader._networkInitialized && !string.IsNullOrEmpty(Loader._currentWorldId);
					if (flag4)
					{
						MelonCoroutines.Start(Loader.DelayedSceneNetworkUpdate());
					}
					IKernelModule[] cachedEnabledModules = Loader.GetCachedEnabledModules();
					IKernelModule[] array = cachedEnabledModules;
					for (int i = 0; i < array.Length; i++)
					{
						IKernelModule module = array[i];
						Loader.SafeExecute(delegate
						{
							module.OnSceneWasLoaded(buildIndex, sceneName);
							return true;
						}, "Module " + module.ModuleName + " scene notification", null);
					}
					return true;
				}, "Scene load handling", null);
			}
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x0003A466 File Offset: 0x00038666
		private static IEnumerator DelayedSceneNetworkUpdate()
		{
			yield return new WaitForSeconds(3f);
			bool flag = Loader._networkInitialized && !string.IsNullOrEmpty(Loader._currentWorldId);
			if (flag)
			{
				Loader.UpdateNetworkWorldInfo();
			}
			yield break;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x0003A470 File Offset: 0x00038670
		public static void HandleWorldJoined(string worldId, string worldName)
		{
			bool flag = !Loader._isInitialized;
			if (!flag)
			{
				Loader.SafeExecute(delegate
				{
					Loader._currentWorldId = worldId;
					Loader._currentWorldName = worldName;
					Loader._worldLoaded = true;
					kernelllogger.Msg(string.Concat(new string[]
					{
						"[Loader] World joined: ",
						worldName,
						" (ID: ",
						worldId,
						")"
					}));
					bool flag2 = Loader._networkInitialized && !string.IsNullOrEmpty(worldId);
					if (flag2)
					{
						MelonCoroutines.Start(Loader.DelayedWorldJoinedNetworkUpdate(worldId));
					}
					try
					{
						Action<string> onWorldLoaded = Loader.OnWorldLoaded;
						if (onWorldLoaded != null)
						{
							onWorldLoaded(worldId);
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[Loader] Error firing world loaded event: " + ex.Message);
					}
					return true;
				}, "World joined handling", null);
			}
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x0003A4BA File Offset: 0x000386BA
		private static IEnumerator DelayedWorldJoinedNetworkUpdate(string worldId)
		{
			yield return new WaitForSeconds(5f);
			bool flag = Loader._networkInitialized && !string.IsNullOrEmpty(worldId);
			if (flag)
			{
				Loader.UpdateNetworkWorldInfo();
			}
			yield break;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x0003A4CC File Offset: 0x000386CC
		private static bool SafeExecute(Func<bool> action, string operationName, string stepName = null)
		{
			bool result;
			try
			{
				bool flag = action();
				bool flag2 = stepName != null;
				if (flag2)
				{
					Loader.SetInitStep(stepName, flag);
				}
				bool flag3 = !flag;
				if (flag3)
				{
					string item = operationName + " returned false";
					Loader._initWarnings.Add(item);
				}
				result = flag;
			}
			catch (Exception ex)
			{
				string text = operationName + " failed: " + ex.Message;
				kernelllogger.Error("[Loader] " + text);
				Loader._initErrors.Add(text);
				bool flag4 = stepName != null;
				if (flag4)
				{
					Loader.SetInitStep(stepName, false);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0003A57C File Offset: 0x0003877C
		private static void SetInitStep(string stepName, bool success)
		{
			Loader._initializationSteps[stepName] = success;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0003A58C File Offset: 0x0003878C
		private static void PrintStartupBanner()
		{
			kernelllogger.Msg("");
			kernelllogger.Msg(ConsoleColor.DarkMagenta, "==============================================");
			kernelllogger.Msg(ConsoleColor.Blue, "  KernelVRC v2.0.0");
			kernelllogger.Msg(ConsoleColor.DarkCyan, "  Advanced VRChat modification framework");
			kernelllogger.Msg(ConsoleColor.Green, "  Created by KernelVRC Team");
			kernelllogger.Msg(ConsoleColor.DarkMagenta, "==============================================");
			kernelllogger.Msg("");
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0003A5EE File Offset: 0x000387EE
		private static void CompleteInitialization(bool success)
		{
			Loader.SafeExecute(delegate
			{
				Loader._isInitialized = true;
				Loader._initializationInProgress = false;
				double totalMilliseconds = Loader.InitializationTime.TotalMilliseconds;
				bool isReady = MenuSetup.IsReady;
				if (isReady)
				{
					Loader._menuSetupComplete = true;
					Loader.SetInitStep("MenuSetup", true);
					kernelllogger.Msg("[Loader] ✓ MenuSetup completed");
				}
				else
				{
					bool isInitialized = MenuSetup.IsInitialized;
					if (isInitialized)
					{
						Loader._menuSetupComplete = true;
						Loader.SetInitStep("MenuSetup", true);
						kernelllogger.Msg("[Loader] ✓ MenuSetup partially completed");
					}
					else
					{
						Loader.SetInitStep("MenuSetup", false);
						kernelllogger.Error("[Loader] ✗ MenuSetup failed");
					}
				}
				bool flag = Loader._userLoggedIn && Loader._worldLoaded && Loader._uiSystemsReady && Loader._modulesInitialized;
				bool flag2 = flag && Loader._initErrors.Count == 0;
				if (flag2)
				{
					kernelllogger.Msg(string.Format("[Loader] ✓ Initialization completed successfully in {0:F0}ms", totalMilliseconds));
				}
				else
				{
					bool flag3 = Loader._initErrors.Count == 0;
					if (flag3)
					{
						kernelllogger.Warning(string.Format("[Loader] ⚠ Initialization completed with warnings in {0:F0}ms", totalMilliseconds));
					}
					else
					{
						kernelllogger.Error(string.Format("[Loader] ✗ Initialization completed with errors in {0:F0}ms", totalMilliseconds));
					}
				}
				Loader.LogInitializationSummary();
				return true;
			}, "Initialization completion", null);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x0003A61C File Offset: 0x0003881C
		private static void LogInitializationSummary()
		{
			kernelllogger.Msg("");
			kernelllogger.Msg("=== Initialization Summary ===");
			kernelllogger.Msg(string.Format("Total Time: {0:F0}ms", Loader.InitializationTime.TotalMilliseconds));
			kernelllogger.Msg("User Login: " + (Loader.IsUserLoggedIn ? string.Concat(new string[]
			{
				"Success - ",
				Loader._currentUser.displayName,
				" (",
				Loader._currentUser.id,
				")"
			}) : "Failed"));
			kernelllogger.Msg("World Loaded: " + (Loader.IsWorldLoaded ? (Loader._currentWorldName + " (" + Loader._currentWorldId + ")") : "Failed"));
			kernelllogger.Msg("UI Systems: " + (Loader.IsUIReady ? "Ready" : "Failed"));
			kernelllogger.Msg("Canvas_QuickMenu: " + (Loader.IsCanvasReady ? "Ready" : "Not Found"));
			kernelllogger.Msg("Network Client: " + (Loader.IsNetworkInitialized ? "Initialized" : "Failed"));
			bool isReady = MenuSetup.IsReady;
			if (isReady)
			{
				kernelllogger.Msg("Menu Setup: Success");
			}
			else
			{
				bool isInitialized = MenuSetup.IsInitialized;
				if (isInitialized)
				{
					kernelllogger.Warning("Menu Setup: Partial");
				}
				else
				{
					kernelllogger.Error("Menu Setup: Failed");
				}
			}
			kernelllogger.Msg("Modules: " + (Loader._modulesInitialized ? "Initialized" : "Failed"));
			bool flag = Loader.IsUserLoggedIn && Loader.IsWorldLoaded && Loader.IsUIReady && Loader._modulesInitialized;
			kernelllogger.Msg("Overall Status: " + (flag ? "SUCCESS" : "PARTIAL/FAILED"));
			bool flag2 = !string.IsNullOrEmpty(Loader._applicationPath);
			if (flag2)
			{
				kernelllogger.Msg("Application Path: " + Loader._applicationPath);
			}
			foreach (KeyValuePair<string, bool> keyValuePair in Enumerable.OrderBy<KeyValuePair<string, bool>, string>(Loader._initializationSteps, (KeyValuePair<string, bool> k) => k.Key))
			{
				string str = keyValuePair.Value ? "✓" : "✗";
				kernelllogger.Msg("  " + str + " " + keyValuePair.Key);
			}
			bool flag3 = Loader._initErrors.Count > 0;
			if (flag3)
			{
				kernelllogger.Error(string.Format("Errors: {0}", Loader._initErrors.Count));
				foreach (string str2 in Loader._initErrors)
				{
					kernelllogger.Error("  - " + str2);
				}
			}
			bool flag4 = Loader._initWarnings.Count > 0;
			if (flag4)
			{
				kernelllogger.Warning(string.Format("Warnings: {0}", Loader._initWarnings.Count));
				foreach (string str3 in Loader._initWarnings)
				{
					kernelllogger.Warning("  - " + str3);
				}
			}
			kernelllogger.Msg("==============================");
			kernelllogger.Msg("");
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x0003A9E8 File Offset: 0x00038BE8
		public static void InvalidateCache()
		{
			Loader.InvalidateModuleCache();
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x0003A9F4 File Offset: 0x00038BF4
		public static void ManualNetworkInitialization()
		{
			bool networkInitialized = Loader._networkInitialized;
			if (!networkInitialized)
			{
				bool flag = Loader._userLoggedIn && Loader._currentUser != null;
				if (flag)
				{
					kernelllogger.Msg("[Loader] Manually retrying network initialization...");
					Loader._networkInitAttempts = 0;
					Loader.InitializeNetworkClient();
				}
				else
				{
					kernelllogger.Warning("[Loader] Cannot retry network initialization - user not logged in");
				}
			}
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x0003AA50 File Offset: 0x00038C50
		public static void ManualWorldUpdate()
		{
			bool flag = Loader._worldLoaded && Loader._networkInitialized && !string.IsNullOrEmpty(Loader._currentWorldId);
			if (flag)
			{
				Loader.UpdateNetworkWorldInfo();
			}
		}

		// Token: 0x040004A0 RID: 1184
		public const string CLIENT_NAME = "KernelVRC";

		// Token: 0x040004A1 RID: 1185
		public const string CLIENT_VERSION = "2.0.0";

		// Token: 0x040004A2 RID: 1186
		public const string CLIENT_AUTHOR = "KernelVRC Team";

		// Token: 0x040004A3 RID: 1187
		public const string CLIENT_DESCRIPTION = "Advanced VRChat modification framework";

		// Token: 0x040004A7 RID: 1191
		private static volatile bool _isInitialized = false;

		// Token: 0x040004A8 RID: 1192
		private static volatile bool _userLoggedIn = false;

		// Token: 0x040004A9 RID: 1193
		private static volatile bool _worldLoaded = false;

		// Token: 0x040004AA RID: 1194
		private static volatile bool _uiSystemsReady = false;

		// Token: 0x040004AB RID: 1195
		private static volatile bool _canvasObjectsReady = false;

		// Token: 0x040004AC RID: 1196
		private static volatile bool _modulesInitialized = false;

		// Token: 0x040004AD RID: 1197
		private static volatile bool _menuSetupComplete = false;

		// Token: 0x040004AE RID: 1198
		private static volatile bool _initializationInProgress = false;

		// Token: 0x040004AF RID: 1199
		private static volatile bool _networkInitialized = false;

		// Token: 0x040004B0 RID: 1200
		private static APIUser _currentUser;

		// Token: 0x040004B1 RID: 1201
		private static VRCUiManager _vrcUiManager;

		// Token: 0x040004B2 RID: 1202
		private static UIManager _unityUiManager;

		// Token: 0x040004B3 RID: 1203
		private static string _applicationPath = "";

		// Token: 0x040004B4 RID: 1204
		private static string _currentWorldName = "";

		// Token: 0x040004B5 RID: 1205
		private static string _currentWorldId = "";

		// Token: 0x040004B6 RID: 1206
		private static readonly Dictionary<string, bool> _initializationSteps = new Dictionary<string, bool>();

		// Token: 0x040004B7 RID: 1207
		private static DateTime _initStartTime;

		// Token: 0x040004B8 RID: 1208
		private static readonly List<string> _initErrors = new List<string>();

		// Token: 0x040004B9 RID: 1209
		private static readonly List<string> _initWarnings = new List<string>();

		// Token: 0x040004BA RID: 1210
		private static IKernelModule[] _cachedModules = null;

		// Token: 0x040004BB RID: 1211
		private static IKernelModule[] _cachedEnabledModules = null;

		// Token: 0x040004BC RID: 1212
		private static readonly object _moduleCacheLock = new object();

		// Token: 0x040004BD RID: 1213
		private const float INITIAL_WAIT_TIME = 60f;

		// Token: 0x040004BE RID: 1214
		private const float UI_CHECK_TIMEOUT = 120f;

		// Token: 0x040004BF RID: 1215
		private const float UI_STABILIZATION_TIME = 5f;

		// Token: 0x040004C0 RID: 1216
		private const float MENU_INIT_DELAY = 3f;

		// Token: 0x040004C1 RID: 1217
		private const float MODULE_INIT_DELAY = 2f;

		// Token: 0x040004C2 RID: 1218
		private const float NETWORK_RETRY_DELAY = 10f;

		// Token: 0x040004C3 RID: 1219
		private const int MAX_NETWORK_INIT_ATTEMPTS = 3;

		// Token: 0x040004C4 RID: 1220
		private static int _networkInitAttempts = 0;
	}
}
