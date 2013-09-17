namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 20020)]
	public class GeneralHealthStatusCommandParser : ReflectionCommandParser<GeneralHealthStatusCommand>
	{
		public GeneralHealthStatusCommandParser()
			: base(
				command => command.ChargeStatus,
				command => command.FlightTimeRemainingAtCurrentLoad,
				command => command.RawMeasuredBatteryVoltage,
				command => command.TotalCurrentDraw,
				command => command.MaximumCommunicationLatency,
				command => command.StatusBits,
				command => command.EstimateFlightTimeRemainingAtHover)
		{
		}
	}
}