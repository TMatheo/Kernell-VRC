using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExitGames.Client.Photon;
using HarmonyLib;
using KernellVRCLite.Network;
using KernellVRCLite.player.Mono;
using MelonLoader;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC;
using VRC.Core;
using VRC.UI.Elements;

namespace KernelVRC
{
	// Token: 0x020000B8 RID: 184
	public static class Patches
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000990 RID: 2448 RVA: 0x0003AB39 File Offset: 0x00038D39
		public static bool IsInitialized
		{
			get
			{
				return Patches._initialized;
			}
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000991 RID: 2449 RVA: 0x0003AB40 File Offset: 0x00038D40
		public static bool IsInWorldTransition
		{
			get
			{
				return Patches._isInWorldTransition;
			}
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000992 RID: 2450 RVA: 0x0003AB47 File Offset: 0x00038D47
		public static bool IsWorldFullyLoaded
		{
			get
			{
				return Patches._worldFullyLoaded;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000993 RID: 2451 RVA: 0x0003AB4E File Offset: 0x00038D4E
		public static string CurrentWorldId
		{
			get
			{
				return Patches._currentWorldId;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x0003AB55 File Offset: 0x00038D55
		public static long TotalEventsProcessed
		{
			get
			{
				return Patches._totalEventsProcessed;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000995 RID: 2453 RVA: 0x0003AB5C File Offset: 0x00038D5C
		public static long TotalModuleCallbacks
		{
			get
			{
				return Patches._totalModuleCallbacks;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000996 RID: 2454 RVA: 0x0003AB64 File Offset: 0x00038D64
		public static int RegisteredModuleCount
		{
			get
			{
				object moduleLock = Patches._moduleLock;
				int count;
				lock (moduleLock)
				{
					count = Patches._registeredModules.Count;
				}
				return count;
			}
		}

		// Token: 0x06000997 RID: 2455 RVA: 0x0003ABB0 File Offset: 0x00038DB0
		public static bool Initialize()
		{
			bool flag = Patches._initialized || Patches._patchingInProgress;
			bool result;
			if (flag)
			{
				kernelllogger.Warning("[Patches] Already initialized or initialization in progress");
				result = Patches._initialized;
			}
			else
			{
				Patches._patchingInProgress = true;
				try
				{
					kernelllogger.Msg("[Patches] Starting patch system initialization...");
					bool flag2 = !Patches.InitializeHarmony();
					if (flag2)
					{
						kernelllogger.Error("[Patches] Failed to initialize Harmony");
						result = false;
					}
					else
					{
						bool flag3 = !Patches.ApplyAllPatches();
						if (flag3)
						{
							kernelllogger.Error("[Patches] Failed to apply patches");
							result = false;
						}
						else
						{
							Patches._initialized = true;
							kernelllogger.Msg("[Patches] Patch system initialized successfully");
							Patches.LogInitializationResults();
							result = true;
						}
					}
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[Patches] Initialization failed: {0}", arg));
					result = false;
				}
				finally
				{
					Patches._patchingInProgress = false;
				}
			}
			return result;
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x0003AC8C File Offset: 0x00038E8C
		private static bool InitializeHarmony()
		{
			bool result;
			try
			{
				Patches._harmony = new Harmony("KernelVRC.Patches.v5");
				kernelllogger.Msg("[Patches] Harmony instance created");
				result = true;
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] Harmony initialization failed: {0}", arg));
				result = false;
			}
			return result;
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x0003ACE4 File Offset: 0x00038EE4
		public static void RegisterModule(IKernelModule module)
		{
			bool flag = module == null;
			if (flag)
			{
				kernelllogger.Warning("[Patches] Attempted to register null module");
			}
			else
			{
				object moduleLock = Patches._moduleLock;
				lock (moduleLock)
				{
					bool flag3 = !Patches._registeredModules.Contains(module);
					if (flag3)
					{
						Patches._registeredModules.Add(module);
						kernelllogger.Msg(string.Format("[Patches] Registered module: {0} (Capabilities: {1})", module.ModuleName, module.Capabilities));
					}
					else
					{
						kernelllogger.Warning("[Patches] Module " + module.ModuleName + " already registered");
					}
				}
			}
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x0003AD9C File Offset: 0x00038F9C
		public static bool UnregisterModule(IKernelModule module)
		{
			bool flag = module == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object moduleLock = Patches._moduleLock;
				lock (moduleLock)
				{
					bool flag3 = Patches._registeredModules.Remove(module);
					bool flag4 = flag3;
					if (flag4)
					{
						kernelllogger.Msg("[Patches] Unregistered module: " + module.ModuleName);
					}
					result = flag3;
				}
			}
			return result;
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x0003AE18 File Offset: 0x00039018
		public static bool UnregisterModuleByName(string moduleName)
		{
			bool flag = string.IsNullOrEmpty(moduleName);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object moduleLock = Patches._moduleLock;
				lock (moduleLock)
				{
					IKernelModule kernelModule = Enumerable.FirstOrDefault<IKernelModule>(Patches._registeredModules, (IKernelModule m) => m.ModuleName == moduleName);
					bool flag3 = kernelModule != null;
					if (flag3)
					{
						return Patches.UnregisterModule(kernelModule);
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x0003AEAC File Offset: 0x000390AC
		private static void InvokeModuleEvent(ModuleCapabilities requiredCapability, Action<IKernelModule> action, string eventName)
		{
			bool flag = !Patches._initialized;
			if (!flag)
			{
				Patches._totalEventsProcessed += 1L;
				object moduleLock = Patches._moduleLock;
				lock (moduleLock)
				{
					foreach (IKernelModule kernelModule in Patches._registeredModules)
					{
						bool flag3 = !kernelModule.IsEnabled || (kernelModule.Capabilities & requiredCapability) == ModuleCapabilities.None;
						if (!flag3)
						{
							try
							{
								action(kernelModule);
								Patches._totalModuleCallbacks += 1L;
							}
							catch (Exception ex)
							{
								kernelllogger.Error(string.Concat(new string[]
								{
									"[Patches] Module ",
									kernelModule.ModuleName,
									" error in ",
									eventName,
									": ",
									ex.Message
								}));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600099D RID: 2461 RVA: 0x0003AFDC File Offset: 0x000391DC
		private static bool InvokeModuleEventWithResult(ModuleCapabilities requiredCapability, Func<IKernelModule, bool> func, string eventName)
		{
			bool flag = !Patches._initialized;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				Patches._totalEventsProcessed += 1L;
				bool flag2 = true;
				object moduleLock = Patches._moduleLock;
				lock (moduleLock)
				{
					foreach (IKernelModule kernelModule in Patches._registeredModules)
					{
						bool flag4 = !kernelModule.IsEnabled || (kernelModule.Capabilities & requiredCapability) == ModuleCapabilities.None;
						if (!flag4)
						{
							try
							{
								bool flag5 = !func(kernelModule);
								if (flag5)
								{
									flag2 = false;
								}
								Patches._totalModuleCallbacks += 1L;
							}
							catch (Exception ex)
							{
								kernelllogger.Error(string.Concat(new string[]
								{
									"[Patches] Module ",
									kernelModule.ModuleName,
									" error in ",
									eventName,
									": ",
									ex.Message
								}));
							}
						}
					}
				}
				result = flag2;
			}
			return result;
		}

		// Token: 0x0600099E RID: 2462 RVA: 0x0003B124 File Offset: 0x00039324
		private static bool ApplyAllPatches()
		{
			ValueTuple<string, Action>[] array = new ValueTuple<string, Action>[]
			{
				new ValueTuple<string, Action>("Player Events", new Action(Patches.ApplyPlayerEventPatches)),
				new ValueTuple<string, Action>("World Events", new Action(Patches.ApplyWorldEventPatches)),
				new ValueTuple<string, Action>("Scene Events", new Action(Patches.ApplySceneEventPatches)),
				new ValueTuple<string, Action>("Network Events", new Action(Patches.ApplyNetworkEventPatches)),
				new ValueTuple<string, Action>("Menu Events", new Action(Patches.ApplyMenuEventPatches)),
				new ValueTuple<string, Action>("Avatar Events", new Action(Patches.ApplyAvatarEventPatches)),
				new ValueTuple<string, Action>("System Events", new Action(Patches.ApplySystemEventPatches))
			};
			int num = 0;
			int num2 = array.Length;
			foreach (ValueTuple<string, Action> valueTuple in array)
			{
				string item = valueTuple.Item1;
				Action item2 = valueTuple.Item2;
				try
				{
					item2();
					Patches._patchResults[item] = true;
					num++;
					kernelllogger.Msg("[Patches] ✓ " + item);
				}
				catch (Exception ex)
				{
					Patches._patchResults[item] = false;
					kernelllogger.Error("[Patches] ✗ " + item + ": " + ex.Message);
				}
			}
			kernelllogger.Msg(string.Format("[Patches] Applied {0}/{1} patch groups successfully", num, num2));
			return num > 0;
		}

		// Token: 0x0600099F RID: 2463 RVA: 0x0003B2D8 File Offset: 0x000394D8
		private static void ApplyPlayerEventPatches()
		{
			bool flag = false;
			bool flag2 = false;
			MethodInfo[] methods = typeof(NetworkManager).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			string[] array = new string[]
			{
				"Method_Public_Void_Player_PDM_0",
				"Method_Public_Void_Player_PDM_1",
				"Method_Public_Void_Player_Boolean_PDM_0",
				"Method_Internal_Void_Player_0"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string methodName = array2[i];
				MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(methods, (MethodInfo m) => m.Name == methodName);
				bool flag3 = methodInfo != null && Patches.TryPatchMethod(methodInfo, "Player Join", "OnPlayerJoinedPrefix", null);
				if (flag3)
				{
					flag = true;
					break;
				}
			}
			string[] array3 = new string[]
			{
				"Method_Public_Void_Player_PDM_1",
				"Method_Public_Void_Player_PDM_2",
				"Method_Internal_Void_Player_1"
			};
			string[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				string methodName = array4[j];
				MethodInfo methodInfo2 = Enumerable.FirstOrDefault<MethodInfo>(methods, (MethodInfo m) => m.Name == methodName);
				bool flag4 = methodInfo2 != null && Patches.TryPatchMethod(methodInfo2, "Player Left", "OnPlayerLeftPrefix", null);
				if (flag4)
				{
					flag2 = true;
					break;
				}
			}
			bool flag5 = !flag || !flag2;
			if (flag5)
			{
				MethodInfo method = typeof(VRCPlayer).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				bool flag6 = method != null;
				if (flag6)
				{
					Patches.TryPatchMethod(method, "VRCPlayer.Awake", null, "OnVRCPlayerAwakePostfix");
				}
				MethodInfo method2 = typeof(VRCPlayer).GetMethod("OnDestroy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				bool flag7 = method2 != null;
				if (flag7)
				{
					Patches.TryPatchMethod(method2, "VRCPlayer.OnDestroy", "OnVRCPlayerDestroyPrefix", null);
				}
			}
			bool flag8 = !flag && !flag2;
			if (flag8)
			{
				kernelllogger.Warning("[Patches] Could not patch any player events - using fallback monitoring");
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x0003B4C0 File Offset: 0x000396C0
		private static void ApplyWorldEventPatches()
		{
			MethodInfo[] methods = typeof(RoomManager).GetMethods(BindingFlags.Static | BindingFlags.Public);
			MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(methods, (MethodInfo m) => m.Name.StartsWith("Method_Public_Static_Boolean_") && m.GetParameters().Length >= 2 && m.GetParameters()[0].ParameterType == typeof(ApiWorld));
			bool flag = methodInfo != null;
			if (flag)
			{
				Patches.TryPatchMethod(methodInfo, "World Enter", null, "OnEnterWorldPostfix");
			}
			MethodInfo methodInfo2 = Enumerable.FirstOrDefault<MethodInfo>(methods, (MethodInfo m) => m.Name.StartsWith("Method_Public_Static_Void_") && m.GetParameters().Length == 0);
			bool flag2 = methodInfo2 != null;
			if (flag2)
			{
				Patches.TryPatchMethod(methodInfo2, "World Leave", "OnLeaveWorldPrefix", null);
			}
			Type typeFromHandle = typeof(SceneManager);
			EventInfo @event = typeFromHandle.GetEvent("sceneLoaded");
			bool flag3 = @event != null;
			if (flag3)
			{
			}
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x0003B596 File Offset: 0x00039796
		private static void ApplySceneEventPatches()
		{
			kernelllogger.Msg("[Patches] Scene event monitoring enabled");
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x0003B5A4 File Offset: 0x000397A4
		private static void ApplyNetworkEventPatches()
		{
			MethodInfo[] methods = typeof(LoadBalancingClient).GetMethods(BindingFlags.Instance | BindingFlags.Public);
			MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(methods, (MethodInfo m) => m.Name == "OnEvent" && m.GetParameters().Length == 1 && m.GetParameters()[0].ParameterType.Name.Contains("EventData"));
			bool flag = methodInfo != null;
			if (flag)
			{
				Patches.TryPatchMethod(methodInfo, "Network OnEvent", "OnNetworkEventPrefix", null);
			}
			MethodInfo methodInfo2 = Enumerable.FirstOrDefault<MethodInfo>(methods, (MethodInfo m) => m.Name.Contains("RaiseEvent") && m.GetParameters().Length >= 4);
			bool flag2 = methodInfo2 != null;
			if (flag2)
			{
				Patches.TryPatchMethod(methodInfo2, "Network RaiseEvent", "OnRaiseEventPrefix", null);
			}
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x0003B650 File Offset: 0x00039850
		private static void ApplyMenuEventPatches()
		{
			Type typeFromHandle = typeof(QuickMenu);
			MethodInfo method = typeFromHandle.GetMethod("OnEnable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag = method != null;
			if (flag)
			{
				Patches.TryPatchMethod(method, "Menu OnEnable", null, "OnMenuOpenedPostfix");
			}
			MethodInfo method2 = typeFromHandle.GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			bool flag2 = method2 != null;
			if (flag2)
			{
				Patches.TryPatchMethod(method2, "Menu OnDisable", null, "OnMenuClosedPostfix");
			}
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x0003B6C8 File Offset: 0x000398C8
		private static void ApplyAvatarEventPatches()
		{
			PropertyInfo property = typeof(SystemInfo).GetProperty("deviceUniqueIdentifier", BindingFlags.Static | BindingFlags.Public);
			bool flag = ((property != null) ? property.GetGetMethod() : null) != null;
			if (flag)
			{
				Patches.TryPatchMethod(property.GetGetMethod(), "Hardware ID", "OnGetHardwareIdPrefix", null);
			}
			Type typeFromHandle = typeof(APIUser);
			PropertyInfo property2 = typeFromHandle.GetProperty("allowAvatarCopying", BindingFlags.Instance | BindingFlags.Public);
			bool flag2 = ((property2 != null) ? property2.GetSetMethod() : null) != null;
			if (flag2)
			{
				Patches.TryPatchMethod(property2.GetSetMethod(), "Avatar Copying", "OnSetAvatarCopyingPrefix", null);
			}
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x0003B766 File Offset: 0x00039966
		private static void ApplySystemEventPatches()
		{
			kernelllogger.Msg("[Patches] System event patches applied");
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x0003B774 File Offset: 0x00039974
		private static bool TryPatchMethod(MethodInfo method, string patchName, string prefix = null, string postfix = null)
		{
			bool flag = method == null;
			bool result;
			if (flag)
			{
				kernelllogger.Warning("[Patches] Cannot patch " + patchName + ": method is null");
				result = false;
			}
			else
			{
				try
				{
					HarmonyMethod harmonyMethod = (!string.IsNullOrEmpty(prefix)) ? new HarmonyMethod(typeof(Patches).GetMethod(prefix, BindingFlags.Static | BindingFlags.NonPublic)) : null;
					HarmonyMethod harmonyMethod2 = (!string.IsNullOrEmpty(postfix)) ? new HarmonyMethod(typeof(Patches).GetMethod(postfix, BindingFlags.Static | BindingFlags.NonPublic)) : null;
					Patches._harmony.Patch(method, harmonyMethod, harmonyMethod2, null, null, null);
					kernelllogger.Msg("[Patches] ✓ " + patchName + " patched successfully");
					result = true;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[Patches] ✗ " + patchName + " patch failed: " + ex.Message);
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x0003B854 File Offset: 0x00039A54
		[HarmonyPrefix]
		private static void OnPlayerJoinedPrefix(Player __0)
		{
			Player _ = __0;
			bool flag = ((_ != null) ? _.field_Private_APIUser_0 : null) == null;
			if (!flag)
			{
				try
				{
					kernelllogger.Msg("[Patches] Player joined: " + __0.field_Private_APIUser_0.displayName);
					Patches.InvokeModuleEvent(ModuleCapabilities.PlayerEvents, delegate(IKernelModule module)
					{
						module.OnPlayerJoined(__0);
					}, "OnPlayerJoined");
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[Patches] OnPlayerJoinedPrefix error: {0}", arg));
				}
			}
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0003B8F0 File Offset: 0x00039AF0
		[HarmonyPrefix]
		private static void OnPlayerLeftPrefix(Player __0)
		{
			Player _ = __0;
			bool flag = ((_ != null) ? _.field_Private_APIUser_0 : null) == null;
			if (!flag)
			{
				try
				{
					kernelllogger.Msg("[Patches] Player left: " + __0.field_Private_APIUser_0.displayName);
					Patches.InvokeModuleEvent(ModuleCapabilities.PlayerEvents, delegate(IKernelModule module)
					{
						module.OnPlayerLeft(__0);
					}, "OnPlayerLeft");
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[Patches] OnPlayerLeftPrefix error: {0}", arg));
				}
			}
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x0003B98C File Offset: 0x00039B8C
		[HarmonyPostfix]
		private static void OnVRCPlayerAwakePostfix(VRCPlayer __instance)
		{
			try
			{
				Player player = __instance._player;
				Player player2 = player;
				bool flag = ((player2 != null) ? player2.field_Private_APIUser_0 : null) != null;
				if (flag)
				{
					kernelllogger.Msg("[Patches] VRCPlayer awake: " + player.field_Private_APIUser_0.displayName);
					Patches.InvokeModuleEvent(ModuleCapabilities.PlayerEvents, delegate(IKernelModule module)
					{
						module.OnPlayerJoined(player);
					}, "OnPlayerJoined");
					Patches.InvokeModuleEvent(ModuleCapabilities.AvatarEvents, delegate(IKernelModule module)
					{
						module.OnAvatarChanged(player, __instance.gameObject);
					}, "OnAvatarChanged");
				}
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnVRCPlayerAwakePostfix error: {0}", arg));
			}
		}

		// Token: 0x060009AA RID: 2474 RVA: 0x0003BA60 File Offset: 0x00039C60
		[HarmonyPrefix]
		private static void OnVRCPlayerDestroyPrefix(VRCPlayer __instance)
		{
			try
			{
				Player player = __instance._player;
				Player player2 = player;
				bool flag = ((player2 != null) ? player2.field_Private_APIUser_0 : null) != null;
				if (flag)
				{
					kernelllogger.Msg("[Patches] VRCPlayer destroy: " + player.field_Private_APIUser_0.displayName);
					Patches.InvokeModuleEvent(ModuleCapabilities.PlayerEvents, delegate(IKernelModule module)
					{
						module.OnPlayerLeft(player);
					}, "OnPlayerLeft");
				}
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnVRCPlayerDestroyPrefix error: {0}", arg));
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x0003BB00 File Offset: 0x00039D00
		[HarmonyPrefix]
		private static void OnLeaveWorldPrefix()
		{
			try
			{
				kernelllogger.Msg("[Patches] World leaving detected - starting transition");
				Patches._isInWorldTransition = true;
				Patches._worldFullyLoaded = false;
				Patches._lastWorldChangeTime = DateTime.UtcNow;
				CustomNameplateManager.OnWorldLeaving();
				bool flag = KernellNetworkClient.Instance != null;
				if (flag)
				{
					KernellNetworkClient.Instance.OnWorldLeaving();
				}
				Patches.InvokeModuleEvent(ModuleCapabilities.WorldEvents, delegate(IKernelModule module)
				{
					module.OnLeaveWorld();
				}, "OnLeaveWorld");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnLeaveWorldPrefix error: {0}", arg));
			}
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x0003BBA4 File Offset: 0x00039DA4
		[HarmonyPostfix]
		private static void OnEnterWorldPostfix(ApiWorld __0, ApiWorldInstance __1, bool __result)
		{
			bool flag = !__result || __0 == null;
			if (!flag)
			{
				try
				{
					string text = __0.id ?? "Unknown";
					kernelllogger.Msg("[Patches] World entered: " + text);
					Patches._currentWorldId = text;
					Patches._worldLoadStartTime = DateTime.UtcNow;
					Patches._isInWorldTransition = false;
					MelonCoroutines.Start(Patches.DelayedWorldSetup(text, __0, __1));
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[Patches] OnEnterWorldPostfix error: {0}", arg));
				}
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0003BC34 File Offset: 0x00039E34
		private static IEnumerator DelayedWorldSetup(string worldId, ApiWorld world, ApiWorldInstance instance)
		{
			yield return new WaitForSeconds(2f);
			try
			{
				kernelllogger.Msg("[Patches] World fully loaded: " + worldId);
				Patches._worldFullyLoaded = true;
				CustomNameplateManager.OnWorldEntered();
				bool flag = KernellNetworkClient.Instance != null;
				if (flag)
				{
					ApiWorldInstance inst = RoomManager.field_Private_Static_ApiWorldInstance_1;
					string instanceId = inst.id;
					KernellNetworkClient.Instance.OnWorldEntered(instanceId);
					inst = null;
					instanceId = null;
				}
				Patches.InvokeModuleEvent(ModuleCapabilities.WorldEvents, delegate(IKernelModule module)
				{
					module.OnEnterWorld(world, instance);
				}, "OnEnterWorld");
				yield break;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				kernelllogger.Error(string.Format("[Patches] DelayedWorldSetup error: {0}", ex));
				yield break;
			}
			yield break;
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0003BC54 File Offset: 0x00039E54
		[HarmonyPrefix]
		private static bool OnNetworkEventPrefix(EventData __0)
		{
			bool result;
			try
			{
				result = Patches.InvokeModuleEventWithResult(ModuleCapabilities.NetworkEvents, (IKernelModule module) => true, "OnNetworkEvent");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnNetworkEventPrefix error: {0}", arg));
				result = true;
			}
			return result;
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0003BCBC File Offset: 0x00039EBC
		[HarmonyPrefix]
		private static bool OnRaiseEventPrefix(byte __0, object __1, RaiseEventOptions __2, SendOptions __3)
		{
			bool result;
			try
			{
				result = Patches.InvokeModuleEventWithResult(ModuleCapabilities.NetworkEvents, (IKernelModule module) => module.OnEventSent(__0, __1, __2, __3), "OnEventSent");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnRaiseEventPrefix error: {0}", arg));
				result = true;
			}
			return result;
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0003BD30 File Offset: 0x00039F30
		[HarmonyPostfix]
		private static void OnMenuOpenedPostfix()
		{
			try
			{
				Patches.InvokeModuleEvent(ModuleCapabilities.MenuEvents, delegate(IKernelModule module)
				{
					module.OnMenuOpened();
				}, "OnMenuOpened");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnMenuOpenedPostfix error: {0}", arg));
			}
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0003BD98 File Offset: 0x00039F98
		[HarmonyPostfix]
		private static void OnMenuClosedPostfix()
		{
			try
			{
				Patches.InvokeModuleEvent(ModuleCapabilities.MenuEvents, delegate(IKernelModule module)
				{
					module.OnMenuClosed();
				}, "OnMenuClosed");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnMenuClosedPostfix error: {0}", arg));
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0003BE00 File Offset: 0x0003A000
		[HarmonyPrefix]
		private static bool OnGetHardwareIdPrefix(ref string __result)
		{
			bool result;
			try
			{
				__result = Patches.GenerateRandomHardwareId();
				result = false;
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] OnGetHardwareIdPrefix error: {0}", arg));
				result = true;
			}
			return result;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0003BE44 File Offset: 0x0003A044
		[HarmonyPrefix]
		private static void OnSetAvatarCopyingPrefix(ref bool __0)
		{
			__0 = true;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0003BE4C File Offset: 0x0003A04C
		private static string GenerateRandomHardwareId()
		{
			string result;
			try
			{
				Random random = new Random();
				byte[] array = new byte[16];
				random.NextBytes(array);
				result = BitConverter.ToString(array).Replace("-", "").ToLower();
			}
			catch
			{
				result = string.Format("kernelvrc-{0:x}", DateTime.UtcNow.Ticks);
			}
			return result;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0003BEC0 File Offset: 0x0003A0C0
		private static void LogInitializationResults()
		{
			kernelllogger.Msg("=== Patch System Results ===");
			foreach (KeyValuePair<string, bool> keyValuePair in Patches._patchResults)
			{
				string str = keyValuePair.Value ? "✓" : "✗";
				kernelllogger.Msg(str + " " + keyValuePair.Key);
			}
			kernelllogger.Msg(string.Format("Modules: {0}", Patches.RegisteredModuleCount));
			kernelllogger.Msg(string.Format("Events: {0}", Patches._totalEventsProcessed));
			kernelllogger.Msg(string.Format("Callbacks: {0}", Patches._totalModuleCallbacks));
			kernelllogger.Msg("============================");
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0003BFA4 File Offset: 0x0003A1A4
		public static Dictionary<string, bool> GetPatchResults()
		{
			return new Dictionary<string, bool>(Patches._patchResults);
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0003BFB0 File Offset: 0x0003A1B0
		[Obsolete("Obsolete")]
		public static void Shutdown()
		{
			try
			{
				kernelllogger.Msg("[Patches] Shutting down patch system...");
				bool flag = Patches._harmony != null;
				if (flag)
				{
					Patches._harmony.UnpatchAll(null);
					Patches._harmony = null;
				}
				object moduleLock = Patches._moduleLock;
				lock (moduleLock)
				{
					Patches._registeredModules.Clear();
				}
				Patches._patchResults.Clear();
				Patches._initialized = false;
				Patches._isInWorldTransition = false;
				Patches._worldFullyLoaded = false;
				Patches._currentWorldId = null;
				kernelllogger.Msg("[Patches] Patch system shutdown complete");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[Patches] Shutdown error: {0}", arg));
			}
		}

		// Token: 0x040004E0 RID: 1248
		private static Harmony _harmony;

		// Token: 0x040004E1 RID: 1249
		private static bool _initialized = false;

		// Token: 0x040004E2 RID: 1250
		private static bool _patchingInProgress = false;

		// Token: 0x040004E3 RID: 1251
		private static string _currentWorldId = null;

		// Token: 0x040004E4 RID: 1252
		private static bool _isInWorldTransition = false;

		// Token: 0x040004E5 RID: 1253
		private static bool _worldFullyLoaded = false;

		// Token: 0x040004E6 RID: 1254
		private static DateTime _worldLoadStartTime = DateTime.MinValue;

		// Token: 0x040004E7 RID: 1255
		private static DateTime _lastWorldChangeTime = DateTime.MinValue;

		// Token: 0x040004E8 RID: 1256
		private static readonly object _moduleLock = new object();

		// Token: 0x040004E9 RID: 1257
		private static readonly List<IKernelModule> _registeredModules = new List<IKernelModule>();

		// Token: 0x040004EA RID: 1258
		private static readonly Dictionary<string, bool> _patchResults = new Dictionary<string, bool>();

		// Token: 0x040004EB RID: 1259
		private static long _totalEventsProcessed = 0L;

		// Token: 0x040004EC RID: 1260
		private static long _totalModuleCallbacks = 0L;

		// Token: 0x040004ED RID: 1261
		private static bool _networkClientActive = false;

		// Token: 0x040004EE RID: 1262
		private static DateTime _lastNetworkUpdate = DateTime.MinValue;
	}
}
