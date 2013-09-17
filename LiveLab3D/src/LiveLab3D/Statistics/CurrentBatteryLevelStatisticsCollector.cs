namespace LiveLab3D.Statistics
{
	using System.Collections.Generic;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;

	public class CurrentBatteryLevelStatisticsCollector : PerVehicleStatisticsCollectorBase<CurrentBatteryLevelStatistics>
	{
		private readonly IDictionary<ObjectBase, float> batteryLevels;
		private readonly IEventAggregator eventAggregator;

		public CurrentBatteryLevelStatisticsCollector(IObjectSource objectSource, IEventAggregator eventAggregator)
			: base(objectSource)
		{
			this.eventAggregator = eventAggregator;

			this.batteryLevels = new Dictionary<ObjectBase, float>();
		}

		protected void Handle(ObjectBase vehicle, GeneralHealthStatusCommand command)
		{
            float level = command.ChargeStatus;
            if (level > 1.0)
                level = 1.0f;
            if (level <= 0.0)
                level = 0.00f;
            this.batteryLevels[vehicle] = level;
		}

		public override CurrentBatteryLevelStatistics GetStatisticsForVehicle(ObjectBase objectBase)
		{
			if (this.batteryLevels.ContainsKey(objectBase))
				return new CurrentBatteryLevelStatistics {Level = this.batteryLevels[objectBase]};
			return new CurrentBatteryLevelStatistics {Level = 1.0f};
		}

		public override void Start()
		{
			this.eventAggregator.Subscribe<CommandReceivedEvent<GeneralHealthStatusCommand>>(x => Handle(x.Destination, x.Command));
		}

		public override void Stop()
		{
		}
	}
}