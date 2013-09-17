namespace LiveLab3D.Statistics.Visualization
{
	using System;
	using TomShane.Neoforce.Controls;

	public class DurationOfFlightStatisticsVisualizer : IStatisticsVisualizer<DurationOfFlightStatistics>
	{
		private readonly Manager manager;

		public DurationOfFlightStatisticsVisualizer(Manager manager)
		{
			this.manager = manager;
		}

		#region IStatisticsVisualizer<DurationOfFlightStatistics> Members

		public void BindValue(Control ctrl, DurationOfFlightStatistics statistics)
		{
			ctrl.Text = String.Format("{0:0.0} s", statistics.DurationOfFlight.TotalSeconds);
		}

		public Control GetControl()
		{
			return new Label(this.manager)
			       	{
			       		Name = "StatisticsText",
			       		Text = "0.0 s"
			       	};
		}

		public void BindValue(Control ctrl, IStatistics statistics)
		{
			BindValue(ctrl, (DurationOfFlightStatistics) statistics);
		}

		#endregion
	}
}