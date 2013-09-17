namespace LiveLab3D.Statistics.Visualization
{
	using System;
	using TomShane.Neoforce.Controls;

	public interface IStatisticsVisualizationRegistry
	{
		Control GetControl<TStatistics>() where TStatistics : IStatistics;
		Control GetControl(Type statistics);
		void BindValue<TStatistics>(TStatistics statistics, Control control) where TStatistics : IStatistics;
	}
}