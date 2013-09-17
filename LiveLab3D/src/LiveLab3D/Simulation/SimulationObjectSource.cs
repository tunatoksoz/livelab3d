namespace LiveLab3D.Simulation
{
	using System;
	using System.Collections.Generic;
	using LiveLab3D.Objects;
	using LiveLab3D.ObjectSources;
	using LiveLab3D.Planners;
	using Microsoft.Xna.Framework;

	public class VehicleControllerControl
	{
		public ObjectBase Vehicle { get; set; }
		public IObjectController Controller { get; set; }
		public Control Control { get; set; }
	}

	public class VehiclePlanner
	{
		public ObjectBase Vehicle { get; set; }
		public IPlanner Planner { get; set; }
	}

	public class SimulationObjectSource : IObjectSource
	{
		private readonly IDictionary<int, ObjectBase> objectsById;
		private readonly IDictionary<string, ObjectBase> objectsByName;
		private readonly IList<VehiclePlanner> pairs;

		public SimulationObjectSource(LiveLab liveLab)
		{
			this.objectsById = new Dictionary<int, ObjectBase>();
			this.objectsByName = new Dictionary<string, ObjectBase>();
			this.pairs = new List<VehiclePlanner>(100);
			liveLab.GameUpdated += UpdateObjects;
		}

		#region IObjectSource Members

		public IEnumerable<ObjectBase> GetObjects()
		{
			return this.objectsById.Values;
		}

		public ObjectBase GetObject(int id)
		{
			return this.objectsById.ContainsKey(id) ? this.objectsById[id] : null;
		}

		public ObjectBase GetObject(string name)
		{
			return this.objectsByName.ContainsKey(name) ? this.objectsByName[name] : null;
		}

		#endregion

		public event CommandReceivedHandler CommandReceived = delegate { };

		public void AddObject(ObjectBase objectBase, IObjectController controller, Control initialControl)

		{
			var vcc = new VehicleControllerControl
			          	{Vehicle = objectBase, Controller = controller, Control = initialControl};
			AddObject(vcc);
		}

		public void AddObject<TObject, TObjectController, TControl>(TObject objectBase, TObjectController controller,
		                                                            TControl initialControl)
			where TObject : ObjectBase
			where TObjectController : IObjectController<TObject, TControl>
			where TControl : Control
		{
			AddObject((ObjectBase) objectBase, (IObjectController) controller, (Control) initialControl);
		}

		public void AddObjects(IEnumerable<VehicleControllerControl> triples)
		{
			foreach (VehicleControllerControl triple in triples)
			{

				AddObject(triple);
			}
		}

		public void AddObject(VehicleControllerControl triple)
		{
			triple.Controller.Initialize(triple.Vehicle);
			this.AddObject(triple.Vehicle,new NullPlanner(triple.Controller,triple.Vehicle,triple.Control));
		}


		public void AddObject(ObjectBase objectBase, IPlanner planner)
		{
			AddObject(new VehiclePlanner { Planner = planner, Vehicle = objectBase });
		}


		public void AddObject<TObject>(TObject objectBase,IPlanner planner) where TObject:ObjectBase
		{
			AddObject((ObjectBase)objectBase,planner);
		}


		public void AddObjects(IEnumerable<VehiclePlanner> vehiclePlanners)
		{
			foreach (var vehiclePlanner in vehiclePlanners)
			{
				AddObject(vehiclePlanner);
			}
		}

		public void AddObject(VehiclePlanner vehiclePlanner)
		{
			this.objectsById[vehiclePlanner.Vehicle.Id] = vehiclePlanner.Vehicle;
			this.objectsByName[vehiclePlanner.Vehicle.Name] = vehiclePlanner.Vehicle;
			pairs.Add(vehiclePlanner);

		}



		public void UpdateObjects(TimeSpan timeSpan)
		{
			foreach (VehiclePlanner vehiclePlanner in this.pairs)
			{
				vehiclePlanner.Planner.SimulateMotion(timeSpan);
			}
		}
	}
}