using LiveLab3D.Screens.CameraFeed;

namespace LiveLab3D.Windsor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Castle.Facilities.EventWiring;
	using Castle.Facilities.Startable;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;
	using Castle.MicroKernel.Resolvers.SpecializedResolvers;
	using Castle.Windsor;
	using LiveLab3D.Events;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Parsers.CommandParsers;
	using LiveLab3D.Screens;
	using LiveLab3D.Screens.ConsoleModal.Commands;
	using LiveLab3D.Statistics;
	using LiveLab3D.Statistics.Visualization;
	using LiveLab3D.Streams;
	using LiveLab3D.Visual;
	using LiveLab3D.Visual.Cameras;
	using LiveLab3D.Visual.Cameras.Sources;
	using LiveLab3D.Visual.CommandTextifiers;
	using LiveLab3D.Visual.Components.ConsoleModal;
	using LiveLab3D.Visual.ObjectVisuals;
	using Microsoft.Xna.Framework;

	public class BootStrapper : IWindsorInstaller
	{
		#region IWindsorInstaller Members

		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Kernel.Resolver.AddSubResolver(new ModelSubdependencyResolver(container));
			container.Kernel.Resolver.AddSubResolver(new ListResolver(container.Kernel));
			InstallFilters(container, typeof (IDrawingPipeline).Assembly);

			container
				.AddFacility<StartableFacility>()
				.AddFacility<EventWiringFacility>()
				.Register(Component.For<IWindsorContainer>().Instance(container))
				.Register(Component.For<CommandListener>()
				          	.ServiceOverrides(new {listener = "commandStream"})
				          	.StartUsingMethod(x => x.Start))
				.Register(Component.For<IObjectBuilder>().ImplementedBy<DefaultObjectBuilder>())
				.Register(Component.For<ICommandParserRegistry>()
				          	.ImplementedBy<AssemblyCommandParserRegistry>()
				          	.DependsOn(new {assembly = typeof (TakeOffCommandParser).Assembly}))
				.Register(Component.For<ICommandTextifierRegistry>()
				          	.ImplementedBy<AssemblyCommandTextifierRegistry>()
				          	.DependsOn(new {assembly = typeof (ICommandTextifier).Assembly}))
				.Register(Component.For<LiveLab>().Named("LiveLab").Forward<Game>().ImplementedBy<LiveLab>())
				.Register(Component.For(typeof (IObjectVisual<>)).ImplementedBy(typeof (Visual<>))
				          	.LifeStyle.Transient)
				
				.Register(Component.For<CommandBoxComponent>().LifeStyle.Transient
				          	.DependsOn(new {width = 300, height = 300}))

				.Register(AllTypes.Of<DrawableGameComponent>().FromAssembly(typeof(FrameRateCounter).Assembly).Configure(x=>x.LifeStyle.Transient))


				.Register(Component.For<ConsoleModal>().LifeStyle.Transient)
				
				.Register(AllTypes.Of<IPerVehicleStatisticsCollector>().FromAssembly(
					typeof (IPerVehicleStatisticsCollector).Assembly)
				          	.WithService.FirstInterface()
				          	.ConfigureFor<BatteryLevelHistoryStatisticsCollector>(x => x.DependsOn(
				          		new
				          			{
				          				resolution = new TimeSpan(0, 0, 2, 0),
				          				recordWindow = new TimeSpan(0, 0, 5, 0)
				          			}))
				          	.Configure(
				          		x =>
				          		x.Forward(typeof (IPerVehicleStatisticsCollector)).StartUsingMethod("Start").StopUsingMethod("Stop")))
				.Register(Component.For<PerVehicleStatisticsWindow>().LifeStyle.Transient)
				.Register(Component.For<PerStatisticsStatisticsWindow>().LifeStyle.Transient)
				.Register(Component.For<CameraFeedModal>().LifeStyle.Transient.ServiceOverrides(new {cameraImageCapturers =
									new[]
				          				{
				          					"camera1",
				          					"camera2",
				          					"camera3",
                                            "camera4",
                                            "camera5"

				          				}}))
				.Register(Component.For<ICameraImageCapturer>().ImplementedBy<MJPEGCameraImageCapturer>().Named("camera1").DependsOn(new {uri="http://192.168.7.248/nphMotionJpeg?Resolution=192x144&Quality=Clarity",username="rayche",password="uavswarm"}))
				.Register(Component.For<ICameraImageCapturer>().ImplementedBy<MJPEGCameraImageCapturer>().Named("camera2").DependsOn(new {uri="http://192.168.7.231/nphMotionJpeg?Resolution=192x144&Quality=Clarity",username="rayche",password="uavswarm"}))
				.Register(Component.For<ICameraImageCapturer>().ImplementedBy<MJPEGCameraImageCapturer>().Named("camera3").DependsOn(new {uri="http://192.168.7.233/nphMotionJpeg?Resolution=192x144&Quality=Clarity",username="rayche",password="uavswarm"}))
				.Register(Component.For<ICameraImageCapturer>().ImplementedBy<MJPEGCameraImageCapturer>().Named("camera4").DependsOn(new {uri="http://192.168.7.230/nphMotionJpeg?Resolution=192x144&Quality=Clarity",username="rayche",password="uavswarm"}))
				.Register(Component.For<ICameraImageCapturer>().ImplementedBy<MJPEGCameraImageCapturer>().Named("camera5").DependsOn(new {uri="http://192.168.7.234/nphMotionJpeg?Resolution=192x144&Quality=Clarity",username="rayche",password="uavswarm"}))
				.Register(
					AllTypes.Of<IStatisticsVisualizer>().FromAssembly(typeof (IStatisticsVisualizer).Assembly).WithService.
						FirstInterface())
				.Register(Component.For<IStatisticsVisualizationRegistry>().ImplementedBy<StatisticsVisualizationRegistry>())
				.Register(Component.For<IEnvironment>()
				          	.ImplementedBy<BasicEnvironment>()
				          	.OnCreate((kernel, component) =>
				          	          	{
				          	          		var benvironment = (BasicEnvironment) component;
				          	          		benvironment.Width = 1920;
				          	          		benvironment.Height = 1080;
				          	          		benvironment.ObjectSource = kernel.Resolve<IObjectSource>();
				          	          		benvironment.TimeSource = kernel.Resolve<ITimeSource>();
				          	          		benvironment.Camera = new ConstantPositionCamera(new Vector3(2, 0, 10),
				          	          		                                                 new Vector3(2, 0, 0),
				          	          		                                                 new Vector3(0, 1, 0));
				          	          	}
				          	))
				.Register(Component.For<ICameraSource>().ImplementedBy<CompositeCameraSource>()
				          	.Named("compositeCameraSource")
				          	.ServiceOverrides(
				          		new
				          			{
				          				sources =
				          			new[]
				          				{
				          					"pointingToGroundCameraSource",
				          					"followVehicleCameraSource",
				          					"vehicleHeadCameraSource"
				          				}
				          			}))
				.Register(Component.For<IEventAggregator>().UsingFactoryMethod(() => EventAggregatorImpl.Instance))
				.Register(Component.For<ICameraSource>()
				          	.ImplementedBy<PointingToGroundCameraSource>()
				          	.Named("pointingToGroundCameraSource"))
				.Register(Component.For<ICameraSource>()
				          	.ImplementedBy<FollowVehicleCameraSource>()
				          	.Named("followVehicleCameraSource"))
				.Register(Component.For<ICameraSource>()
				          	.ImplementedBy<VehicleHeadCameraSource>()
				          	.Named("vehicleHeadCameraSource"))
				.Register(Component.For<IConsoleCommandInterpreter>()
				          	.ImplementedBy<ChangeCameraCommandInterpreter>()
				          	.ServiceOverrides(new {cameraSource = "compositeCameraSource"}))
				.Register(
					AllTypes.Of<IConsoleCommandInterpreter>().FromAssembly(typeof (IConsoleCommandInterpreter).Assembly).WithService.
						FirstInterface())
				.Register(Component.For<IConsoleCommandInterpreterRegistry>()
				          	.ImplementedBy<SimpleConsoleCommandInterpreterRegistry>());
		}

		#endregion

		protected void InstallFilters(IWindsorContainer container, Assembly assembly)
		{
			Type[] types = assembly.GetTypes().Where(type => typeof (IDrawingFilter).IsAssignableFrom(type)).ToArray();
			IDictionary<string, IList<Type>> keyFilter = new Dictionary<string, IList<Type>>();
			foreach (Type type in types)
			{
				if (type.IsAbstract || type.IsInterface)
					continue;
				container.Register(Component.For(typeof (IDrawingFilter)).ImplementedBy(type).Named(type.FullName));
				FilterKey[] attributes = type.GetCustomAttributes(typeof (FilterKey), true).Cast<FilterKey>().ToArray();
				foreach (FilterKey filterKey in attributes)
				{
					EnsureListInitiated(keyFilter, filterKey.Key);
					keyFilter[filterKey.Key].Add(type);
				}
			}
			var componentKeys = new List<string>();
			foreach (var item in keyFilter)
			{
				string pipeLineComponentKey = item.Key + "_Pipeline";
				string[] keys = item.Value.Select(type => type.FullName).ToArray();
				componentKeys.Add(pipeLineComponentKey);
				container.Register(
					Component.For<IDrawingPipeline>().ImplementedBy<DrawingPipeline>().ServiceOverrides(new {filters = keys}).DependsOn
						(new {key = item.Key}).Named(pipeLineComponentKey));
			}
			container.Kernel.Register(
				Component.For<IDrawingPipelineRegistry>().ImplementedBy<DrawingPipelineRegistry>().ServiceOverrides(
					new {pipelines = componentKeys.ToArray()}));
		}

		protected void EnsureListInitiated(IDictionary<string, IList<Type>> dictionary, string key)
		{
			if (!dictionary.ContainsKey(key))
				dictionary[key] = new List<Type>();
		}

		protected void RegisterNormalListeners(IWindsorContainer container)
		{
			container.Register(Component.For<IUdpListener>()
			                   	.ImplementedBy<UdpListener>()
			                   	.Named("commandStream")
			                   	.DependsOn(new {port = 20000})
			                   	.StartUsingMethod("Start"))
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<UdpListener>()
				          	.Named("positionStream")
				          	.DependsOn(new {port = 9999})
				          	.StartUsingMethod("Start"));
		}

		protected void RegisterSavingListeners(IWindsorContainer container, string commandFile, string viconFile)
		{
			string viconFilePath = viconFile;
			string commandFilePath = commandFile;
			container
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<UdpListener>()
				          	.Named("commandStreamInner")
				          	.DependsOn(new {port = 20000}))
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<UdpListener>()
				          	.Named("positionStreamInner")
				          	.DependsOn(new {port = 9999}))
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<SavingPositionListenerDecorator>()
				          	.Named("positionStream")
				          	.DependsOn(new {fileName = viconFilePath, port = 9999})
				          	.ServiceOverrides(new {innerListener = "positionStreamInner"})
				          	.StartUsingMethod("Start"))
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<SavingCommandListenerDecorator>()
				          	.Named("commandStream")
				          	.DependsOn(new {fileName = commandFilePath, port = 20000})
				          	.ServiceOverrides(new {innerListener = "commandStreamInner"})
				          	.StartUsingMethod("Start"));
		}

		protected void RegisterPlayingListeners(IWindsorContainer container, string commandFile, string viconFile)
		{
			container
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<FileCommandListener>()
				          	.Named("commandStream")
				          	.DependsOn(new {fileName = commandFile})
				          	.StartUsingMethod("Start"))
				.Register(Component.For<IUdpListener>()
				          	.ImplementedBy<FilePositionListener>()
				          	.Named("positionStream")
				          	.DependsOn(new {fileName = viconFile})
				          	.StartUsingMethod("Start"));
		}
	}
}