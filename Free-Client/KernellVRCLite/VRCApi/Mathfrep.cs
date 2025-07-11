using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace VRCApi
{
	// Token: 0x0200001F RID: 31
	internal static class Mathfrep
	{
		// Token: 0x0600011E RID: 286 RVA: 0x000072A0 File Offset: 0x000054A0
		static Mathfrep()
		{
			Mathfrep.InitializeLookupTables();
			Mathfrep._mathWorkerThread = new Thread(new ThreadStart(Mathfrep.MathWorkerLoop))
			{
				Name = "MathWorker",
				IsBackground = true,
				Priority = ThreadPriority.BelowNormal
			};
			Mathfrep._mathWorkerThread.Start();
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000073A0 File Offset: 0x000055A0
		private static void InitializeLookupTables()
		{
			for (int i = 0; i < 3600; i++)
			{
				float num = (float)i / 10f * 0.017453292f;
				Mathfrep._sinLookup[i] = (float)Math.Sin((double)num);
				Mathfrep._cosLookup[i] = (float)Math.Cos((double)num);
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x000073F4 File Offset: 0x000055F4
		private static void MathWorkerLoop()
		{
			while (!Mathfrep._isShuttingDown)
			{
				try
				{
					Mathfrep._mathEvent.WaitOne();
					for (;;)
					{
						Mathfrep.MathOperation mathOperation;
						bool flag = Mathfrep._mathQueue.TryDequeue(out mathOperation);
						if (!flag)
						{
							break;
						}
						try
						{
							Mathfrep.MathOperationType type = mathOperation.Type;
							if (!true)
							{
							}
							float num;
							switch (type)
							{
							case Mathfrep.MathOperationType.Pow:
								num = Mathfrep.ComputePow(mathOperation.Value1, mathOperation.Value2);
								break;
							case Mathfrep.MathOperationType.Sqrt:
								num = Mathfrep.ComputeSqrt(mathOperation.Value1);
								break;
							case Mathfrep.MathOperationType.Sin:
								num = Mathfrep.ComputeSin(mathOperation.Value1);
								break;
							case Mathfrep.MathOperationType.Cos:
								num = Mathfrep.ComputeCos(mathOperation.Value1);
								break;
							case Mathfrep.MathOperationType.Tan:
								num = Mathfrep.ComputeTan(mathOperation.Value1);
								break;
							case Mathfrep.MathOperationType.Asin:
								num = Mathfrep.ComputeAsin(mathOperation.Value1);
								break;
							case Mathfrep.MathOperationType.Acos:
								num = Mathfrep.ComputeAcos(mathOperation.Value1);
								break;
							case Mathfrep.MathOperationType.Atan:
								num = Mathfrep.ComputeAtan(mathOperation.Value1);
								break;
							case Mathfrep.MathOperationType.Atan2:
								num = Mathfrep.ComputeAtan2(mathOperation.Value1, mathOperation.Value2);
								break;
							default:
								num = 0f;
								break;
							}
							if (!true)
							{
							}
							float result = num;
							mathOperation.CompletionSource.SetResult(result);
						}
						catch (Exception exception)
						{
							mathOperation.CompletionSource.SetException(exception);
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007578 File Offset: 0x00005778
		private static Task<float> QueueMathOperation(Mathfrep.MathOperationType type, float value1, float value2 = 0f)
		{
			TaskCompletionSource<float> taskCompletionSource = new TaskCompletionSource<float>();
			Mathfrep.MathOperation item = new Mathfrep.MathOperation
			{
				Type = type,
				Value1 = value1,
				Value2 = value2,
				CompletionSource = taskCompletionSource
			};
			Mathfrep._mathQueue.Enqueue(item);
			Mathfrep._mathEvent.Set();
			return taskCompletionSource.Task;
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000075D8 File Offset: 0x000057D8
		private static float ComputePow(float baseValue, float exponent)
		{
			bool flag = exponent == 0f;
			float result;
			if (flag)
			{
				result = 1f;
			}
			else
			{
				bool flag2 = exponent == 1f;
				if (flag2)
				{
					result = baseValue;
				}
				else
				{
					bool flag3 = baseValue == 0f;
					if (flag3)
					{
						result = 0f;
					}
					else
					{
						bool flag4 = baseValue == 1f;
						if (flag4)
						{
							result = 1f;
						}
						else
						{
							ValueTuple<float, float> valueTuple = new ValueTuple<float, float>(baseValue, exponent);
							float num;
							bool flag5 = Mathfrep._powCache.TryGetValue(valueTuple, ref num);
							if (flag5)
							{
								result = num;
							}
							else
							{
								float num2 = (float)Math.Pow((double)baseValue, (double)exponent);
								bool flag6 = Mathfrep._powCache.Count < 1000;
								if (flag6)
								{
									Mathfrep._powCache.TryAdd(valueTuple, num2);
								}
								result = num2;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000769C File Offset: 0x0000589C
		private static float ComputeSqrt(float value)
		{
			bool flag = value < 0f;
			float result;
			if (flag)
			{
				result = float.NaN;
			}
			else
			{
				bool flag2 = value == 0f || value == 1f;
				if (flag2)
				{
					result = value;
				}
				else
				{
					float num;
					bool flag3 = Mathfrep._sqrtCache.TryGetValue(value, ref num);
					if (flag3)
					{
						result = num;
					}
					else
					{
						float num2 = (float)Math.Sqrt((double)value);
						bool flag4 = Mathfrep._sqrtCache.Count < 1000;
						if (flag4)
						{
							Mathfrep._sqrtCache.TryAdd(value, num2);
						}
						result = num2;
					}
				}
			}
			return result;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00007724 File Offset: 0x00005924
		private static float ComputeSin(float radians)
		{
			float num;
			bool flag = Mathfrep._sinCache.TryGetValue(radians, ref num);
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				float num2 = (float)Math.Sin((double)radians);
				bool flag2 = Mathfrep._sinCache.Count < 1000;
				if (flag2)
				{
					Mathfrep._sinCache.TryAdd(radians, num2);
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000777C File Offset: 0x0000597C
		private static float ComputeCos(float radians)
		{
			float num;
			bool flag = Mathfrep._cosCache.TryGetValue(radians, ref num);
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				float num2 = (float)Math.Cos((double)radians);
				bool flag2 = Mathfrep._cosCache.Count < 1000;
				if (flag2)
				{
					Mathfrep._cosCache.TryAdd(radians, num2);
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000077D4 File Offset: 0x000059D4
		private static float ComputeTan(float radians)
		{
			float num;
			bool flag = Mathfrep._tanCache.TryGetValue(radians, ref num);
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				float num2 = (float)Math.Tan((double)radians);
				bool flag2 = Mathfrep._tanCache.Count < 1000;
				if (flag2)
				{
					Mathfrep._tanCache.TryAdd(radians, num2);
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000782C File Offset: 0x00005A2C
		private static float ComputeAsin(float value)
		{
			float num;
			bool flag = Mathfrep._asinCache.TryGetValue(value, ref num);
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				float num2 = (float)Math.Asin((double)value);
				bool flag2 = Mathfrep._asinCache.Count < 1000;
				if (flag2)
				{
					Mathfrep._asinCache.TryAdd(value, num2);
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007884 File Offset: 0x00005A84
		private static float ComputeAcos(float value)
		{
			float num;
			bool flag = Mathfrep._acosCache.TryGetValue(value, ref num);
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				float num2 = (float)Math.Acos((double)value);
				bool flag2 = Mathfrep._acosCache.Count < 1000;
				if (flag2)
				{
					Mathfrep._acosCache.TryAdd(value, num2);
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x000078DC File Offset: 0x00005ADC
		private static float ComputeAtan(float value)
		{
			float num;
			bool flag = Mathfrep._atanCache.TryGetValue(value, ref num);
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				float num2 = (float)Math.Atan((double)value);
				bool flag2 = Mathfrep._atanCache.Count < 1000;
				if (flag2)
				{
					Mathfrep._atanCache.TryAdd(value, num2);
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x00007934 File Offset: 0x00005B34
		private static float ComputeAtan2(float y, float x)
		{
			ValueTuple<float, float> valueTuple = new ValueTuple<float, float>(y, x);
			float num;
			bool flag = Mathfrep._atan2Cache.TryGetValue(valueTuple, ref num);
			float result;
			if (flag)
			{
				result = num;
			}
			else
			{
				float num2 = (float)Math.Atan2((double)y, (double)x);
				bool flag2 = Mathfrep._atan2Cache.Count < 1000;
				if (flag2)
				{
					Mathfrep._atan2Cache.TryAdd(valueTuple, num2);
				}
				result = num2;
			}
			return result;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00007998 File Offset: 0x00005B98
		private static float WaitForAsyncResult(Task<float> task)
		{
			float result;
			try
			{
				bool flag = task.Wait(100);
				if (!flag)
				{
					throw new TimeoutException("Async math operation timed out");
				}
				result = task.Result;
			}
			catch (AggregateException ex)
			{
				throw ex.InnerException ?? ex;
			}
			return result;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000079E8 File Offset: 0x00005BE8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Abs(float value)
		{
			return (value < 0f) ? (-value) : value;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000079F7 File Offset: 0x00005BF7
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float value, float min, float max)
		{
			return (value < min) ? min : ((value > max) ? max : value);
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007A08 File Offset: 0x00005C08
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Min(float a, float b)
		{
			return (a < b) ? a : b;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00007A08 File Offset: 0x00005C08
		public static int Min(int a, int b)
		{
			return (a < b) ? a : b;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00007A12 File Offset: 0x00005C12
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Max(float a, float b)
		{
			return (a > b) ? a : b;
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00007A1C File Offset: 0x00005C1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Lerp(float a, float b, float t)
		{
			t = Mathfrep.Clamp(t, 0f, 1f);
			return a + (b - a) * t;
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00007A47 File Offset: 0x00005C47
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpUnclamped(float a, float b, float t)
		{
			return a + (b - a) * t;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00007A50 File Offset: 0x00005C50
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MoveTowards(float current, float target, float maxDelta)
		{
			float num = target - current;
			float num2 = Mathfrep.Abs(num);
			bool flag = num2 <= maxDelta;
			float result;
			if (flag)
			{
				result = target;
			}
			else
			{
				result = current + ((num > 0f) ? maxDelta : (-maxDelta));
			}
			return result;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00007A8B File Offset: 0x00005C8B
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Repeat(float value, float length)
		{
			return (value % length + length) % length;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00007A94 File Offset: 0x00005C94
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SmoothStep(float from, float to, float t)
		{
			t = Mathfrep.Clamp(t, 0f, 1f);
			t = t * t * (3f - 2f * t);
			return from + (to - from) * t;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00007AD2 File Offset: 0x00005CD2
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RadiansToDegrees(float radians)
		{
			return radians * 57.29578f;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00007ADB File Offset: 0x00005CDB
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DegreesToRadians(float degrees)
		{
			return degrees * 0.017453292f;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00007AE4 File Offset: 0x00005CE4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float NormalizeAngle(float angle)
		{
			return (angle % 360f + 360f) % 360f;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00007AF9 File Offset: 0x00005CF9
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sign(float value)
		{
			return (value < 0f) ? -1 : ((value > 0f) ? 1 : 0);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00007B12 File Offset: 0x00005D12
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RoundToInt(float value)
		{
			return (int)Math.Round((double)value);
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00007B1C File Offset: 0x00005D1C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FloorToInt(float value)
		{
			return (int)Math.Floor((double)value);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00007B26 File Offset: 0x00005D26
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CeilToInt(float value)
		{
			return (int)Math.Ceiling((double)value);
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00007B30 File Offset: 0x00005D30
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SinFast(float degrees)
		{
			degrees = Mathfrep.NormalizeAngle(degrees);
			int num = (int)(degrees * 10f);
			bool flag = num >= 3600;
			if (flag)
			{
				num = 3599;
			}
			return Mathfrep._sinLookup[num];
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00007B70 File Offset: 0x00005D70
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float CosFast(float degrees)
		{
			degrees = Mathfrep.NormalizeAngle(degrees);
			int num = (int)(degrees * 10f);
			bool flag = num >= 3600;
			if (flag)
			{
				num = 3599;
			}
			return Mathfrep._cosLookup[num];
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00007BAF File Offset: 0x00005DAF
		public static Task<float> SqrtAsync(float value)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Sqrt, value, 0f);
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00007BBD File Offset: 0x00005DBD
		public static Task<float> PowAsync(float baseValue, float exponent)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Pow, baseValue, exponent);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00007BC7 File Offset: 0x00005DC7
		public static Task<float> SinAsync(float radians)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Sin, radians, 0f);
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00007BD5 File Offset: 0x00005DD5
		public static Task<float> CosAsync(float radians)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Cos, radians, 0f);
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00007BE3 File Offset: 0x00005DE3
		public static Task<float> TanAsync(float radians)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Tan, radians, 0f);
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00007BF1 File Offset: 0x00005DF1
		public static Task<float> AsinAsync(float value)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Asin, value, 0f);
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00007BFF File Offset: 0x00005DFF
		public static Task<float> AcosAsync(float value)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Acos, value, 0f);
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00007C0D File Offset: 0x00005E0D
		public static Task<float> AtanAsync(float value)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Atan, value, 0f);
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00007C1B File Offset: 0x00005E1B
		public static Task<float> Atan2Async(float y, float x)
		{
			return Mathfrep.QueueMathOperation(Mathfrep.MathOperationType.Atan2, y, x);
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00007C25 File Offset: 0x00005E25
		public static float Sqrt(float value)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.SqrtAsync(value));
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00007C32 File Offset: 0x00005E32
		public static float Pow(float baseValue, float exponent)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.PowAsync(baseValue, exponent));
		}

		// Token: 0x0600014A RID: 330 RVA: 0x00007C40 File Offset: 0x00005E40
		public static float Sin(float radians)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.SinAsync(radians));
		}

		// Token: 0x0600014B RID: 331 RVA: 0x00007C4D File Offset: 0x00005E4D
		public static float Cos(float radians)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.CosAsync(radians));
		}

		// Token: 0x0600014C RID: 332 RVA: 0x00007C5A File Offset: 0x00005E5A
		public static float Tan(float radians)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.TanAsync(radians));
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00007C67 File Offset: 0x00005E67
		public static float Acos(float value)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.AcosAsync(value));
		}

		// Token: 0x0600014E RID: 334 RVA: 0x00007C74 File Offset: 0x00005E74
		public static float Asin(float value)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.AsinAsync(value));
		}

		// Token: 0x0600014F RID: 335 RVA: 0x00007C81 File Offset: 0x00005E81
		public static float Atan(float value)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.AtanAsync(value));
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00007C8E File Offset: 0x00005E8E
		public static float Atan2(float y, float x)
		{
			return Mathfrep.WaitForAsyncResult(Mathfrep.Atan2Async(y, x));
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00007C9C File Offset: 0x00005E9C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RandomRange(float min, float max)
		{
			Random value = Mathfrep._threadLocalRandom.Value;
			return min + (float)value.NextDouble() * (max - min);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00007CC6 File Offset: 0x00005EC6
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Random01()
		{
			return (float)Mathfrep._threadLocalRandom.Value.NextDouble();
		}

		// Token: 0x06000153 RID: 339 RVA: 0x00007CD8 File Offset: 0x00005ED8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RandomRangeInt(int min, int max)
		{
			return Mathfrep._threadLocalRandom.Value.Next(min, max);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00007CEC File Offset: 0x00005EEC
		public static float Distance2D(float x1, float y1, float x2, float y2)
		{
			float num = x2 - x1;
			float num2 = y2 - y1;
			return Mathfrep.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00007D14 File Offset: 0x00005F14
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared2D(float x1, float y1, float x2, float y2)
		{
			float num = x2 - x1;
			float num2 = y2 - y1;
			return num * num + num2 * num2;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00007D38 File Offset: 0x00005F38
		public static float Distance3D(float x1, float y1, float z1, float x2, float y2, float z2)
		{
			float num = x2 - x1;
			float num2 = y2 - y1;
			float num3 = z2 - z1;
			return Mathfrep.Sqrt(num * num + num2 * num2 + num3 * num3);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00007D68 File Offset: 0x00005F68
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DistanceSquared3D(float x1, float y1, float z1, float x2, float y2, float z2)
		{
			float num = x2 - x1;
			float num2 = y2 - y1;
			float num3 = z2 - z1;
			return num * num + num2 * num2 + num3 * num3;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00007D94 File Offset: 0x00005F94
		[DebuggerStepThrough]
		public static Task<float> Distance2DAsync(float x1, float y1, float x2, float y2)
		{
			Mathfrep.<Distance2DAsync>d__83 <Distance2DAsync>d__ = new Mathfrep.<Distance2DAsync>d__83();
			<Distance2DAsync>d__.<>t__builder = AsyncTaskMethodBuilder<float>.Create();
			<Distance2DAsync>d__.x1 = x1;
			<Distance2DAsync>d__.y1 = y1;
			<Distance2DAsync>d__.x2 = x2;
			<Distance2DAsync>d__.y2 = y2;
			<Distance2DAsync>d__.<>1__state = -1;
			<Distance2DAsync>d__.<>t__builder.Start<Mathfrep.<Distance2DAsync>d__83>(ref <Distance2DAsync>d__);
			return <Distance2DAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00007DF0 File Offset: 0x00005FF0
		[DebuggerStepThrough]
		public static Task<float> Distance3DAsync(float x1, float y1, float z1, float x2, float y2, float z2)
		{
			Mathfrep.<Distance3DAsync>d__84 <Distance3DAsync>d__ = new Mathfrep.<Distance3DAsync>d__84();
			<Distance3DAsync>d__.<>t__builder = AsyncTaskMethodBuilder<float>.Create();
			<Distance3DAsync>d__.x1 = x1;
			<Distance3DAsync>d__.y1 = y1;
			<Distance3DAsync>d__.z1 = z1;
			<Distance3DAsync>d__.x2 = x2;
			<Distance3DAsync>d__.y2 = y2;
			<Distance3DAsync>d__.z2 = z2;
			<Distance3DAsync>d__.<>1__state = -1;
			<Distance3DAsync>d__.<>t__builder.Start<Mathfrep.<Distance3DAsync>d__84>(ref <Distance3DAsync>d__);
			return <Distance3DAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00007E5C File Offset: 0x0000605C
		public static void Shutdown()
		{
			Mathfrep._isShuttingDown = true;
			Mathfrep._mathEvent.Set();
			Thread mathWorkerThread = Mathfrep._mathWorkerThread;
			bool flag = mathWorkerThread != null && mathWorkerThread.IsAlive;
			if (flag)
			{
				Mathfrep._mathWorkerThread.Join(1000);
			}
			Mathfrep.ClearCaches();
			Mathfrep._threadLocalRandom.Dispose();
			Mathfrep._mathEvent.Dispose();
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00007EC0 File Offset: 0x000060C0
		public static void ClearCaches()
		{
			Mathfrep._powCache.Clear();
			Mathfrep._sqrtCache.Clear();
			Mathfrep._sinCache.Clear();
			Mathfrep._cosCache.Clear();
			Mathfrep._tanCache.Clear();
			Mathfrep._asinCache.Clear();
			Mathfrep._acosCache.Clear();
			Mathfrep._atanCache.Clear();
			Mathfrep._atan2Cache.Clear();
		}

		// Token: 0x04000058 RID: 88
		public const float PI = 3.1415927f;

		// Token: 0x04000059 RID: 89
		public const float Deg2Rad = 0.017453292f;

		// Token: 0x0400005A RID: 90
		public const float Rad2Deg = 57.29578f;

		// Token: 0x0400005B RID: 91
		public const float Epsilon = 1E-45f;

		// Token: 0x0400005C RID: 92
		private static readonly ThreadLocal<Random> _threadLocalRandom = new ThreadLocal<Random>(() => new Random(Guid.NewGuid().GetHashCode()));

		// Token: 0x0400005D RID: 93
		private static readonly Thread _mathWorkerThread;

		// Token: 0x0400005E RID: 94
		private static readonly ConcurrentQueue<Mathfrep.MathOperation> _mathQueue = new ConcurrentQueue<Mathfrep.MathOperation>();

		// Token: 0x0400005F RID: 95
		private static readonly AutoResetEvent _mathEvent = new AutoResetEvent(false);

		// Token: 0x04000060 RID: 96
		private static volatile bool _isShuttingDown = false;

		// Token: 0x04000061 RID: 97
		private static readonly float[] _sinLookup = new float[3600];

		// Token: 0x04000062 RID: 98
		private static readonly float[] _cosLookup = new float[3600];

		// Token: 0x04000063 RID: 99
		private const float LOOKUP_SCALE = 10f;

		// Token: 0x04000064 RID: 100
		private static readonly ConcurrentDictionary<ValueTuple<float, float>, float> _powCache = new ConcurrentDictionary<ValueTuple<float, float>, float>();

		// Token: 0x04000065 RID: 101
		private static readonly ConcurrentDictionary<float, float> _sqrtCache = new ConcurrentDictionary<float, float>();

		// Token: 0x04000066 RID: 102
		private static readonly ConcurrentDictionary<float, float> _sinCache = new ConcurrentDictionary<float, float>();

		// Token: 0x04000067 RID: 103
		private static readonly ConcurrentDictionary<float, float> _cosCache = new ConcurrentDictionary<float, float>();

		// Token: 0x04000068 RID: 104
		private static readonly ConcurrentDictionary<float, float> _tanCache = new ConcurrentDictionary<float, float>();

		// Token: 0x04000069 RID: 105
		private static readonly ConcurrentDictionary<float, float> _asinCache = new ConcurrentDictionary<float, float>();

		// Token: 0x0400006A RID: 106
		private static readonly ConcurrentDictionary<float, float> _acosCache = new ConcurrentDictionary<float, float>();

		// Token: 0x0400006B RID: 107
		private static readonly ConcurrentDictionary<float, float> _atanCache = new ConcurrentDictionary<float, float>();

		// Token: 0x0400006C RID: 108
		private static readonly ConcurrentDictionary<ValueTuple<float, float>, float> _atan2Cache = new ConcurrentDictionary<ValueTuple<float, float>, float>();

		// Token: 0x0400006D RID: 109
		private const int MAX_CACHE_SIZE = 1000;

		// Token: 0x0400006E RID: 110
		private const int ASYNC_TIMEOUT_MS = 100;

		// Token: 0x020000C8 RID: 200
		private struct MathOperation
		{
			// Token: 0x04000558 RID: 1368
			public Mathfrep.MathOperationType Type;

			// Token: 0x04000559 RID: 1369
			public float Value1;

			// Token: 0x0400055A RID: 1370
			public float Value2;

			// Token: 0x0400055B RID: 1371
			public TaskCompletionSource<float> CompletionSource;
		}

		// Token: 0x020000C9 RID: 201
		private enum MathOperationType
		{
			// Token: 0x0400055D RID: 1373
			Pow,
			// Token: 0x0400055E RID: 1374
			Sqrt,
			// Token: 0x0400055F RID: 1375
			Sin,
			// Token: 0x04000560 RID: 1376
			Cos,
			// Token: 0x04000561 RID: 1377
			Tan,
			// Token: 0x04000562 RID: 1378
			Asin,
			// Token: 0x04000563 RID: 1379
			Acos,
			// Token: 0x04000564 RID: 1380
			Atan,
			// Token: 0x04000565 RID: 1381
			Atan2
		}
	}
}
