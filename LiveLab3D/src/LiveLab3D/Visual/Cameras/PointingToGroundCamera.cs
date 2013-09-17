namespace LiveLab3D.Visual.Cameras
{
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;

	public class PointingToGroundCamera : ICamera
	{
		private readonly ObjectBase vehicle;
		private Vector3 position;
		private Vector3 target;

		public PointingToGroundCamera(ObjectBase vehicle)
		{
			this.vehicle = vehicle;
		}

		#region ICamera Members

		public void UpdateCamera(GameTime gameTime)
		{
		}

		public CameraOrientation GetCameraOrientation()
		{
			this.target = this.vehicle.PositionalData.Position;
			this.target.Z = 0;
			this.position = this.vehicle.PositionalData.Position;
			this.position.Z -= 0f;
			return new CameraOrientation {Position = this.position, Target = this.target, Up = Vector3.UnitY};
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