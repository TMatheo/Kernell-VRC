using System;
using ExitGames.Client.Photon;
using Il2CppSystem.Diagnostics.Tracing;
using UnityEngine;
using VRC;
using VRC.Core;
using VRC.Udon;

namespace KernelVRC
{
	// Token: 0x020000B0 RID: 176
	public interface IKernelModule
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060008F8 RID: 2296
		string ModuleName { get; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060008F9 RID: 2297
		string Version { get; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060008FA RID: 2298
		string Author { get; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060008FB RID: 2299
		// (set) Token: 0x060008FC RID: 2300
		bool IsEnabled { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060008FD RID: 2301
		ModuleCapabilities Capabilities { get; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060008FE RID: 2302
		UpdateFrequency UpdateFrequency { get; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060008FF RID: 2303
		ModulePriority Priority { get; }

		// Token: 0x06000900 RID: 2304
		void OnApplicationStart();

		// Token: 0x06000901 RID: 2305
		void OnInitialize();

		// Token: 0x06000902 RID: 2306
		void OnShutdown();

		// Token: 0x06000903 RID: 2307
		void OnUserLoggedIn(APIUser user);

		// Token: 0x06000904 RID: 2308
		void OnUiManagerInit();

		// Token: 0x06000905 RID: 2309
		void OnSceneWasLoaded(int buildIndex, string sceneName);

		// Token: 0x06000906 RID: 2310
		void OnUpdate();

		// Token: 0x06000907 RID: 2311
		void OnLateUpdate();

		// Token: 0x06000908 RID: 2312
		void OnGUI();

		// Token: 0x06000909 RID: 2313
		void OnPlayerJoined(Player player);

		// Token: 0x0600090A RID: 2314
		void OnPlayerLeft(Player player);

		// Token: 0x0600090B RID: 2315
		void OnEnterWorld(ApiWorld world, ApiWorldInstance instance);

		// Token: 0x0600090C RID: 2316
		void OnLeaveWorld();

		// Token: 0x0600090D RID: 2317
		void OnAvatarChanged(Player player, GameObject avatar);

		// Token: 0x0600090E RID: 2318
		bool OnEventPatch(EventSource.EventData eventData);

		// Token: 0x0600090F RID: 2319
		bool OnEventPatchVRC(ref EventSource.EventData eventData);

		// Token: 0x06000910 RID: 2320
		bool OnEventSent(byte eventCode, object eventData, RaiseEventOptions options, SendOptions sendOptions);

		// Token: 0x06000911 RID: 2321
		bool OnUdonPatch(UdonBehaviour instance, string program);

		// Token: 0x06000912 RID: 2322
		void OnMenuOpened();

		// Token: 0x06000913 RID: 2323
		void OnMenuClosed();
	}
}
