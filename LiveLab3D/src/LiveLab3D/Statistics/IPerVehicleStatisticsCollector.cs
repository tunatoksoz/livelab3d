namespace LiveLab3D.Statistics
{
	using LiveLab3D.Objects;

	public interface IPerVehicleStatisticsCollector : IStatisticsCollector
	{
		IPerVehicleStatistics GetStatisticsForVehicle(string name);
		IPerVehicleStatistics GetStatisticsForVehicle(int id);
		IPerVehicleStatistics GetStatisticsForVehicle(ObjectBase objectBase);
	}

	public interface IPerVehicleStatisticsCollector<TStatistics> :
		IPerVehicleStatisticsCollector,
		IStatisticsCollector<TStatistics>
		where TStatistics : IPerVehicleStatistics
	{
		new TStatistics GetStatisticsForVehicle(string name);
		new TStatistics GetStatisticsForVehicle(int id);
		new TStatistics GetStatisticsForVehicle(ObjectBase objectBase);
	}
}