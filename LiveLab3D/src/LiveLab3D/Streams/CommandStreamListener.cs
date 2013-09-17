namespace LiveLab3D.Streams
{
	using System;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Parsers.CommandParsers;

	public class CommandListener
	{
		private readonly IEventAggregator eventAggregator;
		private readonly IUdpListener listener;
		private readonly IObjectSource objectSource;
		private readonly ICommandParserRegistry registry;
		private readonly ITimeSource timeSource;

		public CommandListener(IObjectSource objectSource,
		                       IUdpListener listener, ICommandParserRegistry registry,
		                       ITimeSource timeSource, IEventAggregator eventAggregator)
		{
			this.listener = listener;

			this.objectSource = objectSource;
			this.registry = registry;
			this.timeSource = timeSource;
			this.eventAggregator = eventAggregator;
		}

		public void Start()
		{
			this.listener.PacketReceived += PacketReceived;
		}

		public void Stop()
		{
			this.listener.PacketReceived -= PacketReceived;
		}

		private void PacketReceived(string packet)
		{
			string[] arr = packet.Split(' ');
			int destination = int.Parse(arr[1]);
			int commandType = int.Parse(arr[2]);
            if (commandType == 10011)
                commandType.ToString();
			int source = int.Parse(arr[0]);
			IVehicleCommandParser parser = this.registry.GetParserForCommandNumber(commandType);
			VehicleCommandBase command = parser.Parse(packet.Substring(arr[0].Length + arr[1].Length + arr[2].Length + 3));
			command.Time = this.timeSource.Time;
			ObjectBase destVehicle = this.objectSource.GetObject(destination);
            ObjectBase sourceVehicle = this.objectSource.GetObject(source);
			Type commandT = typeof (CommandReceivedEvent<>).MakeGenericType(command.GetType());
			var @event = (CommandReceivedEvent) Activator.CreateInstance(commandT);
			@event.Command = command;
            @event.Destination = destVehicle;
            @event.Source = sourceVehicle;
			this.eventAggregator.Publish(@event);
		}
	}
}