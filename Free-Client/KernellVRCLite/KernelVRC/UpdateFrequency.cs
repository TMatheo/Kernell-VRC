using System;

namespace KernelVRC
{
	// Token: 0x020000AE RID: 174
	public enum UpdateFrequency : byte
	{
		// Token: 0x0400047B RID: 1147
		EveryFrame = 1,
		// Token: 0x0400047C RID: 1148
		Every2Frames,
		// Token: 0x0400047D RID: 1149
		Every3Frames,
		// Token: 0x0400047E RID: 1150
		Every4Frames,
		// Token: 0x0400047F RID: 1151
		Every5Frames,
		// Token: 0x04000480 RID: 1152
		Every10Frames = 10,
		// Token: 0x04000481 RID: 1153
		Every15Frames = 15,
		// Token: 0x04000482 RID: 1154
		Every20Frames = 20,
		// Token: 0x04000483 RID: 1155
		Every25Frames = 25,
		// Token: 0x04000484 RID: 1156
		Every30Frames = 30,
		// Token: 0x04000485 RID: 1157
		Every60Frames = 60,
		// Token: 0x04000486 RID: 1158
		Every120Frames = 120,
		// Token: 0x04000487 RID: 1159
		Every200Frames = 200,
		// Token: 0x04000488 RID: 1160
		OnDemand = 255
	}
}
