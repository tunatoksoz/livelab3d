namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 10001)]
	public class FlyToWaypointCommandParser : ReflectionCommandParser<FlyToWaypointCommand>
	{
		public FlyToWaypointCommandParser()
			: base(cmd => cmd.Add,
			       cmd => cmd.X,
			       cmd => cmd.Y,
			       cmd => cmd.Z,
			       cmd => cmd.VehicleHeadingDuringTransit,
			       cmd => cmd.VelocityToWaypoint,
			       cmd => cmd.TimeToArrival)
		{
		}
	}
}