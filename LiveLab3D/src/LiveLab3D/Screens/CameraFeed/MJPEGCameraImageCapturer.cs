using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using AForge.Video;
using Microsoft.Xna.Framework.Graphics;
using TomShane.Neoforce.Controls;

namespace LiveLab3D.Screens.CameraFeed
{
	public class MJPEGCameraImageCapturer : ICameraImageCapturer
	{
		
		private byte[] data;
		private MJPEGStream mjpegStream;
		public MJPEGCameraImageCapturer(string uri)
			:this(uri,"","")
		{
		}
		public MJPEGCameraImageCapturer(string uri,string username,string password)
		{
			data = new byte[0];
			mjpegStream = new MJPEGStream(uri);
			mjpegStream.Login = username;
			mjpegStream.Password = password;
			mjpegStream.NewFrame += (sender, e) =>
			{
				MemoryStream ms = new MemoryStream();
				e.Frame.Save(ms, ImageFormat.Png);
				data = ms.GetBuffer();
			};
		}

		public void Start()
		{
			mjpegStream.Start();		
		}
		public byte[] GetLastFrame()
		{
			return data;
		}
	}
}
