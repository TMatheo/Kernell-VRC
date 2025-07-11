using System;

namespace KernellClientUI
{
	// Token: 0x02000025 RID: 37
	public class ComponentPriority : Attribute
	{
		// Token: 0x06000183 RID: 387 RVA: 0x00008D94 File Offset: 0x00006F94
		public ComponentPriority(int priority = 0)
		{
			this.Priority = priority;
		}

		// Token: 0x04000084 RID: 132
		public int Priority;
	}
}
