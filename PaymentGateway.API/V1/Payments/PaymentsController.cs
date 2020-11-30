using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Payments.Commands;
using PaymentGateway.Application.Payments.Queries;

namespace PaymentGateway.API.V1.Payments
{
    /// <summary>
    /// Handles payments from merchants
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(IMediator mediator, ILogger<PaymentsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("{paymentId:guid}")]
        public async Task<ActionResult<Response<Payment>>> PostPayment(Guid paymentId, PostPaymentModel model)
        {
            var response = 
                await _mediator.Send(new CreatePaymentCommand(paymentId, model.CardNumber, model.MonthExpired, 
                    model.YearExpired, model.Ccv, model.Amount, model.Currency));

            if (response.Error)
            {
                _logger.Log(LogLevel.Information, $"payment {paymentId} failed");

                return BadRequest(response.Message);
            }

            _logger.Log(LogLevel.Information, $"payment {paymentId} successfully sent");

            return new CreatedResult($"/v1/payment/{paymentId}", response.Data);


        }

        [HttpGet("{paymentId:guid}")]
        public async Task<ActionResult<GetPaymentModel>> GetPayment(Guid paymentId)
        {
            var response = await _mediator.Send(new GetPaymentQuery(paymentId));
            if (response.Error)
            {
                return NotFound();
            }

            var payment = response.Data;


            return Ok(new GetPaymentModel
            {
                Id = payment.Id,
                PaymentState = payment.PaymentState,
                TimeStamp = payment.TimeStamp,
                AcquiringBankPaymentId = payment.AcquiringBankPaymentId,
                Currency = payment.Currency,
                CardNumber = payment.HiddenCardNumber(),
                Amount = payment.Amount
            });
        }

    }
}
