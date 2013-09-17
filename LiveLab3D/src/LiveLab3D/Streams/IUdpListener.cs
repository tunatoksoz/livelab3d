namespace LiveLab3D.Streams
{
	public delegate void PacketReceivedEventHandler(string packet);

	public interface IUdpListener
	{
		void Start();
		void Stop();
		event PacketReceivedEventHandler PacketReceived;
	}
}