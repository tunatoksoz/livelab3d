namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1008)]
	public class RefuelCommandParser : ReflectionCommandParser<RefuelCommand>
	{
		public RefuelCommandParser()
			: base(cmd => cmd.RefuelAmount, cmd => cmd.DelayTime)
		{
		}
	}
}