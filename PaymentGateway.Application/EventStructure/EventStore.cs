using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.EventStructure
{
    public class EventStore<T> : IEventStore<T>
    {
        private readonly ConcurrentDictionary<Guid, IList<IEvent<T>>> _dataStore = new ConcurrentDictionary<Guid, IList<IEvent<T>>>();
        public async Task<IEnumerable<IEvent<T>>> GetEvent(Guid id)
        {
            if (_dataStore.TryGetValue(id, out var events))
            {
                return events.ToList();
            }

            return Enumerable.Empty<IEvent<T>>();
        }

        public void AddEvent(IEvent<T> ev)
        {
            if (_dataStore.TryGetValue(ev.Id, out var events))
            {
                events.Add(ev);
            }
            else
            {
                _dataStore.TryAdd(ev.Id, new List<IEvent<T>>{ev});
            }
        }
    }
}


//if (_dataStore.TryGetValue(id, out var events))
//{
//return await Task.FromResult(events.ToList());
//}
//throw new NotImplementedException();