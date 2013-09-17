namespace LiveLab3D.Statistics
{
	using System;

	[StatisticsDetail(Name = "Flight Duration")]
	public class DurationOfFlightStatistics : IPerVehicleStatistics
	{
		public TimeSpan DurationOfFlight { get; set; }
	}
}