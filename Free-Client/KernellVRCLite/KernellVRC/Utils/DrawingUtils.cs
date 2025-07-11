using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KernellVRC.Utils
{
	// Token: 0x02000079 RID: 121
	public static class DrawingUtils
	{
		// Token: 0x06000570 RID: 1392 RVA: 0x00020F24 File Offset: 0x0001F124
		static DrawingUtils()
		{
			try
			{
				DrawingUtils.WhiteTexture = new Texture2D(1, 1, 4, false, false);
				DrawingUtils.WhiteTexture.SetPixel(0, 0, Color.white);
				DrawingUtils.WhiteTexture.Apply(false, true);
				DrawingUtils.WhiteTexture.hideFlags = 61;
				DrawingUtils.CirclePoints = new Vector2[9];
				DrawingUtils.PreCalculateCirclePoints();
				DrawingUtils.CapsulePoints = new Vector2[10];
				DrawingUtils.PreCalculateOutlineOffsets();
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("[DrawingUtils] Initialization failed: {0}", arg));
			}
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00020FE4 File Offset: 0x0001F1E4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void PreCalculateCirclePoints()
		{
			for (int i = 0; i <= 8; i++)
			{
				float num = (float)i * 0.7853982f;
				DrawingUtils.CirclePoints[i] = new Vector2(Mathf.Cos(num), Mathf.Sin(num));
			}
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0002102C File Offset: 0x0001F22C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void PreCalculateOutlineOffsets()
		{
			DrawingUtils.OutlineOffsets[0] = new Vector2(-1f, -1f);
			DrawingUtils.OutlineOffsets[1] = new Vector2(0f, -1f);
			DrawingUtils.OutlineOffsets[2] = new Vector2(1f, -1f);
			DrawingUtils.OutlineOffsets[3] = new Vector2(-1f, 0f);
			DrawingUtils.OutlineOffsets[4] = new Vector2(1f, 0f);
			DrawingUtils.OutlineOffsets[5] = new Vector2(-1f, 1f);
			DrawingUtils.OutlineOffsets[6] = new Vector2(0f, 1f);
			DrawingUtils.OutlineOffsets[7] = new Vector2(1f, 1f);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0002110A File Offset: 0x0001F30A
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetRect(float x, float y, float w, float h)
		{
			DrawingUtils.SharedRect.x = x;
			DrawingUtils.SharedRect.y = y;
			DrawingUtils.SharedRect.width = w;
			DrawingUtils.SharedRect.height = h;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00021140 File Offset: 0x0001F340
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void SetGuiColor(Color color)
		{
			bool flag = DrawingUtils.LastSetColor == null || !DrawingUtils.ColorsEqual(DrawingUtils.LastSetColor.Value, color);
			if (flag)
			{
				GUI.color = color;
				DrawingUtils.LastSetColor = new Color?(color);
			}
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00021188 File Offset: 0x0001F388
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool ColorsEqual(Color a, Color b)
		{
			return Mathf.Approximately(a.r, b.r) && Mathf.Approximately(a.g, b.g) && Mathf.Approximately(a.b, b.b) && Mathf.Approximately(a.a, b.a);
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000211E7 File Offset: 0x0001F3E7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void DrawTextureOptimized(float x, float y, float w, float h)
		{
			DrawingUtils.SetRect(x, y, w, h);
			GUI.DrawTexture(DrawingUtils.SharedRect, DrawingUtils.WhiteTexture, 0, false, 0f);
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x0002120C File Offset: 0x0001F40C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawBox(float x, float y, float w, float h, Color color, float thickness = 1f)
		{
			Color color2 = GUI.color;
			DrawingUtils.SetGuiColor(color);
			DrawingUtils.DrawTextureOptimized(x, y, w, thickness);
			DrawingUtils.DrawTextureOptimized(x, y + h - thickness, w, thickness);
			DrawingUtils.DrawTextureOptimized(x, y, thickness, h);
			DrawingUtils.DrawTextureOptimized(x + w - thickness, y, thickness, h);
			GUI.color = color2;
			DrawingUtils.LastSetColor = new Color?(color2);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00021270 File Offset: 0x0001F470
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawOutlinedBox(float x, float y, float w, float h, Color boxColor, Color outlineColor, float thickness = 2f)
		{
			Color color = GUI.color;
			DrawingUtils.SetGuiColor(outlineColor);
			float num = x - thickness;
			float num2 = y - thickness;
			float num3 = w + thickness * 2f;
			float num4 = h + thickness * 2f;
			DrawingUtils.DrawTextureOptimized(num, num2, num3, thickness);
			DrawingUtils.DrawTextureOptimized(num, num2 + num4 - thickness, num3, thickness);
			DrawingUtils.DrawTextureOptimized(num, num2, thickness, num4);
			DrawingUtils.DrawTextureOptimized(num + num3 - thickness, num2, thickness, num4);
			DrawingUtils.SetGuiColor(boxColor);
			DrawingUtils.DrawTextureOptimized(x, y, w, thickness);
			DrawingUtils.DrawTextureOptimized(x, y + h - thickness, w, thickness);
			DrawingUtils.DrawTextureOptimized(x, y, thickness, h);
			DrawingUtils.DrawTextureOptimized(x + w - thickness, y, thickness, h);
			GUI.color = color;
			DrawingUtils.LastSetColor = new Color?(color);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00021336 File Offset: 0x0001F536
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawPlayerBox(float x, float y, float width, float height, Color boxColor, Color outlineColor, float thickness = 3f)
		{
			DrawingUtils.DrawBox(x, y, width, height, boxColor, thickness);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00021348 File Offset: 0x0001F548
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawFilledBox(float x, float y, float w, float h, Color color)
		{
			Color color2 = GUI.color;
			DrawingUtils.SetGuiColor(color);
			DrawingUtils.DrawTextureOptimized(x, y, w, h);
			GUI.color = color2;
			DrawingUtils.LastSetColor = new Color?(color2);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00021380 File Offset: 0x0001F580
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float thickness = 1f, Color? outlineColor = null)
		{
			Color color2 = GUI.color;
			Matrix4x4 matrix = GUI.matrix;
			Vector2 vector = end - start;
			float magnitude = vector.magnitude;
			bool flag = magnitude < 0.1f;
			if (flag)
			{
				GUI.color = color2;
			}
			else
			{
				float angle = Mathf.Atan2(vector.y, vector.x) * 57.2958f;
				bool flag2 = outlineColor != null;
				if (flag2)
				{
					DrawingUtils.SetGuiColor(outlineColor.Value);
					DrawingUtils.DrawLineInternal(start, angle, magnitude, thickness + 2f);
				}
				DrawingUtils.SetGuiColor(color);
				DrawingUtils.DrawLineInternal(start, angle, magnitude, thickness);
				GUI.matrix = matrix;
				GUI.color = color2;
				DrawingUtils.LastSetColor = new Color?(color2);
			}
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00021433 File Offset: 0x0001F633
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void DrawLineInternal(Vector2 start, float angle, float length, float thickness)
		{
			GUIUtility.RotateAroundPivot(angle, start);
			DrawingUtils.DrawTextureOptimized(start.x, start.y - thickness * 0.5f, length, thickness);
			GUIUtility.RotateAroundPivot(-angle, start);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x00021464 File Offset: 0x0001F664
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawLineToItem(Vector2 itemScreenPos, Color color, float thickness = 1f)
		{
			Vector2 start;
			start..ctor((float)Screen.width * 0.5f, (float)Screen.height * 0.5f);
			DrawingUtils.DrawLine(start, itemScreenPos, color, thickness, null);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x000214A4 File Offset: 0x0001F6A4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void DrawOutlinedLabel(Rect rect, string text, Color textColor, Color outlineColor)
		{
			Color color = GUI.color;
			bool flag = DrawingUtils.CachedLabelStyle == null;
			if (flag)
			{
				DrawingUtils.CachedLabelStyle = new GUIStyle(GUI.skin.label)
				{
					alignment = 4
				};
			}
			DrawingUtils.SetGuiColor(outlineColor);
			for (int i = 0; i < 8; i++)
			{
				Vector2 vector = DrawingUtils.OutlineOffsets[i];
				DrawingUtils.SetRect(rect.x + vector.x, rect.y + vector.y, rect.width, rect.height);
				GUI.Label(DrawingUtils.SharedRect, text, DrawingUtils.CachedLabelStyle);
			}
			DrawingUtils.SetGuiColor(textColor);
			GUI.Label(rect, text, DrawingUtils.CachedLabelStyle);
			GUI.color = color;
			DrawingUtils.LastSetColor = new Color?(color);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00021570 File Offset: 0x0001F770
		public static void Cleanup()
		{
			bool flag = DrawingUtils.WhiteTexture != null;
			if (flag)
			{
				Object.DestroyImmediate(DrawingUtils.WhiteTexture);
			}
			DrawingUtils.LastSetColor = null;
		}

		// Token: 0x04000276 RID: 630
		private static readonly Texture2D WhiteTexture;

		// Token: 0x04000277 RID: 631
		private const int CIRCLE_SEGMENTS = 8;

		// Token: 0x04000278 RID: 632
		private static readonly Vector2[] CirclePoints;

		// Token: 0x04000279 RID: 633
		private static readonly Vector2[] CapsulePoints;

		// Token: 0x0400027A RID: 634
		private static Rect SharedRect = default(Rect);

		// Token: 0x0400027B RID: 635
		private static GUIStyle CachedLabelStyle;

		// Token: 0x0400027C RID: 636
		private static readonly Vector2[] OutlineOffsets = new Vector2[8];

		// Token: 0x0400027D RID: 637
		private static Color? LastSetColor = null;
	}
}
