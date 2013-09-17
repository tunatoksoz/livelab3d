namespace LiveLab3D.Visual.Cameras
{
	using Microsoft.Xna.Framework;

	public class ConstantPositionCamera : ICamera
	{
		private readonly CameraOrientation orientation;

		public ConstantPositionCamera(Vector3 position, Vector3 target, Vector3 up)
		{
			this.orientation = new CameraOrientation
			                   	{
			                   		Position = position,
			                   		Target = target,
			                   		Up = up
			                   	};
			ViewMatrix = Matrix.CreateLookAt(position, target, up);
		}

		#region ICamera Members

		public void UpdateCamera(GameTime gameTime)
		{
		}

		public CameraOrientation GetCameraOrientation()
		{
			return this.orientation;
		}

		public Matrix ViewMatrix { get; private set; }

		#endregion
	}
}