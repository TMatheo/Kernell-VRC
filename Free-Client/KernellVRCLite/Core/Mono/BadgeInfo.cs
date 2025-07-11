using System;
using UnityEngine;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000093 RID: 147
	[Serializable]
	internal class BadgeInfo
	{
		// Token: 0x04000387 RID: 903
		public string id;

		// Token: 0x04000388 RID: 904
		public string base64;

		// Token: 0x04000389 RID: 905
		[NonSerialized]
		public Texture2D texture;

		// Token: 0x0400038A RID: 906
		[NonSerialized]
		public Sprite sprite;
	}
}
