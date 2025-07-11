using System;

namespace KernelVRC
{
	// Token: 0x020000AF RID: 175
	public enum ModulePriority : byte
	{
		// Token: 0x0400048A RID: 1162
		Lowest,
		// Token: 0x0400048B RID: 1163
		Low = 64,
		// Token: 0x0400048C RID: 1164
		Normal = 128,
		// Token: 0x0400048D RID: 1165
		High = 192,
		// Token: 0x0400048E RID: 1166
		Critical = 255
	}
}
