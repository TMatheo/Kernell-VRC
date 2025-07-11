using System;
using UnityEngine;

namespace KernellClientUI.Unity
{
	// Token: 0x02000031 RID: 49
	public static class ColorExtensions
	{
		// Token: 0x06000212 RID: 530 RVA: 0x0000BA60 File Offset: 0x00009C60
		public static string ToHex(Color color)
		{
			return ColorUtility.ToHtmlStringRGB(color);
		}
	}
}
