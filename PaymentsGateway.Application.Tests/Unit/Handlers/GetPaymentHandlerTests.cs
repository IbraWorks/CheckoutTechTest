using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using PaymentGateway.Application.EventStructure;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Payments.Queries;
using Xunit;

namespace PaymentsGateway.Application.Tests.Unit.Handlers
{
    public class GetPaymentHandlerTests
    {
        private readonly IList<IEvent<Payment>> _mockEvents;
        private readonly GetPaymentHandler _sut;
        private readonly GetPaymentQuery _mockQuery;
        private readonly CancellationToken _mockCancellationToken;
        private readonly Mock<IEventStore<Payment>> _mockEventStore;

        public GetPaymentHandlerTests()
        {
            _mockEvents = new List<IEvent<Payment>>();
            _mockEventStore = new Mock<IEventStore<Payment>>();
            _sut = new GetPaymentHandler(_mockEventStore.Object);
            _mockQuery = new GetPaymentQuery(Guid.Empty);
            _mockCancellationToken = new CancellationToken();
        }

        [Fact]
        public async Task No_Payment_Returned_Given_No_Events()
        {
            _mockEventStore
                .Setup(_ => _.GetEvent(It.IsAny<Guid>())).ReturnsAsync(_mockEvents);
            var response = await _sut.Handle(_mockQuery, _mockCancellationToken);

            Assert.Null(response.Data);
        }


        //figure out way to test this
        [Fact]
        public async Task Events_Processed_In_Order_For_given_Payment()
        {
            _mockEventStore
                .Setup(_ => _.GetEvent(It.IsAny<Guid>())).ReturnsAsync(_mockEvents);
            var response = await _sut.Handle(_mockQuery, _mockCancellationToken);


        }
    }
}
