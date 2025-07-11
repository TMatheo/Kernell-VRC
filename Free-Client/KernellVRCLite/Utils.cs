using System;

// Token: 0x0200001E RID: 30
public class Utils
{
	// Token: 0x06000119 RID: 281 RVA: 0x00007180 File Offset: 0x00005380
	public static DateTime TimetagToDateTime(ulong val)
	{
		bool flag = val == 1UL;
		DateTime result;
		if (flag)
		{
			result = DateTime.Now;
		}
		else
		{
			uint num = (uint)(val >> 32);
			DateTime dateTime = DateTime.Parse("1900-01-01 00:00:00").AddSeconds(num);
			double value = Utils.TimetagToFraction(val);
			result = dateTime.AddSeconds(value);
		}
		return result;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x000071D4 File Offset: 0x000053D4
	public static double TimetagToFraction(ulong val)
	{
		bool flag = val == 1UL;
		double result;
		if (flag)
		{
			result = 0.0;
		}
		else
		{
			uint num = (uint)(val & (ulong)-1);
			result = num / 4294967295.0;
		}
		return result;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00007210 File Offset: 0x00005410
	public static ulong DateTimeToTimetag(DateTime value)
	{
		ulong num = (ulong)((uint)(value - DateTime.Parse("1900-01-01 00:00:00.000")).TotalSeconds);
		ulong num2 = (ulong)((uint)(4294967295.0 * ((double)value.Millisecond / 1000.0)));
		return (num << 32) + num2;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00007264 File Offset: 0x00005464
	public static int AlignedStringLength(string val)
	{
		int num = val.Length + (4 - val.Length % 4);
		bool flag = num <= val.Length;
		if (flag)
		{
			num += 4;
		}
		return num;
	}
}
