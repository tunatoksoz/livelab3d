namespace LiveLab3D.Visual.CommandTextifiers
{
	using System;

	public interface ICommandTextifierRegistry
	{
		ICommandTextifier GetTextifierForCommand(Type commandType);
	}
}