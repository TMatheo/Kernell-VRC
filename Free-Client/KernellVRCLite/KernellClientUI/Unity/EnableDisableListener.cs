using System;
using System.Diagnostics;
using MelonLoader;
using UnhollowerBaseLib.Attributes;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace KernellClientUI.Unity
{
	// Token: 0x02000032 RID: 50
	[RegisterTypeInIl2Cpp]
	public class EnableDisableListener : MonoBehaviour
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000213 RID: 531 RVA: 0x0000BA78 File Offset: 0x00009C78
		// (remove) Token: 0x06000214 RID: 532 RVA: 0x0000BAB0 File Offset: 0x00009CB0
		[method: HideFromIl2Cpp]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnEnableEvent;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000215 RID: 533 RVA: 0x0000BAE8 File Offset: 0x00009CE8
		// (remove) Token: 0x06000216 RID: 534 RVA: 0x0000BB20 File Offset: 0x00009D20
		[method: HideFromIl2Cpp]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnDisableEvent;

		// Token: 0x06000217 RID: 535 RVA: 0x0000BB55 File Offset: 0x00009D55
		public EnableDisableListener(IntPtr obj) : base(obj)
		{
		}

		// Token: 0x06000218 RID: 536 RVA: 0x0000BB60 File Offset: 0x00009D60
		public void OnEnable()
		{
			Action onEnableEvent = this.OnEnableEvent;
			if (onEnableEvent != null)
			{
				onEnableEvent();
			}
		}

		// Token: 0x06000219 RID: 537 RVA: 0x0000BB75 File Offset: 0x00009D75
		public void OnDisable()
		{
			Action onDisableEvent = this.OnDisableEvent;
			if (onDisableEvent != null)
			{
				onDisableEvent();
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000BB8C File Offset: 0x00009D8C
		[HideFromIl2Cpp]
		public static void RegisterSafe()
		{
			bool registered = EnableDisableListener._registered;
			if (!registered)
			{
				try
				{
					ClassInjector.RegisterTypeInIl2Cpp<EnableDisableListener>();
					EnableDisableListener._registered = true;
				}
				catch (Exception)
				{
					EnableDisableListener._registered = true;
				}
			}
		}

		// Token: 0x040000EA RID: 234
		private static bool _registered;
	}
}
