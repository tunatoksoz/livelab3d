namespace LiveLab3D.ObjectSources
{
	using System.Collections.Generic;
	using LiveLab3D.Commands;
	using LiveLab3D.Objects;

	public delegate void CommandReceivedHandler(ObjectBase vehicle, VehicleCommandBase command);

	public interface IObjectSource
	{
		IEnumerable<ObjectBase> GetObjects();
		ObjectBase GetObject(int id);
		ObjectBase GetObject(string name);
	}
}