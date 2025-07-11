using System;
using KernelVRC;

namespace KernellVRCLite.modules.uimods
{
	// Token: 0x020000A5 RID: 165
	public class LaunchpadModule : KernelModuleBase
	{
		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00034986 File Offset: 0x00032B86
		public override string ModuleName
		{
			get
			{
				return "LAUNCHPAD";
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000877 RID: 2167 RVA: 0x000036A0 File Offset: 0x000018A0
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.GUI | ModuleCapabilities.PlayerEvents | ModuleCapabilities.WorldEvents | ModuleCapabilities.NetworkEvents | ModuleCapabilities.MenuEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit | ModuleCapabilities.UserLogin;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x00030694 File Offset: 0x0002E894
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every60Frames;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x0003498D File Offset: 0x00032B8D
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.High;
			}
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x00034994 File Offset: 0x00032B94
		public override void OnInitialize()
		{
			this.textGlitcher = new KernellTextGlitch();
			this.uiModifier = new LaunchpadUIModifier();
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x000349B0 File Offset: 0x00032BB0
		public override void OnMenuOpened()
		{
			try
			{
				this.menuOpenCount++;
				bool flag = this.menuOpenCount > 1;
				if (flag)
				{
					this.textGlitcher.ApplyGlitchEffect();
					this.uiModifier.ApplyUIModifications();
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600087C RID: 2172 RVA: 0x00034A0C File Offset: 0x00032C0C
		public override void OnMenuClosed()
		{
			try
			{
				bool flag = this.menuOpenCount > 1;
				if (flag)
				{
					this.textGlitcher.StopGlitchEffect();
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000414 RID: 1044
		private KernellTextGlitch textGlitcher;

		// Token: 0x04000415 RID: 1045
		private LaunchpadUIModifier uiModifier;

		// Token: 0x04000416 RID: 1046
		private int menuOpenCount = 0;

		// Token: 0x04000417 RID: 1047
		private const int SKIP_FIRST_OPENS = 1;
	}
}
