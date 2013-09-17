namespace LiveLab3D.Statistics
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using Microsoft.Xna.Framework;

	public class FlightDistanceStatisticsCollector : PerVehicleStatisticsCollectorBase<FlightDistanceStatistics>
	{
		private readonly IDictionary<ObjectBase, float> distances;
		private readonly IDictionary<ObjectBase, Vector3?> lastPositions;
		private readonly LiveLab liveLab;
		private readonly IObjectSource objectSource;

		public FlightDistanceStatisticsCollector(IObjectSource objectSource, LiveLab liveLab) : base(objectSource)
		{
			this.liveLab = liveLab;
			this.distances = new Dictionary<ObjectBase, float>();
			this.lastPositions = new Dictionary<ObjectBase, Vector3?>();
			this.objectSource = objectSource;
		}

		public override FlightDistanceStatistics GetStatisticsForVehicle(ObjectBase objectBase)
		{
			EnsureInitialized(objectBase);
			return new FlightDistanceStatistics {Distance = this.distances[objectBase]};
		}

		public override void Start()
		{
			this.liveLab.GameUpdated += HandleUpdate;
		}

		private void EnsureInitialized(ObjectBase item)
		{
			if (!this.distances.ContainsKey(item))
			{
				this.distances[item] = 0;
				this.lastPositions[item] = null;
			}
		}

		private void HandleUpdate(TimeSpan gameTime)
		{
			ObjectBase[] objects = this.objectSource.GetObjects().ToArray();
			foreach (ObjectBase vehicle in objects)
			{
				EnsureInitialized(vehicle);
				Vector3 position = vehicle.PositionalData.Position;
				if (!this.lastPositions[vehicle].HasValue)
					this.lastPositions[vehicle] = position;
				Vector3 diff = position - this.lastPositions[vehicle].Value;
				this.lastPositions[vehicle] = vehicle.PositionalData.Position;
				this.distances[vehicle] += diff.Length();
			}
		}

		public override void Stop()
		{
			this.liveLab.GameUpdated -= HandleUpdate;
		}
	}
}