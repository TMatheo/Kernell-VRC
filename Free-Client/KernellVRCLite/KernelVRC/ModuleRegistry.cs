using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KernelVRC
{
	// Token: 0x020000B4 RID: 180
	public static class ModuleRegistry
	{
		// Token: 0x06000949 RID: 2377 RVA: 0x000391A4 File Offset: 0x000373A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Register(IKernelModule module)
		{
			bool flag = module == null;
			if (!flag)
			{
				object registryLock = ModuleRegistry._registryLock;
				lock (registryLock)
				{
					Type type = module.GetType();
					ModuleRegistry._typeCache[type] = module;
					ModuleRegistry._nameCache[module.ModuleName] = module;
				}
			}
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x00039214 File Offset: 0x00037414
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Unregister(IKernelModule module)
		{
			bool flag = module == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object registryLock = ModuleRegistry._registryLock;
				lock (registryLock)
				{
					Type type = module.GetType();
					bool flag3 = ModuleRegistry._typeCache.Remove(type);
					bool flag4 = ModuleRegistry._nameCache.Remove(module.ModuleName);
					result = (flag3 || flag4);
				}
			}
			return result;
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x0003928C File Offset: 0x0003748C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool UnregisterByType<T>() where T : class, IKernelModule
		{
			object registryLock = ModuleRegistry._registryLock;
			bool result;
			lock (registryLock)
			{
				Type typeFromHandle = typeof(T);
				IKernelModule kernelModule;
				bool flag2 = ModuleRegistry._typeCache.TryGetValue(typeFromHandle, out kernelModule);
				if (flag2)
				{
					ModuleRegistry._typeCache.Remove(typeFromHandle);
					bool flag3 = kernelModule != null;
					if (flag3)
					{
						ModuleRegistry._nameCache.Remove(kernelModule.ModuleName);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0003931C File Offset: 0x0003751C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool UnregisterByName(string name)
		{
			bool flag = string.IsNullOrEmpty(name);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object registryLock = ModuleRegistry._registryLock;
				lock (registryLock)
				{
					IKernelModule kernelModule;
					bool flag3 = ModuleRegistry._nameCache.TryGetValue(name, out kernelModule);
					if (flag3)
					{
						ModuleRegistry._nameCache.Remove(name);
						bool flag4 = kernelModule != null;
						if (flag4)
						{
							ModuleRegistry._typeCache.Remove(kernelModule.GetType());
						}
						result = true;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x000393B0 File Offset: 0x000375B0
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T Get<T>() where T : class, IKernelModule
		{
			object registryLock = ModuleRegistry._registryLock;
			T result;
			lock (registryLock)
			{
				IKernelModule kernelModule;
				result = (ModuleRegistry._typeCache.TryGetValue(typeof(T), out kernelModule) ? (kernelModule as T) : default(T));
			}
			return result;
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x00039420 File Offset: 0x00037620
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static IKernelModule Get(string name)
		{
			bool flag = string.IsNullOrEmpty(name);
			IKernelModule result;
			if (flag)
			{
				result = null;
			}
			else
			{
				object registryLock = ModuleRegistry._registryLock;
				lock (registryLock)
				{
					IKernelModule kernelModule;
					result = (ModuleRegistry._nameCache.TryGetValue(name, out kernelModule) ? kernelModule : null);
				}
			}
			return result;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00039484 File Offset: 0x00037684
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsRegistered<T>() where T : class, IKernelModule
		{
			object registryLock = ModuleRegistry._registryLock;
			bool result;
			lock (registryLock)
			{
				result = ModuleRegistry._typeCache.ContainsKey(typeof(T));
			}
			return result;
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x000394D8 File Offset: 0x000376D8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsRegistered(string name)
		{
			bool flag = string.IsNullOrEmpty(name);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object registryLock = ModuleRegistry._registryLock;
				lock (registryLock)
				{
					result = ModuleRegistry._nameCache.ContainsKey(name);
				}
			}
			return result;
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x00039530 File Offset: 0x00037730
		public static int Count
		{
			get
			{
				object registryLock = ModuleRegistry._registryLock;
				int count;
				lock (registryLock)
				{
					count = ModuleRegistry._typeCache.Count;
				}
				return count;
			}
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0003957C File Offset: 0x0003777C
		public static IKernelModule[] GetAllModules()
		{
			object registryLock = ModuleRegistry._registryLock;
			IKernelModule[] result;
			lock (registryLock)
			{
				IKernelModule[] array = new IKernelModule[ModuleRegistry._typeCache.Count];
				ModuleRegistry._typeCache.Values.CopyTo(array, 0);
				result = array;
			}
			return result;
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x000395E0 File Offset: 0x000377E0
		public static string[] GetAllModuleNames()
		{
			object registryLock = ModuleRegistry._registryLock;
			string[] result;
			lock (registryLock)
			{
				string[] array = new string[ModuleRegistry._nameCache.Count];
				ModuleRegistry._nameCache.Keys.CopyTo(array, 0);
				result = array;
			}
			return result;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x00039644 File Offset: 0x00037844
		public static void Clear()
		{
			object registryLock = ModuleRegistry._registryLock;
			lock (registryLock)
			{
				ModuleRegistry._typeCache.Clear();
				ModuleRegistry._nameCache.Clear();
			}
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x00039698 File Offset: 0x00037898
		public static int CleanupInvalidModules()
		{
			object registryLock = ModuleRegistry._registryLock;
			int result;
			lock (registryLock)
			{
				List<Type> list = new List<Type>();
				List<string> list2 = new List<string>();
				foreach (KeyValuePair<Type, IKernelModule> keyValuePair in ModuleRegistry._typeCache)
				{
					bool flag2 = keyValuePair.Value == null;
					if (flag2)
					{
						list.Add(keyValuePair.Key);
					}
				}
				foreach (KeyValuePair<string, IKernelModule> keyValuePair2 in ModuleRegistry._nameCache)
				{
					bool flag3 = keyValuePair2.Value == null;
					if (flag3)
					{
						list2.Add(keyValuePair2.Key);
					}
				}
				foreach (Type key in list)
				{
					ModuleRegistry._typeCache.Remove(key);
				}
				foreach (string key2 in list2)
				{
					ModuleRegistry._nameCache.Remove(key2);
				}
				result = list.Count + list2.Count;
			}
			return result;
		}

		// Token: 0x0400049D RID: 1181
		private static readonly Dictionary<Type, IKernelModule> _typeCache = new Dictionary<Type, IKernelModule>(128);

		// Token: 0x0400049E RID: 1182
		private static readonly Dictionary<string, IKernelModule> _nameCache = new Dictionary<string, IKernelModule>(128);

		// Token: 0x0400049F RID: 1183
		private static readonly object _registryLock = new object();
	}
}
