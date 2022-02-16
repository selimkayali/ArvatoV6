using ArvatoV6.Models.Dto;
using FluentValidation;

namespace ArvatoV6.Validations
{
    public class CreditCardValidator : AbstractValidator<CreditCardInputDto>
    {
        private IConfiguration _configuration;
        public CreditCardValidator(IConfiguration configuration)
        {
            _configuration = configuration;
            // this.CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.CardHolder).NotEmpty().WithMessage("Card holder name is required");
            RuleFor(x => x.CardHolder).Must(ContainOnlyChars).WithMessage("Card holder name must contain only letters");

            RuleFor(x => x.ExpiryDate).NotEmpty().WithMessage("Expiry date is required");
            RuleFor(x => x.ExpiryDate).Length(6).WithMessage("Invalid expiry date value");
            RuleFor(x => x.ExpiryDate).Must(ContainOnlyDigits).WithMessage("Expiry date must contain only digits");
            RuleFor(x => x.ExpiryDate).Must(BeValidExpiryDate).WithMessage("Expiry date must be valid");

            RuleFor(x => x.CardNumber).NotEmpty().WithMessage("Card number is required");
            RuleFor(x => x.CardNumber).Must(ContainOnlyDigits).WithMessage("Card number must contain only digits");
            RuleFor(x => x.CardNumber).Must(DivideByTen).WithMessage("Card number is invalid");

            RuleFor(x => x.CVV).NotEmpty().WithMessage("CVV is required");
            RuleFor(x => x.CVV).Must(BeInRange).WithMessage("CVV should has 3-4 digits");
            RuleFor(x => x.CVV).Must(ContainOnlyDigits).WithMessage("CVV must contain only digits");
        }

        private bool ContainOnlyChars(string cardHolder)
        {
            return cardHolder.Replace(" ", String.Empty).All(char.IsLetter);
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

            return (sumOfDigits % 10 is 0 && sumOfDigits > 0);
        }

        private bool BeInRange(string cvv)
        {
            return cvv.Length is 3 or 4;
        }

        private bool BeValidExpiryDate(string expiryDate)
        {
            if (String.IsNullOrWhiteSpace(expiryDate)) return false;

            int yearLimit;
            int.TryParse(_configuration["YearLimit"], out yearLimit);

            int month;
            int year;

            int.TryParse(expiryDate.Substring(0, 2), out month);
            int.TryParse(expiryDate.Substring(2, 4), out year);

            if (month < 1 || month > 12)
            {
                return false;
            }

            if (year < DateTime.UtcNow.Year || year > DateTime.Now.AddYears(20).Year)
            {
                return false;
            }
            if (month is 12)
            {
                month = 1;
                year++;
            }

            var cardExpiryDate = new DateTime(year, month, 1);
            return cardExpiryDate > DateTime.Now;
        }
    }
}

