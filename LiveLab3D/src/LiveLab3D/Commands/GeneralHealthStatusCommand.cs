namespace LiveLab3D.Commands
{
	public class GeneralHealthStatusCommand : VehicleCommandBase
	{
		public float ChargeStatus { get; set; }
		public float FlightTimeRemainingAtCurrentLoad { get; set; }
		public float RawMeasuredBatteryVoltage { get; set; }
		public float TotalCurrentDraw { get; set; }
		public float MaximumCommunicationLatency { get; set; }
		public int StatusBits { get; set; }
		public float EstimateFlightTimeRemainingAtHover { get; set; }
	}
}