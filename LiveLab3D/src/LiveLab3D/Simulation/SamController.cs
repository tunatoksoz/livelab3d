namespace LiveLab3D.Simulation
{
	using System;
	using System.Collections.Generic;
	using LiveLab3D.Commands;
	using LiveLab3D.Events;
	using LiveLab3D.Events.Impl;
	using LiveLab3D.Helpers;
	using LiveLab3D.Objects;
	using Microsoft.Xna.Framework;

	public class SamController : IObjectController<Gpucc, GpuccControl>
	{
		private readonly Queue<Waypoint> waypoints;
		private Vector3 lastWaypoint;
		private ObjectBase objectBase;

		public SamController()
		{
			this.waypoints = new Queue<Waypoint>();
		}

		#region IObjectController<Gpucc,GpuccControl> Members

		public GpuccControl UpdateControl(GpuccControl oldControl)
		{
			if (this.waypoints.Count == 0)
				return new GpuccControl {Vl = 0, Vr = 0};
			Waypoint wp = this.waypoints.Peek();
			Vector3 pos = this.objectBase.PositionalData.Position;
			var goal = wp.Position;
			if (goal != this.lastWaypoint)
			{
				EventAggregatorImpl.Instance.Publish(new HeadingToWaypointEvent
				                                     	{
				                                     		Waypoint = new Events.Impl.Waypoint {Point = goal},
				                                     		Vehicle = this.objectBase
				                                     	});
				this.lastWaypoint = goal;
			}
			float vL, vR;
			if ((pos - goal).Length() <= 0.05)
			{
				this.waypoints.Dequeue();
				if (this.waypoints.Count == 0)
					return new GpuccControl {Vl = 0, Vr = 0};
				wp = this.waypoints.Peek();
				goal = wp.Position;
			}

			float yaw = this.objectBase.PositionalData.Yaw;
			// get position error

			float pos_err = (goal - pos).Length();
			// Compute desired heading
			//double th_ref = atan2(goal.y - pos.y, goal.x - pos.x);
			var th_ref = (float) Math.Atan2(goal.X - pos.X, goal.Y - pos.Y);

			//% Compute heading error and check for wrap-around
			float th_err = MathHelper.WrapAngle(th_ref - yaw);

			int sign_vfd = 1;
			if (Math.Abs(th_err) <= MathHelper.PiOver2)
			{
				sign_vfd = 1;
			}
			else
			{
				// % Rather move backwards
				if (th_err > MathHelper.PiOver2)
					th_err = th_err - MathHelper.Pi;
				else if (th_err < -MathHelper.PiOver2)
					th_err = th_err + MathHelper.Pi;

				sign_vfd = -1;
			}

			// saturation limit
			float sat_limit = 0.3f;
			float pos_tol = .05f;

			// % Simple proportional heading controller (inner loop -- faster gain)
			float vth = MathHelper.Clamp(10*th_err, -sat_limit, sat_limit);

			// % Simple proportional range controller (outer loop -- slower gain)
			float vfd = MathHelper.Clamp(sign_vfd*2*pos_err, -sat_limit, sat_limit);

			//% If "too close" to goal and heading error is "large", then turn only
			if (pos_err < pos_tol)
			{
				vL = 0;
				vR = 0;
			}
			else
			{
				//% Check if controls are within allowable range
				if (Math.Abs(vfd + vth) > sat_limit)
				{
					vfd = ((MathHelper.PiOver2 - Math.Abs(th_err))/(MathHelper.PiOver2))*vfd;
					vth = ((Math.Abs(th_err))/(MathHelper.PiOver2))*vth;
				}

				// % Transform to actuators
				vL = (vfd + vth);
				vR = (vfd - vth);
			}
			return new GpuccControl {Vl = vL, Vr = vR};
		}

		public MotionalData UpdatePosition(TimeSpan elapsedTime, MotionalData old, GpuccControl control)
		{
			float vR = control.Vr;
			float vL = control.Vl;
			float yaw = MathHelper.PiOver2 - old.Yaw;
			float base_v = (vR + vL)/2f;
			float delta_v = vR - vL;
			Vector3 pos = old.Position;
			var velocity = new Vector3
			               	{
			               		X = base_v*Math.Cos(yaw).ToFloat(),
			               		Y = base_v*Math.Sin(yaw).ToFloat()
			               	};

			yaw += (float) elapsedTime.TotalSeconds*delta_v/0.26f;
			pos += (float) elapsedTime.TotalSeconds*velocity;

			return new MotionalData
			       	{
			       		Pitch = old.Pitch,
			       		Position = pos,
			       		Velocity = velocity,
			       		Yaw = MathHelper.WrapAngle(MathHelper.PiOver2 - yaw),
			       		Roll = old.Roll
			       	};
		}

		public Control UpdateControl(Control oldControl)
		{
			return UpdateControl((GpuccControl) oldControl);
		}

		public MotionalData UpdatePosition(TimeSpan elapsedTime, MotionalData old, Control control)
		{
			return UpdatePosition(elapsedTime, old, (GpuccControl) control);
		}

		public void Initialize(Gpucc objectBase)
		{
			this.objectBase = objectBase;
			EventAggregatorImpl.Instance.Subscribe<CommandReceivedEvent<FlyToWaypointCommand>>(x => Handle(x.Command),
			                                                                                   x => x.Destination == this.objectBase);
		}

		public void Initialize(ObjectBase item)
		{
			Initialize((Gpucc) item);
		}

		#endregion

		protected void Handle(FlyToWaypointCommand command)
		{
			var waypoint = new Waypoint
			               	{
			               		X = command.X,
			               		Y = command.Y,
			               		Psi = 0f,
			               		Vel = command.VelocityToWaypoint,
			               		L1 = 0.1f,
			               		V_cmd = 0.24f,
			               		Dir = 1,
			               	};
			if(command.Add==1)
				this.waypoints.Clear();
			this.waypoints.Enqueue(waypoint);
		}
	}
}