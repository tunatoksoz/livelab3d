namespace LiveLab3D.Visual.CommandTextifiers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	public class AssemblyCommandTextifierRegistry : ICommandTextifierRegistry
	{
		private readonly IDictionary<Type, Type> commandTextifiers;

		public AssemblyCommandTextifierRegistry(Assembly assembly)
		{
			this.commandTextifiers = new Dictionary<Type, Type>();
			Type[] types = assembly.GetTypes()
				.Where(x => !x.IsInterface && !x.IsAbstract)
				.Where(type => typeof (ICommandTextifier).IsAssignableFrom(type))
				.ToArray();
			foreach (Type item in types)
			{
				Type representing = item.GetInterfaces()
					.Where(x => x.IsGenericType &&
					            typeof (ICommandTextifier<>)
					            	.MakeGenericType(x.GetGenericArguments()[0]).IsAssignableFrom(x))
					.Select(x => x.GetGenericArguments()[0]).FirstOrDefault();
				this.commandTextifiers[representing] = item;
			}
		}

		#region ICommandTextifierRegistry Members

		public ICommandTextifier GetTextifierForCommand(Type commandType)
		{
			if (this.commandTextifiers.ContainsKey(commandType))
				return Activator.CreateInstance(this.commandTextifiers[commandType]) as ICommandTextifier;
			return new NullCommandTextifier();
		}

		#endregion
	}
}