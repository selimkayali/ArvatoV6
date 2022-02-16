Feature: CreditCardValidation
	Validate card with given information

@mytag
Scenario: I should get card type as Visa
	Given I have valid information {cardHolder:"selim kayali", cardNumber: "4111111111111111", expiryDate:"102024", cvv:"123"}
	When I validate the card
	Then I should get card type as Visa
	
