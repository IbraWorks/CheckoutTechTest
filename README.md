# CheckoutTechTest

### How to run

The easiest way is to: 
- open a terminal, cd into the MockAcquiringBank.API directory and run `dotnet watch run`
- open another terminal shell, cd into the `PaymentGateway.API` directory and run `dotnet watch run`, this should open a browser window to the swagger index page. 
- if it doesnt, append `index.html` to the url to get to the swagger index page
- Pass in the required parameters and test the api
- Alternatively, with both projects running one could hit the PaymentGateway.API via postman / curl etc

### Design principles
I went for an event-sourcing model because I wanted an audit trail of every transaction, such that a user would be able to in theory generate a history of all the transactions they had made. 

I chose to use the cqrs pattern with and used the MediatR nuget package to help achieve this. In theory we can even eventually separate the commands and queries into different services and scale them independently

I tried to achieve Idempotency to ensure the same payment was not processed twice.

### Improvements
There is no retry policy or circuit breaker pattern implemented when talking to the MockAcquiringBank api.

The data storage of the event is hosted in memory.

There is no authorization or authentication used in this project so far.

Finally, although there are Dockerfiles, ssl certs needs to be setup so that the containers can talk over https.
