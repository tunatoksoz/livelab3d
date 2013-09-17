namespace LiveLab3D.Screens
{
	using System.Collections.Generic;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class FuelLevelIndicatorComponent : DrawableGameComponent
	{
		private readonly IDictionary<ObjectBase, float> batteryLevels;
		private readonly IEnvironment environment;
		private readonly IEventAggregator eventAggregator;
		private readonly IObjectSource objectSource;
		private readonly Matrix projection;
		private Texture2D mHealthBar;
		private SpriteBatch spriteBatch;

		public FuelLevelIndicatorComponent(Game game, IObjectSource objectSource, IEnvironment environment,
		                                   IEventAggregator eventAggregator)
			: base(game)
		{
			this.objectSource = objectSource;
			this.environment = environment;
			this.eventAggregator = eventAggregator;
			this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), environment.AspectRatio, 0.01f,
			                                                      900);
			this.eventAggregator.Subscribe<CommandReceivedEvent<GeneralHealthStatusCommand>>(Handle);
			this.batteryLevels = new Dictionary<ObjectBase, float>();
		}

		private void Handle(CommandReceivedEvent<GeneralHealthStatusCommand> @event)
		{
			this.batteryLevels[@event.Destination] = @event.Command.ChargeStatus;
		}

		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(GraphicsDevice);
			var aLoader = new ContentManager(Game.Services);
			this.mHealthBar = CreateRectangle(45, 10);
		}

		private Texture2D CreateRectangle(int width, int height)
		{
			var rectangleTexture = new Texture2D(GraphicsDevice, width, height, 1, TextureUsage.None,
			                                     SurfaceFormat.Color);
			var color = new Color[width*height];
			for (int i = 0; i < color.Length; i++)
				color[i] = new Color(255, 255, 255, 255);
			rectangleTexture.SetData(color);
			return rectangleTexture;
		}

		public override void Draw(GameTime gameTime)
		{
			IEnumerable<ObjectBase> objects = this.objectSource.GetObjects();
			this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
			                       SpriteSortMode.Immediate,
			                       SaveStateMode.SaveState);

			int i = 0;
			foreach (ObjectBase vehicle in objects)
			{
				i++;
				float level;
				if (this.batteryLevels.ContainsKey(vehicle))
				{
					level = this.batteryLevels[vehicle];
                    if (level > 1.0)
                        level = 1.0f;
                    if (level <= 0.0)
                        level = 0.00f;
					Vector3 to = vehicle.PositionalData.Position;
					Vector3 twodpos = GraphicsDevice.Viewport.Project(Vector3.Zero, this.projection, this.environment.Camera.ViewMatrix,
					                                                  Matrix.CreateTranslation(to));
					this.spriteBatch.Draw(this.mHealthBar,
					                      new Rectangle((int) twodpos.X - this.mHealthBar.Width/2,
					                                    (int) twodpos.Y + 25 - this.mHealthBar.Height/2,
					                                    (this.mHealthBar.Width), this.mHealthBar.Height), null, Color.Black);
					this.spriteBatch.Draw(this.mHealthBar,
					                      new Rectangle((int) twodpos.X - this.mHealthBar.Width/2,
					                                    (int) twodpos.Y + 25 - this.mHealthBar.Height/2,
					                                    (int) (this.mHealthBar.Width*level), this.mHealthBar.Height), null,
					                      GetColor(level));
				}
			}

			this.spriteBatch.End();
			base.Draw(gameTime);
		}

		protected Color GetColor(float level)
		{
			if (level > 0.75f)
				return Color.LightGreen;
			else if (level > 0.5f)
				return Color.YellowGreen;
			else if (level > 0.25f)
				return Color.Yellow;
			else
				return Color.Red;
		}
	}
}