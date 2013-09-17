namespace LiveLab3D.Windsor
{
	using System;
	using System.IO;
	using Castle.Core;
	using Castle.MicroKernel;
	using Castle.Windsor;
	using LiveLab3D.Visual.ObjectVisuals;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class ModelSubdependencyResolver : ISubDependencyResolver
	{
		private readonly IWindsorContainer container;

		public ModelSubdependencyResolver(IWindsorContainer container)
		{
			this.container = container;
		}

		#region ISubDependencyResolver Members

		public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
		                       ComponentModel model,
		                       DependencyModel dependency)
		{
			Type interfaceType = model.Implementation.GetInterface(typeof (IObjectVisual<>).Name);
			return interfaceType != null && typeof (Model).IsAssignableFrom(dependency.TargetType);
		}

		public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver,
		                      ComponentModel model,
		                      DependencyModel dependency)
		{
			Type interfaceType = model.Implementation.GetInterface(typeof (IObjectVisual<>).Name);
			Type objectType = interfaceType.GetGenericArguments()[0];

			return LoadModel("Models/" + objectType.Name.ToLower());
		}

		#endregion

		//THERE BE DRAGONS
		private Model LoadModel(string path)
		{
			var game = this.container.Resolve<Game>();
			var contentManager = new ContentManager(game.Services, "Content");
			string r = Guid.NewGuid().ToString();
			string destinationAsset = path + r;
			string sourceAsset = path;
			string source = contentManager.RootDirectory + "/" + sourceAsset + ".xnb";
			string dest = contentManager.RootDirectory + "/" + destinationAsset + ".xnb";
			File.Copy(source, dest);
			var model = contentManager.Load<Model>(destinationAsset);
			File.Delete(dest);
			return model;
		}
	}
}