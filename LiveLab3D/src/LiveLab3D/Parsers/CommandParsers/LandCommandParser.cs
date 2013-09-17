namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1007)]
	public class LandCommandParser : ReflectionCommandParser<LandCommand>
	{
	}
}