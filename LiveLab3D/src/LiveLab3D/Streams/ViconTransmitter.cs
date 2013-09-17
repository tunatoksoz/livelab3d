namespace LiveLab3D.Streams
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using System.Threading;
	using System.Timers;
	using LiveLab3D.Helpers;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using Microsoft.Xna.Framework;
	using Timer = System.Timers.Timer;

	public class ViconTransmitter
	{
		private readonly IObjectSource objectSource;
		private readonly ITimeSource timeSource;
		private readonly Timer timer;
		private readonly IUdpTransmitter transmitter;

		public ViconTransmitter(int interval, IObjectSource objectSource, ITimeSource timeSource,
		                        IUdpTransmitter transmitter)
		{
			this.objectSource = objectSource;
			this.transmitter = transmitter;
			this.timeSource = timeSource;
			this.timer = new Timer(interval);
			this.timer.Elapsed += OnTimerElapsed;
		}

		public void StartTransmission()
		{
			var t = new Thread(new ThreadStart(delegate
			                                   	{
			                                   		Thread.Sleep(1000);
			                                   		this.timer.Start();
			                                   	}));
			t.Start();
		}

		private void OnTimerElapsed(object sender, ElapsedEventArgs e)
		{
			IEnumerable<ObjectBase> items = this.objectSource.GetObjects();
			var sb = new StringBuilder();
			sb.Append(this.timeSource.Time.TotalMilliseconds).Append(";");


			foreach (ObjectBase objectBase in items)
				sb.Append(GetString(objectBase)).Append(";");

			this.transmitter.Transmit(Encoding.ASCII.GetBytes(sb.ToString()));
		}

		private StringBuilder GetString(ObjectBase objectBase)
		{
			float troll = objectBase.PositionalData.Roll;
			float tpitch = objectBase.PositionalData.Pitch;
			float tyaw = objectBase.PositionalData.Yaw;

			float yaw, pitch, roll;
			yaw = tyaw;
			roll = troll;
			pitch = tpitch;

			Quaternion result;
			result.X = ((Math.Cos(roll*0.5f).ToFloat()*Math.Cos((pitch*0.5f)).ToFloat())*(float) Math.Cos((yaw*0.5f))) +
			           ((Math.Sin(roll*0.5f).ToFloat()*Math.Sin(pitch*0.5f).ToFloat())*(float) Math.Sin(yaw*0.5f));
			result.Y = ((Math.Sin(roll*0.5f).ToFloat()*Math.Cos((pitch*0.5f)).ToFloat())*(float) Math.Cos((yaw*0.5f))) -
			           ((Math.Cos(roll*0.5f).ToFloat()*Math.Sin(pitch*0.5f).ToFloat())*(float) Math.Sin(yaw*0.5f));
			result.Z = ((Math.Cos(roll*0.5f).ToFloat()*Math.Sin((pitch*0.5f)).ToFloat())*(float) Math.Cos((yaw*0.5f))) +
			           ((Math.Sin(roll*0.5f).ToFloat()*Math.Cos(pitch*0.5f).ToFloat())*(float) Math.Sin(yaw*0.5f));
			result.W = ((Math.Cos(roll*0.5f).ToFloat()*Math.Cos((pitch*0.5f)).ToFloat())*(float) Math.Sin((yaw*0.5f))) -
			           ((Math.Sin(roll*0.5f).ToFloat()*Math.Sin(pitch*0.5f).ToFloat())*(float) Math.Cos(yaw*0.5f));


			Quaternion q = result;

			return new StringBuilder().Append(objectBase.Name).Append(" ")
				.Append(objectBase.Id).Append(",")
				.Append(objectBase.PositionalData.Position.X.ToFormattedFloat()).Append(",")
				.Append(objectBase.PositionalData.Position.Y.ToFormattedFloat()).Append(",")
				.Append((objectBase.PositionalData.Position.Z + 0.1f).ToFormattedFloat()).Append(",")
				.Append(roll.ToFormattedFloat()).Append(",")
				.Append(pitch.ToFormattedFloat()).Append(",")
				.Append(yaw.ToFormattedFloat()).Append(",")
				.Append(0.0001f.ToFormattedFloat()).Append(",")
				.Append(0.0001f.ToFormattedFloat()).Append(",")
				.Append(0.0001f.ToFormattedFloat()).Append(",")
				.Append(0.0001f.ToFormattedFloat()).Append(",")
				.Append(0.0001f.ToFormattedFloat()).Append(",")
				.Append(0.0001f.ToFormattedFloat()).Append(",")
				.Append(q.W.ToFormattedFloat()).Append(",")
				.Append(q.X.ToFormattedFloat()).Append(",")
				.Append(q.Y.ToFormattedFloat()).Append(",")
				.Append(q.Z.ToFormattedFloat());
		}
	}
}