namespace LiveLab3D.Screens
{
	using System.Collections.Generic;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Visual.ObjectVisuals;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class VisualizationComponent : DrawableGameComponent
	{
		private readonly ContentManager contentManager;
		private readonly IEnvironment environment;
		private readonly IModelSource modelSource;
		private readonly Matrix projection;
		private Matrix[] transforms;
		private Model world;

		public VisualizationComponent(LiveLab game, IEnvironment environment, IModelSource modelSource) : base(game)
		{
			this.environment = environment;
			this.modelSource = modelSource;
			this.contentManager = new ContentManager(game.Services, "Content");
			this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f),
			                                                      game.GraphicsDevice.Viewport.Width/
			                                                      (float) game.GraphicsDevice.Viewport.Height, 0.01f, 900);
		}

		protected override void LoadContent()
		{
			this.world = this.contentManager.Load<Model>("Models/lab");
			this.transforms = new Matrix[this.world.Bones.Count];
			this.world.CopyAbsoluteBoneTransformsTo(this.transforms);

			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			DrawWorld(this.environment.Camera.ViewMatrix, gameTime);
			DrawVehicles(this.environment.Camera.ViewMatrix, gameTime);
		}

		protected void DrawWorld(Matrix view, GameTime gameTime)
		{
			foreach (ModelMesh mesh in this.world.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();
					effect.World = this.transforms[mesh.ParentBone.Index];
					effect.View = view;
					effect.Projection = this.projection;
				}
				mesh.Draw();
			}
		}

		protected void DrawVehicles(Matrix view, GameTime gameTime)
		{
			IEnumerable<ObjectBase> objects = this.environment.ObjectSource.GetObjects();
			foreach (ObjectBase o in objects)
			{
				IObjectVisual visual = this.modelSource.GetModelFor(o);
				visual.Draw(view, this.projection);
			}
		}
	}
}