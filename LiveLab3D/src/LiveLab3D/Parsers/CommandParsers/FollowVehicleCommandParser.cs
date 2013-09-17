namespace LiveLab3D.Parsers.CommandParsers
{
	using Commands;

	[Parses(CommandNumber = 10008)]
	public class FollowVehicleCommandParser : ReflectionCommandParser<FollowVehicleCommand>
	{
		public FollowVehicleCommandParser()
			: base(cmd => cmd.AddRevise,
				   cmd => cmd.TargetId,
				   cmd => cmd.VehicleOffsetX,
				   cmd => cmd.VehicleOffsetY,
				   cmd => cmd.VehicleOffsetZ,
				   cmd => cmd.FollowingTime)
		{
		}
	}
}
