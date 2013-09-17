namespace LiveLab3D.Windsor
{
	using Castle.Facilities.Startable;
	using Castle.Windsor;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Streams;
	using Rhino.Commons.Binsor;
	using Component = Castle.MicroKernel.Registration.Component;

	public class SaveModeInstaller : IModeInstaller<SaveModeSetting>
	{
		#region IModeInstaller<SaveModeSetting> Members

		public void Register(SaveModeSetting modeSetting)
		{
			IWindsorContainer windsorContainer = AbstractConfigurationRunner.IoC.Container;
			windsorContainer
				.Register(Component.For<IObjectSource>().ImplementedBy<ViconObjectSource>())
				.Register(Component.For<ITimeSource>().ImplementedBy<ViconTimeSource>())
				.Register(Component.For<IUdpListener>().ImplementedBy<SavingPositionListenerDecorator>().Named(
					"positionStream")
				          	.DependsOn(new {fileName = modeSetting.PositionFileName})
				          	.ServiceOverrides(new {innerListener = "positionStreamInner"})
				          	.StartUsingMethod(x => x.Start)
				          	.StopUsingMethod(x => x.Stop))
				.Register(Component.For<IUdpListener>().ImplementedBy<SavingCommandListenerDecorator>().Named("commandStream")
				          	.DependsOn(new {fileName = modeSetting.CommandFileName})
				          	.ServiceOverrides(new {innerListener = "commandStreamInner"})
				          	.StartUsingMethod(x => x.Start)
				          	.StopUsingMethod(x => x.Stop))
				.Register(Component.For<IUdpListener>().ImplementedBy<UdpListener>().Named("positionStreamInner")
				          	.DependsOn(new {port = 9999}))
				.Register(Component.For<IUdpListener>().ImplementedBy<UdpListener>().Named("commandStreamInner")
				          	.DependsOn(new {port = 20000}));
		}

		#endregion
	}
}