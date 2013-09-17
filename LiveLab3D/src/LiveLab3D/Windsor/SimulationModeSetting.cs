namespace LiveLab3D.Windsor
{
	using System.Collections.Generic;
	using LiveLab3D.Simulation;

	public class SimulationModeSetting : IModeSetting
	{
		public IEnumerable<VehicleControllerControl> Vehicles { get; set; }
	}
}