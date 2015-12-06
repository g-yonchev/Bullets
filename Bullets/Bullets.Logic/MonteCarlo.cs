namespace Bullets.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    internal class MonteCarlo : IMonteCarlo
    {
        private const float SimulationsCount = 10000f;

        public float CalculateWinningChance(
            IList<Card> playerCards,
            IReadOnlyCollection<Card> communityCards)
        {
            var deck = this.GetDeck(playerCards, communityCards);

            int cardStrengthResult = 0;

            for (int i = 0; i < SimulationsCount; i++)
            {
                var currentDeck = new Stack<Card>(deck.Shuffle());

                IList<Card> opponentCards = new List<Card> { currentDeck.Pop(), currentDeck.Pop() };

                var currentCommunityCards = communityCards.ToList();
                while (currentCommunityCards.Count < 5)
                {
                    currentCommunityCards.Add(currentDeck.Pop());
                }

                var myHand = playerCards.Concat(currentCommunityCards);
                var opponentHand = opponentCards.Concat(currentCommunityCards);

                int currentCardStrengthResult = Helpers.CompareCards(myHand, opponentHand);

                cardStrengthResult += currentCardStrengthResult;
            }

            float result = cardStrengthResult / SimulationsCount;

            return result;
        }

        private IList<Card> GetDeck(
            IList<Card> playerCards,
            IReadOnlyCollection<Card> communityCards)
        {
            var deck = Deck.AllCards;
            foreach (var playerCard in playerCards)
            {
                deck.Remove(playerCard);
            }

            foreach (var communityCard in communityCards)
            {
                deck.Remove(communityCard);
            }

            return deck;
        }
    }
}
