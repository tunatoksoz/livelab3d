namespace LiveLab3D.Objects
{
	using LiveLab3D.Commands;

	public abstract class GenericCommandHandlerBase<TCommand> : ICommandHandler<TCommand>
		where TCommand : VehicleCommandBase
	{
		#region ICommandHandler<TCommand> Members

		public abstract void Handle(TCommand command);

		public virtual void Handle(VehicleCommandBase command)
		{
			Handle((TCommand) command);
		}

		#endregion
	}
}