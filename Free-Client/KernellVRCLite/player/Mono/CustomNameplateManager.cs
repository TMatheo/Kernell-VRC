using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Il2CppSystem.Collections.Generic;
using KernellVRCLite.Core.Mono;
using KernelVRC;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Networking;

namespace KernellVRCLite.player.Mono
{
	// Token: 0x02000084 RID: 132
	public static class CustomNameplateManager
	{
		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x000274AD File Offset: 0x000256AD
		// (set) Token: 0x0600065C RID: 1628 RVA: 0x000274B4 File Offset: 0x000256B4
		public static bool BackgroundSpriteApply { get; set; } = false;

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600065D RID: 1629 RVA: 0x000274BC File Offset: 0x000256BC
		// (set) Token: 0x0600065E RID: 1630 RVA: 0x000274C3 File Offset: 0x000256C3
		public static bool UseKernelSprite { get; set; } = true;

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x0600065F RID: 1631 RVA: 0x000274CB File Offset: 0x000256CB
		// (set) Token: 0x06000660 RID: 1632 RVA: 0x000274D2 File Offset: 0x000256D2
		public static bool Use2018Sprite { get; set; } = false;

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000661 RID: 1633 RVA: 0x000274DA File Offset: 0x000256DA
		// (set) Token: 0x06000662 RID: 1634 RVA: 0x000274E1 File Offset: 0x000256E1
		public static bool EnableFrameColor { get; set; } = false;

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000663 RID: 1635 RVA: 0x000274E9 File Offset: 0x000256E9
		// (set) Token: 0x06000664 RID: 1636 RVA: 0x000274F0 File Offset: 0x000256F0
		public static bool EnablePingColor { get; set; } = false;

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x000274F8 File Offset: 0x000256F8
		// (set) Token: 0x06000666 RID: 1638 RVA: 0x000274FF File Offset: 0x000256FF
		public static bool EnableAsyncProcessing { get; set; } = true;

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000667 RID: 1639 RVA: 0x00027507 File Offset: 0x00025707
		// (set) Token: 0x06000668 RID: 1640 RVA: 0x0002750E File Offset: 0x0002570E
		public static int UpdateIntervalMs { get; set; } = 2000;

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00027516 File Offset: 0x00025716
		// (set) Token: 0x0600066A RID: 1642 RVA: 0x0002751D File Offset: 0x0002571D
		public static int MaxProcessPerFrame { get; set; } = 3;

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600066B RID: 1643 RVA: 0x00027525 File Offset: 0x00025725
		// (set) Token: 0x0600066C RID: 1644 RVA: 0x0002752C File Offset: 0x0002572C
		public static int NameplateRenderDistance { get; set; } = 20;

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x0600066D RID: 1645 RVA: 0x00027534 File Offset: 0x00025734
		// (set) Token: 0x0600066E RID: 1646 RVA: 0x0002753B File Offset: 0x0002573B
		public static int CacheDurationMs { get; set; } = 30000;

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x0600066F RID: 1647 RVA: 0x00027543 File Offset: 0x00025743
		// (set) Token: 0x06000670 RID: 1648 RVA: 0x0002754A File Offset: 0x0002574A
		public static Color NameplateBackgroundColor { get; set; } = new Color(0.1f, 0.1f, 0.1f, 0.85f);

