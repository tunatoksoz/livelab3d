namespace LiveLab3D.Statistics.Visualization
{
	using System.Linq;
	using Microsoft.Xna.Framework;
	using TomShane.Neoforce.Controls;

	public class Plot : Control
	{
		public Plot(Manager manager)
			: base(manager)
		{
			Data = new Point[] {};
		}

		public Point[] Data { get; set; }


		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			CheckLayer(Skin, "Control");
			CheckLayer(Skin, "Graph");
			base.DrawControl(renderer, rect, gameTime);
			if (Data.Length <= 0)
				return;
			SkinLayer layer = Skin.Layers["Control"];
			SkinLayer layer2 = Skin.Layers["Graph"];
			var rectangle = new Rectangle(rect.Left + 2*layer.ContentMargins.Left, rect.Top + 2*layer.ContentMargins.Top,
			                              rect.Width - 2*layer.ContentMargins.Vertical,
			                              rect.Height - 2*layer.ContentMargins.Horizontal);
			int rectangleWidth = rect.Width - layer.ContentMargins.Vertical;
			int rectangleHeight = rect.Height - layer.ContentMargins.Horizontal;
			int numberOfPoints = Data.Length;
			float xlow = Data[0].X;
			float xhigh = Data[numberOfPoints - 1].X;
			float ylow = Data.Min(d => d.Y);
			float yhigh = Data.Max(d => d.Y);
			float xrange = xhigh - xlow;
			float yrange = yhigh - ylow;
			float xstep = xrange/rectangleWidth;
			float ystep = yrange/rectangleHeight;
			int currentXindex = 0;
			for (int i = 0; i < rectangleWidth; i++)
			{
				float xreal = xstep*i + xlow;
				if (numberOfPoints > 1 && xreal >= Data[currentXindex + 1].X)
					currentXindex++;

				float pieceXleft = Data[currentXindex].X;
				float pieceXright = Data[currentXindex + 1].X;
				float pieceYleft = Data[currentXindex].Y;
				float pieceYright = Data[currentXindex + 1].Y;
				float pieceslope = (pieceYright - pieceYleft)/
				                   (pieceXright - pieceXleft);

				float ycorresponding = pieceslope*(xreal - pieceXleft) + pieceYleft;
				float graphHeight = (ycorresponding - ylow)/ystep;
				var rectangle2 = new Rectangle(rectangle.Left + i, rectangle.Top + rectangleHeight - (int) graphHeight, 1,
				                               (int) graphHeight);
				renderer.DrawLayer(this, layer2, rectangle2);
			}
		}

		#region Nested type: Point

		public class Point
		{
			public Point(float x, float y)
			{
				X = x;
				Y = y;
			}

			public float X { get; set; }
			public float Y { get; set; }
		}

		#endregion
	}
}