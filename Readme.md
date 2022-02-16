Credit Card Validation API
Application created with latest stable version of .NET 6 sdk.
There is an POST endpoit for handling the credit card information. Input validations completed with _FluentValidation_,
business validations completed manually. Luhn algorithm helped me for calculating card number division by 10. Application is containerized and host Docker Hub please use "docker run -p 8080:80 -d selimkayali/arvatov6" command to download image and serve the api from http://localhost:8080/api/cardValidation.

You can use this payload for making a Post request and getting result.

`{
  "cardHolder": "selim kayali",
  "cardNumber": "4111111111111111",
  "expiryDate": "122025",
  "cvv": "123"
}`

For unit testing, xUnit, for bdd testing (just one test added) Specflow packaged used.
