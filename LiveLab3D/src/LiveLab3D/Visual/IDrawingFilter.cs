namespace LiveLab3D.Visual
{
	using System;
	using Microsoft.Xna.Framework.Graphics;

	public interface IDrawingFilter
	{
		void Modify(Model model,object additionalData);
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class FilterKey : Attribute
	{
		public string Key { get; set; }
	}
}