namespace LiveLab3D.Statistics.Visualization
{
	using System;
	using System.Reflection;
	using Castle.Windsor;
	using TomShane.Neoforce.Controls;

	public class StatisticsVisualizationRegistry : IStatisticsVisualizationRegistry
	{
		private readonly MethodInfo methodInfo;
		private readonly IWindsorContainer windsorContainer;

		public StatisticsVisualizationRegistry(IWindsorContainer windsorContainer)
		{
			this.windsorContainer = windsorContainer;
		}

		#region IStatisticsVisualizationRegistry Members

		public Control GetControl<TStatistics>() where TStatistics : IStatistics
		{
			return GetControl(typeof (TStatistics));
		}

		public Control GetControl(Type statisticsType)
		{
			return
				((IStatisticsVisualizer)
				 this.windsorContainer.Resolve(typeof (IStatisticsVisualizer<>).MakeGenericType(statisticsType)))
					.GetControl();
		}

		public void BindValue<TStatistics>(TStatistics statistics, Control control) where TStatistics : IStatistics
		{
			((IStatisticsVisualizer)
			 this.windsorContainer.Resolve(typeof (IStatisticsVisualizer<>).MakeGenericType(statistics.GetType())))
				.BindValue(control, statistics);
		}

		#endregion
	}
}