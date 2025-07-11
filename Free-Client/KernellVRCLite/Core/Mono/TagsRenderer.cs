using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KernelVRC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000090 RID: 144
	internal class TagsRenderer
	{
		// Token: 0x06000709 RID: 1801 RVA: 0x0002B8E0 File Offset: 0x00029AE0
		public TagsRenderer(TagsState state, bool overRender, string playerName)
		{
			this._state = state;
			this._overRender = overRender;
			this._playerName = playerName;
			kernelllogger.Msg(string.Format("[TagsRenderer] Created for player: {0}, overRender: {1}", this._playerName, overRender));
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002B940 File Offset: 0x00029B40
		public bool Initialize()
		{
			kernelllogger.Msg("[TagsRenderer] Initializing for player: " + this._playerName);
			bool result;
			try
			{
				bool flag = !this._state.IsValid();
				if (flag)
				{
					kernelllogger.Error("[TagsRenderer] Cannot initialize with invalid state for player: " + this._playerName);
					result = false;
				}
				else
				{
					VRCPlayer vrcplayer = this._state.Player._vrcplayer;
					PlayerNameplate playerNameplate = (vrcplayer != null) ? vrcplayer.field_Public_PlayerNameplate_0 : null;
					bool flag2 = playerNameplate == null;
					if (flag2)
					{
						kernelllogger.Warning("[TagsRenderer] No nameplate found for " + this._playerName);
						result = false;
					}
					else
					{
						this.SetupNameplate(playerNameplate);
						this._isInitialized = true;
						kernelllogger.Msg("[TagsRenderer] Successfully initialized for player: " + this._playerName);
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error(string.Concat(new string[]
				{
					"[TagsRenderer] Initialization error for player ",
					this._playerName,
					": ",
					ex.Message,
					"\n",
					ex.StackTrace
				}));
				result = false;
			}
			return result;
		}

		// Token: 0x0600070B RID: 1803 RVA: 0x0002BA60 File Offset: 0x00029C60
		private void SetupNameplate(PlayerNameplate playerNameplate)
		{
			try
			{
				kernelllogger.Msg("[TagsRenderer] Setting up nameplate for player: " + this._playerName);
				GameObject field_Public_GameObject_ = playerNameplate.field_Public_GameObject_5;
				Transform transform = (field_Public_GameObject_ != null) ? field_Public_GameObject_.transform : null;
				bool flag = transform == null;
				if (flag)
				{
					kernelllogger.Error("[TagsRenderer] Original nameplate transform not found for player: " + this._playerName);
				}
				else
				{
					this._nameplateTransform = Object.Instantiate<Transform>(transform, transform);
					bool flag2 = this._nameplateTransform == null;
					if (flag2)
					{
						kernelllogger.Error("[TagsRenderer] Failed to instantiate nameplate transform for player: " + this._playerName);
					}
					else
					{
						GameObject field_Public_GameObject_2 = playerNameplate.field_Public_GameObject_0;
						bool flag3 = field_Public_GameObject_2 == null;
						if (flag3)
						{
							kernelllogger.Error("[TagsRenderer] Parent GameObject is null for player: " + this._playerName);
						}
						else
						{
							this._nameplateTransform.parent = field_Public_GameObject_2.transform;
							this._nameplateTransform.localPosition = new Vector3(0f, 175f, 0f);
							this._nameplateTransform.gameObject.SetActive(false);
							kernelllogger.Msg(string.Format("[TagsRenderer] Created nameplate transform at vertical offset {0} for player: {1}", 175f, this._playerName));
							this.SetupTagsText();
							this.SetupBadgesContainer();
							this.DisableUnusedElements();
							kernelllogger.Msg("[TagsRenderer] Nameplate setup completed for player: " + this._playerName);
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error(string.Concat(new string[]
				{
					"[TagsRenderer] Error setting up nameplate for player ",
					this._playerName,
					": ",
					ex.Message,
					"\n",
					ex.StackTrace
				}));
				throw;
			}
		}

		// Token: 0x0600070C RID: 1804 RVA: 0x0002BC24 File Offset: 0x00029E24
		private void SetupTagsText()
		{
			try
			{
				Transform transform = this._nameplateTransform.Find("Trust Text");
				bool flag = transform == null;
				if (flag)
				{
					kernelllogger.Error("[TagsRenderer] Trust Text transform not found for player: " + this._playerName);
				}
				else
				{
					this._tagsText = transform.GetComponent<TextMeshProUGUI>();
					bool flag2 = this._tagsText == null;
					if (flag2)
					{
						kernelllogger.Error("[TagsRenderer] TextMeshProUGUI component not found on Trust Text for player: " + this._playerName);
					}
					else
					{
						this._tagsText.color = Color.white;
						this._tagsText.fontSize += 1f;
						this._tagsText.enableAutoSizing = true;
						this._tagsText.fontStyle = 1;
						this._tagsText.characterSpacing = 1f;
						this._tagsText.isOverlay = this._overRender;
						this._tagsText.alignment = 514;
						this._tagsText.text = string.Empty;
						kernelllogger.Msg("[TagsRenderer] Tags text configured for player: " + this._playerName);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsRenderer] Error setting up tags text for player " + this._playerName + ": " + ex.Message);
			}
		}

		// Token: 0x0600070D RID: 1805 RVA: 0x0002BD88 File Offset: 0x00029F88
		private void SetupBadgesContainer()
		{
			try
			{
				this._badgesContainer = new GameObject("BadgesContainer");
				this._badgesContainer.transform.SetParent(this._nameplateTransform);
				this._badgesContainer.transform.localPosition = new Vector3(0f, 20f, 0f);
				HorizontalLayoutGroup horizontalLayoutGroup = this._badgesContainer.AddComponent<HorizontalLayoutGroup>();
				horizontalLayoutGroup.spacing = 2f;
				horizontalLayoutGroup.childAlignment = 4;
				horizontalLayoutGroup.childControlWidth = false;
				horizontalLayoutGroup.childControlHeight = false;
				horizontalLayoutGroup.childForceExpandWidth = false;
				horizontalLayoutGroup.childForceExpandHeight = false;
				kernelllogger.Msg("[TagsRenderer] Badges container created for player: " + this._playerName);
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsRenderer] Error setting up badges container for player " + this._playerName + ": " + ex.Message);
			}
		}

		// Token: 0x0600070E RID: 1806 RVA: 0x0002BE74 File Offset: 0x0002A074
		private void DisableUnusedElements()
		{
			try
			{
				string[] array = new string[]
				{
					"Trust Icon",
					"Performance Icon",
					"Performance Text",
					"Friend Anchor Stats"
				};
				foreach (string text in array)
				{
					Transform transform = this._nameplateTransform.Find(text);
					bool flag = transform != null;
					if (flag)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsRenderer] Error disabling unused elements for player " + this._playerName + ": " + ex.Message);
			}
		}

		// Token: 0x0600070F RID: 1807 RVA: 0x0002BF28 File Offset: 0x0002A128
		public void UpdateTagsDisplay()
		{
			bool flag = !this._isInitialized;
			if (flag)
			{
				kernelllogger.Warning("[TagsRenderer] Cannot update tags display, not initialized for player: " + this._playerName);
			}
			else
			{
				bool flag2 = this._tagsText == null;
				if (flag2)
				{
					kernelllogger.Warning("[TagsRenderer] Tags text is null for player: " + this._playerName);
				}
				else
				{
					bool flag3 = this._state.PlayerTags.Count == 0;
					if (flag3)
					{
						bool flag4 = !string.IsNullOrEmpty(this._lastTagsText);
						if (flag4)
						{
							this._tagsText.text = string.Empty;
							this._lastTagsText = string.Empty;
						}
					}
					else
					{
						try
						{
							string text = this.FormatTags();
							bool flag5 = text != this._lastTagsText;
							if (flag5)
							{
								this._tagsText.text = text;
								this._lastTagsText = text;
								kernelllogger.Msg("[TagsRenderer] Updated tags display for player: " + this._playerName + ", content: " + text);
							}
							this.UpdateNameplateVisibility();
						}
						catch (Exception ex)
						{
							kernelllogger.Error(string.Concat(new string[]
							{
								"[TagsRenderer] Tags update error for player ",
								this._playerName,
								": ",
								ex.Message,
								"\n",
								ex.StackTrace
							}));
						}
					}
				}
			}
		}

		// Token: 0x06000710 RID: 1808 RVA: 0x0002C090 File Offset: 0x0002A290
		private string FormatTags()
		{
			bool flag = this._state.PlayerTags.Count == 0;
			string result;
			if (flag)
			{
				result = string.Empty;
			}
			else
			{
				try
				{
					StringBuilder stringBuilder = new StringBuilder(this._state.PlayerTags.Count * 25);
					bool flag2 = true;
					foreach (TagInfo tagInfo in Enumerable.Where<TagInfo>(this._state.PlayerTags, (TagInfo t) => !string.IsNullOrEmpty((t != null) ? t.text : null)))
					{
						bool flag3 = !flag2;
						if (flag3)
						{
							stringBuilder.Append(" | ");
						}
						Color unityColor = tagInfo.GetUnityColor();
						string value = ColorUtility.ToHtmlStringRGB(unityColor);
						stringBuilder.Append("<color=#").Append(value).Append(">").Append(tagInfo.text).Append("</color>");
						flag2 = false;
					}
					result = stringBuilder.ToString();
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[TagsRenderer] Error formatting tags for player " + this._playerName + ": " + ex.Message);
					result = "Error";
				}
			}
			return result;
		}

		// Token: 0x06000711 RID: 1809 RVA: 0x0002C1EC File Offset: 0x0002A3EC
		public void UpdateBadgesDisplay()
		{
			bool flag = !this._isInitialized;
			if (flag)
			{
				kernelllogger.Warning("[TagsRenderer] Cannot update badges display, not initialized for player: " + this._playerName);
			}
			else
			{
				bool flag2 = this._badgesContainer == null;
				if (flag2)
				{
					kernelllogger.Warning("[TagsRenderer] Badges container is null for player: " + this._playerName);
				}
				else
				{
					try
					{
						List<BadgeInfo> list = Enumerable.ToList<BadgeInfo>(Enumerable.Take<BadgeInfo>(Enumerable.Where<BadgeInfo>(this._state.PlayerBadges, (BadgeInfo b) => b != null && b.texture != null), 6));
						bool flag3 = list.Count == this._lastBadgeCount;
						if (!flag3)
						{
							this._lastBadgeCount = list.Count;
							kernelllogger.Msg(string.Format("[TagsRenderer] Updating badges display for player: {0}, badge count: {1}", this._playerName, list.Count));
							this.ClearExistingBadges();
							this.CreateBadgeImages(list);
							this.UpdateNameplateVisibility();
							kernelllogger.Msg("[TagsRenderer] Badge display updated for player: " + this._playerName);
						}
					}
					catch (Exception ex)
					{
						kernelllogger.Error(string.Concat(new string[]
						{
							"[TagsRenderer] Badges update error for player ",
							this._playerName,
							": ",
							ex.Message,
							"\n",
							ex.StackTrace
						}));
					}
				}
			}
		}

		// Token: 0x06000712 RID: 1810 RVA: 0x0002C35C File Offset: 0x0002A55C
		private void ClearExistingBadges()
		{
			try
			{
				int childCount = this._badgesContainer.transform.childCount;
				for (int i = childCount - 1; i >= 0; i--)
				{
					try
					{
						Object.Destroy(this._badgesContainer.transform.GetChild(i).gameObject);
					}
					catch (Exception ex)
					{
						kernelllogger.Error("[TagsRenderer] Error destroying badge GameObject for player " + this._playerName + ": " + ex.Message);
					}
				}
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("[TagsRenderer] Error clearing existing badges for player " + this._playerName + ": " + ex2.Message);
			}
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x0002C420 File Offset: 0x0002A620
		private void CreateBadgeImages(List<BadgeInfo> badgesWithTextures)
		{
			try
			{
				int num = 0;
				foreach (BadgeInfo badge in badgesWithTextures)
				{
					this.CreateBadgeImage(badge, num);
					num++;
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsRenderer] Error creating badge images for player " + this._playerName + ": " + ex.Message);
			}
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0002C4B4 File Offset: 0x0002A6B4
		private void CreateBadgeImage(BadgeInfo badge, int index)
		{
			try
			{
				GameObject gameObject = new GameObject(string.Format("Badge_{0}", index));
				gameObject.transform.SetParent(this._badgesContainer.transform, false);
				RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
				rectTransform.sizeDelta = new Vector2(16f, 16f);
				Image image = gameObject.AddComponent<Image>();
				bool flag = badge.sprite == null && badge.texture != null;
				if (flag)
				{
					badge.sprite = Sprite.Create(badge.texture, new Rect(0f, 0f, (float)badge.texture.width, (float)badge.texture.height), new Vector2(0.5f, 0.5f));
				}
				image.sprite = badge.sprite;
			}
			catch (Exception ex)
			{
				kernelllogger.Error(string.Format("[TagsRenderer] Error creating badge image {0} for player {1}: {2}", index, this._playerName, ex.Message));
			}
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x0002C5C8 File Offset: 0x0002A7C8
		private void UpdateNameplateVisibility()
		{
			try
			{
				bool flag = this._nameplateTransform == null;
				if (!flag)
				{
					bool flag2 = this._state.HasContent();
					bool flag3 = this._nameplateTransform.gameObject.activeSelf != flag2;
					if (flag3)
					{
						this._nameplateTransform.gameObject.SetActive(flag2);
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[TagsRenderer] Error updating nameplate visibility for player " + this._playerName + ": " + ex.Message);
			}
		}

		// Token: 0x06000716 RID: 1814 RVA: 0x0002C65C File Offset: 0x0002A85C
		public void Dispose()
		{
			kernelllogger.Msg("[TagsRenderer] Disposing renderer for player: " + this._playerName);
			try
			{
				bool flag = this._tagsText != null;
				if (flag)
				{
					this._tagsText.text = null;
					this._tagsText = null;
				}
				bool flag2 = this._badgesContainer != null;
				if (flag2)
				{
					Object.Destroy(this._badgesContainer);
					this._badgesContainer = null;
				}
				bool flag3 = this._nameplateTransform != null;
				if (flag3)
				{
					Object.Destroy(this._nameplateTransform.gameObject);
					this._nameplateTransform = null;
				}
				this._isInitialized = false;
				kernelllogger.Msg("[TagsRenderer] Successfully disposed renderer for player: " + this._playerName);
			}
			catch (Exception ex)
			{
				kernelllogger.Error(string.Concat(new string[]
				{
					"[TagsRenderer] Error disposing renderer for player ",
					this._playerName,
					": ",
					ex.Message,
					"\n",
					ex.StackTrace
				}));
			}
		}

		// Token: 0x04000375 RID: 885
		private readonly TagsState _state;

		// Token: 0x04000376 RID: 886
		private readonly bool _overRender;

		// Token: 0x04000377 RID: 887
		private readonly string _playerName;

		// Token: 0x04000378 RID: 888
		private TextMeshProUGUI _tagsText;

		// Token: 0x04000379 RID: 889
		private Transform _nameplateTransform;

		// Token: 0x0400037A RID: 890
		private GameObject _badgesContainer;

		// Token: 0x0400037B RID: 891
		private bool _isInitialized = false;

		// Token: 0x0400037C RID: 892
		private string _lastTagsText = string.Empty;

		// Token: 0x0400037D RID: 893
		private int _lastBadgeCount = 0;
	}
}
