namespace LiveLab3D.Visual.Cameras.Sources
{
	using System.Collections.Generic;

	public class InMemoryCameraSource : ICameraSource
	{
		private readonly IDictionary<string, ICamera> cameras;

		public InMemoryCameraSource()
		{
			this.cameras = new Dictionary<string, ICamera>();
		}

		#region ICameraSource Members

		public ICamera GetCamera(string key)
		{
			return this.cameras[key];
		}

		public ICamera this[string key]
		{
			get { return GetCamera(key); }
		}

		public bool HasCamera(string key)
		{
			return this.cameras.ContainsKey(key);
		}

		#endregion

		public void AddCamera(string key, ICamera camera)
		{
			this.cameras[key] = camera;
		}
	}
}