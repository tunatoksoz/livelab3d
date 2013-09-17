namespace LiveLab3D.ObjectSources
{
	using LiveLab3D.Objects;
	using LiveLab3D.Visual.ObjectVisuals;

	public interface IModelSource
	{
		IObjectVisual GetModelFor<T>(T obj) where T : ObjectBase;
	}
}