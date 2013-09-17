namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1012)]
	public class VehicleReadyCommandParser : ReflectionCommandParser<VehicleReadyCommand>
	{
		public VehicleReadyCommandParser()
			: base(cmd => cmd.X,
			       cmd => cmd.Y,
			       cmd => cmd.Z,
			       cmd => cmd.Heading)
		{
		}
	}
}