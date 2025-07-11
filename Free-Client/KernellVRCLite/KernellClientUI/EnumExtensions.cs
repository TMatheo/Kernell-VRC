using System;

namespace KernellClientUI
{
	// Token: 0x02000027 RID: 39
	public static class EnumExtensions
	{
		// Token: 0x0600018E RID: 398 RVA: 0x00008F84 File Offset: 0x00007184
		public static int ToInt<T>(T value) where T : Enum
		{
			return (int)((object)value);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00008FA4 File Offset: 0x000071A4
		public static bool HasFlag<T>(T one, T other) where T : Enum
		{
			return (EnumExtensions.ToInt<T>(one) & EnumExtensions.ToInt<T>(other)) == EnumExtensions.ToInt<T>(other);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00008FCC File Offset: 0x000071CC
		public static T RemoveFlag<T>(T one, T other) where T : Enum
		{
			return (T)((object)Enum.ToObject(typeof(T), EnumExtensions.ToInt<T>(one) & ~EnumExtensions.ToInt<T>(other)));
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009000 File Offset: 0x00007200
		public static T AddFlag<T>(T one, T other) where T : Enum
		{
			return (T)((object)Enum.ToObject(typeof(T), EnumExtensions.ToInt<T>(one) | EnumExtensions.ToInt<T>(other)));
		}
	}
}
