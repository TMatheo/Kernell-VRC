using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using MelonLoader;
using UnityEngine;

namespace KernelVRC
{
	// Token: 0x020000AB RID: 171
	public static class EmbeddedResourceLoader
	{
		// Token: 0x060008E3 RID: 2275 RVA: 0x00037C7C File Offset: 0x00035E7C
		public static void Initialize()
		{
			bool initialized = EmbeddedResourceLoader._initialized;
			if (initialized)
			{
				MelonLogger.Warning("[ResourceLoader] Already initialized");
			}
			else
			{
				try
				{
					MelonLogger.Msg("[ResourceLoader] Initializing embedded resource system...");
					EmbeddedResourceLoader._assembly = Assembly.GetExecutingAssembly();
					bool flag = EmbeddedResourceLoader._assembly == null;
					if (flag)
					{
						throw new Exception("Could not get executing assembly");
					}
					EmbeddedResourceLoader._availableResources = EmbeddedResourceLoader._assembly.GetManifestResourceNames();
					bool flag2 = EmbeddedResourceLoader._availableResources == null;
					if (flag2)
					{
						EmbeddedResourceLoader._availableResources = new string[0];
						MelonLogger.Warning("[ResourceLoader] No manifest resource names found");
					}
					EmbeddedResourceLoader.CreateFallbackResources();
					EmbeddedResourceLoader.LogAvailableResources();
					EmbeddedResourceLoader._initialized = true;
					MelonLogger.Msg(string.Format("[ResourceLoader] Initialized with {0} embedded resources", EmbeddedResourceLoader._availableResources.Length));
				}
				catch (Exception arg)
				{
					MelonLogger.Error(string.Format("[ResourceLoader] Initialization failed: {0}", arg));
					EmbeddedResourceLoader._initialized = true;
					EmbeddedResourceLoader._availableResources = new string[0];
					EmbeddedResourceLoader.CreateFallbackResources();
				}
			}
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x00037D78 File Offset: 0x00035F78
		private static void CreateFallbackResources()
		{
			try
			{
				EmbeddedResourceLoader._fallbackTexture = EmbeddedResourceLoader.CreateFallbackTexture();
				bool flag = EmbeddedResourceLoader._fallbackTexture != null;
				if (flag)
				{
					EmbeddedResourceLoader._textureCache["fallback_texture"] = EmbeddedResourceLoader._fallbackTexture;
				}
				EmbeddedResourceLoader._fallbackSprite = EmbeddedResourceLoader.CreateFallbackSprite();
				bool flag2 = EmbeddedResourceLoader._fallbackSprite != null;
				if (flag2)
				{
					EmbeddedResourceLoader._spriteCache["fallback_sprite"] = EmbeddedResourceLoader._fallbackSprite;
				}
				MelonLogger.Msg("[ResourceLoader] Fallback resources created");
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("[ResourceLoader] Failed to create fallback resources: {0}", arg));
			}
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x00037E1C File Offset: 0x0003601C
		public static Texture2D LoadEmbeddedTexture(string resourcePath)
		{
			bool flag = !EmbeddedResourceLoader._initialized;
			Texture2D result;
			if (flag)
			{
				MelonLogger.Warning("[ResourceLoader] Not initialized, returning fallback texture");
				result = EmbeddedResourceLoader._fallbackTexture;
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(resourcePath);
				if (flag2)
				{
					MelonLogger.Warning("[ResourceLoader] Empty resource path, returning fallback texture");
					result = EmbeddedResourceLoader._fallbackTexture;
				}
				else
				{
					try
					{
						string text = EmbeddedResourceLoader.NormalizeResourcePath(resourcePath);
						Texture2D texture2D;
						bool flag3 = EmbeddedResourceLoader._textureCache.TryGetValue(text, ref texture2D);
						if (flag3)
						{
							EmbeddedResourceLoader.UpdateResourceAccess(text);
							result = texture2D;
						}
						else
						{
							string text2 = EmbeddedResourceLoader.FindResourcePath(text);
							bool flag4 = text2 == null;
							if (flag4)
							{
								MelonLogger.Warning("[ResourceLoader] Texture resource not found: " + resourcePath);
								result = EmbeddedResourceLoader._fallbackTexture;
							}
							else
							{
								Texture2D texture2D2 = EmbeddedResourceLoader.LoadTextureFromStream(text2);
								bool flag5 = texture2D2 != null;
								if (flag5)
								{
									EmbeddedResourceLoader._textureCache[text] = texture2D2;
									EmbeddedResourceLoader.StoreResourceMetadata(text, text2, 0L, EmbeddedResourceLoader.ResourceType.Texture);
									MelonLogger.Msg("[ResourceLoader] Loaded texture: " + resourcePath);
									result = texture2D2;
								}
								else
								{
									MelonLogger.Error("[ResourceLoader] Failed to load texture: " + resourcePath);
									result = EmbeddedResourceLoader._fallbackTexture;
								}
							}
						}
					}
					catch (Exception arg)
					{
						MelonLogger.Error(string.Format("[ResourceLoader] Error loading texture {0}: {1}", resourcePath, arg));
						result = EmbeddedResourceLoader._fallbackTexture;
					}
				}
			}
			return result;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x00037F5C File Offset: 0x0003615C
		public static Sprite LoadEmbeddedSprite(string resourcePath, Vector2? pivot = null, float pixelsPerUnit = 100f)
		{
			bool flag = !EmbeddedResourceLoader._initialized;
			Sprite result;
			if (flag)
			{
				MelonLogger.Warning("[ResourceLoader] Not initialized, returning fallback sprite");
				result = EmbeddedResourceLoader._fallbackSprite;
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(resourcePath);
				if (flag2)
				{
					MelonLogger.Warning("[ResourceLoader] Empty resource path, returning fallback sprite");
					result = EmbeddedResourceLoader._fallbackSprite;
				}
				else
				{
					try
					{
						string text = EmbeddedResourceLoader.NormalizeResourcePath(resourcePath);
						string text2 = string.Format("{0}_sprite_{1}_{2}_{3}", new object[]
						{
							text,
							(pivot != null) ? pivot.GetValueOrDefault().x : 0.5f,
							(pivot != null) ? pivot.GetValueOrDefault().y : 0.5f,
							pixelsPerUnit
						});
						Sprite sprite;
						bool flag3 = EmbeddedResourceLoader._spriteCache.TryGetValue(text2, ref sprite);
						if (flag3)
						{
							EmbeddedResourceLoader.UpdateResourceAccess(text);
							result = sprite;
						}
						else
						{
							Texture2D texture2D = EmbeddedResourceLoader.LoadEmbeddedTexture(resourcePath);
							bool flag4 = texture2D == null || texture2D == EmbeddedResourceLoader._fallbackTexture;
							if (flag4)
							{
								result = EmbeddedResourceLoader._fallbackSprite;
							}
							else
							{
								Sprite sprite2 = EmbeddedResourceLoader.CreateSpriteFromTexture(texture2D, pivot ?? new Vector2(0.5f, 0.5f), pixelsPerUnit);
								bool flag5 = sprite2 != null;
								if (flag5)
								{
									EmbeddedResourceLoader._spriteCache[text2] = sprite2;
									MelonLogger.Msg("[ResourceLoader] Created sprite: " + resourcePath);
									result = sprite2;
								}
								else
								{
									MelonLogger.Error("[ResourceLoader] Failed to create sprite: " + resourcePath);
									result = EmbeddedResourceLoader._fallbackSprite;
								}
							}
						}
					}
					catch (Exception arg)
					{
						MelonLogger.Error(string.Format("[ResourceLoader] Error loading sprite {0}: {1}", resourcePath, arg));
						result = EmbeddedResourceLoader._fallbackSprite;
					}
				}
			}
			return result;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0003812C File Offset: 0x0003632C
		public static byte[] LoadEmbeddedData(string resourcePath)
		{
			bool flag = !EmbeddedResourceLoader._initialized;
			byte[] result;
			if (flag)
			{
				MelonLogger.Warning("[ResourceLoader] Not initialized");
				result = null;
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(resourcePath);
				if (flag2)
				{
					MelonLogger.Warning("[ResourceLoader] Empty resource path");
					result = null;
				}
				else
				{
					try
					{
						string text = EmbeddedResourceLoader.NormalizeResourcePath(resourcePath);
						byte[] array;
						bool flag3 = EmbeddedResourceLoader._dataCache.TryGetValue(text, ref array);
						if (flag3)
						{
							EmbeddedResourceLoader.UpdateResourceAccess(text);
							result = array;
						}
						else
						{
							string text2 = EmbeddedResourceLoader.FindResourcePath(text);
							bool flag4 = text2 == null;
							if (flag4)
							{
								MelonLogger.Warning("[ResourceLoader] Data resource not found: " + resourcePath);
								result = null;
							}
							else
							{
								byte[] array2 = EmbeddedResourceLoader.LoadDataFromStream(text2);
								bool flag5 = array2 != null;
								if (flag5)
								{
									EmbeddedResourceLoader._dataCache[text] = array2;
									EmbeddedResourceLoader.StoreResourceMetadata(text, text2, (long)array2.Length, EmbeddedResourceLoader.ResourceType.Data);
									MelonLogger.Msg(string.Format("[ResourceLoader] Loaded data: {0} ({1} bytes)", resourcePath, array2.Length));
								}
								result = array2;
							}
						}
					}
					catch (Exception arg)
					{
						MelonLogger.Error(string.Format("[ResourceLoader] Error loading data {0}: {1}", resourcePath, arg));
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00038248 File Offset: 0x00036448
		public static string LoadEmbeddedText(string resourcePath)
		{
			string result;
			try
			{
				byte[] array = EmbeddedResourceLoader.LoadEmbeddedData(resourcePath);
				result = ((array != null) ? Encoding.UTF8.GetString(array) : null);
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("[ResourceLoader] Error loading text {0}: {1}", resourcePath, arg));
				result = null;
			}
			return result;
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0003829C File Offset: 0x0003649C
		private static Texture2D LoadTextureFromStream(string fullPath)
		{
			Texture2D result;
			try
			{
				using (Stream manifestResourceStream = EmbeddedResourceLoader._assembly.GetManifestResourceStream(fullPath))
				{
					bool flag = manifestResourceStream == null;
					if (flag)
					{
						MelonLogger.Warning("[ResourceLoader] Stream is null for: " + fullPath);
						result = null;
					}
					else
					{
						byte[] array = new byte[manifestResourceStream.Length];
						int i;
						int num;
						for (i = 0; i < array.Length; i += num)
						{
							num = manifestResourceStream.Read(array, i, array.Length - i);
							bool flag2 = num == 0;
							if (flag2)
							{
								break;
							}
						}
						bool flag3 = i != array.Length;
						if (flag3)
						{
							MelonLogger.Warning(string.Format("[ResourceLoader] Incomplete read for {0}: {1}/{2}", fullPath, i, array.Length));
						}
						Texture2D texture2D = new Texture2D(2, 2, 5, false);
						bool flag4 = ImageConversion.LoadImage(texture2D, array);
						if (flag4)
						{
							result = texture2D;
						}
						else
						{
							MelonLogger.Warning("[ResourceLoader] ImageConversion.LoadImage failed for: " + fullPath);
							Object.DestroyImmediate(texture2D);
							result = null;
						}
					}
				}
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("[ResourceLoader] Stream loading error for {0}: {1}", fullPath, arg));
				result = null;
			}
			return result;
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x000383D4 File Offset: 0x000365D4
		private static byte[] LoadDataFromStream(string fullPath)
		{
			byte[] result;
			try
			{
				using (Stream manifestResourceStream = EmbeddedResourceLoader._assembly.GetManifestResourceStream(fullPath))
				{
					bool flag = manifestResourceStream == null;
					if (flag)
					{
						MelonLogger.Warning("[ResourceLoader] Stream is null for: " + fullPath);
						result = null;
					}
					else
					{
						byte[] array = new byte[manifestResourceStream.Length];
						int i;
						int num;
						for (i = 0; i < array.Length; i += num)
						{
							num = manifestResourceStream.Read(array, i, array.Length - i);
							bool flag2 = num == 0;
							if (flag2)
							{
								break;
							}
						}
						bool flag3 = i != array.Length;
						if (flag3)
						{
							MelonLogger.Warning(string.Format("[ResourceLoader] Incomplete read for {0}: {1}/{2}", fullPath, i, array.Length));
							Array.Resize<byte>(ref array, i);
						}
						result = array;
					}
				}
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("[ResourceLoader] Data stream loading error for {0}: {1}", fullPath, arg));
				result = null;
			}
			return result;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x000384D4 File Offset: 0x000366D4
		private static Sprite CreateSpriteFromTexture(Texture2D texture, Vector2 pivot, float pixelsPerUnit)
		{
			Sprite result;
			try
			{
				result = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), pivot, pixelsPerUnit);
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("[ResourceLoader] Sprite creation error: {0}", arg));
				result = null;
			}
			return result;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00038534 File Offset: 0x00036734
		private static Texture2D CreateFallbackTexture()
		{
			Texture2D result;
			try
			{
				Texture2D texture2D = new Texture2D(64, 64, 5, false);
				Color32[] array = new Color32[4096];
				for (int i = 0; i < array.Length; i++)
				{
					int num = i % 64;
					int num2 = i / 64;
					bool flag = (num / 8 + num2 / 8) % 2 == 0;
					array[i] = (flag ? new Color32(153, 50, 204, byte.MaxValue) : new Color32(75, 0, 130, byte.MaxValue));
				}
				texture2D.SetPixels32(array);
				texture2D.Apply();
				result = texture2D;
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("[ResourceLoader] Failed to create fallback texture: {0}", arg));
				result = null;
			}
			return result;
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00038604 File Offset: 0x00036804
		private static Sprite CreateFallbackSprite()
		{
			Sprite result;
			try
			{
				bool flag = EmbeddedResourceLoader._fallbackTexture == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					result = Sprite.Create(EmbeddedResourceLoader._fallbackTexture, new Rect(0f, 0f, (float)EmbeddedResourceLoader._fallbackTexture.width, (float)EmbeddedResourceLoader._fallbackTexture.height), new Vector2(0.5f, 0.5f), 100f);
				}
			}
			catch (Exception arg)
			{
				MelonLogger.Error(string.Format("[ResourceLoader] Failed to create fallback sprite: {0}", arg));
				result = null;
			}
			return result;
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00038694 File Offset: 0x00036894
		private static string NormalizeResourcePath(string path)
		{
			bool flag = string.IsNullOrEmpty(path);
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				bool flag2 = path.StartsWith("KernelVRC.assets.") || path.StartsWith("KernellVRCLite.");
				if (flag2)
				{
					result = path;
				}
				else
				{
					result = "KernelVRC.assets." + path.Replace("/", ".").Replace("\\", ".");
				}
			}
			return result;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00038704 File Offset: 0x00036904
		private static string FindResourcePath(string normalizedPath)
		{
			bool flag = EmbeddedResourceLoader._availableResources == null || EmbeddedResourceLoader._availableResources.Length == 0;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				bool flag2 = Enumerable.Contains<string>(EmbeddedResourceLoader._availableResources, normalizedPath);
				if (flag2)
				{
					result = normalizedPath;
				}
				else
				{
					string[] array = new string[]
					{
						normalizedPath,
						"KernellVRCLite." + normalizedPath.Replace("KernelVRC.assets.", "assets."),
						normalizedPath.Replace("KernelVRC.assets.", "KernellVRCLite.assets."),
						normalizedPath.Replace("KernelVRC.assets.", ""),
						"KernellVRCLite." + normalizedPath.Replace("KernelVRC.assets.", "")
					};
					foreach (string text in array)
					{
						bool flag3 = Enumerable.Contains<string>(EmbeddedResourceLoader._availableResources, text);
						if (flag3)
						{
							return text;
						}
					}
					string[] array3 = Enumerable.ToArray<string>(Enumerable.Where<string>(EmbeddedResourceLoader._availableResources, (string r) => r.EndsWith(normalizedPath.Replace("KernelVRC.assets.", ""), StringComparison.OrdinalIgnoreCase) || r.EndsWith(Path.GetFileName(normalizedPath), StringComparison.OrdinalIgnoreCase)));
					bool flag4 = array3.Length >= 1;
					if (flag4)
					{
						bool flag5 = array3.Length > 1;
						if (flag5)
						{
							MelonLogger.Warning("[ResourceLoader] Multiple matches found for " + normalizedPath + ": " + string.Join(", ", array3));
						}
						result = array3[0];
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x0003888C File Offset: 0x00036A8C
		private static void UpdateResourceAccess(string path)
		{
			EmbeddedResourceLoader.ResourceMetadata resourceMetadata;
			bool flag = EmbeddedResourceLoader._resourceMetadata.TryGetValue(path, ref resourceMetadata);
			if (flag)
			{
				resourceMetadata.AccessCount++;
				resourceMetadata.LoadTime = DateTime.Now;
				EmbeddedResourceLoader._resourceMetadata[path] = resourceMetadata;
			}
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000388D4 File Offset: 0x00036AD4
		private static void StoreResourceMetadata(string path, string fullPath, long size, EmbeddedResourceLoader.ResourceType type)
		{
			EmbeddedResourceLoader._resourceMetadata[path] = new EmbeddedResourceLoader.ResourceMetadata
			{
				FullPath = fullPath,
				Size = size,
				LoadTime = DateTime.Now,
				AccessCount = 1,
				Type = type
			};
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00038924 File Offset: 0x00036B24
		private static void LogAvailableResources()
		{
			bool flag = EmbeddedResourceLoader._availableResources.Length == 0;
			if (flag)
			{
				MelonLogger.Warning("[ResourceLoader] No embedded resources found");
			}
			else
			{
				MelonLogger.Msg("[ResourceLoader] Available embedded resources:");
				IEnumerable<IGrouping<string, string>> enumerable = Enumerable.GroupBy<string, string>(EmbeddedResourceLoader._availableResources, delegate(string r)
				{
					bool flag3 = r.Contains(".assets.") || r.Contains("assets");
					string result;
					if (flag3)
					{
						result = "Assets";
					}
					else
					{
						bool flag4 = r.Contains(".dll") || r.Contains(".exe");
						if (flag4)
						{
							result = "Assemblies";
						}
						else
						{
							result = "Other";
						}
					}
					return result;
				});
				foreach (IGrouping<string, string> grouping in Enumerable.OrderBy<IGrouping<string, string>, string>(enumerable, (IGrouping<string, string> g) => g.Key))
				{
					MelonLogger.Msg(string.Format("  {0}: {1} files", grouping.Key, Enumerable.Count<string>(grouping)));
					foreach (string str in Enumerable.Take<string>(grouping, 10))
					{
						MelonLogger.Msg("    - " + str);
					}
					bool flag2 = Enumerable.Count<string>(grouping) > 10;
					if (flag2)
					{
						MelonLogger.Msg(string.Format("    ... and {0} more", Enumerable.Count<string>(grouping) - 10));
					}
				}
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x00038A8C File Offset: 0x00036C8C
		private static void CleanupCache<T>(ConcurrentDictionary<string, T> cache, int maxSize) where T : class
		{
			bool flag = cache.Count <= maxSize;
			if (!flag)
			{
				try
				{
					int num = cache.Count - maxSize;
					string[] array = Enumerable.ToArray<string>(Enumerable.Take<string>(cache.Keys, num));
					foreach (string text in array)
					{
						T t;
						cache.TryRemove(text, ref t);
					}
				}
				catch (Exception arg)
				{
					MelonLogger.Error(string.Format("[ResourceLoader] Cache cleanup error: {0}", arg));
				}
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00038B1C File Offset: 0x00036D1C
		public static Dictionary<string, object> GetCacheStatistics()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["TextureCount"] = EmbeddedResourceLoader._textureCache.Count;
			dictionary["SpriteCount"] = EmbeddedResourceLoader._spriteCache.Count;
			dictionary["DataCount"] = EmbeddedResourceLoader._dataCache.Count;
			string key = "TotalResources";
			string[] availableResources = EmbeddedResourceLoader._availableResources;
			dictionary[key] = ((availableResources != null) ? availableResources.Length : 0);
			dictionary["Initialized"] = EmbeddedResourceLoader._initialized;
			dictionary["LastCleanup"] = EmbeddedResourceLoader._lastCacheCleanup;
			return dictionary;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x00038BD0 File Offset: 0x00036DD0
		public static string[] GetAvailableResources()
		{
			string[] availableResources = EmbeddedResourceLoader._availableResources;
			return ((availableResources != null) ? Enumerable.ToArray<string>(availableResources) : null) ?? new string[0];
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x00038C00 File Offset: 0x00036E00
		public static bool ResourceExists(string resourcePath)
		{
			bool flag = !EmbeddedResourceLoader._initialized || EmbeddedResourceLoader._availableResources == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string normalizedPath = EmbeddedResourceLoader.NormalizeResourcePath(resourcePath);
				result = (EmbeddedResourceLoader.FindResourcePath(normalizedPath) != null);
			}
			return result;
		}

		// Token: 0x04000447 RID: 1095
		private const string FALLBACK_TEXTURE_NAME = "fallback_texture";

		// Token: 0x04000448 RID: 1096
		private const string FALLBACK_SPRITE_NAME = "fallback_sprite";

		// Token: 0x04000449 RID: 1097
		private const int FALLBACK_TEXTURE_SIZE = 64;

		// Token: 0x0400044A RID: 1098
		private const int CACHE_CLEANUP_INTERVAL = 300;

		// Token: 0x0400044B RID: 1099
		private const string ASSETS_PREFIX = "KernelVRC.assets.";

		// Token: 0x0400044C RID: 1100
		private const string KERNELLVRC_PREFIX = "KernellVRCLite.";

		// Token: 0x0400044D RID: 1101
		private static bool _initialized = false;

		// Token: 0x0400044E RID: 1102
		private static Assembly _assembly;

		// Token: 0x0400044F RID: 1103
		private static string[] _availableResources;

		// Token: 0x04000450 RID: 1104
		private static DateTime _lastCacheCleanup = DateTime.MinValue;

		// Token: 0x04000451 RID: 1105
		private static readonly ConcurrentDictionary<string, Texture2D> _textureCache = new ConcurrentDictionary<string, Texture2D>();

		// Token: 0x04000452 RID: 1106
		private static readonly ConcurrentDictionary<string, Sprite> _spriteCache = new ConcurrentDictionary<string, Sprite>();

		// Token: 0x04000453 RID: 1107
		private static readonly ConcurrentDictionary<string, byte[]> _dataCache = new ConcurrentDictionary<string, byte[]>();

		// Token: 0x04000454 RID: 1108
		private static readonly ConcurrentDictionary<string, EmbeddedResourceLoader.ResourceMetadata> _resourceMetadata = new ConcurrentDictionary<string, EmbeddedResourceLoader.ResourceMetadata>();

		// Token: 0x04000455 RID: 1109
		private static Texture2D _fallbackTexture;

		// Token: 0x04000456 RID: 1110
		private static Sprite _fallbackSprite;

		// Token: 0x0200018D RID: 397
		private struct ResourceMetadata
		{
			// Token: 0x0400094F RID: 2383
			public string FullPath;

			// Token: 0x04000950 RID: 2384
			public long Size;

			// Token: 0x04000951 RID: 2385
			public DateTime LoadTime;

			// Token: 0x04000952 RID: 2386
			public int AccessCount;

			// Token: 0x04000953 RID: 2387
			public EmbeddedResourceLoader.ResourceType Type;
		}

		// Token: 0x0200018E RID: 398
		private enum ResourceType
		{
			// Token: 0x04000955 RID: 2389
			Texture,
			// Token: 0x04000956 RID: 2390
			Sprite,
			// Token: 0x04000957 RID: 2391
			Data,
			// Token: 0x04000958 RID: 2392
			Unknown
		}
	}
}
