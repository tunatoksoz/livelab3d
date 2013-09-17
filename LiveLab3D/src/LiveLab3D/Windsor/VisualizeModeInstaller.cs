namespace LiveLab3D.Windsor
{
	using Castle.Facilities.Startable;
	using Castle.Windsor;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Streams;
	using Rhino.Commons.Binsor;
	using Component = Castle.MicroKernel.Registration.Component;

	public class VisualizeModeInstaller : IModeInstaller<VisualizeModeSetting>
	{
		#region IModeInstaller<VisualizeModeSetting> Members

		public void Register(VisualizeModeSetting modeSetting)
		{
			IWindsorContainer windsorContainer = AbstractConfigurationRunner.IoC.Container;
			windsorContainer
				.Register(Component.For<IObjectSource>().ImplementedBy<ViconObjectSource>())
				.Register(Component.For<ITimeSource>().ImplementedBy<ViconTimeSource>())
				.Register(Component.For<IUdpListener>().ImplementedBy<UdpListener>().Named("positionStream")
				          	.DependsOn(new {port = 9999})
				          	.StartUsingMethod(x => x.Start)
				          	.StopUsingMethod(x => x.Stop))
				.Register(Component.For<IUdpListener>().ImplementedBy<UdpListener>().Named("commandStream")
				          	.DependsOn(new {port = 20000})
				          	.StartUsingMethod(x => x.Start)
				          	.StopUsingMethod(x => x.Stop));
		}

		#endregion
	}
}