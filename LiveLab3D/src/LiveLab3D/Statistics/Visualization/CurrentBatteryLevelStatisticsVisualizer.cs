namespace LiveLab3D.Statistics.Visualization
{
	using TomShane.Neoforce.Controls;

	public class CurrentBatteryLevelStatisticsVisualizer : IStatisticsVisualizer<CurrentBatteryLevelStatistics>
	{
		private readonly Manager manager;

		public CurrentBatteryLevelStatisticsVisualizer(Manager manager)
		{
			this.manager = manager;
		}

		#region IStatisticsVisualizer<CurrentBatteryLevelStatistics> Members

		public void BindValue(Control ctrl, CurrentBatteryLevelStatistics statistics)
		{
			((ProgressBar) ctrl).Value = (int) (statistics.Level*100);
		}

		public Control GetControl()
		{
			return new ProgressBar(this.manager)
			       	{
			       		Range = 100,
			       		Value = 1,
			       	};
		}

		public void BindValue(Control ctrl, IStatistics statistics)
		{
			BindValue(ctrl, (CurrentBatteryLevelStatistics) statistics);
		}

		#endregion
	}
}