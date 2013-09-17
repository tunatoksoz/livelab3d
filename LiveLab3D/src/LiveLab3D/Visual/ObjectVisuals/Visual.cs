namespace LiveLab3D.Visual.ObjectVisuals
{
	using System;
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class Visual<T> : IObjectVisual<T> where T : ObjectBase
	{
		private readonly T item;
		private readonly Model model;
		private readonly IDrawingPipeline pipeline;
		private readonly Matrix[] transforms;

		public Visual(T item, Model model, IDrawingPipelineRegistry pipelineRegistry)
		{
			this.item = item;
			this.model = model;
			this.transforms = new Matrix[Model.Bones.Count];
			this.model.CopyAbsoluteBoneTransformsTo(this.transforms);
			this.pipeline = pipelineRegistry.GetPipeline("ObjectVisual");
			foreach (ModelMesh mesh in this.model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();
				}
			}
		}

		#region IObjectVisual<T> Members

		public T Object
		{
			get { return this.item; }
		}

		public Model Model
		{
			get { return this.model; }
		}


		ObjectBase IObjectVisual.Object
		{
			get { return this.item; }
		}

		public virtual void Draw(Matrix view, Matrix projection)
		{
			DrawVehicle(view, projection);
		}

		#endregion

		protected virtual void DrawVehicle(Matrix view, Matrix projection)
		{
			IDisposable processResult = this.pipeline.Process(this.model,item);

			foreach (ModelMesh mesh in this.model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = this.transforms[mesh.ParentBone.Index]*Object.ThreeDPositionMatrix;
					effect.View = view;
					effect.Projection = projection;
				}
				mesh.Draw();
			}
			processResult.Dispose();
		}
	}
}