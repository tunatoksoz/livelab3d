namespace LiveLab3D.ObjectSources
{
	using LiveLab3D.Objects;

	public interface IObjectBuilder
	{
		ObjectBase BuildObject(string name);
	}
}