using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;

namespace KernellVRCLite
{
	// Token: 0x0200007D RID: 125
	public class JoinWorldById : KernelModuleBase
	{
		// Token: 0x17000119 RID: 281
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x0002239A File Offset: 0x0002059A
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.WorldEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x000223A4 File Offset: 0x000205A4
		public override void OnUiManagerInit()
		{
			ReMenuButton reMenuButton = MenuSetup._uiManager.QMMenu.GetCategoryPage("Utility").AddCategory("Force Join", true, "#ffffff", false).AddButton("Join World By ID", "Join a world by an inputted ID", delegate
			{
				this.CreateWorldIdInputBox();
			}, null, "#ffffff");
		}

		// Token: 0x0600059C RID: 1436 RVA: 0x000223FC File Offset: 0x000205FC
		[DebuggerStepThrough]
		private void CreateWorldIdInputBox()
		{
			JoinWorldById.<CreateWorldIdInputBox>d__3 <CreateWorldIdInputBox>d__ = new JoinWorldById.<CreateWorldIdInputBox>d__3();
			<CreateWorldIdInputBox>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<CreateWorldIdInputBox>d__.<>4__this = this;
			<CreateWorldIdInputBox>d__.<>1__state = -1;
			<CreateWorldIdInputBox>d__.<>t__builder.Start<JoinWorldById.<CreateWorldIdInputBox>d__3>(ref <CreateWorldIdInputBox>d__);
		}
	}
}
