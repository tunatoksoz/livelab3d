namespace LiveLab3D.Statistics
{
	using System;
	using System.Collections.Generic;

	[StatisticsDetail(Name = "Battery History")]
	public class BatteryLevelHistoryStatistics : IPerVehicleStatistics
	{
		public IEnumerable<Pair<TimeSpan, float>> History { get; set; }
	}
}