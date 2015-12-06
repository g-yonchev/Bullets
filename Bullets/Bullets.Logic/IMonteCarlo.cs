namespace Bullets.Logic
{
    using System.Collections.Generic;
    using TexasHoldem.Logic.Cards;

    internal interface IMonteCarlo
    {
        float CalculateWinningChance(IList<Card> playerCards, IReadOnlyCollection<Card> communityCards);
    }
}