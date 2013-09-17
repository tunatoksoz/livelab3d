namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1013)]
	public class VehicleDeactivatedCommandParser : ReflectionCommandParser<VehicleDeactivatedCommand>
	{
		public VehicleDeactivatedCommandParser()
			: base(cmd => cmd.X, cmd => cmd.Y)
		{
		}
	}
}