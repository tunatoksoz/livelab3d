namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 10010)]
	public class LandNowCommandParser : ReflectionCommandParser<LandNowCommand>
	{
		public LandNowCommandParser()
			: base(command => command.X,
			       command => command.Y,
			       command => command.ApproachAltitude,
			       command => command.ApproachHeading,
			       command => command.ApproachVelocity,
			       command => command.HoverTime,
			       command => command.PadOffset)
		{
		}
	}
}