namespace LiveLab3D.Visual.Cameras
{
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;

	public class VehicleHeadCamera : ICamera
	{
		private readonly ObjectBase vehicle;

		private Vector3 position;
		private Vector3 target;

		public VehicleHeadCamera(ObjectBase vehicle)
		{
			this.vehicle = vehicle;
		}

		#region ICamera Members

		public void UpdateCamera(GameTime gameTime)
		{
		}

		public CameraOrientation GetCameraOrientation()
		{
			float yaw = this.vehicle.PositionalData.Yaw;
			float pitch = this.vehicle.PositionalData.Pitch;
			float roll = this.vehicle.PositionalData.Roll;

			Vector3 pos = this.vehicle.PositionalData.Position;


			Vector3 x = Vector3.UnitX;
			Vector3 y = Vector3.UnitY;
			Vector3 z = Vector3.UnitZ;

			x = Vector3.Transform(x, Matrix.CreateFromAxisAngle(z, -yaw));
			y = Vector3.Transform(y, Matrix.CreateFromAxisAngle(z, -yaw));

			y = Vector3.Transform(y, Matrix.CreateFromAxisAngle(x, pitch));
			z = Vector3.Transform(z, Matrix.CreateFromAxisAngle(x, pitch));


			z = Vector3.Transform(z, Matrix.CreateFromAxisAngle(y, roll));
			x = Vector3.Transform(x, Matrix.CreateFromAxisAngle(y, roll));

			Vector3 targetUnit = y;
			pos += z*0.2f;
			this.position = pos;
			this.target = this.position + targetUnit*100;

			return new CameraOrientation {Position = this.position, Target = this.target, Up = z};
		}


		public Matrix ViewMatrix
		{
			get
			{
				CameraOrientation orientation = GetCameraOrientation();
				return Matrix.CreateLookAt(orientation.Position, orientation.Target, orientation.Up);
			}
		}

		#endregion
	}
}