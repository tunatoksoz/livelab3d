namespace LiveLab3D.Statistics
{
	using System;
	using System.Collections.Generic;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;

	public class DurationOfFlightStatisticsCollector : PerVehicleStatisticsCollectorBase<DurationOfFlightStatistics>
	{
		private readonly IEventAggregator eventAggregator;
		private readonly ITimeSource timeSource;
		private IDictionary<ObjectBase, TimeSpan?> startTimes;
		private IDictionary<ObjectBase, TimeSpan> totalTimes;

		public DurationOfFlightStatisticsCollector(IObjectSource objectSource, IEventAggregator eventAggregator,
		                                           ITimeSource timeSource) : base(objectSource)
		{
			this.eventAggregator = eventAggregator;
			this.timeSource = timeSource;
			this.startTimes = new Dictionary<ObjectBase, TimeSpan?>();
			this.totalTimes = new Dictionary<ObjectBase, TimeSpan>();
		}

		public override DurationOfFlightStatistics GetStatisticsForVehicle(ObjectBase objectBase)
		{
			EnsureInitialized(objectBase, this.timeSource.Time);
			return new DurationOfFlightStatistics
			       	{
			       		DurationOfFlight =
			       			this.totalTimes[objectBase] +
			       			(this.startTimes[objectBase].HasValue
			       			 	? this.timeSource.Time - this.startTimes[objectBase].Value
			       			 	: TimeSpan.Zero)
			       	};
		}

		protected void EnsureInitialized(ObjectBase vehicle, TimeSpan time)
		{
			if (!this.startTimes.ContainsKey(vehicle))
			{
				this.startTimes[vehicle] = null;
				this.totalTimes[vehicle] = TimeSpan.Zero;
			}
		}

		private void Handle(DeactivateCommand deactivateCommand, ObjectBase vehicle)
		{
			EnsureInitialized(vehicle, deactivateCommand.Time);
			if (this.startTimes[vehicle].HasValue)
			{
				this.totalTimes[vehicle] += deactivateCommand.Time - this.startTimes[vehicle].Value;
				this.startTimes[vehicle] = null;
			}
		}

		private void Handle(ActivateCommand activateCommand, ObjectBase vehicle)
		{
			EnsureInitialized(vehicle, activateCommand.Time);
			this.startTimes[vehicle] = activateCommand.Time;
		}

		public override void Start()
		{
			this.eventAggregator.Subscribe<CommandReceivedEvent<ActivateCommand>>(x => Handle(x.Command, x.Destination));
			this.eventAggregator.Subscribe<CommandReceivedEvent<DeactivateCommand>>(x => Handle(x.Command, x.Destination));
		}


		public override void Stop()
		{
		}
	}
}