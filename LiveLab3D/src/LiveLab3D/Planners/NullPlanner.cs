namespace LiveLab3D.Planners
{
	using System;
	using LiveLab3D.Objects;
	using LiveLab3D.Simulation;

	public class NullPlanner : IPlanner
	{
		private IObjectController objectController;
		private ObjectBase vehicle;
		private Control control;
		public NullPlanner(IObjectController controller, ObjectBase vehicle, Control initialControl)
		{
			this.objectController = controller;
			this.vehicle = vehicle;
			this.control = initialControl;
		}
		public void Start()
		{
			objectController.Initialize(vehicle);
		}

		public void Stop()
		{

		}

		public void SimulateMotion(TimeSpan timeSpan)
		{
			this.control = objectController.UpdateControl(control);
			var position = objectController.UpdatePosition(timeSpan, vehicle.PositionalData, control);
			vehicle.PositionalData = position;
		}
	}
}