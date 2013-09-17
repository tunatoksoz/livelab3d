namespace LiveLab3D.Visual.ObjectVisuals
{
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;

	public interface IObjectVisual
	{
		Model Model { get; }
		ObjectBase Object { get; }
		void Draw(Matrix view, Matrix projection);
	}

	public interface IObjectVisual<T> : IObjectVisual where T : ObjectBase
	{
		new T Object { get; }
	}
}