using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Application.EventStructure
{
    public interface IEvent<in T>
    {
        public DateTime Timestamp { get;  }
        public Guid Id { get; }
        void Process(T payment);
    }
}
