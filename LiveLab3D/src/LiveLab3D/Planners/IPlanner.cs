namespace LiveLab3D.Planners
{
	using System;

	public interface IPlanner
	{
		void Start();
		void Stop();
		void SimulateMotion(TimeSpan timeSpan);
	}
}
