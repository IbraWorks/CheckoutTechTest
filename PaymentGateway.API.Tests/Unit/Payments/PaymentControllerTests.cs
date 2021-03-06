﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PaymentGateway.API.V1.Payments;
using PaymentGateway.Application;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Payments.Commands;
using PaymentGateway.Application.Payments.Queries;
using Xunit;

namespace PaymentGateway.API.Tests.Unit.Payments
{
    public class PaymentControllerTests
    {
        private readonly Mock<IMediator> _mediator;
        private readonly PaymentsController _sut;
        private readonly Mock<ILogger<PaymentsController>> _mockLogger;

        public PaymentControllerTests()
        {
            _mockLogger = new Mock<ILogger<PaymentsController>>();
            _mediator = new Mock<IMediator>();
            _sut = new PaymentsController(_mediator.Object, _mockLogger.Object);
        }



        [Fact]
        public async Task Get_Payment_Returns_NotFound_When_Payment_Does_Not_Exist()
        {
            _mediator
                .Setup(_ => _.Send(It.IsAny<GetPaymentQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.Failure<Payment>("nope"));

            var response = await _sut.GetPayment(Guid.Empty);

            Assert.IsAssignableFrom<NotFoundResult>(response.Result);

        }

        [Fact]
        public async Task Get_Payment_Returns_Ok_When_Payment_Exists()
        {
            _mediator
                .Setup(_ => _.Send(It.IsAny<GetPaymentQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.Success(new Payment(), "nope"));

            var response = await _sut.GetPayment(Guid.NewGuid());

            Assert.IsAssignableFrom<OkObjectResult>(response.Result);
        }

        [Fact]
        public async Task Post_Payment_Returns_CreatedResult_When_Payment_Is_Successful()
        {
            _mediator
                .Setup(_ => _.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.Success<Guid>(Guid.Empty, "test"));

            var response = await _sut.PostPayment(Guid.Empty, new PostPaymentModel());

            Assert.IsAssignableFrom<CreatedResult>(response.Result);
        }

        [Fact]
        public async Task Post_Payment_Returns_BadRequest_When_Payment_Is_Unsuccessful()
        {
            _mediator
                .Setup(_ => _.Send(It.IsAny<CreatePaymentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Response.Failure<Guid>("failed"));

            var response = await _sut.PostPayment(Guid.Empty, new PostPaymentModel());

            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);
        }


    }
}
