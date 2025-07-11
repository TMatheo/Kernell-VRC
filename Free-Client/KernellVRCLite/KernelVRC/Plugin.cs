using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.CompilerServices;
using KernellVRC;
using MelonLoader;
using UnityEngine;

namespace KernelVRC
{
	// Token: 0x020000B9 RID: 185
	public class Plugin : MelonMod
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x0003C0F4 File Offset: 0x0003A2F4
		public override void OnInitializeMelon()
		{
			bool flag = Plugin._isInitialized || Plugin._initializationStarted || Plugin._isShuttingDown;
			if (!flag)
			{
				object initLock = Plugin._initLock;
				lock (initLock)
				{
					bool flag3 = Plugin._isInitialized || Plugin._initializationStarted || Plugin._isShuttingDown;
					if (!flag3)
					{
						Plugin._initializationStarted = true;
						Plugin._initStartTime = DateTime.Now;
						try
						{
							kernelllogger.Msg("[KernelVRC] Initializing Plugin with optimized module management and GC...");
							MelonCoroutines.Start(Plugin.MemoryMonitoringCoroutine());
							MelonCoroutines.Start(Plugin.InitializationCoroutine());
							kernelllogger.Msg("[KernelVRC] Plugin initialization started!");
						}
						catch (Exception arg)
						{
							kernelllogger.Error(string.Format("[KernelVRC] Failed to start initialization: {0}", arg));
							Plugin.FallbackInitialization();
						}
					}
				}
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0003C1E8 File Offset: 0x0003A3E8
		private static IEnumerator InitializationCoroutine()
		{
			kernelllogger.Msg("[KernelVRC] Starting initialization sequence...");
			try
			{
				ClassicEmbeddedResourceLoader.Initialize();
				kernelllogger.Msg("[KernelVRC] ✓ ClassicEmbeddedResourceLoader initialized");
			}
			catch (Exception ex6)
			{
				Exception ex = ex6;
				kernelllogger.Error("[KernelVRC] ClassicEmbeddedResourceLoader failed: " + ex.Message);
			}
			yield return null;
			yield return new WaitForSeconds(1f);
			bool modulesLoaded = false;
			try
			{
				modulesLoaded = Plugin.LoadModulesOptimized();
			}
			catch (Exception ex6)
			{
				Exception ex2 = ex6;
				kernelllogger.Error("[KernelVRC] Module loading error: " + ex2.Message);
			}
			bool flag = !modulesLoaded;
			if (flag)
			{
				kernelllogger.Error("[KernelVRC] Module loading failed");
				yield break;
			}
			yield return null;
			try
			{
				bool patchSuccess = Patches.Initialize();
				bool flag2 = patchSuccess;
				if (flag2)
				{
					kernelllogger.Msg("[KernelVRC] ✓ Patches initialized");
				}
				else
				{
					kernelllogger.Error("[KernelVRC] Patches initialization failed");
				}
			}
			catch (Exception ex6)
			{
				Exception ex3 = ex6;
				kernelllogger.Error("[KernelVRC] Patches failed: " + ex3.Message);
			}
			yield return null;
			try
			{
				Plugin.InitializeModuleSubscriptions();
				Plugin.CallModulesInPriorityOrder(delegate(IKernelModule m)
				{
					m.OnApplicationStart();
				});
			}
			catch (Exception ex6)
			{
				Exception ex4 = ex6;
				kernelllogger.Error(string.Format("[KernelVRC] Module initialization failed: {0}", ex4));
			}
			Plugin._coreInitialized = true;
			try
			{
				Loader.Initialize();
				kernelllogger.Msg("[KernelVRC] ✓ Loader initialized");
			}
			catch (Exception ex6)
			{
				Exception ex5 = ex6;
				kernelllogger.Error("[KernelVRC] Loader failed: " + ex5.Message);
			}
			MenuSetup.OnMenuSetupComplete += Plugin.OnMenuSetupCompleted;
			Plugin.RecordMemoryBaseline();
			Plugin._isInitialized = true;
			double totalTime = (DateTime.Now - Plugin._initStartTime).TotalMilliseconds;
			kernelllogger.Msg(string.Format("[KernelVRC] ✓ Complete initialization in {0:F0}ms!", totalTime));
			yield break;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0003C1F0 File Offset: 0x0003A3F0
		private static IEnumerator MemoryMonitoringCoroutine()
		{
			WaitForSeconds memoryCheckWait = new WaitForSeconds(30f);
			while (!Plugin._isShuttingDown)
			{
				try
				{
					DateTime now = DateTime.Now;
					bool flag = now - Plugin._lastMemoryCheck > Plugin.MEMORY_CHECK_INTERVAL;
					if (flag)
					{
						Plugin.CheckMemoryUsage();
						Plugin._lastMemoryCheck = now;
					}
					Plugin.PerformMaintenanceTasks();
					bool flag2 = now - Plugin._lastGCStatsLog > Plugin.GC_STATS_LOG_INTERVAL;
					if (flag2)
					{
						Plugin.LogGCStatistics();
						Plugin._lastGCStatsLog = now;
					}
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					kernelllogger.Error("[KernelVRC] Memory monitoring error: " + ex.Message);
				}
				yield return memoryCheckWait;
			}
			yield break;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0003C1F8 File Offset: 0x0003A3F8
		private static void CheckMemoryUsage()
		{
			try
			{
				long totalMemory = GC.GetTotalMemory(false);
				long num = totalMemory / 1048576L;
				Plugin._lastRecordedMemoryMB = num;
				bool flag = num > Plugin._peakMemoryMB;
				if (flag)
				{
					Plugin._peakMemoryMB = num;
				}
				DateTime now = DateTime.Now;
				bool flag2 = false;
				string reason = "";
				bool flag3 = num > Plugin.CRITICAL_MEMORY_THRESHOLD_MB;
				if (flag3)
				{
					flag2 = true;
					reason = string.Format("Critical memory usage: {0}MB > {1}MB", num, Plugin.CRITICAL_MEMORY_THRESHOLD_MB);
				}
				else
				{
					bool flag4 = num > Plugin.MEMORY_THRESHOLD_MB && now - Plugin._lastGCCollection > TimeSpan.FromMinutes(5.0);
					if (flag4)
					{
						flag2 = true;
						reason = string.Format("High memory usage: {0}MB > {1}MB", num, Plugin.MEMORY_THRESHOLD_MB);
					}
					else
					{
						bool flag5 = now - Plugin._lastGCCollection > Plugin.GC_COLLECTION_INTERVAL;
						if (flag5)
						{
							flag2 = true;
							reason = string.Format("Scheduled GC interval reached ({0} minutes)", Plugin.GC_COLLECTION_INTERVAL.TotalMinutes);
						}
					}
				}
				bool flag6 = flag2;
				if (flag6)
				{
					Plugin.PerformGarbageCollection(reason, true);
				}
				bool flag7 = now.Minute % 10 == 0 && now.Second < 30;
				if (flag7)
				{
					kernelllogger.Msg(string.Format("[KernelVRC] Memory Status: {0}MB (Peak: {1}MB)", num, Plugin._peakMemoryMB));
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernelVRC] Error checking memory usage: " + ex.Message);
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0003C394 File Offset: 0x0003A594
		private static void RecordMemoryBaseline()
		{
			try
			{
				long num = GC.GetTotalMemory(false) / 1048576L;
				Plugin._lastRecordedMemoryMB = num;
				Plugin._peakMemoryMB = num;
				kernelllogger.Msg(string.Format("[KernelVRC] Memory baseline: {0}MB", num));
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernelVRC] Error recording memory baseline: " + ex.Message);
			}
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0003C404 File Offset: 0x0003A604
		private static void PerformGarbageCollection(string reason = "Scheduled", bool forced = false)
		{
			try
			{
				DateTime now = DateTime.Now;
				long totalMemory = GC.GetTotalMemory(false);
				long num = totalMemory / 1048576L;
				kernelllogger.Msg(string.Format("[KernelVRC] Starting GC: {0} (Memory: {1}MB)", reason, num));
				Plugin.CleanupManagedResources();
				int num2 = GC.CollectionCount(0);
				int num3 = GC.CollectionCount(1);
				int num4 = GC.CollectionCount(2);
				GC.Collect(0, GCCollectionMode.Optimized);
				long totalMemory2 = GC.GetTotalMemory(true);
				long num5 = totalMemory2 / 1048576L;
				long num6 = num - num5;
				int num7 = GC.CollectionCount(0);
				int num8 = GC.CollectionCount(1);
				int num9 = GC.CollectionCount(2);
				TimeSpan timeSpan = DateTime.Now - now;
				Plugin._lastGCCollection = DateTime.Now;
				Plugin._totalGCCollections++;
				if (forced)
				{
					Plugin._forcedGCCollections++;
				}
				kernelllogger.Msg(string.Format("[KernelVRC] GC Complete: {0}MB → {1}MB ", num, num5) + string.Format("(Freed: {0}MB) in {1:F0}ms", num6, timeSpan.TotalMilliseconds));
				kernelllogger.Msg(string.Format("[KernelVRC] GC Collections - Gen0: +{0}, ", num7 - num2) + string.Format("Gen1: +{0}, Gen2: +{1}", num8 - num3, num9 - num4));
				bool flag = num6 > 500L;
				if (flag)
				{
					try
					{
						GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
						kernelllogger.Msg("[KernelVRC] Large Object Heap compacted");
					}
					catch (Exception ex)
					{
						kernelllogger.Warning("[KernelVRC] LOH compaction failed: " + ex.Message);
					}
				}
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("[KernelVRC] GC error: " + ex2.Message);
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0003C5E4 File Offset: 0x0003A7E4
		private static void LogGCStatistics()
		{
			try
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["CurrentMemoryMB"] = Plugin._lastRecordedMemoryMB;
				dictionary["PeakMemoryMB"] = Plugin._peakMemoryMB;
				dictionary["TotalGCRuns"] = Plugin._totalGCCollections;
				dictionary["ForcedGCRuns"] = Plugin._forcedGCCollections;
				dictionary["Gen0Collections"] = GC.CollectionCount(0);
				dictionary["Gen1Collections"] = GC.CollectionCount(1);
				dictionary["Gen2Collections"] = GC.CollectionCount(2);
				dictionary["LastGCTime"] = Plugin._lastGCCollection.ToString("HH:mm:ss");
				dictionary["NextScheduledGC"] = (Plugin._lastGCCollection + Plugin.GC_COLLECTION_INTERVAL).ToString("HH:mm:ss");
				kernelllogger.Msg(string.Format("[KernelVRC] GC Stats - Memory: {0}MB/{1}MB peak, ", Plugin._lastRecordedMemoryMB, Plugin._peakMemoryMB) + string.Format("Collections: {0} total ({1} forced), ", Plugin._totalGCCollections, Plugin._forcedGCCollections) + string.Format("Generations: {0}/{1}/{2}", GC.CollectionCount(0), GC.CollectionCount(1), GC.CollectionCount(2)));
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernelVRC] Error logging GC statistics: " + ex.Message);
			}
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0003C788 File Offset: 0x0003A988
		public static Dictionary<string, object> GetMemoryStatistics()
		{
			Dictionary<string, object> result;
			try
			{
				long totalMemory = GC.GetTotalMemory(false);
				long num = 0L;
				try
				{
					using (Process currentProcess = Process.GetCurrentProcess())
					{
						num = currentProcess.WorkingSet64;
					}
				}
				catch
				{
				}
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["GCMemoryMB"] = totalMemory / 1048576L;
				dictionary["ProcessMemoryMB"] = num / 1048576L;
				dictionary["PeakMemoryMB"] = Plugin._peakMemoryMB;
				dictionary["TotalGCCollections"] = Plugin._totalGCCollections;
				dictionary["ForcedGCCollections"] = Plugin._forcedGCCollections;
				dictionary["Gen0Collections"] = GC.CollectionCount(0);
				dictionary["Gen1Collections"] = GC.CollectionCount(1);
				dictionary["Gen2Collections"] = GC.CollectionCount(2);
				dictionary["LastGCTime"] = Plugin._lastGCCollection;
				dictionary["TimeSinceLastGC"] = (DateTime.Now - Plugin._lastGCCollection).TotalMinutes;
				dictionary["NextScheduledGC"] = Plugin._lastGCCollection + Plugin.GC_COLLECTION_INTERVAL;
				dictionary["MemoryThresholdMB"] = Plugin.MEMORY_THRESHOLD_MB;
				dictionary["CriticalThresholdMB"] = Plugin.CRITICAL_MEMORY_THRESHOLD_MB;
				dictionary["ManagedResources"] = Plugin._managedResources.Count;
				dictionary["DisposalQueueSize"] = Plugin._disposalQueue.Count;
				result = dictionary;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernelVRC] Error getting memory statistics: " + ex.Message);
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2["Error"] = ex.Message;
				result = dictionary2;
			}
			return result;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0003C9CC File Offset: 0x0003ABCC
		private static void PerformMaintenanceTasks()
		{
			DateTime now = DateTime.Now;
			bool flag = now - Plugin._lastModuleCleanup > Plugin.MODULE_CLEANUP_INTERVAL;
			if (flag)
			{
				Plugin._lastModuleCleanup = now;
				Plugin.CleanupModules();
			}
			Plugin.ProcessDisposalQueue();
			Plugin.CleanupWeakReferences();
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0003CA14 File Offset: 0x0003AC14
		private static void CleanupManagedResources()
		{
			List<WeakReference> list = new List<WeakReference>();
			foreach (WeakReference weakReference in Enumerable.ToArray<WeakReference>(Plugin._managedResources))
			{
				bool flag = !weakReference.IsAlive;
				if (flag)
				{
					list.Add(weakReference);
				}
				else
				{
					Object @object = weakReference.Target as Object;
					bool flag2 = @object != null && @object == null;
					if (flag2)
					{
						list.Add(weakReference);
					}
				}
			}
			foreach (WeakReference weakReference2 in list)
			{
				Plugin._managedResources.Remove(weakReference2);
			}
			bool flag3 = list.Count > 0;
			if (flag3)
			{
				kernelllogger.Msg(string.Format("[KernelVRC] Cleaned up {0} dead resource references", list.Count));
			}
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0003CB08 File Offset: 0x0003AD08
		private static void ProcessDisposalQueue()
		{
			int num = 0;
			while (Plugin._disposalQueue.Count > 0 && num < Plugin.MAX_DISPOSAL_PER_FRAME)
			{
				WeakReference weakReference = Plugin._disposalQueue.Dequeue();
				IDisposable disposable;
				bool flag;
				if (weakReference.IsAlive)
				{
					disposable = (weakReference.Target as IDisposable);
					flag = (disposable != null);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				if (flag2)
				{
					try
					{
						disposable.Dispose();
					}
					catch (Exception ex)
					{
						kernelllogger.Warning("[KernelVRC] Error disposing resource: " + ex.Message);
					}
				}
				num++;
			}
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0003CBA4 File Offset: 0x0003ADA4
		private static void CleanupWeakReferences()
		{
			object modulesLock = Plugin._modulesLock;
			lock (modulesLock)
			{
				List<Type> list = new List<Type>();
				foreach (KeyValuePair<Type, WeakReference> keyValuePair in Enumerable.ToArray<KeyValuePair<Type, WeakReference>>(Plugin._moduleTypeCache))
				{
					bool flag2 = !keyValuePair.Value.IsAlive;
					if (flag2)
					{
						list.Add(keyValuePair.Key);
					}
				}
				foreach (Type key in list)
				{
					Plugin._moduleTypeCache.Remove(key);
				}
			}
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0003CC80 File Offset: 0x0003AE80
		private static void OnMenuSetupCompleted()
		{
			kernelllogger.Msg("[KernelVRC] MenuSetup completed callback received");
			Plugin._menuSetupReady = true;
			bool flag = !Plugin._moduleUIInitialized && Plugin._coreInitialized;
			if (flag)
			{
				MelonCoroutines.Start(Plugin.DelayedUIInitialization());
			}
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0003CCC5 File Offset: 0x0003AEC5
		private static IEnumerator DelayedUIInitialization()
		{
			yield return new WaitForSeconds(0.5f);
			bool flag = !MenuSetup.IsReady;
			if (flag)
			{
				kernelllogger.Warning("[KernelVRC] MenuSetup not ready, waiting...");
				float waitTime = 0f;
				while (!MenuSetup.IsReady && waitTime < 10f)
				{
					yield return new WaitForSeconds(0.5f);
					waitTime += 0.5f;
				}
			}
			bool isReady = MenuSetup.IsReady;
			if (isReady)
			{
				Plugin.InitializeModuleUIs();
			}
			else
			{
				kernelllogger.Error("[KernelVRC] MenuSetup timeout - proceeding without UI initialization");
			}
			yield break;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0003CCD0 File Offset: 0x0003AED0
		private static void InitializeModuleUIs()
		{
			bool flag = Plugin._moduleUIInitialized || Plugin._isShuttingDown;
			if (!flag)
			{
				object modulesLock = Plugin._modulesLock;
				lock (modulesLock)
				{
					bool flag3 = Plugin._moduleUIInitialized || Plugin._isShuttingDown;
					if (!flag3)
					{
						kernelllogger.Msg("[KernelVRC] Starting module UI initialization...");
						int num = 0;
						int num2 = 0;
						List<IKernelModule> modulesSortedByPriority = Plugin.GetModulesSortedByPriority();
						foreach (IKernelModule kernelModule in modulesSortedByPriority)
						{
							bool isShuttingDown = Plugin._isShuttingDown;
							if (isShuttingDown)
							{
								break;
							}
							bool flag4 = kernelModule != null && kernelModule.IsEnabled && (kernelModule.Capabilities & ModuleCapabilities.UIInit) > ModuleCapabilities.None;
							if (flag4)
							{
								num2++;
								try
								{
									kernelModule.OnUiManagerInit();
									num++;
									Plugin._moduleLastActivity[kernelModule] = DateTime.Now;
									kernelllogger.Msg("[KernelVRC] ✓ UI initialized for: " + kernelModule.ModuleName);
								}
								catch (Exception ex)
								{
									Plugin.RecordModuleError(kernelModule, ex);
									kernelllogger.Error("[KernelVRC] UI init failed for " + kernelModule.ModuleName + ": " + ex.Message);
								}
							}
						}
						Plugin._moduleUIInitialized = true;
						kernelllogger.Msg(string.Format("[KernelVRC] UI initialization complete: {0}/{1} modules", num, num2));
					}
				}
			}
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x0003CEA4 File Offset: 0x0003B0A4
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			bool isShuttingDown = Plugin._isShuttingDown;
			if (!isShuttingDown)
			{
				kernelllogger.Msg(string.Format("[KernelVRC] Scene loaded: {0} (Build Index: {1})", sceneName, buildIndex));
				bool flag = !Plugin._coreInitialized;
				if (!flag)
				{
					try
					{
						List<IKernelModule> modulesSortedByPriority = Plugin.GetModulesSortedByPriority();
						foreach (IKernelModule kernelModule in modulesSortedByPriority)
						{
							bool isShuttingDown2 = Plugin._isShuttingDown;
							if (isShuttingDown2)
							{
								break;
							}
							bool flag2 = kernelModule != null && kernelModule.IsEnabled && (kernelModule.Capabilities & ModuleCapabilities.SceneEvents) > ModuleCapabilities.None;
							if (flag2)
							{
								try
								{
									kernelModule.OnSceneWasLoaded(buildIndex, sceneName);
									Plugin._moduleLastActivity[kernelModule] = DateTime.Now;
								}
								catch (Exception ex)
								{
									Plugin.RecordModuleError(kernelModule, ex);
									kernelllogger.Error("[" + kernelModule.ModuleName + "] Scene error: " + ex.Message);
								}
							}
						}
						Loader.OnSceneLoaded(buildIndex, sceneName);
						MelonCoroutines.Start(Plugin.DelayedMaintenanceAfterScene());
					}
					catch (Exception arg)
					{
						kernelllogger.Error(string.Format("[KernelVRC] OnSceneWasLoaded error: {0}", arg));
					}
				}
			}
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0003D00C File Offset: 0x0003B20C
		private static IEnumerator DelayedMaintenanceAfterScene()
		{
			yield return new WaitForSeconds(5f);
			Plugin.PerformMaintenanceTasks();
			Plugin.CheckMemoryUsage();
			yield break;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0003D014 File Offset: 0x0003B214
		private static bool LoadModulesOptimized()
		{
			bool result;
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				Type typeFromHandle = typeof(IKernelModule);
				Type typeFromHandle2 = typeof(KernelModuleBase);
				Type typeFromHandle3 = typeof(ComponentDisabled);
				Type[] types = executingAssembly.GetTypes();
				List<ValueTuple<IKernelModule, ModulePriority>> list = new List<ValueTuple<IKernelModule, ModulePriority>>();
				foreach (Type type in types)
				{
					bool isShuttingDown = Plugin._isShuttingDown;
					if (isShuttingDown)
					{
						break;
					}
					bool flag = type.IsAbstract || !type.IsClass || type.IsDefined(typeFromHandle3, false);
					if (!flag)
					{
						bool flag2 = typeFromHandle.IsAssignableFrom(type) || type.IsSubclassOf(typeFromHandle2);
						bool flag3 = flag2;
						if (flag3)
						{
							try
							{
								IKernelModule kernelModule = Activator.CreateInstance(type) as IKernelModule;
								bool flag4 = kernelModule != null;
								if (flag4)
								{
									list.Add(new ValueTuple<IKernelModule, ModulePriority>(kernelModule, kernelModule.Priority));
									Plugin._moduleLastActivity[kernelModule] = DateTime.Now;
									Plugin._moduleErrorCounts[kernelModule] = 0;
								}
							}
							catch (Exception ex)
							{
								kernelllogger.Error("[KernelVRC] Failed to create " + type.Name + ": " + ex.Message);
							}
						}
					}
				}
				list.Sort(([TupleElementNames(new string[]
				{
					"Instance",
					"Priority"
				})] ValueTuple<IKernelModule, ModulePriority> a, [TupleElementNames(new string[]
				{
					"Instance",
					"Priority"
				})] ValueTuple<IKernelModule, ModulePriority> b) => b.Item2.CompareTo(a.Item2));
				int num = 0;
				foreach (ValueTuple<IKernelModule, ModulePriority> valueTuple in list)
				{
					IKernelModule item = valueTuple.Item1;
					ModulePriority item2 = valueTuple.Item2;
					bool isShuttingDown2 = Plugin._isShuttingDown;
					if (isShuttingDown2)
					{
						break;
					}
					try
					{
						Plugin.RegisterModuleOptimized(item);
						num++;
						kernelllogger.Msg(string.Format("[KernelVRC] ✓ Loaded: {0} (Priority: {1})", item.ModuleName, item2));
					}
					catch (Exception ex2)
					{
						Plugin.RecordModuleError(item, ex2);
						kernelllogger.Error("[KernelVRC] Failed to register " + item.ModuleName + ": " + ex2.Message);
					}
				}
				kernelllogger.Msg(string.Format("[KernelVRC] Module loading complete: {0} modules loaded", num));
				result = (num > 0);
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[KernelVRC] Module discovery failed: {0}", arg));
				result = false;
			}
			return result;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0003D2E0 File Offset: 0x0003B4E0
		private static void RegisterModuleOptimized(IKernelModule module)
		{
			bool flag = module == null || Plugin._isShuttingDown;
			if (!flag)
			{
				object modulesLock = Plugin._modulesLock;
				lock (modulesLock)
				{
					Plugin._modules.Add(module);
					Plugin._moduleTypeCache[module.GetType()] = new WeakReference(module);
					ModuleRegistry.Register(module);
					bool flag3 = (module.Capabilities & ModuleCapabilities.AllNetworkEvents) != ModuleCapabilities.None || (module.Capabilities & ModuleCapabilities.AllPlayerEvents) > ModuleCapabilities.None;
					if (flag3)
					{
						try
						{
							Patches.RegisterModule(module);
						}
						catch (Exception ex)
						{
							Plugin.RecordModuleError(module, ex);
							kernelllogger.Error("[KernelVRC] Patch registration failed for " + module.ModuleName + ": " + ex.Message);
						}
					}
				}
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0003D3CC File Offset: 0x0003B5CC
		private static void InitializeModuleSubscriptions()
		{
			object modulesLock = Plugin._modulesLock;
			lock (modulesLock)
			{
				List<IKernelModule> list = new List<IKernelModule>();
				List<IKernelModule> list2 = new List<IKernelModule>();
				List<IKernelModule> list3 = new List<IKernelModule>();
				Plugin._frequencyGroups.Clear();
				Plugin._priorityGroups.Clear();
				foreach (IKernelModule kernelModule in Plugin._modules.ToArray())
				{
					bool flag2 = kernelModule == null || !kernelModule.IsEnabled || Plugin._isShuttingDown;
					if (!flag2)
					{
						ModulePriority priority = kernelModule.Priority;
						bool flag3 = !Plugin._priorityGroups.ContainsKey(priority);
						if (flag3)
						{
							Plugin._priorityGroups[priority] = new List<IKernelModule>();
						}
						Plugin._priorityGroups[priority].Add(kernelModule);
						ModuleCapabilities capabilities = kernelModule.Capabilities;
						bool flag4 = (capabilities & ModuleCapabilities.Update) > ModuleCapabilities.None;
						if (flag4)
						{
							list.Add(kernelModule);
							UpdateFrequency updateFrequency = kernelModule.UpdateFrequency;
							bool flag5 = !Plugin._frequencyGroups.ContainsKey(updateFrequency);
							if (flag5)
							{
								Plugin._frequencyGroups[updateFrequency] = new List<IKernelModule>();
							}
							Plugin._frequencyGroups[updateFrequency].Add(kernelModule);
						}
						bool flag6 = (capabilities & ModuleCapabilities.LateUpdate) > ModuleCapabilities.None;
						if (flag6)
						{
							list2.Add(kernelModule);
						}
						bool flag7 = (capabilities & ModuleCapabilities.GUI) > ModuleCapabilities.None;
						if (flag7)
						{
							list3.Add(kernelModule);
						}
					}
				}
				Plugin._updateModules = Plugin.SortByPriority(list).ToArray();
				Plugin._lateUpdateModules = Plugin.SortByPriority(list2).ToArray();
				Plugin._guiModules = Plugin.SortByPriority(list3).ToArray();
				kernelllogger.Msg(string.Format("[KernelVRC] Subscriptions: Update={0}, LateUpdate={1}, GUI={2}", Plugin._updateModules.Length, Plugin._lateUpdateModules.Length, Plugin._guiModules.Length));
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0003D5D4 File Offset: 0x0003B7D4
		private static void CleanupModules()
		{
			bool isShuttingDown = Plugin._isShuttingDown;
			if (!isShuttingDown)
			{
				object modulesLock = Plugin._modulesLock;
				lock (modulesLock)
				{
					List<IKernelModule> list = new List<IKernelModule>();
					DateTime now = DateTime.Now;
					foreach (IKernelModule kernelModule in Plugin._modules.ToArray())
					{
						bool flag2 = kernelModule == null;
						if (flag2)
						{
							list.Add(kernelModule);
						}
						else
						{
							int num;
							bool flag3 = Plugin._moduleErrorCounts.TryGetValue(kernelModule, out num) && num >= Plugin.MAX_MODULE_ERRORS;
							if (flag3)
							{
								kernelllogger.Warning(string.Format("[KernelVRC] Disabling module {0} due to excessive errors ({1})", kernelModule.ModuleName, num));
								list.Add(kernelModule);
							}
							else
							{
								DateTime d;
								bool flag4 = Plugin._moduleLastActivity.TryGetValue(kernelModule, out d) && (now - d).TotalMinutes > 30.0;
								if (flag4)
								{
									kernelllogger.Msg(string.Format("[KernelVRC] Module {0} inactive for {1:F1} minutes", kernelModule.ModuleName, (now - d).TotalMinutes));
								}
							}
						}
					}
					foreach (IKernelModule module in list)
					{
						Plugin.RemoveModule(module);
					}
					bool flag5 = list.Count > 0;
					if (flag5)
					{
						Plugin.InitializeModuleSubscriptions();
						kernelllogger.Msg(string.Format("[KernelVRC] Removed {0} problematic modules", list.Count));
					}
				}
			}
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0003D7BC File Offset: 0x0003B9BC
		private static void RemoveModule(IKernelModule module)
		{
			bool flag = module == null;
			if (!flag)
			{
				try
				{
					Plugin._modules.Remove(module);
					Plugin._moduleTypeCache.Remove(module.GetType());
					Plugin._moduleLastActivity.Remove(module);
					Plugin._moduleErrorCounts.Remove(module);
					foreach (List<IKernelModule> list in Plugin._priorityGroups.Values)
					{
						list.Remove(module);
					}
					foreach (List<IKernelModule> list2 in Plugin._frequencyGroups.Values)
					{
						list2.Remove(module);
					}
					IDisposable disposable = module as IDisposable;
					bool flag2 = disposable != null;
					if (flag2)
					{
						Plugin._disposalQueue.Enqueue(new WeakReference(disposable));
					}
					ModuleRegistry.Unregister(module);
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[KernelVRC] Error removing module " + ((module != null) ? module.ModuleName : null) + ": " + ex.Message);
				}
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0003D914 File Offset: 0x0003BB14
		private static void RecordModuleError(IKernelModule module, Exception ex)
		{
			bool flag = module == null;
			if (!flag)
			{
				bool flag2 = !Plugin._moduleErrorCounts.ContainsKey(module);
				if (flag2)
				{
					Plugin._moduleErrorCounts[module] = 0;
				}
				Dictionary<IKernelModule, int> moduleErrorCounts = Plugin._moduleErrorCounts;
				int num = moduleErrorCounts[module];
				moduleErrorCounts[module] = num + 1;
				bool flag3 = Plugin._moduleErrorCounts[module] >= Plugin.MAX_MODULE_ERRORS;
				if (flag3)
				{
					kernelllogger.Error("[KernelVRC] Module " + module.ModuleName + " has reached error limit and will be disabled");
				}
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0003D99C File Offset: 0x0003BB9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IKernelModule[] GetAllModules()
		{
			object modulesLock = Plugin._modulesLock;
			IKernelModule[] result;
			lock (modulesLock)
			{
				result = Plugin._modules.ToArray();
			}
			return result;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0003D9E8 File Offset: 0x0003BBE8
		private static List<IKernelModule> SortByPriority(List<IKernelModule> modules)
		{
			List<IKernelModule> list = new List<IKernelModule>(modules);
			list.Sort((IKernelModule a, IKernelModule b) => b.Priority.CompareTo(a.Priority));
			return list;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0003DA28 File Offset: 0x0003BC28
		private static List<IKernelModule> GetModulesSortedByPriority()
		{
			object modulesLock = Plugin._modulesLock;
			List<IKernelModule> result;
			lock (modulesLock)
			{
				result = Plugin.SortByPriority(new List<IKernelModule>(Enumerable.Where<IKernelModule>(Plugin._modules, (IKernelModule m) => m != null)));
			}
			return result;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0003DA9C File Offset: 0x0003BC9C
		private static void CallModulesInPriorityOrder(Action<IKernelModule> action)
		{
			bool isShuttingDown = Plugin._isShuttingDown;
			if (!isShuttingDown)
			{
				List<IKernelModule> modulesSortedByPriority = Plugin.GetModulesSortedByPriority();
				foreach (IKernelModule kernelModule in modulesSortedByPriority)
				{
					bool isShuttingDown2 = Plugin._isShuttingDown;
					if (isShuttingDown2)
					{
						break;
					}
					bool flag = kernelModule != null && kernelModule.IsEnabled;
					if (flag)
					{
						try
						{
							action(kernelModule);
							Plugin._moduleLastActivity[kernelModule] = DateTime.Now;
						}
						catch (Exception ex)
						{
							Plugin.RecordModuleError(kernelModule, ex);
							kernelllogger.Error("[" + kernelModule.ModuleName + "] Error: " + ex.Message);
						}
					}
				}
			}
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0003DB80 File Offset: 0x0003BD80
		public override void OnUpdate()
		{
			bool flag = !Plugin._coreInitialized || Plugin._isShuttingDown;
			if (!flag)
			{
				Plugin._frameCount += 1U;
				try
				{
					Plugin.ProcessDisposalQueue();
					foreach (KeyValuePair<UpdateFrequency, List<IKernelModule>> keyValuePair in Enumerable.ToArray<KeyValuePair<UpdateFrequency, List<IKernelModule>>>(Plugin._frequencyGroups))
					{
						bool isShuttingDown = Plugin._isShuttingDown;
						if (isShuttingDown)
						{
							break;
						}
						bool flag2 = Plugin._frameCount % (uint)keyValuePair.Key == 0U;
						if (flag2)
						{
							foreach (IKernelModule kernelModule in keyValuePair.Value.ToArray())
							{
								bool isShuttingDown2 = Plugin._isShuttingDown;
								if (isShuttingDown2)
								{
									break;
								}
								bool flag3 = kernelModule != null && kernelModule.IsEnabled;
								if (flag3)
								{
									try
									{
										kernelModule.OnUpdate();
										Plugin._moduleLastActivity[kernelModule] = DateTime.Now;
									}
									catch (Exception ex)
									{
										Plugin.RecordModuleError(kernelModule, ex);
										kernelllogger.Error("[" + kernelModule.ModuleName + "] Update error: " + ex.Message);
									}
								}
							}
						}
					}
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[KernelVRC] OnUpdate error: {0}", arg));
				}
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0003DD0C File Offset: 0x0003BF0C
		public override void OnLateUpdate()
		{
			bool flag = !Plugin._coreInitialized || Plugin._isShuttingDown;
			if (!flag)
			{
				try
				{
					IKernelModule[] array = Enumerable.ToArray<IKernelModule>(Plugin._lateUpdateModules);
					foreach (IKernelModule kernelModule in array)
					{
						bool isShuttingDown = Plugin._isShuttingDown;
						if (isShuttingDown)
						{
							break;
						}
						bool flag2 = kernelModule != null && kernelModule.IsEnabled;
						if (flag2)
						{
							try
							{
								kernelModule.OnLateUpdate();
								Plugin._moduleLastActivity[kernelModule] = DateTime.Now;
							}
							catch (Exception ex)
							{
								Plugin.RecordModuleError(kernelModule, ex);
								kernelllogger.Error("[" + kernelModule.ModuleName + "] LateUpdate error: " + ex.Message);
							}
						}
					}
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[KernelVRC] OnLateUpdate error: {0}", arg));
				}
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0003DE10 File Offset: 0x0003C010
		public override void OnGUI()
		{
			bool flag = !Plugin._coreInitialized || Plugin._isShuttingDown;
			if (!flag)
			{
				try
				{
					IKernelModule[] array = Enumerable.ToArray<IKernelModule>(Plugin._guiModules);
					foreach (IKernelModule kernelModule in array)
					{
						bool isShuttingDown = Plugin._isShuttingDown;
						if (isShuttingDown)
						{
							break;
						}
						bool flag2 = kernelModule != null && kernelModule.IsEnabled;
						if (flag2)
						{
							try
							{
								kernelModule.OnGUI();
								Plugin._moduleLastActivity[kernelModule] = DateTime.Now;
							}
							catch (Exception ex)
							{
								Plugin.RecordModuleError(kernelModule, ex);
								kernelllogger.Error("[" + kernelModule.ModuleName + "] OnGUI error: " + ex.Message);
							}
						}
					}
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[KernelVRC] OnGUI error: {0}", arg));
				}
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0003DF14 File Offset: 0x0003C114
		public override void OnApplicationQuit()
		{
			kernelllogger.Msg("[KernelVRC] Shutting down...");
			Plugin._isShuttingDown = true;
			try
			{
				Plugin.CallModulesInPriorityOrder(delegate(IKernelModule m)
				{
					m.OnShutdown();
				});
				try
				{
					Patches.Shutdown();
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[KernelVRC] Patches shutdown error: " + ex.Message);
				}
				kernelllogger.Msg("[KernelVRC] Performing final garbage collection...");
				Plugin.PerformGarbageCollection("Shutdown", true);
				Plugin.CleanupAllResources();
				object modulesLock = Plugin._modulesLock;
				lock (modulesLock)
				{
					Plugin._modules.Clear();
					Plugin._moduleTypeCache.Clear();
					Plugin._moduleLastActivity.Clear();
					Plugin._moduleErrorCounts.Clear();
					Plugin._frequencyGroups.Clear();
					Plugin._priorityGroups.Clear();
					Plugin._updateModules = new IKernelModule[0];
					Plugin._lateUpdateModules = new IKernelModule[0];
					Plugin._guiModules = new IKernelModule[0];
				}
				ModuleRegistry.Clear();
				Plugin.LogGCStatistics();
				kernelllogger.Msg("[KernelVRC] Shutdown complete");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[KernelVRC] Shutdown error: {0}", arg));
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0003E09C File Offset: 0x0003C29C
		private static void CleanupAllResources()
		{
			try
			{
				while (Plugin._disposalQueue.Count > 0)
				{
					WeakReference weakReference = Plugin._disposalQueue.Dequeue();
					IDisposable disposable;
					bool flag;
					if (weakReference.IsAlive)
					{
						disposable = (weakReference.Target as IDisposable);
						flag = (disposable != null);
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					if (flag2)
					{
						try
						{
							disposable.Dispose();
						}
						catch (Exception ex)
						{
							kernelllogger.Warning("[KernelVRC] Error disposing resource during shutdown: " + ex.Message);
						}
					}
				}
				Plugin._managedResources.Clear();
				kernelllogger.Msg("[KernelVRC] All resources cleaned up");
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("[KernelVRC] Error during resource cleanup: " + ex2.Message);
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0003E168 File Offset: 0x0003C368
		private static void FallbackInitialization()
		{
			try
			{
				kernelllogger.Warning("[KernelVRC] Fallback initialization...");
				Plugin.LoadModulesOptimized();
				Plugin.InitializeModuleSubscriptions();
				try
				{
					Patches.Initialize();
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[KernelVRC] Fallback patches initialization error: " + ex.Message);
				}
				try
				{
					Loader.Initialize();
				}
				catch
				{
				}
				Plugin.CallModulesInPriorityOrder(delegate(IKernelModule m)
				{
					m.OnApplicationStart();
				});
				MenuSetup.OnMenuSetupComplete += Plugin.OnMenuSetupCompleted;
				MelonCoroutines.Start(Plugin.MemoryMonitoringCoroutine());
				Plugin._coreInitialized = true;
				Plugin._isInitialized = true;
				kernelllogger.Warning("[KernelVRC] Fallback initialization complete");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[KernelVRC] Fallback failed: {0}", arg));
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060009DA RID: 2522 RVA: 0x0003E264 File Offset: 0x0003C464
		public static bool IsInitialized
		{
			get
			{
				return Plugin._isInitialized;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060009DB RID: 2523 RVA: 0x0003E26D File Offset: 0x0003C46D
		public static bool IsCoreInitialized
		{
			get
			{
				return Plugin._coreInitialized;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060009DC RID: 2524 RVA: 0x0003E276 File Offset: 0x0003C476
		public static bool IsModuleUIInitialized
		{
			get
			{
				return Plugin._moduleUIInitialized;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060009DD RID: 2525 RVA: 0x0003E27F File Offset: 0x0003C47F
		public static bool IsShuttingDown
		{
			get
			{
				return Plugin._isShuttingDown;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060009DE RID: 2526 RVA: 0x0003E288 File Offset: 0x0003C488
		public static int ModuleCount
		{
			get
			{
				return Plugin._modules.Count;
			}
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x0003E294 File Offset: 0x0003C494
		public static T GetModule<T>() where T : class, IKernelModule
		{
			object modulesLock = Plugin._modulesLock;
			T result;
			lock (modulesLock)
			{
				WeakReference weakReference;
				bool flag2 = Plugin._moduleTypeCache.TryGetValue(typeof(T), out weakReference) && weakReference.IsAlive;
				if (flag2)
				{
					result = (weakReference.Target as T);
				}
				else
				{
					result = Enumerable.FirstOrDefault<T>(Enumerable.OfType<T>(Plugin._modules));
				}
			}
			return result;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x0003E320 File Offset: 0x0003C520
		public static void ForceGarbageCollection()
		{
			Plugin.PerformGarbageCollection("Manual Force", true);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x0003E330 File Offset: 0x0003C530
		public static Dictionary<string, object> GetModuleStatistics()
		{
			object modulesLock = Plugin._modulesLock;
			Dictionary<string, object> result;
			lock (modulesLock)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary["TotalModules"] = Plugin._modules.Count;
				dictionary["EnabledModules"] = Enumerable.Count<IKernelModule>(Plugin._modules, (IKernelModule m) => m != null && m.IsEnabled);
				dictionary["UpdateModules"] = Plugin._updateModules.Length;
				dictionary["LateUpdateModules"] = Plugin._lateUpdateModules.Length;
				dictionary["GUIModules"] = Plugin._guiModules.Length;
				dictionary["ModulesWithErrors"] = Enumerable.Count<KeyValuePair<IKernelModule, int>>(Plugin._moduleErrorCounts, (KeyValuePair<IKernelModule, int> kvp) => kvp.Value > 0);
				dictionary["TotalErrors"] = Enumerable.Sum(Plugin._moduleErrorCounts.Values);
				Dictionary<string, object> dictionary2 = dictionary;
				result = dictionary2;
			}
			return result;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x0003E47C File Offset: 0x0003C67C
		public static Dictionary<string, object> GetSystemStatistics()
		{
			Dictionary<string, object> moduleStatistics = Plugin.GetModuleStatistics();
			Dictionary<string, object> memoryStatistics = Plugin.GetMemoryStatistics();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> keyValuePair in moduleStatistics)
			{
				dictionary["Module_" + keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (KeyValuePair<string, object> keyValuePair2 in memoryStatistics)
			{
				dictionary["Memory_" + keyValuePair2.Key] = keyValuePair2.Value;
			}
			dictionary["System_IsInitialized"] = Plugin._isInitialized;
			dictionary["System_CoreInitialized"] = Plugin._coreInitialized;
			dictionary["System_UIInitialized"] = Plugin._moduleUIInitialized;
			dictionary["System_FrameCount"] = Plugin._frameCount;
			dictionary["System_Uptime"] = (Plugin._isInitialized ? (DateTime.Now - Plugin._initStartTime).TotalMinutes : 0.0);
			return dictionary;
		}

		// Token: 0x040004EF RID: 1263
		private static volatile bool _isInitialized;

		// Token: 0x040004F0 RID: 1264
		private static volatile bool _initializationStarted;

		// Token: 0x040004F1 RID: 1265
		private static volatile bool _moduleUIInitialized;

		// Token: 0x040004F2 RID: 1266
		private static volatile bool _coreInitialized;

		// Token: 0x040004F3 RID: 1267
		private static volatile bool _menuSetupReady;

		// Token: 0x040004F4 RID: 1268
		private static volatile bool _isShuttingDown;

		// Token: 0x040004F5 RID: 1269
		private static readonly object _initLock = new object();

		// Token: 0x040004F6 RID: 1270
		private static DateTime _initStartTime;

		// Token: 0x040004F7 RID: 1271
		private static DateTime _lastGCCollection = DateTime.MinValue;

		// Token: 0x040004F8 RID: 1272
		private static DateTime _lastMemoryCheck = DateTime.MinValue;

		// Token: 0x040004F9 RID: 1273
		private static readonly TimeSpan GC_COLLECTION_INTERVAL = TimeSpan.FromMinutes(20.0);

		// Token: 0x040004FA RID: 1274
		private static readonly TimeSpan MEMORY_CHECK_INTERVAL = TimeSpan.FromMinutes(2.0);

		// Token: 0x040004FB RID: 1275
		private static readonly long MEMORY_THRESHOLD_MB = 2048L;

		// Token: 0x040004FC RID: 1276
		private static readonly long CRITICAL_MEMORY_THRESHOLD_MB = 3072L;

		// Token: 0x040004FD RID: 1277
		private static readonly TimeSpan MODULE_CLEANUP_INTERVAL = TimeSpan.FromMinutes(5.0);

		// Token: 0x040004FE RID: 1278
		private static DateTime _lastModuleCleanup = DateTime.MinValue;

		// Token: 0x040004FF RID: 1279
		private static int _totalGCCollections = 0;

		// Token: 0x04000500 RID: 1280
		private static int _forcedGCCollections = 0;

		// Token: 0x04000501 RID: 1281
		private static long _lastRecordedMemoryMB = 0L;

		// Token: 0x04000502 RID: 1282
		private static long _peakMemoryMB = 0L;

		// Token: 0x04000503 RID: 1283
		private static DateTime _lastGCStatsLog = DateTime.MinValue;

		// Token: 0x04000504 RID: 1284
		private static readonly TimeSpan GC_STATS_LOG_INTERVAL = TimeSpan.FromMinutes(10.0);

		// Token: 0x04000505 RID: 1285
		private static readonly List<IKernelModule> _modules = new List<IKernelModule>(32);

		// Token: 0x04000506 RID: 1286
		private static readonly object _modulesLock = new object();

		// Token: 0x04000507 RID: 1287
		private static readonly Dictionary<Type, WeakReference> _moduleTypeCache = new Dictionary<Type, WeakReference>(32);

		// Token: 0x04000508 RID: 1288
		private static IKernelModule[] _updateModules = new IKernelModule[0];

		// Token: 0x04000509 RID: 1289
		private static IKernelModule[] _lateUpdateModules = new IKernelModule[0];

		// Token: 0x0400050A RID: 1290
		private static IKernelModule[] _guiModules = new IKernelModule[0];

		// Token: 0x0400050B RID: 1291
		private static readonly Dictionary<ModulePriority, List<IKernelModule>> _priorityGroups = new Dictionary<ModulePriority, List<IKernelModule>>(5);

		// Token: 0x0400050C RID: 1292
		private static readonly Dictionary<UpdateFrequency, List<IKernelModule>> _frequencyGroups = new Dictionary<UpdateFrequency, List<IKernelModule>>(8);

		// Token: 0x0400050D RID: 1293
		private static uint _frameCount;

		// Token: 0x0400050E RID: 1294
		private static readonly Dictionary<IKernelModule, DateTime> _moduleLastActivity = new Dictionary<IKernelModule, DateTime>();

		// Token: 0x0400050F RID: 1295
		private static readonly Dictionary<IKernelModule, int> _moduleErrorCounts = new Dictionary<IKernelModule, int>();

		// Token: 0x04000510 RID: 1296
		private static readonly int MAX_MODULE_ERRORS = 10;

		// Token: 0x04000511 RID: 1297
		private static readonly HashSet<WeakReference> _managedResources = new HashSet<WeakReference>();

		// Token: 0x04000512 RID: 1298
		private static readonly Queue<WeakReference> _disposalQueue = new Queue<WeakReference>();

		// Token: 0x04000513 RID: 1299
		private static readonly int MAX_DISPOSAL_PER_FRAME = 5;
	}
}
