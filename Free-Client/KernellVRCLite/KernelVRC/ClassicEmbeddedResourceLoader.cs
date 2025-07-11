using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using MelonLoader;
using UnityEngine;
using UnityEngine.Networking;

namespace KernelVRC
{
	// Token: 0x020000AA RID: 170
	public class ClassicEmbeddedResourceLoader : MonoBehaviour
	{
		// Token: 0x060008C1 RID: 2241 RVA: 0x00035BA0 File Offset: 0x00033DA0
		public static void Initialize()
		{
			bool isInitialized = ClassicEmbeddedResourceLoader._isInitialized;
			if (isInitialized)
			{
				kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Already initialized");
			}
			else
			{
				try
				{
					ClassicEmbeddedResourceLoader.EnsureDirectoriesExist();
					ClassicEmbeddedResourceLoader.LogSystemInfo();
					ClassicEmbeddedResourceLoader._isInitialized = true;
					kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Initialized successfully");
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to initialize: " + ex.Message);
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Stack trace: " + ex.StackTrace);
				}
			}
		}

		// Token: 0x060008C2 RID: 2242 RVA: 0x00035C28 File Offset: 0x00033E28
		public static void EnsureDirectoriesExist()
		{
			bool flag = !ClassicEmbeddedResourceLoader._cacheDirInitialized;
			if (flag)
			{
				try
				{
					bool flag2 = !Directory.Exists(ClassicEmbeddedResourceLoader.CacheDirectory);
					if (flag2)
					{
						Directory.CreateDirectory(ClassicEmbeddedResourceLoader.CacheDirectory);
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Created cache directory: " + ClassicEmbeddedResourceLoader.CacheDirectory);
					}
					else
					{
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Cache directory exists: " + ClassicEmbeddedResourceLoader.CacheDirectory);
					}
					ClassicEmbeddedResourceLoader._cacheDirInitialized = true;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to create cache directory: " + ex.Message);
				}
			}
			bool flag3 = !ClassicEmbeddedResourceLoader._themesDirInitialized;
			if (flag3)
			{
				try
				{
					bool flag4 = !Directory.Exists(ClassicEmbeddedResourceLoader.ThemesDirectory);
					if (flag4)
					{
						Directory.CreateDirectory(ClassicEmbeddedResourceLoader.ThemesDirectory);
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Created themes directory: " + ClassicEmbeddedResourceLoader.ThemesDirectory);
					}
					else
					{
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Themes directory exists: " + ClassicEmbeddedResourceLoader.ThemesDirectory);
					}
					ClassicEmbeddedResourceLoader._themesDirInitialized = true;
				}
				catch (Exception ex2)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to create themes directory: " + ex2.Message);
				}
			}
		}

		// Token: 0x060008C3 RID: 2243 RVA: 0x00035D54 File Offset: 0x00033F54
		private static void LogSystemInfo()
		{
			try
			{
				kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Unity Version: " + Application.unityVersion);
				kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Platform: {0}", Application.platform));
				kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Supported audio formats: " + string.Join(", ", ClassicEmbeddedResourceLoader.AudioFormatMap.Keys));
				kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Supported image formats: " + string.Join(", ", ClassicEmbeddedResourceLoader.SupportedImageFormats));
				string[] manifestResourceNames = ClassicEmbeddedResourceLoader._executingAssembly.GetManifestResourceNames();
				int num = 0;
				foreach (string text in manifestResourceNames)
				{
					bool flag = text.Contains("assets") || text.Contains("Assets");
					if (flag)
					{
						num++;
					}
				}
				kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Found {0} embedded assets in assembly", num));
			}
			catch (Exception ex)
			{
				kernelllogger.Warning("[ClassicEmbeddedResourceLoader] Could not log system info: " + ex.Message);
			}
		}

