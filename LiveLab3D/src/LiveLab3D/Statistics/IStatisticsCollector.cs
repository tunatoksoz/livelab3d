namespace LiveLab3D.Statistics
{
	public interface IStatisticsCollector<TStatistics> : IStatisticsCollector
		where TStatistics : IStatistics
	{
	}

	public interface IStatisticsCollector
	{
		void Start();
		void Stop();
	}
}