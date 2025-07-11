using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace KernellVRC
{
	// Token: 0x02000071 RID: 113
	internal class InputBox
	{
		// Token: 0x060004C9 RID: 1225 RVA: 0x0001BB9C File Offset: 0x00019D9C
		[DebuggerStepThrough]
		public static Task<string> Create(string Title, string Placeholder, string ButonText, bool Numberkeyboard = false, string FilledText = "")
		{
			InputBox.<Create>d__0 <Create>d__ = new InputBox.<Create>d__0();
			<Create>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<Create>d__.Title = Title;
			<Create>d__.Placeholder = Placeholder;
			<Create>d__.ButonText = ButonText;
			<Create>d__.Numberkeyboard = Numberkeyboard;
			<Create>d__.FilledText = FilledText;
			<Create>d__.<>1__state = -1;
			<Create>d__.<>t__builder.Start<InputBox.<Create>d__0>(ref <Create>d__);
			return <Create>d__.<>t__builder.Task;
		}
	}
}
