namespace LiveLab3D.Commands
{
	public class LandNowCommand : VehicleCommandBase
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float ApproachAltitude { get; set; }
		public float ApproachHeading { get; set; }
		public float ApproachVelocity { get; set; }
		public float HoverTime { get; set; }
		public float PadOffset { get; set; }
	}
}