using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentGateway.Application.Models;

namespace PaymentGateway.API.V1.Payments
{
    public class GetPaymentModel
    {
        public Guid Id { get; set; }
        public PaymentState PaymentState { get; set; }
        public DateTime TimeStamp { get; set; }
        public Guid AcquiringBankPaymentId { get; set; }
        public Currency Currency { get; set; }
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }

        public string Message { get; set; }
    }
}
