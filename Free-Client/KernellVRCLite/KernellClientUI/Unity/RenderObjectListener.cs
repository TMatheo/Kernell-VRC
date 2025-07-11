using System;
using System.Diagnostics;
using MelonLoader;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace KernellClientUI.Unity
{
	// Token: 0x02000033 RID: 51
	[RegisterTypeInIl2Cpp]
	public class RenderObjectListener : MonoBehaviour
	{
		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600021B RID: 539 RVA: 0x0000BBD4 File Offset: 0x00009DD4
		// (remove) Token: 0x0600021C RID: 540 RVA: 0x0000BC0C File Offset: 0x00009E0C
		[method: HideFromIl2Cpp]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action RenderObject;

		// Token: 0x0600021D RID: 541 RVA: 0x0000BB55 File Offset: 0x00009D55
		public RenderObjectListener(IntPtr obj0) : base(obj0)
		{
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000BC41 File Offset: 0x00009E41
		public void OnRenderObject()
		{
			Action renderObject = this.RenderObject;
			if (renderObject != null)
			{
				renderObject();
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000BC58 File Offset: 0x00009E58
		[HideFromIl2Cpp]
		public static void RegisterSafe()
		{
			bool registered = RenderObjectListener._registered;
			if (!registered)
			{
				try
				{
					ClassInjector.RegisterTypeInIl2Cpp<RenderObjectListener>();
					RenderObjectListener._registered = true;
				}
				catch (Exception)
				{
					RenderObjectListener._registered = true;
				}
			}
		}

		// Token: 0x040000ED RID: 237
		private static bool _registered;
	}
}
