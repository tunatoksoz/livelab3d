namespace LiveLab3D.Visual.Cameras.Sources
{
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;

	public class FollowVehicleCameraSource : VehicleAwareCameraSourceBase
	{
		private readonly IEnvironment environment;

		public FollowVehicleCameraSource(IObjectSource objectSource, IEnvironment environment)
			: base(objectSource)
		{
			this.environment = environment;
		}

		protected override string Suffix
		{
			get { return "_follow"; }
		}

		protected override ICamera GetCameraForVehicle(ObjectBase vehicle)
		{
			return new FollowVehicleCamera(this.environment, vehicle);
		}
	}
}