using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;
using Newtonsoft.Json;
using UnityEngine;
using VRC;
using VRC.Core;

// Token: 0x02000012 RID: 18
internal class OSC : KernelModuleBase
{
	// Token: 0x17000018 RID: 24
	// (get) Token: 0x0600005E RID: 94 RVA: 0x0000386C File Offset: 0x00001A6C
	public override string ModuleName
	{
		get
		{
			return "OSC";
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x0600005F RID: 95 RVA: 0x00003873 File Offset: 0x00001A73
	public override string Version
	{
		get
		{
			return "2.1.0";
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000060 RID: 96 RVA: 0x0000387A File Offset: 0x00001A7A
	public override ModuleCapabilities Capabilities
	{
		get
		{
			return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.PlayerEvents | ModuleCapabilities.AvatarEvents | ModuleCapabilities.NetworkEvents | ModuleCapabilities.UdonEvents | ModuleCapabilities.UIInit | ModuleCapabilities.UserLogin;
		}
	}

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000061 RID: 97 RVA: 0x00003312 File Offset: 0x00001512
	public override UpdateFrequency UpdateFrequency
	{
		get
		{
			return UpdateFrequency.Every2Frames;
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00003881 File Offset: 0x00001A81
	public override void OnUiManagerInit()
	{
		this.LoadConfig();
		this.CreateUI();
		this.InitializeOSC();
	}

	// Token: 0x06000063 RID: 99 RVA: 0x0000389C File Offset: 0x00001A9C
	private void InitializeOSC()
	{
		try
		{
			this._vrchatEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9000);
			this._udpClient = new UdpClient();
			this._isConnected = true;
			kernelllogger.Msg("[OSC] OSC client initialized successfully");
		}
		catch (Exception ex)
		{
			kernelllogger.Error("[OSC] Failed to initialize OSC: " + ex.Message);
			this._isConnected = false;
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00003918 File Offset: 0x00001B18
	public override void OnUpdate()
	{
		bool flag = !this._canConnect || !OSC._config.OSC || !this._isConnected;
		if (!flag)
		{
			bool flag2 = Time.time - this._lastOSCUpdate >= 1.5f;
			if (flag2)
			{
				this.ProcessOSCCycle();
				this._lastOSCUpdate = Time.time;
			}
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x0000397C File Offset: 0x00001B7C
	private void CreateUI()
	{
		ReMenuCategory category = MenuSetup._uiManager.QMMenu.GetCategoryPage("OSC").GetCategory("Controls");
		category.AddToggle("OSC Connect", "", delegate(bool s)
		{
			OSC._config.OSC = s;
			this.SaveConfig();
			this._forceUpdate = true;
			bool flag = s && !this._isConnected;
			if (flag)
			{
				this.InitializeOSC();
			}
		}, OSC._config.OSC, "#ffffff");
		category.AddToggle("Spotify", "", delegate(bool s)
		{
			OSC._config.OSCMusic = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCMusic, "#ffffff");
		category.AddToggle("Time", "", delegate(bool s)
		{
			OSC._config.OSCTime = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCTime, "#ffffff");
		category.AddToggle("Messages", "", delegate(bool s)
		{
			OSC._config.OSCMessage = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCMessage, "#ffffff");
		category.AddToggle("System Info", "", delegate(bool s)
		{
			OSC._config.OSCSysInfo = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCSysInfo, "#ffffff");
		category.AddToggle("App Info", "", delegate(bool s)
		{
			OSC._config.OSCAppInfo = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCAppInfo, "#ffffff");
		category.AddToggle("Performance", "", delegate(bool s)
		{
			OSC._config.OSCPerformance = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCPerformance, "#ffffff");
		category.AddToggle("Rape Broadcast", "", delegate(bool s)
		{
			OSC._config.OSCPlayerGreeting = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCPlayerGreeting, "#ffffff");
		category.AddToggle("Battery", "", delegate(bool s)
		{
			OSC._config.OSCBattery = s;
			this.SaveConfig();
			this._forceUpdate = true;
		}, OSC._config.OSCBattery, "#ffffff");
		category.AddButton("Add Message", "", new Action(this.AddMessage), null, "#ffffff");
		category.AddButton("Clear All Messages", "", new Action(this.ClearMessage), null, "#ffffff");
		category.AddButton("Force Refresh", "", delegate
		{
			this._forceUpdate = true;
		}, null, "#ffffff");
		category.AddButton("Test OSC", "", new Action(this.TestOSC), null, "#ffffff");
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00003BC4 File Offset: 0x00001DC4
	private void TestOSC()
	{
		bool isConnected = this._isConnected;
		if (isConnected)
		{
			this.SendOSCMessage("/chatbox/input", "OSC Test Message", true);
			kernelllogger.Msg("[OSC] Test message sent");
		}
		else
		{
			kernelllogger.Error("[OSC] Cannot test - OSC not connected");
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x00003C0C File Offset: 0x00001E0C
	[DebuggerStepThrough]
	private void AddMessage()
	{
		OSC.<AddMessage>d__29 <AddMessage>d__ = new OSC.<AddMessage>d__29();
		<AddMessage>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
		<AddMessage>d__.<>4__this = this;
		<AddMessage>d__.<>1__state = -1;
		<AddMessage>d__.<>t__builder.Start<OSC.<AddMessage>d__29>(ref <AddMessage>d__);
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00003C45 File Offset: 0x00001E45
	private void ClearMessage()
	{
		OSC._config.OSCMessageList.Clear();
		this.SaveConfig();
		this._forceUpdate = true;
	}

	// Token: 0x06000069 RID: 105 RVA: 0x00003C68 File Offset: 0x00001E68
	private void ProcessOSCCycle()
	{
		bool flag = !this._isConnected;
		if (!flag)
		{
			this._messageBuilder.Clear();
			this.GetTime();
			this.SpotifyFind();
			this.MessageSend();
			this.GetSystemInfo();
			this.GetAppInfo();
			this.GetPerformanceInfo();
			bool oscplayerGreeting = OSC._config.OSCPlayerGreeting;
			if (oscplayerGreeting)
			{
				this.GetRandomPlayerGreeting();
			}
			bool oscbattery = OSC._config.OSCBattery;
			if (oscbattery)
			{
				this.GetBatteryInfo();
			}
			string text = this._messageBuilder.ToString();
			bool flag2 = (text != this._lastSentMessage || this._forceUpdate) && !string.IsNullOrEmpty(text);
			if (flag2)
			{
				this.SendOSCMessage("/chatbox/input", text, true);
				this._lastSentMessage = text;
				this._forceUpdate = false;
			}
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00003D40 File Offset: 0x00001F40
	private void SendOSCMessage(string address, object value, bool immediate = false)
	{
		bool flag = !this._isConnected || this._udpClient == null;
		if (!flag)
		{
			try
			{
				bool flag2 = address == "/chatbox/input";
				byte[] array;
				if (flag2)
				{
					array = this.CreateOSCMessage(address, new object[]
					{
						value.ToString(),
						immediate
					});
				}
				else
				{
					array = this.CreateOSCMessage(address, new object[]
					{
						value
					});
				}
				this._udpClient.Send(array, array.Length, this._vrchatEndpoint);
				kernelllogger.Msg(string.Format("[OSC] Sent: {0} = {1}", address, value));
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[OSC] Failed to send message: " + ex.Message);
				this.InitializeOSC();
			}
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x00003E14 File Offset: 0x00002014
	private byte[] CreateOSCMessage(string address, params object[] args)
	{
		byte[] result;
		using (MemoryStream memoryStream = new MemoryStream())
		{
			byte[] bytes = Encoding.UTF8.GetBytes(address);
			memoryStream.Write(bytes, 0, bytes.Length);
			int num = 4 - bytes.Length % 4;
			for (int i = 0; i < num; i++)
			{
				memoryStream.WriteByte(0);
			}
			string text = ",";
			foreach (object obj in args)
			{
				bool flag = obj is string;
				if (flag)
				{
					text += "s";
				}
				else
				{
					bool flag2 = obj is int;
					if (flag2)
					{
						text += "i";
					}
					else
					{
						bool flag3 = obj is float;
						if (flag3)
						{
							text += "f";
						}
						else
						{
							bool flag4 = obj is bool;
							if (flag4)
							{
								text += (((bool)obj) ? "T" : "F");
							}
						}
					}
				}
			}
			byte[] bytes2 = Encoding.UTF8.GetBytes(text);
			memoryStream.Write(bytes2, 0, bytes2.Length);
			int num2 = 4 - bytes2.Length % 4;
			for (int k = 0; k < num2; k++)
			{
				memoryStream.WriteByte(0);
			}
			foreach (object obj2 in args)
			{
				string text2 = obj2 as string;
				bool flag5 = text2 != null;
				if (flag5)
				{
					byte[] bytes3 = Encoding.UTF8.GetBytes(text2);
					memoryStream.Write(bytes3, 0, bytes3.Length);
					int num3 = 4 - bytes3.Length % 4;
					for (int m = 0; m < num3; m++)
					{
						memoryStream.WriteByte(0);
					}
				}
				else
				{
					int host;
					bool flag6;
					if (obj2 is int)
					{
						host = (int)obj2;
						flag6 = true;
					}
					else
					{
						flag6 = false;
					}
					bool flag7 = flag6;
					if (flag7)
					{
						byte[] bytes4 = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(host));
						memoryStream.Write(bytes4, 0, 4);
					}
					else
					{
						float value;
						bool flag8;
						if (obj2 is float)
						{
							value = (float)obj2;
							flag8 = true;
						}
						else
						{
							flag8 = false;
						}
						bool flag9 = flag8;
						if (flag9)
						{
							byte[] bytes5 = BitConverter.GetBytes(value);
							bool isLittleEndian = BitConverter.IsLittleEndian;
							if (isLittleEndian)
							{
								Array.Reverse(bytes5);
							}
							memoryStream.Write(bytes5, 0, 4);
						}
					}
				}
			}
			result = memoryStream.ToArray();
		}
		return result;
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0000409C File Offset: 0x0000229C
	public override void OnEnterWorld(ApiWorld world, ApiWorldInstance instance)
	{
		this._canConnect = true;
		this._forceUpdate = true;
		kernelllogger.Msg("[OSC] Entered world, enabling OSC connection");
	}

	// Token: 0x0600006D RID: 109 RVA: 0x000040B8 File Offset: 0x000022B8
	public override void OnLeaveWorld()
	{
		this._canConnect = false;
		kernelllogger.Msg("[OSC] Left world, disabling OSC connection");
	}

	// Token: 0x0600006E RID: 110 RVA: 0x000040D0 File Offset: 0x000022D0
	private void GetTime()
	{
		bool flag = !OSC._config.OSCTime;
		if (!flag)
		{
			this.AppendWithNewline("\ud83d\udd52 " + DateTime.Now.ToString("HH:mm:ss"));
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00004114 File Offset: 0x00002314
	public void SpotifyFind()
	{
		bool flag = !OSC._config.OSCMusic;
		if (!flag)
		{
			bool flag2 = Time.time - this._lastSpotifyCheck < 2f;
			if (flag2)
			{
				Process[] spotifyProcesses = this._spotifyProcesses;
				bool flag3 = spotifyProcesses != null && spotifyProcesses.Length != 0 && !this._spotifyProcesses[0].HasExited;
				if (flag3)
				{
					this.ProcessSpotifyTitle(this._spotifyProcesses[0].MainWindowTitle);
					return;
				}
			}
			this._lastSpotifyCheck = Time.time;
			try
			{
				this._spotifyProcesses = Process.GetProcessesByName("Spotify");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[OSC] Error getting Spotify processes: " + ex.Message);
				return;
			}
			bool flag4 = this._spotifyProcesses.Length == 0;
			if (flag4)
			{
				bool flag5 = !string.IsNullOrEmpty(this._lastSong);
				if (flag5)
				{
					this.AppendWithNewline("\ud83c\udfb5 No Spotify");
					this._lastSong = "";
				}
			}
			else
			{
				this.ProcessSpotifyTitle(this._spotifyProcesses[0].MainWindowTitle);
			}
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00004234 File Offset: 0x00002434
	private void ProcessSpotifyTitle(string title)
	{
		bool flag = string.IsNullOrEmpty(title) || title == "Spotify";
		if (flag)
		{
			bool flag2 = !string.IsNullOrEmpty(this._lastSong);
			if (flag2)
			{
				this.AppendWithNewline("⏸️ " + this._lastSong.Replace("▶ ", ""));
			}
		}
		else
		{
			string[] array = title.Split(new char[]
			{
				'-'
			}, 2);
			bool flag3 = array.Length > 1;
			if (flag3)
			{
				string str = array[0].Trim();
				string str2 = array[1].Trim();
				string text = "▶ " + str2 + " by " + str;
				this.AppendWithNewline("\ud83c\udfb5 " + text);
				this._lastSong = text;
			}
			else
			{
				this.AppendWithNewline("\ud83c\udfb5 ▶ " + title);
				this._lastSong = title;
			}
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000431C File Offset: 0x0000251C
	public void MessageSend()
	{
		bool flag = !OSC._config.OSCMessage || OSC._config.OSCMessageList.Count == 0;
		if (!flag)
		{
			int count = OSC._config.OSCMessageList.Count;
			int num = this.lastMessageSent;
			bool flag2 = num >= count;
			if (flag2)
			{
				num = 0;
				this.lastMessageSent = 0;
			}
			this.AppendWithNewline("\ud83d\udcac " + OSC._config.OSCMessageList[num]);
			this.lastMessageSent++;
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x000043B0 File Offset: 0x000025B0
	private void GetSystemInfo()
	{
		bool flag = !OSC._config.OSCSysInfo;
		if (!flag)
		{
			string arg = SystemInfo.operatingSystem.Split(new char[]
			{
				' '
			})[0];
			string text = string.Format("\ud83d\udcbb {0} | {1}C | {2}MB", arg, SystemInfo.processorCount, SystemInfo.systemMemorySize);
			this.AppendWithNewline(text);
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00004414 File Offset: 0x00002614
	private void GetAppInfo()
	{
		bool flag = !OSC._config.OSCAppInfo;
		if (!flag)
		{
			string text = "\ud83c\udfae " + Application.productName + " v" + Application.version;
			this.AppendWithNewline(text);
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00004458 File Offset: 0x00002658
	private void GetPerformanceInfo()
	{
		bool flag = !OSC._config.OSCPerformance;
		if (!flag)
		{
			int num = Mathf.RoundToInt(1f / Time.deltaTime);
			string arg = (num >= 90) ? "\ud83d\udfe2" : ((num >= 60) ? "\ud83d\udfe1" : "\ud83d\udd34");
			this.AppendWithNewline(string.Format("{0} {1} FPS", arg, num));
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x000044C0 File Offset: 0x000026C0
	private void GetRandomPlayerGreeting()
	{
		PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
		List<Player> list = (playerManager != null) ? playerManager.field_Private_List_1_Player_0 : null;
		bool flag = list == null || list.Count <= 1;
		if (!flag)
		{
			List<Player> list2 = new List<Player>();
			Player player = PlayerManager.Method_Public_Static_get_Player_PDM_0();
			foreach (Player player2 in list)
			{
				bool flag2 = player2 != null && player2 != player;
				if (flag2)
				{
					list2.Add(player2);
				}
			}
			bool flag3 = list2.Count > 0;
			if (flag3)
			{
				int index = Random.Range(0, list2.Count);
				Player player3 = list2[index];
				string text;
				if (player3 == null)
				{
					text = null;
				}
				else
				{
					APIUser apiuser = player3.Method_Internal_get_APIUser_0();
					text = ((apiuser != null) ? apiuser.displayName : null);
				}
				string str = text ?? "Unknown";
				this.AppendWithNewline("Hey " + str + " im gonna fucking rape you <3");
			}
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x000045B4 File Offset: 0x000027B4
	private void GetBatteryInfo()
	{
		try
		{
			this.AppendWithNewline("\ud83d\udd0b Battery: 100%");
		}
		catch (Exception ex)
		{
			kernelllogger.Error("[OSC] Error getting battery info: " + ex.Message);
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00004600 File Offset: 0x00002800
	private void AppendWithNewline(string text)
	{
		bool flag = this._messageBuilder.Length > 0;
		if (flag)
		{
			this._messageBuilder.AppendLine();
		}
		this._messageBuilder.Append(text);
	}

	// Token: 0x06000078 RID: 120 RVA: 0x0000463C File Offset: 0x0000283C
	private void LoadConfig()
	{
		bool flag = File.Exists("UserData/OSCConfig.json");
		if (flag)
		{
			try
			{
				string text = File.ReadAllText("UserData/OSCConfig.json");
				OSC._config = JsonConvert.DeserializeObject<OSCConfig>(text);
				kernelllogger.Msg("[OSC] Config loaded successfully");
			}
			catch (Exception ex)
			{
				kernelllogger.Error("[OSC] Failed to load config: " + ex.Message);
				OSC._config = new OSCConfig();
				this.SaveConfig();
			}
		}
		else
		{
			OSC._config = new OSCConfig();
			this.SaveConfig();
			kernelllogger.Msg("[OSC] Created new config file");
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000046DC File Offset: 0x000028DC
	private void SaveConfig()
	{
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName("UserData/OSCConfig.json") ?? string.Empty);
			string contents = JsonConvert.SerializeObject(OSC._config, 1);
			File.WriteAllText("UserData/OSCConfig.json", contents);
		}
		catch (Exception ex)
		{
			kernelllogger.Error("[OSC] Failed to save config: " + ex.Message);
		}
	}

	// Token: 0x04000023 RID: 35
	private const string ConfigPath = "UserData/OSCConfig.json";

	// Token: 0x04000024 RID: 36
	private static OSCConfig _config;

	// Token: 0x04000025 RID: 37
	private bool _canConnect;

	// Token: 0x04000026 RID: 38
	private UdpClient _udpClient;

	// Token: 0x04000027 RID: 39
	private IPEndPoint _vrchatEndpoint;

	// Token: 0x04000028 RID: 40
	private bool _isConnected = false;

	// Token: 0x04000029 RID: 41
	private int lastMessageSent = 0;

	// Token: 0x0400002A RID: 42
	private string _lastSong = "";

	// Token: 0x0400002B RID: 43
	private readonly StringBuilder _messageBuilder = new StringBuilder(512);

	// Token: 0x0400002C RID: 44
	private Process[] _spotifyProcesses;

	// Token: 0x0400002D RID: 45
	private float _lastSpotifyCheck = 0f;

	// Token: 0x0400002E RID: 46
	private const float SPOTIFY_CHECK_INTERVAL = 2f;

	// Token: 0x0400002F RID: 47
	private string _lastSentMessage = "";

	// Token: 0x04000030 RID: 48
	private bool _forceUpdate = false;

	// Token: 0x04000031 RID: 49
	private float _lastOSCUpdate = 0f;

	// Token: 0x04000032 RID: 50
	private const float OSC_UPDATE_INTERVAL = 1.5f;
}
