namespace LiveLab3D.Screens.ConsoleModal.Commands
{
	using LiveLab3D.Visual.Cameras;

	public class ChangeCameraCommandInterpreter : IConsoleCommandInterpreter
	{
		private const string CommandPrefix = "camera";
		private readonly IEnvironment environment;
		private readonly ICameraSource source;

		public ChangeCameraCommandInterpreter(IEnvironment environment, ICameraSource cameraSource)
		{
			this.environment = environment;
			this.source = cameraSource;
		}

		#region IConsoleCommandInterpreter Members

		public bool CanInterpret(string text)
		{
			return text.StartsWith(CommandPrefix);
		}

		public void Interpret(string text)
		{
			string cameraName = text.Substring(CommandPrefix.Length + 1);
			if (!this.source.HasCamera(cameraName))
				return;
			ICamera camera = this.source.GetCamera(cameraName);
			this.environment.Camera = camera;
		}

		#endregion
	}
}