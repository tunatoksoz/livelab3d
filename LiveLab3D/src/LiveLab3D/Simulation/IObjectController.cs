namespace LiveLab3D.Simulation
{
	using System;
	using LiveLab3D.Objects;

	public interface IObjectController
	{
		void Initialize(ObjectBase item);
		Control UpdateControl(Control oldControl);
		MotionalData UpdatePosition(TimeSpan elapsedTime, MotionalData old, Control control);
	}

	public interface IObjectController<TObject, TControl> : IObjectController
		where TControl : Control
		where TObject : ObjectBase
	{
		void Initialize(TObject item);
		TControl UpdateControl(TControl oldControl);
		MotionalData UpdatePosition(TimeSpan elapsedTime, MotionalData old, TControl control);
	}

	public abstract class Control
	{
	}
}