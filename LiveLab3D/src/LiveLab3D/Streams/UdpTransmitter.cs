namespace LiveLab3D.Streams
{
	using System.Net;
	using System.Net.Sockets;

	public class UdpTransmitter : IUdpTransmitter
	{
		private readonly IPEndPoint groupEP;
		private readonly int port;
		private readonly UdpClient udpClient;

		public UdpTransmitter(int sourcePort, int destinationPort)
		{
			this.port = this.port;
			this.udpClient = new UdpClient(sourcePort);
			this.groupEP = new IPEndPoint(IPAddress.Broadcast, destinationPort);
		}

		#region IUdpTransmitter Members

		public void Transmit(byte[] data)
		{
			this.udpClient.Send(data, data.Length, this.groupEP);
		}

		#endregion
	}
}