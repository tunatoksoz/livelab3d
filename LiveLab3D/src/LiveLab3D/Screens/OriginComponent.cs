namespace LiveLab3D.Screens
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public class OriginComponent : DrawableGameComponent
	{
		private readonly VertexPositionColor[] endPoints;
		private readonly IEnvironment environment;
		private BasicEffect basicEffect;

		public OriginComponent(Game game, IEnvironment environment)
			: base(game)
		{
			this.environment = environment;
			this.endPoints = new[]
			                 	{
			                 		new VertexPositionColor(Vector3.Zero + Vector3.UnitZ*0.01f, Color.Black),
			                 		new VertexPositionColor(0.3f*Vector3.UnitX + Vector3.UnitZ*0.01f, Color.Black),
			                 		new VertexPositionColor(0.3f*Vector3.UnitY + Vector3.UnitZ*0.01f, Color.Black),
			                 		new VertexPositionColor(0.3f*Vector3.UnitZ, Color.Black),
			                 	};
		}

		public override void Initialize()
		{
			base.Initialize();
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), this.environment.AspectRatio,
			                                                        0.01f, 900);
			this.basicEffect = new BasicEffect(GraphicsDevice, null)
			                   	{
			                   		VertexColorEnabled = true,
			                   		World = Matrix.CreateTranslation(0, 0, 0),
			                   		Projection = projection
			                   	};
		}

		public override void Draw(GameTime gameTime)
		{
			GraphicsDevice.RenderState.PointSize = 5;
			GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
			this.basicEffect.View = this.environment.Camera.ViewMatrix;
			this.basicEffect.Begin();
			foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();
				GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, this.endPoints, 0, 4,
				                                         new[] {0, 1, 0, 2, 0, 3}, 0, 3);
				pass.End();
			}
			this.basicEffect.End();
		}
	}
}