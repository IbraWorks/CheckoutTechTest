using System;
using PaymentGateway.Application.EventStructure.Payments;

namespace PaymentGateway.Application.Models
{
    public class Payment
    {

        public void HandleCreatePayment(CreatePaymentEvent ev)
        {
            if (PaymentState != PaymentState.Unknown)
            {
                throw new Exception("This payment has already been created.");
            }

            PaymentState = PaymentState.Created;
            TimeStamp = ev.Timestamp;
            Id = ev.Id;

            Currency = ev.Currency;
            CardNumber = ev.CardNumber;
            Amount = ev.Amount;
        }

        public void HandleSentToAcquiringBank(PaymentSentToAcquiringBankEvent ev)
        {
            if (PaymentState != PaymentState.Created)
            {
                throw new Exception("This payment has not yet been created");
            }

            PaymentState = PaymentState.PendingAcquiringBankResponse;
        }

        public void HandlePaymentSucceeded(PaymentSucceededEvent ev)
        {
            PaymentState = PaymentState.Successful;
            AcquiringBankPaymentId = ev.AcquiringBankPaymentId;
        }

        public void HandlePaymentFailed(PaymentFailedEvent ev)
        {
            PaymentState = PaymentState.Failed;
            FailureMessage = ev.FailureMessage;
        }

        public PaymentState PaymentState { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public Guid Id { get; private set; }
        public Guid AcquiringBankPaymentId { get; private set; }

        public Currency Currency { get; private set; }
        public string CardNumber { get; private set; }
        public decimal Amount { get; private set; }

        public string FailureMessage { get; private set; }


        public string HiddenCardNumber()
        {
            return $"{CardNumber?.Substring(0, 4)} **** **** ****";
        }
    }
}
