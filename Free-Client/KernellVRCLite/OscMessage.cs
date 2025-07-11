using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KernellVRCLite;

// Token: 0x02000018 RID: 24
public class OscMessage : OscPacket
{
	// Token: 0x060000E1 RID: 225 RVA: 0x00005C64 File Offset: 0x00003E64
	public OscMessage(string address, params object[] args)
	{
		this.Address = address;
		this.Arguments = new List<object>();
		this.Arguments.AddRange(args);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00005C90 File Offset: 0x00003E90
	public override byte[] GetBytes()
	{
		List<byte[]> list = new List<byte[]>();
		List<object> list2 = this.Arguments;
		int num = 0;
		string text = ",";
		int i = 0;
		while (i < list2.Count)
		{
			object obj = list2[i];
			string text2 = (obj != null) ? obj.GetType().ToString() : "null";
			string text3 = text2;
			string text4 = text3;
			uint num2 = <PrivateImplementationDetails>.ComputeStringHash(text4);
			if (num2 <= 1996966820U)
			{
				if (num2 <= 875577056U)
				{
					if (num2 <= 848225627U)
					{
						if (num2 != 347085918U)
						{
							if (num2 != 848225627U)
							{
								goto IL_57B;
							}
							if (!(text4 == "System.Double"))
							{
								goto IL_57B;
							}
							bool flag = double.IsPositiveInfinity((double)obj);
							if (flag)
							{
								text += "I";
								goto IL_58D;
							}
							text += "d";
							list.Add(OscPacket.SetDouble((double)obj));
							goto IL_58D;
						}
						else
						{
							if (!(text4 == "System.Boolean"))
							{
								goto IL_57B;
							}
							text += (((bool)obj) ? "T" : "F");
							goto IL_58D;
						}
					}
					else if (num2 != 865714359U)
					{
						if (num2 != 875577056U)
						{
							goto IL_57B;
						}
						if (!(text4 == "System.UInt64"))
						{
							goto IL_57B;
						}
						text += "t";
						list.Add(OscPacket.SetULong((ulong)obj));
						goto IL_58D;
					}
					else
					{
						if (!(text4 == "SharpOSC.Timetag"))
						{
							goto IL_57B;
						}
						text += "t";
						list.Add(OscPacket.SetULong(((Timetag)obj).Tag));
						goto IL_58D;
					}
				}
				else if (num2 <= 1461188995U)
				{
					if (num2 != 1192274782U)
					{
						if (num2 != 1461188995U)
						{
							goto IL_57B;
						}
						if (!(text4 == "System.Collections.Generic.List`1[System.Object]"))
						{
							goto IL_57B;
						}
					}
					else
					{
						if (!(text4 == "SharpOSC.RGBA"))
						{
							goto IL_57B;
						}
						text += "r";
						list.Add(OscPacket.SetRGBA((RGBA)obj));
						goto IL_58D;
					}
				}
				else if (num2 != 1764058053U)
				{
					if (num2 != 1996966820U)
					{
						goto IL_57B;
					}
					if (!(text4 == "null"))
					{
						goto IL_57B;
					}
					text += "N";
					goto IL_58D;
				}
				else
				{
					if (!(text4 == "System.Int64"))
					{
						goto IL_57B;
					}
					text += "h";
					list.Add(OscPacket.SetLong((long)obj));
					goto IL_58D;
				}
			}
			else if (num2 <= 3809001313U)
			{
				if (num2 <= 2249825754U)
				{
					if (num2 != 2185383742U)
					{
						if (num2 != 2249825754U)
						{
							goto IL_57B;
						}
						if (!(text4 == "System.Char"))
						{
							goto IL_57B;
						}
						text += "c";
						list.Add(OscPacket.SetChar((char)obj));
						goto IL_58D;
					}
					else
					{
						if (!(text4 == "System.Single"))
						{
							goto IL_57B;
						}
						bool flag2 = float.IsPositiveInfinity((float)obj);
						if (flag2)
						{
							text += "I";
							goto IL_58D;
						}
						text += "f";
						list.Add(OscPacket.SetFloat((float)obj));
						goto IL_58D;
					}
				}
				else if (num2 != 3498330581U)
				{
					if (num2 != 3809001313U)
					{
						goto IL_57B;
					}
					if (!(text4 == "SharpOSC.Midi"))
					{
						goto IL_57B;
					}
					text += "m";
					list.Add(OscPacket.SetMidi((Midi)obj));
					goto IL_58D;
				}
				else if (!(text4 == "System.Object[]"))
				{
					goto IL_57B;
				}
			}
			else if (num2 <= 4197559036U)
			{
				if (num2 != 4180476474U)
				{
					if (num2 != 4197559036U)
					{
						goto IL_57B;
					}
					if (!(text4 == "SharpOSC.Symbol"))
					{
						goto IL_57B;
					}
					text += "S";
					list.Add(OscPacket.SetString(((Symbol)obj).Value));
					goto IL_58D;
				}
				else
				{
					if (!(text4 == "System.Int32"))
					{
						goto IL_57B;
					}
					text += "i";
					list.Add(OscPacket.SetInt((int)obj));
					goto IL_58D;
				}
			}
			else if (num2 != 4201364391U)
			{
				if (num2 != 4256690024U)
				{
					goto IL_57B;
				}
				if (!(text4 == "System.Byte[]"))
				{
					goto IL_57B;
				}
				text += "b";
				list.Add(OscPacket.SetBlob((byte[])obj));
				goto IL_58D;
			}
			else
			{
				if (!(text4 == "System.String"))
				{
					goto IL_57B;
				}
				text += "s";
				list.Add(OscPacket.SetString((string)obj));
				goto IL_58D;
			}
			bool flag3 = obj.GetType() == typeof(object[]);
			if (flag3)
			{
				obj = Enumerable.ToList<object>((object[])obj);
			}
			bool flag4 = this.Arguments != list2;
			if (flag4)
			{
				throw new Exception("Nested Arrays are not supported");
			}
			text += "[";
			list2 = (List<object>)obj;
			num = i;
			i = 0;
			continue;
			IL_58D:
			i++;
			bool flag5 = list2 != this.Arguments && i == list2.Count;
			if (flag5)
			{
				text += "]";
				list2 = this.Arguments;
				i = num + 1;
			}
			continue;
			IL_57B:
			throw new Exception("Unable to transmit values of type " + text2);
		}
		int num3 = (this.Address.Length != 0 && this.Address != null) ? Utils.AlignedStringLength(this.Address) : 0;
		int num4 = Utils.AlignedStringLength(text);
		int num5 = num3 + num4 + Enumerable.Sum<byte[]>(list, (byte[] x) => x.Length);
		byte[] array = new byte[num5];
		i = 0;
		Encoding.UTF8.GetBytes(this.Address).CopyTo(array, i);
		i += num3;
		Encoding.UTF8.GetBytes(text).CopyTo(array, i);
		i += num4;
		foreach (byte[] array2 in list)
		{
			array2.CopyTo(array, i);
			i += array2.Length;
		}
		return array;
	}

	// Token: 0x0400004C RID: 76
	public string Address;

	// Token: 0x0400004D RID: 77
	public List<object> Arguments;
}
