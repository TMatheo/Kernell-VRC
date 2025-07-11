using System;
using System.Collections.Generic;
using System.Linq;

namespace KernellVRCFL.Configuration
{
	// Token: 0x0200000B RID: 11
	internal static class ___1I_l_OI1__Il_01IO
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002C24 File Offset: 0x00000E24
		internal static string _I100l0OlO_0IlO101II()
		{
			return string.Join(".", ___1I_l_OI1__Il_01IO.___0lIIOl11_O1_O_II_);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002C44 File Offset: 0x00000E44
		internal static void ___lO11I1O___lll_IlO()
		{
			List<string> list = Enumerable.ToList<string>(Enumerable.Select<string, string>(___1I_l_OI1__Il_01IO.___0lIIOl11_O1_O_II_, new Func<string, string>(___1I_l_OI1__Il_01IO.<>c.<>9.___O_0l1OlI0I0III__0)));
			list.Sort();
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002C88 File Offset: 0x00000E88
		internal static int ____0Ol0_10O___O1l0I()
		{
			return Enumerable.Sum<string>(___1I_l_OI1__Il_01IO.___0lIIOl11_O1_O_II_, new Func<string, int>(___1I_l_OI1__Il_01IO.<>c.<>9._Ol_O11__1O1OlIOI1lO));
		}

		// Token: 0x04000036 RID: 54
		private static readonly List<string> ___0lIIOl11_O1_O_II_ = new List<string>
		{
			"kernell",
			"net"
		};

		// Token: 0x04000037 RID: 55
		private string __OIll0I_O_1_00_Il0I;

		// Token: 0x04000038 RID: 56
		private int _OOII0OIlI__1OIIOI11;
	}
}
