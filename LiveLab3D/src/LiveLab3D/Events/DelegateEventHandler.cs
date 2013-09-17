namespace LiveLab3D.Events
{
	using System;

	public class DelegateEventHandler<TEvent> : IEventHandler<TEvent> where TEvent : EventBase
	{
		private readonly Action<TEvent> eventHandler;

		public DelegateEventHandler(Action<TEvent> eventHandler)
		{
			this.eventHandler = eventHandler;
		}

		#region IEventHandler<TEvent> Members

		public void Handle(TEvent @event)
		{
			this.eventHandler(@event);
		}

		public void Handle(EventBase @event)
		{
			this.eventHandler((TEvent) @event);
		}

		#endregion
	}
}