using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using PaymentGateway.Application.EventStructure;
using PaymentGateway.Application.EventStructure.Payments;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Payments.Commands;
using Xunit;

namespace PaymentGateway.Application.Tests.Unit.Handlers
{
    public class CreatePaymentHandlerTests
    {
        private readonly IList<IEvent<Payment>> _mockEvents = new List<IEvent<Payment>>();

        private readonly Mock<IAcquiringBankHttpClient> _abApiClient;
        private readonly Mock<IEventStore<Payment>> _eventStore;
        private readonly Mock<IEventDispatcher<Payment>> _eventDispatcher;
        private readonly CreatePaymentHandler _sut;
        private readonly CreatePaymentCommand _mockCommand;
        private readonly CancellationToken _mockCancellationToken;

        public CreatePaymentHandlerTests()
        {
            _eventStore = new Mock<IEventStore<Payment>>();
            _eventDispatcher = new Mock<IEventDispatcher<Payment>>();
            _abApiClient = new Mock<IAcquiringBankHttpClient>();
            _sut = new CreatePaymentHandler(_eventStore.Object, _eventDispatcher.Object, _abApiClient.Object);
            _mockCommand = new CreatePaymentCommand(default, default, default, default, default, default, default);
            _mockCancellationToken = new CancellationToken();
        }

        [Fact]
        public async Task Given_Payment_Exists_Failure_Response_Returned_Bank_Is_Not_Called_No_Events_Dispatched()
        {
            _eventStore
                .Setup(_ => _.GetEvent(It.IsAny<Guid>())).ReturnsAsync(_mockEvents);

            _abApiClient
                .Setup(_ => _.Post<Guid>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(Response.Success(Guid.Empty, "test"));

            _mockEvents.Add(new Mock<IEvent<Payment>>().Object);

            var response = await _sut.Handle(_mockCommand, _mockCancellationToken);
            response.Error.Should().BeTrue();
            _eventDispatcher.Verify(_ => _.DispatchEvent(It.IsAny<IEvent<Payment>>()), Times.Never);
            _abApiClient.Verify(_ => _.Post<Guid>(It.IsAny<string>(), It.IsAny<object>()), Times.Never);
        }

        [Fact]
        public async Task Given_Payment_Does_Not_Exist_CreateEvent_PaymentSentToAcquiringBankEvent_Dispatched()
        {
            _eventStore
                .Setup(_ => _.GetEvent(It.IsAny<Guid>())).ReturnsAsync(_mockEvents);

            _abApiClient
                .Setup(_ => _.Post<Guid>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(Response.Success(Guid.Empty, "test"));

            var response = await _sut.Handle(_mockCommand, _mockCancellationToken);
            _eventDispatcher.Verify(_ => _.DispatchEvent(It.IsAny<CreatePaymentEvent>()), Times.Once);
            _eventDispatcher.Verify(_ => _.DispatchEvent(It.IsAny<PaymentSentToAcquiringBankEvent>()), Times.Once);
        }

        [Fact]
        public async Task Given_PaymentSentToAcquiringBankEvent_Successful_PaymentSucceededEvent_IS_Dispatched()
        {
            _eventStore
                .Setup(_ => _.GetEvent(It.IsAny<Guid>())).ReturnsAsync(_mockEvents);

            _abApiClient
                .Setup(_ => _.Post<Guid>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(Response.Success(Guid.Empty, "test"));

            var response = await _sut.Handle(_mockCommand, _mockCancellationToken);
            _eventDispatcher.Verify(_ => _.DispatchEvent(It.IsAny<PaymentSucceededEvent>()), Times.Once);
            _eventDispatcher.Verify(_ => _.DispatchEvent(It.IsAny<PaymentFailedEvent>()), Times.Never);
            Assert.False(response.Error);
        }

        [Fact]
        public async Task Given_PaymentSentToAcquiringBankEvent_Unsuccessful_PaymentFailedEvent_Is_Dispatched()
        {
            _eventStore
                .Setup(_ => _.GetEvent(It.IsAny<Guid>())).ReturnsAsync(_mockEvents);

            _abApiClient
                .Setup(_ => _.Post<Guid>(It.IsAny<string>(), It.IsAny<object>()))
                .ReturnsAsync(Response.Failure<Guid>("error"));

            var response = await _sut.Handle(_mockCommand, _mockCancellationToken);
            _eventDispatcher.Verify(_ => _.DispatchEvent(It.IsAny<PaymentSucceededEvent>()), Times.Never);
            _eventDispatcher.Verify(_ => _.DispatchEvent(It.IsAny<PaymentFailedEvent>()), Times.Once);
            Assert.True(response.Error);
        }
    }
}
