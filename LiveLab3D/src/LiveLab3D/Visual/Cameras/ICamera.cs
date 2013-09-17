namespace LiveLab3D.Visual.Cameras
{
	using Microsoft.Xna.Framework;

	public interface ICamera
	{
		Matrix ViewMatrix { get; }
		void UpdateCamera(GameTime gameTime);
		CameraOrientation GetCameraOrientation();
	}

	public class CameraOrientation
	{
		public Vector3 Position { get; set; }
		public Vector3 Target { get; set; }
		public Vector3 Up { get; set; }
	}
}