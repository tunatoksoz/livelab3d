namespace LiveLab3D.Visual.Cameras
{
	public interface ICameraSource
	{
		ICamera this[string key] { get; }
		bool HasCamera(string key);
		ICamera GetCamera(string key);
	}
}