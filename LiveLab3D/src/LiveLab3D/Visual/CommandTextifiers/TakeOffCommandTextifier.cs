namespace LiveLab3D.Visual.CommandTextifiers
{
	using LiveLab3D.Commands;

	public class TakeOffCommandTextifier : CommandTextifierBase<TakeOffCommand>
	{
		public override string ToText(TakeOffCommand command, string vehicleName)
		{
			return string.Format("{0} receives take off", vehicleName);
		}
	}
}