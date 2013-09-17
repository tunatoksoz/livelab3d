using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using LiveLab3D.Commands;

namespace LiveLab3D.Parsers.CommandParsers
{
	[Parses(CommandNumber = 6666)]
	public class GoodGuysCommandParser : IVehicleCommandParser<GoodGuysCommand>
	{

		#region IVehicleCommandParser<T> Members

		public GoodGuysCommand Parse(string command)
		{
			string[] parts = command.Split(' ',';');
			var item = new GoodGuysCommand();
			item.GoodGuyIds = parts.Where(x=>(!"0".Equals(x))&&!string.IsNullOrEmpty(x)) .Select(x => int.Parse(x)).ToArray();
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