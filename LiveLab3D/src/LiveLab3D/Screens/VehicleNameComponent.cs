namespace LiveLab3D.Screens
{
	using System.Collections.Generic;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class VehicleNameComponent : DrawableGameComponent
	{
		private readonly ContentManager contentManager;
		private readonly IEnvironment environment;
		private readonly IObjectSource objectSource;
		private readonly Matrix projection;
		private SpriteBatch spriteBatch;
		private SpriteFont spriteFont;

		public VehicleNameComponent(Game game, IObjectSource objectSource, IEnvironment environment) : base(game)
		{
			this.objectSource = objectSource;
			this.environment = environment;
			this.contentManager = new ContentManager(game.Services, "Content");
			this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), environment.AspectRatio, 0.01f,
			                                                      900);
		}

		protected override void LoadContent()
		{
			this.spriteFont = this.contentManager.Load<SpriteFont>("Fonts/DefaultFont");
			this.spriteBatch = new SpriteBatch(GraphicsDevice);
			base.LoadContent();
		}

		public override void Draw(GameTime gameTime)
		{
			IEnumerable<ObjectBase> objects = this.objectSource.GetObjects();

			this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
			                       SpriteSortMode.Immediate,
			                       SaveStateMode.SaveState);
			Matrix view = this.environment.Camera.ViewMatrix;
			foreach (ObjectBase vehicle in objects)
			{
				Vector3 screenSpace = GraphicsDevice.Viewport.Project(Vector3.Zero, this.projection, view,
				                                                      Matrix.CreateTranslation(vehicle.PositionalData.Position));
				this.spriteBatch.DrawString(this.spriteFont, vehicle.Name, new Vector2(screenSpace.X, screenSpace.Y), Color.Red);
			}
			this.spriteBatch.End();


			base.Draw(gameTime);
		}
	}
}