namespace LiveLab3D.Streams
{
	using System.IO;
	using System.Threading;
	using LiveLab3D.ObjectSources;

	public class FileCommandListener : IUdpListener
	{
		private readonly StreamReader streamReader;
		private readonly ITimeSource timeSource;
		private Thread thread;

		public FileCommandListener(ITimeSource timeSource, string fileName)
		{
			this.timeSource = timeSource;
			this.streamReader = new StreamReader(fileName);
		}

		#region IUdpListener Members

		public virtual void Start()
		{
			this.thread = new Thread(delegate()
			                         	{
			                         		while (!this.timeSource.Started) ;
			                         		while (true)
			                         		{
			                         			string firstLine = this.streamReader.ReadLine();
			                         			if (string.IsNullOrEmpty(firstLine))
			                         				return;
			                         			string time = firstLine.Substring(0, firstLine.IndexOf(" "));
			                         			long ttime = long.Parse(time)*10;
			                         			double timeToSleep = ttime - this.timeSource.Time.TotalMilliseconds;
			                         			if (!(timeToSleep <= 0))
			                         				Thread.Sleep((int) timeToSleep);
			                         			firstLine = firstLine.Substring(firstLine.IndexOf(" ") + 1);
			                         			PacketReceived(firstLine);
			                         		}
			                         	});
			this.thread.Start();
		}

		public void Stop()
		{
			this.thread.Abort();
		}

		public event PacketReceivedEventHandler PacketReceived = delegate { };

		#endregion
	}
}