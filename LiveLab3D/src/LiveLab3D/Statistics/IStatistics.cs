namespace LiveLab3D.Statistics
{
	using System;

	public interface IStatistics
	{
	}

	public class StatisticsDetail : Attribute
	{
		public string Name { get; set; }
	}
}