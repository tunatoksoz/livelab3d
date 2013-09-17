namespace LiveLab3D
{
	using System;
	using Castle.Windsor;
	using LiveLab3D.Windsor;
	using Rhino.Commons.Binsor;

	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private static void Main(string[] args)
		{
			IWindsorContainer container = new WindsorContainer();
			new BootStrapper().Install(container, container.Kernel.ConfigurationStore);
			string source;
			if (args.Length == 0)
				source = "allinone.boo";
			else
				source = args[0];
			BooReader.Read(container, source);
			

			var game = container.Resolve<LiveLab>();
			{
				game.TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 20);
				game.IsFixedTimeStep = true;
				game.Run();
			}
		}
	}
}