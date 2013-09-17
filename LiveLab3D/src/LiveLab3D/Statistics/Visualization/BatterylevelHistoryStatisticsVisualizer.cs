namespace LiveLab3D.Statistics.Visualization
{
	using System.Linq;
	using LiveLab3D.Helpers;
	using TomShane.Neoforce.Controls;

	public class BatterylevelHistoryStatisticsVisualizer : IStatisticsVisualizer<BatteryLevelHistoryStatistics>
	{
		private readonly Manager manager;

		public BatterylevelHistoryStatisticsVisualizer(Manager manager)
		{
			this.manager = manager;
		}

		#region IStatisticsVisualizer<BatteryLevelHistoryStatistics> Members

		public void BindValue(Control ctrl, BatteryLevelHistoryStatistics statistics)
		{
			var control = (Plot) ctrl;
			control.Data = statistics.History.Select(h => new Plot.Point(h.Item1.TotalMilliseconds.ToFloat(), h.Item2)).ToArray();
		}

		public Control GetControl()
		{
			return new Plot(this.manager);
		}

		public void BindValue(Control ctrl, IStatistics statistics)
		{
			BindValue(ctrl, (BatteryLevelHistoryStatistics) statistics);
		}

		#endregion
	}
}