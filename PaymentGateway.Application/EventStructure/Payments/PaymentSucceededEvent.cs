using System;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.EventStructure.Payments
{
    public class PaymentSucceededEvent : IEvent<Payment>
    {
        public PaymentSucceededEvent(DateTime timestamp, Guid id, Guid acquiringBankPaymentId)
        {
            Timestamp = timestamp;
            Id = id;
            AcquiringBankPaymentId = acquiringBankPaymentId;
        }
        public DateTime Timestamp { get; }
        public Guid Id { get; }
        public Guid AcquiringBankPaymentId { get; }

        public void Process(Payment payment)
        {
            payment.HandlePaymentSucceeded(this);
        }
    }
}
