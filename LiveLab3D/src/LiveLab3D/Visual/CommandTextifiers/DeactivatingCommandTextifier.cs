namespace LiveLab3D.Visual.CommandTextifiers
{
	using LiveLab3D.Commands;

	public class DeactivatingCommandTextifier : CommandTextifierBase<DeactivateCommand>
	{
		public override string ToText(DeactivateCommand command, string vehicleName)
		{
			return string.Format("Deactivating {0}", vehicleName);
		}
	}
}