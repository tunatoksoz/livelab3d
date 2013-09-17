namespace LiveLab3D.Parsers.CommandParsers
{
	using LiveLab3D.Commands;

	[Parses(CommandNumber = 1001)]
	public class ActivateCommandParser : ReflectionCommandParser<ActivateCommand>
	{
		public ActivateCommandParser()
			: base(cmd => cmd.X,
			       cmd => cmd.Y,
			       cmd => cmd.Delay,
			       cmd => cmd.BatterySafetyCheck,
			       cmd => cmd.CommunicationSafetyCheck,
			       cmd => cmd.HealthDownload,
			       cmd => cmd.GyroFeedback,
			       cmd => cmd.DebugDataAsHealthDownload)
		{
		}
	}
}