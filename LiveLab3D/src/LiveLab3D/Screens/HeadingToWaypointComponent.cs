namespace LiveLab3D.Screens
{
	using System.Collections.Generic;
	using System.Linq;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class HeadingToWaypointComponent : DrawableGameComponent
	{
		private static readonly Matrix world = Matrix.CreateTranslation(0, 0, 0);
		private readonly ContentManager contentManager;
		private readonly IEnvironment environment;
		private readonly IEventAggregator eventAggregator;
		private readonly GraphicsDevice graphicsDevice;
		private readonly IDictionary<ObjectBase, HeadingToWaypointEvent> waypoints;
		private BasicEffect basicEffect;
		private Matrix projection;
		private Model sphere;
		private Matrix[] transforms;

		public HeadingToWaypointComponent(Game game, IEventAggregator eventAggregator, IEnvironment environment)
			: base(game)
		{
			this.eventAggregator = eventAggregator;
			this.environment = environment;

			this.waypoints = new Dictionary<ObjectBase, HeadingToWaypointEvent>();
			this.eventAggregator.Subscribe<HeadingToWaypointEvent>(HandleWaypointCommand);
			this.contentManager = new ContentManager(game.Services, "Content");
			this.graphicsDevice = game.GraphicsDevice;
		}

		public override void Initialize()
		{
			base.Initialize();
			this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), this.environment.AspectRatio,
			                                                      0.01f, 900);
			this.basicEffect = new BasicEffect(GraphicsDevice, null)
			                   	{VertexColorEnabled = true, World = world, Projection = this.projection};
		}

		protected override void LoadContent()
		{
			base.LoadContent();
			this.sphere = this.contentManager.Load<Model>("Models/sphere");
			this.transforms = new Matrix[this.sphere.Bones.Count];
			this.sphere.CopyAbsoluteBoneTransformsTo(this.transforms);
		}

		protected void HandleWaypointCommand(HeadingToWaypointEvent @event)
		{
			this.waypoints[@event.Vehicle] = @event;
		}

		public override void Draw(GameTime gameTime)
		{
			Matrix view = this.environment.Camera.ViewMatrix;
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), this.environment.AspectRatio,
			                                                        0.01f, 900);
			List<KeyValuePair<ObjectBase, HeadingToWaypointEvent>> items = this.waypoints.ToList();
			foreach (var wpd in items)
			{
				DrawTargetSphere(wpd.Value, view, projection);
				DrawLine(wpd.Value, view, projection);
			}
			base.Draw(gameTime);
		}


		protected void DrawLine(HeadingToWaypointEvent @event, Matrix view, Matrix projection)
		{
			Waypoint waypoint = @event.Waypoint;
			ObjectBase item = @event.Vehicle;
			Vector3 targetVector = waypoint.Point;
			Vector3 sourceVector = item.PositionalData.Position;
			this.graphicsDevice.RenderState.PointSize = 2;
			var pointList = new[]
			                	{
			                		new VertexPositionColor(sourceVector, Color.Green),
			                		new VertexPositionColor(targetVector, Color.Green)
			                	};
			this.graphicsDevice.VertexDeclaration = new VertexDeclaration(this.graphicsDevice, VertexPositionColor.VertexElements);
			this.basicEffect.View = this.environment.Camera.ViewMatrix;
			this.basicEffect.Begin();
			foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();
				this.graphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, pointList, 0, 2, new[] {0, 1}, 0, 1);
				pass.End();
			}
			this.basicEffect.End();
		}

		protected void DrawTargetSphere(HeadingToWaypointEvent @event, Matrix view, Matrix projection)
		{
			Waypoint destination = @event.Waypoint;
			Matrix sphereTranslation = Matrix.CreateTranslation(destination.Point.X, destination.Point.Y, destination.Point.Z);
			foreach (ModelMesh mesh in this.sphere.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.DiffuseColor = Color.Green.ToVector3();
					effect.EnableDefaultLighting();
					effect.World = this.transforms[mesh.ParentBone.Index]*sphereTranslation;
					effect.View = view;
					effect.Projection = projection;
				}
				mesh.Draw();
			}
		}
	}
}