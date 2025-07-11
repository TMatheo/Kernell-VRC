using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using KernellVRCLite.Network;
using KernelVRC;
using MelonLoader;
using TMPro;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.Localization;

namespace KernellVRC.modules
{
	// Token: 0x02000074 RID: 116
	public class PlayerList : KernelModuleBase
	{
		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001C087 File Offset: 0x0001A287
		public override string ModuleName { get; } = "PlayerList";

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004D6 RID: 1238 RVA: 0x0001C08F File Offset: 0x0001A28F
		public override string Version { get; } = "4.3.1";

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x0001C097 File Offset: 0x0001A297
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.PlayerEvents | ModuleCapabilities.NetworkEvents | ModuleCapabilities.MenuEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004D8 RID: 1240 RVA: 0x000036A7 File Offset: 0x000018A7
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every30Frames;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004D9 RID: 1241 RVA: 0x00003315 File Offset: 0x00001515
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.Normal;
			}
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001C0A0 File Offset: 0x0001A2A0
		public override void OnUiManagerInit()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					this.Log("Starting PlayerList initialization...");
					this.EnsureDirectories();
					this.LoadState();
					this.InitializeNetworkIntegration();
					this.StartInitialization();
				}, "OnUiManagerInit");
			}
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001C0D4 File Offset: 0x0001A2D4
		public override void OnUpdate()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				bool flag = !this._isInitialized && !this._isInitializing && Time.time >= this._nextRetryTime;
				if (flag)
				{
					this.StartInitialization();
				}
				else
				{
					bool flag2 = this._consecutiveErrors > 0 && Time.time - this._lastErrorTime > 30f;
					if (flag2)
					{
						this.SafeExecute(delegate
						{
							this._consecutiveErrors = 0;
							this.Log("PlayerList error recovery complete");
						}, "ErrorRecovery");
					}
					object stateLock = this._stateLock;
					lock (stateLock)
					{
						bool worldTransitioning = this._worldTransitioning;
						if (worldTransitioning)
						{
							return;
						}
					}
					bool flag4 = Time.time - this._lastNetworkCheck >= 5f;
					if (flag4)
					{
						this.CheckNetworkStatus();
						this._lastNetworkCheck = Time.time;
					}
					bool flag5 = Time.time - this._lastKrnlCheckTime >= 0.5f;
					if (flag5)
					{
						this.ProcessKrnlCheckQueue();
						this._lastKrnlCheckTime = Time.time;
					}
					bool flag6;
					if (this._isInitialized)
					{
						GameObject playerListCanvas = this._playerListCanvas;
						if (playerListCanvas != null && playerListCanvas.activeInHierarchy)
						{
							flag6 = (this._consecutiveErrors < 5);
							goto IL_147;
						}
					}
					flag6 = false;
					IL_147:
					bool flag7 = flag6;
					if (flag7)
					{
						bool flag8 = Time.time - this._lastUpdateTime >= 1.5f;
						if (flag8)
						{
							this.UpdatePlayerList();
							this._lastUpdateTime = Time.time;
						}
					}
					bool flag9 = Time.frameCount % 1800 == 0;
					if (flag9)
					{
						this.SafeExecute(new Action(this.CheckAndRecoverFromBrokenState), "BrokenStateCheck");
					}
				}
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x0001C2A0 File Offset: 0x0001A4A0
		public override void OnEnterWorld(ApiWorld world, ApiWorldInstance instance)
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					PlayerList <>4__this = this;
					string str = "Entered world: ";
					ApiWorld world2 = world;
					<>4__this.Log(str + (((world2 != null) ? world2.name : null) ?? "Unknown"));
					object stateLock = this._stateLock;
					lock (stateLock)
					{
						this._worldTransitioning = false;
					}
					object updateLock = this._updateLock;
					lock (updateLock)
					{
						this._playerCache.Clear();
						this._activePlayerIds.Clear();
						this._hasInitialScanStarted = false;
						this._pendingKrnlChecks.Clear();
						this._krnlCheckQueue.Clear();
						this._isScanning = false;
					}
					ApiWorld world3 = world;
					bool flag3 = ((world3 != null) ? world3.id : null) != null;
					if (flag3)
					{
						KernellNetworkIntegration.OnWorldEntered(world.id);
					}
					bool flag4;
					if (this._isInitialized)
					{
						GameObject playerListCanvas = this._playerListCanvas;
						flag4 = (playerListCanvas != null && playerListCanvas.activeInHierarchy);
					}
					else
					{
						flag4 = false;
					}
					bool flag5 = flag4;
					if (flag5)
					{
						this.UpdatePlayerList();
						bool networkConnected = this._networkConnected;
						if (networkConnected)
						{
							this.StartInitialScan();
						}
					}
				}, "OnEnterWorld");
			}
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001C2E8 File Offset: 0x0001A4E8
		public override void OnLeaveWorld()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					this.Log("Left world");
					object stateLock = this._stateLock;
					lock (stateLock)
					{
						this._worldTransitioning = true;
					}
					KernellNetworkIntegration.OnWorldLeaving();
					object updateLock = this._updateLock;
					lock (updateLock)
					{
						this._playerCache.Clear();
						this._activePlayerIds.Clear();
						this._hasInitialScanStarted = false;
						this._pendingKrnlChecks.Clear();
						this._krnlCheckQueue.Clear();
						this._isScanning = false;
					}
				}, "OnLeaveWorld");
			}
		}

		// Token: 0x060004DE RID: 1246 RVA: 0x0001C31C File Offset: 0x0001A51C
		public override void OnPlayerJoined(Player player)
		{
			bool flag;
			if (!this._disposed)
			{
				Player player2 = player;
				object obj;
				if (player2 == null)
				{
					obj = null;
				}
				else
				{
					APIUser field_Private_APIUser_ = player2.field_Private_APIUser_0;
					obj = ((field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null);
				}
				flag = (obj == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this.SafeExecute(delegate
				{
					string id = player.field_Private_APIUser_0.id;
					string str = player.field_Private_APIUser_0.displayName ?? "Unknown";
					this.Log("Player joined: " + str);
					bool networkConnected = this._networkConnected;
					if (networkConnected)
					{
						this.QueueKrnlCheck(id);
					}
					bool flag3;
					if (this._isInitialized)
					{
						GameObject playerListCanvas = this._playerListCanvas;
						flag3 = (playerListCanvas != null && playerListCanvas.activeInHierarchy);
					}
					else
					{
						flag3 = false;
					}
					bool flag4 = flag3;
					if (flag4)
					{
						this.UpdatePlayerList();
					}
				}, "OnPlayerJoined");
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x0001C38C File Offset: 0x0001A58C
		public override void OnPlayerLeft(Player player)
		{
			bool flag;
			if (!this._disposed)
			{
				Player player2 = player;
				object obj;
				if (player2 == null)
				{
					obj = null;
				}
				else
				{
					APIUser field_Private_APIUser_ = player2.field_Private_APIUser_0;
					obj = ((field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null);
				}
				flag = (obj == null);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			if (!flag2)
			{
				this.SafeExecute(delegate
				{
					string id = player.field_Private_APIUser_0.id;
					string str = player.field_Private_APIUser_0.displayName ?? "Unknown";
					this.Log("Player left: " + str);
					object updateLock = this._updateLock;
					lock (updateLock)
					{
						this._playerCache.Remove(id);
						this._activePlayerIds.Remove(id);
						this._pendingKrnlChecks.Remove(id);
					}
					bool flag4;
					if (this._isInitialized)
					{
						GameObject playerListCanvas = this._playerListCanvas;
						flag4 = (playerListCanvas != null && playerListCanvas.activeInHierarchy);
					}
					else
					{
						flag4 = false;
					}
					bool flag5 = flag4;
					if (flag5)
					{
						this.UpdatePlayerList();
					}
				}, "OnPlayerLeft");
			}
		}

		// Token: 0x060004E0 RID: 1248 RVA: 0x0001C3FC File Offset: 0x0001A5FC
		public override void OnMenuOpened()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					bool flag = !this._isInitialized && !this._isInitializing;
					if (flag)
					{
						this.StartInitialization();
					}
					else
					{
						bool flag2 = this._isInitialized && this._playerListCanvas != null && PlayerList._playerListVisible;
						if (flag2)
						{
							this._playerListCanvas.SetActive(true);
							this.UpdatePlayerList();
						}
						else
						{
							bool flag3 = this._isInitialized && this._playerListCanvas == null && PlayerList._playerListVisible;
							if (flag3)
							{
								this.RecreateCanvas();
							}
						}
					}
				}, "OnMenuOpened");
			}
		}

		// Token: 0x060004E1 RID: 1249 RVA: 0x000053C4 File Offset: 0x000035C4
		public override void OnMenuClosed()
		{
		}

		// Token: 0x060004E2 RID: 1250 RVA: 0x0001C430 File Offset: 0x0001A630
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					this.Log(string.Format("Scene loaded: {0} (Index: {1})", sceneName, buildIndex));
					object stateLock = this._stateLock;
					lock (stateLock)
					{
						this._worldTransitioning = true;
					}
					MelonCoroutines.Start(this.HandleSceneTransition(sceneName));
				}, "OnSceneWasLoaded");
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x0001C47F File Offset: 0x0001A67F
		private IEnumerator HandleSceneTransition(string sceneName)
		{
			yield return new WaitForSeconds(3f);
			object obj = this._stateLock;
			lock (obj)
			{
				this._worldTransitioning = false;
			}
			obj = null;
			this.Log("Scene transition completed for " + sceneName);
			bool flag2;
			if (this._isInitialized)
			{
				GameObject playerListCanvas = this._playerListCanvas;
				flag2 = (playerListCanvas != null && playerListCanvas.activeInHierarchy);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag3)
			{
				this.UpdatePlayerList();
			}
			yield break;
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0001C498 File Offset: 0x0001A698
		private void StartInitialization()
		{
			bool flag = this._disposed || this._isInitializing || this._isInitialized;
			if (!flag)
			{
				object stateLock = this._stateLock;
				lock (stateLock)
				{
					bool flag3 = this._isInitializing || this._isInitialized;
					if (flag3)
					{
						return;
					}
					this._isInitializing = true;
				}
				this._initAttempts++;
				this.Log(string.Format("Starting PlayerList initialization (Attempt {0}/{1})", this._initAttempts, 15));
				MelonCoroutines.Start(this.InitializationCoroutine());
			}
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x0001C560 File Offset: 0x0001A760
		private IEnumerator InitializationCoroutine()
		{
			yield return new WaitForSeconds(0.5f);
			bool success = false;
			this.SafeExecute(delegate
			{
				success = this.PerformInitialization();
			}, "PerformInitialization");
			bool success2 = success;
			if (success2)
			{
				object obj = this._stateLock;
				lock (obj)
				{
					this._isInitialized = true;
					this._isInitializing = false;
				}
				obj = null;
				this.Log("PlayerList initialized successfully!");
				bool flag2 = this._playerListCanvas != null;
				if (flag2)
				{
					this._playerListCanvas.SetActive(PlayerList._playerListVisible);
					bool playerListVisible = PlayerList._playerListVisible;
					if (playerListVisible)
					{
						this.UpdatePlayerList();
						bool networkConnected = this._networkConnected;
						if (networkConnected)
						{
							this.StartInitialScan();
						}
					}
				}
			}
			else
			{
				this._isInitializing = false;
				bool flag3 = this._initAttempts < 15;
				if (flag3)
				{
					this._nextRetryTime = Time.time + 3f;
					this.Log(string.Format("Initialization failed, will retry in {0} seconds", 3f));
				}
				else
				{
					this.LogError(string.Format("Failed to initialize after {0} attempts", 15));
				}
			}
			yield break;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x0001C570 File Offset: 0x0001A770
		private bool PerformInitialization()
		{
			bool result;
			try
			{
				bool flag = !this.FindUIParent();
				if (flag)
				{
					this.LogError("Could not find UI parent");
					result = false;
				}
				else
				{
					bool flag2 = !this.CreatePlayerListUI();
					if (flag2)
					{
						this.LogError("Could not create player list UI");
						result = false;
					}
					else
					{
						this.CreateToggleButton();
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				this.LogError("Initialization error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060004E7 RID: 1255 RVA: 0x0001C5F4 File Offset: 0x0001A7F4
		private bool FindUIParent()
		{
			string[] array = new string[]
			{
				"UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/Wing_Left/Button",
				"UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/Wing_Left",
				"UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window",
				"UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container",
				"UserInterface/Canvas_QuickMenu(Clone)"
			};
			foreach (string text in array)
			{
				GameObject gameObject = GameObject.Find(text);
				bool flag = gameObject != null;
				if (flag)
				{
					this._menuParent = gameObject.transform;
					this.Log("Found UI parent: " + gameObject.name);
					return true;
				}
			}
			GameObject[] array3 = Enumerable.ToArray<GameObject>(Enumerable.Where<GameObject>(Resources.FindObjectsOfTypeAll<GameObject>(), (GameObject go) => go.name.Contains("QuickMenu") && go.activeInHierarchy));
			bool flag2 = array3.Length != 0;
			if (flag2)
			{
				this._menuParent = array3[0].transform;
				this.Log("Found fallback UI parent: " + this._menuParent.name);
				return true;
			}
			return false;
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		private bool CreatePlayerListUI()
		{
			bool flag = this._playerListCanvas != null;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this._menuParent == null;
				if (flag2)
				{
					result = false;
				}
				else
				{
					try
					{
						this._playerListCanvas = new GameObject("KernelVRC_PlayerList");
						this._playerListCanvas.transform.SetParent(this._menuParent, false);
						Object.DontDestroyOnLoad(this._playerListCanvas);
						RectTransform rectTransform = this._playerListCanvas.AddComponent<RectTransform>();
						rectTransform.sizeDelta = new Vector2(750f, 1200f);
						rectTransform.anchoredPosition = new Vector2(-450f, 0f);
						rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
						rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
						rectTransform.pivot = new Vector2(0.5f, 0.5f);
						rectTransform.localScale = Vector3.one;
						Image image = this._playerListCanvas.AddComponent<Image>();
						try
						{
							image.sprite = ClassicEmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.PLayer_List.png");
						}
						catch
						{
							image.color = new Color(0.1f, 0.1f, 0.15f, 0.95f);
						}
						bool flag3 = image.sprite != null;
						if (flag3)
						{
							image.color = new Color(0.8f, 0.8f, 0.8f, 1f);
						}
						this.CreatePlayerListContent();
						this.Log("Player list UI created successfully with increased size");
						result = true;
					}
					catch (Exception ex)
					{
						this.LogError("Error creating player list UI: " + ex.Message);
						bool flag4 = this._playerListCanvas != null;
						if (flag4)
						{
							Object.Destroy(this._playerListCanvas);
							this._playerListCanvas = null;
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x0001C908 File Offset: 0x0001AB08
		private void CreatePlayerListContent()
		{
			GameObject gameObject = new GameObject("Title");
			gameObject.transform.SetParent(this._playerListCanvas.transform, false);
			this._titleText = gameObject.AddComponent<TextMeshProUGUI>();
			this._titleText.text = "★━━【<color=#FFD700>PLAYER LIST</color>】━━★";
			this._titleText.font = Enumerable.FirstOrDefault<TMP_FontAsset>(Enumerable.Where<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>(), (TMP_FontAsset f) => f != null));
			this._titleText.fontSize = 24f;
			this._titleText.color = Color.white;
			this._titleText.alignment = 514;
			this._titleText.fontStyle = 1;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMin = new Vector2(10f, -50f);
			component.offsetMax = new Vector2(-10f, -10f);
			GameObject gameObject2 = new GameObject("ScrollView");
			gameObject2.transform.SetParent(this._playerListCanvas.transform, false);
			RectTransform rectTransform = gameObject2.AddComponent<RectTransform>();
			rectTransform.anchorMin = new Vector2(0f, 0f);
			rectTransform.anchorMax = new Vector2(1f, 1f);
			rectTransform.offsetMin = new Vector2(15f, 15f);
			rectTransform.offsetMax = new Vector2(-15f, -60f);
			this._scrollRect = gameObject2.AddComponent<ScrollRect>();
			this._scrollRect.horizontal = false;
			this._scrollRect.vertical = true;
			this._scrollRect.scrollSensitivity = 20f;
			this._scrollRect.movementType = 1;
			GameObject gameObject3 = new GameObject("Viewport");
			gameObject3.transform.SetParent(gameObject2.transform, false);
			RectTransform rectTransform2 = gameObject3.AddComponent<RectTransform>();
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = Vector2.zero;
			rectTransform2.offsetMax = new Vector2(-20f, 0f);
			gameObject3.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0.3f);
			gameObject3.AddComponent<Mask>().showMaskGraphic = false;
			GameObject gameObject4 = new GameObject("Content");
			gameObject4.transform.SetParent(gameObject3.transform, false);
			RectTransform rectTransform3 = gameObject4.AddComponent<RectTransform>();
			rectTransform3.anchorMin = new Vector2(0f, 1f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.pivot = new Vector2(0.5f, 1f);
			rectTransform3.sizeDelta = new Vector2(0f, 0f);
			ContentSizeFitter contentSizeFitter = gameObject4.AddComponent<ContentSizeFitter>();
			contentSizeFitter.verticalFit = 2;
			GameObject gameObject5 = new GameObject("PlayerListText");
			gameObject5.transform.SetParent(gameObject4.transform, false);
			this._playerListText = gameObject5.AddComponent<TextMeshProUGUI>();
			this._playerListText.font = Enumerable.FirstOrDefault<TMP_FontAsset>(Enumerable.Where<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>(), (TMP_FontAsset f) => f != null));
			this._playerListText.fontSize = 19f;
			this._playerListText.color = Color.white;
			this._playerListText.alignment = 257;
			this._playerListText.fontStyle = 0;
			this._playerListText.enableWordWrapping = true;
			this._playerListText.overflowMode = 0;
			this._playerListText.margin = new Vector4(12f, 12f, 12f, 12f);
			RectTransform component2 = gameObject5.GetComponent<RectTransform>();
			component2.anchorMin = Vector2.zero;
			component2.anchorMax = Vector2.one;
			component2.offsetMin = Vector2.zero;
			component2.offsetMax = Vector2.zero;
			this._scrollRect.content = rectTransform3;
			this._scrollRect.viewport = rectTransform2;
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001CD68 File Offset: 0x0001AF68
		private void CreateToggleButton()
		{
			bool buttonCreated = this._buttonCreated;
			if (!buttonCreated)
			{
				this.SafeExecute(delegate
				{
					GameObject gameObject = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/Header_H1");
					bool flag = gameObject != null;
					if (flag)
					{
						Transform transform = gameObject.transform.Find("LeftItemContainer");
						bool flag2 = transform != null;
						if (flag2)
						{
							this.CreatePlayerListButton(transform);
							return;
						}
					}
					this.CreateFallbackButton();
				}, "CreateToggleButton");
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x0001CD9C File Offset: 0x0001AF9C
		private void CreatePlayerListButton(Transform parent)
		{
			try
			{
				GameObject gameObject = new GameObject("PlayerList_Toggle");
				gameObject.transform.SetParent(parent, false);
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.sizeDelta = new Vector2(40f, 40f);
				Image image = gameObject.AddComponent<Image>();
				try
				{
					image.sprite = ClassicEmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.PlayerListIcon.png");
				}
				catch
				{
					image.color = new Color(0.3f, 0.6f, 1f, 0.8f);
				}
				Button button = gameObject.AddComponent<Button>();
				button.onClick.AddListener(delegate()
				{
					this.TogglePlayerList();
				});
				try
				{
					VRCButtonHandle vrcbuttonHandle = gameObject.AddComponent<VRCButtonHandle>();
					ToolTip toolTip = gameObject.AddComponent<ToolTip>();
					bool flag = toolTip != null;
					if (flag)
					{
						toolTip._localizableString = LocalizableStringExtensions.Localize("Toggle Player List", null, null, null);
					}
				}
				catch
				{
				}
				gameObject.transform.SetSiblingIndex(0);
				this._buttonCreated = true;
				this.Log("Player list toggle button created");
			}
			catch (Exception ex)
			{
				this.LogError("Error creating player list button: " + ex.Message);
			}
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001CEF0 File Offset: 0x0001B0F0
		private void CreateFallbackButton()
		{
			this.Log("Using fallback button creation method");
			this._buttonCreated = true;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x0001CF08 File Offset: 0x0001B108
		private void InitializeNetworkIntegration()
		{
			try
			{
				KernellNetworkIntegration.Initialize();
				bool flag = KernellNetworkClient.Instance != null;
				if (flag)
				{
					this._networkCallbackId = KernellNetworkClient.Instance.RegisterUserCheckCallback(new Action<string, bool>(this.OnUserCheckResult));
				}
				this.Log("Network integration initialized");
			}
			catch (Exception ex)
			{
				this.LogError("Failed to initialize network integration: " + ex.Message);
			}
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x0001CF84 File Offset: 0x0001B184
		private void CheckNetworkStatus()
		{
			try
			{
				bool networkConnected = this._networkConnected;
				this._networkConnected = KernellNetworkIntegration.IsConnected();
				bool flag = networkConnected != this._networkConnected;
				if (flag)
				{
					this.Log(string.Format("Network connection changed: {0}", this._networkConnected));
					bool networkConnected2 = this._networkConnected;
					if (networkConnected2)
					{
						this.StartInitialScan();
					}
					else
					{
						this.ClearAllKrnlUserStatus();
						object updateLock = this._updateLock;
						lock (updateLock)
						{
							this._hasInitialScanStarted = false;
							this._pendingKrnlChecks.Clear();
							this._krnlCheckQueue.Clear();
							this._isScanning = false;
						}
					}
				}
			}
			catch (Exception ex)
			{
				this.LogError("Error checking network status: " + ex.Message);
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x0001D07C File Offset: 0x0001B27C
		private void StartInitialScan()
		{
			bool flag = !this._networkConnected || this._hasInitialScanStarted;
			if (!flag)
			{
				this.SafeExecute(delegate
				{
					object updateLock = this._updateLock;
					lock (updateLock)
					{
						bool hasInitialScanStarted = this._hasInitialScanStarted;
						if (hasInitialScanStarted)
						{
							return;
						}
						this._hasInitialScanStarted = true;
						this._isScanning = true;
					}
					this.Log("Starting initial KRNL user scan");
					PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
					bool flag3 = ((playerManager != null) ? playerManager.field_Private_List_1_Player_0 : null) != null;
					if (flag3)
					{
						Il2CppArrayBase<Player> il2CppArrayBase = playerManager.field_Private_List_1_Player_0.ToArray();
						foreach (Player player in il2CppArrayBase)
						{
							object obj;
							if (player == null)
							{
								obj = null;
							}
							else
							{
								APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
								obj = ((field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null);
							}
							bool flag4 = obj != null;
							if (flag4)
							{
								this.QueueKrnlCheck(player.field_Private_APIUser_0.id);
							}
						}
						this.Log(string.Format("Queued {0} players for KRNL checking", il2CppArrayBase.Length));
					}
				}, "StartInitialScan");
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x0001D0BC File Offset: 0x0001B2BC
		private void QueueKrnlCheck(string userId)
		{
			bool flag = string.IsNullOrEmpty(userId) || !this._networkConnected;
			if (!flag)
			{
				object updateLock = this._updateLock;
				lock (updateLock)
				{
					bool flag3 = this._krnlUserCache.ContainsKey(userId);
					if (flag3)
					{
						PlayerList.KrnlUserInfo krnlUserInfo = this._krnlUserCache[userId];
						bool flag4 = Time.time - krnlUserInfo.LastChecked < 120f && !krnlUserInfo.CheckInProgress;
						if (flag4)
						{
							return;
						}
					}
					bool flag5 = this._pendingKrnlChecks.Contains(userId);
					if (!flag5)
					{
						this._pendingKrnlChecks.Add(userId);
						this._krnlCheckQueue.Enqueue(userId);
					}
				}
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x0001D190 File Offset: 0x0001B390
		private void ProcessKrnlCheckQueue()
		{
			bool flag = !this._networkConnected || this._krnlCheckQueue.Count == 0;
			if (!flag)
			{
				object updateLock = this._updateLock;
				lock (updateLock)
				{
					bool flag3 = this._krnlCheckQueue.Count == 0;
					if (!flag3)
					{
						string text = this._krnlCheckQueue.Dequeue();
						this._pendingKrnlChecks.Remove(text);
						bool flag4 = !this._krnlUserCache.ContainsKey(text);
						if (flag4)
						{
							this._krnlUserCache[text] = new PlayerList.KrnlUserInfo
							{
								IsKrnlUser = false,
								LastChecked = 0f,
								CheckInProgress = true,
								DisplayName = this.GetDisplayNameForUserId(text)
							};
						}
						else
						{
							this._krnlUserCache[text].CheckInProgress = true;
						}
						bool flag5 = KernellNetworkClient.Instance != null;
						if (flag5)
						{
							KernellNetworkClient.Instance.CheckUserAsync(text);
						}
					}
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001D2AC File Offset: 0x0001B4AC
		private void OnUserCheckResult(string userId, bool isKrnlUser)
		{
			try
			{
				object updateLock = this._updateLock;
				lock (updateLock)
				{
					this._krnlUserCache[userId] = new PlayerList.KrnlUserInfo
					{
						IsKrnlUser = isKrnlUser,
						LastChecked = Time.time,
						CheckInProgress = false,
						DisplayName = this.GetDisplayNameForUserId(userId)
					};
					bool flag2 = this._playerCache.ContainsKey(userId);
					if (flag2)
					{
						bool isKrnlUser2 = this._playerCache[userId].IsKrnlUser;
						this._playerCache[userId].IsKrnlUser = isKrnlUser;
						this._playerCache[userId].LastKrnlCheck = Time.time;
						this._playerCache[userId].KrnlCheckInProgress = false;
						bool flag3 = isKrnlUser2 != isKrnlUser;
						if (flag3)
						{
							this.Log(string.Format("KRNL status changed for {0}: {1} -> {2}", this.GetDisplayNameForUserId(userId), isKrnlUser2, isKrnlUser));
						}
					}
				}
				this.UpdateKrnlUserCount();
				bool flag4 = false;
				object updateLock2 = this._updateLock;
				lock (updateLock2)
				{
					bool flag6 = this._isScanning && this._pendingKrnlChecks.Count == 0 && this._krnlCheckQueue.Count == 0;
					if (flag6)
					{
						this._isScanning = false;
						flag4 = true;
					}
				}
				bool flag7 = flag4;
				if (flag7)
				{
					this.Log("Initial KRNL user scan completed");
				}
			}
			catch (Exception ex)
			{
				this.LogError("Error processing user check result: " + ex.Message);
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001D49C File Offset: 0x0001B69C
		private void UpdateKrnlUserCount()
		{
			object updateLock = this._updateLock;
			lock (updateLock)
			{
				this._totalKrnlUsers = Enumerable.Count<PlayerList.KrnlUserInfo>(this._krnlUserCache.Values, (PlayerList.KrnlUserInfo info) => info.IsKrnlUser);
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001D510 File Offset: 0x0001B710
		private void ClearAllKrnlUserStatus()
		{
			this.SafeExecute(delegate
			{
				object updateLock = this._updateLock;
				lock (updateLock)
				{
					foreach (PlayerList.PlayerInfo playerInfo in this._playerCache.Values)
					{
						playerInfo.IsKrnlUser = false;
						playerInfo.KrnlCheckInProgress = false;
					}
					this._totalKrnlUsers = 0;
				}
				this.Log("Cleared all KRNL user status due to network disconnection");
			}, "ClearAllKrnlUserStatus");
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001D52C File Offset: 0x0001B72C
		private void UpdatePlayerList()
		{
			bool flag = this._disposed || !this._isInitialized || this._isUpdating;
			if (!flag)
			{
				object stateLock = this._stateLock;
				lock (stateLock)
				{
					bool worldTransitioning = this._worldTransitioning;
					if (worldTransitioning)
					{
						return;
					}
				}
				object updateLock = this._updateLock;
				lock (updateLock)
				{
					bool isUpdating = this._isUpdating;
					if (isUpdating)
					{
						return;
					}
					this._isUpdating = true;
				}
				MelonCoroutines.Start(this.UpdatePlayerListCoroutine());
			}
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001D5F8 File Offset: 0x0001B7F8
		private IEnumerator UpdatePlayerListCoroutine()
		{
			try
			{
				try
				{
					PlayerList.<>c__DisplayClass86_0 CS$<>8__locals1 = new PlayerList.<>c__DisplayClass86_0();
					CS$<>8__locals1.<>4__this = this;
					bool flag = this._playerListText == null;
					if (flag)
					{
						yield break;
					}
					object obj = this._stateLock;
					lock (obj)
					{
						bool worldTransitioning = this._worldTransitioning;
						if (worldTransitioning)
						{
							yield break;
						}
					}
					obj = null;
					Dictionary<string, PlayerList.PlayerInfo> allPlayers = this.GetCurrentPlayers();
					CS$<>8__locals1.displayData = this.BuildDisplayData(allPlayers);
					this.SafeExecute(delegate
					{
						bool flag4 = CS$<>8__locals1.<>4__this._disposed || CS$<>8__locals1.<>4__this._titleText == null || CS$<>8__locals1.<>4__this._playerListText == null;
						if (!flag4)
						{
							CS$<>8__locals1.<>4__this._titleText.text = CS$<>8__locals1.displayData.TitleText;
							CS$<>8__locals1.<>4__this._playerListText.text = CS$<>8__locals1.displayData.PlayerListText;
							CS$<>8__locals1.<>4__this._playerListText.fontSize = CS$<>8__locals1.displayData.FontSize;
							ScrollRect scrollRect = CS$<>8__locals1.<>4__this._scrollRect;
							bool flag5 = ((scrollRect != null) ? scrollRect.content : null) != null;
							if (flag5)
							{
								Canvas.ForceUpdateCanvases();
								LayoutRebuilder.ForceRebuildLayoutImmediate(CS$<>8__locals1.<>4__this._scrollRect.content);
							}
						}
					}, "UpdateUI");
					this._consecutiveErrors = 0;
					CS$<>8__locals1 = null;
					allPlayers = null;
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					this._consecutiveErrors++;
					this.LogError(string.Format("UpdatePlayerListCoroutine error #{0}: {1}", this._consecutiveErrors, ex.Message));
					bool flag3 = this._consecutiveErrors >= 5;
					if (flag3)
					{
						this.LogError("Too many consecutive update errors, entering recovery mode");
						this._lastErrorTime = Time.time;
					}
				}
				yield break;
			}
			finally
			{
				this._isUpdating = false;
			}
			yield break;
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001D608 File Offset: 0x0001B808
		private Dictionary<string, PlayerList.PlayerInfo> GetCurrentPlayers()
		{
			Dictionary<string, PlayerList.PlayerInfo> allPlayers = new Dictionary<string, PlayerList.PlayerInfo>();
			this.SafeExecute(delegate
			{
				PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
				bool flag = ((playerManager != null) ? playerManager.field_Private_List_1_Player_0 : null) == null;
				if (!flag)
				{
					Il2CppArrayBase<Player> il2CppArrayBase = playerManager.field_Private_List_1_Player_0.ToArray();
					float time = Time.time;
					HashSet<string> currentPlayerIds = new HashSet<string>();
					foreach (Player player in il2CppArrayBase)
					{
						bool disposed = this._disposed;
						if (disposed)
						{
							break;
						}
						try
						{
							object obj;
							if (player == null)
							{
								obj = null;
							}
							else
							{
								APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
								obj = ((field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null);
							}
							bool flag2 = obj == null;
							if (!flag2)
							{
								string id2 = player.field_Private_APIUser_0.id;
								string displayName = player.field_Private_APIUser_0.displayName ?? "Unknown";
								currentPlayerIds.Add(id2);
								bool isKrnlUser = this.IsUserKrnlCached(id2);
								bool krnlCheckInProgress = false;
								bool flag3 = this._krnlUserCache.ContainsKey(id2);
								if (flag3)
								{
									krnlCheckInProgress = this._krnlUserCache[id2].CheckInProgress;
								}
								bool flag4 = this._networkConnected && this.ShouldCheckKrnlStatus(id2);
								if (flag4)
								{
									this.QueueKrnlCheck(id2);
								}
								PlayerList.PlayerInfo value = new PlayerList.PlayerInfo
								{
									UserId = id2,
									DisplayName = displayName,
									TrustRank = PlayerList.GetPlayerRank(player.field_Private_APIUser_0),
									PlatformIcon = this.GetPlatformIcon(player),
									AgeVerified = this.GetAgeVerified(player),
									IsLocal = true,
									IsKrnlUser = isKrnlUser,
									LastUpdate = time,
									LastKrnlCheck = (this._krnlUserCache.ContainsKey(id2) ? this._krnlUserCache[id2].LastChecked : 0f),
									KrnlCheckInProgress = krnlCheckInProgress
								};
								allPlayers[id2] = value;
								this._playerCache[id2] = value;
							}
						}
						catch (Exception ex)
						{
							this.LogError("Error processing player: " + ex.Message);
						}
					}
					this._activePlayerIds.Clear();
					this._activePlayerIds.UnionWith(currentPlayerIds);
					List<string> list = Enumerable.ToList<string>(Enumerable.Where<string>(this._playerCache.Keys, (string id) => !currentPlayerIds.Contains(id)));
					foreach (string key in list)
					{
						this._playerCache.Remove(key);
					}
				}
			}, "GetCurrentPlayers");
			return allPlayers;
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001D650 File Offset: 0x0001B850
		private bool ShouldCheckKrnlStatus(string userId)
		{
			bool flag = !this._krnlUserCache.ContainsKey(userId);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				PlayerList.KrnlUserInfo krnlUserInfo = this._krnlUserCache[userId];
				result = (!krnlUserInfo.CheckInProgress && Time.time - krnlUserInfo.LastChecked > 120f);
			}
			return result;
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001D6A4 File Offset: 0x0001B8A4
		private bool IsUserKrnlCached(string userId)
		{
			return this._krnlUserCache.ContainsKey(userId) && this._krnlUserCache[userId].IsKrnlUser;
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001D6D8 File Offset: 0x0001B8D8
		private string GetDisplayNameForUserId(string userId)
		{
			bool flag = this._playerCache.ContainsKey(userId);
			string result;
			if (flag)
			{
				result = this._playerCache[userId].DisplayName;
			}
			else
			{
				PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
				bool flag2 = ((playerManager != null) ? playerManager.field_Private_List_1_Player_0 : null) != null;
				if (flag2)
				{
					Player player = null;
					foreach (Player player2 in playerManager.field_Private_List_1_Player_0)
					{
						string a;
						if (player2 == null)
						{
							a = null;
						}
						else
						{
							APIUser field_Private_APIUser_ = player2.field_Private_APIUser_0;
							a = ((field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null);
						}
						bool flag3 = a == userId;
						if (flag3)
						{
							player = player2;
							break;
						}
					}
					object obj;
					if (player == null)
					{
						obj = null;
					}
					else
					{
						APIUser field_Private_APIUser_2 = player.field_Private_APIUser_0;
						obj = ((field_Private_APIUser_2 != null) ? field_Private_APIUser_2.displayName : null);
					}
					bool flag4 = obj != null;
					if (flag4)
					{
						return player.field_Private_APIUser_0.displayName;
					}
				}
				result = "Unknown";
			}
			return result;
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001D7BC File Offset: 0x0001B9BC
		private PlayerList.PlayerListDisplayData BuildDisplayData(Dictionary<string, PlayerList.PlayerInfo> allPlayers)
		{
			PlayerList.PlayerListDisplayData result;
			try
			{
				int count = allPlayers.Count;
				int num = Enumerable.Count<PlayerList.PlayerInfo>(allPlayers.Values, (PlayerList.PlayerInfo p) => p.IsLocal);
				int num2 = Enumerable.Count<PlayerList.PlayerInfo>(allPlayers.Values, (PlayerList.PlayerInfo p) => p.IsKrnlUser);
				float fontSize = Mathf.Clamp(24f - (float)count * 0.08f, 16f, 24f);
				string titleText = string.Format("★━【<color=#FFD700>Players({0})</color>|<color=#8143E6>KRNL({1})</color>】━★", num, num2);
				string playerListText = this.BuildPlayerListText(allPlayers);
				result = new PlayerList.PlayerListDisplayData
				{
					TitleText = titleText,
					PlayerListText = playerListText,
					FontSize = fontSize,
					PlayerCount = count
				};
			}
			catch (Exception ex)
			{
				this.LogError("BuildDisplayData error: " + ex.Message);
				result = new PlayerList.PlayerListDisplayData
				{
					TitleText = "★━【<color=#FFD700>Players</color>|<color=#FF0000>ERROR</color>】━★",
					PlayerListText = "<color=#FF0000>Error loading player data: " + ex.Message + "</color>",
					FontSize = 18f,
					PlayerCount = 0
				};
			}
			return result;
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001D908 File Offset: 0x0001BB08
		private string BuildPlayerListText(Dictionary<string, PlayerList.PlayerInfo> allPlayers)
		{
			string result;
			try
			{
				bool flag = allPlayers.Count == 0;
				if (flag)
				{
					result = "<color=#888888>No players detected...</color>";
				}
				else
				{
					List<PlayerList.PlayerInfo> list = Enumerable.ToList<PlayerList.PlayerInfo>(Enumerable.Take<PlayerList.PlayerInfo>(Enumerable.ThenBy<PlayerList.PlayerInfo, string>(Enumerable.ThenByDescending<PlayerList.PlayerInfo, int>(Enumerable.OrderByDescending<PlayerList.PlayerInfo, bool>(allPlayers.Values, (PlayerList.PlayerInfo p) => p.IsKrnlUser), (PlayerList.PlayerInfo p) => (int)p.TrustRank), (PlayerList.PlayerInfo p) => p.DisplayName), 80));
					StringBuilder stringBuilder = new StringBuilder();
					foreach (PlayerList.PlayerInfo playerInfo in list)
					{
						try
						{
							string colorForRank = this.GetColorForRank(playerInfo.TrustRank);
							string rankText = this.GetRankText(playerInfo.TrustRank);
							string text = "";
							bool isKrnlUser = playerInfo.IsKrnlUser;
							if (isKrnlUser)
							{
								text = " <color=#8143E6><b>[KRNL]</b></color>";
							}
							else
							{
								bool krnlCheckInProgress = playerInfo.KrnlCheckInProgress;
								if (krnlCheckInProgress)
								{
									text = " <color=#FFA500><b>[?]</b></color>";
								}
							}
							stringBuilder.AppendLine(string.Concat(new string[]
							{
								"<color=",
								colorForRank,
								">[",
								playerInfo.PlatformIcon,
								"][",
								rankText,
								"]",
								playerInfo.AgeVerified,
								" ",
								playerInfo.DisplayName,
								text,
								"</color>"
							}));
						}
						catch (Exception ex)
						{
							this.LogError("Error building player entry: " + ex.Message);
						}
					}
					bool flag2 = allPlayers.Count > 80;
					if (flag2)
					{
						stringBuilder.AppendLine(string.Format("<color=#888888>... and {0} more players</color>", allPlayers.Count - 80));
					}
					bool isScanning = this._isScanning;
					if (isScanning)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("<color=#FFA500>\ud83d\udd0d Scanning for KRNL users...</color>");
					}
					result = stringBuilder.ToString();
				}
			}
			catch (Exception ex2)
			{
				this.LogError("BuildPlayerListText error: " + ex2.Message);
				result = "<color=#FF0000>Error building player list</color>";
			}
			return result;
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001DBA0 File Offset: 0x0001BDA0
		private string GetAgeVerified(Player player)
		{
			try
			{
				bool ageVerified_k__BackingField = player.field_Private_APIUser_0._ageVerified_k__BackingField;
				if (ageVerified_k__BackingField)
				{
					return "<color=red>[18+]</color>";
				}
			}
			catch
			{
			}
			return "";
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001DBE8 File Offset: 0x0001BDE8
		private string GetPlatformIcon(Player player)
		{
			string result;
			try
			{
				bool flag = ((player != null) ? player.Method_Public_get_VRCPlayerApi_0() : null) == null;
				if (flag)
				{
					result = "PC";
				}
				else
				{
					bool flag2 = player.Method_Public_get_VRCPlayerApi_0().IsUserInVR();
					if (flag2)
					{
						result = "VR";
					}
					else
					{
						string text = player.ToString();
						result = ((text.Contains("Android") || text.Contains("IOS")) ? "MOB" : "PC");
					}
				}
			}
			catch
			{
				result = "PC";
			}
			return result;
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001DC74 File Offset: 0x0001BE74
		private static TrustRank GetPlayerRank(APIUser user)
		{
			TrustRank result;
			try
			{
				bool isFriend = user.isFriend;
				if (isFriend)
				{
					result = TrustRank.Friend;
				}
				else
				{
					bool flag = false;
					bool flag2 = false;
					bool flag3 = false;
					bool flag4 = false;
					bool flag5 = false;
					foreach (string text in user.tags)
					{
						string text2 = text;
						string a = text2;
						if (!(a == "system_trust_troll"))
						{
							if (!(a == "system_trust_veteran"))
							{
								if (!(a == "system_trust_trusted"))
								{
									if (!(a == "system_trust_known"))
									{
										if (a == "system_trust_basic")
										{
											flag5 = true;
										}
									}
									else
									{
										flag4 = true;
									}
								}
								else
								{
									flag3 = true;
								}
							}
							else
							{
								flag2 = true;
							}
						}
						else
						{
							flag = true;
						}
					}
					bool flag6 = flag;
					if (flag6)
					{
						result = TrustRank.Troll;
					}
					else
					{
						bool flag7 = flag2;
						if (flag7)
						{
							result = TrustRank.Trusted;
						}
						else
						{
							bool flag8 = flag3;
							if (flag8)
							{
								result = TrustRank.Known;
							}
							else
							{
								bool flag9 = flag4;
								if (flag9)
								{
									result = TrustRank.User;
								}
								else
								{
									bool flag10 = flag5;
									if (flag10)
									{
										result = TrustRank.NewUser;
									}
									else
									{
										result = TrustRank.Visitor;
									}
								}
							}
						}
					}
				}
			}
			catch
			{
				result = TrustRank.Visitor;
			}
			return result;
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001DD88 File Offset: 0x0001BF88
		private string GetColorForRank(TrustRank rank)
		{
			string result;
			switch (rank)
			{
			case TrustRank.Troll:
				result = "#8B0000";
				break;
			case TrustRank.Visitor:
				result = "#CCCCCC";
				break;
			case TrustRank.NewUser:
				result = "#1778FF";
				break;
			case TrustRank.User:
				result = "#2BCF5C";
				break;
			case TrustRank.Known:
				result = "#FF7B42";
				break;
			case TrustRank.Trusted:
				result = "#8143E6";
				break;
			case TrustRank.Friend:
				result = "#FFD700";
				break;
			default:
				result = "#FFFFFF";
				break;
			}
			return result;
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001DE00 File Offset: 0x0001C000
		private string GetRankText(TrustRank rank)
		{
			string result;
			switch (rank)
			{
			case TrustRank.Troll:
				result = "TROLL";
				break;
			case TrustRank.Visitor:
				result = "VIS";
				break;
			case TrustRank.NewUser:
				result = "NEW";
				break;
			case TrustRank.User:
				result = "USR";
				break;
			case TrustRank.Known:
				result = "KNW";
				break;
			case TrustRank.Trusted:
				result = "TRS";
				break;
			case TrustRank.Friend:
				result = "FRD";
				break;
			default:
				result = "UNK";
				break;
			}
			return result;
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001DE78 File Offset: 0x0001C078
		private void EnsureDirectories()
		{
			try
			{
				string directoryName = Path.GetDirectoryName(PlayerList.STATE_FILE_PATH);
				bool flag = !Directory.Exists(directoryName);
				if (flag)
				{
					Directory.CreateDirectory(directoryName);
				}
			}
			catch (Exception ex)
			{
				this.LogError("Error creating directories: " + ex.Message);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001DED8 File Offset: 0x0001C0D8
		private void LoadState()
		{
			try
			{
				bool flag = File.Exists(PlayerList.STATE_FILE_PATH);
				if (flag)
				{
					string text = File.ReadAllText(PlayerList.STATE_FILE_PATH).Trim();
					PlayerList._playerListVisible = text.Equals("true", StringComparison.OrdinalIgnoreCase);
					this.Log(string.Format("Loaded player list state: {0}", PlayerList._playerListVisible));
				}
				else
				{
					this.SaveState();
					this.Log(string.Format("Created default player list state: {0}", PlayerList._playerListVisible));
				}
			}
			catch (Exception ex)
			{
				this.LogError("Error loading state: " + ex.Message);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001DF88 File Offset: 0x0001C188
		private void SaveState()
		{
			try
			{
				File.WriteAllText(PlayerList.STATE_FILE_PATH, PlayerList._playerListVisible.ToString().ToLower());
			}
			catch (Exception ex)
			{
				this.LogError("Error saving state: " + ex.Message);
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001DFE0 File Offset: 0x0001C1E0
		private void TogglePlayerList()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					PlayerList._playerListVisible = !PlayerList._playerListVisible;
					this.SaveState();
					bool flag = this._playerListCanvas != null;
					if (flag)
					{
						this._playerListCanvas.SetActive(PlayerList._playerListVisible);
						bool playerListVisible = PlayerList._playerListVisible;
						if (playerListVisible)
						{
							this.UpdatePlayerList();
							bool flag2 = this._networkConnected && !this._hasInitialScanStarted;
							if (flag2)
							{
								this.StartInitialScan();
							}
						}
					}
					else
					{
						bool isInitialized = this._isInitialized;
						if (isInitialized)
						{
							this.RecreateCanvas();
						}
						else
						{
							this.StartInitialization();
						}
					}
					this.Log("Player list toggled: " + (PlayerList._playerListVisible ? "visible" : "hidden"));
				}, "TogglePlayerList");
			}
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001E014 File Offset: 0x0001C214
		private void RecreateCanvas()
		{
			this.LogError("PlayerList canvas was null, recreating...");
			this.SafeExecute(delegate
			{
				bool flag = this.CreatePlayerListUI();
				if (flag)
				{
					this._playerListCanvas.SetActive(PlayerList._playerListVisible);
					bool playerListVisible = PlayerList._playerListVisible;
					if (playerListVisible)
					{
						this.UpdatePlayerList();
						bool flag2 = this._networkConnected && !this._hasInitialScanStarted;
						if (flag2)
						{
							this.StartInitialScan();
						}
					}
				}
			}, "RecreateCanvas");
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0001E03C File Offset: 0x0001C23C
		private void CheckAndRecoverFromBrokenState()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				bool flag = this._isInitialized && this._playerListCanvas == null && !this._isInitializing;
				if (flag)
				{
					this.LogError("Detected broken PlayerList state. Attempting to recover...");
					object stateLock = this._stateLock;
					lock (stateLock)
					{
						this._isInitialized = false;
						this._initAttempts = 0;
						this._buttonCreated = false;
					}
					this.StartInitialization();
				}
				bool flag3 = this._playerListCanvas != null && this._playerListCanvas.activeSelf != PlayerList._playerListVisible;
				if (flag3)
				{
					this._playerListCanvas.SetActive(PlayerList._playerListVisible);
				}
			}
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0001E124 File Offset: 0x0001C324
		private void SafeExecute(Action action, string operationName)
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				try
				{
					action();
				}
				catch (Exception ex)
				{
					this.LogError("Error in " + operationName + ": " + ex.Message);
				}
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001E180 File Offset: 0x0001C380
		private new void Log(string message)
		{
			try
			{
				kernelllogger.Msg("[PlayerList] " + message);
			}
			catch
			{
			}
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001E1B8 File Offset: 0x0001C3B8
		private new void LogError(string message)
		{
			try
			{
				kernelllogger.Error("[PlayerList] " + message);
			}
			catch
			{
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001E1F0 File Offset: 0x0001C3F0
		public void Dispose()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				object stateLock = this._stateLock;
				lock (stateLock)
				{
					this._disposed = true;
				}
				this.SafeExecute(delegate
				{
					this.Log("Disposing PlayerList");
					try
					{
						bool flag2 = !string.IsNullOrEmpty(this._networkCallbackId) && KernellNetworkClient.Instance != null;
						if (flag2)
						{
							KernellNetworkClient.Instance.UnregisterUserCheckCallback(this._networkCallbackId);
						}
					}
					catch (Exception ex)
					{
						this.LogError("Error unregistering network callback: " + ex.Message);
					}
					bool flag3 = this._playerListCanvas != null;
					if (flag3)
					{
						Object.Destroy(this._playerListCanvas);
						this._playerListCanvas = null;
					}
					object updateLock = this._updateLock;
					lock (updateLock)
					{
						this._playerCache.Clear();
						this._krnlUserCache.Clear();
						this._activePlayerIds.Clear();
						this._pendingKrnlChecks.Clear();
						this._krnlCheckQueue.Clear();
					}
					this._isInitialized = false;
					this._isInitializing = false;
					this._isUpdating = false;
				}, "Dispose");
			}
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001E260 File Offset: 0x0001C460
		public override void OnShutdown()
		{
			this.Dispose();
			base.OnShutdown();
		}

		// Token: 0x04000214 RID: 532
		private volatile bool _disposed = false;

		// Token: 0x04000215 RID: 533
		private volatile bool _isInitialized = false;

		// Token: 0x04000216 RID: 534
		private volatile bool _isInitializing = false;

		// Token: 0x04000217 RID: 535
		private volatile bool _isUpdating = false;

		// Token: 0x04000218 RID: 536
		private volatile bool _worldTransitioning = false;

		// Token: 0x04000219 RID: 537
		private readonly object _stateLock = new object();

		// Token: 0x0400021A RID: 538
		private readonly object _updateLock = new object();

		// Token: 0x0400021B RID: 539
		private int _initAttempts = 0;

		// Token: 0x0400021C RID: 540
		private const int MAX_INIT_ATTEMPTS = 15;

		// Token: 0x0400021D RID: 541
		private float _nextRetryTime = 0f;

		// Token: 0x0400021E RID: 542
		private const float RETRY_DELAY = 3f;

		// Token: 0x0400021F RID: 543
		private float _lastUpdateTime = 0f;

		// Token: 0x04000220 RID: 544
		private const float UPDATE_INTERVAL = 1.5f;

		// Token: 0x04000221 RID: 545
		private float _lastFullUpdate = 0f;

		// Token: 0x04000222 RID: 546
		private const float FULL_UPDATE_INTERVAL = 5f;

		// Token: 0x04000223 RID: 547
		private int _consecutiveErrors = 0;

		// Token: 0x04000224 RID: 548
		private const int MAX_CONSECUTIVE_ERRORS = 5;

		// Token: 0x04000225 RID: 549
		private float _lastErrorTime = 0f;

		// Token: 0x04000226 RID: 550
		private const float ERROR_RECOVERY_TIME = 30f;

		// Token: 0x04000227 RID: 551
		private GameObject _playerListCanvas;

		// Token: 0x04000228 RID: 552
		private Transform _menuParent;

		// Token: 0x04000229 RID: 553
		private TextMeshProUGUI _playerListText;

		// Token: 0x0400022A RID: 554
		private TextMeshProUGUI _titleText;

		// Token: 0x0400022B RID: 555
		private ScrollRect _scrollRect;

		// Token: 0x0400022C RID: 556
		private bool _buttonCreated = false;

		// Token: 0x0400022D RID: 557
		private static bool _playerListVisible = true;

		// Token: 0x0400022E RID: 558
		private static readonly string STATE_FILE_PATH = Path.Combine(Environment.CurrentDirectory, "UserData", "KernellVRCLite", "PlayerListState.txt");

		// Token: 0x0400022F RID: 559
		private readonly Dictionary<string, PlayerList.PlayerInfo> _playerCache = new Dictionary<string, PlayerList.PlayerInfo>();

		// Token: 0x04000230 RID: 560
		private readonly Dictionary<string, PlayerList.KrnlUserInfo> _krnlUserCache = new Dictionary<string, PlayerList.KrnlUserInfo>();

		// Token: 0x04000231 RID: 561
		private readonly HashSet<string> _activePlayerIds = new HashSet<string>();

		// Token: 0x04000232 RID: 562
		private readonly HashSet<string> _pendingKrnlChecks = new HashSet<string>();

		// Token: 0x04000233 RID: 563
		private readonly Queue<string> _krnlCheckQueue = new Queue<string>();

		// Token: 0x04000234 RID: 564
		private bool _networkConnected = false;

		// Token: 0x04000235 RID: 565
		private int _totalKrnlUsers = 0;

		// Token: 0x04000236 RID: 566
		private string _networkCallbackId = null;

		// Token: 0x04000237 RID: 567
		private bool _hasInitialScanStarted = false;

		// Token: 0x04000238 RID: 568
		private bool _isScanning = false;

		// Token: 0x04000239 RID: 569
		private const float KRNL_CACHE_TIME = 120f;

		// Token: 0x0400023A RID: 570
		private const float PLAYER_CACHE_TIME = 10f;

		// Token: 0x0400023B RID: 571
		private float _lastNetworkCheck = 0f;

		// Token: 0x0400023C RID: 572
		private const float NETWORK_CHECK_INTERVAL = 5f;

		// Token: 0x0400023D RID: 573
		private const float KRNL_CHECK_INTERVAL = 0.5f;

		// Token: 0x0400023E RID: 574
		private float _lastKrnlCheckTime = 0f;

		// Token: 0x02000122 RID: 290
		private class PlayerInfo
		{
			// Token: 0x17000206 RID: 518
			// (get) Token: 0x06000B6A RID: 2922 RVA: 0x00043220 File Offset: 0x00041420
			// (set) Token: 0x06000B6B RID: 2923 RVA: 0x00043228 File Offset: 0x00041428
			public string UserId { get; set; }

			// Token: 0x17000207 RID: 519
			// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00043231 File Offset: 0x00041431
			// (set) Token: 0x06000B6D RID: 2925 RVA: 0x00043239 File Offset: 0x00041439
			public string DisplayName { get; set; }

			// Token: 0x17000208 RID: 520
			// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00043242 File Offset: 0x00041442
			// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0004324A File Offset: 0x0004144A
			public TrustRank TrustRank { get; set; }

			// Token: 0x17000209 RID: 521
			// (get) Token: 0x06000B70 RID: 2928 RVA: 0x00043253 File Offset: 0x00041453
			// (set) Token: 0x06000B71 RID: 2929 RVA: 0x0004325B File Offset: 0x0004145B
			public string PlatformIcon { get; set; }

			// Token: 0x1700020A RID: 522
			// (get) Token: 0x06000B72 RID: 2930 RVA: 0x00043264 File Offset: 0x00041464
			// (set) Token: 0x06000B73 RID: 2931 RVA: 0x0004326C File Offset: 0x0004146C
			public string AgeVerified { get; set; }

			// Token: 0x1700020B RID: 523
			// (get) Token: 0x06000B74 RID: 2932 RVA: 0x00043275 File Offset: 0x00041475
			// (set) Token: 0x06000B75 RID: 2933 RVA: 0x0004327D File Offset: 0x0004147D
			public bool IsLocal { get; set; }

			// Token: 0x1700020C RID: 524
			// (get) Token: 0x06000B76 RID: 2934 RVA: 0x00043286 File Offset: 0x00041486
			// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0004328E File Offset: 0x0004148E
			public bool IsKrnlUser { get; set; }

			// Token: 0x1700020D RID: 525
			// (get) Token: 0x06000B78 RID: 2936 RVA: 0x00043297 File Offset: 0x00041497
			// (set) Token: 0x06000B79 RID: 2937 RVA: 0x0004329F File Offset: 0x0004149F
			public float LastUpdate { get; set; }

			// Token: 0x1700020E RID: 526
			// (get) Token: 0x06000B7A RID: 2938 RVA: 0x000432A8 File Offset: 0x000414A8
			// (set) Token: 0x06000B7B RID: 2939 RVA: 0x000432B0 File Offset: 0x000414B0
			public float LastKrnlCheck { get; set; }

			// Token: 0x1700020F RID: 527
			// (get) Token: 0x06000B7C RID: 2940 RVA: 0x000432B9 File Offset: 0x000414B9
			// (set) Token: 0x06000B7D RID: 2941 RVA: 0x000432C1 File Offset: 0x000414C1
			public bool KrnlCheckInProgress { get; set; }
		}

		// Token: 0x02000123 RID: 291
		private class KrnlUserInfo
		{
			// Token: 0x17000210 RID: 528
			// (get) Token: 0x06000B7F RID: 2943 RVA: 0x000432CA File Offset: 0x000414CA
			// (set) Token: 0x06000B80 RID: 2944 RVA: 0x000432D2 File Offset: 0x000414D2
			public bool IsKrnlUser { get; set; }

			// Token: 0x17000211 RID: 529
			// (get) Token: 0x06000B81 RID: 2945 RVA: 0x000432DB File Offset: 0x000414DB
			// (set) Token: 0x06000B82 RID: 2946 RVA: 0x000432E3 File Offset: 0x000414E3
			public float LastChecked { get; set; }

			// Token: 0x17000212 RID: 530
			// (get) Token: 0x06000B83 RID: 2947 RVA: 0x000432EC File Offset: 0x000414EC
			// (set) Token: 0x06000B84 RID: 2948 RVA: 0x000432F4 File Offset: 0x000414F4
			public bool CheckInProgress { get; set; }

			// Token: 0x17000213 RID: 531
			// (get) Token: 0x06000B85 RID: 2949 RVA: 0x000432FD File Offset: 0x000414FD
			// (set) Token: 0x06000B86 RID: 2950 RVA: 0x00043305 File Offset: 0x00041505
			public string DisplayName { get; set; }
		}

		// Token: 0x02000124 RID: 292
		private class PlayerListDisplayData
		{
			// Token: 0x17000214 RID: 532
			// (get) Token: 0x06000B88 RID: 2952 RVA: 0x0004330E File Offset: 0x0004150E
			// (set) Token: 0x06000B89 RID: 2953 RVA: 0x00043316 File Offset: 0x00041516
			public string TitleText { get; set; } = "";

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x06000B8A RID: 2954 RVA: 0x0004331F File Offset: 0x0004151F
			// (set) Token: 0x06000B8B RID: 2955 RVA: 0x00043327 File Offset: 0x00041527
			public string PlayerListText { get; set; } = "";

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00043330 File Offset: 0x00041530
			// (set) Token: 0x06000B8D RID: 2957 RVA: 0x00043338 File Offset: 0x00041538
			public float FontSize { get; set; } = 24f;

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x06000B8E RID: 2958 RVA: 0x00043341 File Offset: 0x00041541
			// (set) Token: 0x06000B8F RID: 2959 RVA: 0x00043349 File Offset: 0x00041549
			public int PlayerCount { get; set; } = 0;
		}
	}
}
