# CheckoutTechTest

### How to run

The easiest way is to: 
- open a terminal, cd into the MockAcquiringBank.API directory and run `dotnet watch run`
- open another terminal shell, cd into the `PaymentGateway.API` directory and run `dotnet watch run`, this should open a browser window to the swagger index page. 
- if it doesnt, append `index.html` to the url to get to the swagger index page
- Pass in the required parameters and test the api
- Alternatively, with both projects running one could hit the PaymentGateway.API via postman / curl etc
