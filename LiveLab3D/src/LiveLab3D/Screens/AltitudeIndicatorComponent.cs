namespace LiveLab3D.Screens
{
	using System.Collections.Generic;
	using System.Linq;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class AltitudeIndicatorComponent : DrawableGameComponent
	{
		private readonly ContentManager contentManager;
		private readonly IEnvironment environment;
		private readonly IObjectSource objectSource;
		private readonly Matrix projection;
		private readonly Matrix world;
		private BasicEffect basicEffect;
		private SpriteBatch spriteBatch;
		private SpriteFont spriteFont;

		public AltitudeIndicatorComponent(Game game, IObjectSource objectSource, IEnvironment environment) : base(game)
		{
			this.environment = environment;
			this.contentManager = new ContentManager(game.Services, "Content");
			this.objectSource = objectSource;
			this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), environment.AspectRatio, 0.01f,
			                                                      900);
			this.world = Matrix.CreateTranslation(0, 0, 0);
			;
		}

		protected override void LoadContent()
		{
			this.spriteFont = this.contentManager.Load<SpriteFont>("Fonts/DefaultFont");
			this.spriteBatch = new SpriteBatch(GraphicsDevice);
			base.LoadContent();
		}

		public override void Initialize()
		{
			base.Initialize();
			this.basicEffect = new BasicEffect(GraphicsDevice, null);
			this.basicEffect.VertexColorEnabled = true;
			this.basicEffect.World = this.world;
			this.basicEffect.Projection = this.projection;
		}

		public override void Draw(GameTime gameTime)
		{
			IEnumerable<ObjectBase> objects = this.objectSource.GetObjects();


			Matrix view = this.environment.Camera.ViewMatrix;

			Vector3[] positions = objects.Select(x => x.PositionalData.Position).ToArray();
			this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
			                       SpriteSortMode.Immediate,
			                       SaveStateMode.SaveState);


			DrawAltitude(view, this.projection, this.world, positions);
			DrawLines(view, this.projection, this.world, positions);
			this.spriteBatch.End();
			base.Draw(gameTime);
		}

		protected void DrawAltitude(Matrix view, Matrix projection, Matrix world, Vector3[] positions)
		{
			foreach (Vector3 position in positions)
			{
				Vector3 to = position;
				var from = new Vector3(to.X, to.Y, 0);
				Vector3 middle = (from + to)/2;
				Vector3 screenSpaceMiddle = GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view,
				                                                            Matrix.CreateTranslation(middle));
				if (to.Z > 0.15)
					this.spriteBatch.DrawString(this.spriteFont, string.Format("{0:0.0}", to.Z),
					                            new Vector2(screenSpaceMiddle.X, screenSpaceMiddle.Y), Color.White);
			}
		}

		protected void DrawLines(Matrix view, Matrix projection, Matrix world, Vector3[] positions)
		{
			this.basicEffect.View = view;
			this.basicEffect.Begin();
			foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();
				foreach (Vector3 position in positions)
				{
					Vector3 to = position;
					var from = new Vector3(to.X, to.Y, 0);
					Vector3 screenSpaceFrom = GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view,
					                                                          Matrix.CreateTranslation(from));
					Vector3 screenSpaceTo = GraphicsDevice.Viewport.Project(Vector3.Zero, projection, view,
					                                                        Matrix.CreateTranslation(to));
					var pointList = new[]
					                	{
					                		new VertexPositionColor(screenSpaceFrom, Color.Green),
					                		new VertexPositionColor(screenSpaceTo, Color.Green)
					                	};
					GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, pointList, 0, 2, new[] {0, 1}, 0, 1);
				}
				pass.End();
			}
			this.basicEffect.End();
		}
	}
}