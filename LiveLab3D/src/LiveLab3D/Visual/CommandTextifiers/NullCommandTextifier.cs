namespace LiveLab3D.Visual.CommandTextifiers
{
	using LiveLab3D.Commands;

	public class NullCommandTextifier : ICommandTextifier<VehicleCommandBase>
	{
		#region ICommandTextifier<VehicleCommandBase> Members

		public string ToText(VehicleCommandBase command, string vehicleName)
		{
			return "";
		}

		#endregion
	}
}