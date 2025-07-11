using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KernellClientUI.Managers;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernellVRCLite.Network;
using KernelVRC;
using MelonLoader;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.Core;

namespace KernellVRCLite.modules
{
	// Token: 0x0200009F RID: 159
	public class LiveChat : KernelModuleBase
	{
		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600080F RID: 2063 RVA: 0x00031354 File Offset: 0x0002F554
		public override string ModuleName
		{
			get
			{
				return "LiveChat";
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0003135B File Offset: 0x0002F55B
		public override string Version
		{
			get
			{
				return "4.4.4";
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000811 RID: 2065 RVA: 0x00031362 File Offset: 0x0002F562
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.PlayerEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000812 RID: 2066 RVA: 0x00031369 File Offset: 0x0002F569
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every5Frames;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000813 RID: 2067 RVA: 0x00003315 File Offset: 0x00001515
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.Normal;
			}
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0003136C File Offset: 0x0002F56C
		public override void OnInitialize()
		{
			try
			{
				this.InitializeConfiguration();
				this.LoadAudioResources();
				kernelllogger.Msg("[" + this.ModuleName + "] Configuration initialized");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Initialization error: " + ex.Message);
			}
		}

		// Token: 0x06000815 RID: 2069 RVA: 0x000313E0 File Offset: 0x0002F5E0
		public override void OnUiManagerInit()
		{
			try
			{
				bool flag;
				if (MenuSetup.IsReady)
				{
					UiManager uiManager = MenuSetup._uiManager;
					flag = (((uiManager != null) ? uiManager.QMMenu : null) == null);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				if (flag2)
				{
					kernelllogger.Warning("[" + this.ModuleName + "] MenuSetup not ready, deferring UI initialization");
				}
				else
				{
					this.CreateMenuUI();
					this.InitializeNetwork();
					this.CreateOverlay();
					kernelllogger.Msg("[" + this.ModuleName + "] UI initialized successfully");
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] UI setup failed: " + ex.Message);
			}
		}

		// Token: 0x06000816 RID: 2070 RVA: 0x00031498 File Offset: 0x0002F698
		public override void OnUpdate()
		{
			try
			{
				this.ProcessPendingMessages();
				bool flag = Time.time - this._lastUpdateTime < 0.5f;
				if (!flag)
				{
					this._lastUpdateTime = Time.time;
					bool flag2 = this._uiNeedsUpdate && this._overlayText != null && this._overlayEnabled;
					if (flag2)
					{
						this.UpdateChatDisplay();
						this._uiNeedsUpdate = false;
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Update error: " + ex.Message);
			}
		}

		// Token: 0x06000817 RID: 2071 RVA: 0x00031540 File Offset: 0x0002F740
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			try
			{
				bool flag = this._overlayEnabled && sceneName != "ui" && sceneName != "Application2";
				if (flag)
				{
					MelonCoroutines.Start(this.RecreateOverlayCoroutine());
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Scene load error: " + ex.Message);
			}
		}

		// Token: 0x06000818 RID: 2072 RVA: 0x000315C0 File Offset: 0x0002F7C0
		private void LoadAudioResources()
		{
			try
			{
				ClassicEmbeddedResourceLoader.Initialize();
				GameObject gameObject = new GameObject("LiveChatAudio");
				Object.DontDestroyOnLoad(gameObject);
				this._audioSource = gameObject.AddComponent<AudioSource>();
				AudioSource audioSource = this._audioSource;
				LiveChat.ChatConfiguration config = this._config;
				audioSource.volume = ((config != null) ? config.NotificationVolume : 0.7f);
				this._audioSource.playOnAwake = false;
				try
				{
					this._messageSound = ClassicEmbeddedResourceLoader.LoadEmbeddedAudioClip("KernellVRCLite.assets.message_notification.wav");
					this._sentSound = ClassicEmbeddedResourceLoader.LoadEmbeddedAudioClip("KernellVRCLite.assets.sent.wav");
					bool flag = this._messageSound != null && this._sentSound != null;
					if (flag)
					{
						kernelllogger.Msg("[" + this.ModuleName + "] Audio resources loaded successfully");
					}
					else
					{
						kernelllogger.Warning("[" + this.ModuleName + "] Some audio clips failed to load");
						bool flag2 = this._messageSound == null;
						if (flag2)
						{
							kernelllogger.Warning("[" + this.ModuleName + "] Failed to load message_notification.wav");
						}
						bool flag3 = this._sentSound == null;
						if (flag3)
						{
							kernelllogger.Warning("[" + this.ModuleName + "] Failed to load sent.wav");
						}
					}
				}
				catch (Exception ex)
				{
					kernelllogger.Warning("[" + this.ModuleName + "] Could not load audio clips: " + ex.Message);
				}
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Audio initialization error: " + ex2.Message);
			}
		}

		// Token: 0x06000819 RID: 2073 RVA: 0x00031780 File Offset: 0x0002F980
		private void PlayNotificationSound(bool isSentMessage = false)
		{
			try
			{
				bool flag = !this._config.EnableSoundNotifications || this._audioSource == null;
				if (!flag)
				{
					AudioClip audioClip = isSentMessage ? this._sentSound : this._messageSound;
					bool flag2 = audioClip != null;
					if (flag2)
					{
						this._audioSource.volume = this._config.NotificationVolume;
						this._audioSource.PlayOneShot(audioClip);
						kernelllogger.Msg(string.Format("[{0}] Playing {1} sound at volume {2}", this.ModuleName, isSentMessage ? "sent" : "message", this._config.NotificationVolume));
					}
					else
					{
						kernelllogger.Warning("[" + this.ModuleName + "] Sound clip not available: " + (isSentMessage ? "sent" : "message"));
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Error playing notification sound: " + ex.Message);
			}
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x00031894 File Offset: 0x0002FA94
		private void InitializeConfiguration()
		{
			try
			{
				string text = Path.Combine(Environment.CurrentDirectory, "UserData", "KernellVRCLite");
				Directory.CreateDirectory(text);
				this._configFilePath = Path.Combine(text, "chat_config.json");
				bool flag = File.Exists(this._configFilePath);
				if (flag)
				{
					string text2 = File.ReadAllText(this._configFilePath);
					this._config = (JsonConvert.DeserializeObject<LiveChat.ChatConfiguration>(text2) ?? new LiveChat.ChatConfiguration());
				}
				else
				{
					this._config = new LiveChat.ChatConfiguration();
					this.SaveConfiguration();
				}
				this._overlayEnabled = this._config.ShowOverlay;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Config initialization error: " + ex.Message);
				this._config = new LiveChat.ChatConfiguration();
			}
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00031970 File Offset: 0x0002FB70
		private void SaveConfiguration()
		{
			try
			{
				bool flag = this._config != null && !string.IsNullOrEmpty(this._configFilePath);
				if (flag)
				{
					string contents = JsonConvert.SerializeObject(this._config, 1);
					File.WriteAllText(this._configFilePath, contents);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Config save error: " + ex.Message);
			}
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x000319F4 File Offset: 0x0002FBF4
		private void CreateMenuUI()
		{
			try
			{
				ReMenuCategory reMenuCategory = MenuSetup._uiManager.QMMenu.GetCategoryPage("Utility").AddCategory("Live Chat", true, "#ffffff", false);
				reMenuCategory.AddButton("\ud83d\udcdd Send Message", "Send a message to all KRNL users", delegate
				{
					this.OpenMessageInput();
				}, null, "#00ff00");
				ReMenuCategory reMenuCategory2 = reMenuCategory;
				string text = "\ud83d\uddbc️ Show Overlay";
				string tooltip = "Toggle chat overlay display";
				Action<bool> onToggle = delegate(bool value)
				{
					this.ToggleOverlay(value);
				};
				LiveChat.ChatConfiguration config = this._config;
				reMenuCategory2.AddToggle(text, tooltip, onToggle, config == null || config.ShowOverlay, "#9966ff");
				ReMenuCategory reMenuCategory3 = reMenuCategory;
				string text2 = "\ud83d\udce2 Notifications";
				string tooltip2 = "Toggle toast notifications";
				Action<bool> onToggle2 = delegate(bool value)
				{
					this.ToggleNotifications(value);
				};
				LiveChat.ChatConfiguration config2 = this._config;
				reMenuCategory3.AddToggle(text2, tooltip2, onToggle2, config2 == null || config2.ShowNotifications, "#ff9900");
				ReMenuCategory reMenuCategory4 = reMenuCategory;
				string text3 = "\ud83d\udd0a Sounds";
				string tooltip3 = "Toggle sound notifications";
				Action<bool> onToggle3 = delegate(bool value)
				{
					this.ToggleSounds(value);
				};
				LiveChat.ChatConfiguration config3 = this._config;
				reMenuCategory4.AddToggle(text3, tooltip3, onToggle3, config3 != null && config3.EnableSoundNotifications, "#00ff99");
				ReCategoryPage reCategoryPage = reMenuCategory.AddCategoryPage("Settings", "", null, "#ffffff");
				ReMenuSliderCategory reMenuSliderCategory = reCategoryPage.AddSliderCategory("Audio Settings", true, "#ffffff", false);
				ReMenuSliderCategory reMenuSliderCategory2 = reMenuSliderCategory;
				string text4 = "Volume";
				string tooltip4 = "Notification sound volume";
				Action<float> onSlide = delegate(float value)
				{
					this.SetNotificationVolume(value);
				};
				LiveChat.ChatConfiguration config4 = this._config;
				reMenuSliderCategory2.AddSlider(text4, tooltip4, onSlide, (config4 != null) ? config4.NotificationVolume : 0.7f, 0f, 1f, "#ffffff");
				reMenuCategory.AddButton("\ud83d\udd04 Reconnect", "Reconnect to KRNL network", delegate
				{
					this.ReconnectNetwork();
				}, null, "#ffaa00");
				reMenuCategory.AddButton("\ud83d\uddd1️ Clear Chat", "Clear all chat messages", delegate
				{
					this.ClearChat();
				}, null, "#ff6666");
				kernelllogger.Msg("[" + this.ModuleName + "] Menu UI created");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Menu UI creation error: " + ex.Message);
			}
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00031BFC File Offset: 0x0002FDFC
		private void CreateOverlay()
		{
			try
			{
				bool flag = this._overlayCanvas != null;
				if (!flag)
				{
					this._overlayCanvas = new GameObject("LiveChatOverlay");
					Object.DontDestroyOnLoad(this._overlayCanvas);
					Canvas canvas = this._overlayCanvas.AddComponent<Canvas>();
					canvas.renderMode = 0;
					canvas.sortingOrder = 10000;
					CanvasScaler canvasScaler = this._overlayCanvas.AddComponent<CanvasScaler>();
					canvasScaler.uiScaleMode = 1;
					canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
					this.CreateOverlayPanel();
					kernelllogger.Msg("[" + this.ModuleName + "] Overlay created");
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Overlay creation error: " + ex.Message);
			}
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x00031CE4 File Offset: 0x0002FEE4
		private void CreateOverlayPanel()
		{
			try
			{
				this._chatOverlay = new GameObject("ChatPanel");
				this._chatOverlay.transform.SetParent(this._overlayCanvas.transform, false);
				RectTransform rectTransform = this._chatOverlay.AddComponent<RectTransform>();
				rectTransform.anchorMin = new Vector2(1f, 1f);
				rectTransform.anchorMax = new Vector2(1f, 1f);
				rectTransform.pivot = new Vector2(1f, 1f);
				rectTransform.sizeDelta = new Vector2(400f, 250f);
				rectTransform.anchoredPosition = new Vector2(-20f, -40f);
				CanvasGroup canvasGroup = this._chatOverlay.AddComponent<CanvasGroup>();
				canvasGroup.interactable = false;
				canvasGroup.blocksRaycasts = false;
				CanvasGroup canvasGroup2 = canvasGroup;
				LiveChat.ChatConfiguration config = this._config;
				canvasGroup2.alpha = ((config != null) ? config.OverlayOpacity : 0.9f);
				Image image = this._chatOverlay.AddComponent<Image>();
				image.color = new Color(0.02f, 0.02f, 0.1f, 0.95f);
				image.raycastTarget = false;
				GameObject gameObject = new GameObject("Title");
				gameObject.transform.SetParent(this._chatOverlay.transform, false);
				TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
				textMeshProUGUI.text = "★━【<color=#00ffff>KRNL LIVE CHAT</color>】━★";
				textMeshProUGUI.fontSize = 14f;
				textMeshProUGUI.fontStyle = 1;
				textMeshProUGUI.color = Color.white;
				textMeshProUGUI.alignment = 514;
				textMeshProUGUI.raycastTarget = false;
				RectTransform component = gameObject.GetComponent<RectTransform>();
				component.anchorMin = new Vector2(0f, 1f);
				component.anchorMax = new Vector2(1f, 1f);
				component.sizeDelta = new Vector2(0f, 25f);
				component.anchoredPosition = new Vector2(0f, -12.5f);
				GameObject gameObject2 = new GameObject("ScrollView");
				gameObject2.transform.SetParent(this._chatOverlay.transform, false);
				RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
				rectTransform2.anchorMin = Vector2.zero;
				rectTransform2.anchorMax = Vector2.one;
				rectTransform2.offsetMin = new Vector2(5f, 5f);
				rectTransform2.offsetMax = new Vector2(-5f, -30f);
				this._overlayScrollRect = gameObject2.AddComponent<ScrollRect>();
				this._overlayScrollRect.horizontal = false;
				this._overlayScrollRect.vertical = true;
				this._overlayScrollRect.scrollSensitivity = 20f;
				this._overlayScrollRect.movementType = 2;
				GameObject gameObject3 = new GameObject("Content");
				gameObject3.transform.SetParent(gameObject2.transform, false);
				RectTransform rectTransform3 = gameObject3.AddComponent<RectTransform>();
				rectTransform3.anchorMin = new Vector2(0f, 1f);
				rectTransform3.anchorMax = new Vector2(1f, 1f);
				rectTransform3.pivot = new Vector2(0.5f, 1f);
				rectTransform3.sizeDelta = new Vector2(0f, 200f);
				ContentSizeFitter contentSizeFitter = gameObject3.AddComponent<ContentSizeFitter>();
				contentSizeFitter.verticalFit = 2;
				this._overlayText = gameObject3.AddComponent<TextMeshProUGUI>();
				this._overlayText.fontSize = 11f;
				this._overlayText.color = Color.white;
				this._overlayText.alignment = 257;
				this._overlayText.enableWordWrapping = true;
				this._overlayText.margin = new Vector4(5f, 5f, 5f, 5f);
				this._overlayText.text = "<color=#888888>Waiting for messages...</color>";
				this._overlayText.raycastTarget = false;
				this._overlayText.overflowMode = 0;
				this._overlayScrollRect.content = rectTransform3;
				this._overlayScrollRect.viewport = rectTransform2;
				this._chatOverlay.SetActive(this._overlayEnabled);
				this._uiNeedsUpdate = true;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Overlay panel creation error: " + ex.Message);
			}
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x00032148 File Offset: 0x00030348
		private IEnumerator RecreateOverlayCoroutine()
		{
			yield return new WaitForSeconds(2f);
			try
			{
				this.CleanupOverlay();
				this.CreateOverlay();
				this._uiNeedsUpdate = true;
				yield break;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				kernelllogger.Error("[" + this.ModuleName + "] Overlay recreation error: " + ex.Message);
				yield break;
			}
			yield break;
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x00032158 File Offset: 0x00030358
		private void CleanupOverlay()
		{
			try
			{
				bool flag = this._overlayCanvas != null;
				if (flag)
				{
					Object.Destroy(this._overlayCanvas);
					this._overlayCanvas = null;
				}
				this._chatOverlay = null;
				this._overlayText = null;
				this._overlayScrollRect = null;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Overlay cleanup error: " + ex.Message);
			}
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x000321DC File Offset: 0x000303DC
		private void InitializeNetwork()
		{
			try
			{
				bool flag = !string.IsNullOrEmpty(this._networkCallbackId);
				if (flag)
				{
					KernellNetworkIntegration.UnregisterChatMessageCallback(new Action<string, string, string>(this.OnChatMessageReceived));
					this._networkCallbackId = null;
					kernelllogger.Msg("[" + this.ModuleName + "] Cleaned up existing network callback");
				}
				this._networkCallbackId = KernellNetworkIntegration.RegisterChatMessageCallback(new Action<string, string, string>(this.OnChatMessageReceived));
				bool flag2 = !string.IsNullOrEmpty(this._networkCallbackId);
				if (flag2)
				{
					this._networkInitialized = true;
					kernelllogger.Msg("[" + this.ModuleName + "] Network initialized with callback ID: " + this._networkCallbackId);
				}
				else
				{
					kernelllogger.Error("[" + this.ModuleName + "] Network initialization failed - no callback ID");
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Network initialization error: " + ex.Message);
			}
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x000322E0 File Offset: 0x000304E0
		private void ReconnectNetwork()
		{
			try
			{
				this._networkInitialized = false;
				HashSet<string> processedMessageIds = this._processedMessageIds;
				lock (processedMessageIds)
				{
					this._processedMessageIds.Clear();
				}
				this.InitializeNetwork();
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Reconnect error: " + ex.Message);
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x00032370 File Offset: 0x00030570
		private void OnChatMessageReceived(string userId, string displayName, string content)
		{
			try
			{
				kernelllogger.Msg(string.Concat(new string[]
				{
					"[",
					this.ModuleName,
					"] Network callback received: ",
					displayName,
					" - ",
					content
				}));
				string safeUserId = this.GetSafeUserId();
				bool flag = userId == safeUserId;
				if (flag)
				{
					kernelllogger.Msg("[" + this.ModuleName + "] Ignoring own message from " + userId);
				}
				else
				{
					string text = string.Format("{0}_{1}_{2}_{3}", new object[]
					{
						userId,
						displayName,
						content,
						DateTime.Now.Ticks / 10000000L
					});
					HashSet<string> processedMessageIds = this._processedMessageIds;
					lock (processedMessageIds)
					{
						bool flag3 = this._processedMessageIds.Contains(text);
						if (flag3)
						{
							kernelllogger.Msg("[" + this.ModuleName + "] Duplicate message detected, ignoring: " + text);
							return;
						}
						this._processedMessageIds.Add(text);
						bool flag4 = this._processedMessageIds.Count > 100;
						if (flag4)
						{
							List<string> list = Enumerable.ToList<string>(Enumerable.Take<string>(this._processedMessageIds, this._processedMessageIds.Count - 100));
							foreach (string text2 in list)
							{
								this._processedMessageIds.Remove(text2);
							}
						}
					}
					string sender = this.SanitizeText(displayName ?? "Unknown");
					string text3 = this.SanitizeText(content ?? "");
					bool flag5 = string.IsNullOrEmpty(text3);
					if (flag5)
					{
						kernelllogger.Warning("[" + this.ModuleName + "] Empty content after sanitization");
					}
					else
					{
						LiveChat.ChatMessage item = new LiveChat.ChatMessage
						{
							Sender = sender,
							Content = text3,
							Timestamp = DateTime.Now,
							IsLocal = false,
							MessageId = text
						};
						this._pendingMessages.Enqueue(item);
						kernelllogger.Msg("[" + this.ModuleName + "] Message queued for main thread processing: " + text);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Network callback error: " + ex.Message);
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0003262C File Offset: 0x0003082C
		private void ProcessPendingMessages()
		{
			try
			{
				for (;;)
				{
					LiveChat.ChatMessage message;
					bool flag = this._pendingMessages.TryDequeue(out message);
					if (!flag)
					{
						break;
					}
					bool flag2 = false;
					object chatLock = this._chatLock;
					lock (chatLock)
					{
						IEnumerable<LiveChat.ChatMessage> enumerable = Enumerable.Skip<LiveChat.ChatMessage>(this._chatHistory, Math.Max(0, this._chatHistory.Count - 10));
						flag2 = Enumerable.Any<LiveChat.ChatMessage>(enumerable, (LiveChat.ChatMessage m) => m.Sender == message.Sender && m.Content == message.Content && Math.Abs((m.Timestamp - message.Timestamp).TotalSeconds) < 5.0);
					}
					bool flag4 = flag2;
					if (flag4)
					{
						kernelllogger.Msg(string.Concat(new string[]
						{
							"[",
							this.ModuleName,
							"] Duplicate message in history, skipping: ",
							message.Sender,
							" - ",
							message.Content
						}));
					}
					else
					{
						object chatLock2 = this._chatLock;
						lock (chatLock2)
						{
							this._chatHistory.Add(message);
							while (this._chatHistory.Count > 50)
							{
								this._chatHistory.RemoveAt(0);
							}
						}
						this._uiNeedsUpdate = true;
						LiveChat.ChatConfiguration config = this._config;
						bool flag6 = config != null && config.ShowNotifications;
						if (flag6)
						{
							this.ShowNotification(message.Sender, message.Content);
							this.PlayNotificationSound(false);
						}
						kernelllogger.Msg("[" + this.ModuleName + "] Processed message from " + message.Sender);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Process pending messages error: " + ex.Message);
			}
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00032858 File Offset: 0x00030A58
		private void OpenMessageInput()
		{
			try
			{
				bool flag = !this._networkInitialized;
				if (flag)
				{
					ToastNotif.Toast("KRNL Live Chat", "Not connected to network", null, 3f);
				}
				else
				{
					string safeUsername = this.GetSafeUsername();
					string prompt = "\ud83d\udcac Send message to all KRNL users as " + safeUsername + ":";
					MelonCoroutines.Start(this.ShowInputDialogCoroutine(prompt));
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Message input error: " + ex.Message);
			}
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x000328EC File Offset: 0x00030AEC
		private IEnumerator ShowInputDialogCoroutine(string prompt)
		{
			Task<string> inputTask = InputBox.Create(prompt, "Enter your message...", "Send", false, "");
			while (!inputTask.IsCompleted)
			{
				yield return null;
			}
			string message = inputTask.Result;
			bool flag = !string.IsNullOrEmpty(message);
			if (flag)
			{
				this.SendMessage(message);
			}
			yield break;
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00032904 File Offset: 0x00030B04
		private void SendMessage(string message)
		{
			try
			{
				message = this.SanitizeText(message);
				bool flag = string.IsNullOrEmpty(message);
				if (flag)
				{
					ToastNotif.Toast("KRNL Live Chat", "Message is empty or invalid", null, 3f);
				}
				else
				{
					bool flag2 = this._networkInitialized && KernellNetworkIntegration.IsConnected();
					if (flag2)
					{
						KernellNetworkIntegration.SendChatMessage(message);
						string safeUsername = this.GetSafeUsername();
						LiveChat.ChatMessage item = new LiveChat.ChatMessage
						{
							Sender = safeUsername,
							Content = message,
							Timestamp = DateTime.Now,
							IsLocal = true,
							MessageId = string.Format("local_{0}", DateTime.Now.Ticks)
						};
						object chatLock = this._chatLock;
						lock (chatLock)
						{
							this._chatHistory.Add(item);
							while (this._chatHistory.Count > 50)
							{
								this._chatHistory.RemoveAt(0);
							}
						}
						this._uiNeedsUpdate = true;
						this.PlayNotificationSound(true);
						ToastNotif.Toast("KRNL Live Chat", "Message sent!", ClassicEmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.message.png"), 2f);
						kernelllogger.Msg("[" + this.ModuleName + "] Message sent: " + message);
					}
					else
					{
						ToastNotif.Toast("KRNL Live Chat", "Not connected to network", null, 3f);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Send message error: " + ex.Message);
				ToastNotif.Toast("KRNL Live Chat", "Failed to send message", null, 3f);
			}
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x00032AEC File Offset: 0x00030CEC
		private void UpdateChatDisplay()
		{
			try
			{
				bool flag = this._overlayText == null;
				if (!flag)
				{
					object chatLock = this._chatLock;
					List<LiveChat.ChatMessage> list;
					lock (chatLock)
					{
						list = Enumerable.ToList<LiveChat.ChatMessage>(Enumerable.Skip<LiveChat.ChatMessage>(this._chatHistory, Math.Max(0, this._chatHistory.Count - 10)));
					}
					StringBuilder stringBuilder = new StringBuilder();
					bool flag3 = list.Count == 0;
					if (flag3)
					{
						stringBuilder.AppendLine("<color=#666666>No messages yet... Be the first to chat! \ud83d\udcac</color>");
					}
					else
					{
						foreach (LiveChat.ChatMessage chatMessage in list)
						{
							string text = chatMessage.Timestamp.ToString("HH:mm");
							string messageColor = this.GetMessageColor(chatMessage);
							string messagePrefix = this.GetMessagePrefix(chatMessage);
							string value = string.Concat(new string[]
							{
								"<color=#555>[",
								text,
								"]</color> ",
								messagePrefix,
								"<color=",
								messageColor,
								"><b>",
								chatMessage.Sender,
								":</b></color> <color=#ffffff>",
								chatMessage.Content,
								"</color>"
							});
							stringBuilder.AppendLine(value);
						}
					}
					this._overlayText.text = stringBuilder.ToString();
					bool flag4 = this._overlayScrollRect != null;
					if (flag4)
					{
						Canvas.ForceUpdateCanvases();
						this._overlayScrollRect.verticalNormalizedPosition = 0f;
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Update chat display error: " + ex.Message);
			}
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x00032D04 File Offset: 0x00030F04
		private string GetMessageColor(LiveChat.ChatMessage msg)
		{
			bool isLocal = msg.IsLocal;
			string result;
			if (isLocal)
			{
				result = "#00ff88";
			}
			else
			{
				result = "#ffffff";
			}
			return result;
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x00032D30 File Offset: 0x00030F30
		private string GetMessagePrefix(LiveChat.ChatMessage msg)
		{
			bool isLocal = msg.IsLocal;
			string result;
			if (isLocal)
			{
				result = "\ud83d\udfe2 ";
			}
			else
			{
				result = "\ud83d\udd35 ";
			}
			return result;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x00032D5C File Offset: 0x00030F5C
		private void ClearChat()
		{
			try
			{
				object chatLock = this._chatLock;
				lock (chatLock)
				{
					this._chatHistory.Clear();
				}
				HashSet<string> processedMessageIds = this._processedMessageIds;
				lock (processedMessageIds)
				{
					this._processedMessageIds.Clear();
				}
				this._uiNeedsUpdate = true;
				ToastNotif.Toast("KRNL Live Chat", "Chat history cleared", null, 2f);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Clear chat error: " + ex.Message);
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x00032E38 File Offset: 0x00031038
		private void ToggleOverlay(bool value)
		{
			try
			{
				this._overlayEnabled = value;
				bool flag = this._chatOverlay != null;
				if (flag)
				{
					this._chatOverlay.SetActive(this._overlayEnabled);
				}
				bool flag2 = this._config != null;
				if (flag2)
				{
					this._config.ShowOverlay = value;
					this.SaveConfiguration();
				}
				string str = value ? "enabled" : "disabled";
				ToastNotif.Toast("KRNL Live Chat", "Overlay " + str, null, 2f);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Toggle overlay error: " + ex.Message);
			}
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x00032EFC File Offset: 0x000310FC
		private void ToggleNotifications(bool value)
		{
			try
			{
				bool flag = this._config != null;
				if (flag)
				{
					this._config.ShowNotifications = value;
					this.SaveConfiguration();
				}
				string str = value ? "enabled" : "disabled";
				ToastNotif.Toast("KRNL Live Chat", "Notifications " + str, null, 2f);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Toggle notifications error: " + ex.Message);
			}
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x00032F94 File Offset: 0x00031194
		private void ToggleSounds(bool value)
		{
			try
			{
				bool flag = this._config != null;
				if (flag)
				{
					this._config.EnableSoundNotifications = value;
					this.SaveConfiguration();
				}
				string str = value ? "enabled" : "disabled";
				ToastNotif.Toast("KRNL Live Chat", "Sound notifications " + str, null, 2f);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Toggle sounds error: " + ex.Message);
			}
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0003302C File Offset: 0x0003122C
		private void SetNotificationVolume(float volume)
		{
			try
			{
				bool flag = this._config != null;
				if (flag)
				{
					this._config.NotificationVolume = volume;
					this.SaveConfiguration();
				}
				bool flag2 = this._audioSource != null;
				if (flag2)
				{
					this._audioSource.volume = volume;
				}
				ToastNotif.Toast("KRNL Live Chat", string.Format("Volume set to {0:F0}%", volume * 100f), null, 2f);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Set volume error: " + ex.Message);
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000330E0 File Offset: 0x000312E0
		private void ShowNotification(string sender, string message)
		{
			try
			{
				bool flag = !this._config.ShowNotifications;
				if (!flag)
				{
					sender = this.SanitizeText(sender);
					message = this.SanitizeText(message);
					string description = (message.Length > 50) ? (message.Substring(0, 50) + "...") : message;
					ToastNotif.Toast("\ud83d\udcac " + sender, description, ClassicEmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.message.png"), 4f);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Show notification error: " + ex.Message);
			}
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x00033190 File Offset: 0x00031390
		private string SanitizeText(string text)
		{
			bool flag = string.IsNullOrEmpty(text);
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				try
				{
					text = text.Trim();
					text = text.Replace("<", "&lt;").Replace(">", "&gt;");
					text = Regex.Replace(text, "<[^>]*>", "");
					bool flag2 = text.Length > 150;
					if (flag2)
					{
						text = text.Substring(0, 150) + "...";
					}
					result = text;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[" + this.ModuleName + "] Text sanitization error: " + ex.Message);
					result = "";
				}
			}
			return result;
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0003325C File Offset: 0x0003145C
		private string GetSafeUsername()
		{
			string result;
			try
			{
				APIUser currentUser = APIUser.CurrentUser;
				bool flag = currentUser != null && !string.IsNullOrEmpty(currentUser.displayName);
				if (flag)
				{
					result = currentUser.displayName;
				}
				else
				{
					result = "Unknown User";
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Get username error: " + ex.Message);
				result = "Unknown User";
			}
			return result;
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x000332D8 File Offset: 0x000314D8
		private string GetSafeUserId()
		{
			string result;
			try
			{
				APIUser currentUser = APIUser.CurrentUser;
				bool flag = currentUser != null && !string.IsNullOrEmpty(currentUser.id);
				if (flag)
				{
					result = currentUser.id;
				}
				else
				{
					result = "";
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Get user ID error: " + ex.Message);
				result = "";
			}
			return result;
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00033354 File Offset: 0x00031554
		public override void OnShutdown()
		{
			try
			{
				bool flag = !string.IsNullOrEmpty(this._networkCallbackId);
				if (flag)
				{
					KernellNetworkIntegration.UnregisterChatMessageCallback(new Action<string, string, string>(this.OnChatMessageReceived));
					kernelllogger.Msg("[" + this.ModuleName + "] Unregistered network callback: " + this._networkCallbackId);
				}
				this.CleanupOverlay();
				bool flag2 = this._audioSource != null;
				if (flag2)
				{
					Object.Destroy(this._audioSource.gameObject);
					this._audioSource = null;
				}
				object chatLock = this._chatLock;
				lock (chatLock)
				{
					this._chatHistory.Clear();
				}
				HashSet<string> processedMessageIds = this._processedMessageIds;
				lock (processedMessageIds)
				{
					this._processedMessageIds.Clear();
				}
				LiveChat.ChatMessage chatMessage;
				while (this._pendingMessages.TryDequeue(out chatMessage))
				{
				}
				kernelllogger.Msg("[" + this.ModuleName + "] Shutdown complete");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[" + this.ModuleName + "] Shutdown error: " + ex.Message);
			}
			base.OnShutdown();
		}

		// Token: 0x040003DB RID: 987
		private LiveChat.ChatConfiguration _config;

		// Token: 0x040003DC RID: 988
		private string _configFilePath;

		// Token: 0x040003DD RID: 989
		private readonly List<LiveChat.ChatMessage> _chatHistory = new List<LiveChat.ChatMessage>();

		// Token: 0x040003DE RID: 990
		private readonly object _chatLock = new object();

		// Token: 0x040003DF RID: 991
		private readonly ConcurrentQueue<LiveChat.ChatMessage> _pendingMessages = new ConcurrentQueue<LiveChat.ChatMessage>();

		// Token: 0x040003E0 RID: 992
		private readonly HashSet<string> _processedMessageIds = new HashSet<string>();

		// Token: 0x040003E1 RID: 993
		private bool _overlayEnabled = true;

		// Token: 0x040003E2 RID: 994
		private bool _networkInitialized = false;

		// Token: 0x040003E3 RID: 995
		private string _networkCallbackId;

		// Token: 0x040003E4 RID: 996
		private bool _uiNeedsUpdate = false;

		// Token: 0x040003E5 RID: 997
		private GameObject _overlayCanvas;

		// Token: 0x040003E6 RID: 998
		private GameObject _chatOverlay;

		// Token: 0x040003E7 RID: 999
		private TextMeshProUGUI _overlayText;

		// Token: 0x040003E8 RID: 1000
		private ScrollRect _overlayScrollRect;

		// Token: 0x040003E9 RID: 1001
		private AudioSource _audioSource;

		// Token: 0x040003EA RID: 1002
		private AudioClip _messageSound;

		// Token: 0x040003EB RID: 1003
		private AudioClip _sentSound;

		// Token: 0x040003EC RID: 1004
		private float _lastUpdateTime = 0f;

		// Token: 0x040003ED RID: 1005
		private const float UPDATE_INTERVAL = 0.5f;

		// Token: 0x040003EE RID: 1006
		private const int MAX_MESSAGES = 50;

		// Token: 0x040003EF RID: 1007
		private const int MAX_DISPLAY_MESSAGES = 10;

		// Token: 0x040003F0 RID: 1008
		private const int MAX_LINE_LENGTH = 150;

		// Token: 0x040003F1 RID: 1009
		private const int MAX_PROCESSED_IDS = 100;

		// Token: 0x02000179 RID: 377
		[Serializable]
		private class ChatConfiguration
		{
			// Token: 0x17000255 RID: 597
			// (get) Token: 0x06000CF7 RID: 3319 RVA: 0x0004C318 File Offset: 0x0004A518
			// (set) Token: 0x06000CF8 RID: 3320 RVA: 0x0004C320 File Offset: 0x0004A520
			public bool ShowOverlay { get; set; } = true;

			// Token: 0x17000256 RID: 598
			// (get) Token: 0x06000CF9 RID: 3321 RVA: 0x0004C329 File Offset: 0x0004A529
			// (set) Token: 0x06000CFA RID: 3322 RVA: 0x0004C331 File Offset: 0x0004A531
			public bool ShowNotifications { get; set; } = true;

			// Token: 0x17000257 RID: 599
			// (get) Token: 0x06000CFB RID: 3323 RVA: 0x0004C33A File Offset: 0x0004A53A
			// (set) Token: 0x06000CFC RID: 3324 RVA: 0x0004C342 File Offset: 0x0004A542
			public float OverlayOpacity { get; set; } = 0.9f;

			// Token: 0x17000258 RID: 600
			// (get) Token: 0x06000CFD RID: 3325 RVA: 0x0004C34B File Offset: 0x0004A54B
			// (set) Token: 0x06000CFE RID: 3326 RVA: 0x0004C353 File Offset: 0x0004A553
			public bool EnableSoundNotifications { get; set; } = false;

			// Token: 0x17000259 RID: 601
			// (get) Token: 0x06000CFF RID: 3327 RVA: 0x0004C35C File Offset: 0x0004A55C
			// (set) Token: 0x06000D00 RID: 3328 RVA: 0x0004C364 File Offset: 0x0004A564
			public float NotificationVolume { get; set; } = 0.7f;
		}

		// Token: 0x0200017A RID: 378
		private class ChatMessage
		{
			// Token: 0x1700025A RID: 602
			// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0004C3A1 File Offset: 0x0004A5A1
			// (set) Token: 0x06000D03 RID: 3331 RVA: 0x0004C3A9 File Offset: 0x0004A5A9
			public string Sender { get; set; } = "";

			// Token: 0x1700025B RID: 603
			// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0004C3B2 File Offset: 0x0004A5B2
			// (set) Token: 0x06000D05 RID: 3333 RVA: 0x0004C3BA File Offset: 0x0004A5BA
			public string Content { get; set; } = "";

			// Token: 0x1700025C RID: 604
			// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0004C3C3 File Offset: 0x0004A5C3
			// (set) Token: 0x06000D07 RID: 3335 RVA: 0x0004C3CB File Offset: 0x0004A5CB
			public DateTime Timestamp { get; set; } = DateTime.Now;

			// Token: 0x1700025D RID: 605
			// (get) Token: 0x06000D08 RID: 3336 RVA: 0x0004C3D4 File Offset: 0x0004A5D4
			// (set) Token: 0x06000D09 RID: 3337 RVA: 0x0004C3DC File Offset: 0x0004A5DC
			public bool IsLocal { get; set; } = false;

			// Token: 0x1700025E RID: 606
			// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0004C3E5 File Offset: 0x0004A5E5
			// (set) Token: 0x06000D0B RID: 3339 RVA: 0x0004C3ED File Offset: 0x0004A5ED
			public string MessageId { get; set; } = "";
		}
	}
}
