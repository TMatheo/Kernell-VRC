using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KernellClientUI.UI.QuickMenu;
using KernellClientUI.VRChat;
using KernellVRC;
using KernellVRCLite.Utils;
using KernelVRC;
using MelonLoader;
using TMPro;
using UnityEngine;
using VRC;
using VRC.Udon;

namespace KernellVRCLite.Modules.World
{
	// Token: 0x02000095 RID: 149
	internal class AmongUsKernel : KernelModuleBase
	{
		// Token: 0x1700015B RID: 347
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0002D423 File Offset: 0x0002B623
		public override string ModuleName
		{
			get
			{
				return "Among us";
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x06000731 RID: 1841 RVA: 0x00005413 File Offset: 0x00003613
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.WorldEvents | ModuleCapabilities.UdonEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x06000733 RID: 1843 RVA: 0x0002D42C File Offset: 0x0002B62C
		public override void OnUiManagerInit()
		{
			ReMenuCategory reMenuCategory = MenuSetup._uiManager.QMMenu.GetCategoryPage("Exploits").GetCategory("Game Worlds").AddCategoryPage("Among Us", "", null, "#ffffff").AddCategory("Logic", true, "#ffffff", false);
			reMenuCategory.AddButton("Start Game", "", delegate
			{
				this.SendEvent("SyncCountdown");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Abort Game", "", delegate
			{
				this.SendEvent("SyncAbort");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Bystanders Win", "", delegate
			{
				this.SendEvent("SyncVictoryC");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Imposter Win", "", delegate
			{
				this.SendEvent("SyncVictoryI");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Complete All Tasks", "", delegate
			{
				this.SendEvent("OnLocalPlayerCompletedTask");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Call Meeting", "", delegate
			{
				this.SendEvent("StartMeeting");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Emergency Meeting", "", delegate
			{
				this.SendEvent("SyncEmergencyMeeting");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Report Body", "", delegate
			{
				this.SendEvent("OnBodyWasFound");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Skip Vote", "", delegate
			{
				this.SendEvent("Btn_SkipVoting");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Close Voting", "", delegate
			{
				this.SendEvent("SyncCloseVoting");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Kill All", "", delegate
			{
				this.SendEvent("KillLocalPlayer");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Enable Vents", "", new Action(AmongUsKernel.EnableVents), null, "#ffffff");
			reMenuCategory.AddButton("Trigger All Sabotages", "", delegate
			{
				this.SendEvent("SyncDoSabotageLights");
				this.SendEvent("SyncDoSabotageReactor");
				this.SendEvent("SyncDoSabotageComms");
				this.SendEvent("SyncDoSabotageOxygen");
			}, null, "#ffffff");
			reMenuCategory.AddButton("Stop All Sabotages", "", delegate
			{
				this.SendEvent("CancelAllSabotage");
			}, null, "#ffffff");
			reMenuCategory.AddToggle("AnyKill", "Loop Kill Buttons", delegate(bool state)
			{
				AmongUsKernel.AnyKill = state;
				bool anyKill = AmongUsKernel.AnyKill;
				if (anyKill)
				{
					this.anyKillCoroutine = (Coroutine)MelonCoroutines.Start(this.AnyKillLoop());
				}
				else
				{
					bool flag = this.anyKillCoroutine != null;
					if (flag)
					{
						MelonCoroutines.Stop(this.anyKillCoroutine);
						this.anyKillCoroutine = null;
					}
				}
			}, false, "#ffffff");
			reMenuCategory.AddToggle("Loop All", "Kill everyone in loop", delegate(bool state)
			{
				AmongUsKernel.LoopAll = state;
				bool loopAll = AmongUsKernel.LoopAll;
				if (loopAll)
				{
					foreach (Player player in Enumerable.Where<Player>(PlayerUtil.GetAllPlayers(), (Player x) => x != PlayerUtil.LocalPlayer()))
					{
						this.AddToLoop(player._vrcplayer);
					}
				}
				else
				{
					foreach (object obj in this.playerLoops.Values)
					{
						MelonCoroutines.Stop(obj);
					}
					this.playerLoops.Clear();
					this.loopTargets.Clear();
				}
			}, false, "#ffffff");
			ReMenuPage reMenuPage = MenuSetup._uiManager.TargetMenu.AddMenuPage("Among Us Target", "", null, "#ffffff");
			reMenuPage.AddButton("Kill", "", delegate
			{
				this.KillTarget(this.GetTarget());
			}, null, "#ffffff");
			reMenuPage.AddButton("Report", "", delegate
			{
				MelonCoroutines.Start(this.ReportTarget(this.GetTarget()));
			}, null, "#ffffff");
			reMenuPage.AddButton("Eject", "", delegate
			{
				this.SendNodeEvent(this.GetTarget(), "SyncVotedOut");
			}, null, "#ffffff");
			reMenuPage.AddButton("Assign Crew", "", delegate
			{
				this.SendNodeEvent(this.GetTarget(), "SyncAssignB");
			}, null, "#ffffff");
			reMenuPage.AddButton("Assign Imposter", "", delegate
			{
				this.SendNodeEvent(this.GetTarget(), "SyncAssignM");
			}, null, "#ffffff");
			reMenuPage.AddButton("Vote Legit", "", delegate
			{
				MelonCoroutines.Start(this.VoteOutTarget(this.GetTarget(), true));
			}, null, "#ffffff");
			reMenuPage.AddButton("Vote Obvious", "", delegate
			{
				MelonCoroutines.Start(this.VoteOutTarget(this.GetTarget(), false));
			}, null, "#ffffff");
			reMenuPage.AddButton("Add Loop", "", delegate
			{
				this.AddToLoop(this.GetTarget());
			}, null, "#ffffff");
			reMenuPage.AddButton("Stop All Loops", "", delegate
			{
				foreach (object obj in this.playerLoops.Values)
				{
					MelonCoroutines.Stop(obj);
				}
				this.loopTargets.Clear();
				this.playerLoops.Clear();
			}, null, "#ffffff");
		}

		// Token: 0x06000734 RID: 1844 RVA: 0x0002D80A File Offset: 0x0002BA0A
		private IEnumerator AnyKillLoop()
		{
			List<GameObject> killButtons = new List<GameObject>();
			int num;
			for (int i = 0; i < 24; i = num + 1)
			{
				GameObject go = GameObject.Find(string.Format("Game Logic/Player Nodes/Player Node ({0})/Player Kill Button", i));
				bool flag = go != null;
				if (flag)
				{
					killButtons.Add(go);
				}
				go = null;
				num = i;
			}
			while (AmongUsKernel.AnyKill)
			{
				foreach (GameObject btn in killButtons)
				{
					btn.SetActive(true);
					btn = null;
				}
				List<GameObject>.Enumerator enumerator = default(List<GameObject>.Enumerator);
				yield return new WaitForSeconds(0.5f);
			}
			yield break;
		}

		// Token: 0x06000735 RID: 1845 RVA: 0x0002D81C File Offset: 0x0002BA1C
		public static void EnableVents()
		{
			GameObject[] array = Object.FindObjectsOfType<GameObject>();
			foreach (GameObject gameObject in array)
			{
				bool flag = gameObject.name.ToLower().Contains("vent");
				if (flag)
				{
					Collider component = gameObject.GetComponent<Collider>();
					bool flag2 = component != null;
					if (flag2)
					{
						component.enabled = true;
					}
				}
			}
		}

		// Token: 0x06000736 RID: 1846 RVA: 0x0002D88C File Offset: 0x0002BA8C
		private void AddToLoop(VRCPlayer player)
		{
			bool flag = player == null || this.loopTargets.Contains(player);
			if (!flag)
			{
				this.loopTargets.Add(player);
				object value = MelonCoroutines.Start(this.LoopPlayer(player));
				this.playerLoops[player] = value;
			}
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x0002D8E0 File Offset: 0x0002BAE0
		private IEnumerator LoopPlayer(VRCPlayer player)
		{
			GameObject node = this.GetNodeObject(player);
			bool flag = !node;
			if (flag)
			{
				yield break;
			}
			UdonBehaviour udon = node.GetComponent<UdonBehaviour>();
			float delay = 0.07f;
			while (AmongUsKernel.LoopAll || this.loopTargets.Contains(player))
			{
				udon.SendCustomNetworkEvent(0, "SyncAssignB");
				yield return new WaitForSeconds(delay);
				udon.SendCustomNetworkEvent(0, "SyncKill");
				yield return new WaitForSeconds(delay);
			}
			yield break;
		}

		// Token: 0x06000738 RID: 1848 RVA: 0x0002D8F6 File Offset: 0x0002BAF6
		private IEnumerator ReportTarget(VRCPlayer player)
		{
			GameObject node = this.GetNodeObject(player);
			GameObject gameObject = node;
			if (gameObject != null)
			{
				UdonBehaviour component = gameObject.GetComponent<UdonBehaviour>();
				if (component != null)
				{
					component.SendCustomNetworkEvent(0, "SyncPlayerIsReporter");
				}
			}
			this.SendEvent("StartMeeting");
			this.SendEvent("SyncEmergencyMeeting");
			yield return new WaitForSeconds(0.1f);
			yield break;
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x0002D90C File Offset: 0x0002BB0C
		private IEnumerator VoteOutTarget(VRCPlayer player, bool legit)
		{
			string nodeId = this.GetNodeIndex(player);
			Player[] all = PlayerUtil.GetAllPlayers();
			IEnumerable<Player> enumerable2;
			if (!legit)
			{
				IEnumerable<Player> enumerable = all;
				enumerable2 = enumerable;
			}
			else
			{
				enumerable2 = Enumerable.Take<Player>(Enumerable.OrderBy<Player, float>(all, (Player _) => Random.value), Mathf.Max(2, all.Length / 2 + 1));
			}
			IEnumerable<Player> players = enumerable2;
			foreach (Player p in players)
			{
				GameObject node = this.GetNodeObject(p._vrcplayer);
				GameObject gameObject = node;
				if (gameObject != null)
				{
					UdonBehaviour component = gameObject.GetComponent<UdonBehaviour>();
					if (component != null)
					{
						component.SendCustomNetworkEvent(0, "SyncVotedFor" + nodeId);
					}
				}
				yield return new WaitForSeconds(0.03f);
				node = null;
				p = null;
			}
			IEnumerator<Player> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x0002D929 File Offset: 0x0002BB29
		private void KillTarget(VRCPlayer player)
		{
			this.SendNodeEvent(player, "SyncKill");
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x0002D93C File Offset: 0x0002BB3C
		private GameObject GetNodeObject(VRCPlayer player)
		{
			string b = (player != null) ? player._player.ToString() : null;
			for (int i = 0; i < 24; i++)
			{
				GameObject gameObject = GameObject.Find(string.Format("Game Logic/Game Canvas/Game In Progress/Player List/Player List Group/Player Entry ({0})/Player Name Text", i));
				bool flag = gameObject != null && gameObject.GetComponent<TextMeshProUGUI>().text == b;
				if (flag)
				{
					return GameObject.Find(string.Format("Game Logic/Player Nodes/Player Node ({0})", i));
				}
			}
			return null;
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x0002D9C8 File Offset: 0x0002BBC8
		private string GetNodeIndex(VRCPlayer player)
		{
			string b = (player != null) ? player._player.ToString() : null;
			for (int i = 0; i < 24; i++)
			{
				GameObject gameObject = GameObject.Find(string.Format("Game Logic/Game Canvas/Game In Progress/Player List/Player List Group/Player Entry ({0})/Player Name Text", i));
				bool flag = gameObject != null && gameObject.GetComponent<TextMeshProUGUI>().text == b;
				if (flag)
				{
					return i.ToString();
				}
			}
			return "0";
		}

		// Token: 0x0600073D RID: 1853 RVA: 0x0002DA4A File Offset: 0x0002BC4A
		private void SendEvent(string eventName)
		{
			GameObject gameObject = GameObject.Find("Game Logic");
			if (gameObject != null)
			{
				UdonBehaviour component = gameObject.GetComponent<UdonBehaviour>();
				if (component != null)
				{
					component.SendCustomNetworkEvent(0, eventName);
				}
			}
		}

		// Token: 0x0600073E RID: 1854 RVA: 0x0002DA70 File Offset: 0x0002BC70
		private void SendNodeEvent(VRCPlayer player, string eventName)
		{
			GameObject nodeObject = this.GetNodeObject(player);
			if (nodeObject != null)
			{
				UdonBehaviour component = nodeObject.GetComponent<UdonBehaviour>();
				if (component != null)
				{
					component.SendCustomNetworkEvent(0, eventName);
				}
			}
		}

		// Token: 0x0600073F RID: 1855 RVA: 0x0002DAA0 File Offset: 0x0002BCA0
		private VRCPlayer GetTarget()
		{
			VRCPlayer result;
			if (!(MenuEx.QMSelectedUserLocal != null))
			{
				result = null;
			}
			else
			{
				Player player = PlayerExtensions.GetPlayer(MenuEx.QMSelectedUserLocal.Method_Public_Virtual_Final_New_get_String_0());
				result = ((player != null) ? player._vrcplayer : null);
			}
			return result;
		}

		// Token: 0x0400038E RID: 910
		private static bool AnyKill;

		// Token: 0x0400038F RID: 911
		private static bool LoopAll;

		// Token: 0x04000390 RID: 912
		private Coroutine anyKillCoroutine;

		// Token: 0x04000391 RID: 913
		private HashSet<VRCPlayer> loopTargets = new HashSet<VRCPlayer>();

		// Token: 0x04000392 RID: 914
		private Dictionary<VRCPlayer, object> playerLoops = new Dictionary<VRCPlayer, object>();
	}
}
