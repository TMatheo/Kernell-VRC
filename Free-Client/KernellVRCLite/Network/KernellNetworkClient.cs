using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KernelVRC;
using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using VRC.Core;

namespace KernellVRCLite.Network
{
	// Token: 0x0200007F RID: 127
	public class KernellNetworkClient : IDisposable
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x000225F9 File Offset: 0x000207F9
		public static KernellNetworkClient Instance
		{
			get
			{
				return KernellNetworkClient._instance;
			}
		}

		// Token: 0x14000019 RID: 25
		// (add) Token: 0x060005A5 RID: 1445 RVA: 0x00022600 File Offset: 0x00020800
		// (remove) Token: 0x060005A6 RID: 1446 RVA: 0x00022638 File Offset: 0x00020838
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<bool> OnConnectionStateChanged;

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060005A7 RID: 1447 RVA: 0x00022670 File Offset: 0x00020870
		// (remove) Token: 0x060005A8 RID: 1448 RVA: 0x000226A8 File Offset: 0x000208A8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<int> OnKernellUserCountUpdated;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060005A9 RID: 1449 RVA: 0x000226E0 File Offset: 0x000208E0
		// (remove) Token: 0x060005AA RID: 1450 RVA: 0x00022718 File Offset: 0x00020918
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string, string, string> OnChatMessageReceived;

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060005AB RID: 1451 RVA: 0x00022750 File Offset: 0x00020950
		// (remove) Token: 0x060005AC RID: 1452 RVA: 0x00022788 File Offset: 0x00020988
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string, bool> OnUserCheckResult;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060005AD RID: 1453 RVA: 0x000227C0 File Offset: 0x000209C0
		// (remove) Token: 0x060005AE RID: 1454 RVA: 0x000227F8 File Offset: 0x000209F8
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> OnDebugMessage;

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060005AF RID: 1455 RVA: 0x00022830 File Offset: 0x00020A30
		// (remove) Token: 0x060005B0 RID: 1456 RVA: 0x00022868 File Offset: 0x00020A68
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string, bool> OnForceClose;

