namespace LiveLab3D.Commands
{
	public class TakeOffCommand : VehicleCommandBase
	{
		public double X { get; set; }
		public double Y { get; set; }
		public double TakeOffDelay { get; set; }
		public double TakeOffLocationX { get; set; }
		public double TakeOffLocationY { get; set; }
	}
}