namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1006)]
	public class PrepareToLandCommandParser : ReflectionCommandParser<PrepareToLandCommand>
	{
		public PrepareToLandCommandParser()
			: base(cmd => cmd.ReturnToBase,
			       cmd => cmd.LandingLocationX,
			       cmd => cmd.LandingLocationY,
			       cmd => cmd.LandingLocationZ,
			       cmd => cmd.ApproachHeading,
			       cmd => cmd.Velocity)
		{
		}
	}
}