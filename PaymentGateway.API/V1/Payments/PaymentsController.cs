using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
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

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{paymentId:guid}")]
        public async Task<ActionResult<Response<Payment>>> PostPayment(Guid paymentId, PostPaymentModel model)
        {
            var response = 
                await _mediator.Send(new CreatePaymentCommand(paymentId, model.CardNumber, model.MonthExpired, 
                    model.YearExpired, model.Ccv, model.Amount, model.Currency));

            if (response.Error)
            {
                return BadRequest(response.Message);
            }
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
                Amount = payment.Amount,
                Message = response.Message
            });
        }

    }
}
