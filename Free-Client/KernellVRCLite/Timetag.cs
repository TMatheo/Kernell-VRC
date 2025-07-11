using System;

// Token: 0x0200001C RID: 28
public struct Timetag
{
	// Token: 0x17000036 RID: 54
	// (get) Token: 0x06000109 RID: 265 RVA: 0x00006F28 File Offset: 0x00005128
	// (set) Token: 0x0600010A RID: 266 RVA: 0x00006F45 File Offset: 0x00005145
	public DateTime Timestamp
	{
		get
		{
			return Utils.TimetagToDateTime(this.Tag);
		}
		set
		{
			this.Tag = Utils.DateTimeToTimetag(value);
		}
	}

	// Token: 0x17000037 RID: 55
	// (get) Token: 0x0600010B RID: 267 RVA: 0x00006F54 File Offset: 0x00005154
	// (set) Token: 0x0600010C RID: 268 RVA: 0x00006F71 File Offset: 0x00005171
	public double Fraction
	{
		get
		{
			return Utils.TimetagToFraction(this.Tag);
		}
		set
		{
			this.Tag = (this.Tag & 18446744069414584320UL) + (ulong)((uint)(value * 4294967295.0));
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00006F98 File Offset: 0x00005198
	public Timetag(ulong value)
	{
		this.Tag = value;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00006FA2 File Offset: 0x000051A2
	public Timetag(DateTime value)
	{
		this.Tag = 0UL;
		this.Timestamp = value;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00006FB8 File Offset: 0x000051B8
	public override bool Equals(object obj)
	{
		bool flag = obj.GetType() == typeof(Timetag);
		bool result;
		if (flag)
		{
			bool flag2 = this.Tag == ((Timetag)obj).Tag;
			result = flag2;
		}
		else
		{
			bool flag3 = obj.GetType() == typeof(ulong);
			if (flag3)
			{
				bool flag4 = this.Tag == (ulong)obj;
				result = flag4;
			}
			else
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x0000703C File Offset: 0x0000523C
	public static bool operator ==(Timetag a, Timetag b)
	{
		return a.Equals(b);
	}

	// Token: 0x06000111 RID: 273 RVA: 0x0000706C File Offset: 0x0000526C
	public static bool operator !=(Timetag a, Timetag b)
	{
		return a.Equals(b);
	}

	// Token: 0x06000112 RID: 274 RVA: 0x0000709C File Offset: 0x0000529C
	public override int GetHashCode()
	{
		return ((int)(this.Tag >> 32) + (int)(this.Tag & (ulong)-1)) / 2;
	}

	// Token: 0x04000053 RID: 83
	public ulong Tag;
}
