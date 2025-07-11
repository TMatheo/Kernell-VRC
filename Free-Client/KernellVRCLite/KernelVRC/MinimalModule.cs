using System;

namespace KernelVRC
{
	// Token: 0x020000B3 RID: 179
	public sealed class MinimalModule : KernelModuleBase
	{
		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x00039186 File Offset: 0x00037386
		public override string ModuleName
		{
			get
			{
				return "Minimal Module";
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x000354F5 File Offset: 0x000336F5
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.None;
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x00038C95 File Offset: 0x00036E95
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.OnDemand;
			}
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x0003918D File Offset: 0x0003738D
		public override void OnInitialize()
		{
			base.OnInitialize();
			base.Log("Minimal module ready");
		}
	}
}
