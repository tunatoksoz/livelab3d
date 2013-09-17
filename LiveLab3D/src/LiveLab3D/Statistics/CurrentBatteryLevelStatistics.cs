namespace LiveLab3D.Statistics
{
	[StatisticsDetail(Name = "Battery Level")]
	public class CurrentBatteryLevelStatistics : IPerVehicleStatistics
	{
		public float Level { get; set; }
	}
}