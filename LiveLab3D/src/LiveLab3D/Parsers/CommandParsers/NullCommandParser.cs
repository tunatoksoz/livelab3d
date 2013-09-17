namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 0)]
	public class NullCommandParser : IVehicleCommandParser<NullCommand>
	{
		#region IVehicleCommandParser<NullCommand> Members

		public NullCommand Parse(string content)
		{
			return new NullCommand();
		}

		VehicleCommandBase IVehicleCommandParser.Parse(string content)
		{
			return Parse(content);
		}

		object IParser.Parse(string line)
		{
			return Parse(line);
		}

		#endregion
	}
}