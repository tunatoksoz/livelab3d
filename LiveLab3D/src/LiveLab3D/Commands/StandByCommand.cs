namespace LiveLab3D.Commands
{
	public class StandByCommand : VehicleCommandBase
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double ReadyStateDelay { get; set; }
	}
}