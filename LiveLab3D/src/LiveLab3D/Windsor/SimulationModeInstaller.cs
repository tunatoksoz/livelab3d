namespace LiveLab3D.Windsor
{
	using Castle.Facilities.Startable;
	using Castle.Windsor;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Simulation;
	using LiveLab3D.Streams;
	using Rhino.Commons.Binsor;
	using Component = Castle.MicroKernel.Registration.Component;

	public class SimulationModeInstaller : IModeInstaller<SimulationModeSetting>
	{
		#region IModeInstaller<SimulationModeSetting> Members

		public void Register(SimulationModeSetting modeSetting)
		{
			IWindsorContainer windsorContainer = AbstractConfigurationRunner.IoC.Container;
			windsorContainer
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<UdpListener>().Named("commandStream")
				          	.DependsOn(new {port = 20000})
				          	.StartUsingMethod(x => x.Start)
				          	.StopUsingMethod(x => x.Stop))
				.Register(Component.For<IObjectSource>()
				          	.ImplementedBy<SimulationObjectSource>()
				          	.Forward(typeof(SimulationObjectSource))
							.OnCreate((kernel, source) =>
				          	          	{
				          	          		var s = source as SimulationObjectSource;
				          	          		s.AddObjects(modeSetting.Vehicles);
				          	          	}))
				.Register(Component.For<ITimeSource>().ImplementedBy<SimulationTimeSource>())
				.Register(Component.For<IUdpTransmitter>()
				          	.ImplementedBy<UdpTransmitter>()
				          	.DependsOn(new {sourcePort = 1059, destinationPort = 9999}))
				.Register(Component.For<ViconTransmitter>().StartUsingMethod(x => x.StartTransmission)
				          	.StartUsingMethod(x => x.StartTransmission)
				          	.DependsOn(new {interval = 20}));
		}

		#endregion
	}
}