namespace LiveLab3D.Streams
{
	using System.IO;
	using System.Timers;

	public class SavingPositionListenerDecorator : IUdpListener
	{
		private readonly IUdpListener innerListener;
		private readonly object lockObject;
		private readonly StreamWriter streamWriter;
		private readonly Timer timer;

		public SavingPositionListenerDecorator(string fileName, IUdpListener innerListener)
		{
			if (File.Exists(fileName))
				File.Delete(fileName);
			this.innerListener = innerListener;
			var fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
			this.streamWriter = new StreamWriter(fileStream);
			this.innerListener.PacketReceived += SavePacket;
			this.lockObject = new object();
			this.timer = new Timer(5000);
			this.timer.Elapsed += FlushWriter;
		}

		#region IUdpListener Members

		public void Start()
		{
			this.innerListener.Start();
		}

		public void Stop()
		{
			this.innerListener.Stop();
		}

		public event PacketReceivedEventHandler PacketReceived
		{
			add { this.innerListener.PacketReceived += value; }
			remove { this.innerListener.PacketReceived -= value; }
		}

		#endregion

		private void FlushWriter(object sender, ElapsedEventArgs e)
		{
			lock (this.lockObject)
			{
				this.streamWriter.Flush();
			}
		}

		private void SavePacket(string packet)
		{
			lock (this.lockObject)
			{
				this.streamWriter.WriteLine(packet);
			}
		}
	}
}