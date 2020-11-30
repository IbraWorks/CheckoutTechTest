using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentGateway.Application
{
    public interface IAcquiringBankHttpClient
    {
        Task<Response<T>> Post<T>(string url, object body);
    }
}
