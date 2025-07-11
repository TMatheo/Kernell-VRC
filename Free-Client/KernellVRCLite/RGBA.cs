using System;

// Token: 0x0200001A RID: 26
public struct RGBA
{
	// Token: 0x060000FD RID: 253 RVA: 0x00006C3D File Offset: 0x00004E3D
	public RGBA(byte red, byte green, byte blue, byte alpha)
	{
		this.R = red;
		this.G = green;
		this.B = blue;
		this.A = alpha;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00006C60 File Offset: 0x00004E60
	public override bool Equals(object obj)
	{
		bool flag = obj.GetType() == typeof(RGBA);
		bool result;
		if (flag)
		{
			bool flag2 = this.R == ((RGBA)obj).R && this.G == ((RGBA)obj).G && this.B == ((RGBA)obj).B && this.A == ((RGBA)obj).A;
			result = flag2;
		}
		else
		{
			bool flag3 = obj.GetType() == typeof(byte[]);
			if (flag3)
			{
				bool flag4 = this.R == ((byte[])obj)[0] && this.G == ((byte[])obj)[1] && this.B == ((byte[])obj)[2] && this.A == ((byte[])obj)[3];
				result = flag4;
			}
			else
			{
				result = false;
			}
		}
		return result;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006D58 File Offset: 0x00004F58
	public static bool operator ==(RGBA a, RGBA b)
	{
		return a.Equals(b);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00006D88 File Offset: 0x00004F88
	public static bool operator !=(RGBA a, RGBA b)
	{
		return !a.Equals(b);
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006DBC File Offset: 0x00004FBC
	public override int GetHashCode()
	{
		return ((int)this.R << 24) + ((int)this.G << 16) + ((int)this.B << 8) + (int)this.A;
	}

	// Token: 0x0400004E RID: 78
	public byte R;

	// Token: 0x0400004F RID: 79
	public byte G;

	// Token: 0x04000050 RID: 80
	public byte B;

	// Token: 0x04000051 RID: 81
	public byte A;
}
