namespace LiveLab3D.Events
{
	using System;

	public interface IEventAggregator
	{
		void Publish(EventBase @event);
		void Publish<TEvent>(TEvent @event) where TEvent : EventBase;
		void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : EventBase;
		void Subscribe<TEvent>(Action<TEvent> eventHandler) where TEvent : EventBase;
		void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler, Predicate<TEvent> condition) where TEvent : EventBase;
		void Subscribe<TEvent>(Action<TEvent> eventHandler, Predicate<TEvent> condition) where TEvent : EventBase;
	}
}