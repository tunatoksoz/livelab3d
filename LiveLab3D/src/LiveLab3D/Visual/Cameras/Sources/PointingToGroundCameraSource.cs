namespace LiveLab3D.Visual.Cameras.Sources
{
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;

	public class PointingToGroundCameraSource : VehicleAwareCameraSourceBase
	{
		public PointingToGroundCameraSource(IObjectSource objectSource)
			: base(objectSource)
		{
		}


		protected override string Suffix
		{
			get { return "_ground"; }
		}

		protected override ICamera GetCameraForVehicle(ObjectBase vehicle)
		{
			return new PointingToGroundCamera(vehicle);
		}
	}
}