using System;

// Token: 0x0200001B RID: 27
public class Symbol
{
	// Token: 0x06000102 RID: 258 RVA: 0x00006DF1 File Offset: 0x00004FF1
	public Symbol()
	{
		this.Value = "";
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00006E06 File Offset: 0x00005006
	public Symbol(string value)
	{
		this.Value = value;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00006E18 File Offset: 0x00005018
	public new string ToString()
	{
		return this.Value;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00006E30 File Offset: 0x00005030
	public override bool Equals(object obj)
	{
		bool flag = obj.GetType() == typeof(Symbol);
		bool result;
		if (flag)
		{
			bool flag2 = this.Value == ((Symbol)obj).Value;
			result = flag2;
		}
		else
		{
			bool flag3 = obj.GetType() == typeof(string);
			if (flag3)
			{
				bool flag4 = this.Value == (string)obj;
				result = flag4;
			}
			else
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00006EBC File Offset: 0x000050BC
	public static bool operator ==(Symbol a, Symbol b)
	{
		return a.Equals(b);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00006EE0 File Offset: 0x000050E0
	public static bool operator !=(Symbol a, Symbol b)
	{
		return !a.Equals(b);
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00006F08 File Offset: 0x00005108
	public override int GetHashCode()
	{
		return this.Value.GetHashCode();
	}

	// Token: 0x04000052 RID: 82
	public string Value;
}
