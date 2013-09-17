namespace LiveLab3D.Streams
{
	using System.Net;
	using System.Net.Sockets;
	using System.Text;
	using System.Threading;

	public class UdpListener : IUdpListener
	{
		private readonly int port;
		private Thread listenerThread;

		public UdpListener(int port)
		{
			this.port = port;
		}

		#region IUdpListener Members

		public event PacketReceivedEventHandler PacketReceived = delegate { };

		public virtual void Start()
		{
			this.listenerThread = new Thread(Listen);
			this.listenerThread.Start();
		}

		public void Stop()
		{
			this.listenerThread.Abort();
		}

		#endregion

		protected virtual void FirePacketReceived(string packet)
		{
			PacketReceived(packet);
		}

		protected virtual void Listen()
		{
			var listener = new UdpClient(this.port);
			var groupEP = new IPEndPoint(IPAddress.Any, this.port);
			while (true)
			{
				byte[] data = listener.Receive(ref groupEP);
				string str = Encoding.ASCII.GetString(data);
				FirePacketReceived(str);
			}
		}
	}
}