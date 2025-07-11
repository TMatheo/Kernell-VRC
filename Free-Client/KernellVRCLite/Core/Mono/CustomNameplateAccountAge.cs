using System;
using System.Collections.Generic;
using System.Globalization;
using KernellVRCLite.Network;
using KernelVRC;
using TMPro;
using UnityEngine;
using VRC;
using VRC.Core;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x0200008C RID: 140
	public class CustomNameplateAccountAge : MonoBehaviour
	{
		// Token: 0x060006D6 RID: 1750 RVA: 0x0002A616 File Offset: 0x00028816
		public Player GetPlayer()
		{
			return this._player;
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0002A61E File Offset: 0x0002881E
		public void SetPlayer(Player player)
		{
			this._player = player;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0002A627 File Offset: 0x00028827
		public string GetPlayerAge()
		{
			return this._playerAge;
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0002A62F File Offset: 0x0002882F
		public void SetPlayerAge(string age)
		{
			this._playerAge = age;
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0002A638 File Offset: 0x00028838
		public bool GetOverRender()
		{
			return this._overRender;
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0002A640 File Offset: 0x00028840
		public void SetOverRender(bool value)
		{
			this._overRender = value;
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0002A649 File Offset: 0x00028849
		public bool GetEnabled()
		{
			return this._enabled;
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0002A651 File Offset: 0x00028851
		public void SetEnabled(bool value)
		{
			this._enabled = value;
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0002A65C File Offset: 0x0002885C
		public CustomNameplateAccountAge(IntPtr ptr) : base(ptr)
		{
			this._player = null;
			this._playerAge = "";
			this._overRender = false;
			this._enabled = true;
			this._skipCounter = 20000;
			this._isInitialized = false;
			this._isDisposed = false;
			this._shouldCancel = false;
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0002A6DC File Offset: 0x000288DC
		private void Start()
		{
			bool flag = !base.enabled || !this._enabled || this._player == null || this._isDisposed || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				try
				{
					this.SetupNameplate();
				}
				catch (Exception ex)
				{
					bool flag2 = !CustomNameplate.IsGlobalShutdown();
					if (flag2)
					{
						kernelllogger.Error("[CustomNameplateAccountAge] Start error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0002A764 File Offset: 0x00028964
		private void Update()
		{
			bool flag = !this._enabled || !this._isInitialized || this._player == null || this._isDisposed || this._shouldCancel || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				try
				{
					bool flag2 = this._skipCounter >= 20000;
					if (flag2)
					{
						this.UpdateNameplateDisplay();
						this._skipCounter = 0;
					}
					else
					{
						this._skipCounter++;
					}
				}
				catch (Exception ex)
				{
					bool flag3 = !CustomNameplate.IsGlobalShutdown();
					if (flag3)
					{
						kernelllogger.Error("[CustomNameplateAccountAge] Update error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002A824 File Offset: 0x00028A24
		private void OnDestroy()
		{
			this.CleanupResources();
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0002A830 File Offset: 0x00028A30
		private void SetupNameplate()
		{
			bool flag = CustomNameplate.IsGlobalShutdown() || this._shouldCancel;
			if (!flag)
			{
				try
				{
					Player player = this._player;
					Object @object;
					if (player == null)
					{
						@object = null;
					}
					else
					{
						VRCPlayer vrcplayer = player._vrcplayer;
						@object = ((vrcplayer != null) ? vrcplayer.field_Public_PlayerNameplate_0 : null);
					}
					bool flag2 = @object == null;
					if (flag2)
					{
						kernelllogger.Error("[CustomNameplateAccountAge] Player nameplate is null");
					}
					else
					{
						PlayerNameplate field_Public_PlayerNameplate_ = this._player._vrcplayer.field_Public_PlayerNameplate_0;
						GameObject field_Public_GameObject_ = field_Public_PlayerNameplate_.field_Public_GameObject_5;
						Transform transform = (field_Public_GameObject_ != null) ? field_Public_GameObject_.transform : null;
						bool flag3 = transform == null;
						if (flag3)
						{
							kernelllogger.Error("[CustomNameplateAccountAge] Original transform is null");
						}
						else
						{
							bool flag4 = CustomNameplate.IsGlobalShutdown() || this._shouldCancel;
							if (!flag4)
							{
								this._nameplateTransform = Object.Instantiate<Transform>(transform, transform);
								bool flag5 = CustomNameplate.IsGlobalShutdown() || this._shouldCancel;
								if (flag5)
								{
									bool flag6 = this._nameplateTransform != null;
									if (flag6)
									{
										Object.Destroy(this._nameplateTransform.gameObject);
										this._nameplateTransform = null;
									}
								}
								else
								{
									this._nameplateTransform.parent = field_Public_PlayerNameplate_.field_Public_GameObject_0.transform;
									this._nameplateTransform.gameObject.SetActive(true);
									this._nameplateTransform.localPosition = new Vector3(0f, 180f, 0f);
									Transform transform2 = this._nameplateTransform.Find("Trust Text");
									this._statsText = ((transform2 != null) ? transform2.GetComponent<TextMeshProUGUI>() : null);
									bool flag7 = this._statsText != null && !CustomNameplate.IsGlobalShutdown() && !this._shouldCancel;
									if (flag7)
									{
										this.ConfigureTextComponent();
										this.DisableUnusedElements();
										this._isInitialized = true;
										kernelllogger.Msg("[CustomNameplateAccountAge] Successfully initialized");
									}
									else
									{
										kernelllogger.Error("[CustomNameplateAccountAge] Failed to find Trust Text component");
										this.CleanupResources();
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					bool flag8 = !CustomNameplate.IsGlobalShutdown();
					if (flag8)
					{
						kernelllogger.Error("[CustomNameplateAccountAge] Setup error: " + ex.Message);
					}
					this.CleanupResources();
				}
			}
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0002AA64 File Offset: 0x00028C64
		private void ConfigureTextComponent()
		{
			bool flag = CustomNameplate.IsGlobalShutdown() || this._shouldCancel || this._statsText == null;
			if (!flag)
			{
				try
				{
					this._statsText.color = Color.white;
					this._statsText.fontSize += 1f;
					this._statsText.enableAutoSizing = true;
					this._statsText.fontStyle = 1;
					this._statsText.characterSpacing = 1f;
					this._statsText.isOverlay = (this._overRender && this._enabled);
					this._statsText.enableWordWrapping = false;
					this._statsText.richText = true;
				}
				catch (Exception ex)
				{
					bool flag2 = !CustomNameplate.IsGlobalShutdown();
					if (flag2)
					{
						kernelllogger.Error("[CustomNameplateAccountAge] Text configuration error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0002AB64 File Offset: 0x00028D64
		private void DisableUnusedElements()
		{
			bool flag = CustomNameplate.IsGlobalShutdown() || this._shouldCancel || this._nameplateTransform == null;
			if (!flag)
			{
				string[] array = new string[]
				{
					"Trust Icon",
					"Performance Icon",
					"Performance Text",
					"Friend Anchor Stats"
				};
				try
				{
					foreach (string text in array)
					{
						bool flag2 = CustomNameplate.IsGlobalShutdown() || this._shouldCancel;
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
				catch (Exception ex)
				{
					bool flag4 = !CustomNameplate.IsGlobalShutdown();
					if (flag4)
					{
						kernelllogger.Error("[CustomNameplateAccountAge] Element disable error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0002AC60 File Offset: 0x00028E60
		private void UpdateNameplateDisplay()
		{
			bool flag = string.IsNullOrEmpty(this._playerAge) || this._statsText == null || CustomNameplate.IsGlobalShutdown() || this._shouldCancel;
			if (!flag)
			{
				try
				{
					DateTime dateTime;
					bool flag2 = !DateTime.TryParseExact(this._playerAge.Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
					if (!flag2)
					{
						DateTime today = DateTime.Today;
						TimeSpan timeSpan = today - dateTime;
						int num = (int)(timeSpan.TotalDays / 365.25);
						int num2 = (int)(timeSpan.TotalDays % 365.25);
						string text = (num > 0) ? string.Format("{0}y {1}d", num, num2) : string.Format("{0}d", num2);
						bool flag3 = CustomNameplate.IsGlobalShutdown() || this._shouldCancel;
						if (!flag3)
						{
							bool flag4 = false;
							try
							{
								Player player = this._player;
								string text2;
								if (player == null)
								{
									text2 = null;
								}
								else
								{
									APIUser apiuser = player.Method_Internal_get_APIUser_0();
									text2 = ((apiuser != null) ? apiuser.id : null);
								}
								string text3 = text2 ?? "";
								bool flag5 = !string.IsNullOrEmpty(text3) && !CustomNameplate.IsGlobalShutdown();
								if (flag5)
								{
									flag4 = KernellNetworkIntegration.IsKernellUser(text3);
								}
							}
							catch (Exception ex)
							{
								bool flag6 = !CustomNameplate.IsGlobalShutdown();
								if (flag6)
								{
									kernelllogger.Error("[CustomNameplateAccountAge] Error checking KRNL status: " + ex.Message);
								}
							}
							bool flag7 = CustomNameplate.IsGlobalShutdown() || this._shouldCancel;
							if (!flag7)
							{
								List<string> list = new List<string>
								{
									"<color=#" + ColorUtility.ToHtmlStringRGB(CustomNameplateAccountAge.ACCENT_PURPLE) + ">\ud83d\udcc5</color>",
									string.Format("<color=#{0}>{1:dd MMM yyyy}</color>", ColorUtility.ToHtmlStringRGB(CustomNameplateAccountAge.ACCENT_BLUE), dateTime),
									string.Concat(new string[]
									{
										"<color=#",
										ColorUtility.ToHtmlStringRGB(CustomNameplateAccountAge.ACCENT_GREEN),
										">(",
										text,
										")</color>"
									})
								};
								bool flag8 = flag4;
								if (flag8)
								{
									list.Add("<color=#" + CustomNameplate.GetKRNLColorHex() + ">[KRNL]</color>");
								}
								bool flag9 = !CustomNameplate.IsGlobalShutdown() && !this._shouldCancel && this._statsText != null;
								if (flag9)
								{
									this._statsText.text = string.Join(" ", list);
								}
							}
						}
					}
				}
				catch (Exception ex2)
				{
					bool flag10 = !CustomNameplate.IsGlobalShutdown();
					if (flag10)
					{
						kernelllogger.Error("[CustomNameplateAccountAge] Display update error: " + ex2.Message);
					}
				}
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x0002AF44 File Offset: 0x00029144
		private void CleanupResources()
		{
			bool isDisposed = this._isDisposed;
			if (!isDisposed)
			{
				this._isDisposed = true;
				this._shouldCancel = true;
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
					this._isInitialized = false;
					base.enabled = false;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[CustomNameplateAccountAge] Cleanup error: " + ex.Message);
				}
			}
		}

		// Token: 0x0400034B RID: 843
		private static readonly Color ACCENT_BLUE = new Color(0.012f, 0.322f, 1f);

		// Token: 0x0400034C RID: 844
		private static readonly Color ACCENT_GREEN = new Color(0.133f, 0.894f, 0.333f);

		// Token: 0x0400034D RID: 845
		private static readonly Color ACCENT_PURPLE = new Color(0.541f, 0.169f, 0.886f);

		// Token: 0x0400034E RID: 846
		private TextMeshProUGUI _statsText;

		// Token: 0x0400034F RID: 847
		private Transform _nameplateTransform;

		// Token: 0x04000350 RID: 848
		private Player _player;

		// Token: 0x04000351 RID: 849
		private string _playerAge;

		// Token: 0x04000352 RID: 850
		private bool _overRender;

		// Token: 0x04000353 RID: 851
		private bool _enabled = true;

		// Token: 0x04000354 RID: 852
		private int _skipCounter = 20000;

		// Token: 0x04000355 RID: 853
		private bool _isInitialized = false;

		// Token: 0x04000356 RID: 854
		private bool _isDisposed = false;

		// Token: 0x04000357 RID: 855
		private bool _shouldCancel = false;
	}
}
