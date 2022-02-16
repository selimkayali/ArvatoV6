using ArvatoV6.Models.Abstract;

namespace ArvatoV6.Models.Concrete
{
    public class Visa : ICard
    {
        public string GetCardType()
        {
            return CardType.Visa;
        }
    }
}