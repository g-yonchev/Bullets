namespace Bullets.Logic
{
    using System.Collections.Generic;
    using TexasHoldem.Logic.Cards;

    internal interface IMonteCarlo
    {
        float CalculateWinningChance(Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards);
    }
}