namespace LiveLab3D.Screens.ConsoleModal.Commands
{
	public interface IConsoleCommandInterpreter
	{
		bool CanInterpret(string text);
		void Interpret(string text);
	}
}