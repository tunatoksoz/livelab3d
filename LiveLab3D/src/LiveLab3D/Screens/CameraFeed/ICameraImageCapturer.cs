namespace LiveLab3D.Screens.CameraFeed
{
	public interface ICameraImageCapturer
	{
		void Start();
		byte[] GetLastFrame();
	}
}