using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KernelVRC;
using MelonLoader;
using Newtonsoft.Json;
using UnityEngine;

namespace KernellVRCLite.Network
{
	// Token: 0x02000081 RID: 129
	public class KernellSocket : IDisposable
	{
		// Token: 0x1400001F RID: 31
		// (add) Token: 0x06000606 RID: 1542 RVA: 0x00025DB8 File Offset: 0x00023FB8
		// (remove) Token: 0x06000607 RID: 1543 RVA: 0x00025DF0 File Offset: 0x00023FF0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnConnected;

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x06000608 RID: 1544 RVA: 0x00025E28 File Offset: 0x00024028
		// (remove) Token: 0x06000609 RID: 1545 RVA: 0x00025E60 File Offset: 0x00024060
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> OnDisconnected;

		// Token: 0x14000021 RID: 33
		// (add) Token: 0x0600060A RID: 1546 RVA: 0x00025E98 File Offset: 0x00024098
		// (remove) Token: 0x0600060B RID: 1547 RVA: 0x00025ED0 File Offset: 0x000240D0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Exception> OnError;

		// Token: 0x14000022 RID: 34
		// (add) Token: 0x0600060C RID: 1548 RVA: 0x00025F08 File Offset: 0x00024108
		// (remove) Token: 0x0600060D RID: 1549 RVA: 0x00025F40 File Offset: 0x00024140
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> OnMessageReceived;

		// Token: 0x14000023 RID: 35
		// (add) Token: 0x0600060E RID: 1550 RVA: 0x00025F78 File Offset: 0x00024178
		// (remove) Token: 0x0600060F RID: 1551 RVA: 0x00025FB0 File Offset: 0x000241B0
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string> OnDebugLog;

		// Token: 0x14000024 RID: 36
		// (add) Token: 0x06000610 RID: 1552 RVA: 0x00025FE8 File Offset: 0x000241E8
		// (remove) Token: 0x06000611 RID: 1553 RVA: 0x00026020 File Offset: 0x00024220
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<string, bool> OnForceClose;

