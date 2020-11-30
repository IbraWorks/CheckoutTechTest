using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Application.Models
{
    public enum PaymentState
    {
        Unknown,
        Created,
        PendingAcquiringBankResponse,
        Successful,
        Failed
    }
}
