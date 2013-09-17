namespace LiveLab3D.Streams
{
	using System.IO;
	using System.Timers;
	using LiveLab3D.ObjectSources;

	public class SavingCommandListenerDecorator : IUdpListener
	{
		private readonly IUdpListener innerListener;
		private readonly object lockObject = new object();
		private readonly StreamWriter streamWriter;
		private readonly ITimeSource timeSource;
		private readonly Timer timer;

		public SavingCommandListenerDecorator(string fileName, ITimeSource timeSource, IUdpListener innerListener)
		{
			if (File.Exists(fileName))
				File.Delete(fileName);
			this.timeSource = timeSource;
			this.innerListener = innerListener;

			innerListener.PacketReceived += SavePacket;
			var fileStream = new FileStream(fileName, FileMode.OpenOrCreate);
			this.streamWriter = new StreamWriter(fileStream);
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
			if (this.timeSource.Time.TotalMilliseconds == 0)
				return; //Drop the packet if there is no position information available.


			string s = string.Format("{0} {1}", this.timeSource.Time.TotalMilliseconds/10, packet);
			lock (this.lockObject)
			{
				this.streamWriter.WriteLine(s);
			}
		}
	}
}