		// Token: 0x06000671 RID: 1649 RVA: 0x00027554 File Offset: 0x00025754
		public static void OnWorldLeaving()
		{
			kernelllogger.Msg("[CustomNameplateManager] World leaving - cancelling all operations");
			object cancellationLock = CustomNameplateManager._cancellationLock;
			lock (cancellationLock)
			{
				CustomNameplateManager._isInWorldTransition = true;
				CustomNameplateManager._processingActive = false;
				CancellationTokenSource globalCancellation = CustomNameplateManager._globalCancellation;
				if (globalCancellation != null)
				{
					globalCancellation.Cancel();
				}
				CancellationTokenSource globalCancellation2 = CustomNameplateManager._globalCancellation;
				if (globalCancellation2 != null)
				{
					globalCancellation2.Dispose();
				}
				CustomNameplateManager._globalCancellation = new CancellationTokenSource();
			}
			Queue<Player> updateQueue = CustomNameplateManager._updateQueue;
			lock (updateQueue)
			{
				CustomNameplateManager._updateQueue.Clear();
				CustomNameplateManager._processingPlayers.Clear();
			}
			CustomNameplateManager._playerCache.Clear();
			CustomNameplate.OnWorldLeaving();
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002762C File Offset: 0x0002582C
		public static void OnWorldEntered()
		{
			kernelllogger.Msg("[CustomNameplateManager] World entered - resetting for new world");
			object cancellationLock = CustomNameplateManager._cancellationLock;
			lock (cancellationLock)
			{
				CustomNameplateManager._isInWorldTransition = false;
				bool isCancellationRequested = CustomNameplateManager._globalCancellation.IsCancellationRequested;
				if (isCancellationRequested)
				{
					CancellationTokenSource globalCancellation = CustomNameplateManager._globalCancellation;
					if (globalCancellation != null)
					{
						globalCancellation.Dispose();
					}
					CustomNameplateManager._globalCancellation = new CancellationTokenSource();
				}
			}
			CustomNameplate.OnWorldEntered();
			kernelllogger.Msg("[CustomNameplateManager] Ready for new world operations");
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x000276BC File Offset: 0x000258BC
		private static CancellationToken GetCancellationToken()
		{
			object cancellationLock = CustomNameplateManager._cancellationLock;
			CancellationToken result;
			lock (cancellationLock)
			{
				CancellationTokenSource globalCancellation = CustomNameplateManager._globalCancellation;
				result = ((globalCancellation != null) ? globalCancellation.Token : CancellationToken.None);
			}
			return result;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x00027710 File Offset: 0x00025910
		static CustomNameplateManager()
		{
			try
			{
				kernelllogger.Msg("[CustomNameplateManager] Initializing...");
				CustomNameplateManager.Initialize();
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Static constructor failed: " + ex.Message);
			}
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x00027864 File Offset: 0x00025A64
		private static void Initialize()
		{
			bool isInitialized = CustomNameplateManager._isInitialized;
			if (!isInitialized)
			{
				try
				{
					MelonCoroutines.Start(CustomNameplateManager.MainProcessingLoop());
					MelonCoroutines.Start(CustomNameplateManager.CacheCleanupLoop());
					MelonCoroutines.Start(CustomNameplateManager.PreloadSpritesCoroutine());
					CustomNameplateManager._isInitialized = true;
					kernelllogger.Msg("[CustomNameplateManager] Initialized successfully");
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[CustomNameplateManager] Initialization failed: " + ex.Message);
				}
			}
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x000278E4 File Offset: 0x00025AE4
		private static IEnumerator PreloadSpritesCoroutine()
		{
			yield return new WaitForSeconds(2f);
			bool isCancellationRequested = CustomNameplateManager.GetCancellationToken().IsCancellationRequested;
			if (isCancellationRequested)
			{
				yield break;
			}
			bool flag = !CustomNameplateManager._spritesPreloaded;
			if (flag)
			{
				try
				{
					CustomNameplateManager._cachedKernelSprite = EmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.Banner-NP-2025.png", null, 100f);
					CustomNameplateManager._cached2018Sprite = EmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.Visitor_2018.png", null, 100f);
					CustomNameplateManager._spritesPreloaded = true;
					kernelllogger.Msg("[CustomNameplateManager] Sprites preloaded");
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					kernelllogger.Error("[CustomNameplateManager] Sprite preload failed: " + ex.Message);
				}
			}
			yield break;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x000278EC File Offset: 0x00025AEC
		public static void Shutdown()
		{
			CustomNameplateManager._isShuttingDown = true;
			CustomNameplateManager._isInitialized = false;
			object cancellationLock = CustomNameplateManager._cancellationLock;
			lock (cancellationLock)
			{
				CancellationTokenSource globalCancellation = CustomNameplateManager._globalCancellation;
				if (globalCancellation != null)
				{
					globalCancellation.Cancel();
				}
				CancellationTokenSource globalCancellation2 = CustomNameplateManager._globalCancellation;
				if (globalCancellation2 != null)
				{
					globalCancellation2.Dispose();
				}
			}
			Queue<Player> updateQueue = CustomNameplateManager._updateQueue;
			lock (updateQueue)
			{
				CustomNameplateManager._updateQueue.Clear();
				CustomNameplateManager._processingPlayers.Clear();
			}
			CustomNameplateManager._playerCache.Clear();
			kernelllogger.Msg("[CustomNameplateManager] Shutdown complete");
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x000279B4 File Offset: 0x00025BB4
		private static IEnumerator MainProcessingLoop()
		{
			kernelllogger.Msg("[CustomNameplateManager] Main processing loop started");
			while (!CustomNameplateManager._isShuttingDown)
			{
				CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					yield return new WaitForSeconds(1f);
				}
				else
				{
					CustomNameplateManager._frameCount++;
					bool flag = CustomNameplateManager._frameCount % 300 == 0;
					if (flag)
					{
						try
						{
							bool flag2 = !cancellationToken.IsCancellationRequested;
							if (flag2)
							{
								CustomNameplateManager.UpdateLocalPlayerReference();
							}
						}
						catch (Exception ex3)
						{
							Exception ex = ex3;
							kernelllogger.Error("[CustomNameplateManager] Error updating local player: " + ex.Message);
						}
					}
					bool flag3 = CustomNameplateManager._frameCount - CustomNameplateManager._lastUpdateFrame >= CustomNameplateManager.UpdateIntervalMs / 16;
					if (flag3)
					{
						CustomNameplateManager._lastUpdateFrame = CustomNameplateManager._frameCount;
						bool flag4 = !CustomNameplateManager._isInWorldTransition && !cancellationToken.IsCancellationRequested;
						if (flag4)
						{
							yield return CustomNameplateManager.ProcessNameplateUpdates();
						}
					}
					bool flag5 = DateTime.Now - CustomNameplateManager._lastStatsLog > TimeSpan.FromMinutes(2.0);
					if (flag5)
					{
						try
						{
							bool flag6 = !cancellationToken.IsCancellationRequested;
							if (flag6)
							{
								CustomNameplateManager.LogPerformanceStats();
								CustomNameplateManager._lastStatsLog = DateTime.Now;
							}
						}
						catch (Exception ex3)
						{
							Exception ex2 = ex3;
							kernelllogger.Error("[CustomNameplateManager] Error logging stats: " + ex2.Message);
						}
					}
					yield return null;
					cancellationToken = default(CancellationToken);
				}
			}
			kernelllogger.Msg("[CustomNameplateManager] Main processing loop stopped");
			yield break;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x000279BC File Offset: 0x00025BBC
		private static IEnumerator ProcessNameplateUpdates()
		{
			bool flag = CustomNameplateManager._processingActive || CustomNameplateManager._isShuttingDown || CustomNameplateManager._isInWorldTransition;
			if (flag)
			{
				yield break;
			}
			CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
			bool isCancellationRequested = cancellationToken.IsCancellationRequested;
			if (isCancellationRequested)
			{
				yield break;
			}
			CustomNameplateManager._processingActive = true;
			try
			{
				List<Player> players = CustomNameplateManager.SafeGetAllPlayers();
				bool flag2 = players == null || players.Count == 0 || cancellationToken.IsCancellationRequested;
				if (flag2)
				{
					CustomNameplateManager._processingActive = false;
					yield break;
				}
				List<Player> eligiblePlayers = CustomNameplateManager.SafeFilterEligiblePlayers(players);
				bool flag3 = eligiblePlayers.Count == 0 || cancellationToken.IsCancellationRequested;
				if (flag3)
				{
					CustomNameplateManager._processingActive = false;
					yield break;
				}
				int processed = 0;
				foreach (Player player in eligiblePlayers)
				{
					bool flag4 = CustomNameplateManager._isShuttingDown || CustomNameplateManager._isInWorldTransition || cancellationToken.IsCancellationRequested;
					if (flag4)
					{
						break;
					}
					bool success = CustomNameplateManager.SafeProcessSinglePlayer(player);
					bool flag5 = success;
					if (flag5)
					{
						int num = processed;
						processed = num + 1;
					}
					bool flag6 = processed % CustomNameplateManager.MaxProcessPerFrame == 0;
					if (flag6)
					{
						yield return null;
						bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
						if (isCancellationRequested2)
						{
							break;
						}
					}
					player = null;
				}
				List<Player>.Enumerator enumerator = default(List<Player>.Enumerator);
				CustomNameplateManager._totalProcessed += processed;
				players = null;
				eligiblePlayers = null;
			}
			finally
			{
				CustomNameplateManager._processingActive = false;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x000279C4 File Offset: 0x00025BC4
		private static void UpdateLocalPlayerReference()
		{
			PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
			bool flag = playerManager != null;
			if (flag)
			{
				CustomNameplateManager._localPlayer = playerManager.field_Private_Player_0;
				bool flag2 = CustomNameplateManager._localPlayer != null;
				if (flag2)
				{
					CustomNameplateManager._lastLocalPosition = CustomNameplateManager._localPlayer.transform.position;
				}
			}
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x00027A14 File Offset: 0x00025C14
		private static List<Player> SafeGetAllPlayers()
		{
			List<Player> result;
			try
			{
				result = CustomNameplateManager.GetAllPlayers();
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error getting players: " + ex.Message);
				result = new List<Player>();
			}
			return result;
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00027A5C File Offset: 0x00025C5C
		private static List<Player> GetAllPlayers()
		{
			List<Player> list = new List<Player>();
			PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
			bool flag = ((playerManager != null) ? playerManager.field_Private_List_1_Player_0 : null) != null;
			if (flag)
			{
				List<Player> field_Private_List_1_Player_ = playerManager.field_Private_List_1_Player_0;
				Il2CppArrayBase<Player> il2CppArrayBase = field_Private_List_1_Player_.ToArray();
				foreach (Player player in il2CppArrayBase)
				{
					bool isCancellationRequested = CustomNameplateManager.GetCancellationToken().IsCancellationRequested;
					if (isCancellationRequested)
					{
						break;
					}
					try
					{
						bool flag2 = ((player != null) ? player.field_Private_APIUser_0 : null) != null;
						if (flag2)
						{
							list.Add(player);
						}
					}
					catch
					{
					}
				}
			}
			return list;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x00027B30 File Offset: 0x00025D30
		private static List<Player> SafeFilterEligiblePlayers(List<Player> players)
		{
			List<Player> result;
			try
			{
				result = CustomNameplateManager.FilterEligiblePlayers(players);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error filtering players: " + ex.Message);
				result = new List<Player>();
			}
			return result;
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x00027B7C File Offset: 0x00025D7C
		private static List<Player> FilterEligiblePlayers(List<Player> players)
		{
			List<Player> list = new List<Player>();
			int tickCount = Environment.TickCount;
			CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
			foreach (Player player in players)
			{
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					break;
				}
				try
				{
					bool flag = player == null || player == CustomNameplateManager._localPlayer;
					if (!flag)
					{
						APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
						string text = (field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null;
						bool flag2 = string.IsNullOrEmpty(text);
						if (!flag2)
						{
							bool flag3 = CustomNameplateManager._localPlayer != null && CustomNameplateManager.NameplateRenderDistance > 0;
							if (flag3)
							{
								float num = Vector3.Distance(CustomNameplateManager._localPlayer.transform.position, player.transform.position);
								bool flag4 = num > (float)CustomNameplateManager.NameplateRenderDistance;
								if (flag4)
								{
									continue;
								}
							}
							CustomNameplateManager.NameplateCache nameplateCache;
							bool flag5 = CustomNameplateManager._playerCache.TryGetValue(text, ref nameplateCache);
							if (flag5)
							{
								bool flag6 = !nameplateCache.ShouldUpdate(tickCount);
								if (flag6)
								{
									CustomNameplateManager._cacheHits++;
									continue;
								}
							}
							bool flag7 = CustomNameplateManager._processingPlayers.Contains(text);
							if (!flag7)
							{
								list.Add(player);
								CustomNameplateManager._cacheMisses++;
							}
						}
					}
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[CustomNameplateManager] Error filtering player: " + ex.Message);
				}
			}
			return list;
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x00027D44 File Offset: 0x00025F44
		private static bool SafeProcessSinglePlayer(Player player)
		{
			bool result;
			try
			{
				CustomNameplateManager.ProcessSinglePlayer(player);
				result = true;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error processing player: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x00027D8C File Offset: 0x00025F8C
		private static void ProcessSinglePlayer(Player player)
		{
			CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
			bool isCancellationRequested = cancellationToken.IsCancellationRequested;
			if (!isCancellationRequested)
			{
				APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
				string text = (field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null;
				bool flag = string.IsNullOrEmpty(text);
				if (!flag)
				{
					CustomNameplateManager._processingPlayers.Add(text);
					try
					{
						CustomNameplateManager.NameplateCache orAdd = CustomNameplateManager._playerCache.GetOrAdd(text, (string _) => new CustomNameplateManager.NameplateCache());
						CustomNameplateManager.NameplateData nameplateData = CustomNameplateManager.CollectNameplateData(player);
						bool flag2 = nameplateData != null && !cancellationToken.IsCancellationRequested;
						if (flag2)
						{
							CustomNameplateManager.ApplyNameplateUpdates(player, nameplateData);
							orAdd.MarkUpdated();
						}
					}
					finally
					{
						CustomNameplateManager._processingPlayers.Remove(text);
					}
				}
			}
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x00027E5C File Offset: 0x0002605C
		private static CustomNameplateManager.NameplateData CollectNameplateData(Player player)
		{
			CustomNameplateManager.NameplateData result;
			try
			{
				bool isCancellationRequested = CustomNameplateManager.GetCancellationToken().IsCancellationRequested;
				if (isCancellationRequested)
				{
					result = null;
				}
				else
				{
					bool flag = ((player != null) ? player.field_Private_APIUser_0 : null) == null;
					if (flag)
					{
						result = null;
					}
					else
					{
						APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
						result = new CustomNameplateManager.NameplateData
						{
							PlayerId = field_Private_APIUser_.id,
							PlayerName = field_Private_APIUser_.displayName,
							TrustColor = CustomNameplateManager.CalculateTrustColor(field_Private_APIUser_.tags),
							FrameRate = (CustomNameplateManager.EnableFrameColor ? CustomNameplateManager.CalculateFrameRate(player) : -1f),
							Ping = (CustomNameplateManager.EnablePingColor ? CustomNameplateManager.CalculatePing(player) : -1)
						};
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error collecting nameplate data: " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x00027F3C File Offset: 0x0002613C
		private static Color CalculateTrustColor(List<string> tags)
		{
			Color result;
			try
			{
				CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					result = Color.white;
				}
				else
				{
					bool flag = tags == null || tags.Count == 0;
					if (flag)
					{
						result = VRCPlayer.field_Internal_Static_Color_2;
					}
					else
					{
						List<string> list = new List<string>();
						Il2CppArrayBase<string> il2CppArrayBase = tags.ToArray();
						foreach (string text in il2CppArrayBase)
						{
							bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
							if (isCancellationRequested2)
							{
								break;
							}
							bool flag2 = text != null && text.StartsWith("system_trust_", StringComparison.OrdinalIgnoreCase);
							if (flag2)
							{
								list.Add(text);
							}
						}
						bool flag3 = list.Count == 0;
						if (flag3)
						{
							result = VRCPlayer.field_Internal_Static_Color_2;
						}
						else
						{
							string key = string.Join("|", list);
							object trustColorLock = CustomNameplateManager._trustColorLock;
							lock (trustColorLock)
							{
								Color result2;
								bool flag5 = CustomNameplateManager._trustColorCache.TryGetValue(key, out result2);
								if (flag5)
								{
									return result2;
								}
							}
							Color color = VRCPlayer.field_Internal_Static_Color_2;
							foreach (string text2 in list)
							{
								bool isCancellationRequested3 = cancellationToken.IsCancellationRequested;
								if (isCancellationRequested3)
								{
									break;
								}
								string text3 = text2.ToLower();
								string a = text3;
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
													color = VRCPlayer.field_Internal_Static_Color_3;
												}
											}
											else
											{
												color = VRCPlayer.field_Internal_Static_Color_4;
											}
										}
										else
										{
											color = VRCPlayer.field_Internal_Static_Color_5;
										}
									}
									else
									{
										color = VRCPlayer.field_Internal_Static_Color_6;
									}
								}
								else
								{
									color = new Color(1f, 0.255f, 0.294f);
								}
							}
							bool flag6 = !cancellationToken.IsCancellationRequested;
							if (flag6)
							{
								object trustColorLock2 = CustomNameplateManager._trustColorLock;
								lock (trustColorLock2)
								{
									bool flag8 = CustomNameplateManager._trustColorCache.Count < 100;
									if (flag8)
									{
										CustomNameplateManager._trustColorCache[key] = color;
									}
								}
							}
							result = color;
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error calculating trust color: " + ex.Message);
				result = Color.white;
			}
			return result;
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x00028244 File Offset: 0x00026444
		private static float CalculateFrameRate(Player player)
		{
			float result;
			try
			{
				bool isCancellationRequested = CustomNameplateManager.GetCancellationToken().IsCancellationRequested;
				if (isCancellationRequested)
				{
					result = -1f;
				}
				else
				{
					VRCPlayer vrcplayer = player._vrcplayer;
					FlatBufferNetworkSerializer flatBufferNetworkSerializer = (vrcplayer != null) ? vrcplayer.Method_Internal_get_FlatBufferNetworkSerializer_0() : null;
					bool flag = flatBufferNetworkSerializer == null;
					if (flag)
					{
						result = -1f;
					}
					else
					{
						byte field_Private_Byte_ = flatBufferNetworkSerializer.field_Private_Byte_0;
						result = ((field_Private_Byte_ == 0) ? -1f : (1000f / (float)field_Private_Byte_));
					}
				}
			}
			catch
			{
				result = -1f;
			}
			return result;
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x000282D0 File Offset: 0x000264D0
		private static int CalculatePing(Player player)
		{
			int result;
			try
			{
				bool isCancellationRequested = CustomNameplateManager.GetCancellationToken().IsCancellationRequested;
				if (isCancellationRequested)
				{
					result = -1;
				}
				else
				{
					VRCPlayer vrcplayer = player._vrcplayer;
					int? num;
					if (vrcplayer == null)
					{
						num = null;
					}
					else
					{
						FlatBufferNetworkSerializer flatBufferNetworkSerializer = vrcplayer.Method_Internal_get_FlatBufferNetworkSerializer_0();
						num = ((flatBufferNetworkSerializer != null) ? new byte?(flatBufferNetworkSerializer.field_Private_Byte_1) : null);
					}
					result = (num ?? -1);
				}
			}
			catch
			{
				result = -1;
			}
			return result;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00028358 File Offset: 0x00026558
		private static void ApplyNameplateUpdates(Player player, CustomNameplateManager.NameplateData data)
		{
			try
			{
				CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (!isCancellationRequested)
				{
					VRCPlayer vrcplayer = player._vrcplayer;
					PlayerNameplate playerNameplate = (vrcplayer != null) ? vrcplayer.field_Public_PlayerNameplate_0 : null;
					bool flag = playerNameplate == null;
					if (!flag)
					{
						playerNameplate.field_Public_Color_0 = data.TrustColor;
						bool flag2 = playerNameplate.field_Public_GameObject_0 != null && !cancellationToken.IsCancellationRequested;
						if (flag2)
						{
							CustomNameplateManager.ApplyBackgroundSettings(playerNameplate.field_Public_GameObject_0.transform);
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error applying nameplate updates: " + ex.Message);
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00028410 File Offset: 0x00026610
		private static IEnumerator CacheCleanupLoop()
		{
			while (!CustomNameplateManager._isShuttingDown)
			{
				yield return new WaitForSeconds(30f);
				CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (isCancellationRequested)
				{
					yield return new WaitForSeconds(5f);
				}
				else
				{
					try
					{
						CustomNameplateManager.CleanupExpiredCache();
					}
					catch (Exception ex2)
					{
						Exception ex = ex2;
						kernelllogger.Error("[CustomNameplateManager] Cache cleanup error: " + ex.Message);
					}
					cancellationToken = default(CancellationToken);
				}
			}
			yield break;
		}

		// Token: 0x06000687 RID: 1671 RVA: 0x00028418 File Offset: 0x00026618
		private static void CleanupExpiredCache()
		{
			try
			{
				CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
				bool isCancellationRequested = cancellationToken.IsCancellationRequested;
				if (!isCancellationRequested)
				{
					int tickCount = Environment.TickCount;
					List<string> list = new List<string>();
					foreach (KeyValuePair<string, CustomNameplateManager.NameplateCache> keyValuePair in CustomNameplateManager._playerCache)
					{
						bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
						if (isCancellationRequested2)
						{
							break;
						}
						bool flag = keyValuePair.Value.IsExpired(tickCount);
						if (flag)
						{
							list.Add(keyValuePair.Key);
						}
					}
					foreach (string text in list)
					{
						bool isCancellationRequested3 = cancellationToken.IsCancellationRequested;
						if (isCancellationRequested3)
						{
							break;
						}
						CustomNameplateManager.NameplateCache nameplateCache;
						CustomNameplateManager._playerCache.TryRemove(text, ref nameplateCache);
					}
					bool flag2 = list.Count > 0;
					if (flag2)
					{
						kernelllogger.Msg(string.Format("[CustomNameplateManager] Cleaned {0} expired cache entries", list.Count));
					}
					object trustColorLock = CustomNameplateManager._trustColorLock;
					lock (trustColorLock)
					{
						bool flag4 = CustomNameplateManager._trustColorCache.Count > 200;
						if (flag4)
						{
							CustomNameplateManager._trustColorCache.Clear();
							kernelllogger.Msg("[CustomNameplateManager] Cleared trust color cache");
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error in cache cleanup: " + ex.Message);
			}
		}

		// Token: 0x06000688 RID: 1672 RVA: 0x00028600 File Offset: 0x00026800
		public static void ApplyBackgroundSettings(Transform nameplateTransform)
		{
			try
			{
				bool isCancellationRequested = CustomNameplateManager.GetCancellationToken().IsCancellationRequested;
				if (!isCancellationRequested)
				{
					ImageThreeSlice component = nameplateTransform.GetComponent<ImageThreeSlice>();
					bool flag = component == null;
					if (!flag)
					{
						bool flag2 = CustomNameplateManager.BackgroundSpriteApply && CustomNameplateManager._spritesPreloaded;
						if (flag2)
						{
							bool flag3 = CustomNameplateManager.UseKernelSprite && CustomNameplateManager._cachedKernelSprite != null;
							if (flag3)
							{
								component._sprite = CustomNameplateManager._cachedKernelSprite;
							}
							else
							{
								bool flag4 = CustomNameplateManager.Use2018Sprite && CustomNameplateManager._cached2018Sprite != null;
								if (flag4)
								{
									component._sprite = CustomNameplateManager._cached2018Sprite;
								}
							}
						}
						else
						{
							component.color = CustomNameplateManager.NameplateBackgroundColor;
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error applying background: " + ex.Message);
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x000286E8 File Offset: 0x000268E8
		public static IEnumerator ApplyNameplateCustomizations(Player player, bool bypass = false, float time = 10f)
		{
			bool flag = player == null || CustomNameplateManager._isShuttingDown;
			if (flag)
			{
				yield break;
			}
			CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
			bool isCancellationRequested = cancellationToken.IsCancellationRequested;
			if (isCancellationRequested)
			{
				yield break;
			}
			bool flag2 = !bypass;
			if (flag2)
			{
				yield return new WaitForSecondsRealtime(time);
			}
			bool isCancellationRequested2 = cancellationToken.IsCancellationRequested;
			if (isCancellationRequested2)
			{
				yield break;
			}
			try
			{
				CustomNameplateManager.QueuePlayerForUpdate(player);
				yield break;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				kernelllogger.Error("[CustomNameplateManager] Error in ApplyNameplateCustomizations: " + ex.Message);
				yield break;
			}
			yield break;
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00028708 File Offset: 0x00026908
		public static void QueuePlayerForUpdate(Player player)
		{
			CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
			bool flag = ((player != null) ? player.field_Private_APIUser_0 : null) == null || CustomNameplateManager._isShuttingDown || cancellationToken.IsCancellationRequested;
			if (!flag)
			{
				try
				{
					string id = player.field_Private_APIUser_0.id;
					bool flag2 = string.IsNullOrEmpty(id);
					if (!flag2)
					{
						bool flag3 = !CustomNameplateManager._processingPlayers.Contains(id);
						if (flag3)
						{
							Queue<Player> updateQueue = CustomNameplateManager._updateQueue;
							lock (updateQueue)
							{
								CustomNameplateManager._updateQueue.Enqueue(player);
							}
						}
					}
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[CustomNameplateManager] Error queuing player: " + ex.Message);
				}
			}
		}

		// Token: 0x0600068B RID: 1675 RVA: 0x000287E4 File Offset: 0x000269E4
		public static void ApplyNameplateCustomizationsToAll()
		{
			CancellationToken cancellationToken = CustomNameplateManager.GetCancellationToken();
			bool flag = CustomNameplateManager._isShuttingDown || cancellationToken.IsCancellationRequested;
			if (!flag)
			{
				kernelllogger.Msg("[CustomNameplateManager] Queuing all players for nameplate updates");
				try
				{
					List<Player> allPlayers = CustomNameplateManager.GetAllPlayers();
					foreach (Player player in allPlayers)
					{
						bool isCancellationRequested = cancellationToken.IsCancellationRequested;
						if (isCancellationRequested)
						{
							break;
						}
						CustomNameplateManager.QueuePlayerForUpdate(player);
					}
					kernelllogger.Msg(string.Format("[CustomNameplateManager] Queued {0} players for updates", allPlayers.Count));
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[CustomNameplateManager] Error in ApplyNameplateCustomizationsToAll: " + ex.Message);
				}
			}
		}

		// Token: 0x0600068C RID: 1676 RVA: 0x000288C4 File Offset: 0x00026AC4
		public static void LogPerformanceStats()
		{
			try
			{
				float num = (CustomNameplateManager._cacheMisses > 0) ? ((float)CustomNameplateManager._cacheHits * 100f / (float)(CustomNameplateManager._cacheHits + CustomNameplateManager._cacheMisses)) : 0f;
				kernelllogger.Msg("[CustomNameplateManager] Performance Stats:");
				kernelllogger.Msg(string.Format("  Total Processed: {0}", CustomNameplateManager._totalProcessed));
				kernelllogger.Msg(string.Format("  Cache Efficiency: {0:F1}% ({1} hits, {2} misses)", num, CustomNameplateManager._cacheHits, CustomNameplateManager._cacheMisses));
				kernelllogger.Msg(string.Format("  Cache Entries: {0}", CustomNameplateManager._playerCache.Count));
				kernelllogger.Msg(string.Format("  Currently Processing: {0}", CustomNameplateManager._processingPlayers.Count));
				kernelllogger.Msg(string.Format("  Queue Size: {0}", CustomNameplateManager._updateQueue.Count));
				kernelllogger.Msg(string.Format("  Async Processing: {0}", CustomNameplateManager.EnableAsyncProcessing));
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateManager] Error logging stats: " + ex.Message);
			}
		}

		// Token: 0x040002F3 RID: 755
		private static Sprite _cachedKernelSprite;

		// Token: 0x040002F4 RID: 756
		private static Sprite _cached2018Sprite;

		// Token: 0x040002F5 RID: 757
		private static bool _spritesPreloaded = false;

		// Token: 0x040002F6 RID: 758
		private static readonly ConcurrentDictionary<string, CustomNameplateManager.NameplateCache> _playerCache = new ConcurrentDictionary<string, CustomNameplateManager.NameplateCache>();

		// Token: 0x040002F7 RID: 759
		private static readonly Queue<Player> _updateQueue = new Queue<Player>();

		// Token: 0x040002F8 RID: 760
		private static readonly HashSet<string> _processingPlayers = new HashSet<string>();

		// Token: 0x040002F9 RID: 761
		private static volatile bool _isInitialized = false;

		// Token: 0x040002FA RID: 762
		private static volatile bool _isShuttingDown = false;

		// Token: 0x040002FB RID: 763
		private static volatile bool _processingActive = false;

		// Token: 0x040002FC RID: 764
		private static volatile bool _isInWorldTransition = false;

		// Token: 0x040002FD RID: 765
		private static Player _localPlayer;

		// Token: 0x040002FE RID: 766
		private static Vector3 _lastLocalPosition = Vector3.zero;

		// Token: 0x040002FF RID: 767
		private static int _frameCount = 0;

		// Token: 0x04000300 RID: 768
		private static int _lastUpdateFrame = 0;

		// Token: 0x04000301 RID: 769
		private static int _totalProcessed = 0;

		// Token: 0x04000302 RID: 770
		private static int _cacheHits = 0;

		// Token: 0x04000303 RID: 771
		private static int _cacheMisses = 0;

		// Token: 0x04000304 RID: 772
		private static DateTime _lastStatsLog = DateTime.MinValue;

		// Token: 0x04000305 RID: 773
		private static readonly Dictionary<string, Color> _trustColorCache = new Dictionary<string, Color>(32);

		// Token: 0x04000306 RID: 774
		private static readonly object _trustColorLock = new object();

		// Token: 0x04000307 RID: 775
		private static CancellationTokenSource _globalCancellation = new CancellationTokenSource();

		// Token: 0x04000308 RID: 776
		private static readonly object _cancellationLock = new object();

		// Token: 0x02000155 RID: 341
		private class NameplateData
		{
			// Token: 0x1700022A RID: 554
			// (get) Token: 0x06000C3A RID: 3130 RVA: 0x00048CD0 File Offset: 0x00046ED0
			// (set) Token: 0x06000C3B RID: 3131 RVA: 0x00048CD8 File Offset: 0x00046ED8
			public string PlayerId { get; set; }

			// Token: 0x1700022B RID: 555
			// (get) Token: 0x06000C3C RID: 3132 RVA: 0x00048CE1 File Offset: 0x00046EE1
			// (set) Token: 0x06000C3D RID: 3133 RVA: 0x00048CE9 File Offset: 0x00046EE9
			public string PlayerName { get; set; }

			// Token: 0x1700022C RID: 556
			// (get) Token: 0x06000C3E RID: 3134 RVA: 0x00048CF2 File Offset: 0x00046EF2
			// (set) Token: 0x06000C3F RID: 3135 RVA: 0x00048CFA File Offset: 0x00046EFA
			public Color TrustColor { get; set; }

			// Token: 0x1700022D RID: 557
			// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00048D03 File Offset: 0x00046F03
			// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00048D0B File Offset: 0x00046F0B
			public float FrameRate { get; set; } = -1f;

			// Token: 0x1700022E RID: 558
			// (get) Token: 0x06000C42 RID: 3138 RVA: 0x00048D14 File Offset: 0x00046F14
			// (set) Token: 0x06000C43 RID: 3139 RVA: 0x00048D1C File Offset: 0x00046F1C
			public int Ping { get; set; } = -1;
		}

		// Token: 0x02000156 RID: 342
		private class NameplateCache
		{
			// Token: 0x06000C45 RID: 3141 RVA: 0x00048D40 File Offset: 0x00046F40
			public bool ShouldUpdate(int currentTime)
			{
				return currentTime - this._lastUpdateTime > CustomNameplateManager.UpdateIntervalMs;
			}

			// Token: 0x06000C46 RID: 3142 RVA: 0x00048D61 File Offset: 0x00046F61
			public void MarkUpdated()
			{
				this._lastUpdateTime = Environment.TickCount;
			}

			// Token: 0x06000C47 RID: 3143 RVA: 0x00048D70 File Offset: 0x00046F70
			public bool IsExpired(int currentTime)
			{
				return currentTime - this._lastUpdateTime > CustomNameplateManager.CacheDurationMs;
			}

			// Token: 0x040007EB RID: 2027
			private int _lastUpdateTime;
		}
	}
}
