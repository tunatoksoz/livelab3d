namespace LiveLab3D
{
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Visual.Cameras;
	using Microsoft.Xna.Framework.Graphics;

	public interface IEnvironment
	{
		Model World { get; }
		IObjectSource ObjectSource { get; }
		ICamera Camera { get; set; }
		ITimeSource TimeSource { get; }
		int Width { get; }
		int Height { get; }
		float AspectRatio { get; }
	}
}