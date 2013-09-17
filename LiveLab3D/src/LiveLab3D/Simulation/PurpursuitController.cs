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

	public class PurpursuitControllerControl : GpuccControl
	{
		public float Eta;
		public float Psi;
		public float RefX;
		public float RefY;
		public float Velocity;
	}

	public class PurpursuitController : IObjectController<Gpucc, PurpursuitControllerControl>
	{
		private const float Lw = .26f;
		private const float L_dir = .2f;
		private const float l_dir = .03f;
		private const float VEH_RADIUS = .17f;

		private readonly Queue<Waypoint> waypoints;
		private Vector3 lastWaypoint;
		private ObjectBase objectBase;

		public PurpursuitController(ObjectBase objectBase)
		{
			this.objectBase = objectBase;
			this.waypoints = new Queue<Waypoint>();
			var wp = new Waypoint();
			wp.X = 0;
			wp.Y = 0;
			wp.Psi = 0; // not needed for PP
			wp.Vel = 0.0F; // not needed for PP
			wp.V_cmd = 0.0F;
			wp.L1 = 0.0F;
			wp.Dir = 1.0F;
			this.waypoints.Enqueue(wp);
		}

		#region IObjectController<Gpucc,PurpursuitControllerControl> Members

		public Control UpdateControl(Control oldControl)
		{
			return UpdateControl((PurpursuitControllerControl) oldControl);
		}

		public MotionalData UpdatePosition(TimeSpan elapsedTime, MotionalData old, Control control)
		{
			return UpdatePosition(elapsedTime, old, (PurpursuitControllerControl) control);
		}

		public PurpursuitControllerControl UpdateControl(PurpursuitControllerControl oldControl)
		{
			float yaw = this.objectBase.PositionalData.Yaw;

			var pos = new Waypoint();
			pos.X = this.objectBase.PositionalData.Position.X;
			pos.Y = this.objectBase.PositionalData.Position.Y;
			pos.Psi = yaw;
			pos.Vel = this.objectBase.PositionalData.Velocity.Length();
			Waypoint goal = this.waypoints.Peek();
			var goalVector = new Vector3(goal.X, goal.Y, 0);
			if (goalVector != this.lastWaypoint)
			{
				EventAggregatorImpl.Instance.Publish(new HeadingToWaypointEvent
				                                     	{
				                                     		Waypoint = new Events.Impl.Waypoint {Point = goalVector},
				                                     		Vehicle = this.objectBase
				                                     	});
				this.lastWaypoint = goalVector;
			}
			float dx = goal.X - oldControl.RefX;
			float dy = goal.Y - oldControl.RefY;
			if (Math.Sqrt(dx*dx + dy*dy) < 0.05)
			{
				if (this.waypoints.Count > 1)
					// remove the current goal
					this.waypoints.Dequeue();
				else
				{
					// replace the current goal with current location
					goal.X = pos.X;
					goal.Y = pos.Y;
					goal.Psi = pos.Psi;
					goal.Vel = 0.0f;
					goal.V_cmd = 0.0f;
					goal.L1 = 0.0f;
					goal.Dir = 1.0f;
				}
			}

			PurpursuitControllerControl controls = ReferenceGenerator(oldControl, pos, this.waypoints.Peek());
			var deltaV2 =
				(float) (controls.Velocity*Lw*Math.Sin(controls.Eta)/(goal.L1 + 2*l_dir*Math.Cos(controls.Eta)));
			float vR = controls.Velocity - deltaV2;
			float vL = controls.Velocity + deltaV2;
			vR = MathHelper.Clamp(vR, -0.3f, 0.3f);
			vL = MathHelper.Clamp(vL, -0.3f, 0.3f);
			controls.Vl = vL;
			controls.Vr = vR;
			return controls;
		}

		public MotionalData UpdatePosition(TimeSpan elapsedTime, MotionalData old, PurpursuitControllerControl control)
		{
			float vR = control.Vr;
			float vL = control.Vl;
			float yaw = old.Yaw;
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
			       		Roll = old.Roll,                        
			       	};
		}

		public void Initialize(Gpucc item)
		{
			this.objectBase = item;
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
			FlyToWaypointCommand flyToWaypointCommand = command;
			var waypoint = new Waypoint
			               	{
			               		X = flyToWaypointCommand.X,
			               		Y = flyToWaypointCommand.Y,
			               		Psi = 0f,
			               		Vel = flyToWaypointCommand.VelocityToWaypoint,
			               		L1 = 0.1f,
			               		V_cmd = 2.8f,
			               		Dir = 1,
			               	};
			this.waypoints.Enqueue(waypoint);
		}

		private PurpursuitControllerControl ReferenceGenerator(PurpursuitControllerControl oldControls, Waypoint pos,
		                                                       Waypoint goal)
		{
			PurpursuitControllerControl controls = oldControls;
			// Unit vector from reference to waypoint
			float veh_x, veh_y;
			var ref_to_wp = new Vector2(goal.X - controls.RefX, goal.Y - controls.RefY);
			if (ref_to_wp.Length() != 0)
				ref_to_wp.Normalize();
			// Extract vehicle state
			// (note that for GPUCCs wheel_base is 0)

			if (goal.Dir > 0.5)
			{
				veh_x = pos.X + l_dir*(float) Math.Sin(pos.Psi);
				veh_y = pos.Y + l_dir*(float) Math.Cos(pos.Psi);
			}
			else
			{
				veh_x = pos.X - l_dir*(float) Math.Sin(pos.Psi);
				veh_y = pos.Y - l_dir*(float) Math.Cos(pos.Psi);
			}
			var veh_pos = new Vector2(veh_x, veh_y);

			// Find projection of vehicle onto line from reference to waypoint
			float dotp = (veh_pos.X - controls.RefX)*ref_to_wp.X + (veh_pos.Y - controls.RefY)*ref_to_wp.Y;
			var veh_proj = new Vector2(controls.RefX + ref_to_wp.X*dotp, controls.RefY + ref_to_wp.Y*dotp);

			// Compute distance between vehicle and projection
			var veh_to_proj = new Vector2(veh_proj.X - veh_pos.X, veh_proj.Y - veh_pos.Y);
			float dist_proj = veh_to_proj.Length();


			// Compute how far in front of projection point L1 point must be
			float dist_wp;
			if (dist_proj > goal.L1)
			{
				// too far away; will keep L1 point at projection
				dist_wp = 0.0f;
			}
			else
			{
				dist_wp = (float) Math.Sqrt(goal.L1*goal.L1 - dist_proj*dist_proj);
			}
			//printf("********************* x: %f, y: %f, dist to ref %f, dist proj %f\n\n",x0-xP,y0-yP,dist_to_ref,dist_proj);

			// Compute L1 point
			var L1_pos = new Vector2(veh_proj.X + ref_to_wp.X*dist_wp, veh_proj.Y + ref_to_wp.Y*dist_wp);

			// Compute eta (difference between vehicle heading and L1 heading)
			controls.Eta = (-pos.Psi +
			                MathHelper.WrapAngle(MathHelper.PiOver2 -
			                                     (float) Math.Atan2(L1_pos.Y - veh_pos.Y, L1_pos.X - veh_pos.X)));
			//if (goal.dir < 0.5) {
			//  ctl->eta = ctl->eta - PI;
			//}
			//printf("************* L1_pos = (%f, %f), veh_pos = (%f,%f)\n", L1_pos.x, L1_pos.y, veh_pos.x, veh_pos.y);
			//printf("************* -pos.psi %f\t atan2 %f\t pi/2-atan2 %f\n\n", -pos.psi, atan2(L1_pos.y-veh_pos.y,L1_pos.x-veh_pos.x), PI/2.0-atan2(L1_pos.y-veh_pos.y,L1_pos.x-veh_pos.x));

			// Determine whether reference has reached/passed waypoint; if so, snap to it
			var L1_to_wp = new Vector2(goal.X - L1_pos.X, goal.Y - L1_pos.Y);
			L1_to_wp.Normalize();
			if (Vector2.Dot(L1_to_wp, ref_to_wp) <= 0)
			{
				controls.RefX = goal.X;
				controls.RefY = goal.Y;
			}
			else
			{
				controls.RefX = L1_pos.X;
				controls.RefY = L1_pos.Y;
			}

			controls.Velocity = goal.V_cmd;
			if (goal.Dir < 0.5)
			{
				controls.Velocity = -controls.Velocity;
			}
			return controls;
		}
	}
}