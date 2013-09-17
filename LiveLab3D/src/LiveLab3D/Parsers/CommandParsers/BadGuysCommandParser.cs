using System.Linq;
using LiveLab3D.Commands;

namespace LiveLab3D.Parsers.CommandParsers
{
	[Parses(CommandNumber = 6667)]
	public class BadGuysCommandParser : IVehicleCommandParser<BadGuysCommand>
	{

		#region IVehicleCommandParser<T> Members

		public BadGuysCommand Parse(string command)
		{
			string[] parts = command.Split(' ', ';');
			var item = new BadGuysCommand();
			item.BadGuyIds= parts.Where(x => (!"0".Equals(x)) && !string.IsNullOrEmpty(x)).Select(x => int.Parse(x)).ToArray();
			return item;
		}

		object IParser.Parse(string line)
		{
			return Parse(line);
		}

		VehicleCommandBase IVehicleCommandParser.Parse(string content)
		{
			return Parse(content);
		}

		#endregion
	}
}
