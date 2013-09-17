namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1004)]
	public class TakeOffCommandParser : ReflectionCommandParser<TakeOffCommand>
	{
		public TakeOffCommandParser()
			: base(
				cmd => cmd.X,
				cmd => cmd.Y,
				cmd => cmd.TakeOffDelay,
				cmd => cmd.TakeOffLocationX,
				cmd => cmd.TakeOffLocationY)
		{
		}
	}
}