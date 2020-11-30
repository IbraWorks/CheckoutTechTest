using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace PaymentGateway.Application
{
    public class AcquiringBankHttpClient : IAcquiringBankHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AcquiringBankHttpClient> _logger;

        public AcquiringBankHttpClient(HttpClient httpClient, ILogger<AcquiringBankHttpClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

 
        public async Task<Response<T>> Post<T>(string url, object body)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(url, body);
                if (!response.IsSuccessStatusCode) return Response.Failure<T>($"{response.StatusCode.ToString()}");

                var data = await response.Content.ReadAsAsync<T>();
                return Response.Success<T>(data, "Successfully sent payment to acquiring bank");

            }
            catch (Exception e)
            {
                _logger.LogCritical(e, "Unable to connect with Acquiring Bank Api");

                return Response.Failure<T>("Error occurred, payment was not processed");
            }
        }
    }
}
