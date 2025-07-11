using System;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices
{
	// Token: 0x0200000B RID: 11
	[CompilerGenerated]
	[Embedded]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	internal sealed class NullableContextAttribute : Attribute
	{
		// Token: 0x0600002B RID: 43 RVA: 0x000026F0 File Offset: 0x000008F0
		public NullableContextAttribute(byte A_1)
		{
			this.Flag = A_1;
		}

		// Token: 0x0400000D RID: 13
		public readonly byte Flag;
	}
}
