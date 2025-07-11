using System;
using System.Collections.Generic;
using KernelVRC;
using UnityEngine;
using VRC;
using VRC.Core;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x0200008E RID: 142
	public class CustomNameplateTags : MonoBehaviour, IDisposable
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006E9 RID: 1769 RVA: 0x0002B077 File Offset: 0x00029277
		// (set) Token: 0x060006EA RID: 1770 RVA: 0x0002B07F File Offset: 0x0002927F
		public Player Player { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006EB RID: 1771 RVA: 0x0002B088 File Offset: 0x00029288
		// (set) Token: 0x060006EC RID: 1772 RVA: 0x0002B090 File Offset: 0x00029290
		public bool Enabled { get; set; } = true;

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006ED RID: 1773 RVA: 0x0002B099 File Offset: 0x00029299
		// (set) Token: 0x060006EE RID: 1774 RVA: 0x0002B0A1 File Offset: 0x000292A1
		public bool OverRender { get; set; } = false;

		// Token: 0x060006EF RID: 1775 RVA: 0x0002B0AC File Offset: 0x000292AC
		public CustomNameplateTags(IntPtr ptr) : base(ptr)
		{
			kernelllogger.Msg(string.Format("[CustomNameplateTags] Created instance with ptr: {0}", ptr));
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0002B100 File Offset: 0x00029300
		public void Dispose()
		{
			kernelllogger.Msg("[CustomNameplateTags] Disposing tags for player: " + this._playerName);
			try
			{
				TagsController controller = this._controller;
				if (controller != null)
				{
					controller.Dispose();
				}
				this._controller = null;
				TagsRenderer renderer = this._renderer;
				if (renderer != null)
				{
					renderer.Dispose();
				}
				this._renderer = null;
				TagsState state = this._state;
				if (state != null)
				{
					state.Dispose();
				}
				this._state = null;
				this._isInitialized = false;
				base.enabled = false;
				kernelllogger.Msg("[CustomNameplateTags] Successfully disposed tags for player: " + this._playerName);
			}
			catch (Exception ex)
			{
				kernelllogger.Error(string.Concat(new string[]
				{
					"[CustomNameplateTags] Dispose error for ",
					this._playerName,
					": ",
					ex.Message,
					"\n",
					ex.StackTrace
				}));
			}
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0002B1F4 File Offset: 0x000293F4
		private void Start()
		{
			try
			{
				bool flag = !base.enabled || !this.Enabled;
				if (flag)
				{
					kernelllogger.Warning("[CustomNameplateTags] Component is disabled, skipping Start()");
				}
				else
				{
					bool flag2 = this.Player == null;
					if (flag2)
					{
						kernelllogger.Error("[CustomNameplateTags] Player reference is null, cannot initialize tags");
					}
					else
					{
						APIUser apiuser = this.Player.Method_Internal_get_APIUser_0();
						this._playerName = (((apiuser != null) ? apiuser.displayName : null) ?? "Unknown");
						kernelllogger.Msg("[CustomNameplateTags] Starting initialization for player: " + this._playerName);
						this._state = new TagsState(this.Player);
						this._renderer = new TagsRenderer(this._state, this.OverRender, this._playerName);
						this._controller = new TagsController(this._state, this._renderer, this._playerName);
						bool flag3 = this._controller.Initialize();
						if (flag3)
						{
							this._isInitialized = true;
							kernelllogger.Msg("[CustomNameplateTags] Successfully initialized for player: " + this._playerName);
							bool flag4 = !this.CheckAndLoadFromCache();
							if (flag4)
							{
								this._controller.FetchPlayerData();
							}
						}
						else
						{
							kernelllogger.Error("[CustomNameplateTags] Failed to initialize for player: " + this._playerName);
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[CustomNameplateTags] Start error: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		// Token: 0x060006F2 RID: 1778 RVA: 0x0002B380 File Offset: 0x00029580
		private void Update()
		{
			bool flag = !this._isInitialized || !base.enabled || !this.Enabled;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter >= 60;
				if (flag2)
				{
					this._frameCounter = 0;
					bool flag3 = this._state.IsValid() && this._state.HasContent() && this._state.IsCacheExpired();
					if (flag3)
					{
						this._controller.FetchPlayerData();
					}
				}
			}
		}

		// Token: 0x060006F3 RID: 1779 RVA: 0x0002B414 File Offset: 0x00029614
		private bool CheckAndLoadFromCache()
		{
			bool flag = this._state == null || !this._state.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				string userId = this._state.GetUserId();
				bool flag2 = string.IsNullOrEmpty(userId);
				if (flag2)
				{
					result = false;
				}
				else
				{
					Dictionary<string, TagsCacheEntry> globalTagsCache = CustomNameplateTags._globalTagsCache;
					lock (globalTagsCache)
					{
						TagsCacheEntry tagsCacheEntry;
						bool flag4 = CustomNameplateTags._globalTagsCache.TryGetValue(userId, out tagsCacheEntry) && !tagsCacheEntry.IsExpired();
						if (flag4)
						{
							kernelllogger.Msg("[CustomNameplateTags] Loading data from cache for player: " + this._playerName);
							this._state.PlayerTags = tagsCacheEntry.Tags;
							this._state.PlayerBadges = tagsCacheEntry.Badges;
							this._state.IsTagFetched = true;
							this._state.IsBadgeFetched = true;
							this._state.CacheTimestamp = tagsCacheEntry.Timestamp;
							this._renderer.UpdateTagsDisplay();
							this._renderer.UpdateBadgesDisplay();
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x04000362 RID: 866
		private TagsController _controller;

		// Token: 0x04000363 RID: 867
		private TagsRenderer _renderer;

		// Token: 0x04000364 RID: 868
		private TagsState _state;

		// Token: 0x04000365 RID: 869
		private string _playerName = "Unknown";

		// Token: 0x04000366 RID: 870
		private int _frameCounter = 0;

		// Token: 0x04000367 RID: 871
		private bool _isInitialized = false;

		// Token: 0x04000368 RID: 872
		private static readonly Dictionary<string, TagsCacheEntry> _globalTagsCache = new Dictionary<string, TagsCacheEntry>(64);
	}
}
