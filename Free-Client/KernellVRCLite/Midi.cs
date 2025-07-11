using System;

namespace KernellVRCLite
{
	// Token: 0x0200007E RID: 126
	public struct Midi
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x00022448 File Offset: 0x00020648
		public Midi(byte port, byte status, byte data1, byte data2)
		{
			this.Port = port;
			this.Status = status;
			this.Data1 = data1;
			this.Data2 = data2;
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x00022468 File Offset: 0x00020668
		public override bool Equals(object obj)
		{
			bool flag = obj.GetType() == typeof(Midi);
			bool result;
			if (flag)
			{
				bool flag2 = this.Port == ((Midi)obj).Port && this.Status == ((Midi)obj).Status && this.Data1 == ((Midi)obj).Data1 && this.Data2 == ((Midi)obj).Data2;
				result = flag2;
			}
			else
			{
				bool flag3 = obj.GetType() == typeof(byte[]);
				if (flag3)
				{
					bool flag4 = this.Port == ((byte[])obj)[0] && this.Status == ((byte[])obj)[1] && this.Data1 == ((byte[])obj)[2] && this.Data2 == ((byte[])obj)[3];
					result = flag4;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060005A1 RID: 1441 RVA: 0x00022560 File Offset: 0x00020760
		public static bool operator ==(Midi a, Midi b)
		{
			return a.Equals(b);
		}

		// Token: 0x060005A2 RID: 1442 RVA: 0x00022590 File Offset: 0x00020790
		public static bool operator !=(Midi a, Midi b)
		{
			return !a.Equals(b);
		}

		// Token: 0x060005A3 RID: 1443 RVA: 0x000225C4 File Offset: 0x000207C4
		public override int GetHashCode()
		{
			return ((int)this.Port << 24) + ((int)this.Status << 16) + ((int)this.Data1 << 8) + (int)this.Data2;
		}

		// Token: 0x04000284 RID: 644
		public byte Port;

		// Token: 0x04000285 RID: 645
		public byte Status;

		// Token: 0x04000286 RID: 646
		public byte Data1;

		// Token: 0x04000287 RID: 647
		public byte Data2;
	}
}
