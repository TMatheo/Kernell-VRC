using System;
using System.Collections;
using System.Collections.Generic;
using KernelVRC;
using MelonLoader;
using UnityEngine;
using VRC;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000085 RID: 133
	public class CustomNameplate : MonoBehaviour
	{
		// Token: 0x0600068D RID: 1677 RVA: 0x000289F0 File Offset: 0x00026BF0
		public Player GetPlayer()
		{
			return this._player;
		}

		// Token: 0x0600068E RID: 1678 RVA: 0x000289F8 File Offset: 0x00026BF8
		public void SetPlayer(Player player)
		{
			this._player = player;
		}

		// Token: 0x0600068F RID: 1679 RVA: 0x00028A01 File Offset: 0x00026C01
		public bool GetOverRender()
		{
			return this._overRender;
		}

		// Token: 0x06000690 RID: 1680 RVA: 0x00028A09 File Offset: 0x00026C09
		public void SetOverRender(bool value)
		{
			this._overRender = value;
		}

		// Token: 0x06000691 RID: 1681 RVA: 0x00028A12 File Offset: 0x00026C12
		public bool GetEnabled()
		{
			return this._enabled;
		}

		// Token: 0x06000692 RID: 1682 RVA: 0x00028A1A File Offset: 0x00026C1A
		public void SetEnabled(bool value)
		{
			this._enabled = value;
		}

		// Token: 0x06000693 RID: 1683 RVA: 0x00028A24 File Offset: 0x00026C24
		public static void OnWorldLeaving()
		{
			object globalStateLock = CustomNameplate._globalStateLock;
			lock (globalStateLock)
			{
				CustomNameplate._globalShutdown = true;
				kernelllogger.Msg("[CustomNameplate] Global shutdown initiated for world change");
			}
		}

		// Token: 0x06000694 RID: 1684 RVA: 0x00028A78 File Offset: 0x00026C78
		public static void OnWorldEntered()
		{
			object globalStateLock = CustomNameplate._globalStateLock;
			lock (globalStateLock)
			{
				CustomNameplate._globalShutdown = false;
				kernelllogger.Msg("[CustomNameplate] Ready for new world operations");
			}
		}

		// Token: 0x06000695 RID: 1685 RVA: 0x00028ACC File Offset: 0x00026CCC
		public static bool IsGlobalShutdown()
		{
			object globalStateLock = CustomNameplate._globalStateLock;
			bool globalShutdown;
			lock (globalStateLock)
			{
				globalShutdown = CustomNameplate._globalShutdown;
			}
			return globalShutdown;
		}

		// Token: 0x06000696 RID: 1686 RVA: 0x00028B14 File Offset: 0x00026D14
		public CustomNameplate(IntPtr ptr) : base(ptr)
		{
			this._player = null;
			this._overRender = true;
			this._enabled = true;
			this._isInitialized = false;
			this._isDestroyed = false;
			this._frameCounter = 0;
			this._errorCount = 0;
			this._shouldCancel = false;
		}

		// Token: 0x06000697 RID: 1687 RVA: 0x00028B94 File Offset: 0x00026D94
		private void Start()
		{
			bool flag = !base.enabled || !this._enabled || this._player == null || this._isDestroyed || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				MelonCoroutines.Start(this.InitializeWithDelay());
			}
		}

		// Token: 0x06000698 RID: 1688 RVA: 0x00028BE2 File Offset: 0x00026DE2
		private IEnumerator InitializeWithDelay()
		{
			yield return new WaitForFixedUpdate();
			yield return new WaitForFixedUpdate();
			bool flag = this._shouldCancel || this._isDestroyed || CustomNameplate.IsGlobalShutdown();
			if (flag)
			{
				yield break;
			}
			bool initSuccess = false;
			try
			{
				this._state = new NameplateState(this._player);
				bool flag2 = this._shouldCancel || this._isDestroyed || CustomNameplate.IsGlobalShutdown();
				if (flag2)
				{
					yield break;
				}
				this._renderer = new NameplateRenderer(this._state, this._overRender);
				bool flag3 = this._shouldCancel || this._isDestroyed || CustomNameplate.IsGlobalShutdown();
				if (flag3)
				{
					yield break;
				}
				this._controller = new NameplateController(this._state, this._renderer);
				bool flag4 = this._shouldCancel || this._isDestroyed || CustomNameplate.IsGlobalShutdown();
				if (flag4)
				{
					yield break;
				}
				bool flag5 = this._controller.Initialize();
				if (flag5)
				{
					this._isInitialized = true;
					initSuccess = true;
					kernelllogger.Msg("[CustomNameplate] Successfully initialized");
				}
				else
				{
					kernelllogger.Error("[CustomNameplate] Failed to initialize controller");
				}
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				kernelllogger.Error("[CustomNameplate] Initialization error: " + ex.Message);
			}
			bool flag6 = !initSuccess;
			if (flag6)
			{
				this.CleanupResources();
			}
			yield break;
		}

		// Token: 0x06000699 RID: 1689 RVA: 0x00028BF4 File Offset: 0x00026DF4
		private void Update()
		{
			bool flag = !this._isInitialized || !base.enabled || !this._enabled || this._player == null || this._isDestroyed || this._shouldCancel || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				try
				{
					this._frameCounter++;
					bool flag2 = this._frameCounter >= 60;
					if (flag2)
					{
						this._frameCounter = 0;
						bool flag3;
						if (this._controller != null)
						{
							NameplateState state = this._state;
							flag3 = (state != null && state.IsValid());
						}
						else
						{
							flag3 = false;
						}
						bool flag4 = flag3;
						if (flag4)
						{
							this._controller.UpdateNameplate();
						}
					}
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[CustomNameplate] Update error: " + ex.Message);
					this._errorCount++;
					bool flag5 = this._errorCount > 20;
					if (flag5)
					{
						base.enabled = false;
						kernelllogger.Error("[CustomNameplate] Disabled due to repeated errors");
					}
				}
			}
		}

		// Token: 0x0600069A RID: 1690 RVA: 0x00028D04 File Offset: 0x00026F04
		private void OnDestroy()
		{
			this.CleanupResources();
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00028D10 File Offset: 0x00026F10
		public void CleanupResources()
		{
			bool isDestroyed = this._isDestroyed;
			if (!isDestroyed)
			{
				this._isDestroyed = true;
				this._shouldCancel = true;
				try
				{
					bool flag = this._controller != null;
					if (flag)
					{
						this._controller.Dispose();
						this._controller = null;
					}
					bool flag2 = this._renderer != null;
					if (flag2)
					{
						this._renderer.Dispose();
						this._renderer = null;
					}
					bool flag3 = this._state != null;
					if (flag3)
					{
						this._state.Reset();
						this._state = null;
					}
					base.StopAllCoroutines();
					this._isInitialized = false;
					base.enabled = false;
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[CustomNameplate] Cleanup error: " + ex.Message);
				}
			}
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00028DE8 File Offset: 0x00026FE8
		public static Dictionary<string, Color> GetAccentColors()
		{
			return new Dictionary<string, Color>(CustomNameplate._accentColors);
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00028E04 File Offset: 0x00027004
		public static string GetKRNLColorHex()
		{
			return ColorUtility.ToHtmlStringRGB(CustomNameplate.KRNL_COLOR);
		}

		// Token: 0x04000309 RID: 777
		private const float UPDATE_INTERVAL = 1f;

		// Token: 0x0400030A RID: 778
		private const float FLASH_SPEED = 20f;

		// Token: 0x0400030B RID: 779
		private const float FLASH_MIN_ALPHA = 0.1f;

		// Token: 0x0400030C RID: 780
		private const float FLASH_MAX_ALPHA = 1f;

		// Token: 0x0400030D RID: 781
		private const float NAMEPLATE_Y_OFFSET = -80f;

		// Token: 0x0400030E RID: 782
		private const int CRASHED_THRESHOLD = 260;

		// Token: 0x0400030F RID: 783
		private const int WARNING_THRESHOLD = 100;

		// Token: 0x04000310 RID: 784
		private const int UPDATE_FRAME_INTERVAL = 60;

		// Token: 0x04000311 RID: 785
		private static readonly Color BLUE_COLOR = new Color(0f, 0.5f, 1f);

		// Token: 0x04000312 RID: 786
		private static readonly Color GREEN_COLOR = new Color(0f, 0.8f, 0.4f);

		// Token: 0x04000313 RID: 787
		private static readonly Color RED_COLOR = new Color(1f, 0.2f, 0.2f);

		// Token: 0x04000314 RID: 788
		private static readonly Color YELLOW_COLOR = new Color(1f, 0.8f, 0.2f);

		// Token: 0x04000315 RID: 789
		private static readonly Color ORANGE_COLOR = new Color(1f, 0.5f, 0f);

		// Token: 0x04000316 RID: 790
		private static readonly Color PURPLE_COLOR = new Color(0.5f, 0f, 1f);

		// Token: 0x04000317 RID: 791
		private static readonly Color KRNL_COLOR = new Color(0.5f, 0.26f, 0.9f);

		// Token: 0x04000318 RID: 792
		private static readonly Color DEFAULT_NAMEPLATE_COLOR = new Color(0.157f, 0.004f, 0.216f);

		// Token: 0x04000319 RID: 793
		private static readonly Dictionary<string, Color> _accentColors = new Dictionary<string, Color>
		{
			{
				"Blue",
				CustomNameplate.BLUE_COLOR
			},
			{
				"Green",
				CustomNameplate.GREEN_COLOR
			},
			{
				"Red",
				CustomNameplate.RED_COLOR
			},
			{
				"Yellow",
				CustomNameplate.YELLOW_COLOR
			},
			{
				"Orange",
				CustomNameplate.ORANGE_COLOR
			},
			{
				"Purple",
				CustomNameplate.PURPLE_COLOR
			},
			{
				"KRNL",
				CustomNameplate.KRNL_COLOR
			},
			{
				"Default",
				CustomNameplate.DEFAULT_NAMEPLATE_COLOR
			}
		};

		// Token: 0x0400031A RID: 794
		private static volatile bool _globalShutdown = false;

		// Token: 0x0400031B RID: 795
		private static readonly object _globalStateLock = new object();

		// Token: 0x0400031C RID: 796
		private Player _player;

		// Token: 0x0400031D RID: 797
		private bool _overRender = true;

		// Token: 0x0400031E RID: 798
		private bool _enabled = true;

		// Token: 0x0400031F RID: 799
		private bool _isInitialized = false;

		// Token: 0x04000320 RID: 800
		private bool _isDestroyed = false;

		// Token: 0x04000321 RID: 801
		private int _frameCounter = 0;

		// Token: 0x04000322 RID: 802
		private int _errorCount = 0;

		// Token: 0x04000323 RID: 803
		private NameplateController _controller;

		// Token: 0x04000324 RID: 804
		private NameplateRenderer _renderer;

		// Token: 0x04000325 RID: 805
		private NameplateState _state;

		// Token: 0x04000326 RID: 806
		private bool _shouldCancel = false;
	}
}
