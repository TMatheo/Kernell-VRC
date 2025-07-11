using System;
using System.Collections.Generic;
using System.Linq;
using KernelVRC;
using UnityEngine;
using VRC;
using VRC.Core;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x0200008F RID: 143
	internal class TagsState
	{
		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x0002B556 File Offset: 0x00029756
		// (set) Token: 0x060006F6 RID: 1782 RVA: 0x0002B55E File Offset: 0x0002975E
		public Player Player { get; private set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x0002B567 File Offset: 0x00029767
		// (set) Token: 0x060006F8 RID: 1784 RVA: 0x0002B56F File Offset: 0x0002976F
		public List<TagInfo> PlayerTags { get; set; } = new List<TagInfo>();

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x0002B578 File Offset: 0x00029778
		// (set) Token: 0x060006FA RID: 1786 RVA: 0x0002B580 File Offset: 0x00029780
		public List<BadgeInfo> PlayerBadges { get; set; } = new List<BadgeInfo>();

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x0002B589 File Offset: 0x00029789
		// (set) Token: 0x060006FC RID: 1788 RVA: 0x0002B591 File Offset: 0x00029791
		public bool IsTagFetched { get; set; } = false;

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x0002B59A File Offset: 0x0002979A
		// (set) Token: 0x060006FE RID: 1790 RVA: 0x0002B5A2 File Offset: 0x000297A2
		public bool IsBadgeFetched { get; set; } = false;

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x0002B5AB File Offset: 0x000297AB
		// (set) Token: 0x06000700 RID: 1792 RVA: 0x0002B5B3 File Offset: 0x000297B3
		public bool IsInitialized { get; set; } = false;

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000701 RID: 1793 RVA: 0x0002B5BC File Offset: 0x000297BC
		// (set) Token: 0x06000702 RID: 1794 RVA: 0x0002B5C4 File Offset: 0x000297C4
		public long CacheTimestamp { get; set; } = 0L;

		// Token: 0x06000703 RID: 1795 RVA: 0x0002B5D0 File Offset: 0x000297D0
		public TagsState(Player player)
		{
			this.Player = player;
			string text;
			if (player == null)
			{
				text = null;
			}
			else
			{
				APIUser apiuser = player.Method_Internal_get_APIUser_0();
				text = ((apiuser != null) ? apiuser.displayName : null);
			}
			this._playerName = (text ?? "Unknown");
			this.CacheTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			kernelllogger.Msg("[TagsState] Created for player: " + this._playerName);
		}

		// Token: 0x06000704 RID: 1796 RVA: 0x0002B674 File Offset: 0x00029874
		public bool IsValid()
		{
			Player player = this.Player;
			bool flag = ((player != null) ? player.Method_Internal_get_APIUser_0() : null) != null && !string.IsNullOrEmpty(this.Player.Method_Internal_get_APIUser_0().id);
			bool flag2 = !flag;
			if (flag2)
			{
				kernelllogger.Warning("[TagsState] Invalid state for player: " + this._playerName);
			}
			return flag;
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x0002B6D8 File Offset: 0x000298D8
		public string GetUserId()
		{
			bool flag = string.IsNullOrEmpty(this._cachedUserId);
			if (flag)
			{
				Player player = this.Player;
				string text;
				if (player == null)
				{
					text = null;
				}
				else
				{
					APIUser apiuser = player.Method_Internal_get_APIUser_0();
					text = ((apiuser != null) ? apiuser.id : null);
				}
				this._cachedUserId = (text ?? "");
				bool flag2 = string.IsNullOrEmpty(this._cachedUserId);
				if (flag2)
				{
					kernelllogger.Warning("[TagsState] Could not get user ID for player: " + this._playerName);
				}
			}
			return this._cachedUserId;
		}

		// Token: 0x06000706 RID: 1798 RVA: 0x0002B758 File Offset: 0x00029958
		public bool HasContent()
		{
			bool result;
			if (this.PlayerTags.Count <= 0)
			{
				if (this.PlayerBadges.Count > 0)
				{
					result = Enumerable.Any<BadgeInfo>(this.PlayerBadges, (BadgeInfo b) => b.texture != null);
				}
				else
				{
					result = false;
				}
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000707 RID: 1799 RVA: 0x0002B7B8 File Offset: 0x000299B8
		public bool IsCacheExpired()
		{
			long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			return num - this.CacheTimestamp > 300L;
		}

		// Token: 0x06000708 RID: 1800 RVA: 0x0002B7E8 File Offset: 0x000299E8
		public void Dispose()
		{
			kernelllogger.Msg("[TagsState] Disposing state for player: " + this._playerName);
			try
			{
				foreach (BadgeInfo badgeInfo in Enumerable.Where<BadgeInfo>(this.PlayerBadges, (BadgeInfo b) => ((b != null) ? b.texture : null) != null))
				{
					Object.Destroy(badgeInfo.texture);
				}
				this.PlayerTags.Clear();
				this.PlayerBadges.Clear();
				kernelllogger.Msg("[TagsState] Successfully disposed state for player: " + this._playerName);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsState] Error disposing state for " + this._playerName + ": " + ex.Message);
			}
		}

		// Token: 0x04000373 RID: 883
		private string _playerName;

		// Token: 0x04000374 RID: 884
		private string _cachedUserId;
	}
}
