namespace LiveLab3D.Events.Impl
{
	using Microsoft.Xna.Framework;

	public class HeadingToWaypointEvent : VehicleTriggeredEventBase
	{
		public Waypoint Waypoint { get; set; }
	}

	public class Waypoint
	{
		public Vector3 Point { get; set; }
	}
}