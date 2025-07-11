using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200000A RID: 10
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.ReturnValue | AttributeTargets.GenericParameter, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableAttribute : Attribute
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000026C7 File Offset: 0x000008C7
		public NullableAttribute(byte A_1)
		{
			this.NullableFlags = new byte[]
			{
				A_1
			};
		}

		// Token: 0x0600002A RID: 42 RVA: 0x000026E0 File Offset: 0x000008E0
		public NullableAttribute(byte[] A_1)
		{
			this.NullableFlags = A_1;
		}

		// Token: 0x0400000C RID: 12
		public readonly byte[] NullableFlags;
	}
}
