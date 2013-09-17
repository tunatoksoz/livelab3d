namespace LiveLab3D.Commands
{
	using Microsoft.Xna.Framework;

	public class FlyToWaypointCommand : VehicleCommandBase
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public Vector3 Position
		{
			get{return new Vector3(X,Y,Z);}
		}
		public Vector2 XYPosition
		{
			get{return new Vector2(X,Y);}
		}
		public int Add { get; set; }
		public float VehicleHeadingDuringTransit { get; set; }
		public float VelocityToWaypoint { get; set; }
		public float TimeToArrival { get; set; }
	}
}