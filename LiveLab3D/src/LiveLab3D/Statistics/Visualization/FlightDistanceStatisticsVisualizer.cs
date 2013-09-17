namespace LiveLab3D.Statistics.Visualization
{
	using TomShane.Neoforce.Controls;

	public class FlightDistanceStatisticsVisualizer : IStatisticsVisualizer<FlightDistanceStatistics>
	{
		private readonly Manager manager;

		public FlightDistanceStatisticsVisualizer(Manager manager)
		{
			this.manager = manager;
		}

		#region IStatisticsVisualizer<FlightDistanceStatistics> Members

		public void BindValue(Control ctrl, FlightDistanceStatistics statistics)
		{
			ctrl.Text = string.Format("{0:0.0} m", statistics.Distance);
		}

		public Control GetControl()
		{
			return new Label(this.manager) {Text = "0.0 m", Name = "StatisticsText"};
		}

		public void BindValue(Control ctrl, IStatistics statistics)
		{
			BindValue(ctrl, (FlightDistanceStatistics) statistics);
		}

		#endregion
	}
}