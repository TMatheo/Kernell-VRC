using System;
using System.Linq;
using System.Reflection;
using MelonLoader;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.SDKBase;
using VRC.UI.Elements;

namespace KernellClientUI.VRChat
{
	// Token: 0x0200002B RID: 43
	public static class PlayerExtensions
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000A710 File Offset: 0x00008910
		private static MethodInfo LoadAvatarMethod
		{
			get
			{
				bool flag = PlayerExtensions._reloadAvatarMethod == null;
				bool flag2 = flag;
				if (flag2)
				{
					PlayerExtensions._reloadAvatarMethod = Enumerable.First<MethodInfo>(typeof(VRCPlayer).GetMethods(), delegate(MethodInfo mi)
					{
						bool flag3 = mi.Name.StartsWith("Method_Private_Void_Boolean_") && mi.Name.Length < 31;
						if (flag3)
						{
							bool flag4 = Enumerable.Any<ParameterInfo>(mi.GetParameters(), (ParameterInfo pi) => pi.IsOptional);
							if (flag4)
							{
								return XrefUtils.CheckUsedBy(mi, "ReloadAvatarNetworkedRPC", null);
							}
						}
						return false;
					});
				}
				return PlayerExtensions._reloadAvatarMethod;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001D2 RID: 466 RVA: 0x0000A774 File Offset: 0x00008974
		private static MethodInfo ReloadAllAvatarsMethod
		{
			get
			{
				bool flag = PlayerExtensions._reloadAllAvatarsMethod == null;
				bool flag2 = flag;
				if (flag2)
				{
					PlayerExtensions._reloadAllAvatarsMethod = Enumerable.First<MethodInfo>(typeof(VRCPlayer).GetMethods(), delegate(MethodInfo mi)
					{
						bool flag3 = mi.Name.StartsWith("Method_Public_Void_Boolean_") && mi.Name.Length < 30;
						if (flag3)
						{
							bool flag4 = Enumerable.All<ParameterInfo>(mi.GetParameters(), (ParameterInfo pi) => pi.IsOptional);
							if (flag4)
							{
								return XrefUtils.CheckUsedBy(mi, "Method_Public_Void_", typeof(FeaturePermissionManager));
							}
						}
						return false;
					});
				}
				return PlayerExtensions._reloadAllAvatarsMethod;
			}
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0000A7D8 File Offset: 0x000089D8
		public static Player GetPlayer(string UserID)
		{
			foreach (Player player in Enumerable.ToList<Player>(PlayerManager.Method_Private_Static_get_PlayerManager_0().field_Private_List_1_Player_0.ToArray()))
			{
				bool flag = player.field_Private_APIUser_0.id == UserID;
				bool flag2 = flag;
				if (flag2)
				{
					return player;
				}
			}
			return null;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000A85C File Offset: 0x00008A5C
		public static SelectedUserMenuQM GetTarget()
		{
			QuickMenu quickMenu = Enumerable.FirstOrDefault<QuickMenu>(Resources.FindObjectsOfTypeAll<QuickMenu>());
			bool flag = quickMenu != null;
			bool flag2 = flag;
			SelectedUserMenuQM result;
			if (flag2)
			{
				result = quickMenu.field_Private_UIPage_1.GetComponent<SelectedUserMenuQM>();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000A8A0 File Offset: 0x00008AA0
		public static Player GetPlayer(IUser value)
		{
			MelonLogger.Msg("DEV ASSIST: prop_String_0=" + value.Method_Public_Abstract_Virtual_New_get_String_0());
			MelonLogger.Msg("DEV ASSIST: prop_String_1=" + value.Method_Public_Abstract_Virtual_New_get_String_1());
			MelonLogger.Msg("DEV ASSIST: prop_String_2=" + value.Method_Public_Abstract_Virtual_New_get_String_2());
			MelonLogger.Msg("DEV ASSIST: prop_String_3=" + value.Method_Public_Abstract_Virtual_New_get_String_3());
			MelonLogger.Msg("DEV ASSIST: prop_String_4=" + value.Method_Public_Abstract_Virtual_New_get_String_4());
			return PlayerExtensions.GetPlayer(value.Method_Public_Abstract_Virtual_New_get_String_0());
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000A92C File Offset: 0x00008B2C
		public static VRCPlayer GetVRCPlayer(IUser value)
		{
			return PlayerExtensions.GetPlayer(value)._vrcplayer;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000A94C File Offset: 0x00008B4C
		public static APIUser GetAPIUser(IUser value)
		{
			return PlayerExtensions.GetPlayer(value).Method_Internal_get_APIUser_0();
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000A96C File Offset: 0x00008B6C
		public static ApiAvatar GetApiAvatar(IUser value)
		{
			return PlayerExtensions.GetPlayer(value).Method_Public_get_ApiAvatar_PDM_0();
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000A98C File Offset: 0x00008B8C
		public static IUser SelectedIUser()
		{
			return PlayerExtensions.GetTarget().field_Private_IUser_0;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000A9A8 File Offset: 0x00008BA8
		public static VRCPlayer GetVRCPlayer()
		{
			return PlayerExtensions.GetPlayer(PlayerExtensions.GetTarget().field_Private_IUser_0)._vrcplayer;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000A9D0 File Offset: 0x00008BD0
		public static APIUser GetAPIUser()
		{
			return PlayerExtensions.GetPlayer(PlayerExtensions.GetTarget().field_Private_IUser_0).field_Private_APIUser_0;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000A9F8 File Offset: 0x00008BF8
		public static ApiAvatar GetApiAvatar()
		{
			return PlayerExtensions.GetPlayer(PlayerExtensions.GetTarget().field_Private_IUser_0).Method_Public_get_ApiAvatar_PDM_0();
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000AA20 File Offset: 0x00008C20
		public static Player[] GetPlayers(PlayerManager playerManager)
		{
			return playerManager.field_Private_List_1_Player_0.ToArray();
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000AA44 File Offset: 0x00008C44
		public static GameObject GetAvatarObject(VRCPlayer vrcPlayer)
		{
			return vrcPlayer.field_Internal_GameObject_0;
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000AA5C File Offset: 0x00008C5C
		public static VRCPlayerApi GetPlayerApi(VRCPlayer vrcPlayer)
		{
			return vrcPlayer.field_Private_VRCPlayerApi_0;
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000AA74 File Offset: 0x00008C74
		public static bool IsStaff(APIUser user)
		{
			bool hasModerationPowers = user.hasModerationPowers;
			bool flag = hasModerationPowers;
			return flag || user.developerType > 0 || user.tags.Contains("admin_moderator") || user.tags.Contains("admin_scripting_access") || user.tags.Contains("admin_official_thumbnail");
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000AAE2 File Offset: 0x00008CE2
		public static void ReloadAvatar(VRCPlayer instance)
		{
			PlayerExtensions.LoadAvatarMethod.Invoke(instance, new object[]
			{
				true
			});
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000AB00 File Offset: 0x00008D00
		public static void ReloadAllAvatars(VRCPlayer instance, bool ignoreSelf = false)
		{
			PlayerExtensions.ReloadAllAvatarsMethod.Invoke(instance, new object[]
			{
				ignoreSelf
			});
		}

		// Token: 0x040000B4 RID: 180
		private static MethodInfo _reloadAvatarMethod;

		// Token: 0x040000B5 RID: 181
		private static MethodInfo _reloadAllAvatarsMethod;
	}
}
