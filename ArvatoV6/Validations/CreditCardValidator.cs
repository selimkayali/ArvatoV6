using ArvatoV6.Models;
using FluentValidation;

namespace ArvatoV6.Validations
{


    public class CreditCardValidator : AbstractValidator<CreditCardInputDto>
    {
        public CreditCardValidator()
        {
            this.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.CardHolder).NotEmpty().WithMessage("Card holder name is required");
            RuleFor(x => x.CardHolder).Must(ContainOnlyChars).WithMessage("Card holder name must contain only letters");

            RuleFor(x => x.ExpiryDate).NotEmpty().WithMessage("Expiry date is required");
            RuleFor(x => x.ExpiryDate).Must(ContainOnlyDigits).WithMessage("Expiry date must contain only digits");
            RuleFor(x => x.ExpiryDate).Must(BeValidExpiryDate).WithMessage("Expiry date must be valid");

            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("Card number is required");
            RuleFor(x => x.CardNumber).Must(ContainOnlyDigits).WithMessage("Card number must contain only digits");
            RuleFor(x => x.CardNumber).Must(DivideByTen).WithMessage("Card number is invalid");

            RuleFor(x => x.CVV).NotEmpty().WithMessage("CVV is required");
            RuleFor(x => x.CVV).Must(BeInRange).WithMessage("CVV should has 3-4 digits");
            RuleFor(x => x.CVV).Must(ContainOnlyDigits).WithMessage("CVV must contain only digits");
            //rule of amex cvv is 4 digits
            RuleFor(x => x.CardNumber).Must(BeAmex).WithMessage("CVV should has 4 digits for amex card");
        }

        private bool ContainOnlyChars(string cardHolder)
        {
            return cardHolder.All(char.IsLetter);
        }

        private bool ContainOnlyDigits(string cardNumber)
        {
            return cardNumber.Replace(" ", String.Empty).All(char.IsDigit);
        }

        private bool DivideByTen(string cardNumber)
        {
            //Luhn Algorithm
            int sumOfDigits = cardNumber.Replace(" ", String.Empty)
            .Where((e) => e >= '0' && e <= '9')
            .Reverse()
            .Select((e, i) => ((int)e - 48) * (i % 2 == 0 ? 1 : 2))
            .Sum((e) => e / 10 + e % 10);

            return sumOfDigits % 10 is 0;
        }

        private bool BeInRange(string cvv)
        {
            return cvv.Length is 3 or 4;
        }

        private bool BeValidExpiryDate(string expiryDate)
        {
            var date = DateTime.ParseExact(expiryDate, "MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            return date > DateTime.Now;


        }

        private bool BeAmex(string cardNumber)
        {
            return cardNumber.Length is 4;
        }
    }
}

