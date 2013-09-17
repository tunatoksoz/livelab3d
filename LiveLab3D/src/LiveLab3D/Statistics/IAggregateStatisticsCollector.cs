namespace LiveLab3D.Statistics
{
	public interface IAggregateStatisticsCollector<TStatistics> : IStatisticsCollector<TStatistics>
		where TStatistics : IAggregateStatistics
	{
		TStatistics GetStatistics();
	}
}