		// Token: 0x06000612 RID: 1554 RVA: 0x00026058 File Offset: 0x00024258
		public KernellSocket(string serverUrl, Dictionary<string, string> headers = null)
		{
			bool flag = string.IsNullOrEmpty(serverUrl);
			if (flag)
			{
				throw new ArgumentNullException("serverUrl");
			}
			this.ParseUrl(serverUrl);
			this._headers = (headers ?? new Dictionary<string, string>());
			this._connectionTimeout = TimeSpan.FromSeconds(10.0);
			this._pingInterval = TimeSpan.FromSeconds(25.0);
			this._maxReconnectAttempts = 5;
			this._reconnectDelay = TimeSpan.FromSeconds(3.0);
			this._cancellationTokenSource = new CancellationTokenSource();
			this.InitializeSSLWorkaround();
			this.LogDebug("KernellSocket initialized");
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x00026178 File Offset: 0x00024378
		private void ParseUrl(string url)
		{
			try
			{
				Uri uri = new Uri(url);
				this._useSSL = uri.Scheme.Equals("wss", StringComparison.OrdinalIgnoreCase);
				this._host = uri.Host;
				this._port = ((uri.Port != -1) ? uri.Port : (this._useSSL ? 443 : 80));
				this._path = (string.IsNullOrEmpty(uri.PathAndQuery) ? "/" : uri.PathAndQuery);
				this.LogDebug(string.Format("Parsed URL - Host: {0}, Port: {1}, Path: {2}, SSL: {3}", new object[]
				{
					this._host,
					this._port,
					this._path,
					this._useSSL
				}));
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Invalid WebSocket URL: " + ex.Message, "url");
			}
		}

		// Token: 0x06000614 RID: 1556 RVA: 0x00026270 File Offset: 0x00024470
		public void OnWorldChanged(string worldId = null)
		{
			this._worldChangeTime = DateTime.UtcNow;
			this._worldTransitioning = true;
			this._pauseHeartbeat = true;
			this.LogDebug("World changed detected" + ((worldId != null) ? (": " + worldId) : "") + " - pausing network operations");
			this.HandleWorldTransitionAsync();
		}

		// Token: 0x06000615 RID: 1557 RVA: 0x000262D0 File Offset: 0x000244D0
		[DebuggerStepThrough]
		private Task HandleWorldTransitionAsync()
		{
			KernellSocket.<HandleWorldTransitionAsync>d__55 <HandleWorldTransitionAsync>d__ = new KernellSocket.<HandleWorldTransitionAsync>d__55();
			<HandleWorldTransitionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<HandleWorldTransitionAsync>d__.<>4__this = this;
			<HandleWorldTransitionAsync>d__.<>1__state = -1;
			<HandleWorldTransitionAsync>d__.<>t__builder.Start<KernellSocket.<HandleWorldTransitionAsync>d__55>(ref <HandleWorldTransitionAsync>d__);
			return <HandleWorldTransitionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00026314 File Offset: 0x00024514
		private void InitializeSSLWorkaround()
		{
			try
			{
				ServicePointManager.ServerCertificateValidationCallback = ((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true);
				ServicePointManager.SecurityProtocol = (SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12);
				this.LogDebug("SSL configuration completed");
			}
			catch (Exception ex)
			{
				this.LogDebug("SSL configuration warning: " + ex.Message);
			}
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00026390 File Offset: 0x00024590
		[DebuggerStepThrough]
		public Task<bool> ConnectAsync()
		{
			KernellSocket.<ConnectAsync>d__57 <ConnectAsync>d__ = new KernellSocket.<ConnectAsync>d__57();
			<ConnectAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<ConnectAsync>d__.<>4__this = this;
			<ConnectAsync>d__.<>1__state = -1;
			<ConnectAsync>d__.<>t__builder.Start<KernellSocket.<ConnectAsync>d__57>(ref <ConnectAsync>d__);
			return <ConnectAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000618 RID: 1560 RVA: 0x000263D4 File Offset: 0x000245D4
		[DebuggerStepThrough]
		private Task<bool> PerformWebSocketHandshake()
		{
			KernellSocket.<PerformWebSocketHandshake>d__58 <PerformWebSocketHandshake>d__ = new KernellSocket.<PerformWebSocketHandshake>d__58();
			<PerformWebSocketHandshake>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<PerformWebSocketHandshake>d__.<>4__this = this;
			<PerformWebSocketHandshake>d__.<>1__state = -1;
			<PerformWebSocketHandshake>d__.<>t__builder.Start<KernellSocket.<PerformWebSocketHandshake>d__58>(ref <PerformWebSocketHandshake>d__);
			return <PerformWebSocketHandshake>d__.<>t__builder.Task;
		}

		// Token: 0x06000619 RID: 1561 RVA: 0x00026418 File Offset: 0x00024618
		private void StartReceiveTask()
		{
			this._receiveTask = Task.Run(delegate()
			{
				KernellSocket.<<StartReceiveTask>b__59_0>d <<StartReceiveTask>b__59_0>d = new KernellSocket.<<StartReceiveTask>b__59_0>d();
				<<StartReceiveTask>b__59_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<StartReceiveTask>b__59_0>d.<>4__this = this;
				<<StartReceiveTask>b__59_0>d.<>1__state = -1;
				<<StartReceiveTask>b__59_0>d.<>t__builder.Start<KernellSocket.<<StartReceiveTask>b__59_0>d>(ref <<StartReceiveTask>b__59_0>d);
				return <<StartReceiveTask>b__59_0>d.<>t__builder.Task;
			});
		}

		// Token: 0x0600061A RID: 1562 RVA: 0x00026434 File Offset: 0x00024634
		[DebuggerStepThrough]
		private Task<WebSocketFrame> ReadWebSocketFrame()
		{
			KernellSocket.<ReadWebSocketFrame>d__60 <ReadWebSocketFrame>d__ = new KernellSocket.<ReadWebSocketFrame>d__60();
			<ReadWebSocketFrame>d__.<>t__builder = AsyncTaskMethodBuilder<WebSocketFrame>.Create();
			<ReadWebSocketFrame>d__.<>4__this = this;
			<ReadWebSocketFrame>d__.<>1__state = -1;
			<ReadWebSocketFrame>d__.<>t__builder.Start<KernellSocket.<ReadWebSocketFrame>d__60>(ref <ReadWebSocketFrame>d__);
			return <ReadWebSocketFrame>d__.<>t__builder.Task;
		}

		// Token: 0x0600061B RID: 1563 RVA: 0x00026478 File Offset: 0x00024678
		[DebuggerStepThrough]
		private Task<byte[]> ReadExactly(int count)
		{
			KernellSocket.<ReadExactly>d__61 <ReadExactly>d__ = new KernellSocket.<ReadExactly>d__61();
			<ReadExactly>d__.<>t__builder = AsyncTaskMethodBuilder<byte[]>.Create();
			<ReadExactly>d__.<>4__this = this;
			<ReadExactly>d__.count = count;
			<ReadExactly>d__.<>1__state = -1;
			<ReadExactly>d__.<>t__builder.Start<KernellSocket.<ReadExactly>d__61>(ref <ReadExactly>d__);
			return <ReadExactly>d__.<>t__builder.Task;
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x000264C4 File Offset: 0x000246C4
		[DebuggerStepThrough]
		private Task ProcessWebSocketFrame(WebSocketFrame frame)
		{
			KernellSocket.<ProcessWebSocketFrame>d__62 <ProcessWebSocketFrame>d__ = new KernellSocket.<ProcessWebSocketFrame>d__62();
			<ProcessWebSocketFrame>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ProcessWebSocketFrame>d__.<>4__this = this;
			<ProcessWebSocketFrame>d__.frame = frame;
			<ProcessWebSocketFrame>d__.<>1__state = -1;
			<ProcessWebSocketFrame>d__.<>t__builder.Start<KernellSocket.<ProcessWebSocketFrame>d__62>(ref <ProcessWebSocketFrame>d__);
			return <ProcessWebSocketFrame>d__.<>t__builder.Task;
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x00026510 File Offset: 0x00024710
		private void HandleMessage(string message)
		{
			try
			{
				bool flag = message.Contains("\"event\":\"pong\"");
				if (flag)
				{
					this._lastPongTime = DateTime.UtcNow;
					this.LogDebug("Application pong received");
				}
				else
				{
					bool flag2 = message.Contains("\"event\":\"forceClose\"");
					if (flag2)
					{
						try
						{
							this.HandleForceCloseMessage(message);
							return;
						}
						catch (Exception ex)
						{
							this.LogDebug("Force close message parsing error: " + ex.Message);
						}
					}
					bool flag3 = this._messageQueue.Count >= 1000;
					if (flag3)
					{
						string text;
						this._messageQueue.TryDequeue(out text);
						this.LogDebug("Message queue full, dropping oldest");
					}
					this._messageQueue.Enqueue(message);
					Action<string> onMessageReceived = this.OnMessageReceived;
					if (onMessageReceived != null)
					{
						onMessageReceived(message);
					}
				}
			}
			catch (Exception ex2)
			{
				this.LogDebug("Message handling error: " + ex2.Message);
			}
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x00026618 File Offset: 0x00024818
		private void HandleForceCloseMessage(string message)
		{
			try
			{
				string text = "Connection terminated by server";
				bool flag = false;
				int num = message.IndexOf("\"reason\":\"");
				bool flag2 = num != -1;
				if (flag2)
				{
					num += 10;
					int num2 = message.IndexOf("\"", num);
					bool flag3 = num2 != -1;
					if (flag3)
					{
						text = message.Substring(num, num2 - num);
					}
				}
				bool flag4 = message.Contains("\"permanent\":true");
				if (flag4)
				{
					flag = true;
				}
				this.LogDebug(string.Format("Force close received: {0} (Permanent: {1})", text, flag));
				bool flag5 = flag;
				if (flag5)
				{
					this._permanentlyBlocked = true;
					this._shouldReconnect = false;
					this.LogDebug("Reconnection disabled due to permanent block");
				}
				Action<string, bool> onForceClose = this.OnForceClose;
				if (onForceClose != null)
				{
					onForceClose(text, flag);
				}
				this.Disconnect();
				this.LogDebug("FORCE CLOSING GAME due to server request");
				MelonLogger.Error("SERVER REQUESTED SHUTDOWN", new object[]
				{
					text
				});
				Task.Delay(3000).ContinueWith(delegate(Task _)
				{
					try
					{
						Application.Quit();
						Process.GetCurrentProcess().Kill();
					}
					catch (Exception ex2)
					{
						this.LogDebug("Failed to close game: " + ex2.Message);
					}
				});
			}
			catch (Exception ex)
			{
				this.LogDebug("Force close handling error: " + ex.Message);
			}
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00026760 File Offset: 0x00024960
		public string DequeueMessage()
		{
			string result;
			this._messageQueue.TryDequeue(out result);
			return result;
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x00026784 File Offset: 0x00024984
		public int GetQueuedMessageCount()
		{
			return this._messageQueue.Count;
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x000267A4 File Offset: 0x000249A4
		public bool SendMessage(string message)
		{
			bool flag = !this._isConnected || this._stream == null;
			bool result;
			if (flag)
			{
				this.LogDebug("Cannot send: not connected");
				result = false;
			}
			else
			{
				bool worldTransitioning = this._worldTransitioning;
				if (worldTransitioning)
				{
					this.LogDebug("Skipping send: world transitioning");
					result = false;
				}
				else
				{
					try
					{
						byte[] bytes = Encoding.UTF8.GetBytes(message);
						WebSocketFrame frame = this.CreateWebSocketFrame(1, bytes);
						byte[] array = this.SerializeWebSocketFrame(frame);
						this._stream.Write(array, 0, array.Length);
						this._stream.Flush();
						this.LogDebug("Message sent: " + message.Substring(0, Math.Min(50, message.Length)) + "...");
						result = true;
					}
					catch (Exception ex)
					{
						this.LogDebug("Send error: " + ex.Message);
						Action<Exception> onError = this.OnError;
						if (onError != null)
						{
							onError(ex);
						}
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x000268B4 File Offset: 0x00024AB4
		public bool SendJson(object obj)
		{
			bool result;
			try
			{
				string message = JsonConvert.SerializeObject(obj);
				result = this.SendMessage(message);
			}
			catch (Exception ex)
			{
				this.LogDebug("JSON serialization error: " + ex.Message);
				result = false;
			}
			return result;
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00026904 File Offset: 0x00024B04
		public void SendPing()
		{
			try
			{
				bool flag = this._isConnected && this._stream != null && !this._pauseHeartbeat && !this._worldTransitioning;
				if (flag)
				{
					WebSocketFrame frame = this.CreateWebSocketFrame(9, new byte[0]);
					byte[] array = this.SerializeWebSocketFrame(frame);
					this._stream.Write(array, 0, array.Length);
					this._stream.Flush();
					this._lastPingTime = DateTime.UtcNow;
					this.LogDebug("WebSocket ping sent");
					var obj = new
					{
						@event = "ping",
						data = new
						{
							timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
						}
					};
					this.SendJson(obj);
				}
			}
			catch (Exception ex)
			{
				this.LogDebug("Ping error: " + ex.Message);
			}
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x000269E8 File Offset: 0x00024BE8
		[DebuggerStepThrough]
		private Task SendPong(byte[] pingData)
		{
			KernellSocket.<SendPong>d__70 <SendPong>d__ = new KernellSocket.<SendPong>d__70();
			<SendPong>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SendPong>d__.<>4__this = this;
			<SendPong>d__.pingData = pingData;
			<SendPong>d__.<>1__state = -1;
			<SendPong>d__.<>t__builder.Start<KernellSocket.<SendPong>d__70>(ref <SendPong>d__);
			return <SendPong>d__.<>t__builder.Task;
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00026A34 File Offset: 0x00024C34
		private WebSocketFrame CreateWebSocketFrame(byte opcode, byte[] payload)
		{
			return new WebSocketFrame
			{
				IsFinal = true,
				Opcode = opcode,
				PayloadData = (payload ?? new byte[0])
			};
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00026A70 File Offset: 0x00024C70
		private byte[] SerializeWebSocketFrame(WebSocketFrame frame)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				byte value = frame.Opcode | (frame.IsFinal ? 128 : 0);
				memoryStream.WriteByte(value);
				byte[] array = new byte[4];
				using (RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create())
				{
					randomNumberGenerator.GetBytes(array);
				}
				bool flag = frame.PayloadData.Length < 126;
				if (flag)
				{
					byte value2 = (byte)(frame.PayloadData.Length | 128);
					memoryStream.WriteByte(value2);
				}
				else
				{
					bool flag2 = frame.PayloadData.Length <= 65535;
					if (flag2)
					{
						byte value2 = 254;
						memoryStream.WriteByte(value2);
						byte[] bytes = BitConverter.GetBytes((ushort)frame.PayloadData.Length);
						bool isLittleEndian = BitConverter.IsLittleEndian;
						if (isLittleEndian)
						{
							Array.Reverse(bytes);
						}
						memoryStream.Write(bytes, 0, 2);
					}
					else
					{
						byte value2 = byte.MaxValue;
						memoryStream.WriteByte(value2);
						byte[] bytes2 = BitConverter.GetBytes((ulong)((long)frame.PayloadData.Length));
						bool isLittleEndian2 = BitConverter.IsLittleEndian;
						if (isLittleEndian2)
						{
							Array.Reverse(bytes2);
						}
						memoryStream.Write(bytes2, 0, 8);
					}
				}
				memoryStream.Write(array, 0, 4);
				for (int i = 0; i < frame.PayloadData.Length; i++)
				{
					byte value3 = frame.PayloadData[i] ^ array[i % 4];
					memoryStream.WriteByte(value3);
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000627 RID: 1575 RVA: 0x00026C28 File Offset: 0x00024E28
		private void StartHeartbeat()
		{
			this._heartbeatTask = Task.Run(delegate()
			{
				KernellSocket.<<StartHeartbeat>b__73_0>d <<StartHeartbeat>b__73_0>d = new KernellSocket.<<StartHeartbeat>b__73_0>d();
				<<StartHeartbeat>b__73_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<StartHeartbeat>b__73_0>d.<>4__this = this;
				<<StartHeartbeat>b__73_0>d.<>1__state = -1;
				<<StartHeartbeat>b__73_0>d.<>t__builder.Start<KernellSocket.<<StartHeartbeat>b__73_0>d>(ref <<StartHeartbeat>b__73_0>d);
				return <<StartHeartbeat>b__73_0>d.<>t__builder.Task;
			});
		}

		// Token: 0x06000628 RID: 1576 RVA: 0x00026C44 File Offset: 0x00024E44
		private void HandleDisconnection(string reason)
		{
			object connectionLock = this._connectionLock;
			bool isConnected;
			lock (connectionLock)
			{
				isConnected = this._isConnected;
				this._isConnected = false;
			}
			bool flag2 = isConnected;
			if (flag2)
			{
				this.LogDebug("Disconnected: " + reason);
				try
				{
					Stream stream = this._stream;
					if (stream != null)
					{
						stream.Close();
					}
					TcpClient tcpClient = this._tcpClient;
					if (tcpClient != null)
					{
						tcpClient.Close();
					}
				}
				catch (Exception ex)
				{
					this.LogDebug("Cleanup error: " + ex.Message);
				}
				Action<string> onDisconnected = this.OnDisconnected;
				if (onDisconnected != null)
				{
					onDisconnected(reason);
				}
				bool flag3 = this._shouldReconnect && !this._isDisposed && !this._permanentlyBlocked;
				if (flag3)
				{
					this.StartReconnection();
				}
			}
		}

		// Token: 0x06000629 RID: 1577 RVA: 0x00026D48 File Offset: 0x00024F48
		private void StartReconnection()
		{
			bool flag = this._reconnectTask != null && !this._reconnectTask.IsCompleted;
			if (!flag)
			{
				bool permanentlyBlocked = this._permanentlyBlocked;
				if (permanentlyBlocked)
				{
					this.LogDebug("Reconnection blocked - permanently banned");
				}
				else
				{
					this._reconnectTask = Task.Run(delegate()
					{
						KernellSocket.<<StartReconnection>b__75_0>d <<StartReconnection>b__75_0>d = new KernellSocket.<<StartReconnection>b__75_0>d();
						<<StartReconnection>b__75_0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<StartReconnection>b__75_0>d.<>4__this = this;
						<<StartReconnection>b__75_0>d.<>1__state = -1;
						<<StartReconnection>b__75_0>d.<>t__builder.Start<KernellSocket.<<StartReconnection>b__75_0>d>(ref <<StartReconnection>b__75_0>d);
						return <<StartReconnection>b__75_0>d.<>t__builder.Task;
					});
				}
			}
		}

		// Token: 0x0600062A RID: 1578 RVA: 0x00026DA8 File Offset: 0x00024FA8
		public void Disconnect()
		{
			this._shouldReconnect = false;
			object connectionLock = this._connectionLock;
			lock (connectionLock)
			{
				this._isConnected = false;
			}
			try
			{
				bool flag2 = this._stream != null;
				if (flag2)
				{
					WebSocketFrame frame = this.CreateWebSocketFrame(8, new byte[0]);
					byte[] array = this.SerializeWebSocketFrame(frame);
					this._stream.Write(array, 0, array.Length);
					this._stream.Flush();
				}
				Stream stream = this._stream;
				if (stream != null)
				{
					stream.Close();
				}
				TcpClient tcpClient = this._tcpClient;
				if (tcpClient != null)
				{
					tcpClient.Close();
				}
				this.LogDebug("Disconnected gracefully");
			}
			catch (Exception ex)
			{
				this.LogDebug("Disconnect error: " + ex.Message);
			}
		}

		// Token: 0x0600062B RID: 1579 RVA: 0x00026EA0 File Offset: 0x000250A0
		private static ulong SwapBytes(ulong value)
		{
			return (value & 255UL) << 56 | (value & 65280UL) << 40 | (value & 16711680UL) << 24 | (value & (ulong)-16777216) << 8 | (value & 1095216660480UL) >> 8 | (value & 280375465082880UL) >> 24 | (value & 71776119061217280UL) >> 40 | (value & 18374686479671623680UL) >> 56;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00026F1B File Offset: 0x0002511B
		public bool IsConnected
		{
			get
			{
				return this._isConnected;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00026F25 File Offset: 0x00025125
		public bool IsConnecting
		{
			get
			{
				return this._isConnecting;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00026F2F File Offset: 0x0002512F
		public bool IsWorldTransitioning
		{
			get
			{
				return this._worldTransitioning;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00026F39 File Offset: 0x00025139
		public bool IsNetworkPaused
		{
			get
			{
				return this._pauseHeartbeat;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x00026F43 File Offset: 0x00025143
		public bool IsPermanentlyBlocked
		{
			get
			{
				return this._permanentlyBlocked;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x00026F50 File Offset: 0x00025150
		public string ServerUrl
		{
			get
			{
				return string.Format("{0}://{1}:{2}{3}", new object[]
				{
					this._useSSL ? "wss" : "ws",
					this._host,
					this._port,
					this._path
				});
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x00026FA4 File Offset: 0x000251A4
		public int ReconnectAttempts
		{
			get
			{
				return this._reconnectAttempts;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00026FAC File Offset: 0x000251AC
		public DateTime LastPingTime
		{
			get
			{
				return this._lastPingTime;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x00026FB4 File Offset: 0x000251B4
		public DateTime LastPongTime
		{
			get
			{
				return this._lastPongTime;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00026FBC File Offset: 0x000251BC
		public DateTime WorldChangeTime
		{
			get
			{
				return this._worldChangeTime;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x00026FC4 File Offset: 0x000251C4
		public NetworkStatus GetStatus()
		{
			return new NetworkStatus
			{
				IsConnected = this._isConnected,
				IsConnecting = this._isConnecting,
				IsWorldTransitioning = this._worldTransitioning,
				IsNetworkPaused = this._pauseHeartbeat,
				ReconnectAttempts = this._reconnectAttempts,
				LastPingTime = this._lastPingTime,
				LastPongTime = this._lastPongTime,
				WorldChangeTime = this._worldChangeTime,
				QueuedMessages = this.GetQueuedMessageCount(),
				ServerUrl = this.ServerUrl
			};
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x00027068 File Offset: 0x00025268
		private void LogDebug(string message)
		{
			try
			{
				string text = "[KernellSocket] " + message;
				Action<string> onDebugLog = this.OnDebugLog;
				if (onDebugLog != null)
				{
					onDebugLog(text);
				}
				kernelllogger.Msg(text);
			}
			catch
			{
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000270B8 File Offset: 0x000252B8
		public void Dispose()
		{
			bool isDisposed = this._isDisposed;
			if (!isDisposed)
			{
				this._isDisposed = true;
				this._shouldReconnect = false;
				this.LogDebug("Disposing KernellSocket");
				try
				{
					CancellationTokenSource cancellationTokenSource = this._cancellationTokenSource;
					if (cancellationTokenSource != null)
					{
						cancellationTokenSource.Cancel();
					}
					this.Disconnect();
					try
					{
						Task heartbeatTask = this._heartbeatTask;
						if (heartbeatTask != null)
						{
							heartbeatTask.Wait(1000);
						}
						Task reconnectTask = this._reconnectTask;
						if (reconnectTask != null)
						{
							reconnectTask.Wait(1000);
						}
						Task receiveTask = this._receiveTask;
						if (receiveTask != null)
						{
							receiveTask.Wait(1000);
						}
						Task worldTransitionTask = this._worldTransitionTask;
						if (worldTransitionTask != null)
						{
							worldTransitionTask.Wait(1000);
						}
					}
					catch (Exception ex)
					{
						this.LogDebug("Task cleanup warning: " + ex.Message);
					}
					Stream stream = this._stream;
					if (stream != null)
					{
						stream.Dispose();
					}
					TcpClient tcpClient = this._tcpClient;
					if (tcpClient != null)
					{
						tcpClient.Close();
					}
					CancellationTokenSource cancellationTokenSource2 = this._cancellationTokenSource;
					if (cancellationTokenSource2 != null)
					{
						cancellationTokenSource2.Dispose();
					}
					string text;
					while (this._messageQueue.TryDequeue(out text))
					{
					}
					this.LogDebug("KernellSocket disposed");
				}
				catch (Exception ex2)
				{
					this.LogDebug("Disposal error: " + ex2.Message);
				}
			}
		}

		// Token: 0x040002B9 RID: 697
		private string _host;

		// Token: 0x040002BA RID: 698
		private int _port;

		// Token: 0x040002BB RID: 699
		private string _path;

		// Token: 0x040002BC RID: 700
		private bool _useSSL;

		// Token: 0x040002BD RID: 701
		private readonly Dictionary<string, string> _headers;

		// Token: 0x040002BE RID: 702
		private readonly TimeSpan _connectionTimeout;

		// Token: 0x040002BF RID: 703
		private readonly TimeSpan _pingInterval;

		// Token: 0x040002C0 RID: 704
		private readonly int _maxReconnectAttempts;

		// Token: 0x040002C1 RID: 705
		private readonly TimeSpan _reconnectDelay;

		// Token: 0x040002C2 RID: 706
		private volatile bool _isConnected = false;

		// Token: 0x040002C3 RID: 707
		private volatile bool _isConnecting = false;

		// Token: 0x040002C4 RID: 708
		private volatile bool _isDisposed = false;

		// Token: 0x040002C5 RID: 709
		private volatile bool _shouldReconnect = true;

		// Token: 0x040002C6 RID: 710
		private volatile bool _pauseHeartbeat = false;

		// Token: 0x040002C7 RID: 711
		private volatile bool _worldTransitioning = false;

		// Token: 0x040002C8 RID: 712
		private volatile bool _permanentlyBlocked = false;

		// Token: 0x040002C9 RID: 713
		private int _reconnectAttempts = 0;

		// Token: 0x040002CA RID: 714
		private DateTime _lastPingTime = DateTime.MinValue;

		// Token: 0x040002CB RID: 715
		private DateTime _lastPongTime = DateTime.MinValue;

		// Token: 0x040002CC RID: 716
		private DateTime _worldChangeTime = DateTime.MinValue;

		// Token: 0x040002CD RID: 717
		private TcpClient _tcpClient;

		// Token: 0x040002CE RID: 718
		private Stream _stream;

		// Token: 0x040002CF RID: 719
		private readonly object _connectionLock = new object();

		// Token: 0x040002D0 RID: 720
		private readonly ConcurrentQueue<string> _messageQueue = new ConcurrentQueue<string>();

		// Token: 0x040002D1 RID: 721
		private CancellationTokenSource _cancellationTokenSource;

		// Token: 0x040002D2 RID: 722
		private Task _reconnectTask;

		// Token: 0x040002D3 RID: 723
		private Task _heartbeatTask;

		// Token: 0x040002D4 RID: 724
		private Task _receiveTask;

		// Token: 0x040002D5 RID: 725
		private Task _worldTransitionTask;

		// Token: 0x040002D6 RID: 726
		private const int PING_TIMEOUT_SECONDS = 30;

		// Token: 0x040002D7 RID: 727
		private const int MESSAGE_QUEUE_LIMIT = 1000;

		// Token: 0x040002D8 RID: 728
		private const int BUFFER_SIZE = 8192;

		// Token: 0x040002D9 RID: 729
		private const string WEBSOCKET_GUID = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";

		// Token: 0x040002DA RID: 730
		private const int WORLD_TRANSITION_DELAY_MS = 8000;
	}
}
