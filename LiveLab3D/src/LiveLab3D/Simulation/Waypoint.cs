namespace LiveLab3D.Simulation
{
	using Microsoft.Xna.Framework;

	public class Waypoint
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public Vector3 Position
		{
			get
			{
				return new Vector3(X,Y,Z);
			}
		}

		public float Psi { get; set; }
		public float Vel { get; set; }

		public float V_cmd { get; set; }
		public float L1 { get; set; }
		public float Dir { get; set; }
	}
}