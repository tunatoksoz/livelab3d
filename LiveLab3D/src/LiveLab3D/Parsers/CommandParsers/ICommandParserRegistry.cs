namespace LiveLab3D.Parsers.CommandParsers
{
	public interface ICommandParserRegistry
	{
		IVehicleCommandParser GetParserForCommandNumber(int number);
	}
}