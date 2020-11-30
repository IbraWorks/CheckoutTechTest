using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentGateway.Application.EventStructure;
using PaymentGateway.Application.Models;

namespace PaymentGateway.Application.Payments.Queries
{
    public class GetPaymentQuery : IRequest<Response<Payment>>
    {
        public Guid Id { get; }

        public GetPaymentQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, Response<Payment>>
    {
        private readonly IEventStore<Payment> _eventStore;

        public GetPaymentHandler(IEventStore<Payment> eventStore)
        {
            _eventStore = eventStore;
        }
        public async Task<Response<Payment>> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var payment =  await RetrievePayment(request.Id);

            return payment != null ? Response.Success<Payment>(payment, "Payment Found") : Response.Failure<Payment>("Payment does not exist");
        }

        private async Task<Payment> RetrievePayment(Guid requestId)
        {
            var events = await _eventStore.GetEvent(requestId);
            if (!events.Any())
            {
                return null;
            }

            var payment = new Payment();
            foreach (var ev in events.OrderBy(e => e.Timestamp))
            {
                ev.Process(payment);
            }

            return payment;
        }
    }

}
