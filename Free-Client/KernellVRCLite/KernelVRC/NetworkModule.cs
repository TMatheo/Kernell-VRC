using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ExitGames.Client.Photon;
using Il2CppSystem.Diagnostics.Tracing;
using VRC;
using VRC.Core;

namespace KernelVRC
{
	// Token: 0x020000B2 RID: 178
	public sealed class NetworkModule : KernelModuleBase
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06000935 RID: 2357 RVA: 0x00038D4B File Offset: 0x00036F4B
		public override string ModuleName
		{
			get
			{
				return "Network Module";
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x06000937 RID: 2359 RVA: 0x00038D52 File Offset: 0x00036F52
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.PlayerEvents | ModuleCapabilities.NetworkEvents;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x00038D56 File Offset: 0x00036F56
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every10Frames;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0003498D File Offset: 0x00032B8D
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.High;
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00038D5A File Offset: 0x00036F5A
		public override void OnInitialize()
		{
			this._initialized = true;
			base.Log("Network module initialized");
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00038D70 File Offset: 0x00036F70
		public override void OnShutdown()
		{
			this._playerJoinTicks.Clear();
			this._eventCount = 0;
			this._eventWriteIndex = 0;
			this._eventReadIndex = 0;
			base.OnShutdown();
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00038D9C File Offset: 0x00036F9C
		public override void OnUpdate()
		{
			bool flag = !this._monitoringEnabled || this._eventCount == 0;
			if (!flag)
			{
				int num = 0;
				while (this._eventCount > 0 && num < 64)
				{
					NetworkModule.NetworkEvent networkEvent = this._eventBuffer[this._eventReadIndex];
					this._eventReadIndex = (this._eventReadIndex + 1 & 1023);
					this._eventCount--;
					this.ProcessEventFast(ref networkEvent);
					num++;
				}
				this._processedEvents += num;
				bool flag2 = (this._processedEvents & 1023) == 0 && this._processedEvents > 0;
				if (flag2)
				{
					base.Log(string.Format("Processed {0} events", this._processedEvents));
				}
			}
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x000053C4 File Offset: 0x000035C4
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ProcessEventFast(ref NetworkModule.NetworkEvent evt)
		{
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00038E70 File Offset: 0x00037070
		public override bool OnEventPatch(EventSource.EventData eventData)
		{
			bool flag = !this._monitoringEnabled;
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = this._eventCount < 1024;
				if (flag2)
				{
					this._eventBuffer[this._eventWriteIndex] = new NetworkModule.NetworkEvent
					{
						Timestamp = DateTime.UtcNow.Ticks
					};
					this._eventWriteIndex = (this._eventWriteIndex + 1 & 1023);
					this._eventCount++;
				}
				result = true;
			}
			return result;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00038EF8 File Offset: 0x000370F8
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override bool OnEventSent(byte eventCode, object eventData, RaiseEventOptions options, SendOptions sendOptions)
		{
			return this._monitoringEnabled && eventCode != byte.MaxValue;
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00038F20 File Offset: 0x00037120
		public override void OnPlayerJoined(Player player)
		{
			bool flag = !this._monitoringEnabled;
			if (!flag)
			{
				string text;
				if (player == null)
				{
					text = null;
				}
				else
				{
					APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
					text = ((field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null);
				}
				string text2 = text;
				bool flag2 = string.IsNullOrEmpty(text2);
				if (!flag2)
				{
					long ticks = DateTime.UtcNow.Ticks;
					long num;
					bool flag3 = this._playerJoinTicks.TryGetValue(text2, out num);
					if (flag3)
					{
						bool flag4 = ticks - num < 600000000L;
						if (flag4)
						{
							base.LogWarning("Rapid rejoin: " + text2);
						}
					}
					this._playerJoinTicks[text2] = ticks;
					bool flag5 = this._playerJoinTicks.Count > 200;
					if (flag5)
					{
						this.CleanupOldEntriesFast();
					}
				}
			}
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00038FE0 File Offset: 0x000371E0
		public override void OnPlayerLeft(Player player)
		{
			string text;
			if (player == null)
			{
				text = null;
			}
			else
			{
				APIUser field_Private_APIUser_ = player.field_Private_APIUser_0;
				text = ((field_Private_APIUser_ != null) ? field_Private_APIUser_.id : null);
			}
			string text2 = text;
			bool flag = !string.IsNullOrEmpty(text2);
			if (flag)
			{
				this._playerJoinTicks.Remove(text2);
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00039024 File Offset: 0x00037224
		private void CleanupOldEntriesFast()
		{
			long num = DateTime.UtcNow.Ticks - 36000000000L;
			List<string> list = new List<string>(32);
			foreach (KeyValuePair<string, long> keyValuePair in this._playerJoinTicks)
			{
				bool flag = keyValuePair.Value < num;
				if (flag)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (string key in list)
			{
				this._playerJoinTicks.Remove(key);
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00039100 File Offset: 0x00037300
		public NetworkModule()
		{
			HashSet<byte> hashSet = new HashSet<byte>();
			hashSet.Add(250);
			hashSet.Add(251);
			hashSet.Add(252);
			this._blockedEventCodes = hashSet;
			this._eventBuffer = new NetworkModule.NetworkEvent[1024];
			this._eventWriteIndex = 0;
			this._eventReadIndex = 0;
			this._eventCount = 0;
			this._monitoringEnabled = true;
			this._processedEvents = 0;
			base..ctor();
		}

		// Token: 0x04000493 RID: 1171
		private readonly Dictionary<string, long> _playerJoinTicks = new Dictionary<string, long>(128);

		// Token: 0x04000494 RID: 1172
		private readonly HashSet<byte> _blockedEventCodes;

		// Token: 0x04000495 RID: 1173
		private readonly NetworkModule.NetworkEvent[] _eventBuffer;

		// Token: 0x04000496 RID: 1174
		private int _eventWriteIndex;

		// Token: 0x04000497 RID: 1175
		private int _eventReadIndex;

		// Token: 0x04000498 RID: 1176
		private int _eventCount;

		// Token: 0x04000499 RID: 1177
		private bool _monitoringEnabled;

		// Token: 0x0400049A RID: 1178
		private int _processedEvents;

		// Token: 0x0400049B RID: 1179
		private const int BATCH_SIZE = 64;

		// Token: 0x0400049C RID: 1180
		private const long CLEANUP_INTERVAL_TICKS = 36000000000L;

		// Token: 0x02000191 RID: 401
		private struct NetworkEvent
		{
			// Token: 0x0400095D RID: 2397
			public byte Code;

			// Token: 0x0400095E RID: 2398
			public long Timestamp;
		}
	}
}
