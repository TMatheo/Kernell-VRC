using System;

// Token: 0x02000014 RID: 20
public class TagsConfig
{
	// Token: 0x17000026 RID: 38
	// (get) Token: 0x0600009A RID: 154 RVA: 0x000049F2 File Offset: 0x00002BF2
	// (set) Token: 0x0600009B RID: 155 RVA: 0x000049FA File Offset: 0x00002BFA
	public bool Tags { get; set; } = true;

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x0600009C RID: 156 RVA: 0x00004A03 File Offset: 0x00002C03
	// (set) Token: 0x0600009D RID: 157 RVA: 0x00004A0B File Offset: 0x00002C0B
	public bool TagsDates { get; set; } = true;

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x0600009E RID: 158 RVA: 0x00004A14 File Offset: 0x00002C14
	// (set) Token: 0x0600009F RID: 159 RVA: 0x00004A1C File Offset: 0x00002C1C
	public bool NameplateColours { get; set; } = true;

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060000A0 RID: 160 RVA: 0x00004A25 File Offset: 0x00002C25
	// (set) Token: 0x060000A1 RID: 161 RVA: 0x00004A2D File Offset: 0x00002C2D
	public bool BackgroundSpriteApply { get; set; } = false;

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060000A2 RID: 162 RVA: 0x00004A36 File Offset: 0x00002C36
	// (set) Token: 0x060000A3 RID: 163 RVA: 0x00004A3E File Offset: 0x00002C3E
	public bool UseKernelSprite { get; set; } = true;

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060000A4 RID: 164 RVA: 0x00004A47 File Offset: 0x00002C47
	// (set) Token: 0x060000A5 RID: 165 RVA: 0x00004A4F File Offset: 0x00002C4F
	public bool Use2018Sprite { get; set; } = false;

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060000A6 RID: 166 RVA: 0x00004A58 File Offset: 0x00002C58
	// (set) Token: 0x060000A7 RID: 167 RVA: 0x00004A60 File Offset: 0x00002C60
	public bool EnableFrameColor { get; set; } = false;

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060000A8 RID: 168 RVA: 0x00004A69 File Offset: 0x00002C69
	// (set) Token: 0x060000A9 RID: 169 RVA: 0x00004A71 File Offset: 0x00002C71
	public bool EnablePingColor { get; set; } = false;
}
