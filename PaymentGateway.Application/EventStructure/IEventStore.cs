using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.EventStructure
{
    public interface IEventStore<T>
    {
        Task<IEnumerable<IEvent<T>>> GetEvent(Guid id);
        void AddEvent(IEvent<T> ev);
    }
}
