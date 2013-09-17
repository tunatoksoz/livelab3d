namespace LiveLab3D.Visual
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class DrawingPipeline : IDrawingPipeline
	{
		private readonly IDictionary<Model, Disposable> disposables;
		private readonly IDrawingFilter[] filters;
		private readonly GraphicsDevice graphicsDevice;
		private readonly string key;

		public DrawingPipeline(Game game, string key, IDrawingFilter[] filters)
		{
			this.filters = filters;
			this.graphicsDevice = game.GraphicsDevice;
			this.key = key;
			this.disposables = new Dictionary<Model, Disposable>();
		}

		#region IDrawingPipeline Members

		public string Key
		{
			get { return this.key; }
		}

		public IDrawingFilter[] Filters
		{
			get { return this.filters; }
		}

		public IDisposable Process(Model model,object data)
		{
			foreach (IDrawingFilter drawingFilter in this.filters)
				drawingFilter.Modify(model,data);
			return GetDisposableForModel(model);
		}

		#endregion

		protected IDisposable GetDisposableForModel(Model model)
		{
			if (!this.disposables.ContainsKey(model))
				this.disposables[model] = new Disposable(model, this.graphicsDevice);
			return this.disposables[model];
		}

		#region Nested type: Disposable

		private class Disposable : IDisposable
		{
			private readonly IDictionary<ModelMeshPart, Effect> meshPartEffect;
			private readonly Model model;

			public Disposable(Model model, GraphicsDevice graphicsDevice)
			{
				this.meshPartEffect = new Dictionary<ModelMeshPart, Effect>();
				this.model = model;
				foreach (ModelMesh mesh in model.Meshes)
					foreach (ModelMeshPart part in mesh.MeshParts)
						this.meshPartEffect[part] = part.Effect.Clone(graphicsDevice);
			}

			#region IDisposable Members

			public void Dispose()
			{
				foreach (ModelMesh mesh in this.model.Meshes)
					foreach (ModelMeshPart part in mesh.MeshParts)
						part.Effect = this.meshPartEffect[part];
			}

			#endregion
		}

		#endregion
	}
}