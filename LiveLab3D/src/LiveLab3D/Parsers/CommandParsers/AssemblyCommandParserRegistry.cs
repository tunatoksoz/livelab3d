namespace LiveLab3D.Parsers.CommandParsers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class AssemblyCommandParserRegistry : ICommandParserRegistry
	{
		private readonly IDictionary<int, Type> commandParsers;

		public AssemblyCommandParserRegistry(Assembly assembly)
		{
			this.commandParsers = GetParserClasses(assembly);
		}

		#region ICommandParserRegistry Members

		public IVehicleCommandParser GetParserForCommandNumber(int number)
		{
			if (!this.commandParsers.ContainsKey(number))
				return new NullCommandParser();
			Type t = this.commandParsers[number];

			return (IVehicleCommandParser) Activator.CreateInstance(t);
		}

		#endregion

		protected IDictionary<int, Type> GetParserClasses(Assembly asm)
		{
			var dict = new Dictionary<int, Type>();
			List<Type> types = asm.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract)
				.ToList();
			foreach (Type item in types)
			{
				if (typeof (IVehicleCommandParser).IsAssignableFrom(item))
				{
					var attributes = (ParsesAttribute[]) item.GetCustomAttributes(typeof (ParsesAttribute), false);
					ParsesAttribute attribute = attributes[0];
					dict[attribute.CommandNumber] = item;
				}
			}
			return dict;
		}
	}
}