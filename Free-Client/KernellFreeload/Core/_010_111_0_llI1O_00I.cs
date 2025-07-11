using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using KernellVRCFL.Utilities;
using MelonLoader;

namespace KernellVRCFL.Core
{
	// Token: 0x0200000E RID: 14
	internal class _010_111_0_llI1O_00I
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600002C RID: 44 RVA: 0x0000211A File Offset: 0x0000031A
		internal bool __l_l_1l_IOI0O_lI__I
		{
			get
			{
				return this.____O_______0II1O00l;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002122 File Offset: 0x00000322
		internal byte[] ___O_1l10___1___l1O1
		{
			get
			{
				return this.__Ol101l_O0_1_OI_O11;
			}
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000212A File Offset: 0x0000032A
		internal _010_111_0_llI1O_00I()
		{
			this._l__OI1I_01II_l1_Ol_();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x0000213F File Offset: 0x0000033F
		private void _l__OI1I_01II_l1_Ol_()
		{
			this.__1000ll0__1O0OI_O1I = _O_I_0llI_l1__l01I10.___1I_l1O1IOI_l0_O0I();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002D10 File Offset: 0x00000F10
		internal void ___OI_IOIOII___Ol_0O()
		{
			if (!_O0OI1Ol111_11OO_lIO.__l1l1OOlI1I__III___(this.__1000ll0__1O0OI_O1I))
			{
				MelonLogger.Error("Resource location is invalid.");
			}
			else
			{
				MelonLogger.Msg("Resource location valid");
				MelonLogger.Msg("Attempting to download from: " + this.__1000ll0__1O0OI_O1I);
				try
				{
					Task.Run(new Func<Task>(this.__0_IIIIOIO_OO_011_0)).Wait();
				}
				catch (Exception ex)
				{
					MelonLogger.Error(__0_1_O_I1O010I000II.__1l0O0OlOl_l_Il0O_l("", 61680) + ex.Message);
				}
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002DA4 File Offset: 0x00000FA4
		private Task __IOl_11_1l0l_0lO__0()
		{
			_010_111_0_llI1O_00I.<AttemptDownloadWithMultipleMethods>d__10 <AttemptDownloadWithMultipleMethods>d__ = new _010_111_0_llI1O_00I.<AttemptDownloadWithMultipleMethods>d__10();
			<AttemptDownloadWithMultipleMethods>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<AttemptDownloadWithMultipleMethods>d__.<>4__this = this;
			<AttemptDownloadWithMultipleMethods>d__.<>1__state = -1;
			<AttemptDownloadWithMultipleMethods>d__.<>t__builder.Start<_010_111_0_llI1O_00I.<AttemptDownloadWithMultipleMethods>d__10>(ref <AttemptDownloadWithMultipleMethods>d__);
			return <AttemptDownloadWithMultipleMethods>d__.<>t__builder.Task;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002DE8 File Offset: 0x00000FE8
		internal void __O1IOIO_l1I0O_OOO_O()
		{
			byte[] buffer = new byte[16];
			new Random().NextBytes(buffer);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002E08 File Offset: 0x00001008
		[CompilerGenerated]
		private Task __0_IIIIOIO_OO_011_0()
		{
			_010_111_0_llI1O_00I.<<InitiateResourceAcquisition>b__9_0>d <<InitiateResourceAcquisition>b__9_0>d = new _010_111_0_llI1O_00I.<<InitiateResourceAcquisition>b__9_0>d();
			<<InitiateResourceAcquisition>b__9_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
			<<InitiateResourceAcquisition>b__9_0>d.<>4__this = this;
			<<InitiateResourceAcquisition>b__9_0>d.<>1__state = -1;
			<<InitiateResourceAcquisition>b__9_0>d.<>t__builder.Start<_010_111_0_llI1O_00I.<<InitiateResourceAcquisition>b__9_0>d>(ref <<InitiateResourceAcquisition>b__9_0>d);
			return <<InitiateResourceAcquisition>b__9_0>d.<>t__builder.Task;
		}

		// Token: 0x0400003E RID: 62
		private string __1000ll0__1O0OI_O1I;

		// Token: 0x0400003F RID: 63
		private byte[] __Ol101l_O0_1_OI_O11;

		// Token: 0x04000040 RID: 64
		private bool ____O_______0II1O00l = false;

		// Token: 0x04000041 RID: 65
		private static object _1O_II1IO___I____1ll;

		// Token: 0x04000042 RID: 66
		private string _10__O1l0O_lO_lO01Ol;
	}
}
