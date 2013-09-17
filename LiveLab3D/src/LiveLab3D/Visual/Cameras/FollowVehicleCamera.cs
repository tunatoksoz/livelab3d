namespace LiveLab3D.Visual.Cameras
{
	using System;
	using System.Collections.Generic;
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Input;

	public class FollowVehicleCamera : ICamera
	{
		private readonly IDictionary<Keys, Action<GameTime>> keyHandlers;
		private readonly int midX;
		private readonly int midY;

		private readonly ObjectBase vehicle;
		private float angleFromVertical;


		private Vector3 cameraPosition;
		private Vector3 cameraTarget;
		private Vector3 cameraUp;
		private float length;
		private MouseState previousMouseState;
		private Vector3 relativeCameraPosition;
		private float yaw;

		public FollowVehicleCamera(IEnvironment environment, ObjectBase vehicle)
		{
			this.midX = environment.Width/2;
			this.midY = environment.Height/2;

			this.vehicle = vehicle;

			Mouse.SetPosition(this.midX, this.midY);
			this.previousMouseState = Mouse.GetState();
			this.keyHandlers = new Dictionary<Keys, Action<GameTime>>
			                   	{
			                   		{Keys.W, HandleForward},
			                   		{Keys.S, HandleBackward},
			                   	};

			this.length = 4.00f;
			this.yaw = 0;
			this.angleFromVertical = -MathHelper.PiOver4;
		}

		#region ICamera Members

		public void UpdateCamera(GameTime gameTime)
		{
			if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
				foreach (Keys key in this.keyHandlers.Keys)
					if (Keyboard.GetState().IsKeyDown(key))
						this.keyHandlers[key](gameTime);

			var deltaTime = (float) gameTime.ElapsedGameTime.TotalSeconds;
			MouseState mState = Mouse.GetState();
			int mouseX = mState.X - this.previousMouseState.X;
			int mouseY = mState.Y - this.previousMouseState.Y;
			this.previousMouseState = mState;

			this.yaw -= (mouseX*0.2f)*deltaTime;

			this.angleFromVertical += (mouseY*0.2f)*deltaTime;
			this.angleFromVertical = MathHelper.Clamp(this.angleFromVertical, -MathHelper.PiOver2 + 0.2f,
			                                          +MathHelper.PiOver2 - 0.2f);
			Vector3 unitX = Vector3.UnitX;
			Vector3 unitY = Vector3.UnitY;
			Vector3 unitZ = Vector3.UnitZ;

			unitX = Vector3.Transform(unitX, Matrix.CreateRotationZ(this.yaw));
			unitY = Vector3.Cross(unitZ, unitX);
			unitX = Vector3.Transform(unitX, Matrix.CreateFromAxisAngle(unitY, this.angleFromVertical));
			this.relativeCameraPosition = unitX*this.length;
			this.cameraPosition = this.vehicle.PositionalData.Position + this.relativeCameraPosition;
			this.cameraTarget = this.vehicle.PositionalData.Position;
			this.cameraUp = unitZ;
			ViewMatrix = Matrix.CreateLookAt(this.cameraPosition, this.cameraTarget, this.cameraUp);
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

		private void HandleForward(GameTime gameTime)
		{
			this.length -= 0.003f*gameTime.ElapsedGameTime.Milliseconds;
		}

		private void HandleBackward(GameTime gameTime)
		{
			this.length += 0.003f*gameTime.ElapsedGameTime.Milliseconds;
		}
	}
}