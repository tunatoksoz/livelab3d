namespace LiveLab3D.Objects
{
	using System.Collections.Generic;
	using Castle.Windsor;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Visual.ObjectVisuals;

	public class SimpleModelSource : IModelSource
	{
		private readonly IDictionary<ObjectBase, IObjectVisual> visuals;
		private readonly IWindsorContainer windsorContainer;

		public SimpleModelSource(IWindsorContainer container)
		{
			this.windsorContainer = container;
			this.visuals = new Dictionary<ObjectBase, IObjectVisual>();
		}

		#region IModelSource Members

		public IObjectVisual GetModelFor<T>(T obj) where T : ObjectBase
		{
			if (!this.visuals.ContainsKey(obj))
				this.visuals[obj] = this.windsorContainer.Resolve(typeof (IObjectVisual<>).MakeGenericType(obj.GetType()),
				                                                  new
				                                                  	{
				                                                  		item = obj,
				                                                  	}) as IObjectVisual;
			return this.visuals[obj];
		}

		#endregion
	}
}