		// Token: 0x060008C4 RID: 2244 RVA: 0x00035E68 File Offset: 0x00034068
		public static byte[] LoadEmbeddedResourceBytes(string resourcePath)
		{
			bool flag = string.IsNullOrEmpty(resourcePath);
			byte[] result;
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Resource path is null or empty");
				result = null;
			}
			else
			{
				ClassicEmbeddedResourceLoader._totalLoadsAttempted++;
				try
				{
					using (Stream manifestResourceStream = ClassicEmbeddedResourceLoader._executingAssembly.GetManifestResourceStream(resourcePath))
					{
						bool flag2 = manifestResourceStream == null;
						if (flag2)
						{
							kernelllogger.Error("[ClassicEmbeddedResourceLoader] Embedded resource not found: " + resourcePath);
							ClassicEmbeddedResourceLoader.LogAvailableResources();
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
								bool flag3 = num == 0;
								if (flag3)
								{
									break;
								}
							}
							bool flag4 = i != array.Length;
							if (flag4)
							{
								kernelllogger.Warning(string.Format("[ClassicEmbeddedResourceLoader] Only read {0} of {1} bytes from: {2}", i, array.Length, resourcePath));
							}
							ClassicEmbeddedResourceLoader._totalLoadsSuccessful++;
							kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Loaded {0} bytes from: {1}", array.Length, resourcePath));
							result = array;
						}
					}
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Exception loading embedded resource '" + resourcePath + "': " + ex.Message);
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x00035FC4 File Offset: 0x000341C4
		private static void LogAvailableResources()
		{
			try
			{
				string[] manifestResourceNames = ClassicEmbeddedResourceLoader._executingAssembly.GetManifestResourceNames();
				kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Available embedded resources ({0}):", manifestResourceNames.Length));
				int num = 0;
				foreach (string text in manifestResourceNames)
				{
					bool flag = text.Contains("assets") || text.Contains("Assets");
					if (flag)
					{
						kernelllogger.Msg("  - " + text);
						num++;
					}
				}
				bool flag2 = num == 0;
				if (flag2)
				{
					kernelllogger.Warning("  No assets found in embedded resources");
					kernelllogger.Msg("  All resources:");
					foreach (string str in manifestResourceNames)
					{
						kernelllogger.Msg("    - " + str);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Error listing resources: " + ex.Message);
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x000360D0 File Offset: 0x000342D0
		public static Texture2D LoadEmbeddedTexture(string resourcePath)
		{
			bool flag = string.IsNullOrEmpty(resourcePath);
			Texture2D result;
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Texture resource path is null or empty");
				result = null;
			}
			else
			{
				object textureCacheLock = ClassicEmbeddedResourceLoader.TextureCacheLock;
				lock (textureCacheLock)
				{
					Texture2D texture2D;
					bool flag3 = ClassicEmbeddedResourceLoader.TextureCache.TryGetValue(resourcePath, out texture2D) && texture2D != null;
					if (flag3)
					{
						ClassicEmbeddedResourceLoader._cacheHits++;
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Using cached texture: " + resourcePath);
						return texture2D;
					}
				}
				byte[] array = ClassicEmbeddedResourceLoader.LoadEmbeddedResourceBytes(resourcePath);
				bool flag4 = array == null || array.Length == 0;
				if (flag4)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to load image data for: " + resourcePath);
					result = null;
				}
				else
				{
					try
					{
						Texture2D texture2D2 = new Texture2D(2, 2, 4, false);
						bool flag5 = ImageConversion.LoadImage(texture2D2, array);
						if (flag5)
						{
							texture2D2.wrapMode = 1;
							texture2D2.filterMode = 1;
							texture2D2.name = Path.GetFileNameWithoutExtension(resourcePath);
							bool flag6 = texture2D2.width <= 0 || texture2D2.height <= 0;
							if (flag6)
							{
								kernelllogger.Error(string.Format("[ClassicEmbeddedResourceLoader] Invalid texture dimensions: {0}x{1}", texture2D2.width, texture2D2.height));
								Object.Destroy(texture2D2);
								result = null;
							}
							else
							{
								object textureCacheLock2 = ClassicEmbeddedResourceLoader.TextureCacheLock;
								lock (textureCacheLock2)
								{
									ClassicEmbeddedResourceLoader.TextureCache[resourcePath] = texture2D2;
								}
								kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Successfully loaded texture: {0} ({1}x{2})", resourcePath, texture2D2.width, texture2D2.height));
								result = texture2D2;
							}
						}
						else
						{
							kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to convert image data to Texture2D: " + resourcePath);
							Object.Destroy(texture2D2);
							result = null;
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[ClassicEmbeddedResourceLoader] Exception creating texture: " + ex.Message);
						result = null;
					}
				}
			}
			return result;
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0003631C File Offset: 0x0003451C
		private static Sprite CreateSpriteFromTexture(Texture2D texture, string cacheKey)
		{
			bool flag = texture == null;
			Sprite result;
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Cannot create sprite from null texture");
				result = null;
			}
			else
			{
				object spriteCacheLock = ClassicEmbeddedResourceLoader.SpriteCacheLock;
				lock (spriteCacheLock)
				{
					Sprite sprite;
					bool flag3 = ClassicEmbeddedResourceLoader.SpriteCache.TryGetValue(cacheKey, out sprite) && sprite != null;
					if (flag3)
					{
						return sprite;
					}
				}
				try
				{
					Sprite sprite2 = Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0.5f, 0.5f), 100f);
					sprite2.name = Path.GetFileNameWithoutExtension(cacheKey);
					object spriteCacheLock2 = ClassicEmbeddedResourceLoader.SpriteCacheLock;
					lock (spriteCacheLock2)
					{
						ClassicEmbeddedResourceLoader.SpriteCache[cacheKey] = sprite2;
					}
					kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Created sprite: {0} ({1}x{2})", sprite2.name, texture.width, texture.height));
					result = sprite2;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Exception creating sprite: " + ex.Message);
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x00036488 File Offset: 0x00034688
		public static Sprite LoadEmbeddedSprite(string resourcePath)
		{
			bool flag = string.IsNullOrEmpty(resourcePath);
			Sprite result;
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Sprite resource path is null or empty");
				result = null;
			}
			else
			{
				object spriteCacheLock = ClassicEmbeddedResourceLoader.SpriteCacheLock;
				lock (spriteCacheLock)
				{
					Sprite sprite;
					bool flag3 = ClassicEmbeddedResourceLoader.SpriteCache.TryGetValue(resourcePath, out sprite) && sprite != null;
					if (flag3)
					{
						ClassicEmbeddedResourceLoader._cacheHits++;
						return sprite;
					}
				}
				Texture2D texture2D = ClassicEmbeddedResourceLoader.LoadEmbeddedTexture(resourcePath);
				bool flag4 = texture2D == null;
				if (flag4)
				{
					result = null;
				}
				else
				{
					result = ClassicEmbeddedResourceLoader.CreateSpriteFromTexture(texture2D, resourcePath);
				}
			}
			return result;
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x00036538 File Offset: 0x00034738
		public static AudioClip LoadEmbeddedAudioClip(string resourcePath)
		{
			bool flag = string.IsNullOrEmpty(resourcePath);
			AudioClip result;
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Audio resource path is null or empty");
				result = null;
			}
			else
			{
				object audioCacheLock = ClassicEmbeddedResourceLoader.AudioCacheLock;
				lock (audioCacheLock)
				{
					AudioClip audioClip;
					bool flag3 = ClassicEmbeddedResourceLoader.AudioCache.TryGetValue(resourcePath, out audioClip) && audioClip != null;
					if (flag3)
					{
						ClassicEmbeddedResourceLoader._cacheHits++;
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Using cached audio: " + resourcePath);
						return audioClip;
					}
				}
				byte[] array = ClassicEmbeddedResourceLoader.LoadEmbeddedResourceBytes(resourcePath);
				bool flag4 = array == null || array.Length == 0;
				if (flag4)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to load audio data for: " + resourcePath);
					result = null;
				}
				else
				{
					AudioType audioType = ClassicEmbeddedResourceLoader.DetectAudioFormat(resourcePath);
					kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Detected audio format: {0} for {1}", audioType, resourcePath));
					AudioClip audioClip2 = ClassicEmbeddedResourceLoader.LoadAudioClipFromBytes(array, resourcePath, audioType);
					bool flag5 = audioClip2 != null;
					if (flag5)
					{
						object audioCacheLock2 = ClassicEmbeddedResourceLoader.AudioCacheLock;
						lock (audioCacheLock2)
						{
							ClassicEmbeddedResourceLoader.AudioCache[resourcePath] = audioClip2;
						}
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Successfully loaded audio: " + resourcePath + " " + string.Format("(Length: {0:F2}s, Channels: {1}, Frequency: {2}Hz)", audioClip2.length, audioClip2.channels, audioClip2.frequency));
					}
					else
					{
						kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to load audio clip: " + resourcePath);
					}
					result = audioClip2;
				}
			}
			return result;
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x000366E4 File Offset: 0x000348E4
		public static AudioClip LoadEmbeddedAudioClip(string resourcePath, AudioType audioType)
		{
			bool flag = string.IsNullOrEmpty(resourcePath);
			AudioClip result;
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Audio resource path is null or empty");
				result = null;
			}
			else
			{
				string text = string.Format("{0}_{1}", resourcePath, audioType);
				object audioCacheLock = ClassicEmbeddedResourceLoader.AudioCacheLock;
				lock (audioCacheLock)
				{
					AudioClip audioClip;
					bool flag3 = ClassicEmbeddedResourceLoader.AudioCache.TryGetValue(text, out audioClip) && audioClip != null;
					if (flag3)
					{
						ClassicEmbeddedResourceLoader._cacheHits++;
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Using cached audio: " + text);
						return audioClip;
					}
				}
				byte[] array = ClassicEmbeddedResourceLoader.LoadEmbeddedResourceBytes(resourcePath);
				bool flag4 = array == null || array.Length == 0;
				if (flag4)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to load audio data for: " + resourcePath);
					result = null;
				}
				else
				{
					AudioClip audioClip2 = ClassicEmbeddedResourceLoader.LoadAudioClipFromBytes(array, resourcePath, audioType);
					bool flag5 = audioClip2 != null;
					if (flag5)
					{
						object audioCacheLock2 = ClassicEmbeddedResourceLoader.AudioCacheLock;
						lock (audioCacheLock2)
						{
							ClassicEmbeddedResourceLoader.AudioCache[text] = audioClip2;
						}
						kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Successfully loaded audio: {0} as {1}", resourcePath, audioType));
					}
					result = audioClip2;
				}
			}
			return result;
		}

		// Token: 0x060008CB RID: 2251 RVA: 0x00036844 File Offset: 0x00034A44
		public static void LoadEmbeddedAudioClipAsync(string resourcePath, Action<AudioClip> onComplete)
		{
			bool flag = string.IsNullOrEmpty(resourcePath);
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Audio resource path is null or empty");
				if (onComplete != null)
				{
					onComplete(null);
				}
			}
			else
			{
				object audioCacheLock = ClassicEmbeddedResourceLoader.AudioCacheLock;
				lock (audioCacheLock)
				{
					AudioClip audioClip;
					bool flag3 = ClassicEmbeddedResourceLoader.AudioCache.TryGetValue(resourcePath, out audioClip) && audioClip != null;
					if (flag3)
					{
						ClassicEmbeddedResourceLoader._cacheHits++;
						kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Using cached audio (async): " + resourcePath);
						if (onComplete != null)
						{
							onComplete(audioClip);
						}
						return;
					}
				}
				MelonCoroutines.Start(ClassicEmbeddedResourceLoader.LoadAudioCoroutine(resourcePath, onComplete));
			}
		}

		// Token: 0x060008CC RID: 2252 RVA: 0x00036900 File Offset: 0x00034B00
		private static AudioClip LoadAudioClipFromBytes(byte[] audioData, string resourcePath, AudioType audioType)
		{
			ClassicEmbeddedResourceLoader.EnsureDirectoriesExist();
			bool flag = audioData == null || audioData.Length == 0;
			AudioClip result;
			if (flag)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Audio data is null/empty: " + resourcePath);
				result = null;
			}
			else
			{
				string tempAudioPath = null;
				try
				{
					string tempFileExtension = ClassicEmbeddedResourceLoader.GetTempFileExtension(audioType);
					tempAudioPath = Path.Combine(ClassicEmbeddedResourceLoader.CacheDirectory, string.Format("audio_{0}{1}", Guid.NewGuid(), tempFileExtension));
					File.WriteAllBytes(tempAudioPath, audioData);
					kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Wrote {0} bytes to temp file: {1}", audioData.Length, tempAudioPath));
					var array = new <>f__AnonymousType0<string, Func<AudioClip>>[]
					{
						new
						{
							Name = "DirectBytes",
							Loader = (() => ClassicEmbeddedResourceLoader.LoadFromBytesDirectly(audioData, resourcePath, audioType))
						},
						new
						{
							Name = "UnityWebRequest",
							Loader = (() => ClassicEmbeddedResourceLoader.LoadFromFileWithWebRequest(tempAudioPath, audioType, resourcePath))
						},
						new
						{
							Name = "NativeParser",
							Loader = (() => ClassicEmbeddedResourceLoader.LoadWithNativeParser(audioData, resourcePath, audioType))
						}
					};
					var array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						var <>f__AnonymousType = array2[i];
						try
						{
							kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Trying {0} for {1}: {2}", <>f__AnonymousType.Name, audioType, resourcePath));
							AudioClip audioClip = <>f__AnonymousType.Loader();
							bool flag2 = audioClip != null && ClassicEmbeddedResourceLoader.ValidateAudioClip(audioClip, resourcePath);
							if (flag2)
							{
								audioClip.name = Path.GetFileNameWithoutExtension(resourcePath);
								kernelllogger.Msg(string.Concat(new string[]
								{
									"[ClassicEmbeddedResourceLoader] Successfully loaded with ",
									<>f__AnonymousType.Name,
									": ",
									resourcePath,
									" ",
									string.Format("(Length: {0:F2}s, Channels: {1}, Frequency: {2}Hz)", audioClip.length, audioClip.channels, audioClip.frequency)
								}));
								return audioClip;
							}
							kernelllogger.Warning("[ClassicEmbeddedResourceLoader] " + <>f__AnonymousType.Name + " failed or returned invalid clip");
							bool flag3 = audioClip != null;
							if (flag3)
							{
								Object.Destroy(audioClip);
							}
						}
						catch (Exception ex)
						{
							kernelllogger.Warning("[ClassicEmbeddedResourceLoader] " + <>f__AnonymousType.Name + " threw exception: " + ex.Message);
						}
					}
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] All loading strategies failed for: " + resourcePath);
					result = null;
				}
				catch (Exception ex2)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Exception loading audio '" + resourcePath + "': " + ex2.Message);
					result = null;
				}
				finally
				{
					bool flag4 = !string.IsNullOrEmpty(tempAudioPath) && File.Exists(tempAudioPath);
					if (flag4)
					{
						try
						{
							File.Delete(tempAudioPath);
						}
						catch (Exception ex3)
						{
							kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to delete temp audio file: " + ex3.Message);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00036C8C File Offset: 0x00034E8C
		private static AudioClip LoadFromBytesDirectly(byte[] audioData, string resourcePath, AudioType audioType)
		{
			AudioClip result;
			try
			{
				if (audioType != 20)
				{
					result = null;
				}
				else
				{
					result = ClassicEmbeddedResourceLoader.ParseWAVFromBytes(audioData, resourcePath);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Direct bytes loading failed: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00036CE4 File Offset: 0x00034EE4
		private static AudioClip LoadFromFileWithWebRequest(string filePath, AudioType audioType, string resourcePath)
		{
			AudioClip result;
			try
			{
				string text = "file://" + filePath.Replace('\\', '/');
				kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Loading with WebRequest from: " + text);
				AudioClip audioClip = ClassicEmbeddedResourceLoader.LoadWithUnityWebRequestMultimedia(text, audioType, resourcePath);
				bool flag = audioClip != null;
				if (flag)
				{
					result = audioClip;
				}
				else
				{
					result = ClassicEmbeddedResourceLoader.LoadWithBasicWebRequest(text, null, resourcePath, audioType);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] WebRequest loading failed: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x00036D70 File Offset: 0x00034F70
		private static AudioClip LoadWithUnityWebRequestMultimedia(string fileUrl, AudioType audioType, string resourcePath)
		{
			AudioClip result;
			try
			{
				UnityWebRequest unityWebRequest = null;
				try
				{
					unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(fileUrl, audioType);
					UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
					int num = 10000;
					int num2 = 0;
					while (!unityWebRequestAsyncOperation.isDone && num2 < num)
					{
						Thread.Sleep(50);
						num2 += 50;
					}
					bool flag = unityWebRequest.result == 1;
					if (flag)
					{
						AudioClip content = DownloadHandlerAudioClip.GetContent(unityWebRequest);
						bool flag2 = content != null;
						if (flag2)
						{
							kernelllogger.Msg("[ClassicEmbeddedResourceLoader] UnityWebRequestMultimedia successfully loaded: " + resourcePath);
						}
						result = content;
					}
					else
					{
						kernelllogger.Warning("[ClassicEmbeddedResourceLoader] UnityWebRequestMultimedia failed: " + unityWebRequest.error);
						result = null;
					}
				}
				finally
				{
					if (unityWebRequest != null)
					{
						unityWebRequest.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Warning("[ClassicEmbeddedResourceLoader] UnityWebRequestMultimedia not available: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00036E68 File Offset: 0x00035068
		private static AudioClip LoadWithBasicWebRequest(string fileUrl, byte[] audioData, string resourcePath, AudioType audioType)
		{
			AudioClip result;
			try
			{
				UnityWebRequest unityWebRequest = null;
				try
				{
					unityWebRequest = UnityWebRequest.Get(fileUrl);
					UnityWebRequestAsyncOperation unityWebRequestAsyncOperation = unityWebRequest.SendWebRequest();
					int num = 10000;
					int num2 = 0;
					while (!unityWebRequestAsyncOperation.isDone && num2 < num)
					{
						Thread.Sleep(50);
						num2 += 50;
					}
					bool flag = unityWebRequest.result == 1;
					if (flag)
					{
						byte[] audioData2 = unityWebRequest.downloadHandler.data;
						result = ClassicEmbeddedResourceLoader.LoadFromBytesDirectly(audioData2, resourcePath, audioType);
					}
					else
					{
						kernelllogger.Warning("[ClassicEmbeddedResourceLoader] Basic WebRequest failed: " + unityWebRequest.error);
						result = null;
					}
				}
				finally
				{
					if (unityWebRequest != null)
					{
						unityWebRequest.Dispose();
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Basic WebRequest failed: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x00036F50 File Offset: 0x00035150
		private static AudioClip LoadWithNativeParser(byte[] audioData, string resourcePath, AudioType audioType)
		{
			AudioClip result;
			try
			{
				if (audioType != 20)
				{
					result = null;
				}
				else
				{
					result = ClassicEmbeddedResourceLoader.ParseWAVFromBytes(audioData, resourcePath);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Native parser failed: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00036FA8 File Offset: 0x000351A8
		private static AudioClip ParseWAVFromBytes(byte[] wavData, string resourcePath)
		{
			AudioClip result;
			try
			{
				bool flag = wavData.Length < 44;
				if (flag)
				{
					kernelllogger.Error(string.Format("[ClassicEmbeddedResourceLoader] WAV file too small ({0} bytes): {1}", wavData.Length, resourcePath));
					result = null;
				}
				else
				{
					bool flag2 = Encoding.ASCII.GetString(wavData, 0, 4) != "RIFF" || Encoding.ASCII.GetString(wavData, 8, 4) != "WAVE";
					if (flag2)
					{
						kernelllogger.Error("[ClassicEmbeddedResourceLoader] Invalid WAV header: " + resourcePath);
						result = null;
					}
					else
					{
						int num = ClassicEmbeddedResourceLoader.FindChunk(wavData, "fmt ");
						bool flag3 = num == -1;
						if (flag3)
						{
							kernelllogger.Error("[ClassicEmbeddedResourceLoader] No format chunk found: " + resourcePath);
							result = null;
						}
						else
						{
							int num2 = (int)BitConverter.ToInt16(wavData, num + 8);
							int num3 = (int)BitConverter.ToInt16(wavData, num + 10);
							int num4 = BitConverter.ToInt32(wavData, num + 12);
							int num5 = (int)BitConverter.ToInt16(wavData, num + 22);
							kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] WAV Format: {0}, {1}ch, {2}Hz, {3}bit", new object[]
							{
								num2,
								num3,
								num4,
								num5
							}));
							bool flag4 = num2 != 1;
							if (flag4)
							{
								kernelllogger.Error(string.Format("[ClassicEmbeddedResourceLoader] Unsupported audio format: {0} (only PCM supported)", num2));
								result = null;
							}
							else
							{
								bool flag5 = num3 <= 0 || num3 > 8;
								if (flag5)
								{
									kernelllogger.Error(string.Format("[ClassicEmbeddedResourceLoader] Invalid channel count: {0}", num3));
									result = null;
								}
								else
								{
									bool flag6 = num4 <= 0 || num4 > 192000;
									if (flag6)
									{
										kernelllogger.Error(string.Format("[ClassicEmbeddedResourceLoader] Invalid sample rate: {0}", num4));
										result = null;
									}
									else
									{
										int num6 = ClassicEmbeddedResourceLoader.FindChunk(wavData, "data");
										bool flag7 = num6 == -1;
										if (flag7)
										{
											kernelllogger.Error("[ClassicEmbeddedResourceLoader] No data chunk found: " + resourcePath);
											result = null;
										}
										else
										{
											int num7 = BitConverter.ToInt32(wavData, num6 + 4);
											int num8 = num6 + 8;
											bool flag8 = num8 + num7 > wavData.Length;
											if (flag8)
											{
												kernelllogger.Warning("[ClassicEmbeddedResourceLoader] Data chunk extends beyond file, truncating: " + resourcePath);
												num7 = wavData.Length - num8;
											}
											int num9 = num5 / 8;
											int num10 = num7 / num9 / num3;
											float[] array = new float[num10 * num3];
											int num11 = num5;
											int num12 = num11;
											if (num12 <= 16)
											{
												if (num12 == 8)
												{
													for (int i = 0; i < num10 * num3; i++)
													{
														array[i] = (float)(wavData[num8 + i] - 128) / 128f;
													}
													goto IL_392;
												}
												if (num12 == 16)
												{
													for (int j = 0; j < num10 * num3; j++)
													{
														short num13 = BitConverter.ToInt16(wavData, num8 + j * 2);
														array[j] = (float)num13 / 32768f;
													}
													goto IL_392;
												}
											}
											else
											{
												if (num12 == 24)
												{
													for (int k = 0; k < num10 * num3; k++)
													{
														int num14 = (int)wavData[num8 + k * 3] << 8 | (int)wavData[num8 + k * 3 + 1] << 16 | (int)wavData[num8 + k * 3 + 2] << 24;
														array[k] = (float)num14 / 2.1474836E+09f;
													}
													goto IL_392;
												}
												if (num12 == 32)
												{
													for (int l = 0; l < num10 * num3; l++)
													{
														int num15 = BitConverter.ToInt32(wavData, num8 + l * 4);
														array[l] = (float)num15 / 2.1474836E+09f;
													}
													goto IL_392;
												}
											}
											kernelllogger.Error(string.Format("[ClassicEmbeddedResourceLoader] Unsupported bit depth: {0}", num5));
											return null;
											IL_392:
											AudioClip audioClip = AudioClip.Create(Path.GetFileNameWithoutExtension(resourcePath), num10, num3, num4, false);
											bool flag9 = !audioClip.SetData(array, 0);
											if (flag9)
											{
												kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to set audio data: " + resourcePath);
												Object.Destroy(audioClip);
												result = null;
											}
											else
											{
												kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] WAV parsed successfully: {0} samples, {1:F2}s", num10, audioClip.length));
												result = audioClip;
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] WAV parsing error: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x000373F8 File Offset: 0x000355F8
		private static int FindChunk(byte[] data, string chunkId)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(chunkId);
			for (int i = 12; i < data.Length - bytes.Length - 4; i++)
			{
				bool flag = true;
				for (int j = 0; j < bytes.Length; j++)
				{
					bool flag2 = data[i + j] != bytes[j];
					if (flag2)
					{
						flag = false;
						break;
					}
				}
				bool flag3 = flag;
				if (flag3)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00037474 File Offset: 0x00035674
		private static AudioType DetectAudioFormat(string resourcePath)
		{
			AudioType result;
			try
			{
				string extension = Path.GetExtension(resourcePath);
				bool flag = string.IsNullOrEmpty(extension);
				if (flag)
				{
					kernelllogger.Warning("[ClassicEmbeddedResourceLoader] No file extension found for " + resourcePath + ", defaulting to WAV");
					result = 20;
				}
				else
				{
					AudioType audioType;
					bool flag2 = ClassicEmbeddedResourceLoader.AudioFormatMap.TryGetValue(extension, out audioType);
					if (flag2)
					{
						result = audioType;
					}
					else
					{
						kernelllogger.Warning("[ClassicEmbeddedResourceLoader] Unsupported audio format " + extension + ", defaulting to WAV");
						result = 20;
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Error detecting audio format for " + resourcePath + ": " + ex.Message);
				result = 20;
			}
			return result;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0003751C File Offset: 0x0003571C
		private static string GetTempFileExtension(AudioType audioType)
		{
			if (!true)
			{
			}
			string result;
			if (audioType != 2)
			{
				switch (audioType)
				{
				case 10:
					result = ".it";
					goto IL_8E;
				case 12:
					result = ".mod";
					goto IL_8E;
				case 13:
					result = ".mp3";
					goto IL_8E;
				case 14:
					result = ".ogg";
					goto IL_8E;
				case 17:
					result = ".s3m";
					goto IL_8E;
				case 20:
					result = ".wav";
					goto IL_8E;
				case 21:
					result = ".xm";
					goto IL_8E;
				}
				result = ".wav";
			}
			else
			{
				result = ".aiff";
			}
			IL_8E:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x000375C0 File Offset: 0x000357C0
		private static bool ValidateAudioClip(AudioClip clip, string resourcePath)
		{
			bool result;
			try
			{
				bool flag = clip == null;
				if (flag)
				{
					result = false;
				}
				else
				{
					bool flag2 = clip.length <= 0f;
					if (flag2)
					{
						kernelllogger.Warning("[ClassicEmbeddedResourceLoader] AudioClip has zero length: " + resourcePath);
						result = false;
					}
					else
					{
						bool flag3 = clip.channels <= 0 || clip.channels > 8;
						if (flag3)
						{
							kernelllogger.Warning(string.Format("[ClassicEmbeddedResourceLoader] Invalid channel count: {0}", clip.channels));
							result = false;
						}
						else
						{
							bool flag4 = clip.frequency <= 0;
							if (flag4)
							{
								kernelllogger.Warning(string.Format("[ClassicEmbeddedResourceLoader] Invalid frequency: {0}", clip.frequency));
								result = false;
							}
							else
							{
								result = true;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] AudioClip validation error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x000376B0 File Offset: 0x000358B0
		private static IEnumerator LoadAudioCoroutine(string resourcePath, Action<AudioClip> onComplete)
		{
			AudioClip result = null;
			Exception loadException = null;
			try
			{
				kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Starting async load for: " + resourcePath);
				byte[] audioData = ClassicEmbeddedResourceLoader.LoadEmbeddedResourceBytes(resourcePath);
				bool flag = audioData == null || audioData.Length == 0;
				if (flag)
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Failed to load audio data: " + resourcePath);
					if (onComplete != null)
					{
						onComplete(null);
					}
					yield break;
				}
				AudioType audioType = ClassicEmbeddedResourceLoader.DetectAudioFormat(resourcePath);
				result = ClassicEmbeddedResourceLoader.LoadFromBytesDirectly(audioData, resourcePath, audioType);
				bool flag2 = result == null;
				if (flag2)
				{
					string tempExtension = ClassicEmbeddedResourceLoader.GetTempFileExtension(audioType);
					string tempPath = Path.Combine(ClassicEmbeddedResourceLoader.CacheDirectory, string.Format("audio_coroutine_{0}{1}", Guid.NewGuid(), tempExtension));
					try
					{
						File.WriteAllBytes(tempPath, audioData);
						string fileUrl = "file://" + tempPath.Replace('\\', '/');
						UnityWebRequest request = null;
						try
						{
							request = UnityWebRequestMultimedia.GetAudioClip(fileUrl, audioType);
							request.SendWebRequest();
							bool flag3 = request.result == 1;
							if (flag3)
							{
								result = DownloadHandlerAudioClip.GetContent(request);
							}
							else
							{
								kernelllogger.Error("[ClassicEmbeddedResourceLoader] Async WebRequest error: " + request.error);
							}
						}
						finally
						{
							UnityWebRequest unityWebRequest = request;
							if (unityWebRequest != null)
							{
								unityWebRequest.Dispose();
							}
						}
						fileUrl = null;
						request = null;
					}
					catch (Exception ex3)
					{
						Exception ex = ex3;
						loadException = ex;
					}
					finally
					{
						bool flag4 = File.Exists(tempPath);
						if (flag4)
						{
							try
							{
								File.Delete(tempPath);
							}
							catch
							{
							}
						}
					}
					tempExtension = null;
					tempPath = null;
				}
				bool flag5 = result != null && ClassicEmbeddedResourceLoader.ValidateAudioClip(result, resourcePath);
				if (flag5)
				{
					result.name = Path.GetFileNameWithoutExtension(resourcePath);
					object obj = ClassicEmbeddedResourceLoader.AudioCacheLock;
					lock (obj)
					{
						ClassicEmbeddedResourceLoader.AudioCache[resourcePath] = result;
					}
					obj = null;
					kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Async loaded audio: " + resourcePath);
				}
				else
				{
					kernelllogger.Error("[ClassicEmbeddedResourceLoader] Async load failed: " + resourcePath);
					bool flag7 = loadException != null;
					if (flag7)
					{
						kernelllogger.Error("[ClassicEmbeddedResourceLoader] Exception: " + loadException.Message);
					}
					result = null;
				}
				audioData = null;
			}
			catch (Exception ex3)
			{
				Exception ex2 = ex3;
				kernelllogger.Error("[ClassicEmbeddedResourceLoader] Async load exception: " + ex2.Message);
				result = null;
			}
			if (onComplete != null)
			{
				onComplete(result);
			}
			yield break;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x000376C8 File Offset: 0x000358C8
		public static void ClearAllCaches()
		{
			object textureCacheLock = ClassicEmbeddedResourceLoader.TextureCacheLock;
			int count;
			lock (textureCacheLock)
			{
				count = ClassicEmbeddedResourceLoader.TextureCache.Count;
				ClassicEmbeddedResourceLoader.TextureCache.Clear();
			}
			object spriteCacheLock = ClassicEmbeddedResourceLoader.SpriteCacheLock;
			int count2;
			lock (spriteCacheLock)
			{
				count2 = ClassicEmbeddedResourceLoader.SpriteCache.Count;
				ClassicEmbeddedResourceLoader.SpriteCache.Clear();
			}
			object audioCacheLock = ClassicEmbeddedResourceLoader.AudioCacheLock;
			int count3;
			lock (audioCacheLock)
			{
				count3 = ClassicEmbeddedResourceLoader.AudioCache.Count;
				ClassicEmbeddedResourceLoader.AudioCache.Clear();
			}
			kernelllogger.Msg(string.Format("[ClassicEmbeddedResourceLoader] Cleared caches - Textures: {0}, Sprites: {1}, Audio: {2}", count, count2, count3));
		}

		// Token: 0x060008D9 RID: 2265 RVA: 0x000377D0 File Offset: 0x000359D0
		public static void ClearCacheEntry(string resourcePath)
		{
			bool flag = string.IsNullOrEmpty(resourcePath);
			if (!flag)
			{
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				object textureCacheLock = ClassicEmbeddedResourceLoader.TextureCacheLock;
				lock (textureCacheLock)
				{
					flag2 = ClassicEmbeddedResourceLoader.TextureCache.Remove(resourcePath);
				}
				object spriteCacheLock = ClassicEmbeddedResourceLoader.SpriteCacheLock;
				lock (spriteCacheLock)
				{
					flag3 = ClassicEmbeddedResourceLoader.SpriteCache.Remove(resourcePath);
				}
				object audioCacheLock = ClassicEmbeddedResourceLoader.AudioCacheLock;
				lock (audioCacheLock)
				{
					flag4 = ClassicEmbeddedResourceLoader.AudioCache.Remove(resourcePath);
				}
				bool flag8 = flag2 || flag3 || flag4;
				if (flag8)
				{
					kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Cleared cache entry: " + resourcePath);
				}
			}
		}

		// Token: 0x060008DA RID: 2266 RVA: 0x000378CC File Offset: 0x00035ACC
		[return: TupleElementNames(new string[]
		{
			"textures",
			"sprites",
			"audio",
			"cacheHits",
			"hitRatio"
		})]
		public static ValueTuple<int, int, int, int, float> GetCacheStats()
		{
			object textureCacheLock = ClassicEmbeddedResourceLoader.TextureCacheLock;
			ValueTuple<int, int, int, int, float> result;
			lock (textureCacheLock)
			{
				object spriteCacheLock = ClassicEmbeddedResourceLoader.SpriteCacheLock;
				lock (spriteCacheLock)
				{
					object audioCacheLock = ClassicEmbeddedResourceLoader.AudioCacheLock;
					lock (audioCacheLock)
					{
						float item = (ClassicEmbeddedResourceLoader._totalLoadsAttempted > 0) ? ((float)ClassicEmbeddedResourceLoader._cacheHits / (float)ClassicEmbeddedResourceLoader._totalLoadsAttempted) : 0f;
						result = new ValueTuple<int, int, int, int, float>(ClassicEmbeddedResourceLoader.TextureCache.Count, ClassicEmbeddedResourceLoader.SpriteCache.Count, ClassicEmbeddedResourceLoader.AudioCache.Count, ClassicEmbeddedResourceLoader._cacheHits, item);
					}
				}
			}
			return result;
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x000379AC File Offset: 0x00035BAC
		public static void LogStats()
		{
			ValueTuple<int, int, int, int, float> cacheStats = ClassicEmbeddedResourceLoader.GetCacheStats();
			kernelllogger.Msg("[ClassicEmbeddedResourceLoader] Statistics:");
			kernelllogger.Msg(string.Format("  Cached - Textures: {0}, Sprites: {1}, Audio: {2}", cacheStats.Item1, cacheStats.Item2, cacheStats.Item3));
			kernelllogger.Msg(string.Format("  Loads - Attempted: {0}, Successful: {1}", ClassicEmbeddedResourceLoader._totalLoadsAttempted, ClassicEmbeddedResourceLoader._totalLoadsSuccessful));
			kernelllogger.Msg(string.Format("  Cache Hits: {0}, Hit Ratio: {1:P1}", cacheStats.Item4, cacheStats.Item5));
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060008DC RID: 2268 RVA: 0x00037A46 File Offset: 0x00035C46
		public static bool IsInitialized
		{
			get
			{
				return ClassicEmbeddedResourceLoader._isInitialized;
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00037A50 File Offset: 0x00035C50
		public static string[] GetSupportedAudioFormats()
		{
			return Enumerable.ToArray<string>(ClassicEmbeddedResourceLoader.AudioFormatMap.Keys);
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00037A74 File Offset: 0x00035C74
		public static string[] GetSupportedImageFormats()
		{
			return Enumerable.ToArray<string>(ClassicEmbeddedResourceLoader.SupportedImageFormats);
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x00037A90 File Offset: 0x00035C90
		public static bool IsAudioFormatSupported(string extension)
		{
			return !string.IsNullOrEmpty(extension) && ClassicEmbeddedResourceLoader.AudioFormatMap.ContainsKey(extension);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00037AB8 File Offset: 0x00035CB8
		public static bool IsImageFormatSupported(string extension)
		{
			return !string.IsNullOrEmpty(extension) && ClassicEmbeddedResourceLoader.SupportedImageFormats.Contains(extension);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x00037AEC File Offset: 0x00035CEC
		// Note: this type is marked as 'beforefieldinit'.
		static ClassicEmbeddedResourceLoader()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			hashSet.Add(".png");
			hashSet.Add(".jpg");
			hashSet.Add(".jpeg");
			hashSet.Add(".bmp");
			hashSet.Add(".tga");
			hashSet.Add(".gif");
			ClassicEmbeddedResourceLoader.SupportedImageFormats = hashSet;
			ClassicEmbeddedResourceLoader.TextureCache = new Dictionary<string, Texture2D>();
			ClassicEmbeddedResourceLoader.SpriteCache = new Dictionary<string, Sprite>();
			ClassicEmbeddedResourceLoader.AudioCache = new Dictionary<string, AudioClip>();
			ClassicEmbeddedResourceLoader.ThemeFileTimestamps = new Dictionary<string, DateTime>();
			ClassicEmbeddedResourceLoader.TextureCacheLock = new object();
			ClassicEmbeddedResourceLoader.SpriteCacheLock = new object();
			ClassicEmbeddedResourceLoader.AudioCacheLock = new object();
			ClassicEmbeddedResourceLoader._totalLoadsAttempted = 0;
			ClassicEmbeddedResourceLoader._totalLoadsSuccessful = 0;
			ClassicEmbeddedResourceLoader._cacheHits = 0;
			ClassicEmbeddedResourceLoader._cacheDirInitialized = false;
			ClassicEmbeddedResourceLoader._themesDirInitialized = false;
			ClassicEmbeddedResourceLoader._isInitialized = false;
			ClassicEmbeddedResourceLoader._executingAssembly = Assembly.GetExecutingAssembly();
		}

		// Token: 0x04000435 RID: 1077
		private static readonly string CacheDirectory = Path.Combine(Application.persistentDataPath, "cache");

		// Token: 0x04000436 RID: 1078
		private static readonly string ThemesDirectory = Path.Combine(Environment.CurrentDirectory, "KernellVRC", "Themes");

		// Token: 0x04000437 RID: 1079
		private static readonly Dictionary<string, AudioType> AudioFormatMap = new Dictionary<string, AudioType>(StringComparer.OrdinalIgnoreCase)
		{
			{
				".wav",
				20
			},
			{
				".ogg",
				14
			},
			{
				".mp3",
				13
			},
			{
				".aiff",
				2
			},
			{
				".aif",
				2
			},
			{
				".mod",
				12
			},
			{
				".it",
				10
			},
			{
				".s3m",
				17
			},
			{
				".xm",
				21
			}
		};

		// Token: 0x04000438 RID: 1080
		private static readonly HashSet<string> SupportedImageFormats;

		// Token: 0x04000439 RID: 1081
		private static readonly Dictionary<string, Texture2D> TextureCache;

		// Token: 0x0400043A RID: 1082
		private static readonly Dictionary<string, Sprite> SpriteCache;

		// Token: 0x0400043B RID: 1083
		private static readonly Dictionary<string, AudioClip> AudioCache;

		// Token: 0x0400043C RID: 1084
		private static readonly Dictionary<string, DateTime> ThemeFileTimestamps;

		// Token: 0x0400043D RID: 1085
		private static readonly object TextureCacheLock;

		// Token: 0x0400043E RID: 1086
		private static readonly object SpriteCacheLock;

		// Token: 0x0400043F RID: 1087
		private static readonly object AudioCacheLock;

		// Token: 0x04000440 RID: 1088
		private static int _totalLoadsAttempted;

		// Token: 0x04000441 RID: 1089
		private static int _totalLoadsSuccessful;

		// Token: 0x04000442 RID: 1090
		private static int _cacheHits;

		// Token: 0x04000443 RID: 1091
		private static bool _cacheDirInitialized;

		// Token: 0x04000444 RID: 1092
		private static bool _themesDirInitialized;

		// Token: 0x04000445 RID: 1093
		private static bool _isInitialized;

		// Token: 0x04000446 RID: 1094
		private static readonly Assembly _executingAssembly;
	}
}
