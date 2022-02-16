using ArvatoV6.Models.Abstract;

namespace ArvatoV6.Models.Concrete
{
    public class MasterCard : ICard
    {
        public string GetCardType()
        {
            return CardType.MasterCard;
        }
    }
}