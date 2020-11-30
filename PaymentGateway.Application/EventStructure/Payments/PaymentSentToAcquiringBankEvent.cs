using System;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.EventStructure.Payments
{
    public class PaymentSentToAcquiringBankEvent : IEvent<Payment>
    {
        public DateTime Timestamp { get; }
        public Guid Id { get; }


        public PaymentSentToAcquiringBankEvent(DateTime timestamp, Guid id)
        {
            Timestamp = timestamp;
            Id = id;
        }
        public void Process(Payment payment)
        {
            payment.HandleSentToAcquiringBank(this);
        }
    }
}
