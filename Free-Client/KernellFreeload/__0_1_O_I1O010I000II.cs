using System;

// Token: 0x02000011 RID: 17
public static class __0_1_O_I1O010I000II
{
	// Token: 0x0600003A RID: 58 RVA: 0x00003374 File Offset: 0x00001574
	public static string __1l0O0OlOl_l_Il0O_l(string ____l0__Ol, int _I_I___lI0)
	{
		char[] array = ____l0__Ol.ToCharArray();
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			array[i] = (char)((int)array[i] ^ _I_I___lI0);
		}
		return new string(array);
	}

	// Token: 0x04000054 RID: 84
	private object _ll00_1101I1lO0OI1_l;

	// Token: 0x04000055 RID: 85
	private string _O0lI0_1I_OlO_11lll_;

	// Token: 0x04000056 RID: 86
	private static string _l11I_1_OII_I_1_I_11;

	// Token: 0x04000057 RID: 87
	private static int _I0l1l00IOl0O_0OIlII;

	// Token: 0x04000058 RID: 88
	private string ___0I_0_1__0IOO0_1l1;
}
