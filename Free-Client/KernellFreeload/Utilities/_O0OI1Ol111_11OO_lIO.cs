using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace KernellVRCFL.Utilities
{
	// Token: 0x02000003 RID: 3
	internal static class _O0OI1Ol111_11OO_lIO
	{
		// Token: 0x06000004 RID: 4 RVA: 0x000021AC File Offset: 0x000003AC
		internal static Task<byte[]> __l1_1__0_0IOO_1O_0l(string ___O1l__01)
		{
			_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_WebClient>d__0 <FetchResourceAsync_WebClient>d__ = new _O0OI1Ol111_11OO_lIO.<FetchResourceAsync_WebClient>d__0();
			<FetchResourceAsync_WebClient>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<FetchResourceAsync_WebClient>d__.____00I1ll0l1II1_1_O = ___O1l__01;
			<FetchResourceAsync_WebClient>d__.<>1__state = -1;
			<FetchResourceAsync_WebClient>d__.<>t__builder.Start<_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_WebClient>d__0>(ref <FetchResourceAsync_WebClient>d__);
			return <FetchResourceAsync_WebClient>d__.<>t__builder.Task;
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000021F0 File Offset: 0x000003F0
		internal static Task<byte[]> __0000O__Ol__llIIlOI(string ___IO1lI01)
		{
			_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_HttpClient>d__1 <FetchResourceAsync_HttpClient>d__ = new _O0OI1Ol111_11OO_lIO.<FetchResourceAsync_HttpClient>d__1();
			<FetchResourceAsync_HttpClient>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<FetchResourceAsync_HttpClient>d__.___lO_O001OI0Ol0OO01 = ___IO1lI01;
			<FetchResourceAsync_HttpClient>d__.<>1__state = -1;
			<FetchResourceAsync_HttpClient>d__.<>t__builder.Start<_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_HttpClient>d__1>(ref <FetchResourceAsync_HttpClient>d__);
			return <FetchResourceAsync_HttpClient>d__.<>t__builder.Task;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002234 File Offset: 0x00000434
		internal static Task<byte[]> __01_I1ll_0O1_II1_0I(string ___10O_0Ol)
		{
			_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_HttpWebRequest>d__2 <FetchResourceAsync_HttpWebRequest>d__ = new _O0OI1Ol111_11OO_lIO.<FetchResourceAsync_HttpWebRequest>d__2();
			<FetchResourceAsync_HttpWebRequest>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<FetchResourceAsync_HttpWebRequest>d__.____II__O_0l01_____1 = ___10O_0Ol;
			<FetchResourceAsync_HttpWebRequest>d__.<>1__state = -1;
			<FetchResourceAsync_HttpWebRequest>d__.<>t__builder.Start<_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_HttpWebRequest>d__2>(ref <FetchResourceAsync_HttpWebRequest>d__);
			return <FetchResourceAsync_HttpWebRequest>d__.<>t__builder.Task;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002278 File Offset: 0x00000478
		internal static Task<byte[]> ___IIO__lO_10_1__O_I(string __1IIOI0_1)
		{
			_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_WebClientAlt>d__3 <FetchResourceAsync_WebClientAlt>d__ = new _O0OI1Ol111_11OO_lIO.<FetchResourceAsync_WebClientAlt>d__3();
			<FetchResourceAsync_WebClientAlt>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<FetchResourceAsync_WebClientAlt>d__.___1O_lO_I01IO1Il1IO = __1IIOI0_1;
			<FetchResourceAsync_WebClientAlt>d__.<>1__state = -1;
			<FetchResourceAsync_WebClientAlt>d__.<>t__builder.Start<_O0OI1Ol111_11OO_lIO.<FetchResourceAsync_WebClientAlt>d__3>(ref <FetchResourceAsync_WebClientAlt>d__);
			return <FetchResourceAsync_WebClientAlt>d__.<>t__builder.Task;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022BC File Offset: 0x000004BC
		internal static bool __l1l1OOlI1I__III___(string __l0__llll)
		{
			Uri uri;
			return Uri.TryCreate(__l0__llll, UriKind.Absolute, out uri);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x000022D4 File Offset: 0x000004D4
		internal static int _I__1l_1_O_OIllII___()
		{
			Random random = new Random();
			return random.Next(5, 100);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0000207F File Offset: 0x0000027F
		internal static void ____O_O_0O_1_I_I0lIO()
		{
			Thread.Sleep(10);
		}

		// Token: 0x04000002 RID: 2
		private object _1IIO_O_III1l1IlOOO_;

		// Token: 0x04000003 RID: 3
		private static bool _OI1O0_OlI_OO0I0OI01;

		// Token: 0x04000004 RID: 4
		private object _0_00__1_0___lIO_Ol1;
	}
}
