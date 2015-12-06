namespace Bullets.Logic
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    internal class MonteCarlo : IMonteCarlo
    {
        private const float SimulationsCount = 50f;

        public float CalculateWinningChance(
            Card firstCard,
            Card secondCard,
            IReadOnlyCollection<Card> communityCards)
        {
            var deck = this.GetDeck(firstCard, secondCard, communityCards);

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

                var myHand = currentCommunityCards.ToList();
                myHand.Add(firstCard);
                myHand.Add(secondCard);

                var opponentHand = opponentCards.Concat(currentCommunityCards);

                int currentCardStrengthResult = Helpers.CompareCards(myHand, opponentHand);

                cardStrengthResult += currentCardStrengthResult;
            }

            float result = 100 * (cardStrengthResult + SimulationsCount) / (2 * SimulationsCount);

            return result;
        }

        private IList<Card> GetDeck(
            Card firstCard,
            Card secondCard,
            IReadOnlyCollection<Card> communityCards)
        {
            var deck = Deck.AllCards.ToList();

            deck.Remove(firstCard);
            deck.Remove(secondCard);

            foreach (var communityCard in communityCards)
            {
                deck.Remove(communityCard);
            }

            return deck;
        }
    }
}