		// Token: 0x060005B1 RID: 1457 RVA: 0x000228A0 File Offset: 0x00020AA0
		public static bool Initialize()
		{
			bool result;
			try
			{
				bool flag = KernellNetworkClient._instance != null;
				if (flag)
				{
					kernelllogger.Warning("[KernellNetworkClient] Already initialized");
					result = true;
				}
				else
				{
					KernellNetworkClient._instance = new KernellNetworkClient();
					result = KernellNetworkClient._instance.InitializeInternal();
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Initialization failed: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x00022910 File Offset: 0x00020B10
		private bool InitializeInternal()
		{
			bool result;
			try
			{
				kernelllogger.Msg("[KernellNetworkClient] Initializing Kernell Network Client...");
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary["User-Agent"] = "KernellVRCLite/2.1.0";
				dictionary["Origin"] = "https://kernell.net";
				dictionary["X-Client-Version"] = "2.1.0";
				Dictionary<string, string> headers = dictionary;
				this._socket = new KernellSocket("wss://rtcs.kernell.net/ws", headers);
				this._socket.OnConnected += this.HandleConnected;
				this._socket.OnDisconnected += this.HandleDisconnected;
				this._socket.OnError += this.HandleError;
				this._socket.OnMessageReceived += this.HandleMessageReceived;
				this._socket.OnDebugLog += this.HandleDebugLog;
				this._socket.OnForceClose += this.HandleForceClose;
				this._isInitialized = true;
				kernelllogger.Msg("[KernellNetworkClient] Client initialized successfully");
				result = true;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Internal initialization failed: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x00022A4C File Offset: 0x00020C4C
		private void HandleForceClose(string reason, bool isPermanent)
		{
			try
			{
				kernelllogger.Error(string.Format("[KernellNetworkClient] Force close received: {0} (Permanent: {1})", reason, isPermanent));
				if (isPermanent)
				{
					object stateLock = this._stateLock;
					lock (stateLock)
					{
						this._allowConnections = false;
						this._permanentlyBlocked = true;
					}
					kernelllogger.Warning("[KernellNetworkClient] Reconnections disabled due to permanent block");
				}
				Action<string, bool> onForceClose = this.OnForceClose;
				if (onForceClose != null)
				{
					onForceClose(reason, isPermanent);
				}
				this.DisconnectImmediate();
				kernelllogger.Error("[KernellNetworkClient] FORCE CLOSING GAME due to server request");
				MelonLogger.Error("SERVER REQUESTED SHUTDOWN", new object[]
				{
					reason
				});
				Task.Delay(3000).ContinueWith(delegate(Task _)
				{
					try
					{
						Application.Quit();
						Process.GetCurrentProcess().Kill();
					}
					catch (Exception ex2)
					{
						kernelllogger.Error("[KernellNetworkClient] Failed to close game: " + ex2.Message);
					}
				});
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Force close handling error: " + ex.Message);
			}
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x00022B5C File Offset: 0x00020D5C
		public void OnWorldLeaving()
		{
			kernelllogger.Msg("[KernellNetworkClient] World leaving - preparing for transition");
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				this._isInWorldTransition = true;
				this._allowConnections = false;
				this._lastWorldChangeTime = DateTime.UtcNow;
			}
			object userCacheLock = this._userCacheLock;
			lock (userCacheLock)
			{
				this._connectedKrnlUsers.Clear();
				string[] array = Enumerable.ToArray<string>(this._knownKrnlUsers.Keys);
				foreach (string key in array)
				{
					bool flag3 = this._knownKrnlUsers.ContainsKey(key);
					if (flag3)
					{
						this._knownKrnlUsers[key].IsConnected = false;
					}
				}
			}
			KernellSocket socket = this._socket;
			if (socket != null)
			{
				socket.OnWorldChanged(null);
			}
			this.DisconnectImmediate();
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00022C74 File Offset: 0x00020E74
		public void OnWorldEntered(string worldId)
		{
			kernelllogger.Msg("[KernellNetworkClient] World entered: " + worldId);
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				this._pendingWorldId = worldId;
				this._lastWorldChangeTime = DateTime.UtcNow;
				this._retryAttempts = 0;
			}
			this.HandleWorldTransitionAsync(worldId);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00022CE8 File Offset: 0x00020EE8
		[DebuggerStepThrough]
		private Task HandleWorldTransitionAsync(string worldId)
		{
			KernellNetworkClient.<HandleWorldTransitionAsync>d__57 <HandleWorldTransitionAsync>d__ = new KernellNetworkClient.<HandleWorldTransitionAsync>d__57();
			<HandleWorldTransitionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<HandleWorldTransitionAsync>d__.<>4__this = this;
			<HandleWorldTransitionAsync>d__.worldId = worldId;
			<HandleWorldTransitionAsync>d__.<>1__state = -1;
			<HandleWorldTransitionAsync>d__.<>t__builder.Start<KernellNetworkClient.<HandleWorldTransitionAsync>d__57>(ref <HandleWorldTransitionAsync>d__);
			return <HandleWorldTransitionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005B7 RID: 1463 RVA: 0x00022D34 File Offset: 0x00020F34
		public void ForceDisconnect()
		{
			kernelllogger.Msg("[KernellNetworkClient] Force disconnect requested");
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				this._allowConnections = false;
			}
			this.DisconnectImmediate();
		}

		// Token: 0x060005B8 RID: 1464 RVA: 0x00022D90 File Offset: 0x00020F90
		public void AllowReconnection()
		{
			kernelllogger.Msg("[KernellNetworkClient] Reconnection allowed");
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				bool flag2 = !this._permanentlyBlocked;
				if (flag2)
				{
					this._allowConnections = true;
				}
				this._retryAttempts = 0;
			}
		}

		// Token: 0x060005B9 RID: 1465 RVA: 0x00022DFC File Offset: 0x00020FFC
		private void DisconnectImmediate()
		{
			try
			{
				KernellSocket socket = this._socket;
				bool flag = socket != null && socket.IsConnected;
				if (flag)
				{
					this._socket.Disconnect();
					kernelllogger.Msg("[KernellNetworkClient] Disconnected from network");
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Error during disconnect: " + ex.Message);
			}
			finally
			{
				object stateLock = this._stateLock;
				lock (stateLock)
				{
					this._isConnecting = false;
				}
			}
		}

		// Token: 0x060005BA RID: 1466 RVA: 0x00022EB0 File Offset: 0x000210B0
		[DebuggerStepThrough]
		private Task AttemptConnectionAsync()
		{
			KernellNetworkClient.<AttemptConnectionAsync>d__61 <AttemptConnectionAsync>d__ = new KernellNetworkClient.<AttemptConnectionAsync>d__61();
			<AttemptConnectionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AttemptConnectionAsync>d__.<>4__this = this;
			<AttemptConnectionAsync>d__.<>1__state = -1;
			<AttemptConnectionAsync>d__.<>t__builder.Start<KernellNetworkClient.<AttemptConnectionAsync>d__61>(ref <AttemptConnectionAsync>d__);
			return <AttemptConnectionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005BB RID: 1467 RVA: 0x00022EF4 File Offset: 0x000210F4
		private void HandleConnectionFailure()
		{
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				this._retryAttempts++;
				this._isConnecting = false;
			}
			bool flag2 = this._retryAttempts < 5 && this._allowConnections && !this._isInWorldTransition && !this._permanentlyBlocked;
			if (flag2)
			{
				kernelllogger.Msg(string.Format("[KernellNetworkClient] Scheduling retry attempt {0}/{1}", this._retryAttempts, 5));
				this.DelayedRetryAsync();
			}
			else
			{
				kernelllogger.Warning("[KernellNetworkClient] Max retry attempts reached or connections disabled");
			}
		}

		// Token: 0x060005BC RID: 1468 RVA: 0x00022FB4 File Offset: 0x000211B4
		[DebuggerStepThrough]
		private Task DelayedRetryAsync()
		{
			KernellNetworkClient.<DelayedRetryAsync>d__63 <DelayedRetryAsync>d__ = new KernellNetworkClient.<DelayedRetryAsync>d__63();
			<DelayedRetryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DelayedRetryAsync>d__.<>4__this = this;
			<DelayedRetryAsync>d__.<>1__state = -1;
			<DelayedRetryAsync>d__.<>t__builder.Start<KernellNetworkClient.<DelayedRetryAsync>d__63>(ref <DelayedRetryAsync>d__);
			return <DelayedRetryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005BD RID: 1469 RVA: 0x00022FF8 File Offset: 0x000211F8
		[DebuggerStepThrough]
		public Task<bool> ConnectAsync()
		{
			KernellNetworkClient.<ConnectAsync>d__64 <ConnectAsync>d__ = new KernellNetworkClient.<ConnectAsync>d__64();
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<ConnectAsync>d__.<>4__this = this;
			<ConnectAsync>d__.<>1__state = -1;
			<ConnectAsync>d__.<>t__builder.Start<KernellNetworkClient.<ConnectAsync>d__64>(ref <ConnectAsync>d__);
			return <ConnectAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005BE RID: 1470 RVA: 0x0002303C File Offset: 0x0002123C
		private void HandleConnected()
		{
			kernelllogger.Msg("[KernellNetworkClient] Connected to Kernell Network");
			Action<bool> onConnectionStateChanged = this.OnConnectionStateChanged;
			if (onConnectionStateChanged != null)
			{
				onConnectionStateChanged(true);
			}
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				this._retryAttempts = 0;
			}
		}

		// Token: 0x060005BF RID: 1471 RVA: 0x000230A0 File Offset: 0x000212A0
		private void HandleDisconnected(string reason)
		{
			kernelllogger.Warning("[KernellNetworkClient] Disconnected: " + reason);
			Action<bool> onConnectionStateChanged = this.OnConnectionStateChanged;
			if (onConnectionStateChanged != null)
			{
				onConnectionStateChanged(false);
			}
			object userCacheLock = this._userCacheLock;
			lock (userCacheLock)
			{
				this._connectedKrnlUsers.Clear();
				string[] array = Enumerable.ToArray<string>(this._knownKrnlUsers.Keys);
				foreach (string key in array)
				{
					bool flag2 = this._knownKrnlUsers.ContainsKey(key);
					if (flag2)
					{
						this._knownKrnlUsers[key].IsConnected = false;
					}
				}
			}
			bool flag3 = false;
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				flag3 = (this._allowConnections && !this._isInWorldTransition && !this._isDisposed && this._retryAttempts < 5 && !this._permanentlyBlocked);
			}
			bool flag5 = flag3;
			if (flag5)
			{
				kernelllogger.Msg("[KernellNetworkClient] Scheduling reconnection attempt");
				this.DelayedRetryAsync();
			}
		}

		// Token: 0x060005C0 RID: 1472 RVA: 0x000231F0 File Offset: 0x000213F0
		private void HandleError(Exception error)
		{
			kernelllogger.Error("[KernellNetworkClient] Socket error: " + error.Message);
		}

		// Token: 0x060005C1 RID: 1473 RVA: 0x00023209 File Offset: 0x00021409
		private void HandleDebugLog(string message)
		{
			Action<string> onDebugMessage = this.OnDebugMessage;
			if (onDebugMessage != null)
			{
				onDebugMessage(message);
			}
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x00023220 File Offset: 0x00021420
		[DebuggerStepThrough]
		private Task RegisterWithServerAsync()
		{
			KernellNetworkClient.<RegisterWithServerAsync>d__69 <RegisterWithServerAsync>d__ = new KernellNetworkClient.<RegisterWithServerAsync>d__69();
			<RegisterWithServerAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RegisterWithServerAsync>d__.<>4__this = this;
			<RegisterWithServerAsync>d__.<>1__state = -1;
			<RegisterWithServerAsync>d__.<>t__builder.Start<KernellNetworkClient.<RegisterWithServerAsync>d__69>(ref <RegisterWithServerAsync>d__);
			return <RegisterWithServerAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x00023264 File Offset: 0x00021464
		private string GetCurrentWorldId()
		{
			string result;
			try
			{
				bool flag = RoomManager.field_Private_Static_ApiWorldInstance_0 != null;
				if (flag)
				{
					ApiWorldInstance field_Private_Static_ApiWorldInstance_ = RoomManager.field_Private_Static_ApiWorldInstance_0;
					bool flag2 = !string.IsNullOrEmpty(field_Private_Static_ApiWorldInstance_.id);
					if (flag2)
					{
						return field_Private_Static_ApiWorldInstance_.id;
					}
				}
				bool flag3 = RoomManager.field_Private_Static_ApiWorldInstance_0 != null;
				if (flag3)
				{
					ApiWorldInstance field_Private_Static_ApiWorldInstance_2 = RoomManager.field_Private_Static_ApiWorldInstance_0;
					ApiWorld world = field_Private_Static_ApiWorldInstance_2.world;
					bool flag4 = !string.IsNullOrEmpty((world != null) ? world.id : null);
					if (flag4)
					{
						return field_Private_Static_ApiWorldInstance_2.world.id;
					}
				}
				string text;
				if ((text = this._pendingWorldId) == null)
				{
					text = (this._currentWorldId ?? "Unknown");
				}
				result = text;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Error getting world ID: " + ex.Message);
				string text2;
				if ((text2 = this._pendingWorldId) == null)
				{
					text2 = (this._currentWorldId ?? "Unknown");
				}
				result = text2;
			}
			return result;
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x00023354 File Offset: 0x00021554
		public void UpdateWorld(string worldId)
		{
			try
			{
				bool flag = !this.IsConnected() || string.IsNullOrEmpty(worldId);
				if (!flag)
				{
					this._currentWorldId = worldId;
					var obj = new
					{
						@event = "updateWorld",
						data = new
						{
							worldId
						}
					};
					bool flag2 = this._socket.SendJson(obj);
					bool flag3 = flag2;
					if (flag3)
					{
						kernelllogger.Msg("[KernellNetworkClient] World updated: " + worldId);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] World update failed: " + ex.Message);
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x000233E8 File Offset: 0x000215E8
		private void HandleMessageReceived(string message)
		{
			try
			{
				bool flag = string.IsNullOrEmpty(message);
				if (flag)
				{
					kernelllogger.Warning("[KernellNetworkClient] Received empty message");
				}
				else
				{
					bool flag2 = message.Contains("\"event\":\"forceClose\"");
					if (flag2)
					{
						this.HandleForceCloseMessage(message);
					}
					else
					{
						JObject jobject;
						try
						{
							jobject = JObject.Parse(message);
						}
						catch (JsonException ex)
						{
							kernelllogger.Error("[KernellNetworkClient] JSON parsing failed: " + ex.Message);
							return;
						}
						JToken jtoken = jobject["event"];
						bool flag3 = jtoken == null;
						if (flag3)
						{
							kernelllogger.Warning("[KernellNetworkClient] Message missing 'event' property");
						}
						else
						{
							string text = jtoken.ToString();
							bool flag4 = string.IsNullOrEmpty(text);
							if (flag4)
							{
								kernelllogger.Warning("[KernellNetworkClient] Event type is null or empty");
							}
							else
							{
								string text2 = text;
								string text3 = text2;
								uint num = <PrivateImplementationDetails>.ComputeStringHash(text3);
								if (num <= 823377653U)
								{
									if (num <= 563185489U)
									{
										if (num != 158815249U)
										{
											if (num == 563185489U)
											{
												if (text3 == "error")
												{
													this.HandleServerError(jobject);
													goto IL_26B;
												}
											}
										}
										else if (text3 == "userConnected")
										{
											this.HandleUserConnected(jobject);
											goto IL_26B;
										}
									}
									else if (num != 732027186U)
									{
										if (num == 823377653U)
										{
											if (text3 == "chatHistory")
											{
												this.HandleChatHistory(jobject);
												goto IL_26B;
											}
										}
									}
									else if (text3 == "chatMessage")
									{
										this.HandleChatMessage(jobject);
										goto IL_26B;
									}
								}
								else if (num <= 2061178551U)
								{
									if (num != 1667272331U)
									{
										if (num == 2061178551U)
										{
											if (text3 == "pong")
											{
												goto IL_26B;
											}
										}
									}
									else if (text3 == "userCheckResult")
									{
										this.HandleUserCheckResult(jobject);
										goto IL_26B;
									}
								}
								else if (num != 2910003557U)
								{
									if (num != 3242464270U)
									{
										if (num == 4162125017U)
										{
											if (text3 == "userCount")
											{
												this.HandleUserCount(jobject);
												goto IL_26B;
											}
										}
									}
									else if (text3 == "userList")
									{
										this.HandleUserList(jobject);
										goto IL_26B;
									}
								}
								else if (text3 == "userDisconnected")
								{
									this.HandleUserDisconnected(jobject);
									goto IL_26B;
								}
								kernelllogger.Msg("[KernellNetworkClient] Unknown event: " + text);
								IL_26B:;
							}
						}
					}
				}
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("[KernellNetworkClient] Message handling error: " + ex2.Message);
			}
		}

		// Token: 0x060005C6 RID: 1478 RVA: 0x000236B4 File Offset: 0x000218B4
		private void HandleForceCloseMessage(string message)
		{
			try
			{
				string text = "Connection terminated by server";
				bool flag = false;
				int num = message.IndexOf("\"reason\":\"");
				bool flag2 = num != -1;
				if (flag2)
				{
					num += 10;
					int num2 = message.IndexOf("\"", num);
					bool flag3 = num2 != -1;
					if (flag3)
					{
						text = message.Substring(num, num2 - num);
					}
				}
				bool flag4 = message.Contains("\"permanent\":true");
				if (flag4)
				{
					flag = true;
				}
				kernelllogger.Warning(string.Format("[KernellNetworkClient] Force close message received: {0} (Permanent: {1})", text, flag));
				this.HandleForceClose(text, flag);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Force close message parsing error: " + ex.Message);
			}
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00023778 File Offset: 0x00021978
		private void HandleChatMessage(JObject messageObj)
		{
			try
			{
				JToken jtoken = messageObj["data"];
				bool flag = jtoken == null;
				if (!flag)
				{
					JToken jtoken2 = jtoken["userId"];
					string text = (jtoken2 != null) ? jtoken2.ToString() : null;
					JToken jtoken3 = jtoken["displayName"];
					string text2 = (jtoken3 != null) ? jtoken3.ToString() : null;
					JToken jtoken4 = jtoken["message"];
					string text3 = (jtoken4 != null) ? jtoken4.ToString() : null;
					JToken jtoken5 = jtoken["worldId"];
					string text4 = (jtoken5 != null) ? jtoken5.ToString() : null;
					bool flag2 = string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text3);
					if (!flag2)
					{
						bool flag3 = !string.IsNullOrEmpty(text4);
						if (flag3)
						{
							object userCacheLock = this._userCacheLock;
							lock (userCacheLock)
							{
								bool flag5 = this._knownKrnlUsers.ContainsKey(text);
								if (flag5)
								{
									this._knownKrnlUsers[text].WorldId = text4;
								}
								else
								{
									this._knownKrnlUsers[text] = new KernellNetworkClient.KernellUserInfo
									{
										IsKrnlUser = true,
										LastChecked = DateTime.UtcNow,
										IsConnected = true,
										DisplayName = (text2 ?? "Unknown"),
										WorldId = text4
									};
								}
								this._connectedKrnlUsers.Add(text);
							}
						}
						object callbackLock = this._callbackLock;
						lock (callbackLock)
						{
							foreach (Action<string, string, string> action in this._chatCallbacks.Values)
							{
								try
								{
									if (action != null)
									{
										action(text, text2, text3);
									}
								}
								catch (Exception ex)
								{
									kernelllogger.Error("[KernellNetworkClient] Chat callback error: " + ex.Message);
								}
							}
						}
						Action<string, string, string> onChatMessageReceived = this.OnChatMessageReceived;
						if (onChatMessageReceived != null)
						{
							onChatMessageReceived(text, text2, text3);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("[KernellNetworkClient] Chat message handling error: " + ex2.Message);
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00023A20 File Offset: 0x00021C20
		private void HandleUserConnected(JObject messageObj)
		{
			try
			{
				JToken jtoken = messageObj["data"];
				bool flag = jtoken == null;
				if (!flag)
				{
					JToken jtoken2 = jtoken["userId"];
					string text = (jtoken2 != null) ? jtoken2.ToString() : null;
					JToken jtoken3 = jtoken["displayName"];
					string text2 = (jtoken3 != null) ? jtoken3.ToString() : null;
					JToken jtoken4 = jtoken["worldId"];
					string text3 = (jtoken4 != null) ? jtoken4.ToString() : null;
					bool flag2 = !string.IsNullOrEmpty(text);
					if (flag2)
					{
						object userCacheLock = this._userCacheLock;
						lock (userCacheLock)
						{
							this._connectedKrnlUsers.Add(text);
							this._knownKrnlUsers[text] = new KernellNetworkClient.KernellUserInfo
							{
								IsKrnlUser = true,
								LastChecked = DateTime.UtcNow,
								IsConnected = true,
								DisplayName = (text2 ?? "Unknown"),
								WorldId = (text3 ?? "Unknown")
							};
						}
						this.NotifyUserCheckCallbacks(text, true);
					}
					kernelllogger.Msg(string.Concat(new string[]
					{
						"[KernellNetworkClient] KRNL User connected: ",
						text2,
						" (World: ",
						text3,
						")"
					}));
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] User connected handling error: " + ex.Message);
			}
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00023BB8 File Offset: 0x00021DB8
		private void HandleUserDisconnected(JObject messageObj)
		{
			try
			{
				JToken jtoken = messageObj["data"];
				bool flag = jtoken == null;
				if (!flag)
				{
					JToken jtoken2 = jtoken["userId"];
					string text = (jtoken2 != null) ? jtoken2.ToString() : null;
					JToken jtoken3 = jtoken["worldId"];
					string text2 = (jtoken3 != null) ? jtoken3.ToString() : null;
					bool flag2 = !string.IsNullOrEmpty(text);
					if (flag2)
					{
						object userCacheLock = this._userCacheLock;
						lock (userCacheLock)
						{
							this._connectedKrnlUsers.Remove(text);
							bool flag4 = this._knownKrnlUsers.ContainsKey(text);
							if (flag4)
							{
								this._knownKrnlUsers[text].IsConnected = false;
							}
						}
						kernelllogger.Msg(string.Concat(new string[]
						{
							"[KernellNetworkClient] KRNL User disconnected: ",
							text,
							" (World: ",
							text2,
							")"
						}));
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] User disconnected handling error: " + ex.Message);
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00023CEC File Offset: 0x00021EEC
		private void HandleUserCount(JObject messageObj)
		{
			try
			{
				JToken jtoken = messageObj["data"];
				bool flag = jtoken == null;
				if (!flag)
				{
					JToken jtoken2 = jtoken["count"];
					bool flag2 = jtoken2 == null;
					if (!flag2)
					{
						int num = jtoken2.ToObject<int>();
						this._totalKernellUsers = num;
						Action<int> onKernellUserCountUpdated = this.OnKernellUserCountUpdated;
						if (onKernellUserCountUpdated != null)
						{
							onKernellUserCountUpdated(num);
						}
						kernelllogger.Msg(string.Format("[KernellNetworkClient] User count updated: {0}", num));
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] User count handling error: " + ex.Message);
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00023D90 File Offset: 0x00021F90
		private void HandleUserList(JObject messageObj)
		{
			try
			{
				JToken jtoken = messageObj["data"];
				bool flag = jtoken != null && jtoken.Type == 2;
				if (flag)
				{
					List<string> list = jtoken.ToObject<List<string>>();
					bool flag2 = list != null;
					if (flag2)
					{
						object userCacheLock = this._userCacheLock;
						lock (userCacheLock)
						{
							this._connectedKrnlUsers.Clear();
							foreach (string text in list)
							{
								this._connectedKrnlUsers.Add(text);
								bool flag4 = !this._knownKrnlUsers.ContainsKey(text);
								if (flag4)
								{
									this._knownKrnlUsers[text] = new KernellNetworkClient.KernellUserInfo
									{
										IsKrnlUser = true,
										LastChecked = DateTime.UtcNow,
										IsConnected = true,
										DisplayName = "Unknown",
										WorldId = "Unknown"
									};
								}
								else
								{
									this._knownKrnlUsers[text].IsConnected = true;
								}
							}
						}
						kernelllogger.Msg(string.Format("[KernellNetworkClient] Updated user list with {0} KRNL users", list.Count));
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] User list handling error: " + ex.Message);
			}
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00023F4C File Offset: 0x0002214C
		private void HandleUserCheckResult(JObject messageObj)
		{
			try
			{
				JToken jtoken = messageObj["data"];
				bool flag = jtoken == null;
				if (!flag)
				{
					JToken jtoken2 = jtoken["userId"];
					string text = (jtoken2 != null) ? jtoken2.ToString() : null;
					JToken jtoken3 = jtoken["exists"];
					bool flag2 = jtoken3 != null && jtoken3.ToObject<bool>();
					JToken jtoken4 = jtoken["worldId"];
					string text2 = (jtoken4 != null) ? jtoken4.ToString() : null;
					bool flag3 = string.IsNullOrEmpty(text);
					if (!flag3)
					{
						object userCacheLock = this._userCacheLock;
						lock (userCacheLock)
						{
							this._knownKrnlUsers[text] = new KernellNetworkClient.KernellUserInfo
							{
								IsKrnlUser = flag2,
								LastChecked = DateTime.UtcNow,
								IsConnected = (flag2 && this._connectedKrnlUsers.Contains(text)),
								DisplayName = "Unknown",
								WorldId = (text2 ?? "Unknown")
							};
							bool flag5 = flag2;
							if (flag5)
							{
								this._connectedKrnlUsers.Add(text);
							}
							else
							{
								this._connectedKrnlUsers.Remove(text);
							}
						}
						this.NotifyUserCheckCallbacks(text, flag2);
						kernelllogger.Msg(string.Format("[KernellNetworkClient] User check result: {0} -> {1} (World: {2})", text, flag2, text2));
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] User check result handling error: " + ex.Message);
			}
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x000240F0 File Offset: 0x000222F0
		private void HandleChatHistory(JObject messageObj)
		{
			try
			{
				kernelllogger.Msg("[KernellNetworkClient] Received chat history");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Chat history handling error: " + ex.Message);
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00024138 File Offset: 0x00022338
		private void HandleServerError(JObject messageObj)
		{
			try
			{
				JToken jtoken = messageObj["data"];
				bool flag = jtoken == null;
				if (!flag)
				{
					JToken jtoken2 = jtoken["message"];
					string str = ((jtoken2 != null) ? jtoken2.ToString() : null) ?? "Unknown server error";
					kernelllogger.Error("[KernellNetworkClient] Server error: " + str);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Server error handling error: " + ex.Message);
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000241C0 File Offset: 0x000223C0
		public void SendChatMessage(string message)
		{
			try
			{
				bool flag = !this.IsConnected() || string.IsNullOrEmpty(message);
				if (!flag)
				{
					var obj = new
					{
						@event = "chatMessage",
						data = new
						{
							message
						}
					};
					bool flag2 = this._socket.SendJson(obj);
					bool flag3 = flag2;
					if (flag3)
					{
						string str = (message.Length > 50) ? (message.Substring(0, 50) + "...") : message;
						kernelllogger.Msg("[KernellNetworkClient] Chat message sent: " + str);
					}
					else
					{
						kernelllogger.Error("[KernellNetworkClient] Failed to send chat message");
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Chat send error: " + ex.Message);
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00024284 File Offset: 0x00022484
		public string RegisterChatMessageCallback(Action<string, string, string> callback)
		{
			bool flag = callback == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string text = Guid.NewGuid().ToString();
				object callbackLock = this._callbackLock;
				lock (callbackLock)
				{
					this._chatCallbacks[text] = callback;
				}
				kernelllogger.Msg("[KernellNetworkClient] Chat callback registered: " + text);
				result = text;
			}
			return result;
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0002430C File Offset: 0x0002250C
		public void UnregisterChatMessageCallback(string callbackId)
		{
			bool flag = string.IsNullOrEmpty(callbackId);
			if (!flag)
			{
				object callbackLock = this._callbackLock;
				lock (callbackLock)
				{
					bool flag3 = this._chatCallbacks.Remove(callbackId);
					if (flag3)
					{
						kernelllogger.Msg("[KernellNetworkClient] Chat callback unregistered: " + callbackId);
					}
				}
			}
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x0002437C File Offset: 0x0002257C
		public bool IsKernellUser(string userId)
		{
			bool flag = string.IsNullOrEmpty(userId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object userCacheLock = this._userCacheLock;
				lock (userCacheLock)
				{
					KernellNetworkClient.KernellUserInfo kernellUserInfo;
					bool flag3 = this._knownKrnlUsers.TryGetValue(userId, out kernellUserInfo);
					if (flag3)
					{
						bool flag4 = (DateTime.UtcNow - kernellUserInfo.LastChecked).TotalSeconds < 300.0;
						if (flag4)
						{
							return kernellUserInfo.IsKrnlUser;
						}
					}
					result = this._connectedKrnlUsers.Contains(userId);
				}
			}
			return result;
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00024424 File Offset: 0x00022624
		public bool CheckUser(string userId)
		{
			bool result;
			try
			{
				bool flag = !this.IsConnected() || string.IsNullOrEmpty(userId);
				if (flag)
				{
					result = false;
				}
				else
				{
					var obj = new
					{
						@event = "checkUser",
						data = new
						{
							targetUserId = userId
						}
					};
					result = this._socket.SendJson(obj);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkClient] Check user error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x0002449C File Offset: 0x0002269C
		[DebuggerStepThrough]
		public Task<bool> CheckUserAsync(string userId)
		{
			KernellNetworkClient.<CheckUserAsync>d__87 <CheckUserAsync>d__ = new KernellNetworkClient.<CheckUserAsync>d__87();
			<CheckUserAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<CheckUserAsync>d__.<>4__this = this;
			<CheckUserAsync>d__.userId = userId;
			<CheckUserAsync>d__.<>1__state = -1;
			<CheckUserAsync>d__.<>t__builder.Start<KernellNetworkClient.<CheckUserAsync>d__87>(ref <CheckUserAsync>d__);
			return <CheckUserAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x000244E8 File Offset: 0x000226E8
		public string RegisterUserCheckCallback(Action<string, bool> callback)
		{
			bool flag = callback == null;
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string text = Guid.NewGuid().ToString();
				object callbackLock = this._callbackLock;
				lock (callbackLock)
				{
					this._userCheckCallbacks[text] = callback;
				}
				kernelllogger.Msg("[KernellNetworkClient] User check callback registered: " + text);
				result = text;
			}
			return result;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00024570 File Offset: 0x00022770
		public void UnregisterUserCheckCallback(string callbackId)
		{
			bool flag = string.IsNullOrEmpty(callbackId);
			if (!flag)
			{
				object callbackLock = this._callbackLock;
				lock (callbackLock)
				{
					bool flag3 = this._userCheckCallbacks.Remove(callbackId);
					if (flag3)
					{
						kernelllogger.Msg("[KernellNetworkClient] User check callback unregistered: " + callbackId);
					}
				}
			}
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x000245E0 File Offset: 0x000227E0
		private void NotifyUserCheckCallbacks(string userId, bool isKrnlUser)
		{
			object callbackLock = this._callbackLock;
			lock (callbackLock)
			{
				foreach (Action<string, bool> action in this._userCheckCallbacks.Values)
				{
					try
					{
						if (action != null)
						{
							action(userId, isKrnlUser);
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[KernellNetworkClient] User check callback error: " + ex.Message);
					}
				}
			}
			Action<string, bool> onUserCheckResult = this.OnUserCheckResult;
			if (onUserCheckResult != null)
			{
				onUserCheckResult(userId, isKrnlUser);
			}
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x000246B4 File Offset: 0x000228B4
		public void ClearUserCache()
		{
			object userCacheLock = this._userCacheLock;
			lock (userCacheLock)
			{
				this._knownKrnlUsers.Clear();
				this._connectedKrnlUsers.Clear();
				kernelllogger.Msg("[KernellNetworkClient] User cache cleared");
			}
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00024718 File Offset: 0x00022918
		public bool IsUserCached(string userId)
		{
			bool flag = string.IsNullOrEmpty(userId);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				object userCacheLock = this._userCacheLock;
				lock (userCacheLock)
				{
					result = this._knownKrnlUsers.ContainsKey(userId);
				}
			}
			return result;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00024774 File Offset: 0x00022974
		public int GetCachedUserCount()
		{
			object userCacheLock = this._userCacheLock;
			int result;
			lock (userCacheLock)
			{
				result = Enumerable.Count<KeyValuePair<string, KernellNetworkClient.KernellUserInfo>>(this._knownKrnlUsers, (KeyValuePair<string, KernellNetworkClient.KernellUserInfo> kvp) => kvp.Value.IsKrnlUser);
			}
			return result;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x000247E0 File Offset: 0x000229E0
		public int GetConnectedUserCount()
		{
			object userCacheLock = this._userCacheLock;
			int count;
			lock (userCacheLock)
			{
				count = this._connectedKrnlUsers.Count;
			}
			return count;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x0002482C File Offset: 0x00022A2C
		public string GetUserWorldId(string userId)
		{
			bool flag = string.IsNullOrEmpty(userId);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				object userCacheLock = this._userCacheLock;
				lock (userCacheLock)
				{
					KernellNetworkClient.KernellUserInfo kernellUserInfo;
					bool flag3 = this._knownKrnlUsers.TryGetValue(userId, out kernellUserInfo);
					if (flag3)
					{
						return kernellUserInfo.WorldId;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x000248A0 File Offset: 0x00022AA0
		public bool IsConnected()
		{
			object stateLock = this._stateLock;
			bool result;
			lock (stateLock)
			{
				KernellSocket socket = this._socket;
				result = (socket != null && socket.IsConnected && this._allowConnections && !this._isInWorldTransition);
			}
			return result;
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0002490C File Offset: 0x00022B0C
		public bool IsInitialized
		{
			get
			{
				return this._isInitialized;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x00024916 File Offset: 0x00022B16
		public int TotalKernellUsers
		{
			get
			{
				return this._totalKernellUsers;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x00024920 File Offset: 0x00022B20
		public bool IsInWorldTransition
		{
			get
			{
				object stateLock = this._stateLock;
				bool isInWorldTransition;
				lock (stateLock)
				{
					isInWorldTransition = this._isInWorldTransition;
				}
				return isInWorldTransition;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x00024968 File Offset: 0x00022B68
		public bool ConnectionAllowed
		{
			get
			{
				object stateLock = this._stateLock;
				bool allowConnections;
				lock (stateLock)
				{
					allowConnections = this._allowConnections;
				}
				return allowConnections;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x000249B0 File Offset: 0x00022BB0
		public bool IsPermanentlyBlocked
		{
			get
			{
				object stateLock = this._stateLock;
				bool permanentlyBlocked;
				lock (stateLock)
				{
					permanentlyBlocked = this._permanentlyBlocked;
				}
				return permanentlyBlocked;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x000249F8 File Offset: 0x00022BF8
		public DateTime LastWorldChangeTime
		{
			get
			{
				return this._lastWorldChangeTime;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x00024A00 File Offset: 0x00022C00
		public string CurrentWorldId
		{
			get
			{
				return this._currentWorldId;
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x00024A08 File Offset: 0x00022C08
		public NetworkStatus GetNetworkStatus()
		{
			KernellSocket socket = this._socket;
			NetworkStatus networkStatus;
			if ((networkStatus = ((socket != null) ? socket.GetStatus() : null)) == null)
			{
				NetworkStatus networkStatus2 = new NetworkStatus();
				networkStatus2.IsConnected = false;
				networkStatus2.IsConnecting = false;
				networkStatus = networkStatus2;
				networkStatus2.ServerUrl = "wss://rtcs.kernell.net/ws";
			}
			NetworkStatus networkStatus3 = networkStatus;
			object stateLock = this._stateLock;
			lock (stateLock)
			{
				networkStatus3.IsWorldTransitioning = this._isInWorldTransition;
			}
			return networkStatus3;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x00024A94 File Offset: 0x00022C94
		public void Dispose()
		{
			bool isDisposed = this._isDisposed;
			if (!isDisposed)
			{
				this._isDisposed = true;
				kernelllogger.Msg("[KernellNetworkClient] Disposing...");
				try
				{
					object stateLock = this._stateLock;
					lock (stateLock)
					{
						this._allowConnections = false;
						this._isInWorldTransition = true;
					}
					try
					{
						Task worldTransitionTask = this._worldTransitionTask;
						if (worldTransitionTask != null)
						{
							worldTransitionTask.Wait(2000);
						}
						Task connectionTask = this._connectionTask;
						if (connectionTask != null)
						{
							connectionTask.Wait(2000);
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Warning("[KernellNetworkClient] Task cleanup warning: " + ex.Message);
					}
					KernellSocket socket = this._socket;
					if (socket != null)
					{
						socket.Dispose();
					}
					object callbackLock = this._callbackLock;
					lock (callbackLock)
					{
						this._chatCallbacks.Clear();
						this._userCheckCallbacks.Clear();
					}
					object userCacheLock = this._userCacheLock;
					lock (userCacheLock)
					{
						this._knownKrnlUsers.Clear();
						this._connectedKrnlUsers.Clear();
					}
					KernellNetworkClient._instance = null;
					kernelllogger.Msg("[KernellNetworkClient] Disposed successfully");
				}
				catch (Exception ex2)
				{
					kernelllogger.Error("[KernellNetworkClient] Disposal error: " + ex2.Message);
				}
			}
		}

		// Token: 0x04000288 RID: 648
		private static KernellNetworkClient _instance;

		// Token: 0x0400028F RID: 655
		private const string SERVER_URL = "wss://rtcs.kernell.net/ws";

		// Token: 0x04000290 RID: 656
		private const int CONNECTION_RETRY_DELAY = 3000;

		// Token: 0x04000291 RID: 657
		private const int MAX_RETRY_ATTEMPTS = 5;

		// Token: 0x04000292 RID: 658
		private const float RECONNECT_DELAY_AFTER_WORLD_LOAD = 8f;

		// Token: 0x04000293 RID: 659
		private const float CONNECTION_TIMEOUT = 10f;

		// Token: 0x04000294 RID: 660
		private const int USER_CACHE_DURATION_SECONDS = 300;

		// Token: 0x04000295 RID: 661
		private KernellSocket _socket;

		// Token: 0x04000296 RID: 662
		private volatile bool _isInitialized = false;

		// Token: 0x04000297 RID: 663
		private volatile bool _isDisposed = false;

		// Token: 0x04000298 RID: 664
		private volatile bool _allowConnections = true;

		// Token: 0x04000299 RID: 665
		private volatile bool _isInWorldTransition = false;

		// Token: 0x0400029A RID: 666
		private volatile bool _isConnecting = false;

		// Token: 0x0400029B RID: 667
		private volatile bool _permanentlyBlocked = false;

		// Token: 0x0400029C RID: 668
		private string _currentUserId;

		// Token: 0x0400029D RID: 669
		private string _currentDisplayName;

		// Token: 0x0400029E RID: 670
		private string _currentWorldId;

		// Token: 0x0400029F RID: 671
		private string _pendingWorldId;

		// Token: 0x040002A0 RID: 672
		private int _totalKernellUsers = 0;

		// Token: 0x040002A1 RID: 673
		private int _retryAttempts = 0;

		// Token: 0x040002A2 RID: 674
		private DateTime _lastWorldChangeTime = DateTime.MinValue;

		// Token: 0x040002A3 RID: 675
		private readonly Dictionary<string, Action<string, string, string>> _chatCallbacks = new Dictionary<string, Action<string, string, string>>();

		// Token: 0x040002A4 RID: 676
		private readonly Dictionary<string, Action<string, bool>> _userCheckCallbacks = new Dictionary<string, Action<string, bool>>();

		// Token: 0x040002A5 RID: 677
		private readonly Dictionary<string, KernellNetworkClient.KernellUserInfo> _knownKrnlUsers = new Dictionary<string, KernellNetworkClient.KernellUserInfo>();

		// Token: 0x040002A6 RID: 678
		private readonly HashSet<string> _connectedKrnlUsers = new HashSet<string>();

		// Token: 0x040002A7 RID: 679
		private readonly object _callbackLock = new object();

		// Token: 0x040002A8 RID: 680
		private readonly object _connectionLock = new object();

		// Token: 0x040002A9 RID: 681
		private readonly object _stateLock = new object();

		// Token: 0x040002AA RID: 682
		private readonly object _userCacheLock = new object();

		// Token: 0x040002AB RID: 683
		private Task _worldTransitionTask;

		// Token: 0x040002AC RID: 684
		private Task _connectionTask;

		// Token: 0x0200013C RID: 316
		private class KernellUserInfo
		{
			// Token: 0x17000225 RID: 549
			// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x00044850 File Offset: 0x00042A50
			// (set) Token: 0x06000BE6 RID: 3046 RVA: 0x00044858 File Offset: 0x00042A58
			public bool IsKrnlUser { get; set; }

			// Token: 0x17000226 RID: 550
			// (get) Token: 0x06000BE7 RID: 3047 RVA: 0x00044861 File Offset: 0x00042A61
			// (set) Token: 0x06000BE8 RID: 3048 RVA: 0x00044869 File Offset: 0x00042A69
			public DateTime LastChecked { get; set; }

			// Token: 0x17000227 RID: 551
			// (get) Token: 0x06000BE9 RID: 3049 RVA: 0x00044872 File Offset: 0x00042A72
			// (set) Token: 0x06000BEA RID: 3050 RVA: 0x0004487A File Offset: 0x00042A7A
			public bool IsConnected { get; set; }

			// Token: 0x17000228 RID: 552
			// (get) Token: 0x06000BEB RID: 3051 RVA: 0x00044883 File Offset: 0x00042A83
			// (set) Token: 0x06000BEC RID: 3052 RVA: 0x0004488B File Offset: 0x00042A8B
			public string DisplayName { get; set; }

			// Token: 0x17000229 RID: 553
			// (get) Token: 0x06000BED RID: 3053 RVA: 0x00044894 File Offset: 0x00042A94
			// (set) Token: 0x06000BEE RID: 3054 RVA: 0x0004489C File Offset: 0x00042A9C
			public string WorldId { get; set; }
		}
	}
}
