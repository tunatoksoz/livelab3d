using Microsoft.Xna.Framework;

namespace LiveLab3D.Visual.Filters
{
	using Microsoft.Xna.Framework.Graphics;

	[FilterKey(Key = "ObjectVisual")]
	public class VehicleStateFilter : IDrawingFilter
	{
		#region IDrawingFilter Members

		public void Modify(Model model,object vehicle)
		{
			foreach (var mesh in model.Meshes)
				if ("Status".Equals(mesh.Name))
					foreach (BasicEffect effect in mesh.Effects)
						effect.FogColor= new Vector3(45f,234f,234f)*0.6f;
		}

		#endregion
	}
}