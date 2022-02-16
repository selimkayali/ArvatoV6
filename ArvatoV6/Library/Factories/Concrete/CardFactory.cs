using ArvatoV6.Models.Abstract;
using ArvatoV6.Models.Concrete;

namespace ArvatoV6.Library.Factories.Abstract
{
    public static class CardFactory
    {
        public static ICard? CreateCard(string cardType)
        {
            switch (cardType)
            {
                case CardType.Visa:
                    return new Visa();
                case CardType.MasterCard:
                    return new MasterCard();
                case CardType.AmericanExpress:
                    return new AmEx();
                default:
                    return null;
            }
        }
    }
}