namespace LiveLab3D.Commands
{
	public class ActivateCommand : VehicleCommandBase
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Delay { get; set; }
		public bool BatterySafetyCheck { get; set; }
		public bool HealthDownload { get; set; }
		public bool CommunicationSafetyCheck { get; set; }
		public bool GyroFeedback { get; set; }
		public bool DebugDataAsHealthDownload { get; set; }
	}
}