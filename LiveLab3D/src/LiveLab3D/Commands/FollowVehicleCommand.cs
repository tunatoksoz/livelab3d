namespace LiveLab3D.Commands
{
	public class FollowVehicleCommand:VehicleCommandBase
	{
		public int AddRevise { get; set; }
		public int TargetId { get; set; }
		public float VehicleOffsetX { get; set; }
		public float VehicleOffsetY { get; set; }
		public float VehicleOffsetZ { get; set; }
		public float FollowingTime { get; set; }
	}
}
