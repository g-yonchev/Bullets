namespace Bullets.Logic.MonteCarlo
{
    using System.Collections.Generic;
    using System.Linq;

    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Helpers;

    internal class MonteCarloAlgorithm : IMonteCarlo
    {
        private const float SimulationsCount = 50f;
		private const int MaxCountCommunityCards = 5;

        public float CalculateWinningChance(Card playerFirstCard, Card playerSecondCard, IReadOnlyCollection<Card> communityCards)
        {
            IList<Card> deck = this.GetDeck(playerFirstCard, playerSecondCard, communityCards);

            int cardStrengthResult = 0;

            for (int i = 0; i < SimulationsCount; i++)
            {
                var currentDeck = new Stack<Card>(deck.Shuffle());

                IList<Card> opponentCards = new List<Card> { currentDeck.Pop(), currentDeck.Pop() };

                var currentCommunityCards = communityCards.ToList();
                while (currentCommunityCards.Count < MaxCountCommunityCards)
                {
                    currentCommunityCards.Add(currentDeck.Pop());
                }

                var myHand = currentCommunityCards.ToList();
                myHand.Add(playerFirstCard);
                myHand.Add(playerSecondCard);

                var opponentHand = opponentCards.Concat(currentCommunityCards);

                int currentCardStrengthResult = Helpers.CompareCards(myHand, opponentHand);
                cardStrengthResult += currentCardStrengthResult;
            }

			float result = GetStrengthAsPercentage(cardStrengthResult, SimulationsCount);
            return result;
        }

        private IList<Card> GetDeck(Card firstCard, Card secondCard, IReadOnlyCollection<Card> communityCards)
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
		
		private float GetStrengthAsPercentage(int v1, float v2)
		{
			// magic
			return 100 * (v1 + v2) / (2 * v2);
		}
    }
}
