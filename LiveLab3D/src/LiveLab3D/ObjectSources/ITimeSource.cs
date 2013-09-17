namespace LiveLab3D.ObjectSources
{
	using System;

	public interface ITimeSource
	{
		TimeSpan Time { get; }
		bool Started { get; }
	}
}