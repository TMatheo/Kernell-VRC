using System;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.UI.QuickMenu;
using KernellVRC.Utils;
using KernellVRCLite.Utils;
using KernelVRC;
using UnityEngine;
using VRC;
using VRC.Core;

namespace KernellVRC.Modules
{
	// Token: 0x02000076 RID: 118
	internal class BoxOutlineESP : KernelModuleBase
	{
		// Token: 0x1700010C RID: 268
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x00007F31 File Offset: 0x00006131
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x00003312 File Offset: 0x00001512
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every2Frames;
			}
		}

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001FDBC File Offset: 0x0001DFBC
		public override string ModuleName
		{
			get
			{
				return "Box Outline ESP";
			}
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001FDC4 File Offset: 0x0001DFC4
		public override void OnUiManagerInit()
		{
			try
			{
				ReMenuCategory category = MenuSetup._uiManager.QMMenu.GetCategoryPage("Utility").GetCategory("ESP");
				category.AddToggle("Box ESP", "Show player outlines with boxes", new Action<bool>(this.ToggleESP), this._espEnabled, "#ffffff");
				category.AddToggle("Box Rank Colors", "Color boxes by player rank", new Action<bool>(this.ToggleRankColors), this._useRankColors, "#ffffff");
				category.AddToggle("Box Distance", "Show distance on boxes", new Action<bool>(this.ToggleDistance), this._showDistance, "#ffffff");
				kernelllogger.Msg("[BoxESP] System initialized");
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[BoxESP] Initialization failed: {0}", arg));
			}
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001FEA0 File Offset: 0x0001E0A0
		private void ToggleESP(bool state)
		{
			this._espEnabled = state;
			bool flag = !state;
			if (flag)
			{
				this._playerCache.Clear();
				this._cachedPlayers = null;
			}
			kernelllogger.Msg("[BoxESP] " + (state ? "Enabled" : "Disabled"));
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001FEF1 File Offset: 0x0001E0F1
		private void ToggleRankColors(bool state)
		{
			this._useRankColors = state;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001FEFB File Offset: 0x0001E0FB
		private void ToggleDistance(bool state)
		{
			this._showDistance = state;
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001FF05 File Offset: 0x0001E105
		public override void OnEnterWorld(ApiWorld world, ApiWorldInstance instance)
		{
			this._playerCache.Clear();
			this._cachedPlayers = null;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001FF1C File Offset: 0x0001E11C
		public override void OnPlayerJoined(Player player)
		{
			bool flag = this._espEnabled && this.IsValidPlayer(player);
			if (flag)
			{
				this.RefreshPlayerCache();
			}
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001FF4C File Offset: 0x0001E14C
		public override void OnPlayerLeft(Player player)
		{
			bool flag = ((player != null) ? player.field_Private_APIUser_0 : null) != null;
			if (flag)
			{
				this._playerCache.Remove(player.field_Private_APIUser_0.id);
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001FF88 File Offset: 0x0001E188
		public override void OnUpdate()
		{
			bool flag = !this._espEnabled;
			if (!flag)
			{
				this._frameCounter++;
				this._cachedCamera = Camera.main;
				this.RefreshPlayerCache();
				this.UpdatePlayerData();
			}
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001FFCC File Offset: 0x0001E1CC
		public override void OnGUI()
		{
			bool flag = !this._espEnabled || this._cachedCamera == null || this._cachedPlayers == null;
			if (!flag)
			{
				try
				{
					Vector3 position = this._cachedCamera.transform.position;
					foreach (Player player in this._cachedPlayers)
					{
						bool flag2 = ((player != null) ? player.field_Private_APIUser_0 : null) == null;
						if (!flag2)
						{
							string id = player.field_Private_APIUser_0.id;
							PlayerBoxData boxData;
							bool flag3 = !this._playerCache.TryGetValue(id, out boxData);
							if (!flag3)
							{
								float num = Vector3.Distance(position, player.transform.position);
								bool flag4 = num > 100f;
								if (!flag4)
								{
									this.DrawPlayerBox(boxData, num);
								}
							}
						}
					}
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("[BoxESP] GUI error: {0}", arg));
				}
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x000200D4 File Offset: 0x0001E2D4
		private void DrawPlayerBox(PlayerBoxData boxData, float distance)
		{
			bool flag = !boxData.IsValid;
			if (!flag)
			{
				Rect boxRect = boxData.BoxRect;
				DrawingUtils.DrawPlayerBox(boxRect.x, boxRect.y, boxRect.width, boxRect.height, boxData.Color, Color.black, 2f);
				bool showDistance = this._showDistance;
				if (showDistance)
				{
					string text = string.Format("{0:F1}m", distance);
					Rect rect;
					rect..ctor(boxRect.x, boxRect.y - 25f, boxRect.width, 20f);
					DrawingUtils.DrawOutlinedLabel(rect, text, boxData.Color, Color.black);
				}
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x00020188 File Offset: 0x0001E388
		private void RefreshPlayerCache()
		{
			try
			{
				PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
				List<Player> list = (playerManager != null) ? playerManager.field_Private_List_1_Player_0 : null;
				bool flag = list == null;
				if (!flag)
				{
					List<Player> list2 = new List<Player>();
					HashSet<string> hashSet = new HashSet<string>();
					foreach (Player player in list)
					{
						bool flag2 = this.IsValidPlayer(player);
						if (flag2)
						{
							list2.Add(player);
							hashSet.Add(player.field_Private_APIUser_0.id);
						}
					}
					List<string> list3 = new List<string>();
					foreach (string text in this._playerCache.Keys)
					{
						bool flag3 = !hashSet.Contains(text);
						if (flag3)
						{
							list3.Add(text);
						}
					}
					foreach (string key in list3)
					{
						this._playerCache.Remove(key);
					}
					foreach (Player player2 in list2)
					{
						string id = player2.field_Private_APIUser_0.id;
						bool flag4 = !this._playerCache.ContainsKey(id);
						if (flag4)
						{
							this._playerCache[id] = new PlayerBoxData(player2, this.GetPlayerColor(player2));
						}
					}
					this._cachedPlayers = list2.ToArray();
				}
			}
			catch (Exception arg)
			{
				kernelllogger.Error(string.Format("[BoxESP] Cache refresh error: {0}", arg));
			}
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x000203A8 File Offset: 0x0001E5A8
		private void UpdatePlayerData()
		{
			bool flag = this._cachedCamera == null || this._cachedPlayers == null;
			if (!flag)
			{
				foreach (Player player in this._cachedPlayers)
				{
					bool flag2 = ((player != null) ? player.field_Private_APIUser_0 : null) == null;
					if (!flag2)
					{
						string id = player.field_Private_APIUser_0.id;
						PlayerBoxData playerBoxData;
						bool flag3 = this._playerCache.TryGetValue(id, out playerBoxData);
						if (flag3)
						{
							playerBoxData.UpdateBox(this._cachedCamera);
							bool useRankColors = this._useRankColors;
							if (useRankColors)
							{
								playerBoxData.Color = this.GetPlayerColor(player);
							}
						}
					}
				}
			}
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0002045C File Offset: 0x0001E65C
		private bool IsValidPlayer(Player player)
		{
			return ((player != null) ? player.field_Private_APIUser_0 : null) != null && !player.field_Private_APIUser_0.IsSelf && player.transform != null;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x00020498 File Offset: 0x0001E698
		private Color GetPlayerColor(Player player)
		{
			bool flag = !this._useRankColors;
			Color result;
			if (flag)
			{
				result = Color.cyan;
			}
			else
			{
				bool flag2 = ((player != null) ? player.field_Private_APIUser_0 : null) == null;
				if (flag2)
				{
					result = Color.white;
				}
				else
				{
					bool isFriend = player.Method_Internal_get_APIUser_0().isFriend;
					if (isFriend)
					{
						result = PlayerUtil.Friend();
					}
					else
					{
						APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
						bool flag3 = field_Private_APIUser_.tags != null;
						if (flag3)
						{
							bool flag4 = field_Private_APIUser_.tags.Contains("system_trust_troll");
							if (flag4)
							{
								return PlayerUtil.Troll();
							}
							bool flag5 = field_Private_APIUser_.tags.Contains("system_trust_veteran");
							if (flag5)
							{
								return PlayerUtil.Trusted();
							}
							bool flag6 = field_Private_APIUser_.tags.Contains("system_trust_trusted");
							if (flag6)
							{
								return PlayerUtil.Known();
							}
							bool flag7 = field_Private_APIUser_.tags.Contains("system_trust_known");
							if (flag7)
							{
								return PlayerUtil.User();
							}
							bool flag8 = field_Private_APIUser_.tags.Contains("system_trust_basic");
							if (flag8)
							{
								return PlayerUtil.NewUser();
							}
						}
						result = PlayerUtil.Visitor();
					}
				}
			}
			return result;
		}

		// Token: 0x0400025B RID: 603
		private const float BOX_THICKNESS = 2f;

		// Token: 0x0400025C RID: 604
		private const float MAX_DISTANCE = 100f;

		// Token: 0x0400025D RID: 605
		private bool _espEnabled = false;

		// Token: 0x0400025E RID: 606
		private bool _useRankColors = true;

		// Token: 0x0400025F RID: 607
		private bool _showDistance = false;

		// Token: 0x04000260 RID: 608
		private readonly Dictionary<string, PlayerBoxData> _playerCache = new Dictionary<string, PlayerBoxData>(32);

		// Token: 0x04000261 RID: 609
		private Player[] _cachedPlayers;

		// Token: 0x04000262 RID: 610
		private Camera _cachedCamera;

		// Token: 0x04000263 RID: 611
		private int _frameCounter = 0;
	}
}
