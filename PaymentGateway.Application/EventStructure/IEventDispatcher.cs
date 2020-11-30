using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application.EventStructure
{
    public interface IEventDispatcher<T>
    {
        Task DispatchEvent(IEvent<T> ev);
    }
}
