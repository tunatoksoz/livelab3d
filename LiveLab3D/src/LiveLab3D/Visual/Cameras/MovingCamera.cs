namespace LiveLab3D.Visual.Cameras
{
	using System;
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Input;

	public class MovingCamera : ICamera
	{
		private readonly GraphicsDeviceManager graphicsDeviceManager;
		private readonly IDictionary<Keys, Action<GameTime>> keyHandlers;
		private readonly MouseState mouseStateInitial;


		private Vector3 cameraPosition;
		private Vector3 cameraTarget;
		private Vector3 cameraUp;
		private float cumPitch;
		private Vector3 xAxis;
		private Vector3 yAxis;
		private Vector3 zAxis;

		public MovingCamera(GraphicsDeviceManager graphicsDeviceManager, Vector3 initialPosition, Vector3 initialTarget)
		{
			this.graphicsDeviceManager = graphicsDeviceManager;
			Mouse.SetPosition(graphicsDeviceManager.GraphicsDevice.Viewport.Width/2,
			                  graphicsDeviceManager.GraphicsDevice.Viewport.Height/2);
			this.mouseStateInitial = Mouse.GetState();
			this.cameraPosition = initialPosition;
			this.cameraTarget = initialTarget;
			this.xAxis = Vector3.UnitX;
			this.yAxis = Vector3.UnitY;
			this.zAxis = Vector3.UnitZ;
			this.keyHandlers = new Dictionary<Keys, Action<GameTime>>
			                   	{
			                   		{Keys.W, HandleForward},
			                   		{Keys.S, HandleBackward},
			                   		{Keys.D, HandleRight},
			                   		{Keys.A, HandleLeft},
			                   	};
		}

		#region ICamera Members

		public void UpdateCamera(GameTime gameTime)
		{
			var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
			MouseState mState = Mouse.GetState();
			int mouseX = mState.X - this.mouseStateInitial.X;
			int mouseY = mState.Y - this.mouseStateInitial.Y;
			Mouse.SetPosition(this.graphicsDeviceManager.GraphicsDevice.Viewport.Width/2,
			                  this.graphicsDeviceManager.GraphicsDevice.Viewport.Height/2);

			bool orientationChanged = false;
			foreach (Keys item in this.keyHandlers.Keys)
			{
				if (Keyboard.GetState().IsKeyDown(item))
				{
					orientationChanged = true;
					this.keyHandlers[item](gameTime);
				}
			}
			if (mouseX != 0 || mouseY != 0)
				orientationChanged = true;
			if (orientationChanged)
			{
				float cameraPitch = (mouseY*0.2f)*deltaTime;
				float cameraYaw = -(mouseX*0.2f)*deltaTime;
				float newCumPitch = this.cumPitch + cameraPitch;
				if (newCumPitch >= Math.PI/2 || newCumPitch <= -Math.PI/2)
					cameraPitch = 0;
				this.cumPitch += cameraPitch;

				Matrix rotation = Matrix.CreateFromAxisAngle(this.xAxis, cameraPitch)*Matrix.CreateRotationZ(cameraYaw);

				this.xAxis = Vector3.Transform(this.xAxis, rotation);
				this.yAxis = Vector3.Transform(this.yAxis, rotation);
				this.zAxis = Vector3.Transform(this.zAxis, rotation);

				this.cameraTarget = this.cameraPosition - this.yAxis;
				this.cameraUp = this.zAxis;

				ViewMatrix = Matrix.CreateLookAt(this.cameraPosition, this.cameraTarget, this.cameraUp);
			}
		}

		public CameraOrientation GetCameraOrientation()
		{
			return new CameraOrientation
			       	{
			       		Target = this.cameraTarget,
			       		Position = this.cameraPosition,
			       		Up = this.cameraUp
			       	};
		}

		public Matrix ViewMatrix { get; private set; }

		#endregion

		private void HandleMove(GameTime gameTime, Vector3 vector)
		{
			this.cameraPosition += 0.003f*gameTime.ElapsedGameTime.Milliseconds*vector;
			this.cameraTarget += 0.003f*gameTime.ElapsedGameTime.Milliseconds*-vector;
		}


		private void HandleForward(GameTime gameTime)
		{
			HandleMove(gameTime, -this.yAxis);
		}

		private void HandleBackward(GameTime gameTime)
		{
			HandleMove(gameTime, this.yAxis);
		}

		private void HandleRight(GameTime gameTime)
		{
			HandleMove(gameTime, -this.xAxis);
		}

		private void HandleLeft(GameTime gameTime)
		{
			HandleMove(gameTime, this.xAxis);
		}
	}
}