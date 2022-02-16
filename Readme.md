Credit Card Validation API
Application created with latest stable version of .NET 6 sdk.
There is an POST endpoit for handling the credit card information. Input validations completed with _FluentValidation_,
business validations completed manually. Luhn algorithm helped me for calculating card number division by 10. Application is containerized and pushed to Docker Hub. Please use "docker run -p 8080:80 -d selimkayali/arvatov6" command on your terminal application to download image and run/serve the application on http://localhost:8080/api/cardValidation.

You can use this payload for making a Post request and getting result.

`{
  "cardHolder": "selim kayali",
  "cardNumber": "4111111111111111",
  "expiryDate": "122025",
  "cvv": "123"
}`

For unit testing, xUnit, for bdd testing (just one test added) Specflow packaged used.
