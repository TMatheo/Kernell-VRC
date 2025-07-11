using System;
using System.IO;

namespace KernellVRCFL.Core
{
	// Token: 0x0200000D RID: 13
	internal static class __Ill0__1__Ol__lIO__
	{
		// Token: 0x06000029 RID: 41 RVA: 0x0000210D File Offset: 0x0000030D
		internal static void __0_II_O_0111I_O0_01(byte[] ___I_O_11O)
		{
			File.WriteAllBytes("KernelEZ.dll", ___I_O_11O);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002CC0 File Offset: 0x00000EC0
		internal static void ____0lO__OlI0I0_111I()
		{
			for (int i = 0; i < 10; i++)
			{
				double num = Math.Sin((double)i) * Math.Cos((double)i);
			}
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002CEC File Offset: 0x00000EEC
		internal static string __1__lI_Il_I10OI_OIO()
		{
			return Guid.NewGuid().ToString();
		}

		// Token: 0x0400003C RID: 60
		private string _0_l_1OO0OO_Il1O001_;

		// Token: 0x0400003D RID: 61
		private bool __0O_O0__OIOOIOII100;
	}
}
