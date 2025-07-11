using System;
using UnityEngine;

namespace KernellClientUI.Managers
{
	// Token: 0x0200006E RID: 110
	public static class ResourceManager
	{
		// Token: 0x06000494 RID: 1172 RVA: 0x0001A494 File Offset: 0x00018694
		public static Sprite LoadSpriteFromByteArray(byte[] bytes, int width = 512, int height = 512)
		{
			bool flag = bytes == null || bytes.Length == 0;
			Sprite result;
			if (flag)
			{
				result = null;
			}
			else
			{
				Texture2D texture2D = new Texture2D(width, height);
				bool flag2 = !ImageConversion.LoadImage(texture2D, bytes);
				if (flag2)
				{
					result = null;
				}
				else
				{
					Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0f, 0f), 100000f, 1000U, 0, Vector4.zero, false);
					sprite.hideFlags |= 32;
					result = sprite;
				}
			}
			return result;
		}
	}
}
