namespace LiveLab3D.Windsor
{
	using Castle.Facilities.Startable;
	using Castle.Windsor;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Streams;
	using Rhino.Commons.Binsor;
	using Component = Castle.MicroKernel.Registration.Component;

	public class PlayModeInstaller : IModeInstaller<PlayModeSetting>
	{
		#region IModeInstaller<PlayModeSetting> Members

		public void Register(PlayModeSetting modeSetting)
		{
			IWindsorContainer windsorContainer = AbstractConfigurationRunner.IoC.Container;
			windsorContainer
				.Register(Component.For<IObjectSource>().ImplementedBy<ViconObjectSource>())
				.Register(Component.For<ITimeSource>().ImplementedBy<ViconTimeSource>())
				.Register(Component.For<IUdpListener>().ImplementedBy<FilePositionListener>().Named("positionStream")
				          	.DependsOn(new {fileName = modeSetting.PositionFileName})
				          	.StartUsingMethod(x => x.Start).StopUsingMethod(x => x.Stop))
				.Register(Component.For<IUdpListener>().ImplementedBy<FileCommandListener>().Named("commandStream")
				          	.DependsOn(new {fileName = modeSetting.CommandFileName})
				          	.StartUsingMethod(x => x.Start).StopUsingMethod(x => x.Stop));
		}

		#endregion
	}
}