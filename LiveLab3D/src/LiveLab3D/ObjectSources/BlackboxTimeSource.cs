namespace LiveLab3D.ObjectSources
{
	using System;
	using LiveLab3D.Streams;

	public class BlackboxTimeSource : ITimeSource
	{
		private readonly IUdpListener udpListener;

		private long currentTime;
		private bool started;

		public BlackboxTimeSource(IUdpListener udpListener)
		{
			this.udpListener = udpListener;
			this.udpListener.PacketReceived += PacketReceived;
		}

		#region ITimeSource Members

		public TimeSpan Time
		{
			get { return new TimeSpan(this.currentTime*TimeSpan.TicksPerMillisecond); }
		}


		public bool Started
		{
			get { return this.started; }
		}

		#endregion

		private void PacketReceived(string input)
		{
			this.currentTime = long.Parse(input.Substring(0, input.IndexOf(" ")))*10;
			this.started = true;
		}
	}
}