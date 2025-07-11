using System;
using System.Collections.Generic;
using System.IO;
using Il2CppSystem;
using Il2CppSystem.Collections.Generic;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernellVRCLite.Core.Mono;
using KernellVRCLite.player.Mono;
using KernelVRC;
using MelonLoader;
using Newtonsoft.Json;
using UnhollowerBaseLib;
using UnhollowerRuntimeLib;
using UnityEngine;
using VRC;
using VRC.Core;

// Token: 0x02000015 RID: 21
internal class Tags : KernelModuleBase
{
	// Token: 0x060000AB RID: 171 RVA: 0x00004AC8 File Offset: 0x00002CC8
	static Tags()
	{
		Tags.LoadConfig();
		Tags.ApplyToManager();
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060000AC RID: 172 RVA: 0x00004AD7 File Offset: 0x00002CD7
	public override ModuleCapabilities Capabilities
	{
		get
		{
			return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.PlayerEvents | ModuleCapabilities.WorldEvents | ModuleCapabilities.AvatarEvents | ModuleCapabilities.NetworkEvents | ModuleCapabilities.UdonEvents | ModuleCapabilities.MenuEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060000AD RID: 173 RVA: 0x00004ADE File Offset: 0x00002CDE
	public override UpdateFrequency UpdateFrequency
	{
		get
		{
			return UpdateFrequency.Every15Frames;
		}
	}

	// Token: 0x060000AE RID: 174 RVA: 0x00004AE4 File Offset: 0x00002CE4
	private static void LoadConfig()
	{
		bool flag = File.Exists("TagsConfig.json");
		if (flag)
		{
			try
			{
				Tags._config = JsonConvert.DeserializeObject<TagsConfig>(File.ReadAllText("TagsConfig.json"));
				kernelllogger.Msg("Tags config loaded successfully");
			}
			catch (Exception ex)
			{
				string str = "Failed to load Tags config: ";
				Exception ex2 = ex;
				kernelllogger.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				Tags._config = new TagsConfig();
			}
		}
		else
		{
			kernelllogger.Msg("Creating new Tags config file");
			Tags._config = new TagsConfig();
			Tags.SaveConfig();
		}
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00004B80 File Offset: 0x00002D80
	private static void SaveConfig()
	{
		try
		{
			File.WriteAllText("TagsConfig.json", JsonConvert.SerializeObject(Tags._config, 1));
			kernelllogger.Msg("Tags config saved successfully");
		}
		catch (Exception ex)
		{
			string str = "Failed to save Tags config: ";
			Exception ex2 = ex;
			kernelllogger.Error(str + ((ex2 != null) ? ex2.ToString() : null));
		}
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x00004BE8 File Offset: 0x00002DE8
	private static void ApplyToManager()
	{
		CustomNameplateManager.BackgroundSpriteApply = Tags._config.BackgroundSpriteApply;
		CustomNameplateManager.UseKernelSprite = Tags._config.UseKernelSprite;
		CustomNameplateManager.Use2018Sprite = Tags._config.Use2018Sprite;
		CustomNameplateManager.EnableFrameColor = Tags._config.EnableFrameColor;
		CustomNameplateManager.EnablePingColor = Tags._config.EnablePingColor;
		kernelllogger.Msg("Applied config settings to CustomNameplateManager");
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060000B1 RID: 177 RVA: 0x00004C51 File Offset: 0x00002E51
	public override string ModuleName
	{
		get
		{
			return "Tags";
		}
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x00004C58 File Offset: 0x00002E58
	public override void OnUiManagerInit()
	{
		ReMenuCategory category = MenuSetup._uiManager.QMMenu.GetCategoryPage("Config").AddCategory("Nameplate", true, "#ffffff", false);
		this.AddToggleButton(category, "Symbol Indicator Nameplate", () => Tags._config.Tags, delegate(bool val)
		{
			Tags._config.Tags = val;
		});
		this.AddToggleButton(category, "Account Creation Date Nameplate", () => Tags._config.TagsDates, delegate(bool val)
		{
			Tags._config.TagsDates = val;
		});
		this.AddToggleButton(category, "Rank Nameplate Colours", () => Tags._config.NameplateColours, delegate(bool val)
		{
			Tags._config.NameplateColours = val;
		});
		this.AddToggleButton(category, "Enable Frame Color", () => Tags._config.EnableFrameColor, delegate(bool val)
		{
			Tags._config.EnableFrameColor = val;
			CustomNameplateManager.EnableFrameColor = val;
		});
		this.AddToggleButton(category, "Enable Ping Color", () => Tags._config.EnablePingColor, delegate(bool val)
		{
			Tags._config.EnablePingColor = val;
			CustomNameplateManager.EnablePingColor = val;
		});
		kernelllogger.Msg("Tags UI initialized successfully");
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00004E10 File Offset: 0x00003010
	private void AddToggleButton(ReMenuCategory category, string label, Func<bool> get, Action<bool> set)
	{
		ReMenuToggle reMenuToggle = new ReMenuToggle(label, "Toggle " + label, delegate(bool newValue)
		{
			set(newValue);
			Tags.SaveConfig();
			kernelllogger.Msg(string.Format("{0} set to: {1}", label, newValue));
		}, category.RectTransform, get(), null, null, "#FFFFFF", true, "ENABLED", "DISABLED");
		reMenuToggle.SetValue(get(), false);
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00004E87 File Offset: 0x00003087
	public override void OnLateUpdate()
	{
		this._playerCheckTimer = 0f;
		this.CheckPlayerChanges();
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00004E9C File Offset: 0x0000309C
	private void CheckPlayerChanges()
	{
		try
		{
			PlayerManager playerManager = PlayerManager.Method_Private_Static_get_PlayerManager_0();
			bool flag = playerManager == null;
			if (!flag)
			{
				List<Player> field_Private_List_1_Player_ = playerManager.field_Private_List_1_Player_0;
				bool flag2 = field_Private_List_1_Player_ == null;
				if (!flag2)
				{
					Dictionary<string, Player> dictionary = new Dictionary<string, Player>();
					foreach (Player player in field_Private_List_1_Player_)
					{
						bool flag3 = player != null && player.field_Private_APIUser_0 != null;
						if (flag3)
						{
							string id = player.field_Private_APIUser_0.id;
							bool flag4 = !string.IsNullOrEmpty(id);
							if (flag4)
							{
								dictionary[id] = player;
							}
						}
					}
					foreach (KeyValuePair<string, Player> keyValuePair in dictionary)
					{
						bool flag5 = !this._cachedPlayers.ContainsKey(keyValuePair.Key);
						if (flag5)
						{
							kernelllogger.Msg("Player joined: " + keyValuePair.Value.field_Private_APIUser_0.displayName);
							this.HandlePlayerJoined(keyValuePair.Value);
						}
					}
					foreach (KeyValuePair<string, Player> keyValuePair2 in this._cachedPlayers)
					{
						bool flag6 = !dictionary.ContainsKey(keyValuePair2.Key);
						if (flag6)
						{
							kernelllogger.Msg("Player left: " + keyValuePair2.Value.field_Private_APIUser_0.displayName);
							this.HandlePlayerLeft(keyValuePair2.Value);
						}
					}
					this._cachedPlayers = dictionary;
				}
			}
		}
		catch (Exception arg)
		{
			kernelllogger.Error(string.Format("Error in CheckPlayerChanges: {0}", arg));
		}
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x000050AC File Offset: 0x000032AC
	private void HandlePlayerJoined(Player player)
	{
		try
		{
			bool flag = player != null;
			if (flag)
			{
				try
				{
					CustomNameplate customNameplate = VRCApplication.Method_Internal_Static_get_VRCApplication_PDM_0().gameObject.AddComponent<CustomNameplate>();
					customNameplate.SetPlayer(player);
					kernelllogger.Msg("Added CustomNameplate for " + player.field_Private_APIUser_0.displayName);
				}
				catch (Exception arg)
				{
					kernelllogger.Error(string.Format("Error adding CustomNameplate: {0}", arg));
				}
				try
				{
					CustomNameplateAccountAge nameplateAge = VRCApplication.Method_Internal_Static_get_VRCApplication_PDM_0().gameObject.AddComponent<CustomNameplateAccountAge>();
					nameplateAge.SetPlayer(player);
					Action<string> action = DelegateSupport.ConvertDelegate<Action<string>>(new Action<string>(delegate(string errorMessage)
					{
						kernelllogger.Error("Failed to fetch user data for " + player.field_Private_APIUser_0.displayName + ": " + errorMessage);
					}));
					Action<APIUser> action2 = DelegateSupport.ConvertDelegate<Action<APIUser>>(new Action<APIUser>(delegate(APIUser apiUser)
					{
						nameplateAge.SetPlayerAge(apiUser.date_joined);
						kernelllogger.Msg("Set join date for " + apiUser.displayName + ": " + apiUser.date_joined);
					}));
					APIUser.FetchUser(player.field_Private_APIUser_0.id, action2, action);
					kernelllogger.Msg("Added CustomNameplateAccountAge for " + player.field_Private_APIUser_0.displayName);
				}
				catch (Exception arg2)
				{
					kernelllogger.Error(string.Format("Error adding CustomNameplateAccountAge: {0}", arg2));
				}
				MelonCoroutines.Start(CustomNameplateManager.ApplyNameplateCustomizations(player, true, 10f));
				kernelllogger.Msg("Started nameplate customization for " + player.field_Private_APIUser_0.displayName);
			}
		}
		catch (Exception arg3)
		{
			kernelllogger.Error(string.Format("Error in HandlePlayerJoined: {0}", arg3));
		}
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00005280 File Offset: 0x00003480
	private void HandlePlayerLeft(Player player)
	{
		try
		{
			kernelllogger.Msg("Handling player left: " + player.field_Private_APIUser_0.displayName);
			Il2CppArrayBase<CustomNameplate> il2CppArrayBase = Object.FindObjectsOfType<CustomNameplate>();
			foreach (CustomNameplate customNameplate in il2CppArrayBase)
			{
				bool flag = customNameplate.GetPlayer() == player;
				if (flag)
				{
					kernelllogger.Msg("Destroying CustomNameplate for " + player.field_Private_APIUser_0.displayName);
					customNameplate.CleanupResources();
					Object.Destroy(customNameplate);
				}
			}
			Il2CppArrayBase<CustomNameplateAccountAge> il2CppArrayBase2 = Object.FindObjectsOfType<CustomNameplateAccountAge>();
			foreach (CustomNameplateAccountAge customNameplateAccountAge in il2CppArrayBase2)
			{
				bool flag2 = customNameplateAccountAge.GetPlayer() == player;
				if (flag2)
				{
					kernelllogger.Msg("Destroying CustomNameplateAccountAge for " + player.field_Private_APIUser_0.displayName);
					Object.Destroy(customNameplateAccountAge);
				}
			}
		}
		catch (Exception arg)
		{
			kernelllogger.Error(string.Format("Error in HandlePlayerLeft: {0}", arg));
		}
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000053C4 File Offset: 0x000035C4
	public override void OnPlayerJoined(Player player)
	{
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000053C4 File Offset: 0x000035C4
	public override void OnPlayerLeft(Player player)
	{
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000053C7 File Offset: 0x000035C7
	public override void OnSceneWasLoaded(int buildIndex, string sceneName)
	{
		kernelllogger.Msg(string.Format("Scene loaded: {0} (index: {1}). Resetting player cache.", sceneName, buildIndex));
		this._cachedPlayers.Clear();
	}

	// Token: 0x04000045 RID: 69
	private const string ConfigFileName = "TagsConfig.json";

	// Token: 0x04000046 RID: 70
	private static TagsConfig _config;

	// Token: 0x04000047 RID: 71
	private Dictionary<string, Player> _cachedPlayers = new Dictionary<string, Player>();

	// Token: 0x04000048 RID: 72
	private float _playerCheckTimer = 0f;
}
