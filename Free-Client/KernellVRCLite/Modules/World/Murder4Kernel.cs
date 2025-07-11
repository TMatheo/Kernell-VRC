using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Il2CppSystem;
using KernellClientUI.UI.QuickMenu;
using KernellClientUI.VRChat;
using KernellVRC;
using KernellVRCLite.Utils;
using KernelVRC;
using MelonLoader;
using TMPro;
using UnityEngine;
using VRC;
using VRC.SDKBase;
using VRC.Udon;

namespace KernellVRCLite.Modules.World
{
	// Token: 0x02000096 RID: 150
	internal class Murder4Kernel : KernelModuleBase
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0002DE34 File Offset: 0x0002C034
		public override string ModuleName
		{
			get
			{
				return "Murder4";
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x00005413 File Offset: 0x00003613
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.WorldEvents | ModuleCapabilities.UdonEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x0600075C RID: 1884 RVA: 0x0002DE3C File Offset: 0x0002C03C
		public override void OnUiManagerInit()
		{
			ReCategoryPage reCategoryPage = MenuSetup._uiManager.QMMenu.GetCategoryPage("Exploits").GetCategory("Game Worlds").AddCategoryPage("Murder 4", "", null, "#ffffff");
			ReMenuCategory reMenuCategory = reCategoryPage.AddCategory("Game Logic", true, "#ffffff", false);
			reMenuCategory.AddButton("Start Game", "", delegate
			{
				this.SendGameLogicEvent("SyncCountdown");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Abort Game", "", delegate
			{
				this.SendGameLogicEvent("SyncAbort");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Murder Win", "", delegate
			{
				this.SendGameLogicEvent("SyncVictoryM");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Bystander Win", "", delegate
			{
				this.SendGameLogicEvent("SyncVictoryB");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Lights On", "", delegate
			{
				this.ToggleLights(true);
			}, null, "#ffffff");
			reMenuCategory.AddButton("Lights Off", "", delegate
			{
				this.ToggleLights(false);
			}, null, "#ffffff");
			reMenuCategory.AddButton("Flash All", "", delegate
			{
				this.SendGameLogicEvent("OnLocalPlayerFlashbanged");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Explode Gun", "", delegate
			{
				this.ExplodeGun();
			}, null, "#ffffff");
			reMenuCategory.AddButton("Smoke Gun", "", delegate
			{
				this.SmokeGun();
			}, null, "#ffffff");
			this.autoLockBtn = reMenuCategory.AddButton("Auto Lock Doors", "", new Action(this.ToggleAutoLock), null, "#ffffff");
			this.loopAllBtn = reMenuCategory.AddButton("Loop All Players", "", new Action(this.ToggleLoopAll), null, "#ffffff");
			this.loopDelayBtn = reMenuCategory.AddButton(string.Format("Loop Delay: {0}s", this.loopDelay), "", new Action(this.CycleLoopDelay), null, "#ffffff");
			ReMenuCategory reMenuCategory2 = reCategoryPage.AddCategory("Door Controls", true, "#ffffff", false);
			reMenuCategory2.AddButton("Open All Doors", "", delegate
			{
				this.DoorEvent("Interact open");
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Close All Doors", "", delegate
			{
				this.DoorEvent("Interact close");
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Lock All Doors", "", delegate
			{
				this.DoorEvent("Interact lock");
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Unlock All Doors", "", delegate
			{
				for (int i = 0; i < 5; i++)
				{
					this.DoorEvent("Interact shove");
				}
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Doors ON (Local)", "", delegate
			{
				GameObject gameObject = GameObject.Find("Environment/Doors");
				bool flag = gameObject != null;
				if (flag)
				{
					gameObject.transform.position = new Vector3(0f, 0f, 0f);
				}
			}, null, "#ffffff");
			reMenuCategory2.AddButton("Doors OFF (Local)", "", delegate
			{
				GameObject gameObject = GameObject.Find("Environment/Doors");
				bool flag = gameObject != null;
				if (flag)
				{
					gameObject.transform.position = new Vector3(0f, 200f, 0f);
				}
			}, null, "#ffffff");
			ReMenuCategory reMenuCategory3 = reCategoryPage.AddCategory("Bear Traps", true, "#ffffff", false);
			reMenuCategory3.AddButton("Trap Detective", "", delegate
			{
				this.PlaceBearTrap("Bear Trap (0)", new Vector3(4.931f, 3.107f, 127.691f));
			}, null, "#ffffff");
			reMenuCategory3.AddButton("Trap Stair 1", "", delegate
			{
				this.PlaceBearTrap("Bear Trap (2)", new Vector3(-2.994f, 2.995f, 121.689f));
			}, null, "#ffffff");
			reMenuCategory3.AddButton("Trap Stair 2", "", delegate
			{
				this.PlaceBearTrap("Bear Trap (1)", new Vector3(5.022f, 0.068f, 135.853f));
			}, null, "#ffffff");
			reMenuCategory3.AddButton("Trap Bedroom", "", delegate
			{
				this.PlaceBearTrap("Bear Trap (0)", new Vector3(-4.489f, 3.047f, 129.285f));
			}, null, "#ffffff");
			ReMenuPage reMenuPage = MenuSetup._uiManager.TargetMenu.AddMenuPage("Murder Target", "", null, "#ffffff");
			reMenuPage.AddButton("Set Murderer", "", delegate
			{
				this.SetRole("M");
			}, null, "#ffffff");
			reMenuPage.AddButton("Set Detective", "", delegate
			{
				this.SetRole("D");
			}, null, "#ffffff");
			reMenuPage.AddButton("Set Bystander", "", delegate
			{
				this.SetRole("B");
			}, null, "#ffffff");
			reMenuPage.AddButton("Kill Target", "", delegate
			{
				this.SyncKill();
			}, null, "#ffffff");
			reMenuPage.AddButton("Shoot Target", "", delegate
			{
				this.ShootTarget(this.GetTarget());
			}, null, "#ffffff");
			reMenuPage.AddButton("Explode Target", "", delegate
			{
				this.ExplodeTarget(this.GetTarget());
			}, null, "#ffffff");
			reMenuPage.AddButton("Smoke Target", "", delegate
			{
				this.SmokeTarget(this.GetTarget());
			}, null, "#ffffff");
			this.doorAnnoyBtn = reMenuPage.AddButton("Toggle Door Annoyance", "", new Action(this.ToggleDoorAnnoyance), null, "#ffffff");
			reMenuPage.AddButton("Loop Target", "", delegate
			{
				this.AddTargetToLoop(this.GetTarget());
			}, null, "#ffffff");
			reMenuPage.AddButton("Stop All Loops", "", delegate
			{
				Murder4Kernel.loopTargets.Clear();
				foreach (IEnumerator enumerator2 in this.targetLoopRoutines.Values)
				{
					bool flag = enumerator2 != null;
					if (flag)
					{
						MelonCoroutines.Stop(enumerator2);
					}
				}
				this.targetLoopRoutines.Clear();
				this.loopAll = false;
				this.loopAllBtn.Text = "<color=red>Loop All Players";
			}, null, "#ffffff");
		}

		// Token: 0x0600075D RID: 1885 RVA: 0x0002E38C File Offset: 0x0002C58C
		public override void OnPlayerJoined(Player player)
		{
			bool flag = player == PlayerUtil.LocalPlayer();
			if (flag)
			{
				this.ResetGameObjects();
				bool flag2 = this._mainLoopRoutine != null;
				if (flag2)
				{
					MelonCoroutines.Stop(this._mainLoopRoutine);
				}
				this._mainLoopRoutine = this.MainLoop();
				MelonCoroutines.Start(this._mainLoopRoutine);
			}
		}

		// Token: 0x0600075E RID: 1886 RVA: 0x0002E3E4 File Offset: 0x0002C5E4
		private void ResetGameObjects()
		{
			Murder4Kernel.M4Shotgun = GameObject.Find("Game Logic/Weapons/Unlockables/Shotgun (0");
			Murder4Kernel.M4Revolver = GameObject.Find("Game Logic/Weapons/Revolver");
			Murder4Kernel.doors.Clear();
			Murder4Kernel.doors = Enumerable.ToList<GameObject>(Enumerable.Where<GameObject>(Resources.FindObjectsOfTypeAll<GameObject>(), (GameObject go) => go.name.StartsWith("Door")));
			GameObject gameObject = GameObject.Find("Game Logic/Player HUD/Blind HUD Anim");
			GameObject gameObject2 = GameObject.Find("Game Logic/Player HUD/Flashbang HUD Anim");
			GameObject gameObject3 = GameObject.Find("Game Logic/Player HUD/Death HUD Anim");
			bool flag = gameObject != null;
			if (flag)
			{
				gameObject.SetActive(false);
			}
			bool flag2 = gameObject2 != null;
			if (flag2)
			{
				gameObject2.SetActive(false);
			}
			bool flag3 = gameObject3 != null;
			if (flag3)
			{
				gameObject3.SetActive(false);
			}
			kernelllogger.Msg("Murder 4 objects initialized");
		}

		// Token: 0x0600075F RID: 1887 RVA: 0x0002E4B7 File Offset: 0x0002C6B7
		private IEnumerator MainLoop()
		{
			WaitForSeconds wait = new WaitForSeconds(this.loopDelay);
			WaitForSeconds batchWait = new WaitForSeconds(0.01f);
			for (;;)
			{
				bool flag = this.loopAll;
				if (flag)
				{
					foreach (Player p3 in Enumerable.Where<Player>(PlayerUtil.GetAllPlayers(), (Player x) => x != PlayerUtil.LocalPlayer()))
					{
						bool flag2 = !Murder4Kernel.loopTargets.Contains(p3);
						if (flag2)
						{
							this.AddTargetToLoop(p3);
						}
						p3 = null;
					}
					IEnumerator<Player> enumerator = null;
				}
				Murder4Kernel.loopTargets.RemoveWhere((Player p) => p == null || !Enumerable.Contains<Player>(PlayerUtil.GetAllPlayers(), p));
				foreach (Player key in Enumerable.ToList<Player>(Murder4Kernel.playerUdonCache.Keys))
				{
					bool flag3 = key == null || !Enumerable.Contains<Player>(PlayerUtil.GetAllPlayers(), key);
					if (flag3)
					{
						Murder4Kernel.playerUdonCache.Remove(key);
					}
					key = null;
				}
				List<Player>.Enumerator enumerator2 = default(List<Player>.Enumerator);
				foreach (Player p2 in Murder4Kernel.loopTargets)
				{
					UdonBehaviour behaviour;
					bool flag4 = !Murder4Kernel.playerUdonCache.TryGetValue(p2, out behaviour) || behaviour == null;
					if (flag4)
					{
						behaviour = (Murder4Kernel.playerUdonCache[p2] = this.GetPlayerUdon(p2));
					}
					bool flag5 = behaviour != null;
					if (flag5)
					{
						behaviour.SendCustomNetworkEvent(0, "SyncAssignB");
						yield return new WaitForSeconds(0.05f);
						behaviour.SendCustomNetworkEvent(0, "SyncKill");
						yield return wait;
					}
					behaviour = null;
					p2 = null;
				}
				HashSet<Player>.Enumerator enumerator3 = default(HashSet<Player>.Enumerator);
				bool flag6 = this.autoLockDoors;
				if (flag6)
				{
					this.DoorEvent("Interact lock");
				}
				yield return batchWait;
			}
			yield break;
			yield break;
		}

		// Token: 0x06000760 RID: 1888 RVA: 0x0002E4C6 File Offset: 0x0002C6C6
		private IEnumerator DoorAnnoyance(Player target)
		{
			while (this.doorAnnoyanceRoutines.ContainsKey(target))
			{
				foreach (GameObject door in Murder4Kernel.doors)
				{
					bool flag = Vector3.Distance(door.transform.position, target.transform.position) < 5f;
					if (flag)
					{
						Transform transform = door.transform.Find("Door Anim/Hinge/Interact close");
						UdonBehaviour closeInteract = (transform != null) ? transform.GetComponent<UdonBehaviour>() : null;
						Transform transform2 = door.transform.Find("Door Anim/Hinge/Interact lock");
						UdonBehaviour lockInteract = (transform2 != null) ? transform2.GetComponent<UdonBehaviour>() : null;
						bool flag2 = closeInteract != null;
						if (flag2)
						{
							closeInteract.Interact();
						}
						bool flag3 = lockInteract != null;
						if (flag3)
						{
							lockInteract.Interact();
						}
						closeInteract = null;
						lockInteract = null;
					}
					door = null;
				}
				List<GameObject>.Enumerator enumerator = default(List<GameObject>.Enumerator);
				yield return new WaitForSeconds(0.5f);
			}
			yield break;
		}

		// Token: 0x06000761 RID: 1889 RVA: 0x0002E4DC File Offset: 0x0002C6DC
		private IEnumerator TargetLoop(Player target)
		{
			bool flag = target == null;
			if (flag)
			{
				yield break;
			}
			UdonBehaviour udon = this.GetPlayerUdon(target);
			bool flag2 = udon == null;
			if (flag2)
			{
				yield break;
			}
			Murder4Kernel.playerUdonCache[target] = udon;
			while (Murder4Kernel.loopTargets.Contains(target))
			{
				udon.SendCustomNetworkEvent(0, "SyncAssignB");
				yield return new WaitForSeconds(0.05f);
				udon.SendCustomNetworkEvent(0, "SyncKill");
				yield return new WaitForSeconds(this.loopDelay);
			}
			yield break;
		}

		// Token: 0x06000762 RID: 1890 RVA: 0x0002E4F4 File Offset: 0x0002C6F4
		private void ToggleLights(bool on)
		{
			GameObject gameObject = GameObject.Find("Game Logic/Switch Boxes");
			bool flag = gameObject == null;
			if (!flag)
			{
				foreach (Object @object in gameObject.transform)
				{
					Transform transform = (Transform)@object;
					UdonBehaviour component = transform.GetComponent<UdonBehaviour>();
					bool flag2 = component != null;
					if (flag2)
					{
						component.SendCustomNetworkEvent(0, on ? "SwitchUp" : "SwitchDown");
					}
				}
			}
		}

		// Token: 0x06000763 RID: 1891 RVA: 0x0002E594 File Offset: 0x0002C794
		private void ToggleAutoLock()
		{
			this.autoLockDoors = !this.autoLockDoors;
			this.autoLockBtn.Text = (this.autoLockDoors ? "<color=green>Auto Lock Doors" : "<color=red>Auto Lock Doors");
		}

		// Token: 0x06000764 RID: 1892 RVA: 0x0002E5C8 File Offset: 0x0002C7C8
		private void ToggleLoopAll()
		{
			this.loopAll = !this.loopAll;
			this.loopAllBtn.Text = (this.loopAll ? "<color=green>Loop All Players" : "<color=red>Loop All Players");
			bool flag = !this.loopAll;
			if (flag)
			{
				Murder4Kernel.loopTargets.Clear();
				foreach (IEnumerator enumerator2 in this.targetLoopRoutines.Values)
				{
					bool flag2 = enumerator2 != null;
					if (flag2)
					{
						MelonCoroutines.Stop(enumerator2);
					}
				}
				this.targetLoopRoutines.Clear();
			}
		}

		// Token: 0x06000765 RID: 1893 RVA: 0x0002E684 File Offset: 0x0002C884
		private void ToggleDoorAnnoyance()
		{
			Player target = this.GetTarget();
			bool flag = target == null;
			if (!flag)
			{
				bool flag2 = this.doorAnnoyanceRoutines.ContainsKey(target);
				if (flag2)
				{
					bool flag3 = this.doorAnnoyanceRoutines[target] != null;
					if (flag3)
					{
						MelonCoroutines.Stop(this.doorAnnoyanceRoutines[target]);
					}
					this.doorAnnoyanceRoutines.Remove(target);
					this.doorAnnoyBtn.Text = "Toggle Door Annoyance";
				}
				else
				{
					IEnumerator enumerator = this.DoorAnnoyance(target);
					this.doorAnnoyanceRoutines[target] = enumerator;
					MelonCoroutines.Start(enumerator);
					this.doorAnnoyBtn.Text = "<color=green>Door Annoyance Active";
				}
			}
		}

		// Token: 0x06000766 RID: 1894 RVA: 0x0002E734 File Offset: 0x0002C934
		private void CycleLoopDelay()
		{
			this.loopDelay = ((this.loopDelay >= 0.4f) ? 0.05f : (this.loopDelay + 0.05f));
			this.loopDelayBtn.Text = string.Format("Loop Delay: {0:F2}s", this.loopDelay);
			bool flag = this._mainLoopRoutine != null;
			if (flag)
			{
				MelonCoroutines.Stop(this._mainLoopRoutine);
				this._mainLoopRoutine = this.MainLoop();
				MelonCoroutines.Start(this._mainLoopRoutine);
			}
		}

		// Token: 0x06000767 RID: 1895 RVA: 0x0002E7BC File Offset: 0x0002C9BC
		private Player GetTarget()
		{
			return (MenuEx.QMSelectedUserLocal != null) ? PlayerExtensions.GetPlayer(MenuEx.QMSelectedUserLocal.field_Private_IUser_0) : null;
		}

		// Token: 0x06000768 RID: 1896 RVA: 0x0002E7F0 File Offset: 0x0002C9F0
		private UdonBehaviour GetPlayerUdon(Player player)
		{
			bool flag = player == null;
			UdonBehaviour result;
			if (flag)
			{
				result = null;
			}
			else
			{
				string displayName = player.Method_Internal_get_APIUser_0().displayName;
				for (int i = 0; i < 24; i++)
				{
					GameObject gameObject = GameObject.Find(string.Format("Game Logic/Game Canvas/Game In Progress/Player List/Player List Group/Player Entry ({0})/Player Name Text", i));
					bool flag2 = gameObject == null;
					if (!flag2)
					{
						TextMeshProUGUI component = gameObject.GetComponent<TextMeshProUGUI>();
						bool flag3 = component != null && component.text == displayName;
						if (flag3)
						{
							GameObject gameObject2 = GameObject.Find(string.Format("Game Logic/Player Nodes/Player Node ({0})", i));
							return (gameObject2 != null) ? gameObject2.GetComponent<UdonBehaviour>() : null;
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002E8B0 File Offset: 0x0002CAB0
		private void SendGameLogicEvent(string evt)
		{
			GameObject gameObject = GameObject.Find("Game Logic");
			if (gameObject != null)
			{
				UdonBehaviour component = gameObject.GetComponent<UdonBehaviour>();
				if (component != null)
				{
					component.SendCustomNetworkEvent(0, evt);
				}
			}
		}

		// Token: 0x0600076A RID: 1898 RVA: 0x0002E8E4 File Offset: 0x0002CAE4
		private void DoorEvent(string anim)
		{
			foreach (GameObject gameObject in Murder4Kernel.doors)
			{
				Transform transform = gameObject.transform.Find("Door Anim/Hinge/" + anim);
				UdonBehaviour udonBehaviour = (transform != null) ? transform.GetComponent<UdonBehaviour>() : null;
				bool flag = udonBehaviour != null;
				if (flag)
				{
					udonBehaviour.Interact();
				}
			}
		}

		// Token: 0x0600076B RID: 1899 RVA: 0x0002E96C File Offset: 0x0002CB6C
		private void SyncKill()
		{
			Player target = this.GetTarget();
			bool flag = target == null;
			if (!flag)
			{
				UdonBehaviour playerUdon = this.GetPlayerUdon(target);
				if (playerUdon != null)
				{
					playerUdon.SendCustomNetworkEvent(0, "SyncKill");
				}
			}
		}

		// Token: 0x0600076C RID: 1900 RVA: 0x0002E9A8 File Offset: 0x0002CBA8
		private void SetRole(string role)
		{
			Player target = this.GetTarget();
			bool flag = target == null;
			if (!flag)
			{
				UdonBehaviour playerUdon = this.GetPlayerUdon(target);
				bool flag2 = playerUdon == null;
				if (!flag2)
				{
					if (!true)
					{
					}
					string text;
					if (!(role == "M"))
					{
						if (!(role == "B"))
						{
							if (!(role == "D"))
							{
								text = null;
							}
							else
							{
								text = "SyncAssignD";
							}
						}
						else
						{
							text = "SyncAssignB";
						}
					}
					else
					{
						text = "SyncAssignM";
					}
					if (!true)
					{
					}
					string text2 = text;
					bool flag3 = text2 != null;
					if (flag3)
					{
						playerUdon.SendCustomNetworkEvent(0, text2);
					}
				}
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002EA48 File Offset: 0x0002CC48
		private void ShootTarget(Player player)
		{
			bool flag = player == null || Murder4Kernel.M4Revolver == null;
			if (!flag)
			{
				IEnumerator enumerator = this.FireRevolver(player);
				MelonCoroutines.Start(enumerator);
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002EA82 File Offset: 0x0002CC82
		private IEnumerator FireRevolver(Player player)
		{
			bool flag = player == null || Murder4Kernel.M4Revolver == null;
			if (flag)
			{
				yield break;
			}
			Vector3 pos = player.transform.position;
			Vector3[] offsets = new Vector3[]
			{
				new Vector3(1f, 1f, 0f),
				new Vector3(0f, 1f, 1f),
				new Vector3(0f, 1f, -1f)
			};
			Vector3 originalPos = Murder4Kernel.M4Revolver.transform.position;
			bool flag2 = !Networking.IsOwner(Networking.LocalPlayer, Murder4Kernel.M4Revolver);
			if (flag2)
			{
				Networking.SetOwner(Networking.LocalPlayer, Murder4Kernel.M4Revolver);
			}
			foreach (Vector3 offset in offsets)
			{
				Murder4Kernel.M4Revolver.transform.position = pos + offset;
				Murder4Kernel.M4Revolver.transform.LookAt(pos + new Vector3(0f, 0.5f, 0f));
				UdonBehaviour component = Murder4Kernel.M4Revolver.GetComponent<UdonBehaviour>();
				if (component != null)
				{
					component.SendCustomEvent("Fire");
				}
				yield return new WaitForSeconds(0.1f);
				offset = default(Vector3);
			}
			Vector3[] array = null;
			yield return new WaitForSeconds(0.5f);
			Murder4Kernel.M4Revolver.transform.position = originalPos;
			yield break;
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002EA98 File Offset: 0x0002CC98
		private void SmokeTarget(Player player)
		{
			bool flag = player == null;
			if (!flag)
			{
				MelonCoroutines.Start(this.SmokeByVector3(player.transform.position));
			}
		}

		// Token: 0x06000770 RID: 1904 RVA: 0x0002EACC File Offset: 0x0002CCCC
		private void ExplodeTarget(Player player)
		{
			bool flag = player == null;
			if (!flag)
			{
				MelonCoroutines.Start(this.GranadeExplodeByVector3(player.transform.position));
			}
		}

		// Token: 0x06000771 RID: 1905 RVA: 0x0002EB00 File Offset: 0x0002CD00
		private void ExplodeGun()
		{
			bool flag = Murder4Kernel.M4Revolver == null;
			if (!flag)
			{
				MelonCoroutines.Start(this.GranadeExplodeByVector3(Murder4Kernel.M4Revolver.transform.position));
			}
		}

		// Token: 0x06000772 RID: 1906 RVA: 0x0002EB3C File Offset: 0x0002CD3C
		private void SmokeGun()
		{
			bool flag = Murder4Kernel.M4Revolver == null;
			if (!flag)
			{
				MelonCoroutines.Start(this.SmokeByVector3(Murder4Kernel.M4Revolver.transform.position));
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002EB76 File Offset: 0x0002CD76
		private IEnumerator SmokeByVector3(Vector3 pos)
		{
			GameObject smoke = GameObject.Find("Game Logic/Weapons/Unlockables/Smoke (0)");
			bool flag = smoke == null;
			if (flag)
			{
				yield break;
			}
			GameObject intact = GameObject.Find("Game Logic/Weapons/Unlockables/Smoke (0)/Intact");
			bool flag2 = intact != null;
			if (flag2)
			{
				intact.SetActive(true);
			}
			Vector3 originalPos = smoke.transform.position;
			bool flag3 = !Networking.IsOwner(Networking.LocalPlayer, smoke);
			if (flag3)
			{
				Networking.SetOwner(Networking.LocalPlayer, smoke);
			}
			smoke.transform.position = pos + new Vector3(0f, 0.1f, 0f);
			UdonBehaviour component = smoke.GetComponent<UdonBehaviour>();
			if (component != null)
			{
				component.SendCustomNetworkEvent(0, "Explode");
			}
			yield return new WaitForSeconds(15f);
			bool flag4 = intact != null;
			if (flag4)
			{
				intact.SetActive(true);
			}
			smoke.transform.position = originalPos;
			yield break;
		}

		// Token: 0x06000774 RID: 1908 RVA: 0x0002EB8C File Offset: 0x0002CD8C
		private IEnumerator GranadeExplodeByVector3(Vector3 pos)
		{
			GameObject frag = GameObject.Find("Game Logic/Weapons/Unlockables/Frag (0)");
			bool flag = frag == null;
			if (flag)
			{
				yield break;
			}
			GameObject intact = GameObject.Find("Game Logic/Weapons/Unlockables/Frag (0)/Intact");
			bool flag2 = intact != null;
			if (flag2)
			{
				intact.SetActive(true);
			}
			Vector3 originalPos = frag.transform.position;
			bool flag3 = !Networking.IsOwner(Networking.LocalPlayer, frag);
			if (flag3)
			{
				Networking.SetOwner(Networking.LocalPlayer, frag);
			}
			frag.transform.position = pos + new Vector3(0f, 0.1f, 0f);
			UdonBehaviour component = frag.GetComponent<UdonBehaviour>();
			if (component != null)
			{
				component.SendCustomNetworkEvent(0, "Explode");
			}
			yield return new WaitForSeconds(2f);
			bool flag4 = intact != null;
			if (flag4)
			{
				intact.SetActive(true);
			}
			frag.transform.position = originalPos;
			yield break;
		}

		// Token: 0x06000775 RID: 1909 RVA: 0x0002EBA4 File Offset: 0x0002CDA4
		private void PlaceBearTrap(string trapName, Vector3 location)
		{
			GameObject gameObject = GameObject.Find(trapName);
			bool flag = gameObject == null;
			if (!flag)
			{
				Networking.SetOwner(Networking.LocalPlayer, gameObject);
				gameObject.transform.position = location;
				UdonBehaviour component = gameObject.GetComponent<UdonBehaviour>();
				bool flag2 = component != null;
				if (flag2)
				{
					component.SendCustomNetworkEvent(0, "SyncArm");
					component.SendCustomNetworkEvent(0, "SyncDeploy");
				}
			}
		}

		// Token: 0x06000776 RID: 1910 RVA: 0x0002EC10 File Offset: 0x0002CE10
		private void AddTargetToLoop(Player target)
		{
			bool flag = target == null || target == PlayerUtil.LocalPlayer();
			if (!flag)
			{
				bool flag2 = Murder4Kernel.loopTargets.Contains(target);
				if (!flag2)
				{
					Murder4Kernel.loopTargets.Add(target);
					bool flag3 = this.targetLoopRoutines.ContainsKey(target) && this.targetLoopRoutines[target] != null;
					if (flag3)
					{
						MelonCoroutines.Stop(this.targetLoopRoutines[target]);
					}
					IEnumerator enumerator = this.TargetLoop(target);
					this.targetLoopRoutines[target] = enumerator;
					MelonCoroutines.Start(enumerator);
				}
			}
		}

		// Token: 0x04000393 RID: 915
		private static GameObject M4Shotgun;

		// Token: 0x04000394 RID: 916
		private static GameObject M4Revolver;

		// Token: 0x04000395 RID: 917
		private static List<GameObject> doors = new List<GameObject>();

		// Token: 0x04000396 RID: 918
		private static HashSet<Player> loopTargets = new HashSet<Player>();

		// Token: 0x04000397 RID: 919
		private static Dictionary<Player, UdonBehaviour> playerUdonCache = new Dictionary<Player, UdonBehaviour>();

		// Token: 0x04000398 RID: 920
		private bool autoLockDoors = false;

		// Token: 0x04000399 RID: 921
		private bool loopAll = false;

		// Token: 0x0400039A RID: 922
		private float loopDelay = 0.1f;

		// Token: 0x0400039B RID: 923
		private IEnumerator _mainLoopRoutine;

		// Token: 0x0400039C RID: 924
		private Dictionary<Player, IEnumerator> doorAnnoyanceRoutines = new Dictionary<Player, IEnumerator>();

		// Token: 0x0400039D RID: 925
		private Dictionary<Player, IEnumerator> targetLoopRoutines = new Dictionary<Player, IEnumerator>();

		// Token: 0x0400039E RID: 926
		private ReMenuButton autoLockBtn;

		// Token: 0x0400039F RID: 927
		private ReMenuButton loopAllBtn;

		// Token: 0x040003A0 RID: 928
		private ReMenuButton loopDelayBtn;

		// Token: 0x040003A1 RID: 929
		private ReMenuButton doorAnnoyBtn;
	}
}
