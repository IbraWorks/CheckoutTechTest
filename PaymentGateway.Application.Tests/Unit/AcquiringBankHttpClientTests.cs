using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace PaymentGateway.Application.Tests.Unit
{
   
    public class AcquiringBankHttpClientTests
    {
        private AcquiringBankHttpClient _sut;



        [Fact]
        public async Task Returns_Successful_Response_When_Http_Response_Is_Successful()
        {
            var httpHandlerMock = new Mock<HttpMessageHandler>();
            httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.Accepted,
                    Content = new StringContent("1", Encoding.UTF8, "application/json")
                });
            _sut = new AcquiringBankHttpClient(new HttpClient(httpHandlerMock.Object));

            var response = await _sut.Post<int>("http://test.com", null);
            
            Assert.Equal(1, response.Data);
            Assert.Equal("Successfully sent payment to acquiring bank", response.Message);
            Assert.False(response.Error);
        }

        [Fact]
        public async Task Returns_Failure_Response_When_Response_Is_Unsuccessful()
        {
            var httpHandlerMock = new Mock<HttpMessageHandler>();
            httpHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                ).ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("1", Encoding.UTF8, "application/json")
                });
            _sut = new AcquiringBankHttpClient(new HttpClient(httpHandlerMock.Object));

            var response = await _sut.Post<int>("http://test.com", null);

            Assert.Equal(default, response.Data);
            Assert.Equal("InternalServerError", response.Message);
            Assert.True(response.Error);
        }

    }



    // think of a better way to test this.

    class ReturnSuccessResponseHandler: HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Accepted);
            return Task.FromResult(response);
        }
    }

    class Return500ResponseHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            return Task.FromResult(response);
        }
    }
}
