namespace LiveLab3D.Commands
{
	public class PrepareToLandCommand : VehicleCommandBase
	{
		public int ReturnToBase { get; set; }
		public double LandingLocationX { get; set; }
		public double LandingLocationZ { get; set; }
		public double LandingLocationY { get; set; }
		public double ApproachHeading { get; set; }
		public double Velocity { get; set; }
	}
}