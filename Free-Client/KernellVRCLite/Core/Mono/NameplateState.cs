using System;
using System.Collections.Generic;
using KernellVRCLite.Network;
using KernellVRCLite.Utils;
using KernelVRC;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Networking;
using VRC.SDKBase;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000086 RID: 134
	internal class NameplateState
	{
		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00028F99 File Offset: 0x00027199
		public Player Player
		{
			get
			{
				return this._player;
			}
		}

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x00028FA1 File Offset: 0x000271A1
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x00028FA9 File Offset: 0x000271A9
		public int NoUpdateCounter
		{
			get
			{
				return this._noUpdateCounter;
			}
			set
			{
				this._noUpdateCounter = value;
			}
		}

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x00028FB2 File Offset: 0x000271B2
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x00028FBA File Offset: 0x000271BA
		public byte CurrentFrame
		{
			get
			{
				return this._currentFrame;
			}
			set
			{
				this._currentFrame = value;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x00028FC3 File Offset: 0x000271C3
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x00028FCB File Offset: 0x000271CB
		public byte CurrentPing
		{
			get
			{
				return this._currentPing;
			}
			set
			{
				this._currentPing = value;
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x00028FD4 File Offset: 0x000271D4
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x00028FDC File Offset: 0x000271DC
		public float PulseTimer
		{
			get
			{
				return this._pulseTimer;
			}
			set
			{
				this._pulseTimer = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00028FE5 File Offset: 0x000271E5
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x00028FED File Offset: 0x000271ED
		public bool IsPulsing
		{
			get
			{
				return this._isPulsing;
			}
			set
			{
				this._isPulsing = value;
			}
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x00028FF6 File Offset: 0x000271F6
		public NameplateState(Player player)
		{
			this._player = player;
			this.Reset();
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x00029018 File Offset: 0x00027218
		public void Reset()
		{
			this._noUpdateCounter = 0;
			this._currentFrame = 0;
			this._currentPing = 0;
			this._pulseTimer = 0f;
			this._isPulsing = false;
			this._lastPlayerStatus = PlayerStatus.Normal;
			this._isFriend = null;
			this._isMaster = null;
			this._isBlocked = null;
			this._isInVR = null;
			this._isOnMobile = null;
			this._isInFullBody = null;
			this._isKRNL = null;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000290A8 File Offset: 0x000272A8
		public bool IsValid()
		{
			return !CustomNameplate.IsGlobalShutdown() && this._player != null && this._player._vrcplayer != null && this._player.Method_Public_get_VRCPlayerApi_0() != null;
		}

		// Token: 0x060006AD RID: 1709 RVA: 0x000290F4 File Offset: 0x000272F4
		public PlayerStatus GetPlayerStatus()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			PlayerStatus result;
			if (flag)
			{
				result = PlayerStatus.Normal;
			}
			else
			{
				bool flag2 = this._noUpdateCounter > 460;
				if (flag2)
				{
					result = PlayerStatus.Crashed;
				}
				else
				{
					bool flag3 = this._noUpdateCounter > 200;
					if (flag3)
					{
						result = PlayerStatus.Warning;
					}
					else
					{
						result = PlayerStatus.Normal;
					}
				}
			}
			return result;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00029140 File Offset: 0x00027340
		public void TrackNetworkUpdates()
		{
			bool flag = !this.IsValid() || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				try
				{
					FlatBufferNetworkSerializer flatBufferNetworkSerializer = this._player._vrcplayer.Method_Internal_get_FlatBufferNetworkSerializer_0();
					bool flag2 = flatBufferNetworkSerializer == null;
					if (!flag2)
					{
						bool flag3 = this._currentFrame == flatBufferNetworkSerializer.field_Private_Byte_0 && this._currentPing == flatBufferNetworkSerializer.field_Private_Byte_1;
						if (flag3)
						{
							this._noUpdateCounter++;
						}
						else
						{
							this._noUpdateCounter = 0;
							PlayerStatus playerStatus = this.GetPlayerStatus();
							bool flag4 = playerStatus != this._lastPlayerStatus;
							if (flag4)
							{
								this._lastPlayerStatus = playerStatus;
							}
						}
						this._currentFrame = flatBufferNetworkSerializer.field_Private_Byte_0;
						this._currentPing = flatBufferNetworkSerializer.field_Private_Byte_1;
					}
				}
				catch (Exception ex)
				{
					bool flag5 = !CustomNameplate.IsGlobalShutdown();
					if (flag5)
					{
						kernelllogger.Error("[NameplateState] Network tracking error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00029248 File Offset: 0x00027448
		public bool IsFriend()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._isFriend == null;
				if (flag2)
				{
					this._isFriend = new bool?(PlayerUtil.IsFriend(this._player));
				}
				result = this._isFriend.Value;
			}
			return result;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002929C File Offset: 0x0002749C
		public bool IsMaster()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._isMaster == null;
				if (flag2)
				{
					Player player = this._player;
					bool? flag3;
					if (player == null)
					{
						flag3 = null;
					}
					else
					{
						VRCPlayerApi vrcplayerApi = player.Method_Public_get_VRCPlayerApi_0();
						flag3 = ((vrcplayerApi != null) ? new bool?(vrcplayerApi.isMaster) : null);
					}
					bool? flag4 = flag3;
					this._isMaster = new bool?(flag4.GetValueOrDefault());
				}
				result = this._isMaster.Value;
			}
			return result;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00029320 File Offset: 0x00027520
		public bool IsBlocked()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._isBlocked == null;
				if (flag2)
				{
					Player player = this._player;
					string text;
					if (player == null)
					{
						text = null;
					}
					else
					{
						VRCPlayerApi vrcplayerApi = player.Method_Public_get_VRCPlayerApi_0();
						text = ((vrcplayerApi != null) ? vrcplayerApi.displayName : null);
					}
					string text2 = text;
					bool value;
					if (!string.IsNullOrEmpty(text2))
					{
						List<string> knownBlocks = PlayerUtil.knownBlocks;
						value = (knownBlocks != null && knownBlocks.Contains(text2));
					}
					else
					{
						value = false;
					}
					this._isBlocked = new bool?(value);
				}
				result = this._isBlocked.Value;
			}
			return result;
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x000293A8 File Offset: 0x000275A8
		public bool IsInVR()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._isInVR == null;
				if (flag2)
				{
					Player player = this._player;
					bool? flag3;
					if (player == null)
					{
						flag3 = null;
					}
					else
					{
						VRCPlayerApi vrcplayerApi = player.Method_Public_get_VRCPlayerApi_0();
						flag3 = ((vrcplayerApi != null) ? new bool?(vrcplayerApi.IsUserInVR()) : null);
					}
					bool? flag4 = flag3;
					this._isInVR = new bool?(flag4.GetValueOrDefault());
				}
				result = this._isInVR.Value;
			}
			return result;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x0002942C File Offset: 0x0002762C
		public bool IsOnMobile()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._isOnMobile == null;
				if (flag2)
				{
					Player player = this._player;
					string text = ((player != null) ? player.ToString().ToLower() : null) ?? "";
					this._isOnMobile = new bool?(text.Contains("android") || text.Contains("ios"));
				}
				result = this._isOnMobile.Value;
			}
			return result;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x000294B4 File Offset: 0x000276B4
		public bool IsInFullBody()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._isInFullBody == null;
				if (flag2)
				{
					this._isInFullBody = new bool?(this.DetectFullBody());
				}
				result = this._isInFullBody.Value;
			}
			return result;
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x00029504 File Offset: 0x00027704
		public bool IsKRNLUser()
		{
			bool flag = CustomNameplate.IsGlobalShutdown();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this._isKRNL == null;
				if (flag2)
				{
					this._isKRNL = new bool?(this.CheckKRNLStatus());
				}
				result = this._isKRNL.Value;
			}
			return result;
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x00029554 File Offset: 0x00027754
		private bool CheckKRNLStatus()
		{
			bool result;
			try
			{
				bool flag = CustomNameplate.IsGlobalShutdown();
				if (flag)
				{
					result = false;
				}
				else
				{
					Player player = this._player;
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
					string text2 = text ?? "";
					bool flag2 = string.IsNullOrEmpty(text2);
					if (flag2)
					{
						result = false;
					}
					else
					{
						result = KernellNetworkIntegration.IsKernellUser(text2);
					}
				}
			}
			catch (Exception ex)
			{
				bool flag3 = !CustomNameplate.IsGlobalShutdown();
				if (flag3)
				{
					kernelllogger.Error("[NameplateState] Error checking KRNL status: " + ex.Message);
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x000295F0 File Offset: 0x000277F0
		private bool DetectFullBody()
		{
			bool result;
			try
			{
				bool flag = CustomNameplate.IsGlobalShutdown() || !this.IsInVR();
				if (flag)
				{
					result = false;
				}
				else
				{
					Player player = this._player;
					Animator animator;
					if (player == null)
					{
						animator = null;
					}
					else
					{
						VRCPlayer vrcplayer = player._vrcplayer;
						animator = ((vrcplayer != null) ? vrcplayer.field_Internal_Animator_0 : null);
					}
					Animator animator2 = animator;
					bool flag2 = animator2 == null || !animator2.isHuman;
					if (flag2)
					{
						result = false;
					}
					else
					{
						Transform boneTransform = animator2.GetBoneTransform(0);
						Transform transform = animator2.transform;
						bool flag3 = boneTransform == null || transform == null;
						if (flag3)
						{
							result = false;
						}
						else
						{
							float y = boneTransform.rotation.eulerAngles.y;
							float y2 = transform.rotation.eulerAngles.y;
							float num = Mathf.Abs(Mathf.DeltaAngle(y, y2));
							bool flag4 = num > 15f;
							if (flag4)
							{
								result = true;
							}
							else
							{
								Transform boneTransform2 = animator2.GetBoneTransform(5);
								Transform boneTransform3 = animator2.GetBoneTransform(6);
								bool flag5 = boneTransform2 != null && boneTransform3 != null;
								if (flag5)
								{
									float num2 = Mathf.Abs(boneTransform2.position.y - boneTransform3.position.y);
									bool flag6 = num2 > 0.1f;
									if (flag6)
									{
										return true;
									}
								}
								result = false;
							}
						}
					}
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x04000327 RID: 807
		private Player _player;

		// Token: 0x04000328 RID: 808
		private int _noUpdateCounter;

		// Token: 0x04000329 RID: 809
		private byte _currentFrame;

		// Token: 0x0400032A RID: 810
		private byte _currentPing;

		// Token: 0x0400032B RID: 811
		private float _pulseTimer;

		// Token: 0x0400032C RID: 812
		private bool _isPulsing;

		// Token: 0x0400032D RID: 813
		private bool? _isFriend;

		// Token: 0x0400032E RID: 814
		private bool? _isMaster;

		// Token: 0x0400032F RID: 815
		private bool? _isBlocked;

		// Token: 0x04000330 RID: 816
		private bool? _isInVR;

		// Token: 0x04000331 RID: 817
		private bool? _isOnMobile;

		// Token: 0x04000332 RID: 818
		private bool? _isInFullBody;

		// Token: 0x04000333 RID: 819
		private bool? _isKRNL;

		// Token: 0x04000334 RID: 820
		private PlayerStatus _lastPlayerStatus = PlayerStatus.Normal;
	}
}
