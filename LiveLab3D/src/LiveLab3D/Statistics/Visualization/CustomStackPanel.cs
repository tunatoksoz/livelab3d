namespace LiveLab3D.Statistics.Visualization
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using TomShane.Neoforce.Controls;

	public class CustomStackPanel : Container
	{
		private Orientation orientation;

		public CustomStackPanel(Manager manager, Orientation orientation) : base(manager)
		{
			this.orientation = orientation;
			Color = Color.TransparentWhite;
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			CalcLayout();
			base.OnResize(e);
		}

		private void CalcLayout()
		{
			int top = Left;
			int left = Left;
			int width = Width;
			int height = Height;
			foreach (Control control in ClientArea.Controls)
			{
				Margins margins = control.Margins;
				if (this.orientation == Orientation.Horizontal)
				{
					if (margins.Left + control.Width + margins.Right + left > Width)
					{
						left = Left;
						top += left + margins.Top + control.Height + margins.Bottom;
					}
					left += margins.Left;

					control.Left = left;
					left += control.Width;
					left += margins.Right;
					control.Top = top;
					height = control.Top + control.Height + margins.Top;
				}
			}
			Width = width;
			Height = height;
		}


		protected override void DrawControl(Renderer renderer, Rectangle rect, GameTime gameTime)
		{
			base.DrawControl(renderer, rect, gameTime);
		}
	}
}