namespace LiveLab3D.Visual.CommandTextifiers
{
	using LiveLab3D.Commands;

	public interface ICommandTextifier
	{
		string ToText(VehicleCommandBase command, string vehicleName);
	}

	public interface ICommandTextifier<T> : ICommandTextifier where T : VehicleCommandBase
	{
		string ToText(T command, string vehicleName);
	}

	public abstract class CommandTextifierBase<T> : ICommandTextifier<T> where T : VehicleCommandBase
	{
		#region ICommandTextifier<T> Members

		public abstract string ToText(T command, string vehicleName);

		public string ToText(VehicleCommandBase command, string vehicleName)
		{
			return ToText((T) command, vehicleName);
		}

		#endregion
	}
}