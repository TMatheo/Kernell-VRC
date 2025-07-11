using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.VRChat;
using KernelVRC;
using UnityEngine;
using UnityEngine.XR;
using VRC;
using VRC.Core;
using VRC.SDKBase;

namespace KernellVRCLite.Utils
{
	// Token: 0x020000A7 RID: 167
	public static class PlayerUtil
	{
		// Token: 0x17000193 RID: 403
		// (get) Token: 0x0600088A RID: 2186 RVA: 0x00034E70 File Offset: 0x00033070
		public static bool IsInRoom
		{
			get
			{
				return RoomManager.field_Internal_Static_ApiWorld_0 != null && RoomManager.field_Private_Static_ApiWorldInstance_0 != null;
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00034E94 File Offset: 0x00033094
		public static bool IsInVR
		{
			get
			{
				bool cachedVRState = PlayerUtil._cachedVRState;
				bool isInVrCache;
				if (cachedVRState)
				{
					isInVrCache = PlayerUtil._isInVrCache;
				}
				else
				{
					PlayerUtil._cachedVRState = true;
					List<XRDisplaySubsystem> list = new List<XRDisplaySubsystem>();
					SubsystemManager.GetInstances<XRDisplaySubsystem>(list);
					List<XRDisplaySubsystem>.Enumerator enumerator = list.GetEnumerator();
					while (enumerator.MoveNext())
					{
						bool running = enumerator._current.running;
						if (running)
						{
							PlayerUtil._isInVrCache = true;
							break;
						}
					}
					isInVrCache = PlayerUtil._isInVrCache;
				}
				return isInVrCache;
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x0600088C RID: 2188 RVA: 0x00034F00 File Offset: 0x00033100
		public static string Current_World_ID
		{
			get
			{
				return RoomManager.field_Internal_Static_ApiWorld_0.id + ":" + RoomManager.field_Private_Static_ApiWorldInstance_0.instanceId;
			}
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x00034F30 File Offset: 0x00033130
		internal static bool AnyActionMenuesOpen()
		{
			return ActionMenuController.Method_Public_Static_get_ActionMenuController_PDM_0().field_Public_ActionMenuOpener_0.field_Private_Boolean_0 || ActionMenuController.Method_Public_Static_get_ActionMenuController_PDM_0().field_Private_Boolean_1;
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x00034F60 File Offset: 0x00033160
		public static Player LocalPlayer()
		{
			return Player.Method_Internal_Static_get_Player_0();
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x00034F68 File Offset: 0x00033168
		public static string GetAvatarStatus(Player player)
		{
			string text = player.Method_Public_get_ApiAvatar_PDM_0().releaseStatus.ToLower();
			return (text == "public") ? ("<color=green>" + text + "</color>") : ("<color=red>" + text + "</color>");
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00034FBC File Offset: 0x000331BC
		public static string GetPlatform(Player player)
		{
			bool flag = !(player != null);
			string result;
			if (flag)
			{
				result = "";
			}
			else
			{
				result = (player.field_Private_APIUser_0.IsOnMobile ? "<color=green>Quest</color>" : (PlayerUtil.GetVrcPlayerApi(player).IsUserInVR() ? "<color=#CE00D5>VR</color>" : "<color=grey>PC</color>"));
			}
			return result;
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00035014 File Offset: 0x00033214
		public static string GetFramesColord(Player player)
		{
			float frames = PlayerUtil.GetFrames(player);
			return ((double)frames > 80.0) ? ("<color=green>" + frames.ToString() + "</color>") : (((double)frames > 30.0) ? ("<color=yellow>" + frames.ToString() + "</color>") : ("<color=red>" + frames.ToString() + "</color>"));
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00035090 File Offset: 0x00033290
		public static string GetPlayerRankTextColoured(Player player)
		{
			return (player == null) ? "" : "";
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x000350B8 File Offset: 0x000332B8
		public static string GetPingColord(Player player)
		{
			short ping = PlayerUtil.GetPing(player);
			return (ping > 150) ? ("<color=red>" + ping.ToString() + "</color>") : ((ping > 75) ? ("<color=yellow>" + ping.ToString() + "</color>") : ("<color=green>" + ping.ToString() + "</color>"));
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x00035124 File Offset: 0x00033324
		public static Color Friend()
		{
			return VRCPlayer.field_Internal_Static_Color_7;
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0003512B File Offset: 0x0003332B
		public static Color Trusted()
		{
			return VRCPlayer.field_Internal_Static_Color_6;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00035132 File Offset: 0x00033332
		public static Color Known()
		{
			return VRCPlayer.field_Internal_Static_Color_5;
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00035139 File Offset: 0x00033339
		public static Color User()
		{
			return VRCPlayer.field_Internal_Static_Color_4;
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x00035140 File Offset: 0x00033340
		public static Color NewUser()
		{
			return VRCPlayer.field_Internal_Static_Color_3;
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x00035147 File Offset: 0x00033347
		public static Color Visitor()
		{
			return VRCPlayer.field_Internal_Static_Color_2;
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x0003514E File Offset: 0x0003334E
		public static Color Troll()
		{
			return VRCPlayer.field_Internal_Static_Color_0;
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x00035158 File Offset: 0x00033358
		public static float GetFrames(Player player)
		{
			return (player._vrcplayer._serializer.Method_Internal_get_Byte_1() == 0) ? -1f : Mathf.Floor(1000f / (float)player._vrcplayer._serializer.Method_Internal_get_Byte_1());
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x0003519F File Offset: 0x0003339F
		public static short GetPing(Player player)
		{
			return (short)player._vrcplayer._serializer.Method_Internal_get_Byte_1();
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x000351B4 File Offset: 0x000333B4
		public static bool ClientDetect(Player player)
		{
			return (double)PlayerUtil.GetFrames(player) > 200.0 || (double)PlayerUtil.GetFrames(player) < 1.0 || PlayerUtil.GetPing(player) > 665 || PlayerUtil.GetPing(player) < 0;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00035204 File Offset: 0x00033404
		public static bool GetIsMaster(Player player)
		{
			bool flag = !(player != null);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				try
				{
					result = PlayerUtil.GetVrcPlayerApi(player).isMaster;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x0003524C File Offset: 0x0003344C
		public static VRCPlayer GetLocalVRCPlayer()
		{
			return VRCPlayer.field_Internal_Static_VRCPlayer_0;
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00035253 File Offset: 0x00033453
		public static VRCPlayerApi GetVrcPlayerApi(Player player)
		{
			return (player != null) ? player.Method_Public_get_VRCPlayerApi_0() : null;
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0003524C File Offset: 0x0003344C
		public static VRCPlayer GetVRCPlayer()
		{
			return VRCPlayer.field_Internal_Static_VRCPlayer_0;
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x00035264 File Offset: 0x00033464
		public static Player[] GetAllPlayers()
		{
			return PlayerManager.Method_Private_Static_get_PlayerManager_0().field_Private_List_1_Player_0.ToArray();
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0003528C File Offset: 0x0003348C
		public static Player GetPlayerNewtworkedId(this int id)
		{
			return Enumerable.FirstOrDefault<Player>(Enumerable.Where<Player>(PlayerUtil.GetAllPlayers(), (Player player) => player._vrcplayer._serializer.Method_Internal_get_Int32_0() == id));
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x000352C8 File Offset: 0x000334C8
		public static APIUser GetUserById(string userid)
		{
			return Enumerable.FirstOrDefault<Player>(Enumerable.Where<Player>(PlayerManager.Method_Private_Static_get_PlayerManager_0().field_Private_List_1_Player_0.ToArray(), (Player player) => player.field_Private_APIUser_0.id == userid)).field_Private_APIUser_0;
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x00035314 File Offset: 0x00033514
		public static bool IsFriend(Player player)
		{
			return APIUser.CurrentUser.friendIDs.Contains(player.field_Private_APIUser_0.id);
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x00035340 File Offset: 0x00033540
		public static Player GetPlayer(PlayerManager PM, int actorId)
		{
			foreach (Player player in PlayerExtensions.GetPlayers(PM))
			{
				bool flag = !(player == null) && player.Method_Public_Virtual_Final_New_get_Int32_0() == actorId;
				if (flag)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0003538F File Offset: 0x0003358F
		internal static APIUser GetAPIUser(Player player)
		{
			return player.field_Private_APIUser_0;
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x00035398 File Offset: 0x00033598
		public static Color GetPlayerTrustColor(List<string> tags)
		{
			bool flag = tags == null;
			Color result;
			if (flag)
			{
				result = Color.white;
			}
			else
			{
				foreach (string text in tags)
				{
					bool flag2 = text.Equals("system_trust_troll", StringComparison.OrdinalIgnoreCase);
					if (flag2)
					{
						return PlayerUtil.Troll();
					}
					bool flag3 = text.Equals("system_trust_veteran", StringComparison.OrdinalIgnoreCase);
					if (flag3)
					{
						return PlayerUtil.Trusted();
					}
					bool flag4 = text.Equals("system_trust_trusted", StringComparison.OrdinalIgnoreCase);
					if (flag4)
					{
						return PlayerUtil.Known();
					}
					bool flag5 = text.Equals("system_trust_known", StringComparison.OrdinalIgnoreCase);
					if (flag5)
					{
						return PlayerUtil.User();
					}
					bool flag6 = text.Equals("system_trust_basic", StringComparison.OrdinalIgnoreCase);
					if (flag6)
					{
						return PlayerUtil.NewUser();
					}
				}
				result = PlayerUtil.Visitor();
			}
			return result;
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x00035468 File Offset: 0x00033668
		public static Player GetPlayerInformationById(int index)
		{
			foreach (Player player in PlayerUtil.GetAllPlayers())
			{
				bool flag = player._vrcplayer._serializer.Method_Internal_get_Int32_0() == index;
				if (flag)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x060008AA RID: 2218 RVA: 0x000354B4 File Offset: 0x000336B4
		public static IEnumerator NameplateColours(Player player, bool bypass = false, float time = 10f)
		{
			bool flag = player != null;
			if (flag)
			{
				bool flag2 = !bypass;
				if (flag2)
				{
					yield return new WaitForSecondsRealtime(time);
				}
				try
				{
					PlayerNameplate nameplate = player._vrcplayer.field_Public_PlayerNameplate_0;
					List<string> rank = player.field_Private_APIUser_0.tags;
					ImageThreeSlice img = new ImageThreeSlice();
					img._sprite = EmbeddedResourceLoader.LoadEmbeddedSprite("KernellVRCLite.assets.Banner-NP-2025.png", null, 100f);
					nameplate.field_Public_ImageThreeSlice_0 = img;
					bool troll = false;
					bool trusted = false;
					bool known = false;
					bool user = false;
					bool newUser = false;
					List<string>.Enumerator enumerator = rank.GetEnumerator();
					while (enumerator.MoveNext())
					{
						string tag = enumerator._current;
						bool flag3 = tag == "system_trust_troll";
						if (flag3)
						{
							troll = true;
						}
						bool flag4 = tag == "system_trust_veteran";
						if (flag4)
						{
							trusted = true;
						}
						bool flag5 = tag == "system_trust_trusted";
						if (flag5)
						{
							known = true;
						}
						bool flag6 = tag == "system_trust_known";
						if (flag6)
						{
							user = true;
						}
						bool flag7 = tag == "system_trust_basic";
						if (flag7)
						{
							newUser = true;
						}
						tag = null;
						tag = null;
					}
					bool flag8 = troll;
					if (flag8)
					{
						nameplate.field_Public_Color_0 = PlayerUtil.Troll();
					}
					else
					{
						bool flag9 = trusted;
						if (flag9)
						{
							nameplate.field_Public_Color_0 = PlayerUtil.Trusted();
						}
						else
						{
							bool flag10 = known;
							if (flag10)
							{
								nameplate.field_Public_Color_0 = PlayerUtil.Known();
							}
							else
							{
								bool flag11 = user;
								if (flag11)
								{
									nameplate.field_Public_Color_0 = PlayerUtil.User();
								}
								else
								{
									bool flag12 = newUser;
									if (flag12)
									{
										nameplate.field_Public_Color_0 = PlayerUtil.NewUser();
									}
								}
							}
						}
					}
					nameplate = null;
					rank = null;
					enumerator = null;
					nameplate = null;
					rank = null;
					img = null;
					enumerator = null;
				}
				catch
				{
				}
			}
			yield break;
		}

		// Token: 0x0400041E RID: 1054
		private static bool _isInVrCache;

		// Token: 0x0400041F RID: 1055
		private static bool _cachedVRState;

		// Token: 0x04000420 RID: 1056
		public static Color defaultNameplateColor;

		// Token: 0x04000421 RID: 1057
		public static List<string> knownBlocks = new List<string>();

		// Token: 0x04000422 RID: 1058
		public static List<string> knownMutes = new List<string>();
	}
}
