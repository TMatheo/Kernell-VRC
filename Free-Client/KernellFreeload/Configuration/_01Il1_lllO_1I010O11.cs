using System;

namespace KernellVRCFL.Configuration
{
	// Token: 0x0200000A RID: 10
	internal static class _01Il1_lllO_1I010O11
	{
		// Token: 0x0600001E RID: 30 RVA: 0x00002BA4 File Offset: 0x00000DA4
		internal static void __10_00_1lI1OIl_I_Ol()
		{
			Random random = new Random();
			int num = random.Next(100, 200);
			for (int i = 0; i < num; i++)
			{
				double num2 = Math.Pow((double)i, 2.0) / ((double)num + 0.1);
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002BF4 File Offset: 0x00000DF4
		internal static bool _0OlO000l_1l_lOIOOl1()
		{
			return DateTime.Now.Ticks % 2L == 0L;
		}

		// Token: 0x04000030 RID: 48
		internal static readonly string _10_I_0001I___1_OI0O = "https://";

		// Token: 0x04000031 RID: 49
		private object __OlO1I1___I0O1l111l;

		// Token: 0x04000032 RID: 50
		private int _llOlOI_III_l__OIO1O;

		// Token: 0x04000033 RID: 51
		private object _00O01O_I10l_OlOOO0l;

		// Token: 0x04000034 RID: 52
		private string _lIl1IlI1llI_I11_I_I;

		// Token: 0x04000035 RID: 53
		private int _I_0_IIO__00______10;
	}
}
