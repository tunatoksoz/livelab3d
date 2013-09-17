namespace LiveLab3D.Visual.Cameras.Sources
{
	using System.Collections.Generic;

	public class CompositeCameraSource : ICameraSource
	{
		private readonly IList<ICameraSource> registries;

		public CompositeCameraSource(params ICameraSource[] sources)
		{
			this.registries = new List<ICameraSource>(sources);
		}

		#region ICameraSource Members

		public ICamera GetCamera(string key)
		{
			foreach (ICameraSource item in this.registries)
			{
				if (item.HasCamera(key))
					return item[key];
			}
			return null;
		}

		public ICamera this[string key]
		{
			get { return GetCamera(key); }
		}

		public bool HasCamera(string key)
		{
			foreach (ICameraSource item in this.registries)
			{
				if (item.HasCamera(key))
					return true;
			}
			return false;
		}

		#endregion
	}
}