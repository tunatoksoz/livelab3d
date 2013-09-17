namespace LiveLab3D.Statistics
{
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;

	public abstract class PerVehicleStatisticsCollectorBase<TStatistics> : IPerVehicleStatisticsCollector<TStatistics>
		where TStatistics : IPerVehicleStatistics
	{
		private readonly IObjectSource objectSource;

		protected PerVehicleStatisticsCollectorBase(IObjectSource objectSource)
		{
			this.objectSource = objectSource;
		}

		#region IPerVehicleStatisticsCollector<TStatistics> Members

		public TStatistics GetStatisticsForVehicle(string name)
		{
			return GetStatisticsForVehicle(this.objectSource.GetObject(name));
		}

		IPerVehicleStatistics IPerVehicleStatisticsCollector.GetStatisticsForVehicle(int id)
		{
			return GetStatisticsForVehicle(id);
		}

		IPerVehicleStatistics IPerVehicleStatisticsCollector.GetStatisticsForVehicle(ObjectBase objectBase)
		{
			return GetStatisticsForVehicle(objectBase);
		}

		IPerVehicleStatistics IPerVehicleStatisticsCollector.GetStatisticsForVehicle(string name)
		{
			return GetStatisticsForVehicle(name);
		}

		public TStatistics GetStatisticsForVehicle(int id)
		{
			return GetStatisticsForVehicle(this.objectSource.GetObject(id));
		}

		public abstract TStatistics GetStatisticsForVehicle(ObjectBase objectBase);
		public abstract void Start();
		public abstract void Stop();

		#endregion
	}
}