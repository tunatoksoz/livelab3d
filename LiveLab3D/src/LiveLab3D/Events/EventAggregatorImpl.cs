namespace LiveLab3D.Events
{
	using System;
	using System.Collections.Generic;

	public class EventAggregatorImpl : IEventAggregator
	{
		private static readonly EventAggregatorImpl instance;

		private readonly IDictionary<Type, IList<EventHandlerPredicatePair>> eventHandlers;

		static EventAggregatorImpl()
		{
			instance = new EventAggregatorImpl();
		}

		public EventAggregatorImpl()
		{
			this.eventHandlers = new Dictionary<Type, IList<EventHandlerPredicatePair>>();
		}

		public static EventAggregatorImpl Instance
		{
			get { return instance; }
		}

		#region IEventAggregator Members

		public void Publish(EventBase @event)
		{
			EnsureInitialized(@event.GetType());
			var types=new List<Type>();
			Type t = @event.GetType();
			while((typeof(EventBase).IsAssignableFrom(t)))
			{
				types.Add(t);
				t = t.BaseType;
			}
			var eventHandlers = new List<EventHandlerPredicatePair>();
			foreach (var type in types)
			{
				EnsureInitialized(type);
				var eh = this.eventHandlers[type];
				foreach (var pair in eh)
				{
					if(!eventHandlers.Contains(pair))
						eventHandlers.Add(pair);
				}
			}
			
			foreach (var eventHandler in eventHandlers)
			{
				if (eventHandler.Predicate(@event))
					eventHandler.EventHandler.Handle(@event);
			}
		}

		public void Publish<TEvent>(TEvent @event) where TEvent : EventBase
		{
			Publish((EventBase) @event);
		}

		public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler) where TEvent : EventBase
		{
			Subscribe(eventHandler, (p => true));
		}

		public void Subscribe<TEvent>(Action<TEvent> eventHandler) where TEvent : EventBase
		{
			Subscribe(eventHandler, (p => true));
		}

		public void Subscribe<TEvent>(IEventHandler<TEvent> eventHandler, Predicate<TEvent> condition)
			where TEvent : EventBase
		{
			EnsureInitialized(typeof (TEvent));
			this.eventHandlers[typeof (TEvent)].Add(new EventHandlerPredicatePair
			                                        	{EventHandler = eventHandler, Predicate = (x => condition((TEvent) x))});
		}

		public void Subscribe<TEvent>(Action<TEvent> eventHandler, Predicate<TEvent> condition) where TEvent : EventBase
		{
			EnsureInitialized(typeof (TEvent));
			this.eventHandlers[typeof (TEvent)].Add(new EventHandlerPredicatePair
			                                        	{
			                                        		EventHandler = new DelegateEventHandler<TEvent>(eventHandler),
			                                        		Predicate = (x => condition((TEvent) x))
			                                        	});
		}

		#endregion

		protected void EnsureInitialized(Type t)
		{
			if (!this.eventHandlers.ContainsKey(t))
				this.eventHandlers[t] = new List<EventHandlerPredicatePair>();
		}

		#region Nested type: EventHandlerPredicatePair

		private class EventHandlerPredicatePair
		{
			public IEventHandler EventHandler { get; set; }
			public Predicate<EventBase> Predicate { get; set; }
		}

		#endregion
	}
}