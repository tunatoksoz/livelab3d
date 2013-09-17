namespace LiveLab3D.Statistics
{
	[StatisticsDetail(Name = "Flight Distance")]
	public class FlightDistanceStatistics : IPerVehicleStatistics
	{
		public float Distance { get; set; }
	}
}