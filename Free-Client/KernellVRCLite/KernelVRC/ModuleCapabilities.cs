using System;

namespace KernelVRC
{
	// Token: 0x020000AD RID: 173
	[Flags]
	public enum ModuleCapabilities : uint
	{
		// Token: 0x04000469 RID: 1129
		None = 0U,
		// Token: 0x0400046A RID: 1130
		Update = 1U,
		// Token: 0x0400046B RID: 1131
		LateUpdate = 2U,
		// Token: 0x0400046C RID: 1132
		GUI = 4U,
		// Token: 0x0400046D RID: 1133
		PlayerEvents = 8U,
		// Token: 0x0400046E RID: 1134
		WorldEvents = 16U,
		// Token: 0x0400046F RID: 1135
		AvatarEvents = 32U,
		// Token: 0x04000470 RID: 1136
		NetworkEvents = 64U,
		// Token: 0x04000471 RID: 1137
		UdonEvents = 128U,
		// Token: 0x04000472 RID: 1138
		MenuEvents = 256U,
		// Token: 0x04000473 RID: 1139
		SceneEvents = 512U,
		// Token: 0x04000474 RID: 1140
		UIInit = 1024U,
		// Token: 0x04000475 RID: 1141
		UserLogin = 2048U,
		// Token: 0x04000476 RID: 1142
		AllUpdates = 7U,
		// Token: 0x04000477 RID: 1143
		AllPlayerEvents = 40U,
		// Token: 0x04000478 RID: 1144
		AllWorldEvents = 528U,
		// Token: 0x04000479 RID: 1145
		AllNetworkEvents = 192U
	}
}
