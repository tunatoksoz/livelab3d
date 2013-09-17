namespace LiveLab3D.Visual
{
	using System;
	using Microsoft.Xna.Framework.Graphics;

	public interface IDrawingPipeline
	{
		string Key { get; }
		IDrawingFilter[] Filters { get; }
		IDisposable Process(Model model,object data);
	}
}