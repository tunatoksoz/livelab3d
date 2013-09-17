namespace LiveLab3D.Events
{
	public interface IEventHandler
	{
		void Handle(EventBase @event);
	}

	public interface IEventHandler<TEvent> : IEventHandler where TEvent : EventBase
	{
		void Handle(TEvent @event);
	}
}