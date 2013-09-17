namespace LiveLab3D.ObjectSources
{
	using System;
	using System.Collections.Generic;
	using LiveLab3D.Objects;

	public class DefaultObjectBuilder : IObjectBuilder
	{
		private static readonly IList<FuncBuilderPair> pairs;

		static DefaultObjectBuilder()
		{
			pairs = new List<FuncBuilderPair>();
			pairs.Add(new FuncBuilderPair
			          	{
			          		Predicate = (x) => x.Contains("GP"),
			          		ObjectType = typeof (Gpucc)
			          	});
			pairs.Add(new FuncBuilderPair
			          	{
			          		Predicate = (x) => x.Contains("Q"),
			          		ObjectType = typeof (Quadrotor)
			          	});
			pairs.Add(new FuncBuilderPair
			          	{
			          		Predicate = (x) => x.Contains("C"),
			          		ObjectType = typeof (Truck)
			          	});
			pairs.Add(new FuncBuilderPair
			          	{
			          		Predicate = (x) => true,
			          		ObjectType = typeof (Quadrotor),
			          	});
		}

		#region IObjectBuilder Members

		public ObjectBase BuildObject(string name)
		{
			foreach (FuncBuilderPair item in pairs)
			{
				if (item.Predicate(name))
					return (ObjectBase) Activator.CreateInstance(item.ObjectType);
			}
			return null;
		}

		#endregion

		#region Nested type: FuncBuilderPair

		private class FuncBuilderPair
		{
			public Func<string, bool> Predicate { get; set; }
			public Type ObjectType { get; set; }
		}

		#endregion
	}
}