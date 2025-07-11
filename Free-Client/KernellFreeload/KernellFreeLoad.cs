using System;
using KernellVRCFL.Configuration;
using KernellVRCFL.Core;
using KernellVRCFL.Utilities;
using MelonLoader;

namespace KernellVRCFL
{
	// Token: 0x02000002 RID: 2
	public class KernellFreeLoad : MelonPlugin
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public override void OnPreModsLoaded()
		{
			_01Il1_lllO_1I010O11.__10_00_1lI1OIl_I_Ol();
			___1I_l_OI1__Il_01IO.___lO11I1O___lll_IlO();
			__I_O_Il1_l1111OO_Il._1_1l1l__1OI11_l_0_I();
			this._resourceManager.___OI_IOIOII___Ol_0O();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x0000214C File Offset: 0x0000034C
		public override void OnApplicationStart()
		{
			_O0OI1Ol111_11OO_lIO.____O_O_0O_1_I_I0lIO();
			this._resourceManager.__O1IOIO_l1I0O_OOO_O();
			__Ill0__1__Ol__lIO__.____0lO__OlI0I0_111I();
			if (this._resourceManager.__l_l_1l_IOI0O_lI__I && this._resourceManager.___O_1l10___1___l1O1 != null)
			{
				__Ill0__1__Ol__lIO__.__0_II_O_0111I_O0_01(this._resourceManager.___O_1l10___1___l1O1);
			}
			else
			{
				MelonLogger.Error("The client failed to download or has been nullified, Contact kernell support.");
			}
		}

		// Token: 0x04000001 RID: 1
		private readonly _010_111_0_llI1O_00I _resourceManager = new _010_111_0_llI1O_00I();
	}
}
