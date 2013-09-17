namespace LiveLab3D.Screens
{
	using Microsoft.Xna.Framework;

	public abstract class Screen : DrawableGameComponent
	{
		protected Screen(Game game) : base(game)
		{
		}

		public virtual object Result { get; set; }

		public abstract override void Draw(GameTime gameTime);
		public abstract override void Update(GameTime gameTime);
	}
}