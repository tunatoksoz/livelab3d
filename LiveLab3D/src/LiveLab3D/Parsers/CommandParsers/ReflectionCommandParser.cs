namespace LiveLab3D.Parsers.CommandParsers
{
	using System;
	using System.Linq.Expressions;
	using System.Reflection;
	using LiveLab3D.Commands;

	public abstract class ReflectionCommandParser<T> :
		IVehicleCommandParser<T> where T : VehicleCommandBase
	{
		private readonly Expression<Func<T, object>>[] props;

		public ReflectionCommandParser(params Expression<Func<T, object>>[] props)
		{
			this.props = props;
		}

		#region IVehicleCommandParser<T> Members

		public T Parse(string command)
		{
			string[] parts = command.Split(' ');
			int i = 0;
			var item = Activator.CreateInstance<T>();
			foreach (var prop in this.props)
			{
				var operand = ((UnaryExpression) prop.Body).Operand as MemberExpression;
				var member = operand.Member as PropertyInfo;
				Type memberType = member.PropertyType;
				try
				{
					//TODO: WTF?
					if (memberType == typeof (bool))
						member.SetValue(item, bool.Parse(parts[i++]), null);
					else
						member.SetValue(item, Convert.ChangeType(parts[i++], memberType), null);
				}
				catch
				{
				}
			}
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