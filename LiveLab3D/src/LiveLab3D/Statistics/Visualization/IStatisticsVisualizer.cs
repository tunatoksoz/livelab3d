namespace LiveLab3D.Statistics.Visualization
{
	using TomShane.Neoforce.Controls;

	public interface IStatisticsVisualizer<TStatistics> : IStatisticsVisualizer where TStatistics : IStatistics
	{
		void BindValue(Control ctrl, TStatistics statistics);
	}

	public interface IStatisticsVisualizer
	{
		Control GetControl();
		void BindValue(Control ctrl, IStatistics statistics);
	}
}