using System;
using UnityEngine;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x0200008D RID: 141
	internal static class TagConstants
	{
		// Token: 0x04000358 RID: 856
		public const string TAGS_API_URL = "https://api.kernell.net/tags/grab/{0}";

		// Token: 0x04000359 RID: 857
		public const string BADGES_API_URL = "https://api.kernell.net/tags/grab/{0}/badges/16";

		// Token: 0x0400035A RID: 858
		public const float VERTICAL_OFFSET = 175f;

		// Token: 0x0400035B RID: 859
		public const float BADGE_SIZE = 16f;

		// Token: 0x0400035C RID: 860
		public const float BADGE_SPACING = 2f;

		// Token: 0x0400035D RID: 861
		public const int MAX_BADGES_TO_DISPLAY = 6;

		// Token: 0x0400035E RID: 862
		public const int API_TIMEOUT = 10;

		// Token: 0x0400035F RID: 863
		public const int UPDATE_INTERVAL_FRAMES = 60;

		// Token: 0x04000360 RID: 864
		public const int CACHE_DURATION_SECONDS = 300;

		// Token: 0x04000361 RID: 865
		public static readonly Color DEFAULT_TAG_COLOR = new Color(0.541f, 0.169f, 0.886f);
	}
}
