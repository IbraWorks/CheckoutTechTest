using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.EventStructure;
using PaymentGateway.Application.EventStructure.Payments;
using PaymentGateway.Application.Models;
using PaymentGateway.Application.Payments.Queries;

namespace PaymentGateway.Application.Payments.Commands
{
    public class CreatePaymentCommand : IRequest<Response<Guid>>
    {
        public CreatePaymentCommand(Guid id, string cardNumber, int monthExpired, int yearExpired, string ccv, decimal amount, Currency currency)
        {
            Id = id;
            CardNumber = cardNumber;
            MonthExpired = monthExpired;
            YearExpired = yearExpired;
            Ccv = ccv;
            Amount = amount;
            Currency = currency;
        }

        public Guid Id { get; }
        public string CardNumber { get; }
        public int MonthExpired { get;}
        public int YearExpired { get; }
        public string Ccv { get; }

        public decimal Amount { get; }

        public Currency Currency { get; }
    }

    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, Response<Guid>>
    {
        private readonly IEventStore<Payment> _eventStore;
        private readonly IEventDispatcher<Payment> _eventDispatcher;
        private readonly IAcquiringBankHttpClient _acquiringBankHttpClient;

        public CreatePaymentHandler(IEventStore<Payment> eventStore, IEventDispatcher<Payment> eventDispatcher, IAcquiringBankHttpClient acquiringBankHttpClient )
        {
            _eventStore = eventStore;
            _eventDispatcher = eventDispatcher;
            _acquiringBankHttpClient = acquiringBankHttpClient;
        }

        //this handler will actually fire off multiple events instead of just creating the payment, we should ideally refactor this with a payment service
        // and split the functionality up as the process gets more complicated.
        public async Task<Response<Guid>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            
            var paymentExists = await IsPaymentDuplicate(request.Id);
            if (paymentExists)
            {
                return Response.Failure<Guid>($"Error, a payment with id {request.Id} already exists");
            }

            // dont need to create the payment and process the events. we can get a payment by processing the events on the fly
            // see GetPaymentHandler for details. For now just dispatch the necessary events.
           
            var createPaymentEvent = new CreatePaymentEvent(DateTime.UtcNow, request.Id, request.CardNumber, request.Currency, request.Amount);
            await _eventDispatcher.DispatchEvent(createPaymentEvent);


            await _eventDispatcher.DispatchEvent(new PaymentSentToAcquiringBankEvent(DateTime.UtcNow, request.Id));

            //TODO: Extract this to a config file property in appsettings.json
            var baseUrl = "https://localhost:4001";
            var response = await _acquiringBankHttpClient.Post<Guid>($"{baseUrl}/api/AcquiringBank", new
            {
                request.CardNumber,
                request.MonthExpired,
                request.YearExpired,
                request.Ccv,
                request.Amount,
                request.Currency
            });

            if (!response.Error)
            {
                await _eventDispatcher.DispatchEvent(new PaymentSucceededEvent(DateTime.UtcNow, request.Id, response.Data));
                return Response.Success<Guid>(response.Data, response.Message);
            }
            else
            {
                await _eventDispatcher.DispatchEvent(new PaymentFailedEvent(DateTime.UtcNow, request.Id, response.Message));
                return Response.Failure(response.Message, response.Data);
            }
        }

        private async Task<bool> IsPaymentDuplicate(Guid requestId)
        {
            var events = await _eventStore.GetEvent(requestId);
            return events.Any();
        }

    }
}
