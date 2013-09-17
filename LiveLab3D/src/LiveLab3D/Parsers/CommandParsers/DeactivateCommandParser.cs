namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1010)]
	public class DeactivateCommandParser : ReflectionCommandParser<DeactivateCommand>
	{
	}
}