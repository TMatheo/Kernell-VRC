using System;
using KernellClientUI.UI.MainMenu;
using KernellClientUI.UI.QuickMenu;
using KernellClientUI.VRChat;
using KernellVRC;
using KernelVRC;
using UnityEngine;

namespace KernellVRCLite.Modules.Target
{
	// Token: 0x02000099 RID: 153
	internal class Teleport : KernelModuleBase
	{
		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0002FD30 File Offset: 0x0002DF30
		public override string ModuleName
		{
			get
			{
				return "Teleport to player";
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0002FD37 File Offset: 0x0002DF37
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x0002FD40 File Offset: 0x0002DF40
		public override void OnUiManagerInit()
		{
			try
			{
				IButtonPage targetMenu = MenuSetup._uiManager.TargetMenu;
				Action onClick = delegate()
				{
					try
					{
						bool flag2 = MenuEx.QMSelectedUserLocal != null && MenuEx.QMSelectedUserLocal.field_Private_IUser_0 != null;
						if (flag2)
						{
							this._target = PlayerExtensions.GetVRCPlayer(MenuEx.QMSelectedUserLocal.field_Private_IUser_0);
							this.TeleportToIUser(this._target);
						}
						else
						{
							kernelllogger.Warning("[KernellVRCLite] Teleport: No user selected in Quick Menu");
						}
					}
					catch (Exception ex2)
					{
						kernelllogger.Error("[KernellVRCLite] Teleport QuickMenu action error: " + ex2.Message);
					}
				};
				targetMenu.AddButton("Teleport", "", onClick, null, "#ffffff");
				kernelllogger.Msg("[KernellVRCLite] Added Teleport button to Quick Menu");
				Action onClick2 = delegate()
				{
					try
					{
						MainMenuSelectedUser mainMenuSelectedUser = OtherUtil.GetMainMenuSelectedUser();
						bool flag2 = mainMenuSelectedUser != null && mainMenuSelectedUser.field_Private_IUser_0 != null;
						if (flag2)
						{
							this._target = PlayerExtensions.GetVRCPlayer(mainMenuSelectedUser.field_Private_IUser_0);
							this.TeleportToIUser(this._target);
							bool flag3 = MenuEx.MMInstance != null;
							if (flag3)
							{
								MenuEx.MMInstance.Method_Private_Void_0();
							}
						}
						else
						{
							kernelllogger.Warning("[KernellVRCLite] Teleport: No user selected in Main Menu");
						}
					}
					catch (Exception ex2)
					{
						kernelllogger.Error("[KernellVRCLite] Teleport MainMenu action error: " + ex2.Message);
					}
				};
				bool flag = MMenuPrefabs.MMUserDetailButton != null && MMenuPrefabs.MMUserDetailButton.transform != null && MMenuPrefabs.MMUserDetailButton.transform.parent != null;
				if (flag)
				{
					Transform parent = MMenuPrefabs.MMUserDetailButton.transform.parent;
					new ReMMUserButton("Teleport to them", "Teleport to them", onClick2, null, parent);
					kernelllogger.Msg("[KernellVRCLite] Added Teleport button to Main Menu");
				}
				else
				{
					kernelllogger.Warning("[KernellVRCLite] Could not add Main Menu teleport button - parent reference not found");
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellVRCLite] Teleport OnUiManagerInit error: " + ex.Message + "\nStackTrace: " + ex.StackTrace);
			}
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x0002FE50 File Offset: 0x0002E050
		private void TeleportToIUser(VRCPlayer user)
		{
			try
			{
				bool flag = user == null;
				if (flag)
				{
					kernelllogger.Warning("[KernellVRCLite] Teleport: Target VRCPlayer is null");
				}
				else
				{
					bool flag2 = VRCPlayer.field_Internal_Static_VRCPlayer_0 == null;
					if (flag2)
					{
						kernelllogger.Warning("[KernellVRCLite] Teleport: Local VRCPlayer is null");
					}
					else
					{
						bool flag3 = user.gameObject == null || user.gameObject.transform == null;
						if (flag3)
						{
							kernelllogger.Warning("[KernellVRCLite] Teleport: Target transform is null");
						}
						else
						{
							Vector3 position = user.gameObject.transform.position;
							VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position = position;
							kernelllogger.Msg(string.Format("[KernellVRCLite] Teleported to player at position {0}", position));
						}
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[KernellVRCLite] TeleportToIUser error: " + ex.Message);
			}
		}

		// Token: 0x040003C2 RID: 962
		private VRCPlayer _target;
	}
}
