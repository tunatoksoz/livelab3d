namespace LiveLab3D.Objects
{
	using LiveLab3D.Commands;

	public interface ICommandHandler
	{
		void Handle(VehicleCommandBase command);
	}

	public interface ICommandHandler<TCommand> : ICommandHandler
		where TCommand : VehicleCommandBase
	{
		void Handle(TCommand command);
	}
}