namespace LiveLab3D.Visual.Cameras.Sources
{
	using System.Collections.Generic;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;

	public abstract class VehicleAwareCameraSourceBase : ICameraSource
	{
		private readonly IDictionary<string, ICamera> cameras;
		private readonly IObjectSource objectSource;

		protected VehicleAwareCameraSourceBase(IObjectSource objectSource)
		{
			this.objectSource = objectSource;
			this.cameras = new Dictionary<string, ICamera>();
		}

		protected abstract string Suffix { get; }

		#region ICameraSource Members

		public virtual bool HasCamera(string key)
		{
			string vehicleName = key.Substring(0, key.Length - Suffix.Length);
			return key.EndsWith(Suffix) && this.objectSource.GetObject(vehicleName) != null;
		}

		public ICamera GetCamera(string key)
		{
			if (HasCamera(key))
			{
				if (!this.cameras.ContainsKey(key))
					CreateAndAddCamera(key);
				return this.cameras[key];
			}
			else
			{
				return null;
			}
		}

		public ICamera this[string key]
		{
			get { return GetCamera(key); }
		}

		#endregion

		protected abstract ICamera GetCameraForVehicle(ObjectBase vehicle);

		protected virtual void CreateAndAddCamera(string key)
		{
			string vehicleName = key.Substring(0, key.Length - Suffix.Length);
			ObjectBase vehicle = this.objectSource.GetObject(vehicleName);
			this.cameras[key] = GetCameraForVehicle(vehicle);
		}
	}
}