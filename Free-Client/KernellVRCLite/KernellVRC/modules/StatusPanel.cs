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
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;
using VRC.Localization;

namespace KernellVRC.modules
{
	// Token: 0x02000075 RID: 117
	public class StatusPanel : KernelModuleBase
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001E9D8 File Offset: 0x0001CBD8
		public override string ModuleName { get; } = "StatusPanel";

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001E9E0 File Offset: 0x0001CBE0
		public override string Version { get; } = "3.0.0";

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001E9E8 File Offset: 0x0001CBE8
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.WorldEvents | ModuleCapabilities.MenuEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x000036A7 File Offset: 0x000018A7
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every30Frames;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x00003315 File Offset: 0x00001515
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.Normal;
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0001E9F0 File Offset: 0x0001CBF0
		public override void OnUiManagerInit()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					this.EnsureDirectories();
					this.LoadState();
					this.StartInitialization();
				}, "OnUiManagerInit");
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0001EA24 File Offset: 0x0001CC24
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
					bool flag2 = this._isInitialized && this._statusPanelCanvas != null && this._statusPanelCanvas.activeSelf;
					if (flag2)
					{
						bool flag3 = Time.time - this._lastUpdateTime >= 2f;
						if (flag3)
						{
							this.UpdateStatusDisplay();
							this._lastUpdateTime = Time.time;
						}
					}
				}
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0001EAD4 File Offset: 0x0001CCD4
		public override void OnEnterWorld(ApiWorld world, ApiWorldInstance instance)
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					StatusPanel <>4__this = this;
					ApiWorld world2 = world;
					<>4__this._cachedWorldId = (((world2 != null) ? world2.id : null) ?? "Unknown");
					this._needsFullUpdate = true;
				}, "OnEnterWorld");
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0001EB1C File Offset: 0x0001CD1C
		public override void OnLeaveWorld()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					this._cachedWorldId = "Transitioning...";
					this._cachedPlayerCount = 0;
					this._needsFullUpdate = true;
				}, "OnLeaveWorld");
			}
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0001EB50 File Offset: 0x0001CD50
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
						bool flag2 = this._isInitialized && this._statusPanelCanvas != null && StatusPanel._panelVisible;
						if (flag2)
						{
							this._statusPanelCanvas.SetActive(true);
							this._needsFullUpdate = true;
						}
					}
				}, "OnMenuOpened");
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x000053C4 File Offset: 0x000035C4
		public override void OnMenuClosed()
		{
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0001EB84 File Offset: 0x0001CD84
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
				base.Log(string.Format("Starting StatusPanel initialization (Attempt {0}/{1})", this._initAttempts, 20));
				MelonCoroutines.Start(this.InitializationCoroutine());
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001EC4C File Offset: 0x0001CE4C
		private IEnumerator InitializationCoroutine()
		{
			yield return new WaitForSeconds(0.1f);
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
				base.Log("StatusPanel initialized successfully!");
				bool flag2 = this._statusPanelCanvas != null;
				if (flag2)
				{
					this._statusPanelCanvas.SetActive(StatusPanel._panelVisible);
					bool panelVisible = StatusPanel._panelVisible;
					if (panelVisible)
					{
						this._needsFullUpdate = true;
					}
				}
				this.SubscribeToNetworkEvents();
			}
			else
			{
				this._isInitializing = false;
				bool flag3 = this._initAttempts < 20;
				if (flag3)
				{
					this._nextRetryTime = Time.time + 3f;
					base.Log(string.Format("Initialization failed, will retry in {0} seconds", 3f));
				}
				else
				{
					base.LogError(string.Format("Failed to initialize after {0} attempts", 20));
				}
			}
			yield break;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001EC5C File Offset: 0x0001CE5C
		private bool PerformInitialization()
		{
			bool result;
			try
			{
				bool flag = !this.FindUIParent();
				if (flag)
				{
					base.LogError("Could not find UI parent");
					result = false;
				}
				else
				{
					bool flag2 = !this.CreateStatusPanelUI();
					if (flag2)
					{
						base.LogError("Could not create status panel UI");
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
				base.LogError("Initialization error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001ECE0 File Offset: 0x0001CEE0
		private bool FindUIParent()
		{
			string[] array = new string[]
			{
				"UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/Wing_Right/Button",
				"UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/Wing_Right",
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
					base.Log("Found UI parent: " + gameObject.name);
					return true;
				}
			}
			GameObject[] array3 = Enumerable.ToArray<GameObject>(Enumerable.Where<GameObject>(Resources.FindObjectsOfTypeAll<GameObject>(), (GameObject go) => go.name.Contains("QuickMenu") && go.activeInHierarchy));
			bool flag2 = array3.Length != 0;
			if (flag2)
			{
				this._menuParent = array3[0].transform;
				base.Log("Found fallback UI parent: " + this._menuParent.name);
				return true;
			}
			return false;
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001EDE8 File Offset: 0x0001CFE8
		private bool CreateStatusPanelUI()
		{
			bool flag = this._statusPanelCanvas != null;
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
						this._statusPanelCanvas = new GameObject("KernelVRC_StatusPanel");
						this._statusPanelCanvas.transform.SetParent(this._menuParent, false);
						Object.DontDestroyOnLoad(this._statusPanelCanvas);
						RectTransform rectTransform = this._statusPanelCanvas.AddComponent<RectTransform>();
						rectTransform.sizeDelta = new Vector2(400f, 500f);
						rectTransform.anchoredPosition = new Vector2(350f, 0f);
						rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
						rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
						rectTransform.pivot = new Vector2(0.5f, 0.5f);
						rectTransform.localScale = Vector3.one;
						Image image = this._statusPanelCanvas.AddComponent<Image>();
						image.color = new Color(0.1f, 0.1f, 0.15f, 0.95f);
						this.CreateStatusContent();
						base.Log("Status panel UI created successfully");
						result = true;
					}
					catch (Exception ex)
					{
						base.LogError("Error creating status panel UI: " + ex.Message);
						bool flag3 = this._statusPanelCanvas != null;
						if (flag3)
						{
							Object.Destroy(this._statusPanelCanvas);
							this._statusPanelCanvas = null;
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001EF88 File Offset: 0x0001D188
		private void CreateStatusContent()
		{
			GameObject gameObject = new GameObject("Title");
			gameObject.transform.SetParent(this._statusPanelCanvas.transform, false);
			TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
			textMeshProUGUI.text = "★━━【<color=#8143E6>KERNEL STATUS</color>】━━★";
			textMeshProUGUI.font = Enumerable.FirstOrDefault<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>());
			textMeshProUGUI.fontSize = 20f;
			textMeshProUGUI.color = Color.white;
			textMeshProUGUI.alignment = 514;
			textMeshProUGUI.fontStyle = 1;
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.anchorMin = new Vector2(0f, 1f);
			component.anchorMax = new Vector2(1f, 1f);
			component.offsetMin = new Vector2(10f, -40f);
			component.offsetMax = new Vector2(-10f, -10f);
			GameObject gameObject2 = new GameObject("StatusText");
			gameObject2.transform.SetParent(this._statusPanelCanvas.transform, false);
			this._statusText = gameObject2.AddComponent<TextMeshProUGUI>();
			this._statusText.font = Enumerable.FirstOrDefault<TMP_FontAsset>(Resources.FindObjectsOfTypeAll<TMP_FontAsset>());
			this._statusText.fontSize = 16f;
			this._statusText.color = Color.white;
			this._statusText.alignment = 257;
			this._statusText.fontStyle = 0;
			this._statusText.enableWordWrapping = true;
			this._statusText.overflowMode = 0;
			RectTransform component2 = gameObject2.GetComponent<RectTransform>();
			component2.anchorMin = new Vector2(0f, 0f);
			component2.anchorMax = new Vector2(1f, 1f);
			component2.offsetMin = new Vector2(15f, 15f);
			component2.offsetMax = new Vector2(-15f, -50f);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001F16C File Offset: 0x0001D36C
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
						Transform transform = gameObject.transform.Find("RightItemContainer");
						bool flag2 = transform != null;
						if (flag2)
						{
							this.CreateStatusButton(transform);
							return;
						}
					}
					this.CreateFallbackButton();
				}, "CreateToggleButton");
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001F1A0 File Offset: 0x0001D3A0
		private void CreateStatusButton(Transform parent)
		{
			try
			{
				GameObject gameObject = new GameObject("StatusPanel_Toggle");
				gameObject.transform.SetParent(parent, false);
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.sizeDelta = new Vector2(40f, 40f);
				Image image = gameObject.AddComponent<Image>();
				image.sprite = this.LoadStatusIcon();
				Button button = gameObject.AddComponent<Button>();
				button.onClick.AddListener(delegate()
				{
					this.TogglePanel();
				});
				try
				{
					VRCButtonHandle vrcbuttonHandle = gameObject.AddComponent<VRCButtonHandle>();
					ToolTip toolTip = gameObject.AddComponent<ToolTip>();
					bool flag = toolTip != null;
					if (flag)
					{
						toolTip._localizableString = LocalizableStringExtensions.Localize("Toggle Status Panel", null, null, null);
					}
				}
				catch
				{
				}
				this._buttonCreated = true;
				base.Log("Status panel toggle button created");
			}
			catch (Exception ex)
			{
				base.LogError("Error creating status button: " + ex.Message);
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001F2AC File Offset: 0x0001D4AC
		private void CreateFallbackButton()
		{
			base.Log("Using fallback button creation method");
			this._buttonCreated = true;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001F2C4 File Offset: 0x0001D4C4
		private Sprite LoadStatusIcon()
		{
			Sprite result;
			try
			{
				result = ClassicEmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.StatusPanelIcon.png");
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001F2F8 File Offset: 0x0001D4F8
		private void UpdateStatusDisplay()
		{
			bool flag = this._statusText == null || this._disposed;
			if (!flag)
			{
				try
				{
					bool flag2 = this._needsFullUpdate || Time.time - this._lastFullUpdateTime >= 5f;
					if (flag2)
					{
						this.UpdateCachedData();
						this._needsFullUpdate = false;
						this._lastFullUpdateTime = Time.time;
					}
					this.UpdateFPSHistory();
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(string.Format("<color=#FFD700>Time:</color> <color=#FFFFFF>{0:HH:mm:ss}</color>", DateTime.Now));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("<color=#FF6B6B>License:</color>");
					stringBuilder.AppendLine("<color=#FFFFFF>" + this._cachedLicenseInfo + "</color>");
					stringBuilder.AppendLine();
					float num = this.CalculateAverageFPS();
					stringBuilder.AppendLine(string.Format("<color=#4ECDC4>FPS:</color> <color=#FFFFFF>{0:F0}</color>", num));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("<color=#4ECDC4>World:</color>");
					string str = (this._cachedWorldId.Length > 40) ? (this._cachedWorldId.Substring(0, 40) + "...") : this._cachedWorldId;
					stringBuilder.AppendLine("<color=#FFFFFF>" + str + "</color>");
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(string.Format("<color=#9B59B6>Players:</color> <color=#FFFFFF>{0}</color>", this._cachedPlayerCount));
					stringBuilder.AppendLine();
					string text = this._cachedNetworkStatus ? "Connected" : "Disconnected";
					string text2 = this._cachedNetworkStatus ? "#4CAF50" : "#F44336";
					stringBuilder.AppendLine(string.Concat(new string[]
					{
						"<color=#8143E6>KRNL Network:</color> <color=",
						text2,
						">",
						text,
						"</color>"
					}));
					bool flag3 = this._cachedNetworkStatus && this._cachedKernellUserCount > 0;
					if (flag3)
					{
						stringBuilder.AppendLine(string.Format("<color=#8143E6>KRNL Users:</color> <color=#FFFFFF>{0}</color>", this._cachedKernellUserCount));
					}
					this._statusText.text = stringBuilder.ToString();
				}
				catch (Exception ex)
				{
					base.LogError("Error updating status display: " + ex.Message);
					bool flag4 = this._statusText != null;
					if (flag4)
					{
						this._statusText.text = "<color=#FF0000>Status Update Error:\n" + ex.Message + "</color>";
					}
				}
			}
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001F58C File Offset: 0x0001D78C
		private void UpdateCachedData()
		{
			try
			{
				bool flag = RoomManager.field_Private_Static_ApiWorldInstance_0 != null;
				if (flag)
				{
					string text = RoomManager.field_Private_Static_ApiWorldInstance_0.id ?? "Unknown";
					bool flag2 = this._cachedWorldId != text;
					if (flag2)
					{
						this._cachedWorldId = text;
						bool flag3 = KernellNetworkClient.Instance != null && !KernellNetworkClient.Instance.IsInWorldTransition;
						if (flag3)
						{
							KernellNetworkClient.Instance.UpdateWorld(text);
						}
					}
				}
			}
			catch
			{
			}
			try
			{
				PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
				bool flag4 = ((playerManager != null) ? playerManager.field_Private_List_1_Player_0 : null) != null;
				if (flag4)
				{
					this._cachedPlayerCount = playerManager.field_Private_List_1_Player_0.Count;
				}
				else
				{
					this._cachedPlayerCount = 0;
				}
			}
			catch
			{
				this._cachedPlayerCount = 0;
			}
			try
			{
				bool flag5 = KernellNetworkClient.Instance != null;
				if (flag5)
				{
					KernellNetworkClient instance = KernellNetworkClient.Instance;
					this._cachedNetworkStatus = (instance.IsConnected() && !instance.IsInWorldTransition);
					this._cachedKernellUserCount = (this._cachedNetworkStatus ? instance.TotalKernellUsers : 0);
				}
				else
				{
					this._cachedNetworkStatus = false;
					this._cachedKernellUserCount = 0;
				}
			}
			catch
			{
				this._cachedNetworkStatus = false;
				this._cachedKernellUserCount = 0;
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001F6F4 File Offset: 0x0001D8F4
		private void UpdateFPSHistory()
		{
			float item = 1f / Time.unscaledDeltaTime;
			this._fpsHistory.Enqueue(item);
			while (this._fpsHistory.Count > 30)
			{
				this._fpsHistory.Dequeue();
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001F740 File Offset: 0x0001D940
		private float CalculateAverageFPS()
		{
			bool flag = this._fpsHistory.Count == 0;
			float result;
			if (flag)
			{
				result = 60f;
			}
			else
			{
				result = Enumerable.Average(this._fpsHistory);
			}
			return result;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001F778 File Offset: 0x0001D978
		private void SubscribeToNetworkEvents()
		{
			try
			{
				bool flag = KernellNetworkClient.Instance != null;
				if (flag)
				{
					KernellNetworkClient instance = KernellNetworkClient.Instance;
					instance.OnConnectionStateChanged += this.OnNetworkConnectionChanged;
					instance.OnKernellUserCountUpdated += this.OnKernellUserCountUpdated;
				}
			}
			catch (Exception ex)
			{
				base.LogError("Failed to subscribe to network events: " + ex.Message);
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001F7F0 File Offset: 0x0001D9F0
		private void OnNetworkConnectionChanged(bool isConnected)
		{
			this._cachedNetworkStatus = isConnected;
			this._needsFullUpdate = true;
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001F801 File Offset: 0x0001DA01
		private void OnKernellUserCountUpdated(int userCount)
		{
			this._cachedKernellUserCount = userCount;
			this._needsFullUpdate = true;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001F814 File Offset: 0x0001DA14
		private void EnsureDirectories()
		{
			try
			{
				string directoryName = Path.GetDirectoryName(StatusPanel.STATE_FILE);
				bool flag = !Directory.Exists(directoryName);
				if (flag)
				{
					Directory.CreateDirectory(directoryName);
				}
			}
			catch (Exception ex)
			{
				base.LogError("Error creating directories: " + ex.Message);
			}
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001F874 File Offset: 0x0001DA74
		private void LoadState()
		{
			try
			{
				bool flag = File.Exists(StatusPanel.STATE_FILE);
				if (flag)
				{
					string text = File.ReadAllText(StatusPanel.STATE_FILE).Trim();
					StatusPanel._panelVisible = text.Equals("true", StringComparison.OrdinalIgnoreCase);
					base.Log(string.Format("Loaded panel state: {0}", StatusPanel._panelVisible));
				}
				else
				{
					this.SaveState();
					base.Log(string.Format("Created default panel state: {0}", StatusPanel._panelVisible));
				}
			}
			catch (Exception ex)
			{
				base.LogError("Error loading state: " + ex.Message);
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001F924 File Offset: 0x0001DB24
		private void SaveState()
		{
			try
			{
				File.WriteAllText(StatusPanel.STATE_FILE, StatusPanel._panelVisible.ToString().ToLower());
			}
			catch (Exception ex)
			{
				base.LogError("Error saving state: " + ex.Message);
			}
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001F97C File Offset: 0x0001DB7C
		private void TogglePanel()
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				this.SafeExecute(delegate
				{
					StatusPanel._panelVisible = !StatusPanel._panelVisible;
					this.SaveState();
					bool flag = this._statusPanelCanvas != null;
					if (flag)
					{
						this._statusPanelCanvas.SetActive(StatusPanel._panelVisible);
						bool panelVisible = StatusPanel._panelVisible;
						if (panelVisible)
						{
							this._needsFullUpdate = true;
						}
					}
					else
					{
						bool isInitialized = this._isInitialized;
						if (isInitialized)
						{
							this.StartInitialization();
						}
					}
					base.Log("Panel toggled: " + (StatusPanel._panelVisible ? "visible" : "hidden"));
				}, "TogglePanel");
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001F9B0 File Offset: 0x0001DBB0
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
					base.LogError("Error in " + operationName + ": " + ex.Message);
				}
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001FA0C File Offset: 0x0001DC0C
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
					base.Log("Disposing StatusPanel");
					try
					{
						bool flag2 = KernellNetworkClient.Instance != null;
						if (flag2)
						{
							KernellNetworkClient instance = KernellNetworkClient.Instance;
							instance.OnConnectionStateChanged -= this.OnNetworkConnectionChanged;
							instance.OnKernellUserCountUpdated -= this.OnKernellUserCountUpdated;
						}
					}
					catch
					{
					}
					bool flag3 = this._statusPanelCanvas != null;
					if (flag3)
					{
						Object.Destroy(this._statusPanelCanvas);
						this._statusPanelCanvas = null;
					}
					this._fpsHistory.Clear();
					this._isInitialized = false;
					this._isInitializing = false;
				}, "Dispose");
			}
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001FA7C File Offset: 0x0001DC7C
		public override void OnShutdown()
		{
			this.Dispose();
			base.OnShutdown();
		}

		// Token: 0x04000241 RID: 577
		private volatile bool _disposed = false;

		// Token: 0x04000242 RID: 578
		private volatile bool _isInitialized = false;

		// Token: 0x04000243 RID: 579
		private volatile bool _isInitializing = false;

		// Token: 0x04000244 RID: 580
		private readonly object _stateLock = new object();

		// Token: 0x04000245 RID: 581
		private int _initAttempts = 0;

		// Token: 0x04000246 RID: 582
		private const int MAX_INIT_ATTEMPTS = 20;

		// Token: 0x04000247 RID: 583
		private float _nextRetryTime = 0f;

		// Token: 0x04000248 RID: 584
		private const float RETRY_DELAY = 3f;

		// Token: 0x04000249 RID: 585
		private float _lastUpdateTime = 0f;

		// Token: 0x0400024A RID: 586
		private const float UPDATE_INTERVAL = 2f;

		// Token: 0x0400024B RID: 587
		private GameObject _statusPanelCanvas;

		// Token: 0x0400024C RID: 588
		private Transform _menuParent;

		// Token: 0x0400024D RID: 589
		private TextMeshProUGUI _statusText;

		// Token: 0x0400024E RID: 590
		private bool _buttonCreated = false;

		// Token: 0x0400024F RID: 591
		private static bool _panelVisible = true;

		// Token: 0x04000250 RID: 592
		private static readonly string STATE_FILE = Path.Combine(Environment.CurrentDirectory, "UserData", "KernellVRCLite", "StatusPanelState.txt");

		// Token: 0x04000251 RID: 593
		private readonly Queue<float> _fpsHistory = new Queue<float>();

		// Token: 0x04000252 RID: 594
		private const int FPS_HISTORY_SIZE = 30;

		// Token: 0x04000253 RID: 595
		private string _cachedLicenseInfo = "KernellVRC Lite (Free)";

		// Token: 0x04000254 RID: 596
		private string _cachedWorldId = "Unknown";

		// Token: 0x04000255 RID: 597
		private int _cachedPlayerCount = 0;

		// Token: 0x04000256 RID: 598
		private bool _cachedNetworkStatus = false;

		// Token: 0x04000257 RID: 599
		private int _cachedKernellUserCount = 0;

		// Token: 0x04000258 RID: 600
		private bool _needsFullUpdate = true;

		// Token: 0x04000259 RID: 601
		private float _lastFullUpdateTime = 0f;

		// Token: 0x0400025A RID: 602
		private const float FULL_UPDATE_INTERVAL = 5f;
	}
}
