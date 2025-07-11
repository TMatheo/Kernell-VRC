using System;
using KernelVRC;
using UnityEngine;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000092 RID: 146
	[Serializable]
	internal class TagInfo
	{
		// Token: 0x06000725 RID: 1829 RVA: 0x0002D284 File Offset: 0x0002B484
		public Color GetUnityColor()
		{
			bool flag = this._unityColor != null;
			Color value;
			if (flag)
			{
				value = this._unityColor.Value;
			}
			else
			{
				try
				{
					bool flag2 = string.IsNullOrEmpty(this.color);
					if (flag2)
					{
						this._unityColor = new Color?(TagConstants.DEFAULT_TAG_COLOR);
						value = this._unityColor.Value;
					}
					else
					{
						Color value2;
						bool flag3 = ColorUtility.TryParseHtmlString("#" + this.color.Replace("#", ""), ref value2);
						if (flag3)
						{
							this._unityColor = new Color?(value2);
							value = this._unityColor.Value;
						}
						else
						{
							this._unityColor = new Color?(TagConstants.DEFAULT_TAG_COLOR);
							value = this._unityColor.Value;
						}
					}
				}
				catch (Exception ex)
				{
					kernelllogger.Error("[TagInfo] Error parsing color '" + this.color + "': " + ex.Message);
					this._unityColor = new Color?(TagConstants.DEFAULT_TAG_COLOR);
					value = this._unityColor.Value;
				}
			}
			return value;
		}

		// Token: 0x04000384 RID: 900
		public string text;

		// Token: 0x04000385 RID: 901
		public string color;

		// Token: 0x04000386 RID: 902
		private Color? _unityColor;
	}
}
