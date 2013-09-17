namespace LiveLab3D.Screens.ConsoleModal.Commands
{
	using System.Collections.Generic;
	using Castle.Windsor;

	public class SimpleConsoleCommandInterpreterRegistry : IConsoleCommandInterpreterRegistry
	{
		private readonly IWindsorContainer container;

		public SimpleConsoleCommandInterpreterRegistry(IWindsorContainer container)
		{
			this.container = container;
		}

		#region IConsoleCommandInterpreterRegistry Members

		public IEnumerable<IConsoleCommandInterpreter> GetInterpreters()
		{
			return this.container.ResolveAll<IConsoleCommandInterpreter>();
		}

		#endregion
	}
}