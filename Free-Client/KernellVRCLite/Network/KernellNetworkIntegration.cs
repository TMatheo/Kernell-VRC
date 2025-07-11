using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KernelVRC;
using MelonLoader;
using UnityEngine;

namespace KernellVRCLite.Network
{
	// Token: 0x02000080 RID: 128
	public static class KernellNetworkIntegration
	{
		// Token: 0x060005EA RID: 1514 RVA: 0x00024DC8 File Offset: 0x00022FC8
		public static void Initialize()
		{
			try
			{
				bool isInitialized = KernellNetworkIntegration._isInitialized;
				if (isInitialized)
				{
					kernelllogger.Warning("[KernellNetworkIntegration] Already initialized");
				}
				else
				{
					kernelllogger.Msg("[KernellNetworkIntegration] Initializing Kernell Network Integration...");
					bool flag = KernellNetworkClient.Initialize();
					bool flag2 = flag;
					if (flag2)
					{
						bool flag3 = KernellNetworkClient.Instance != null;
						if (flag3)
						{
							KernellNetworkClient.Instance.RegisterChatMessageCallback(new Action<string, string, string>(KernellNetworkIntegration.HandleNetworkMessage));
							KernellNetworkClient.Instance.OnConnectionStateChanged += KernellNetworkIntegration.HandleConnectionStateChanged;
							KernellNetworkClient.Instance.OnUserCheckResult += KernellNetworkIntegration.HandleUserCheckResult;
							KernellNetworkClient.Instance.OnForceClose += KernellNetworkIntegration.HandleForceClose;
						}
						KernellNetworkIntegration._isInitialized = true;
						kernelllogger.Msg("[KernellNetworkIntegration] Integration initialized successfully");
					}
					else
					{
						kernelllogger.Error("[KernellNetworkIntegration] Failed to initialize network client");
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] Initialization error: " + ex.Message);
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x00024EC4 File Offset: 0x000230C4
		private static void HandleForceClose(string reason, bool isPermanent)
		{
			try
			{
				kernelllogger.Error(string.Format("[KernellNetworkIntegration] Force close received: {0} (Permanent: {1})", reason, isPermanent));
				if (isPermanent)
				{
					kernelllogger.Error("[KernellNetworkIntegration] PERMANENT BAN - Game will exit");
					MelonLogger.Error("KERNELL NETWORK", new object[]
					{
						"You have been permanently banned from the Kernell Network."
					});
					MelonLogger.Error("REASON", new object[]
					{
						reason
					});
				}
				else
				{
					kernelllogger.Warning("[KernellNetworkIntegration] KICKED - " + reason);
					MelonLogger.Warning("KERNELL NETWORK", new object[]
					{
						"You have been kicked from the Kernell Network."
					});
					MelonLogger.Warning("REASON", new object[]
					{
						reason
					});
					MelonLogger.Warning("ACTION", new object[]
					{
						"Game will close. You may restart to reconnect."
					});
				}
				Task.Delay(3000).ContinueWith(delegate(Task _)
				{
					try
					{
						Application.Quit();
						Process.GetCurrentProcess().Kill();
					}
					catch (Exception ex2)
					{
						kernelllogger.Error("[KernellNetworkIntegration] Error forcing game close: " + ex2.Message);
					}
				});
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] HandleForceClose error: " + ex.Message);
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x00024FE4 File Offset: 0x000231E4
		public static void OnWorldLeaving()
		{
			try
			{
				kernelllogger.Msg("[KernellNetworkIntegration] World leaving detected");
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				if (instance != null)
				{
					instance.OnWorldLeaving();
				}
				object userCacheLock = KernellNetworkIntegration._userCacheLock;
				lock (userCacheLock)
				{
					KernellNetworkIntegration._kernellUserCache.Clear();
					kernelllogger.Msg("[KernellNetworkIntegration] User cache cleared for world transition");
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] OnWorldLeaving error: " + ex.Message);
			}
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x00025080 File Offset: 0x00023280
		public static void OnWorldEntered(string worldId)
		{
			try
			{
				kernelllogger.Msg("[KernellNetworkIntegration] World entered: " + worldId);
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				if (instance != null)
				{
					instance.OnWorldEntered(worldId);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] OnWorldEntered error: " + ex.Message);
			}
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x000250E0 File Offset: 0x000232E0
		public static bool IsConnected()
		{
			bool result;
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				result = (instance != null && instance.IsConnected());
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002511C File Offset: 0x0002331C
		public static bool IsInWorldTransition()
		{
			bool result;
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				result = (instance != null && instance.IsInWorldTransition);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00025158 File Offset: 0x00023358
		public static bool IsPermanentlyBlocked()
		{
			bool result;
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				result = (instance != null && instance.IsPermanentlyBlocked);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x00025194 File Offset: 0x00023394
		[DebuggerStepThrough]
		public static Task<bool> ConnectAsync()
		{
			KernellNetworkIntegration.<ConnectAsync>d__13 <ConnectAsync>d__ = new KernellNetworkIntegration.<ConnectAsync>d__13();
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<ConnectAsync>d__.<>1__state = -1;
			<ConnectAsync>d__.<>t__builder.Start<KernellNetworkIntegration.<ConnectAsync>d__13>(ref <ConnectAsync>d__);
			return <ConnectAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x000251D4 File Offset: 0x000233D4
		public static void ForceDisconnect()
		{
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				if (instance != null)
				{
					instance.ForceDisconnect();
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] ForceDisconnect error: " + ex.Message);
			}
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x00025224 File Offset: 0x00023424
		public static void AllowReconnection()
		{
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				if (instance != null)
				{
					instance.AllowReconnection();
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] AllowReconnection error: " + ex.Message);
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00025274 File Offset: 0x00023474
		public static void SendChatMessage(string message)
		{
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				if (instance != null)
				{
					instance.SendChatMessage(message);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] Send chat message error: " + ex.Message);
			}
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x000252C4 File Offset: 0x000234C4
		public static string RegisterChatMessageCallback(Action<string, string, string> callback)
		{
			string result;
			try
			{
				bool flag = callback == null;
				if (flag)
				{
					result = null;
				}
				else
				{
					string text = Guid.NewGuid().ToString();
					object callbackLock = KernellNetworkIntegration._callbackLock;
					lock (callbackLock)
					{
						KernellNetworkIntegration._chatCallbacks[text] = callback;
					}
					KernellNetworkClient instance = KernellNetworkClient.Instance;
					if (instance != null)
					{
						instance.RegisterChatMessageCallback(callback);
					}
					result = text;
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] Register callback error: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x00025378 File Offset: 0x00023578
		public static void UnregisterChatMessageCallback(Action<string, string, string> callback)
		{
			try
			{
				bool flag = callback == null;
				if (!flag)
				{
					object callbackLock = KernellNetworkIntegration._callbackLock;
					lock (callbackLock)
					{
						List<string> list = new List<string>();
						foreach (KeyValuePair<string, Action<string, string, string>> keyValuePair in KernellNetworkIntegration._chatCallbacks)
						{
							bool flag3 = keyValuePair.Value == callback;
							if (flag3)
							{
								list.Add(keyValuePair.Key);
							}
						}
						foreach (string key in list)
						{
							KernellNetworkIntegration._chatCallbacks.Remove(key);
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] Unregister callback error: " + ex.Message);
			}
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x000254A0 File Offset: 0x000236A0
		public static void UpdateWorld(string worldId)
		{
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				if (instance != null)
				{
					instance.UpdateWorld(worldId);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] Update world error: " + ex.Message);
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x000254F0 File Offset: 0x000236F0
		public static bool IsKernellUser(string userId)
		{
			bool result;
			try
			{
				bool flag = string.IsNullOrEmpty(userId);
				if (flag)
				{
					result = false;
				}
				else
				{
					object userCacheLock = KernellNetworkIntegration._userCacheLock;
					lock (userCacheLock)
					{
						bool result2;
						bool flag3 = KernellNetworkIntegration._kernellUserCache.TryGetValue(userId, out result2);
						if (flag3)
						{
							return result2;
						}
					}
					bool flag4 = KernellNetworkClient.Instance != null;
					if (flag4)
					{
						bool flag5 = KernellNetworkClient.Instance.IsKernellUser(userId);
						bool flag6 = flag5;
						if (flag6)
						{
							object userCacheLock2 = KernellNetworkIntegration._userCacheLock;
							lock (userCacheLock2)
							{
								KernellNetworkIntegration._kernellUserCache[userId] = true;
							}
							return true;
						}
					}
					bool flag8 = KernellNetworkIntegration.IsConnected();
					if (flag8)
					{
						KernellNetworkIntegration.CheckKernellUserAsync(userId);
					}
					result = false;
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] IsKernellUser error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x00025608 File Offset: 0x00023808
		[DebuggerStepThrough]
		public static Task<bool> CheckKernellUserAsync(string userId)
		{
			KernellNetworkIntegration.<CheckKernellUserAsync>d__21 <CheckKernellUserAsync>d__ = new KernellNetworkIntegration.<CheckKernellUserAsync>d__21();
			<CheckKernellUserAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<CheckKernellUserAsync>d__.userId = userId;
			<CheckKernellUserAsync>d__.<>1__state = -1;
			<CheckKernellUserAsync>d__.<>t__builder.Start<KernellNetworkIntegration.<CheckKernellUserAsync>d__21>(ref <CheckKernellUserAsync>d__);
			return <CheckKernellUserAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002564C File Offset: 0x0002384C
		public static void InvalidateUserCache(string userId = null)
		{
			try
			{
				object userCacheLock = KernellNetworkIntegration._userCacheLock;
				lock (userCacheLock)
				{
					bool flag2 = string.IsNullOrEmpty(userId);
					if (flag2)
					{
						KernellNetworkIntegration._kernellUserCache.Clear();
						kernelllogger.Msg("[KernellNetworkIntegration] Entire user cache cleared");
					}
					else
					{
						bool flag3 = KernellNetworkIntegration._kernellUserCache.Remove(userId);
						if (flag3)
						{
							kernelllogger.Msg("[KernellNetworkIntegration] User cache entry removed: " + userId);
						}
					}
				}
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				if (instance != null)
				{
					instance.ClearUserCache();
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] InvalidateUserCache error: " + ex.Message);
			}
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00025710 File Offset: 0x00023910
		public static string GetUserWorldId(string userId)
		{
			string result;
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				result = ((instance != null) ? instance.GetUserWorldId(userId) : null);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] GetUserWorldId error: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00025764 File Offset: 0x00023964
		private static void HandleNetworkMessage(string userId, string displayName, string message)
		{
			try
			{
				bool flag = !string.IsNullOrEmpty(userId);
				if (flag)
				{
					object userCacheLock = KernellNetworkIntegration._userCacheLock;
					lock (userCacheLock)
					{
						KernellNetworkIntegration._kernellUserCache[userId] = true;
					}
				}
				object callbackLock = KernellNetworkIntegration._callbackLock;
				lock (callbackLock)
				{
					foreach (Action<string, string, string> action in KernellNetworkIntegration._chatCallbacks.Values)
					{
						try
						{
							action(userId, displayName, message);
						}
						catch (Exception ex)
						{
							kernelllogger.Error("[KernellNetworkIntegration] Error in chat callback: " + ex.Message);
						}
					}
				}
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("[KernellNetworkIntegration] HandleNetworkMessage error: " + ex2.Message);
			}
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00025898 File Offset: 0x00023A98
		private static void HandleConnectionStateChanged(bool connected)
		{
			try
			{
				kernelllogger.Msg("[KernellNetworkIntegration] Connection state changed: " + (connected ? "Connected" : "Disconnected"));
				bool flag = !connected;
				if (flag)
				{
					KernellNetworkIntegration.InvalidateUserCache(null);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] HandleConnectionStateChanged error: " + ex.Message);
			}
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00025908 File Offset: 0x00023B08
		private static void HandleUserCheckResult(string userId, bool isKernellUser)
		{
			try
			{
				object userCacheLock = KernellNetworkIntegration._userCacheLock;
				lock (userCacheLock)
				{
					KernellNetworkIntegration._kernellUserCache[userId] = isKernellUser;
					TaskCompletionSource<bool> taskCompletionSource;
					bool flag2 = KernellNetworkIntegration._pendingUserChecks.TryGetValue(userId, out taskCompletionSource);
					if (flag2)
					{
						KernellNetworkIntegration._pendingUserChecks.Remove(userId);
						taskCompletionSource.TrySetResult(isKernellUser);
					}
				}
				kernelllogger.Msg(string.Format("[KernellNetworkIntegration] User check result: {0} -> {1}", userId, isKernellUser));
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] HandleUserCheckResult error: " + ex.Message);
			}
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x000259C0 File Offset: 0x00023BC0
		public static NetworkStatus GetStatus()
		{
			NetworkStatus result;
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				NetworkStatus networkStatus;
				if ((networkStatus = ((instance != null) ? instance.GetNetworkStatus() : null)) == null)
				{
					NetworkStatus networkStatus2 = new NetworkStatus();
					networkStatus2.IsConnected = false;
					networkStatus2.IsConnecting = false;
					networkStatus = networkStatus2;
					networkStatus2.ServerUrl = "wss://rtcs.kernell.net/ws";
				}
				NetworkStatus networkStatus3 = networkStatus;
				result = networkStatus3;
			}
			catch
			{
				result = new NetworkStatus
				{
					IsConnected = false,
					IsConnecting = false,
					ServerUrl = "wss://rtcs.kernell.net/ws"
				};
			}
			return result;
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00025A44 File Offset: 0x00023C44
		public static int GetCachedUserCount()
		{
			int result;
			try
			{
				object userCacheLock = KernellNetworkIntegration._userCacheLock;
				lock (userCacheLock)
				{
					int num = 0;
					foreach (bool flag2 in KernellNetworkIntegration._kernellUserCache.Values)
					{
						bool flag3 = flag2;
						if (flag3)
						{
							num++;
						}
					}
					result = num;
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x00025AF0 File Offset: 0x00023CF0
		public static int GetConnectedUserCount()
		{
			int result;
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				result = ((instance != null) ? instance.GetConnectedUserCount() : 0);
			}
			catch
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x00025B2C File Offset: 0x00023D2C
		public static bool IsUserCached(string userId)
		{
			bool result;
			try
			{
				bool flag = string.IsNullOrEmpty(userId);
				if (flag)
				{
					result = false;
				}
				else
				{
					object userCacheLock = KernellNetworkIntegration._userCacheLock;
					lock (userCacheLock)
					{
						result = KernellNetworkIntegration._kernellUserCache.ContainsKey(userId);
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x00025B98 File Offset: 0x00023D98
		public static string GetCurrentWorldId()
		{
			string result;
			try
			{
				KernellNetworkClient instance = KernellNetworkClient.Instance;
				result = ((instance != null) ? instance.CurrentWorldId : null);
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x00025BD4 File Offset: 0x00023DD4
		public static void Shutdown()
		{
			try
			{
				kernelllogger.Msg("[KernellNetworkIntegration] Shutting down...");
				object callbackLock = KernellNetworkIntegration._callbackLock;
				lock (callbackLock)
				{
					KernellNetworkIntegration._chatCallbacks.Clear();
				}
				object userCacheLock = KernellNetworkIntegration._userCacheLock;
				lock (userCacheLock)
				{
					KernellNetworkIntegration._kernellUserCache.Clear();
					foreach (TaskCompletionSource<bool> taskCompletionSource in KernellNetworkIntegration._pendingUserChecks.Values)
					{
						taskCompletionSource.SetResult(false);
					}
					KernellNetworkIntegration._pendingUserChecks.Clear();
				}
				bool flag3 = KernellNetworkClient.Instance != null;
				if (flag3)
				{
					KernellNetworkClient.Instance.OnConnectionStateChanged -= KernellNetworkIntegration.HandleConnectionStateChanged;
					KernellNetworkClient.Instance.OnUserCheckResult -= KernellNetworkIntegration.HandleUserCheckResult;
					KernellNetworkClient.Instance.OnForceClose -= KernellNetworkIntegration.HandleForceClose;
				}
				KernellNetworkIntegration._isInitialized = false;
				kernelllogger.Msg("[KernellNetworkIntegration] Shutdown complete");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellNetworkIntegration] Shutdown error: " + ex.Message);
			}
		}

		// Token: 0x040002AD RID: 685
		private static readonly Dictionary<string, Action<string, string, string>> _chatCallbacks = new Dictionary<string, Action<string, string, string>>();

		// Token: 0x040002AE RID: 686
		private static readonly Dictionary<string, bool> _kernellUserCache = new Dictionary<string, bool>();

		// Token: 0x040002AF RID: 687
		private static readonly Dictionary<string, TaskCompletionSource<bool>> _pendingUserChecks = new Dictionary<string, TaskCompletionSource<bool>>();

		// Token: 0x040002B0 RID: 688
		private static readonly object _callbackLock = new object();

		// Token: 0x040002B1 RID: 689
		private static readonly object _userCacheLock = new object();

		// Token: 0x040002B2 RID: 690
		private static bool _isInitialized = false;
	}
}
