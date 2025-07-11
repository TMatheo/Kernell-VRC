using System;
using VRC.Core;

// Token: 0x0200000C RID: 12
public static class AvatarUtil
{
	// Token: 0x0600002C RID: 44 RVA: 0x00002700 File Offset: 0x00000900
	public static ApiAvatar GetAvatarInfo(this VRCPlayer player)
	{
		return (player != null) ? player.Method_Public_get_ApiAvatar_1() : null;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002724 File Offset: 0x00000924
	public static void ChangeAvatar(string avatar_id)
	{
		ApiAvatar apiAvatar = new ApiAvatar
		{
			id = avatar_id
		};
		PageAvatar.Method_Public_Static_Void_ApiAvatar_String_0(apiAvatar, "AvatarMenu");
	}
}
