namespace LiveLab3D.Simulation
{
	using System;
	using LiveLab3D.ObjectSources;

	public class SimulationTimeSource : ITimeSource
	{
		private readonly LiveLab game;
		private TimeSpan timeSpan;

		public SimulationTimeSource(LiveLab game)
		{
			this.game = game;
			this.timeSpan = new TimeSpan();
			this.game.GameUpdated += (timeSpan) =>
			                         this.timeSpan = timeSpan.Add(timeSpan);
		}

		#region ITimeSource Members

		public TimeSpan Time
		{
			get { return this.timeSpan; }
		}

		public bool Started
		{
			get { return true; }
		}

		#endregion
	}
}