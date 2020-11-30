using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.EventStructure
{
    public class EventDispatcher<T> : IEventDispatcher<T>
    {
        private readonly IEventStore<T> _eventStore;

        public EventDispatcher(IEventStore<T> eventStore)
        {
            _eventStore = eventStore;
        }
        public Task DispatchEvent(IEvent<T> ev)
        {
            _eventStore.AddEvent(ev);
            
            return Task.CompletedTask;
        }
    }
}
