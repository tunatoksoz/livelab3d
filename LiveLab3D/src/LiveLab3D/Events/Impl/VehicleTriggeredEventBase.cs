namespace LiveLab3D.Events.Impl
{
	using LiveLab3D.Objects;

	public abstract class VehicleTriggeredEventBase : EventBase
	{
		public ObjectBase Vehicle { get; set; }
	}
}