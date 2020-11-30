using System;
using System.Collections.Generic;
using System.Text;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.EventStructure.Payments
{
    public class CreatePaymentEvent : IEvent<Payment>
    {
        public CreatePaymentEvent(DateTime timestamp, Guid id, string creditCardNumber, Currency currency, decimal amount)
        {
            Timestamp = timestamp;
            Id = id;
            CardNumber = creditCardNumber;
            Currency = currency;
            Amount = amount;
        }

        public DateTime Timestamp { get; }
        public Guid Id { get; }
        public string CardNumber { get; }
        public Currency Currency { get; }
        public decimal Amount { get; }

        public void Process(Payment payment)
        {
            payment.HandleCreatePayment(this);
        }
    }
}
