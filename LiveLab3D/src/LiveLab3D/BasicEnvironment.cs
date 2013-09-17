namespace LiveLab3D
{
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Visual.Cameras;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class BasicEnvironment : IEnvironment
	{
		public Vector3 InitialCameraTarget { get; set; }

		#region IEnvironment Members

		public Model World { get; set; }

		public ICamera Camera { get; set; }

		public IObjectSource ObjectSource { get; set; }


		public ITimeSource TimeSource { get; set; }

		public int Width { get; set; }

		public int Height { get; set; }

		public float AspectRatio
		{
			get { return ((float) Width)/Height; }
		}

		#endregion
	}
}