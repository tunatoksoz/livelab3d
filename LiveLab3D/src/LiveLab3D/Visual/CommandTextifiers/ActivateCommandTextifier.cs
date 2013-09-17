namespace LiveLab3D.Visual.CommandTextifiers
{
	using LiveLab3D.Commands;

	public class ActivateCommandTextifier : CommandTextifierBase<ActivateCommand>
	{
		public override string ToText(ActivateCommand command, string vehicleName)
		{
			return string.Format("Activating {0}", vehicleName);
			//return string.Format("Activating vehicle {0} at X={1:0.##} and Y={2:0.##}", vehicleName, command.X, command.Y);
		}
	}
}