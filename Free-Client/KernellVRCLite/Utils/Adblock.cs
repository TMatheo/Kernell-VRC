using System;
using KernelVRC;
using UnityEngine;

namespace KernellVRCLite.Utils
{
	// Token: 0x020000A8 RID: 168
	internal class Adblock : KernelModuleBase
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060008AC RID: 2220 RVA: 0x000354E7 File Offset: 0x000336E7
		public override string ModuleName
		{
			get
			{
				return "AdBlock";
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060008AD RID: 2221 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060008AE RID: 2222 RVA: 0x000354EE File Offset: 0x000336EE
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.PlayerEvents | ModuleCapabilities.MenuEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x00030694 File Offset: 0x0002E894
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every60Frames;
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060008B0 RID: 2224 RVA: 0x000354F5 File Offset: 0x000336F5
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.Lowest;
			}
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x000354F8 File Offset: 0x000336F8
		public override void OnMenuOpened()
		{
			try
			{
				GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/ScrollRect/Viewport/VerticalLayoutGroup/Carousel_Banners").active = false;
			}
			catch
			{
			}
		}
	}
}
