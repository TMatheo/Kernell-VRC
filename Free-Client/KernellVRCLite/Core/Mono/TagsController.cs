using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KernelVRC;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000091 RID: 145
	internal class TagsController
	{
		// Token: 0x06000717 RID: 1815 RVA: 0x0002C770 File Offset: 0x0002A970
		public TagsController(TagsState state, TagsRenderer renderer, string playerName)
		{
			this._state = state;
			this._renderer = renderer;
			this._playerName = playerName;
			kernelllogger.Msg("[TagsController] Created for player: " + this._playerName);
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002C7AC File Offset: 0x0002A9AC
		public bool Initialize()
		{
			kernelllogger.Msg("[TagsController] Initializing for player: " + this._playerName);
			bool result;
			try
			{
				bool flag = this._renderer.Initialize();
				this._isInitialized = flag;
				bool flag2 = flag;
				if (flag2)
				{
					kernelllogger.Msg("[TagsController] Successfully initialized for player: " + this._playerName);
				}
				else
				{
					kernelllogger.Error("[TagsController] Failed to initialize for player: " + this._playerName);
				}
				result = flag;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsController] Initialization error for player " + this._playerName + ": " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0002C858 File Offset: 0x0002AA58
		public void FetchPlayerData()
		{
			bool flag = !this._isInitialized;
			if (flag)
			{
				kernelllogger.Warning("[TagsController] Cannot fetch player data, not initialized for player: " + this._playerName);
			}
			else
			{
				bool flag2 = !this._state.IsValid();
				if (flag2)
				{
					kernelllogger.Warning("[TagsController] Cannot fetch player data, invalid state for player: " + this._playerName);
				}
				else
				{
					try
					{
						string userId = this._state.GetUserId();
						bool flag3 = string.IsNullOrEmpty(userId);
						if (flag3)
						{
							kernelllogger.Error("[TagsController] Cannot fetch data for player " + this._playerName + ": User ID is empty");
						}
						else
						{
							bool flag4 = this.CheckAndLoadFromCache(userId);
							if (!flag4)
							{
								kernelllogger.Msg("[TagsController] Starting API requests for player: " + this._playerName + ", UserID: " + userId);
								this._state.IsTagFetched = true;
								this._state.IsBadgeFetched = true;
								MelonCoroutines.Start(this.FetchTagsCoroutine(userId));
								MelonCoroutines.Start(this.FetchBadgesCoroutine(userId));
							}
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[TagsController] Error initiating data fetch for player " + this._playerName + ": " + ex.Message);
					}
				}
			}
		}

		// Token: 0x0600071A RID: 1818 RVA: 0x0002C994 File Offset: 0x0002AB94
		private bool CheckAndLoadFromCache(string userId)
		{
			object cacheLock = TagsController._cacheLock;
			lock (cacheLock)
			{
				TagsCacheEntry tagsCacheEntry;
				bool flag2 = TagsController._apiCache.TryGetValue(userId, out tagsCacheEntry) && !tagsCacheEntry.IsExpired();
				if (flag2)
				{
					kernelllogger.Msg("[TagsController] Loading data from API cache for player: " + this._playerName);
					this._state.PlayerTags = new List<TagInfo>(tagsCacheEntry.Tags);
					this._state.PlayerBadges = new List<BadgeInfo>(tagsCacheEntry.Badges);
					this._state.CacheTimestamp = tagsCacheEntry.Timestamp;
					this._renderer.UpdateTagsDisplay();
					this._renderer.UpdateBadgesDisplay();
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600071B RID: 1819 RVA: 0x0002CA6C File Offset: 0x0002AC6C
		private IEnumerator FetchTagsCoroutine(string userId)
		{
			kernelllogger.Msg("[TagsController] Fetching tags for player: " + this._playerName + ", UserID: " + userId);
			string url = string.Format("https://api.kernell.net/tags/grab/{0}", userId);
			UnityWebRequest unityWebRequest = null;
			try
			{
				unityWebRequest = this.CreateRequest(url);
				unityWebRequest.SendWebRequest();
				float time = Time.time;
				while (!unityWebRequest.isDone)
				{
					bool flag = Time.time - time > 10f;
					if (flag)
					{
						kernelllogger.Warning("[TagsController] Tags API request timed out for player: " + this._playerName);
						break;
					}
				}
				bool flag2 = this.IsRequestSuccessful(unityWebRequest);
				if (flag2)
				{
					kernelllogger.Msg("[TagsController] Tags API request successful for player: " + this._playerName);
					this.ProcessTagsResponse(userId, unityWebRequest.downloadHandler.text);
				}
				else
				{
					kernelllogger.Warning("[TagsController] Tags API error for player " + this._playerName + ": " + unityWebRequest.error);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsController] Tags API processing error for player " + this._playerName + ": " + ex.Message);
			}
			finally
			{
				bool flag3 = unityWebRequest != null;
				if (flag3)
				{
					unityWebRequest.Dispose();
				}
			}
			return null;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002CBBC File Offset: 0x0002ADBC
		private IEnumerator FetchBadgesCoroutine(string userId)
		{
			kernelllogger.Msg("[TagsController] Fetching badges for player: " + this._playerName + ", UserID: " + userId);
			string url = string.Format("https://api.kernell.net/tags/grab/{0}/badges/16", userId);
			UnityWebRequest unityWebRequest = null;
			try
			{
				unityWebRequest = this.CreateRequest(url);
				unityWebRequest.SendWebRequest();
				float time = Time.time;
				while (!unityWebRequest.isDone)
				{
					bool flag = Time.time - time > 10f;
					if (flag)
					{
						kernelllogger.Warning("[TagsController] Badges API request timed out for player: " + this._playerName);
						break;
					}
				}
				bool flag2 = this.IsRequestSuccessful(unityWebRequest);
				if (flag2)
				{
					kernelllogger.Msg("[TagsController] Badges API request successful for player: " + this._playerName);
					this.ProcessBadgesResponse(userId, unityWebRequest.downloadHandler.text);
				}
				else
				{
					kernelllogger.Warning("[TagsController] Badges API error for player " + this._playerName + ": " + unityWebRequest.error);
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsController] Badges API processing error for player " + this._playerName + ": " + ex.Message);
			}
			finally
			{
				bool flag3 = unityWebRequest != null;
				if (flag3)
				{
					unityWebRequest.Dispose();
				}
			}
			return null;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0002CD0C File Offset: 0x0002AF0C
		private UnityWebRequest CreateRequest(string url)
		{
			UnityWebRequest result;
			try
			{
				UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);
				unityWebRequest.timeout = 10;
				result = unityWebRequest;
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsController] Error creating API request: " + ex.Message);
				result = UnityWebRequest.Get(url);
			}
			return result;
		}

		// Token: 0x0600071E RID: 1822 RVA: 0x0002CD64 File Offset: 0x0002AF64
		private bool IsRequestSuccessful(UnityWebRequest request)
		{
			return !request.isNetworkError && !request.isHttpError;
		}

		// Token: 0x0600071F RID: 1823 RVA: 0x0002CD8C File Offset: 0x0002AF8C
		private void ProcessTagsResponse(string userId, string response)
		{
			try
			{
				bool flag = string.IsNullOrEmpty(response);
				if (flag)
				{
					kernelllogger.Warning("[TagsController] Empty tags response for player: " + this._playerName);
				}
				else
				{
					List<TagInfo> list = JsonConvert.DeserializeObject<List<TagInfo>>(response);
					bool flag2 = list == null;
					if (flag2)
					{
						kernelllogger.Warning("[TagsController] Failed to deserialize tags for player: " + this._playerName);
						this._state.PlayerTags = new List<TagInfo>();
					}
					else
					{
						this._state.PlayerTags = list;
						this._state.CacheTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
						this.UpdateApiCache(userId);
						this._renderer.UpdateTagsDisplay();
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error(string.Concat(new string[]
				{
					"[TagsController] Tags parsing error for player ",
					this._playerName,
					": ",
					ex.Message,
					"\n",
					ex.StackTrace
				}));
				this._state.PlayerTags = new List<TagInfo>();
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002CEA8 File Offset: 0x0002B0A8
		private void ProcessBadgesResponse(string userId, string response)
		{
			try
			{
				bool flag = string.IsNullOrEmpty(response);
				if (flag)
				{
					kernelllogger.Warning("[TagsController] Empty badges response for player: " + this._playerName);
				}
				else
				{
					List<BadgeInfo> list = JsonConvert.DeserializeObject<List<BadgeInfo>>(response);
					bool flag2 = list == null;
					if (flag2)
					{
						kernelllogger.Warning("[TagsController] Failed to deserialize badges for player: " + this._playerName);
						this._state.PlayerBadges = new List<BadgeInfo>();
					}
					else
					{
						this._state.PlayerBadges = list;
						this.UpdateApiCache(userId);
						int num = Enumerable.Count<BadgeInfo>(list, (BadgeInfo b) => !string.IsNullOrEmpty(b.base64));
						bool flag3 = num > 0;
						if (flag3)
						{
							foreach (BadgeInfo badge in Enumerable.Where<BadgeInfo>(list, (BadgeInfo b) => !string.IsNullOrEmpty(b.base64)))
							{
								MelonCoroutines.Start(this.LoadBadgeTextureCoroutine(badge));
							}
						}
						else
						{
							this._renderer.UpdateBadgesDisplay();
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error(string.Concat(new string[]
				{
					"[TagsController] Badges parsing error for player ",
					this._playerName,
					": ",
					ex.Message,
					"\n",
					ex.StackTrace
				}));
				this._state.PlayerBadges = new List<BadgeInfo>();
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002D064 File Offset: 0x0002B264
		private void UpdateApiCache(string userId)
		{
			object cacheLock = TagsController._cacheLock;
			lock (cacheLock)
			{
				TagsCacheEntry value = new TagsCacheEntry
				{
					Tags = new List<TagInfo>(this._state.PlayerTags),
					Badges = new List<BadgeInfo>(this._state.PlayerBadges),
					Timestamp = this._state.CacheTimestamp
				};
				TagsController._apiCache[userId] = value;
				bool flag2 = TagsController._apiCache.Count > 100;
				if (flag2)
				{
					List<string> list = Enumerable.ToList<string>(Enumerable.Select<KeyValuePair<string, TagsCacheEntry>, string>(Enumerable.Where<KeyValuePair<string, TagsCacheEntry>>(TagsController._apiCache, (KeyValuePair<string, TagsCacheEntry> kv) => kv.Value.IsExpired()), (KeyValuePair<string, TagsCacheEntry> kv) => kv.Key));
					foreach (string key in list)
					{
						TagsController._apiCache.Remove(key);
					}
				}
			}
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002D1C4 File Offset: 0x0002B3C4
		private IEnumerator LoadBadgeTextureCoroutine(BadgeInfo badge)
		{
			bool flag = string.IsNullOrEmpty(badge.base64);
			if (flag)
			{
				yield break;
			}
			bool flag2 = badge.texture != null || badge.sprite != null;
			if (flag2)
			{
				yield break;
			}
			string dataUrl = "data:image/png;base64," + badge.base64;
			UnityWebRequest request = null;
			try
			{
				try
				{
					request = UnityWebRequestTexture.GetTexture(dataUrl);
					request.SendWebRequest();
					float startTime = Time.time;
					while (!request.isDone)
					{
						bool flag3 = Time.time - startTime > 10f;
						if (flag3)
						{
							kernelllogger.Warning("[TagsController] Badge texture load timed out for badge ID: " + badge.id);
							break;
						}
					}
					bool flag4 = this.IsRequestSuccessful(request);
					if (flag4)
					{
						badge.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
						badge.sprite = Sprite.Create(badge.texture, new Rect(0f, 0f, (float)badge.texture.width, (float)badge.texture.height), new Vector2(0.5f, 0.5f));
						this._renderer.UpdateBadgesDisplay();
					}
				}
				catch (Exception ex2)
				{
					Exception ex = ex2;
					kernelllogger.Error("[TagsController] Badge processing error for badge ID " + badge.id + ": " + ex.Message);
				}
				yield break;
			}
			finally
			{
				bool flag5 = request != null;
				if (flag5)
				{
					request.Dispose();
				}
			}
			yield break;
		}

		// Token: 0x06000723 RID: 1827 RVA: 0x0002D1DC File Offset: 0x0002B3DC
		public void Dispose()
		{
			kernelllogger.Msg("[TagsController] Disposing controller for player: " + this._playerName);
			try
			{
				TagsRenderer renderer = this._renderer;
				if (renderer != null)
				{
					renderer.Dispose();
				}
				this._isInitialized = false;
				kernelllogger.Msg("[TagsController] Successfully disposed controller for player: " + this._playerName);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsController] Error disposing controller for player " + this._playerName + ": " + ex.Message);
			}
		}

		// Token: 0x0400037E RID: 894
		private readonly TagsState _state;

		// Token: 0x0400037F RID: 895
		private readonly TagsRenderer _renderer;

		// Token: 0x04000380 RID: 896
		private readonly string _playerName;

		// Token: 0x04000381 RID: 897
		private bool _isInitialized = false;

		// Token: 0x04000382 RID: 898
		private static readonly Dictionary<string, TagsCacheEntry> _apiCache = new Dictionary<string, TagsCacheEntry>(64);

		// Token: 0x04000383 RID: 899
		private static readonly object _cacheLock = new object();
	}
}
