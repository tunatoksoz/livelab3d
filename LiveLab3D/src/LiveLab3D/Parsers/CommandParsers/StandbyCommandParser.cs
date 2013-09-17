namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1002)]
	public class StandbyCommandParser : ReflectionCommandParser<StandByCommand>
	{
		public StandbyCommandParser()
			: base(cmd => cmd.X,
			       cmd => cmd.Y,
			       cmd => cmd.ReadyStateDelay)
		{
		}
	}
}