using System;
using System.Linq;
using System.Reflection;

namespace KernellClientUI.VRChat
{
	// Token: 0x02000030 RID: 48
	public class VRCUiManagerEx
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000B9E0 File Offset: 0x00009BE0
		public static VRCUiManager Instance
		{
			get
			{
				bool flag = VRCUiManagerEx._uiManagerInstance == null;
				if (flag)
				{
					VRCUiManagerEx._uiManagerInstance = (VRCUiManager)Enumerable.First<MethodInfo>(typeof(VRCUiManager).GetMethods(), (MethodInfo x) => x.ReturnType == typeof(VRCUiManager)).Invoke(null, new object[0]);
				}
				return VRCUiManagerEx._uiManagerInstance;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000210 RID: 528 RVA: 0x0000BA51 File Offset: 0x00009C51
		public static bool IsOpen
		{
			get
			{
				return VRCUiManagerEx.Instance.field_Private_Boolean_0;
			}
		}

		// Token: 0x040000E9 RID: 233
		private static VRCUiManager _uiManagerInstance;
	}
}
