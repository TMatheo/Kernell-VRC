using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Token: 0x02000017 RID: 23
public class OscBundle : OscPacket
{
	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060000DB RID: 219 RVA: 0x00005A74 File Offset: 0x00003C74
	// (set) Token: 0x060000DC RID: 220 RVA: 0x00005A91 File Offset: 0x00003C91
	public ulong Timetag
	{
		get
		{
			return this._timetag.Tag;
		}
		set
		{
			this._timetag.Tag = value;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060000DD RID: 221 RVA: 0x00005AA0 File Offset: 0x00003CA0
	// (set) Token: 0x060000DE RID: 222 RVA: 0x00005ABD File Offset: 0x00003CBD
	public DateTime Timestamp
	{
		get
		{
			return this._timetag.Timestamp;
		}
		set
		{
			this._timetag.Timestamp = value;
		}
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00005ACD File Offset: 0x00003CCD
	public OscBundle(ulong timetag, params OscMessage[] args)
	{
		this._timetag = new Timetag(timetag);
		this.Messages = new List<OscMessage>();
		this.Messages.AddRange(args);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00005AFC File Offset: 0x00003CFC
	public override byte[] GetBytes()
	{
		string text = "#bundle";
		int num = Utils.AlignedStringLength(text);
		byte[] array = OscPacket.SetULong(this._timetag.Tag);
		List<byte[]> list = new List<byte[]>();
		foreach (OscMessage oscMessage in this.Messages)
		{
			list.Add(oscMessage.GetBytes());
		}
		int num2 = num + array.Length + Enumerable.Sum<byte[]>(list, (byte[] x) => x.Length + 4);
		int num3 = 0;
		byte[] array2 = new byte[num2];
		Encoding.UTF8.GetBytes(text).CopyTo(array2, num3);
		num3 += num;
		array.CopyTo(array2, num3);
		num3 += array.Length;
		foreach (byte[] array3 in list)
		{
			byte[] array4 = OscPacket.SetInt(array3.Length);
			array4.CopyTo(array2, num3);
			num3 += array4.Length;
			array3.CopyTo(array2, num3);
			num3 += array3.Length;
		}
		return array2;
	}

	// Token: 0x0400004A RID: 74
	private Timetag _timetag;

	// Token: 0x0400004B RID: 75
	public List<OscMessage> Messages;
}
