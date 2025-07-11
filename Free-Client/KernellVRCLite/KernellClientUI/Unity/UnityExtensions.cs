using System;
using Il2CppSystem;
using Il2CppSystem.Collections;
using UnityEngine;

namespace KernellClientUI.Unity
{
	// Token: 0x02000034 RID: 52
	public static class UnityExtensions
	{
		// Token: 0x06000220 RID: 544 RVA: 0x0000BCA0 File Offset: 0x00009EA0
		public static string GetPath(Transform current)
		{
			bool flag = current.parent == null;
			string result;
			if (flag)
			{
				result = "/" + current.name;
			}
			else
			{
				result = UnityExtensions.GetPath(current.parent) + "/" + current.name;
			}
			return result;
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000BCF4 File Offset: 0x00009EF4
		public static T[] GetComponentsInDirectChildren<T>(GameObject gameObject)
		{
			int num = 0;
			IEnumerator enumerator = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Object @object = enumerator.Current;
					Transform transform = @object.Cast<Transform>();
					bool flag = transform.GetComponent<T>() != null;
					if (flag)
					{
						num++;
					}
				}
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				bool flag2 = disposable != null;
				if (flag2)
				{
					disposable.Dispose();
				}
			}
			T[] array = new T[num];
			num = 0;
			IEnumerator enumerator2 = gameObject.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					Object object2 = enumerator2.Current;
					Transform transform2 = object2.Cast<Transform>();
					bool flag3 = transform2.GetComponent<T>() != null;
					if (flag3)
					{
						array[num++] = transform2.GetComponent<T>();
					}
				}
			}
			finally
			{
				IDisposable disposable2 = enumerator2 as IDisposable;
				bool flag4 = disposable2 != null;
				if (flag4)
				{
					disposable2.Dispose();
				}
			}
			return array;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000BE18 File Offset: 0x0000A018
		public static bool IsAbsurd(float f)
		{
			return f <= -34028230f || f >= 34028230f;
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0000BE40 File Offset: 0x0000A040
		public static bool IsBad(Vector3 v3)
		{
			return float.IsNaN(v3.x) || float.IsNaN(v3.y) || float.IsNaN(v3.z) || float.IsInfinity(v3.x) || float.IsInfinity(v3.y) || float.IsInfinity(v3.z);
		}

		// Token: 0x06000224 RID: 548 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
		public static bool IsAbsurd(Vector3 v3)
		{
			return v3.x <= -34028230f || v3.x >= 34028230f || v3.y <= -34028230f || v3.y >= 34028230f || v3.z <= -34028230f || v3.z >= 34028230f;
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000BF0C File Offset: 0x0000A10C
		public static void Clamp(Vector3 v3)
		{
			v3.x = Mathf.Clamp(v3.x, -512000f, 512000f);
			v3.y = Mathf.Clamp(v3.y, -512000f, 512000f);
			v3.z = Mathf.Clamp(v3.z, -512000f, 512000f);
		}

		// Token: 0x06000226 RID: 550 RVA: 0x0000BF70 File Offset: 0x0000A170
		public static void Clamp(Quaternion v3)
		{
			v3.x = Mathf.Clamp(v3.x, -512000f, 512000f);
			v3.y = Mathf.Clamp(v3.y, -512000f, 512000f);
			v3.z = Mathf.Clamp(v3.z, -512000f, 512000f);
			v3.w = Mathf.Clamp(v3.w, -512000f, 512000f);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0000BFF0 File Offset: 0x0000A1F0
		public static string ToCleanString(Vector3 v3, string format = "F4")
		{
			return v3.ToString(format).Replace(" ", string.Empty).Trim(new char[]
			{
				'(',
				')'
			});
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0000C030 File Offset: 0x0000A230
		public static float RoundAmount(float i, float nearestFactor)
		{
			return (float)Math.Round((double)(i / nearestFactor)) * nearestFactor;
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000C050 File Offset: 0x0000A250
		public static Vector3 RoundAmount(Vector3 i, float nearestFactor)
		{
			return new Vector3(UnityExtensions.RoundAmount(i.x, nearestFactor), UnityExtensions.RoundAmount(i.y, nearestFactor), UnityExtensions.RoundAmount(i.z, nearestFactor));
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000C08C File Offset: 0x0000A28C
		public static Vector2 RoundAmount(Vector2 i, float nearestFactor)
		{
			return new Vector2(UnityExtensions.RoundAmount(i.x, nearestFactor), UnityExtensions.RoundAmount(i.y, nearestFactor));
		}

		// Token: 0x040000EF RID: 239
		public const float MaxAllowedValueTop = 34028230f;

		// Token: 0x040000F0 RID: 240
		public const float MaxAllowedValueBottom = -34028230f;
	}
}
