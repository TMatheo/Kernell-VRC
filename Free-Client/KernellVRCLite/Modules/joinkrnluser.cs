using System;
using KernellClientUI.UI.MainMenu;
using KernellClientUI.VRChat;
using KernelVRC;
using UnityEngine;

namespace KernellVRCLite.modules
{
	// Token: 0x0200009E RID: 158
	public class joinkrnluser : KernelModuleBase
	{
		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000808 RID: 2056 RVA: 0x000311D4 File Offset: 0x0002F3D4
		public override string ModuleName
		{
			get
			{
				return "Join User";
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000809 RID: 2057 RVA: 0x0002EF62 File Offset: 0x0002D162
		public override string Version
		{
			get
			{
				return "1.0.0";
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600080A RID: 2058 RVA: 0x0002FD37 File Offset: 0x0002DF37
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600080B RID: 2059 RVA: 0x00003312 File Offset: 0x00001512
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every2Frames;
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x000311DC File Offset: 0x0002F3DC
		public override void OnUiManagerInit()
		{
			try
			{
				Action onClick = delegate()
				{
					try
					{
						MainMenuSelectedUser mainMenuSelectedUser = OtherUtil.GetMainMenuSelectedUser();
						bool flag2 = mainMenuSelectedUser != null && mainMenuSelectedUser.field_Private_IUser_0 != null;
						if (flag2)
						{
							this._target = PlayerExtensions.GetVRCPlayer(mainMenuSelectedUser.field_Private_IUser_0);
							kernelllogger.Msg("[KernellVRCLite] Join button clicked - no action implemented yet");
							bool flag3 = MenuEx.MMInstance != null;
							if (flag3)
							{
								MenuEx.MMInstance.Method_Private_Void_0();
							}
						}
						else
						{
							kernelllogger.Warning("[KernellVRCLite] Join: No user selected in Main Menu");
						}
					}
					catch (Exception ex2)
					{
						kernelllogger.Error("[KernellVRCLite] Join MainMenu action error: " + ex2.Message);
					}
				};
				bool flag = MMenuPrefabs.MMUserDetailButton != null && MMenuPrefabs.MMUserDetailButton.transform != null && MMenuPrefabs.MMUserDetailButton.transform.parent != null;
				if (flag)
				{
					Transform parent = MMenuPrefabs.MMUserDetailButton.transform.parent;
					new ReMMUserButton("Join this user", "If the user is on the Kernell Network then it will attempt to join (THE USER MUST BE IN A PUBLIC INSTANCE)", onClick, null, parent);
					kernelllogger.Msg("[KernellVRCLite] Added Join button to Main Menu");
				}
				else
				{
					kernelllogger.Warning("[KernellVRCLite] Could not add Main Menu join button - parent reference not found");
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellVRCLite] Join OnUiManagerInit error: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
			}
		}

		// Token: 0x040003DA RID: 986
		private VRCPlayer _target;
	}
}
