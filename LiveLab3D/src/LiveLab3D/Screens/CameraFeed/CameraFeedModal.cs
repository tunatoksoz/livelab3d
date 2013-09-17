using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiveLab3D.Statistics.Visualization;
using TomShane.Neoforce.Controls;

namespace LiveLab3D.Screens.CameraFeed
{
	public class CameraFeedModal:Window
	{
		private readonly ICameraImageCapturer[] cameraImageCapturers;
		private readonly CustomStackPanel stackPanel;
		public CameraFeedModal(Manager manager,ICameraImageCapturer[] cameraImageCapturers) : base(manager)
		{
			this.cameraImageCapturers = cameraImageCapturers;
			Alpha = 200;
			Text = "Camera Feeds";
			this.stackPanel = new CustomStackPanel(Manager, Orientation.Horizontal);
			this.stackPanel.Parent = this;
			this.stackPanel.Init();
			this.stackPanel.Left = 5;
			this.stackPanel.Top = 10;
			this.stackPanel.Height = this.Height-4*stackPanel.Top;
			this.stackPanel.Width = this.Width - 4*stackPanel.Left; ;

			this.stackPanel.Parent = this;
			foreach(var capturer in cameraImageCapturers)
			{
                capturer.Start();
				var com = new CameraFeedComponent(manager,capturer);
				com.Parent = this.stackPanel;
				com.Width = 160;
				com.Height = 120;
				com.Init();

				this.stackPanel.Add(com);
				
			}
			this.stackPanel.Init();
			
		}

	}
}
