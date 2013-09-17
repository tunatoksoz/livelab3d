namespace LiveLab3D.Streams
{
	public interface IUdpTransmitter
	{
		void Transmit(byte[] data);
	}
}