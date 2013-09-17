namespace LiveLab3D.Streams
{
	using System.IO;
	using System.Threading;

	public class FilePositionListener : IUdpListener
	{
		private readonly StreamReader streamReader;
		private Thread t;

		public FilePositionListener(string fileName)
		{
			this.streamReader = new StreamReader(fileName);
		}

		#region IUdpListener Members

		public virtual void Start()
		{
			string firstLine = this.streamReader.ReadLine();
			string secondLine = this.streamReader.ReadLine();
			this.t = new Thread(delegate()
			                    	{
			                    		while (true)
			                    		{
			                    			PacketReceived(firstLine);
			                    			if (string.IsNullOrEmpty(secondLine))
			                    				break;
			                    			string time = firstLine.Substring(0, firstLine.IndexOf(";"));
			                    			long ttime = long.Parse(time);
			                    			string time2 = secondLine.Substring(0, firstLine.IndexOf(";"));
			                    			long ttime2 = long.Parse(time2);
			                    			Thread.Sleep((int) (ttime2 - ttime)*10);
			                    			firstLine = secondLine;
			                    			secondLine = this.streamReader.ReadLine();
			                    		}
			                    	});
			this.t.Start();
		}

		public void Stop()
		{
			this.t.Abort();
		}

		public event PacketReceivedEventHandler PacketReceived = delegate { };

		#endregion
	}
}