namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	public interface IVehicleCommandParser : IParser
	{
		new VehicleCommandBase Parse(string content);
	}

	public interface IVehicleCommandParser<T> : IVehicleCommandParser, IParser<T> where T : VehicleCommandBase
	{
		new T Parse(string content);
	}
}