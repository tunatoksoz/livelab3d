namespace LiveLab3D.Commands
{
	public class VehicleReadyCommand : VehicleCommandBase
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double Z { get; set; }
		public double Heading { get; set; }
	}
}