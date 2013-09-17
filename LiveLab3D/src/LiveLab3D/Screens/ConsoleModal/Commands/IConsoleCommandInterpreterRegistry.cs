namespace LiveLab3D.Screens.ConsoleModal.Commands
{
	using System.Collections.Generic;

	public interface IConsoleCommandInterpreterRegistry
	{
		IEnumerable<IConsoleCommandInterpreter> GetInterpreters();
	}
}