using System;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.EventStructure.Payments
{
    public class PaymentFailedEvent : IEvent<Payment>
    {
        public PaymentFailedEvent(DateTime timestamp, Guid id, string failureMessage)
        {
            Timestamp = timestamp;
            Id = id;
            FailureMessage = failureMessage;
        }
        public DateTime Timestamp { get; }
        public Guid Id { get; }
        public string FailureMessage { get; }

        public void Process(Payment payment)
        {
            payment.HandlePaymentFailed(this);
        }
    }
}
