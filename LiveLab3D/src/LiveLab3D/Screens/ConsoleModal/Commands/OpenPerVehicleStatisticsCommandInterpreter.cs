namespace LiveLab3D.Screens.ConsoleModal.Commands
{
	using Castle.Windsor;
	using LiveLab3D.Statistics.Visualization;
	using TomShane.Neoforce.Controls;

	public class OpenPerVehicleStatisticsCommandInterpreter : IConsoleCommandInterpreter
	{
		private readonly IWindsorContainer container;
		private readonly Manager manager;

		public OpenPerVehicleStatisticsCommandInterpreter(Manager manager, IWindsorContainer container)
		{
			this.manager = manager;
			this.container = container;
		}

		#region IConsoleCommandInterpreter Members

		public bool CanInterpret(string text)
		{
			return !string.IsNullOrEmpty(text) && text.StartsWith("vehicle statistics");
		}

		public void Interpret(string text)
		{
			var perVehicleStatistics = this.container.Resolve<PerVehicleStatisticsWindow>();
			perVehicleStatistics.Left = this.manager.ScreenWidth/2;
			perVehicleStatistics.Top = this.manager.ScreenHeight/2;
			perVehicleStatistics.Width = 400;
			perVehicleStatistics.Height = 300;
			this.manager.Add(perVehicleStatistics);
			perVehicleStatistics.Init();
		}

		#endregion
	}
}