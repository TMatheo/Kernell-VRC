using System;
using System.Collections.Generic;
using System.Text;
using KernellVRCLite;

// Token: 0x02000019 RID: 25
public abstract class OscPacket
{
	// Token: 0x060000E3 RID: 227 RVA: 0x0000637C File Offset: 0x0000457C
	public static OscPacket GetPacket(byte[] oscData)
	{
		bool flag = oscData == null || oscData.Length == 0;
		if (flag)
		{
			throw new ArgumentException("OSC data is null or empty");
		}
		bool flag2 = oscData[0] == 35;
		OscPacket result;
		if (flag2)
		{
			result = OscPacket.ParseBundle(oscData);
		}
		else
		{
			result = OscPacket.ParseMessage(oscData);
		}
		return result;
	}

	// Token: 0x060000E4 RID: 228
	public abstract byte[] GetBytes();

	// Token: 0x060000E5 RID: 229 RVA: 0x000063C4 File Offset: 0x000045C4
	private static OscMessage ParseMessage(byte[] msg)
	{
		int num = 0;
		int num2;
		string address = OscPacket.ReadPaddedString(msg, num, out num2);
		num += num2;
		int num3;
		string text = OscPacket.ReadPaddedString(msg, num, out num3);
		num += num3;
		bool flag = string.IsNullOrEmpty(text) || text[0] != ',';
		if (flag)
		{
			throw new Exception("OSC type tag string must start with a comma.");
		}
		text = text.Substring(1);
		List<object> list = new List<object>();
		string text2 = text;
		int i = 0;
		while (i < text2.Length)
		{
			char c = text2[i];
			char c2 = c;
			char c3 = c2;
			if (c3 <= 'S')
			{
				if (c3 <= 'I')
				{
					if (c3 != 'F')
					{
						if (c3 != 'I')
						{
							goto IL_307;
						}
						list.Add(double.PositiveInfinity);
					}
					else
					{
						list.Add(false);
					}
				}
				else if (c3 != 'N')
				{
					if (c3 != 'S')
					{
						goto IL_307;
					}
					int num4;
					string value = OscPacket.ReadPaddedString(msg, num, out num4);
					list.Add(new Symbol(value));
					num += num4;
				}
				else
				{
					list.Add(null);
				}
			}
			else if (c3 <= 'i')
			{
				if (c3 != 'T')
				{
					switch (c3)
					{
					case '[':
					case ']':
						throw new Exception("Nested arrays are not supported.");
					case '\\':
					case '^':
					case '_':
					case '`':
					case 'a':
					case 'e':
					case 'g':
						goto IL_307;
					case 'b':
					{
						int @int = OscPacket.GetInt(msg, num);
						num += 4;
						byte[] item = OscPacket.SubArray(msg, num, @int);
						list.Add(item);
						num += @int;
						num = OscPacket.AlignIndex(num);
						break;
					}
					case 'c':
					{
						char @char = OscPacket.GetChar(msg, num);
						list.Add(@char);
						num += 4;
						break;
					}
					case 'd':
					{
						double @double = OscPacket.GetDouble(msg, num);
						list.Add(@double);
						num += 8;
						break;
					}
					case 'f':
					{
						float @float = OscPacket.GetFloat(msg, num);
						list.Add(@float);
						num += 4;
						break;
					}
					case 'h':
					{
						long @long = OscPacket.GetLong(msg, num);
						list.Add(@long);
						num += 8;
						break;
					}
					case 'i':
					{
						int int2 = OscPacket.GetInt(msg, num);
						list.Add(int2);
						num += 4;
						break;
					}
					default:
						goto IL_307;
					}
				}
				else
				{
					list.Add(true);
				}
			}
			else if (c3 != 'm')
			{
				switch (c3)
				{
				case 'r':
				{
					RGBA rgba = OscPacket.GetRGBA(msg, num);
					list.Add(rgba);
					num += 4;
					break;
				}
				case 's':
				{
					int num5;
					string item2 = OscPacket.ReadPaddedString(msg, num, out num5);
					list.Add(item2);
					num += num5;
					break;
				}
				case 't':
				{
					ulong @ulong = OscPacket.GetULong(msg, num);
					list.Add(new Timetag(@ulong));
					num += 8;
					break;
				}
				default:
					goto IL_307;
				}
			}
			else
			{
				Midi midi = OscPacket.GetMidi(msg, num);
				list.Add(midi);
				num += 4;
			}
			num = OscPacket.AlignIndex(num);
			i++;
			continue;
			IL_307:
			throw new Exception(string.Format("Unknown OSC type tag '{0}'.", c));
		}
		return new OscMessage(address, list.ToArray());
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00006720 File Offset: 0x00004920
	private static OscBundle ParseBundle(byte[] bundle)
	{
		int i = 0;
		string @string = Encoding.UTF8.GetString(OscPacket.SubArray(bundle, i, 8));
		i += 8;
		bool flag = @string != "#bundle\0";
		if (flag)
		{
			throw new Exception("Not a valid OSC bundle.");
		}
		ulong @ulong = OscPacket.GetULong(bundle, i);
		i += 8;
		List<OscMessage> list = new List<OscMessage>();
		while (i < bundle.Length)
		{
			int @int = OscPacket.GetInt(bundle, i);
			i += 4;
			byte[] oscData = OscPacket.SubArray(bundle, i, @int);
			i += @int;
			i = OscPacket.AlignIndex(i);
			OscPacket packet = OscPacket.GetPacket(oscData);
			OscMessage oscMessage = packet as OscMessage;
			bool flag2 = oscMessage != null;
			if (!flag2)
			{
				throw new Exception("Nested bundles are not supported in this implementation.");
			}
			list.Add(oscMessage);
		}
		return new OscBundle(@ulong, list.ToArray());
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x000067F4 File Offset: 0x000049F4
	private static string ReadPaddedString(byte[] data, int start, out int paddedLength)
	{
		int num = start;
		while (num < data.Length && data[num] > 0)
		{
			num++;
		}
		bool flag = num >= data.Length;
		if (flag)
		{
			throw new Exception("String not null-terminated");
		}
		string @string = Encoding.UTF8.GetString(data, start, num - start);
		int num2 = num - start + 1;
		paddedLength = (num2 + 3) / 4 * 4;
		return @string;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00006860 File Offset: 0x00004A60
	private static int AlignIndex(int index)
	{
		return (index % 4 == 0) ? index : ((index + 3) / 4 * 4);
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00006884 File Offset: 0x00004A84
	private static byte[] SubArray(byte[] data, int index, int count)
	{
		byte[] array = new byte[count];
		Array.Copy(data, index, array, 0, count);
		return array;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x000068AC File Offset: 0x00004AAC
	protected static int GetInt(byte[] data, int index)
	{
		return (int)data[index] << 24 | (int)data[index + 1] << 16 | (int)data[index + 2] << 8 | (int)data[index + 3];
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000068DC File Offset: 0x00004ADC
	protected static float GetFloat(byte[] data, int index)
	{
		byte[] value = new byte[]
		{
			data[index + 3],
			data[index + 2],
			data[index + 1],
			data[index]
		};
		return BitConverter.ToSingle(value, 0);
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000691C File Offset: 0x00004B1C
	protected static long GetLong(byte[] data, int index)
	{
		byte[] array = new byte[8];
		for (int i = 0; i < 8; i++)
		{
			array[7 - i] = data[index + i];
		}
		return BitConverter.ToInt64(array, 0);
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00006958 File Offset: 0x00004B58
	protected static ulong GetULong(byte[] data, int index)
	{
		ulong num = 0UL;
		for (int i = 0; i < 8; i++)
		{
			num = (num << 8 | (ulong)data[index + i]);
		}
		return num;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x0000698C File Offset: 0x00004B8C
	protected static double GetDouble(byte[] data, int index)
	{
		byte[] array = new byte[8];
		for (int i = 0; i < 8; i++)
		{
			array[7 - i] = data[index + i];
		}
		return BitConverter.ToDouble(array, 0);
	}

	// Token: 0x060000EF RID: 239 RVA: 0x000069C8 File Offset: 0x00004BC8
	protected static char GetChar(byte[] data, int index)
	{
		return (char)data[index + 3];
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000069E0 File Offset: 0x00004BE0
	protected static RGBA GetRGBA(byte[] data, int index)
	{
		return new RGBA(data[index], data[index + 1], data[index + 2], data[index + 3]);
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00006A0C File Offset: 0x00004C0C
	protected static Midi GetMidi(byte[] data, int index)
	{
		return new Midi(data[index], data[index + 1], data[index + 2], data[index + 3]);
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x00006A38 File Offset: 0x00004C38
	protected static byte[] SetInt(int value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		bool isLittleEndian = BitConverter.IsLittleEndian;
		if (isLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00006A64 File Offset: 0x00004C64
	protected static byte[] SetFloat(float value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		bool isLittleEndian = BitConverter.IsLittleEndian;
		if (isLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x00006A90 File Offset: 0x00004C90
	protected static byte[] SetString(string value)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(value);
		int num = bytes.Length + 1;
		int num2 = (num + 3) / 4 * 4;
		byte[] array = new byte[num2];
		Array.Copy(bytes, array, bytes.Length);
		return array;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00006AD4 File Offset: 0x00004CD4
	protected static byte[] SetBlob(byte[] value)
	{
		int num = value.Length;
		int num2 = (num + 3) / 4 * 4;
		byte[] array = new byte[4 + num2];
		Array.Copy(OscPacket.SetInt(num), array, 4);
		Array.Copy(value, 0, array, 4, num);
		return array;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x00006B18 File Offset: 0x00004D18
	protected static byte[] SetLong(long value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		bool isLittleEndian = BitConverter.IsLittleEndian;
		if (isLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00006B44 File Offset: 0x00004D44
	protected static byte[] SetULong(ulong value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		bool isLittleEndian = BitConverter.IsLittleEndian;
		if (isLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00006B70 File Offset: 0x00004D70
	protected static byte[] SetDouble(double value)
	{
		byte[] bytes = BitConverter.GetBytes(value);
		bool isLittleEndian = BitConverter.IsLittleEndian;
		if (isLittleEndian)
		{
			Array.Reverse(bytes);
		}
		return bytes;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00006B9C File Offset: 0x00004D9C
	public static byte[] SetChar(char value)
	{
		return new byte[]
		{
			0,
			0,
			0,
			(byte)value
		};
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00006BBC File Offset: 0x00004DBC
	public static byte[] SetRGBA(RGBA value)
	{
		return new byte[]
		{
			value.R,
			value.G,
			value.B,
			value.A
		};
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00006BF8 File Offset: 0x00004DF8
	public static byte[] SetMidi(Midi value)
	{
		return new byte[]
		{
			value.Port,
			value.Status,
			value.Data1,
			value.Data2
		};
	}
}
