using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KernellClientUI.Managers;
using KernellClientUI.UI.ActionMenu;
using KernellClientUI.UI.QuickMenu;
using KernelVRC;
using MelonLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KernellVRC
{
	// Token: 0x02000070 RID: 112
	public static class MenuSetup
	{
		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060004A6 RID: 1190 RVA: 0x0001A8CC File Offset: 0x00018ACC
		// (remove) Token: 0x060004A7 RID: 1191 RVA: 0x0001A900 File Offset: 0x00018B00
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action OnMenuSetupComplete;

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x060004A8 RID: 1192 RVA: 0x0001A934 File Offset: 0x00018B34
		// (remove) Token: 0x060004A9 RID: 1193 RVA: 0x0001A968 File Offset: 0x00018B68
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<string> OnInitializationStep;

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x060004AA RID: 1194 RVA: 0x0001A99C File Offset: 0x00018B9C
		// (remove) Token: 0x060004AB RID: 1195 RVA: 0x0001A9D0 File Offset: 0x00018BD0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public static event Action<string> OnInitializationError;

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004AC RID: 1196 RVA: 0x0001AA03 File Offset: 0x00018C03
		public static bool IsInitialized
		{
			get
			{
				return MenuSetup._isInitialized;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001AA0C File Offset: 0x00018C0C
		public static bool IsInitializing
		{
			get
			{
				return MenuSetup._isInitializing;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004AE RID: 1198 RVA: 0x0001AA15 File Offset: 0x00018C15
		public static bool IsComplete
		{
			get
			{
				return MenuSetup._initializationComplete;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001AA1E File Offset: 0x00018C1E
		public static bool IsReady
		{
			get
			{
				return MenuSetup._isInitialized && MenuSetup._uiManager != null && MenuSetup._uiManager.QMMenu != null;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x0001AA40 File Offset: 0x00018C40
		public static UiManager UIManager
		{
			get
			{
				return MenuSetup._uiManager;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001AA47 File Offset: 0x00018C47
		public static string[] InitializationSteps
		{
			get
			{
				return MenuSetup._initializationSteps.ToArray();
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004B2 RID: 1202 RVA: 0x0001AA53 File Offset: 0x00018C53
		public static string[] InitializationErrors
		{
			get
			{
				return MenuSetup._initializationErrors.ToArray();
			}
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001AA60 File Offset: 0x00018C60
		public static void OnSceneChanging(string sceneName)
		{
			object initializationLock = MenuSetup._initializationLock;
			lock (initializationLock)
			{
				MenuSetup._isSceneChanging = true;
				MenuSetup.LogStep("Scene changing to: " + sceneName);
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x0001AAB8 File Offset: 0x00018CB8
		public static void OnSceneChanged(string sceneName)
		{
			object initializationLock = MenuSetup._initializationLock;
			lock (initializationLock)
			{
				MenuSetup._currentSceneName = sceneName;
				MenuSetup._isSceneChanging = false;
				MenuSetup.LogStep("Scene changed to: " + sceneName);
			}
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x0001AB14 File Offset: 0x00018D14
		public static void Initialize()
		{
			object initializationLock = MenuSetup._initializationLock;
			lock (initializationLock)
			{
				bool flag2 = MenuSetup._isInitialized || MenuSetup._isInitializing;
				if (flag2)
				{
					MenuSetup.LogStep("Initialize called but already initialized/initializing");
					return;
				}
				MenuSetup._isInitializing = true;
				MenuSetup._initStartTime = DateTime.Now;
				MenuSetup.ClearState();
			}
			MenuSetup.LogStep("Starting MenuSetup initialization");
			MelonCoroutines.Start(MenuSetup.SafeInitializationCoroutine());
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0001ABA8 File Offset: 0x00018DA8
		public static void OnUiManagerInit()
		{
			MenuSetup.Initialize();
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x0001ABB1 File Offset: 0x00018DB1
		private static IEnumerator SafeInitializationCoroutine()
		{
			MenuSetup.LogStep("Starting safe initialization coroutine");
			float uiWaitTime = 0f;
			bool uiFound = false;
			while (!uiFound && uiWaitTime < 1E+14f)
			{
				uiFound = MenuSetup.TryFindUIComponents();
				bool flag = !uiFound;
				if (flag)
				{
					bool flag2 = uiWaitTime % 5f < 0.5f;
					if (flag2)
					{
						MenuSetup.LogStep(string.Format("Waiting for UI components... ({0:F0}s)", uiWaitTime));
					}
					yield return new WaitForSeconds(0.5f);
					uiWaitTime += 0.5f;
				}
			}
			bool flag3 = !uiFound;
			if (flag3)
			{
				MenuSetup.LogError("Failed to find UI components after timeout");
				MenuSetup.CompleteInitialization(false);
				yield break;
			}
			MenuSetup.LogStep(string.Format("UI components found after {0:F0}s", uiWaitTime));
			yield return new WaitForSeconds(0.5f);
			bool resourcesInitialized = MenuSetup.InitializeResourcesSafe();
			bool flag4 = !resourcesInitialized;
			if (flag4)
			{
				MenuSetup.LogError("Resource initialization failed");
				MenuSetup.CompleteInitialization(false);
				yield break;
			}
			yield return new WaitForSeconds(1f);
			bool uiManagerCreated = MenuSetup.CreateUIManagerSafe();
			bool flag5 = !uiManagerCreated;
			if (flag5)
			{
				MenuSetup.LogError("UI Manager creation failed");
				MenuSetup.CompleteInitialization(false);
				yield break;
			}
			yield return new WaitForSeconds(0.5f);
			bool menuCreated = MenuSetup.CreateMenuStructureSafe();
			bool flag6 = !menuCreated;
			if (flag6)
			{
				MenuSetup.LogError("Menu structure creation failed");
				MenuSetup.CompleteInitialization(false);
				yield break;
			}
			MenuSetup.CompleteInitialization(true);
			yield break;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x0001ABBC File Offset: 0x00018DBC
		private static bool TryFindUIComponents()
		{
			bool result;
			try
			{
				bool flag = MenuSetup._vrcUiManager == null;
				if (flag)
				{
					MenuSetup._vrcUiManager = VRCUiManager.field_Private_Static_VRCUiManager_0;
				}
				GameObject gameObject = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)");
				bool flag2 = gameObject != null;
				if (flag2)
				{
					MenuSetup._quickMenuTransform = gameObject.transform;
					MenuSetup._menuDashboard = MenuSetup._quickMenuTransform.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard");
					bool flag3 = MenuSetup._menuDashboard != null;
					if (flag3)
					{
						MenuSetup.LogStep("Found QuickMenu via direct find");
						return true;
					}
				}
				GameObject[] allRootObjects = MenuSetup.GetAllRootObjects();
				foreach (GameObject gameObject2 in allRootObjects)
				{
					bool flag4 = gameObject2.name.StartsWith("_Application");
					if (flag4)
					{
						Transform transform = gameObject2.transform.Find("UserInterface");
						bool flag5 = transform != null;
						if (flag5)
						{
							Transform transform2 = transform.Find("Canvas_QuickMenu(Clone)");
							bool flag6 = transform2 != null;
							if (flag6)
							{
								MenuSetup._quickMenuTransform = transform2;
								MenuSetup._menuDashboard = transform2.Find("CanvasGroup/Container/Window/QMParent/Menu_Dashboard");
								bool flag7 = MenuSetup._menuDashboard != null;
								if (flag7)
								{
									MenuSetup.LogStep("Found QuickMenu in " + gameObject2.name);
									return true;
								}
							}
						}
					}
				}
				result = false;
			}
			catch (Exception ex)
			{
				MenuSetup.LogError("Error finding UI components: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x0001AD54 File Offset: 0x00018F54
		private static GameObject[] GetAllRootObjects()
		{
			GameObject[] result;
			try
			{
				Scene activeScene = SceneManager.GetActiveScene();
				GameObject[] dontDestroyOnLoadObjects = MenuSetup.GetDontDestroyOnLoadObjects();
				result = Enumerable.ToArray<GameObject>(Enumerable.Where<GameObject>(Enumerable.Concat<GameObject>(activeScene.GetRootGameObjects(), dontDestroyOnLoadObjects), (GameObject obj) => obj != null));
			}
			catch
			{
				result = new GameObject[0];
			}
			return result;
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x0001ADC4 File Offset: 0x00018FC4
		private static GameObject[] GetDontDestroyOnLoadObjects()
		{
			GameObject[] result;
			try
			{
				result = Enumerable.ToArray<GameObject>(Enumerable.Where<GameObject>(Object.FindObjectsOfType<GameObject>(), (GameObject obj) => obj.scene.name == "DontDestroyOnLoad"));
			}
			catch
			{
				result = new GameObject[0];
			}
			return result;
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x0001AE20 File Offset: 0x00019020
		private static bool InitializeResourcesSafe()
		{
			bool result;
			try
			{
				bool flag = !MenuSetup._resourceLoaderInitialized;
				if (flag)
				{
					EmbeddedResourceLoader.Initialize();
					MenuSetup._resourceLoaderInitialized = true;
					MenuSetup.LogStep("Resource loader initialized");
				}
				result = MenuSetup.LoadMainButtonSpriteSafe();
			}
			catch (Exception ex)
			{
				MenuSetup.LogError("Resource initialization error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001AE8C File Offset: 0x0001908C
		private static bool LoadMainButtonSpriteSafe()
		{
			bool result;
			try
			{
				string[] array = new string[]
				{
					"KernellVRCLite.assets.Kernel-icon-2025.png",
					"KernellVRCLite.assets.Banner-NP-2025.png"
				};
				foreach (string text in array)
				{
					MenuSetup._mainButtonSprite = MenuSetup.LoadSprite(text);
					bool flag = MenuSetup._mainButtonSprite != null;
					if (flag)
					{
						MenuSetup.LogStep("Loaded main button sprite: " + text);
						return true;
					}
				}
				MenuSetup._mainButtonSprite = MenuSetup.CreateFallbackSprite();
				MenuSetup.LogStep("Using fallback sprite");
				result = (MenuSetup._mainButtonSprite != null);
			}
			catch (Exception ex)
			{
				MenuSetup.LogError("Sprite loading error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001AF54 File Offset: 0x00019154
		private static Sprite LoadSprite(string path)
		{
			Sprite result;
			try
			{
				Sprite sprite;
				bool flag = MenuSetup._spriteCache.TryGetValue(path, out sprite);
				if (flag)
				{
					result = sprite;
				}
				else
				{
					Sprite sprite2 = EmbeddedResourceLoader.LoadEmbeddedSprite(path, null, 100f);
					bool flag2 = sprite2 != null;
					if (flag2)
					{
						MenuSetup._spriteCache[path] = sprite2;
					}
					result = sprite2;
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0001AFC8 File Offset: 0x000191C8
		private static Sprite CreateFallbackSprite()
		{
			Sprite result;
			try
			{
				Texture2D texture2D = new Texture2D(64, 64, 4, false);
				Color32[] array = new Color32[4096];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = new Color32(128, 0, byte.MaxValue, byte.MaxValue);
				}
				texture2D.SetPixels32(array);
				texture2D.Apply();
				result = Sprite.Create(texture2D, new Rect(0f, 0f, 64f, 64f), new Vector2(0.5f, 0.5f));
			}
			catch (Exception ex)
			{
				MenuSetup.LogError("Failed to create fallback sprite: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0001B098 File Offset: 0x00019298
		private static bool CreateUIManagerSafe()
		{
			bool result;
			try
			{
				bool flag = MenuSetup._mainButtonSprite == null;
				if (flag)
				{
					MenuSetup.LogError("Cannot create UI Manager: no sprite available");
					result = false;
				}
				else
				{
					MenuSetup._uiManager = new UiManager(MenuSetup._menuConfig.MainButtonText, MenuSetup._mainButtonSprite, true, true, false, MenuSetup._menuConfig.MainButtonColor, MenuSetup._menuConfig.Layout, false);
					UiManager uiManager = MenuSetup._uiManager;
					bool flag2 = ((uiManager != null) ? uiManager.QMMenu : null) == null;
					if (flag2)
					{
						MenuSetup.LogError("UI Manager created but QMMenu is null");
						result = false;
					}
					else
					{
						MenuSetup.LogStep("UI Manager created successfully");
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				MenuSetup.LogError("UI Manager creation error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001B160 File Offset: 0x00019360
		private static bool CreateMenuStructureSafe()
		{
			MenuSetup._menuCreationAttempted = true;
			bool result;
			try
			{
				UiManager uiManager = MenuSetup._uiManager;
				bool flag = ((uiManager != null) ? uiManager.QMMenu : null) == null;
				if (flag)
				{
					MenuSetup.LogError("Cannot create menu structure: UI Manager or QMMenu is null");
					result = false;
				}
				else
				{
					MenuSetup.LogStep("Creating menu structure...");
					int num = 0;
					foreach (MenuSetup.CategoryInfo categoryInfo in MenuSetup._menuConfig.Categories)
					{
						try
						{
							Sprite sprite = MenuSetup.LoadSprite(categoryInfo.SpriteResource) ?? MenuSetup._mainButtonSprite;
							ReCategoryPage reCategoryPage = MenuSetup._uiManager.QMMenu.AddCategoryPage(categoryInfo.Name, categoryInfo.Description, sprite, categoryInfo.Color);
							bool flag2 = reCategoryPage != null;
							if (flag2)
							{
								MenuSetup._categoryPages[categoryInfo.Name] = reCategoryPage;
								num++;
								MenuSetup.LogStep("Created category: " + categoryInfo.Name);
							}
						}
						catch (Exception ex)
						{
							MenuSetup.LogError("Failed to create category " + categoryInfo.Name + ": " + ex.Message);
						}
					}
					bool flag3 = num == 0;
					if (flag3)
					{
						MenuSetup.LogError("No categories were created");
						result = false;
					}
					else
					{
						try
						{
							MenuSetup._uiManager.QMMenu.AddSpacer(null);
						}
						catch (Exception ex2)
						{
							MenuSetup.LogError("Failed to add spacer: " + ex2.Message);
						}
						int num2 = 0;
						foreach (KeyValuePair<string, string[]> keyValuePair in MenuSetup._menuConfig.Subcategories)
						{
							ReCategoryPage reCategoryPage2;
							bool flag4 = MenuSetup._categoryPages.TryGetValue(keyValuePair.Key, out reCategoryPage2);
							if (flag4)
							{
								foreach (string text in keyValuePair.Value)
								{
									try
									{
										reCategoryPage2.AddCategory(text, true, "#ffffff", false);
										num2++;
									}
									catch (Exception ex3)
									{
										MenuSetup.LogError("Failed to create subcategory " + text + ": " + ex3.Message);
									}
								}
							}
						}
						try
						{
							MenuSetup._uiManager.SetAllButtonLayouts(MenuSetup._menuConfig.Layout);
						}
						catch (Exception ex4)
						{
							MenuSetup.LogError("Layout configuration failed: " + ex4.Message);
						}
						MenuSetup.CreateAdditionalComponentsSafe();
						MenuSetup.LogStep(string.Format("Menu structure created: {0} categories, {1} subcategories", num, num2));
						result = (num > 0);
					}
				}
			}
			catch (Exception ex5)
			{
				MenuSetup.LogError("Critical error in menu structure creation: " + ex5.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001B4A8 File Offset: 0x000196A8
		private static void CreateAdditionalComponentsSafe()
		{
			try
			{
				bool flag = MenuSetup._mainButtonSprite != null;
				if (flag)
				{
					new ActionMenuPage("Kernell", MenuSetup._mainButtonSprite);
					new ActionMenuPage("Kernell (Alt)", MenuSetup._mainButtonSprite);
				}
				UiManager uiManager = MenuSetup._uiManager;
				bool flag2 = ((uiManager != null) ? uiManager.MMenu : null) != null;
				if (flag2)
				{
					MenuSetup._uiManager.MMenu.AddnGetSection("Kernell Client", false, "#ffffff");
				}
			}
			catch (Exception ex)
			{
				MenuSetup.LogError("Additional components creation failed: " + ex.Message);
			}
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001B54C File Offset: 0x0001974C
		private static void CompleteInitialization(bool success)
		{
			object initializationLock = MenuSetup._initializationLock;
			lock (initializationLock)
			{
				MenuSetup._isInitialized = success;
				MenuSetup._initializationComplete = success;
				MenuSetup._isInitializing = false;
				double totalMilliseconds = (DateTime.Now - MenuSetup._initStartTime).TotalMilliseconds;
				if (success)
				{
					MenuSetup.LogStep(string.Format("MenuSetup completed successfully in {0:F0}ms", totalMilliseconds));
				}
				else
				{
					MenuSetup.LogError(string.Format("MenuSetup failed after {0:F0}ms", totalMilliseconds));
				}
				try
				{
					Action onMenuSetupComplete = MenuSetup.OnMenuSetupComplete;
					if (onMenuSetupComplete != null)
					{
						onMenuSetupComplete();
					}
				}
				catch (Exception ex)
				{
					MenuSetup.LogError("Completion notification error: " + ex.Message);
				}
				MenuSetup.PrintSummary();
			}
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001B638 File Offset: 0x00019838
		private static void ClearState()
		{
			MenuSetup._initializationSteps.Clear();
			MenuSetup._initializationErrors.Clear();
			MenuSetup._categoryPages.Clear();
			MenuSetup._failureCount = 0;
			MenuSetup._menuCreationAttempted = false;
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001B66C File Offset: 0x0001986C
		public static void Cleanup()
		{
			try
			{
				object initializationLock = MenuSetup._initializationLock;
				lock (initializationLock)
				{
					MenuSetup._isInitialized = false;
					MenuSetup._isInitializing = false;
					MenuSetup._initializationComplete = false;
					MenuSetup._uiManager = null;
					MenuSetup._mainButtonSprite = null;
					MenuSetup._spriteCache.Clear();
					MenuSetup.ClearState();
				}
				MenuSetup.LogStep("MenuSetup cleanup completed");
			}
			catch (Exception ex)
			{
				MenuSetup.LogError("Cleanup error: " + ex.Message);
			}
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001B718 File Offset: 0x00019918
		private static void LogStep(string message)
		{
			string item = string.Format("[{0:HH:mm:ss.fff}] {1}", DateTime.Now, message);
			MenuSetup._initializationSteps.Add(item);
			kernelllogger.Msg("[MenuSetup] " + message);
			Action<string> onInitializationStep = MenuSetup.OnInitializationStep;
			if (onInitializationStep != null)
			{
				onInitializationStep(message);
			}
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001B76C File Offset: 0x0001996C
		private static void LogError(string message)
		{
			string item = string.Format("[{0:HH:mm:ss.fff}] ERROR: {1}", DateTime.Now, message);
			MenuSetup._initializationErrors.Add(item);
			kernelllogger.Error("[MenuSetup] " + message);
			Action<string> onInitializationError = MenuSetup.OnInitializationError;
			if (onInitializationError != null)
			{
				onInitializationError(message);
			}
			MenuSetup._failureCount++;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001B7CC File Offset: 0x000199CC
		private static void PrintSummary()
		{
			kernelllogger.Msg("=== MenuSetup Summary ===");
			kernelllogger.Msg("Status: " + (MenuSetup.IsReady ? "Ready" : (MenuSetup._initializationComplete ? "Partial" : "Failed")));
			kernelllogger.Msg(string.Format("Duration: {0:F0}ms", (DateTime.Now - MenuSetup._initStartTime).TotalMilliseconds));
			kernelllogger.Msg("UI Manager: " + ((MenuSetup._uiManager != null) ? "Created" : "Failed"));
			kernelllogger.Msg(string.Format("Categories: {0}", MenuSetup._categoryPages.Count));
			kernelllogger.Msg(string.Format("Errors: {0}", MenuSetup._initializationErrors.Count));
			bool flag = MenuSetup._initializationErrors.Count > 0;
			if (flag)
			{
				kernelllogger.Msg("Recent errors:");
				foreach (string str in Enumerable.Skip<string>(MenuSetup._initializationErrors, Math.Max(0, MenuSetup._initializationErrors.Count - 3)))
				{
					kernelllogger.Error("  " + str);
				}
			}
			bool flag2 = MenuSetup._categoryPages.Count > 0;
			if (flag2)
			{
				kernelllogger.Msg("Created categories:");
				foreach (KeyValuePair<string, ReCategoryPage> keyValuePair in MenuSetup._categoryPages)
				{
					kernelllogger.Msg("  - " + keyValuePair.Key);
				}
			}
			kernelllogger.Msg("========================");
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001B9AC File Offset: 0x00019BAC
		// Note: this type is marked as 'beforefieldinit'.
		static MenuSetup()
		{
			MenuSetup.MenuConfiguration menuConfiguration = new MenuSetup.MenuConfiguration();
			menuConfiguration.MainButtonText = "<color=purple>Kernell Net Lite</color>";
			menuConfiguration.MainButtonColor = "#ffffff";
			menuConfiguration.Layout = ButtonLayoutShape.FivePanelLayout;
			menuConfiguration.Categories = new MenuSetup.CategoryInfo[]
			{
				new MenuSetup.CategoryInfo("Exploits", "Exploits to exploit!", "KernellVRCLite.assets.EI.png", "#ffffff"),
				new MenuSetup.CategoryInfo("Utility", "Movement and utilities", "KernellVRCLite.assets.utility.png", "#ffffff"),
				new MenuSetup.CategoryInfo("OSC", "OSC integration", "KernellVRCLite.assets.OSC.png", "#ffffff"),
				new MenuSetup.CategoryInfo("Security", "Security features", "KernellVRCLite.assets.security.png", "#ffffff"),
				new MenuSetup.CategoryInfo("Config", "Configuration", "KernellVRCLite.assets.config.png", "#ffffff")
			};
			MenuSetup.MenuConfiguration menuConfiguration2 = menuConfiguration;
			Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
			dictionary["Utility"] = new string[]
			{
				"ESP",
				"Movement",
				"Other"
			};
			dictionary["Exploits"] = new string[]
			{
				"GameWorlds"
			};
			dictionary["Security"] = new string[]
			{
				"Security"
			};
			dictionary["OSC"] = new string[]
			{
				"Controls"
			};
			dictionary["Config"] = new string[]
			{
				"Settings"
			};
			menuConfiguration2.Subcategories = dictionary;
			MenuSetup._menuConfig = menuConfiguration;
		}

		// Token: 0x040001EB RID: 491
		private static volatile bool _isInitialized = false;

		// Token: 0x040001EC RID: 492
		private static volatile bool _isInitializing = false;

		// Token: 0x040001ED RID: 493
		private static volatile bool _initializationComplete = false;

		// Token: 0x040001EE RID: 494
		private static volatile bool _menuCreationAttempted = false;

		// Token: 0x040001EF RID: 495
		private static readonly object _initializationLock = new object();

		// Token: 0x040001F0 RID: 496
		private const float INITIALIZATION_TIMEOUT = 1E+14f;

		// Token: 0x040001F1 RID: 497
		private const float UI_CHECK_INTERVAL = 0.5f;

		// Token: 0x040001F2 RID: 498
		private const float MENU_CREATION_DELAY = 1f;

		// Token: 0x040001F3 RID: 499
		private static DateTime _initStartTime;

		// Token: 0x040001F4 RID: 500
		private static readonly List<string> _initializationSteps = new List<string>();

		// Token: 0x040001F5 RID: 501
		private static readonly List<string> _initializationErrors = new List<string>();

		// Token: 0x040001F6 RID: 502
		private static int _failureCount = 0;

		// Token: 0x040001F7 RID: 503
		private static VRCUiManager _vrcUiManager;

		// Token: 0x040001F8 RID: 504
		private static Transform _quickMenuTransform;

		// Token: 0x040001F9 RID: 505
		private static Transform _menuDashboard;

		// Token: 0x040001FA RID: 506
		private static bool _resourceLoaderInitialized = false;

		// Token: 0x040001FB RID: 507
		private static bool _isSceneChanging = false;

		// Token: 0x040001FC RID: 508
		private static string _currentSceneName = "";

		// Token: 0x04000200 RID: 512
		public static UiManager _uiManager = null;

		// Token: 0x04000201 RID: 513
		private static Sprite _mainButtonSprite = null;

		// Token: 0x04000202 RID: 514
		private static readonly Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();

		// Token: 0x04000203 RID: 515
		private static readonly Dictionary<string, ReCategoryPage> _categoryPages = new Dictionary<string, ReCategoryPage>();

		// Token: 0x04000204 RID: 516
		private static readonly MenuSetup.MenuConfiguration _menuConfig;

		// Token: 0x02000115 RID: 277
		private class MenuConfiguration
		{
			// Token: 0x170001F5 RID: 501
			// (get) Token: 0x06000B29 RID: 2857 RVA: 0x000426D2 File Offset: 0x000408D2
			// (set) Token: 0x06000B2A RID: 2858 RVA: 0x000426DA File Offset: 0x000408DA
			public string MainButtonText { get; set; }

			// Token: 0x170001F6 RID: 502
			// (get) Token: 0x06000B2B RID: 2859 RVA: 0x000426E3 File Offset: 0x000408E3
			// (set) Token: 0x06000B2C RID: 2860 RVA: 0x000426EB File Offset: 0x000408EB
			public string MainButtonColor { get; set; }

			// Token: 0x170001F7 RID: 503
			// (get) Token: 0x06000B2D RID: 2861 RVA: 0x000426F4 File Offset: 0x000408F4
			// (set) Token: 0x06000B2E RID: 2862 RVA: 0x000426FC File Offset: 0x000408FC
			public ButtonLayoutShape Layout { get; set; }

			// Token: 0x170001F8 RID: 504
			// (get) Token: 0x06000B2F RID: 2863 RVA: 0x00042705 File Offset: 0x00040905
			// (set) Token: 0x06000B30 RID: 2864 RVA: 0x0004270D File Offset: 0x0004090D
			public MenuSetup.CategoryInfo[] Categories { get; set; }

			// Token: 0x170001F9 RID: 505
			// (get) Token: 0x06000B31 RID: 2865 RVA: 0x00042716 File Offset: 0x00040916
			// (set) Token: 0x06000B32 RID: 2866 RVA: 0x0004271E File Offset: 0x0004091E
			public Dictionary<string, string[]> Subcategories { get; set; }
		}

		// Token: 0x02000116 RID: 278
		private class CategoryInfo
		{
			// Token: 0x170001FA RID: 506
			// (get) Token: 0x06000B34 RID: 2868 RVA: 0x00042727 File Offset: 0x00040927
			// (set) Token: 0x06000B35 RID: 2869 RVA: 0x0004272F File Offset: 0x0004092F
			public string Name { get; set; }

			// Token: 0x170001FB RID: 507
			// (get) Token: 0x06000B36 RID: 2870 RVA: 0x00042738 File Offset: 0x00040938
			// (set) Token: 0x06000B37 RID: 2871 RVA: 0x00042740 File Offset: 0x00040940
			public string Description { get; set; }

			// Token: 0x170001FC RID: 508
			// (get) Token: 0x06000B38 RID: 2872 RVA: 0x00042749 File Offset: 0x00040949
			// (set) Token: 0x06000B39 RID: 2873 RVA: 0x00042751 File Offset: 0x00040951
			public string SpriteResource { get; set; }

			// Token: 0x170001FD RID: 509
			// (get) Token: 0x06000B3A RID: 2874 RVA: 0x0004275A File Offset: 0x0004095A
			// (set) Token: 0x06000B3B RID: 2875 RVA: 0x00042762 File Offset: 0x00040962
			public string Color { get; set; }

			// Token: 0x06000B3C RID: 2876 RVA: 0x0004276B File Offset: 0x0004096B
			public CategoryInfo(string name, string description, string spriteResource, string color = "#ffffff")
			{
				this.Name = name;
				this.Description = description;
				this.SpriteResource = spriteResource;
				this.Color = color;
			}
		}
	}
}
