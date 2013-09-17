namespace LiveLab3D.Events.Impl
{
	using LiveLab3D.Commands;
	using LiveLab3D.Objects;



    public class CommandReceivedEvent : EventBase
	{
		public VehicleCommandBase Command { get; set; }
        public ObjectBase Destination { get; set; }
        public ObjectBase Source { get; set; }
	}

	public class CommandReceivedEvent<TCommandBase> : CommandReceivedEvent where TCommandBase : VehicleCommandBase
	{
		public new TCommandBase Command
		{
			get { return (TCommandBase) base.Command; }
			set { base.Command = value; }
		}
	}
}