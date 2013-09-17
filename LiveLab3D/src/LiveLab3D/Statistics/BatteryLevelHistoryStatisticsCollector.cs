namespace LiveLab3D.Statistics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
    using System.Threading;

	public class BatteryLevelHistoryStatisticsCollector : PerVehicleStatisticsCollectorBase<BatteryLevelHistoryStatistics>
	{
		private readonly IDictionary<ObjectBase, IList<Pair<TimeSpan, float>>> batteryLevelHistories;
		private readonly IEventAggregator eventAggregator;
		private readonly TimeSpan lastAdded;
		private readonly TimeSpan recordWindow;
		private readonly TimeSpan resolution;
		private readonly ITimeSource timeSource;
        private object lockObject = new object();
		public BatteryLevelHistoryStatisticsCollector(IObjectSource objectSource, ITimeSource timeSource,
		                                              IEventAggregator eventAggregator, TimeSpan resolution,
		                                              TimeSpan recordWindow)
			: base(objectSource)
		{
			this.timeSource = timeSource;
			this.recordWindow = recordWindow;
			this.resolution = resolution;
			this.eventAggregator = eventAggregator;
			this.batteryLevelHistories = new Dictionary<ObjectBase, IList<Pair<TimeSpan, float>>>();
			this.lastAdded = TimeSpan.Zero.Add(new TimeSpan(-1, 0, 0, 0));
		}

		protected void Handle(ObjectBase vehicle, GeneralHealthStatusCommand command)
		{
            lock (lockObject)
            {
                EnsureInitialized(vehicle);
                TimeSpan currentTime = this.timeSource.Time;
                IList<Pair<TimeSpan, float>> values = this.batteryLevelHistories[vehicle];

                IEnumerable<Pair<TimeSpan, float>> expired = values.Where(x => x.Item1 < currentTime.Subtract(this.recordWindow)).ToArray();
                foreach (var pair in expired)
                    values.Remove(pair);

                if (currentTime.Subtract(this.lastAdded) >= this.resolution)
                    this.batteryLevelHistories[vehicle].Add(new Pair<TimeSpan, float>(command.Time, command.ChargeStatus));
            }
		}

		protected void EnsureInitialized(ObjectBase vehicle)
		{
			if (!this.batteryLevelHistories.ContainsKey(vehicle))
				this.batteryLevelHistories[vehicle] = new List<Pair<TimeSpan, float>>();
		}

		public override BatteryLevelHistoryStatistics GetStatisticsForVehicle(ObjectBase objectBase)
		{
            lock (lockObject)
            {
                IList<Pair<TimeSpan, float>> items;
                if (this.batteryLevelHistories.ContainsKey(objectBase))
                    items = this.batteryLevelHistories[objectBase];
                else items = new List<Pair<TimeSpan, float>>();
                return new BatteryLevelHistoryStatistics { History = items };
            }
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