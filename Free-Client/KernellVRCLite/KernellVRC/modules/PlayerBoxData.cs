using System;
using UnityEngine;
using VRC;

namespace KernellVRC.Modules
{
	// Token: 0x02000077 RID: 119
	internal class PlayerBoxData
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000558 RID: 1368 RVA: 0x000205E7 File Offset: 0x0001E7E7
		// (set) Token: 0x06000559 RID: 1369 RVA: 0x000205EF File Offset: 0x0001E7EF
		public Player Player { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x0600055A RID: 1370 RVA: 0x000205F8 File Offset: 0x0001E7F8
		// (set) Token: 0x0600055B RID: 1371 RVA: 0x00020600 File Offset: 0x0001E800
		public Color Color { get; set; }

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x00020609 File Offset: 0x0001E809
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x00020611 File Offset: 0x0001E811
		public Rect BoxRect { get; private set; }

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0002061A File Offset: 0x0001E81A
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x00020622 File Offset: 0x0001E822
		public bool IsValid { get; private set; }

		// Token: 0x06000560 RID: 1376 RVA: 0x0002062B File Offset: 0x0001E82B
		public PlayerBoxData(Player player, Color color)
		{
			this.Player = player;
			this.Color = color;
			this.IsValid = true;
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00020650 File Offset: 0x0001E850
		public void UpdateBox(Camera camera)
		{
			Player player = this.Player;
			bool flag = ((player != null) ? player.transform : null) == null || camera == null;
			if (flag)
			{
				this.IsValid = false;
			}
			else
			{
				try
				{
					Vector3 position = this.Player.transform.position;
					Vector3 vector = position + Vector3.up * 2.2f;
					Vector3 vector2 = position - Vector3.up * 0.2f;
					Vector3 vector3 = camera.WorldToScreenPoint(vector);
					Vector3 vector4 = camera.WorldToScreenPoint(vector2);
					bool flag2 = vector3.z <= 0f || vector4.z <= 0f;
					if (flag2)
					{
						this.IsValid = false;
					}
					else
					{
						vector3.y = (float)Screen.height - vector3.y;
						vector4.y = (float)Screen.height - vector4.y;
						float num = Mathf.Abs(vector4.y - vector3.y);
						float num2 = num * 0.4f;
						this.BoxRect = new Rect(vector3.x - num2 * 0.5f, vector3.y, num2, num);
						this.IsValid = true;
					}
				}
				catch
				{
					this.IsValid = false;
				}
			}
		}
	}
}
