using System;
using System.Collections.Generic;
using KernelVRC;
using TMPro;
using UnityEngine;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000087 RID: 135
	internal class NameplateRenderer
	{
		// Token: 0x060006B8 RID: 1720 RVA: 0x00029770 File Offset: 0x00027970
		public NameplateRenderer(NameplateState state, bool overRender)
		{
			this._state = state;
			this._overRender = overRender;
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x000297AC File Offset: 0x000279AC
		public bool Initialize()
		{
			bool flag = CustomNameplate.IsGlobalShutdown() || this._isDisposed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				try
				{
					bool flag2 = !this._state.IsValid();
					if (flag2)
					{
						kernelllogger.Error("[NameplateRenderer] Invalid state during initialization");
						result = false;
					}
					else
					{
						VRCPlayer vrcplayer = this._state.Player._vrcplayer;
						PlayerNameplate playerNameplate = (vrcplayer != null) ? vrcplayer.field_Public_PlayerNameplate_0 : null;
						bool flag3 = ((playerNameplate != null) ? playerNameplate.field_Public_GameObject_5 : null) == null;
						if (flag3)
						{
							kernelllogger.Error("[NameplateRenderer] Nameplate components missing");
							result = false;
						}
						else
						{
							bool flag4 = CustomNameplate.IsGlobalShutdown();
							if (flag4)
							{
								result = false;
							}
							else
							{
								this.SetupNameplateComponents(playerNameplate);
								this._isInitialized = true;
								kernelllogger.Msg("[NameplateRenderer] Successfully initialized");
								result = true;
							}
						}
					}
				}
				catch (Exception ex)
				{
					bool flag5 = !CustomNameplate.IsGlobalShutdown();
					if (flag5)
					{
						kernelllogger.Error("[NameplateRenderer] Initialization error: " + ex.Message);
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x000298B0 File Offset: 0x00027AB0
		private void SetupNameplateComponents(PlayerNameplate playerNameplate)
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				Transform transform = playerNameplate.field_Public_GameObject_5.transform;
				this.HideDefaultElements(transform);
				bool flag2 = CustomNameplate.IsGlobalShutdown();
				if (!flag2)
				{
					this.SetupTrustText(transform);
					bool flag3 = CustomNameplate.IsGlobalShutdown();
					if (!flag3)
					{
						this.CreateCustomNameplate(playerNameplate, transform);
					}
				}
			}
		}

		// Token: 0x060006BB RID: 1723 RVA: 0x00029904 File Offset: 0x00027B04
		private void HideDefaultElements(Transform baseTransform)
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				string[] array = new string[]
				{
					"Performance Icon",
					"Performance Text"
				};
				foreach (string text in array)
				{
					bool flag2 = CustomNameplate.IsGlobalShutdown();
					if (flag2)
					{
						break;
					}
					Transform transform = baseTransform.Find(text);
					bool flag3 = transform != null;
					if (flag3)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x060006BC RID: 1724 RVA: 0x00029984 File Offset: 0x00027B84
		private void SetupTrustText(Transform baseTransform)
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				Transform transform = baseTransform.Find("Trust Text");
				TextMeshProUGUI textMeshProUGUI = (transform != null) ? transform.GetComponent<TextMeshProUGUI>() : null;
				bool flag2 = textMeshProUGUI != null;
				if (flag2)
				{
					textMeshProUGUI.color = CustomNameplate.GetAccentColors()["Default"];
					textMeshProUGUI.isOverlay = this._overRender;
				}
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x000299E8 File Offset: 0x00027BE8
		private void CreateCustomNameplate(PlayerNameplate playerNameplate, Transform baseTransform)
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				this._nameplateTransform = Object.Instantiate<Transform>(baseTransform, baseTransform);
				bool flag2 = CustomNameplate.IsGlobalShutdown();
				if (flag2)
				{
					bool flag3 = this._nameplateTransform != null;
					if (flag3)
					{
						Object.Destroy(this._nameplateTransform.gameObject);
					}
				}
				else
				{
					this._nameplateTransform.parent = playerNameplate.field_Public_GameObject_0.transform;
					this._nameplateTransform.localPosition = new Vector3(0f, -80f, 0f);
					this._nameplateTransform.gameObject.SetActive(true);
					Transform transform = this._nameplateTransform.Find("Trust Text");
					this._statsText = ((transform != null) ? transform.GetComponent<TextMeshProUGUI>() : null);
					bool flag4 = this._statsText != null;
					if (flag4)
					{
						this.ConfigureStatsText();
						bool flag5 = !CustomNameplate.IsGlobalShutdown();
						if (flag5)
						{
							this.DisableUnusedElements();
						}
					}
				}
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00029AE0 File Offset: 0x00027CE0
		private void ConfigureStatsText()
		{
			bool flag = CustomNameplate.IsGlobalShutdown() || this._statsText == null;
			if (!flag)
			{
				this._statsText.color = CustomNameplate.GetAccentColors()["Default"];
				this._statsText.fontSize += 2f;
				this._statsText.enableAutoSizing = true;
				this._statsText.fontStyle = 1;
				this._statsText.characterSpacing = 1f;
				this._statsText.enableWordWrapping = false;
				this._statsText.richText = true;
				this._statsText.isOverlay = this._overRender;
			}
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00029B98 File Offset: 0x00027D98
		private void DisableUnusedElements()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				string[] array = new string[]
				{
					"Trust Icon",
					"Friend Anchor Stats"
				};
				foreach (string text in array)
				{
					bool flag2 = CustomNameplate.IsGlobalShutdown();
					if (flag2)
					{
						break;
					}
					Transform transform = this._nameplateTransform.Find(text);
					bool flag3 = transform != null;
					if (flag3)
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x00029C1C File Offset: 0x00027E1C
		public void UpdateDisplay()
		{
			bool flag = !this._isInitialized || this._isDisposed || this._statsText == null || !this._state.IsValid() || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				try
				{
					PlayerStatus playerStatus = this._state.GetPlayerStatus();
					bool flag2 = this._state.IsKRNLUser();
					bool flag3 = CustomNameplate.IsGlobalShutdown();
					if (!flag3)
					{
						bool flag4 = playerStatus != this._lastDisplayedStatus || flag2 != this._lastKRNLStatus || string.IsNullOrEmpty(this._lastDisplayText);
						if (flag4)
						{
							this._lastDisplayedStatus = playerStatus;
							this._lastKRNLStatus = flag2;
							List<string> list = new List<string>();
							bool flag5 = CustomNameplate.IsGlobalShutdown();
							if (flag5)
							{
								return;
							}
							string platformIcon = this.GetPlatformIcon();
							bool flag6 = !string.IsNullOrEmpty(platformIcon);
							if (flag6)
							{
								list.Add(string.Concat(new string[]
								{
									"<color=#",
									ColorUtility.ToHtmlStringRGB(CustomNameplate.GetAccentColors()["Blue"]),
									">",
									platformIcon,
									"</color>"
								}));
							}
							bool flag7 = CustomNameplate.IsGlobalShutdown();
							if (flag7)
							{
								return;
							}
							StatusInfo statusInfo = this.GetStatusInfo(playerStatus);
							bool flag8 = !string.IsNullOrEmpty(statusInfo.Icon);
							if (flag8)
							{
								list.Add(statusInfo.Color + statusInfo.Icon + "</color>");
							}
							bool flag9 = CustomNameplate.IsGlobalShutdown();
							if (flag9)
							{
								return;
							}
							FriendInfo friendInfo = this.GetFriendInfo();
							bool flag10 = !string.IsNullOrEmpty(friendInfo.Icon);
							if (flag10)
							{
								list.Add(string.Concat(new string[]
								{
									"<color=#",
									ColorUtility.ToHtmlStringRGB(friendInfo.Color),
									">",
									friendInfo.Icon,
									"</color>"
								}));
							}
							bool flag11 = CustomNameplate.IsGlobalShutdown();
							if (flag11)
							{
								return;
							}
							bool flag12 = this._state.IsMaster();
							if (flag12)
							{
								list.Add("<color=#" + ColorUtility.ToHtmlStringRGB(CustomNameplate.GetAccentColors()["Purple"]) + ">\ud83d\udc51</color>");
							}
							bool flag13 = CustomNameplate.IsGlobalShutdown();
							if (flag13)
							{
								return;
							}
							bool flag14 = flag2;
							if (flag14)
							{
								list.Add("<color=#" + CustomNameplate.GetKRNLColorHex() + ">[KRNL]</color>");
							}
							bool flag15 = CustomNameplate.IsGlobalShutdown();
							if (flag15)
							{
								return;
							}
							bool flag16 = this._state.IsBlocked();
							if (flag16)
							{
								list.Add("<color=#FF0000>Blocked you \ud83d\udeab</color>");
							}
							bool flag17 = CustomNameplate.IsGlobalShutdown();
							if (flag17)
							{
								return;
							}
							this._lastDisplayText = string.Join(" | ", list);
							bool flag18 = this._statsText != null && !CustomNameplate.IsGlobalShutdown();
							if (flag18)
							{
								this._statsText.text = this._lastDisplayText;
							}
						}
						bool flag19 = !CustomNameplate.IsGlobalShutdown();
						if (flag19)
						{
							this.HandleVisualEffects(playerStatus);
						}
					}
				}
				catch (Exception ex)
				{
					bool flag20 = !CustomNameplate.IsGlobalShutdown();
					if (flag20)
					{
						kernelllogger.Error("[NameplateRenderer] Display update error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x00029F64 File Offset: 0x00028164
		private string GetPlatformIcon()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				try
				{
					string text = "";
					bool flag2 = this._state.IsInVR();
					if (flag2)
					{
						text += "\ud83e\udd7d";
						bool flag3 = this._state.IsInFullBody();
						if (flag3)
						{
							text += "\ud83d\udc63";
						}
					}
					else
					{
						bool flag4 = this._state.IsOnMobile();
						if (flag4)
						{
							text += "\ud83d\udcf1";
						}
						else
						{
							text += "\ud83d\udda5️";
						}
					}
					result = text;
				}
				catch
				{
					result = "\ud83d\udda5️";
				}
			}
			return result;
		}

		// Token: 0x060006C2 RID: 1730 RVA: 0x0002A01C File Offset: 0x0002821C
		private StatusInfo GetStatusInfo(PlayerStatus status)
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			StatusInfo result;
			if (flag)
			{
				result = new StatusInfo
				{
					Icon = "",
					Color = ""
				};
			}
			else
			{
				Dictionary<string, Color> accentColors = CustomNameplate.GetAccentColors();
				if (!true)
				{
				}
				StatusInfo statusInfo;
				if (status != PlayerStatus.Warning)
				{
					if (status != PlayerStatus.Crashed)
					{
						statusInfo = new StatusInfo
						{
							Icon = "✨",
							Color = "<color=#" + ColorUtility.ToHtmlStringRGB(accentColors["Green"]) + ">"
						};
					}
					else
					{
						statusInfo = new StatusInfo
						{
							Icon = "\ud83d\udc80",
							Color = "<color=#" + ColorUtility.ToHtmlStringRGB(accentColors["Red"]) + ">"
						};
					}
				}
				else
				{
					statusInfo = new StatusInfo
					{
						Icon = "⚠️",
						Color = "<color=#" + ColorUtility.ToHtmlStringRGB(accentColors["Yellow"]) + ">"
					};
				}
				if (!true)
				{
				}
				result = statusInfo;
			}
			return result;
		}

		// Token: 0x060006C3 RID: 1731 RVA: 0x0002A124 File Offset: 0x00028324
		private FriendInfo GetFriendInfo()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			FriendInfo result;
			if (flag)
			{
				result = new FriendInfo
				{
					Icon = "",
					Color = Color.white
				};
			}
			else
			{
				bool flag2 = this._state.IsFriend();
				Dictionary<string, Color> accentColors = CustomNameplate.GetAccentColors();
				result = new FriendInfo
				{
					Icon = (flag2 ? "\ud83d\udc9a" : "\ud83d\udc94"),
					Color = (flag2 ? accentColors["Green"] : accentColors["Red"])
				};
			}
			return result;
		}

		// Token: 0x060006C4 RID: 1732 RVA: 0x0002A1B0 File Offset: 0x000283B0
		private void HandleVisualEffects(PlayerStatus status)
		{
			bool flag = this._statsText == null || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				bool flag2 = status == PlayerStatus.Crashed;
				if (flag2)
				{
					this.ApplyFlashEffect();
				}
				else
				{
					bool isPulsing = this._state.IsPulsing;
					if (isPulsing)
					{
						this.ApplyPulseEffect();
					}
					else
					{
						bool flag3 = this._lastAlpha != 1f;
						if (flag3)
						{
							this._statsText.alpha = 1f;
							this._lastAlpha = 1f;
						}
					}
				}
			}
		}

		// Token: 0x060006C5 RID: 1733 RVA: 0x0002A23C File Offset: 0x0002843C
		private void ApplyFlashEffect()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				this._state.PulseTimer += Time.deltaTime;
				float num = Mathf.Abs(Mathf.Sin(this._state.PulseTimer * 20f)) * 0.9f + 0.1f;
				bool flag2 = Mathf.Abs(num - this._lastAlpha) > 0.05f && this._statsText != null;
				if (flag2)
				{
					this._statsText.alpha = num;
					this._lastAlpha = num;
				}
			}
		}

		// Token: 0x060006C6 RID: 1734 RVA: 0x0002A2D4 File Offset: 0x000284D4
		private void ApplyPulseEffect()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				this._state.PulseTimer += Time.deltaTime;
				float num = Mathf.Sin(this._state.PulseTimer * 4f) * 0.2f + 0.8f;
				bool flag2 = Mathf.Abs(num - this._lastAlpha) > 0.05f && this._statsText != null;
				if (flag2)
				{
					this._statsText.alpha = num;
					this._lastAlpha = num;
				}
				bool flag3 = this._state.PulseTimer > 2f;
				if (flag3)
				{
					this._state.IsPulsing = false;
					bool flag4 = this._statsText != null;
					if (flag4)
					{
						this._statsText.alpha = 1f;
					}
					this._lastAlpha = 1f;
					this._state.PulseTimer = 0f;
				}
			}
		}

		// Token: 0x060006C7 RID: 1735 RVA: 0x0002A3D0 File Offset: 0x000285D0
		public void Dispose()
		{
			bool isDisposed = this._isDisposed;
			if (!isDisposed)
			{
				this._isDisposed = true;
				try
				{
					bool flag = this._statsText != null;
					if (flag)
					{
						this._statsText.text = "";
						this._statsText = null;
					}
					bool flag2 = this._nameplateTransform != null;
					if (flag2)
					{
						Object.Destroy(this._nameplateTransform.gameObject);
						this._nameplateTransform = null;
					}
					this._lastDisplayText = "";
					this._isInitialized = false;
				}
				catch (Exception ex)
				{
					bool flag3 = !CustomNameplate.IsGlobalShutdown();
					if (flag3)
					{
						kernelllogger.Error("[NameplateRenderer] Dispose error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x04000335 RID: 821
		private readonly NameplateState _state;

		// Token: 0x04000336 RID: 822
		private readonly bool _overRender;

		// Token: 0x04000337 RID: 823
		private TextMeshProUGUI _statsText;

		// Token: 0x04000338 RID: 824
		private Transform _nameplateTransform;

		// Token: 0x04000339 RID: 825
		private bool _isInitialized;

		// Token: 0x0400033A RID: 826
		private bool _isDisposed;

		// Token: 0x0400033B RID: 827
		private string _lastDisplayText = string.Empty;

		// Token: 0x0400033C RID: 828
		private PlayerStatus _lastDisplayedStatus = PlayerStatus.Normal;

		// Token: 0x0400033D RID: 829
		private float _lastAlpha = 1f;

		// Token: 0x0400033E RID: 830
		private bool _lastKRNLStatus = false;
	}
}
