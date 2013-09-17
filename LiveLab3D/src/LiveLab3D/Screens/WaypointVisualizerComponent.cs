namespace LiveLab3D.Screens
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Content;
	using Microsoft.Xna.Framework.Graphics;

	public class WaypointVisualizerComponent : DrawableGameComponent
	{
		abstract class WaypointBase
		{
			public abstract Vector3 GetPoint();
		}
		class Waypoint:WaypointBase
		{
			private readonly Vector3 waypoint;

			public Waypoint(Vector3 waypoint)
			{
				this.waypoint = waypoint;
			}

			public override Vector3 GetPoint()
			{
				return waypoint;
			}
		}

		class FollowVehicleWapoint:WaypointBase
		{
			private readonly ObjectBase vehicle;

			public FollowVehicleWapoint(ObjectBase vehicle)
			{
				this.vehicle = vehicle;
			}

			public override Vector3 GetPoint()
			{
				return vehicle.PositionalData.Position;
			}
		}
		private static readonly Matrix world = Matrix.CreateTranslation(0, 0, 0);
		private readonly ContentManager contentManager;
		private readonly IEnvironment environment;
		private readonly IEventAggregator eventAggregator;
		private readonly object lockObject = new object();
		private readonly IDictionary<ObjectBase, IList<WaypointBase>> waypoints;
		private BasicEffect basicEffect;
		private Model sphere;
		private Matrix[] transforms;

		public WaypointVisualizerComponent(Game game, IEventAggregator eventAggregator, IEnvironment environment)
			: base(game)
		{
			this.eventAggregator = eventAggregator;
			this.environment = environment;

			this.waypoints = new Dictionary<ObjectBase, IList<WaypointBase>>(20);
			this.eventAggregator.Subscribe<CommandReceivedEvent<FlyToWaypointCommand>>(HandleWaypointCommand);
			this.eventAggregator.Subscribe<CommandReceivedEvent<FollowVehicleCommand>>(HandleFollowVehicleCommand);
			this.contentManager = new ContentManager(game.Services, "Content");
		}

		public override void Initialize()
		{
			base.Initialize();
			this.basicEffect = new BasicEffect(GraphicsDevice, null)
			                   	{
			                   		VertexColorEnabled = true,
			                   		World = world,
			                   		Projection =
			                   			Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), this.environment.AspectRatio,
			                   			                                    0.01f, 900)
			                   	};
			;
		}

		//Somebody says "There be dragons". Seems like there may be Dinasaurs.
		protected void EnsureInitialized(ObjectBase vehicle)
		{
			Monitor.Enter(this.lockObject);
			if (!this.waypoints.ContainsKey(vehicle))
				this.waypoints[vehicle] = new List<WaypointBase>(10);

			Monitor.Exit(this.lockObject);
		}

		protected override void LoadContent()
		{
			base.LoadContent();
			this.sphere = this.contentManager.Load<Model>("Models/sphere");
			this.transforms = new Matrix[this.sphere.Bones.Count];
			this.sphere.CopyAbsoluteBoneTransformsTo(this.transforms);
		}

		protected void HandleWaypointCommand(CommandReceivedEvent<FlyToWaypointCommand> @event)
		{
			EnsureInitialized(@event.Destination);
			FlyToWaypointCommand command = @event.Command;
			Monitor.Enter(this.lockObject);
			IList<WaypointBase> waypointList = this.waypoints[@event.Destination];
			var waypoint = new Vector3(command.X, command.Y, command.Z);

			if (@event.Command.Add == 1)
				waypointList.Clear();
            try
            {

                this.waypoints[@event.Destination].Add(new Waypoint(waypoint));
            }
            catch
            {

            }
			Monitor.Exit(this.lockObject);
		}

		protected void HandleFollowVehicleCommand(CommandReceivedEvent<FollowVehicleCommand> @event)
		{
			EnsureInitialized(@event.Destination);
			FollowVehicleCommand command = @event.Command;
			Monitor.Enter(this.lockObject);
			IList<WaypointBase> waypointList = this.waypoints[@event.Destination];
			var vehicle = this.environment.ObjectSource.GetObject(command.TargetId);

			if (@event.Command.AddRevise==1)
				waypointList.Clear();

			this.waypoints[@event.Destination].Add(new FollowVehicleWapoint(vehicle));
			Monitor.Exit(this.lockObject);
		}

		public override void Draw(GameTime gameTime)
		{
			Matrix view = this.environment.Camera.ViewMatrix;
			Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), this.environment.AspectRatio,
			                                                        0.01f, 900);
			var snapshot = new Dictionary<ObjectBase, Vector3[]>(10);
			Monitor.Enter(this.lockObject);
			foreach (var wp in this.waypoints)
			{
				snapshot[wp.Key] = wp.Value.Select(x=>x.GetPoint()).ToArray();
			}
			Monitor.Exit(this.lockObject);
			DrawTargetSpheres(snapshot, view, projection);
			DrawLines(snapshot, view, projection);

			base.Draw(gameTime);
		}


		protected void DrawLines(IDictionary<ObjectBase, Vector3[]> snapshot, Matrix view,
		                         Matrix projection)
		{
			GraphicsDevice.RenderState.PointSize = 2;
			GraphicsDevice.VertexDeclaration = new VertexDeclaration(GraphicsDevice, VertexPositionColor.VertexElements);
			this.basicEffect.Begin();
			this.basicEffect.View = this.environment.Camera.ViewMatrix;
			foreach (EffectPass pass in this.basicEffect.CurrentTechnique.Passes)
			{
				pass.Begin();
				foreach (var item in snapshot)
				{
					Vector3[] points = item.Value;
					var list = new List<VertexPositionColor>();
					list.Add(new VertexPositionColor(item.Key.PositionalData.Position, Color.Orange));

					foreach (Vector3 point in points)
					{
						var poscolor = new VertexPositionColor(point, Color.Orange);
						list.Add(poscolor);
						list.Add(poscolor);
					}
					VertexPositionColor[] pointArray = list.ToArray();
					if (pointArray.Length > 1)
						GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineList, pointArray.ToArray(), 0, points.Length);
				}

				pass.End();
			}
			this.basicEffect.End();
		}

		protected void DrawTargetSpheres(IDictionary<ObjectBase, Vector3[]> snapshot, Matrix view,
		                                 Matrix projection)
		{
			foreach (ModelMesh mesh in this.sphere.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.EnableDefaultLighting();
					effect.View = view;
					effect.Projection = projection;
				}
			}
			foreach (var item in snapshot)
			{
				foreach (Vector3 waypoint in item.Value)
				{
					Matrix sphereTranslation = Matrix.CreateTranslation(waypoint);
					foreach (ModelMesh mesh in this.sphere.Meshes)
					{
						foreach (BasicEffect effect in mesh.Effects)
							effect.World = this.transforms[mesh.ParentBone.Index]*sphereTranslation;
						mesh.Draw();
					}
				}
			}
		}
	}
}