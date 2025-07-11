using System;

namespace KernellVRCLite.Network
{
	// Token: 0x02000082 RID: 130
	public class NetworkStatus
	{
		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x000273D0 File Offset: 0x000255D0
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x000273D8 File Offset: 0x000255D8
		public bool IsConnected { get; set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000641 RID: 1601 RVA: 0x000273E1 File Offset: 0x000255E1
		// (set) Token: 0x06000642 RID: 1602 RVA: 0x000273E9 File Offset: 0x000255E9
		public bool IsConnecting { get; set; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000643 RID: 1603 RVA: 0x000273F2 File Offset: 0x000255F2
		// (set) Token: 0x06000644 RID: 1604 RVA: 0x000273FA File Offset: 0x000255FA
		public bool IsWorldTransitioning { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000645 RID: 1605 RVA: 0x00027403 File Offset: 0x00025603
		// (set) Token: 0x06000646 RID: 1606 RVA: 0x0002740B File Offset: 0x0002560B
		public bool IsNetworkPaused { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000647 RID: 1607 RVA: 0x00027414 File Offset: 0x00025614
		// (set) Token: 0x06000648 RID: 1608 RVA: 0x0002741C File Offset: 0x0002561C
		public int ReconnectAttempts { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000649 RID: 1609 RVA: 0x00027425 File Offset: 0x00025625
		// (set) Token: 0x0600064A RID: 1610 RVA: 0x0002742D File Offset: 0x0002562D
		public DateTime LastPingTime { get; set; }

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600064B RID: 1611 RVA: 0x00027436 File Offset: 0x00025636
		// (set) Token: 0x0600064C RID: 1612 RVA: 0x0002743E File Offset: 0x0002563E
		public DateTime LastPongTime { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600064D RID: 1613 RVA: 0x00027447 File Offset: 0x00025647
		// (set) Token: 0x0600064E RID: 1614 RVA: 0x0002744F File Offset: 0x0002564F
		public DateTime WorldChangeTime { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600064F RID: 1615 RVA: 0x00027458 File Offset: 0x00025658
		// (set) Token: 0x06000650 RID: 1616 RVA: 0x00027460 File Offset: 0x00025660
		public int QueuedMessages { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000651 RID: 1617 RVA: 0x00027469 File Offset: 0x00025669
		// (set) Token: 0x06000652 RID: 1618 RVA: 0x00027471 File Offset: 0x00025671
		public string ServerUrl { get; set; }
	}
}
