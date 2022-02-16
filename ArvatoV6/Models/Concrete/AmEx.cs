using ArvatoV6.Models.Abstract;

namespace ArvatoV6.Models.Concrete
{
    public class AmEx : ICard
    {
        public string GetCardType()
        {
            return CardType.AmericanExpress;
        }
    }
}