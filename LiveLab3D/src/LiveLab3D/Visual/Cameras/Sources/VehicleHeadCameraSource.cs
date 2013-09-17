namespace LiveLab3D.Visual.Cameras.Sources
{
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;

	public class VehicleHeadCameraSource : VehicleAwareCameraSourceBase
	{
		public VehicleHeadCameraSource(IObjectSource objectSource) : base(objectSource)
		{
		}

		protected override string Suffix
		{
			get { return "_head"; }
		}

		protected override ICamera GetCameraForVehicle(ObjectBase vehicle)
		{
			return new VehicleHeadCamera(vehicle);
		}
	}
}