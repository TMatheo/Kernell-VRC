using System;

namespace KernellVRCLite.Network
{
	// Token: 0x02000083 RID: 131
	internal class WebSocketFrame
	{
		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000654 RID: 1620 RVA: 0x0002747A File Offset: 0x0002567A
		// (set) Token: 0x06000655 RID: 1621 RVA: 0x00027482 File Offset: 0x00025682
		public bool IsFinal { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000656 RID: 1622 RVA: 0x0002748B File Offset: 0x0002568B
		// (set) Token: 0x06000657 RID: 1623 RVA: 0x00027493 File Offset: 0x00025693
		public byte Opcode { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0002749C File Offset: 0x0002569C
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x000274A4 File Offset: 0x000256A4
		public byte[] PayloadData { get; set; }
	}
}
