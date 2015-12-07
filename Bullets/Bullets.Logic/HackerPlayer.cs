namespace Bullets.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Players;

    public class HackerPlayer : BasePlayer
    {
        private readonly IList<Card> myCards = new List<Card>(2);
        private IList<Card> communityCards;

        public override string Name { get; } = $"HackerPlayer_{Guid.NewGuid()}";

        public override void StartGame(StartGameContext context)
        {
            Deck.AllCards.Clear();

            Deck.AllCards.Add(new Card(CardSuit.Spade, CardType.Two));
            Deck.AllCards.Add(new Card(CardSuit.Heart, CardType.Two));

            Deck.AllCards.Add(new Card(CardSuit.Diamond, CardType.Ace));
            Deck.AllCards.Add(new Card(CardSuit.Club, CardType.King));
            Deck.AllCards.Add(new Card(CardSuit.Spade, CardType.Queen));
            Deck.AllCards.Add(new Card(CardSuit.Heart, CardType.Jack));

            Deck.AllCards.Add(new Card(CardSuit.Diamond, CardType.Nine));
            Deck.AllCards.Add(new Card(CardSuit.Club, CardType.Eight));
            Deck.AllCards.Add(new Card(CardSuit.Spade, CardType.Seven));

            base.StartGame(context);
        }

        public override void StartHand(StartHandContext context)
        {
            myCards.Add(context.FirstCard);
            myCards.Add(context.SecondCard);

            base.StartHand(context);
        }

        public override void EndHand(EndHandContext context)
        {
            myCards.Clear();
            base.EndHand(context);
        }

        public override void StartRound(StartRoundContext context)
        {
            communityCards = new List<Card>(context.CommunityCards);
            base.StartRound(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (myCards.Count(c => c.Type == CardType.Two || c.Type == CardType.Ace) == 2)
            {
                return PlayerAction.Raise(context.MoneyLeft);
            }

            if (myCards.Count(c => c.Type == CardType.Two) == 1)
            {
                if (communityCards.Count(c => c.Type == CardType.Two) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace) == 1
                    && myCards.Count(c => c.Type == CardType.King) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace || c.Type == CardType.King) == 2
                    && myCards.Count(c => c.Type == CardType.Queen) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace || c.Type == CardType.King || c.Type == CardType.Queen) == 3
                    && myCards.Count(c => c.Type == CardType.Jack) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace || c.Type == CardType.King || c.Type == CardType.Queen || c.Type == CardType.Jack) == 4
                    && myCards.Count(c => c.Type == CardType.Nine) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace || c.Type == CardType.King || c.Type == CardType.Queen || c.Type == CardType.Jack || c.Type == CardType.Nine) == 5)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }
            }

            if (communityCards.Count(c => c.Type == CardType.Two) == 2)
            {
                if (myCards.Count(c => c.Type == CardType.Ace) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace) == 1
                    && myCards.Count(c => c.Type == CardType.King) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace || c.Type == CardType.King) == 2
                    && myCards.Count(c => c.Type == CardType.Queen) == 1)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }

                if (communityCards.Count(c => c.Type == CardType.Ace || c.Type == CardType.King || c.Type == CardType.Queen) == 3)
                {
                    return PlayerAction.Raise(context.MoneyLeft);
                }
            }

            if (context.CanCheck)
            {
                return PlayerAction.CheckOrCall();
            }

            return PlayerAction.Fold();
        }
    }
}
