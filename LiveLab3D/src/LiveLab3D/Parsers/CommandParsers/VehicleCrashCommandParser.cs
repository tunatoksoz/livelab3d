namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1011)]
	public class VehicleCrashCommandParser : ReflectionCommandParser<VehicleCrashCommand>
	{
		public VehicleCrashCommandParser()
			: base(cmd => cmd.X, cmd => cmd.Y)
		{
		}
	}
}