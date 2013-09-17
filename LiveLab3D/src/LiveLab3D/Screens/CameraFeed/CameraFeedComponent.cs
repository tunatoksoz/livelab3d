using System.IO;

using Microsoft.Xna.Framework.Graphics;
using TomShane.Neoforce.Controls;
using System.Threading;

namespace LiveLab3D.Screens.CameraFeed
{
	public class CameraFeedComponent:ImageBox
	{
		private readonly ICameraImageCapturer capturer;
        private Timer timer;
		public CameraFeedComponent(Manager manager,ICameraImageCapturer capturer) : base(manager)
		{
			this.capturer = capturer;
            this.timer = new Timer(timer_Elapsed, null, 0, 20);
			this.SizeMode = TomShane.Neoforce.Controls.SizeMode.Stretched;
		}

		void timer_Elapsed(object state)
		{
            byte[] stream = this.capturer.GetLastFrame();
            if (stream.Length <= 0)
                return;
            Texture2D td = Texture2D.FromFile(this.Manager.GraphicsDevice, new MemoryStream(stream));
			this.Image = td;
		}

	}
}
