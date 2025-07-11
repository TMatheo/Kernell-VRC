using System;
using System.Collections.Generic;

// Token: 0x02000013 RID: 19
internal class OSCConfig
{
	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000085 RID: 133 RVA: 0x000048E8 File Offset: 0x00002AE8
	// (set) Token: 0x06000086 RID: 134 RVA: 0x000048F0 File Offset: 0x00002AF0
	public bool OSC { get; set; } = false;

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000087 RID: 135 RVA: 0x000048F9 File Offset: 0x00002AF9
	// (set) Token: 0x06000088 RID: 136 RVA: 0x00004901 File Offset: 0x00002B01
	public bool OSCMusic { get; set; } = false;

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000089 RID: 137 RVA: 0x0000490A File Offset: 0x00002B0A
	// (set) Token: 0x0600008A RID: 138 RVA: 0x00004912 File Offset: 0x00002B12
	public bool OSCTime { get; set; } = false;

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x0600008B RID: 139 RVA: 0x0000491B File Offset: 0x00002B1B
	// (set) Token: 0x0600008C RID: 140 RVA: 0x00004923 File Offset: 0x00002B23
	public bool OSCMessage { get; set; } = false;

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x0600008D RID: 141 RVA: 0x0000492C File Offset: 0x00002B2C
	// (set) Token: 0x0600008E RID: 142 RVA: 0x00004934 File Offset: 0x00002B34
	public bool OSCSysInfo { get; set; } = false;

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x0600008F RID: 143 RVA: 0x0000493D File Offset: 0x00002B3D
	// (set) Token: 0x06000090 RID: 144 RVA: 0x00004945 File Offset: 0x00002B45
	public bool OSCAppInfo { get; set; } = false;

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000091 RID: 145 RVA: 0x0000494E File Offset: 0x00002B4E
	// (set) Token: 0x06000092 RID: 146 RVA: 0x00004956 File Offset: 0x00002B56
	public bool OSCPerformance { get; set; } = false;

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000093 RID: 147 RVA: 0x0000495F File Offset: 0x00002B5F
	// (set) Token: 0x06000094 RID: 148 RVA: 0x00004967 File Offset: 0x00002B67
	public bool OSCPlayerGreeting { get; set; } = false;

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000095 RID: 149 RVA: 0x00004970 File Offset: 0x00002B70
	// (set) Token: 0x06000096 RID: 150 RVA: 0x00004978 File Offset: 0x00002B78
	public bool OSCBattery { get; set; } = false;

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000097 RID: 151 RVA: 0x00004981 File Offset: 0x00002B81
	// (set) Token: 0x06000098 RID: 152 RVA: 0x00004989 File Offset: 0x00002B89
	public List<string> OSCMessageList { get; set; } = new List<string>();
}
