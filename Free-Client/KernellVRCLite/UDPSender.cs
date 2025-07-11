using System;
using System.Net;
using System.Net.Sockets;

// Token: 0x0200001D RID: 29
public class UDPSender
{
	// Token: 0x17000038 RID: 56
	// (get) Token: 0x06000113 RID: 275 RVA: 0x000070C5 File Offset: 0x000052C5
	public int Port
	{
		get
		{
			return this._port;
		}
	}

	// Token: 0x17000039 RID: 57
	// (get) Token: 0x06000114 RID: 276 RVA: 0x000070CD File Offset: 0x000052CD
	public string Address
	{
		get
		{
			return this._address;
		}
	}

	// Token: 0x06000115 RID: 277 RVA: 0x000070D8 File Offset: 0x000052D8
	public UDPSender(string address, int port)
	{
		this._port = port;
		this._address = address;
		this.sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		IPAddress[] hostAddresses = Dns.GetHostAddresses(address);
		bool flag = hostAddresses.Length == 0;
		if (flag)
		{
			throw new Exception("Unable to find IP address for " + address);
		}
		this.RemoteIpEndPoint = new IPEndPoint(hostAddresses[0], port);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000713B File Offset: 0x0000533B
	public void Send(byte[] message)
	{
		this.sock.SendTo(message, this.RemoteIpEndPoint);
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00007154 File Offset: 0x00005354
	public void Send(OscPacket packet)
	{
		byte[] bytes = packet.GetBytes();
		this.Send(bytes);
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00007171 File Offset: 0x00005371
	public void Close()
	{
		this.sock.Close();
	}

	// Token: 0x04000054 RID: 84
	private int _port;

	// Token: 0x04000055 RID: 85
	private string _address;

	// Token: 0x04000056 RID: 86
	private IPEndPoint RemoteIpEndPoint;

	// Token: 0x04000057 RID: 87
	private Socket sock;
}
