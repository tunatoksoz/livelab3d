namespace LiveLab3D.Screens
{
	using System;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class FrameRateCounter : DrawableGameComponent
	{
		private readonly ContentManager content;
		private TimeSpan elapsedTime = TimeSpan.Zero;
		private int frameCounter;
		private int frameRate;
		private SpriteBatch spriteBatch;
		private SpriteFont spriteFont;


		public FrameRateCounter(Game game)
			: base(game)
		{
			this.content = new ContentManager(game.Services);
			this.content.RootDirectory = "Content";
		}


		protected override void LoadContent()
		{
			this.spriteBatch = new SpriteBatch(GraphicsDevice);
			this.spriteFont = this.content.Load<SpriteFont>("Fonts/Lucida Console");
			base.LoadContent();
		}

		protected override void UnloadContent()
		{
			this.content.Unload();
		}


		public override void Update(GameTime gameTime)
		{
			this.elapsedTime += gameTime.ElapsedGameTime;

			if (this.elapsedTime > TimeSpan.FromSeconds(1))
			{
				this.elapsedTime -= TimeSpan.FromSeconds(1);
				this.frameRate = this.frameCounter;
				this.frameCounter = 0;
			}
		}


		public override void Draw(GameTime gameTime)
		{
			this.frameCounter++;

			string fps = string.Format("FPS: {0}", this.frameRate);

			this.spriteBatch.Begin(SpriteBlendMode.AlphaBlend,
			                       SpriteSortMode.Immediate,
			                       SaveStateMode.SaveState);
			this.spriteBatch.DrawString(this.spriteFont, fps, new Vector2(33, 33), Color.Black);
			this.spriteBatch.DrawString(this.spriteFont, fps, new Vector2(32, 32), Color.White);
			this.spriteBatch.End();
		}
	}
}