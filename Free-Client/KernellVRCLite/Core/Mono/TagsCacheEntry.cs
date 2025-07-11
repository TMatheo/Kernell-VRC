using System;
using System.Collections.Generic;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000094 RID: 148
	internal class TagsCacheEntry
	{
		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0002D3A0 File Offset: 0x0002B5A0
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0002D3A8 File Offset: 0x0002B5A8
		public List<TagInfo> Tags { get; set; } = new List<TagInfo>();

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0002D3B1 File Offset: 0x0002B5B1
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0002D3B9 File Offset: 0x0002B5B9
		public List<BadgeInfo> Badges { get; set; } = new List<BadgeInfo>();

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0002D3C2 File Offset: 0x0002B5C2
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0002D3CA File Offset: 0x0002B5CA
		public long Timestamp { get; set; }

		// Token: 0x0600072E RID: 1838 RVA: 0x0002D3D4 File Offset: 0x0002B5D4
		public bool IsExpired()
		{
			long num = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			return num - this.Timestamp > 300L;
		}
	}
}
