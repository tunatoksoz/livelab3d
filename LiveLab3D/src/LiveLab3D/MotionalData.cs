namespace LiveLab3D
{
	using System;
	using Microsoft.Xna.Framework;

	public class MotionalData : ICloneable
	{
		public MotionalData()
		{
		}

		public MotionalData(float x, float y, float z, float yaw, float pitch, float roll)
		{
			Position = new Vector3(x, y, z);
			Yaw = yaw;
			Roll = roll;
			Pitch = pitch;
		}

		public Vector3 Position { get; set; }

		public float Yaw { get; set; }
		public float Pitch { get; set; }
		public float Roll { get; set; }

		public Vector3 Velocity { get; set; }

		#region ICloneable Members

		public object Clone()
		{
			var newData = new MotionalData
			              	{
			              		Pitch = Pitch,
			              		Position = Position,
			              		Roll = Roll,
			              		Yaw = Yaw,
			              		Velocity = Velocity
			              	};
			return newData;
		}

		#endregion
	}
}