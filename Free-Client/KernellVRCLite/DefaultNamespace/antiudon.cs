using System;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;
using VRC.Udon;

namespace DefaultNamespace
{
	// Token: 0x02000022 RID: 34
	public class antiudon : KernelModuleBase
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600017A RID: 378 RVA: 0x00008C6D File Offset: 0x00006E6D
		public override string ModuleName
		{
			get
			{
				return "AntiUdon";
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600017B RID: 379 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600017C RID: 380 RVA: 0x00008C74 File Offset: 0x00006E74
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.UdonEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00008C7C File Offset: 0x00006E7C
		public override void OnUiManagerInit()
		{
			this._safetycat = MenuSetup._uiManager.QMMenu.GetCategoryPage("Security").GetCategory("Security");
			this._safetycat.AddToggle("Anti Udon", "Disables all global udon events.", delegate(bool b)
			{
				this.au = b;
			});
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00008CD0 File Offset: 0x00006ED0
		public override bool OnUdonPatch(UdonBehaviour instance, string program)
		{
			bool flag = this.au;
			return !flag;
		}

		// Token: 0x04000082 RID: 130
		private bool au = false;

		// Token: 0x04000083 RID: 131
		private ReMenuCategory _safetycat;
	}
}
