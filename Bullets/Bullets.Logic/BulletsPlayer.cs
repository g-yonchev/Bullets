namespace Bullets.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using TexasHoldem.Logic;
	using TexasHoldem.Logic.Helpers;
	using TexasHoldem.Logic.Cards;
	using TexasHoldem.Logic.Players;

	using Bullets.Logic.CardValuation;
	using MonteCarlo;

	public class BulletsPlayer : BasePlayer
    {
        private IMonteCarlo monteCarlo = new MonteCarloAlgorithm();

		private float monteCarloChance = -1;

        public override string Name { get; } = $"BulletsPlayer_{Guid.NewGuid()}";

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return this.BulletsStrategy(context);
        }

        private PlayerAction BulletsStrategy(GetTurnContext context)
        {
            var roundType = context.RoundType;

            if (roundType == GameRoundType.PreFlop)
            {
                return this.PreFlopAction(context);
            }
            else if (roundType == GameRoundType.Flop)
            {
                return this.FlopAction(context);
            }
            else if (roundType == GameRoundType.Turn)
            {
                // Turn
                return this.FlopAction(context);
            }
            else
            {
                // River
                return this.FlopAction(context);
            }
        }

        private PlayerAction PreFlopAction(GetTurnContext context)
        {
            CardStrengthType playHand;

            bool isOurTurn = context.PreviousRoundActions.Count == 2;

            if (context.MoneyLeft <= 500)
            {
                playHand = HandStrengthValuation.PreFlopHandStrengthValuation(this.FirstCard, this.SecondCard);

                if (playHand == CardStrengthType.BestHand)
                {
                    return PlayerAction.Raise(context.MoneyLeft + 1);
                }

                if (playHand == CardStrengthType.Playable)
                {
                    return PlayerAction.Raise(context.CurrentPot * 4);
                }

            }
            else if (context.MoneyLeft >= 1500)
            {
                playHand = HandStrengthValuation.PreFlopHandStrengthValuation(this.FirstCard, this.SecondCard);

                if (playHand == CardStrengthType.BestHand)
                {
                    return PlayerAction.Raise(context.MoneyLeft + 1);
                }

                if (playHand == CardStrengthType.Playable)
                {
                    return PlayerAction.Raise(context.CurrentPot * 4);
                }
            }
            else
            {
                playHand = HandStrengthValuation.PreFlopHandStrengthValuation(this.FirstCard, this.SecondCard);
            }
			
            if (context.SmallBlind <= 250)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.Raise(20);
                }
                else
                {
                    return PlayerAction.CheckOrCall();
                }
            }
			
            if (playHand == CardStrengthType.Unplayable)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }

                throw new NotImplementedException("Somewhere there is missing logic");
            }
            else if (playHand == CardStrengthType.Playable)
            {
                if (isOurTurn)
                {
                    return PlayerAction.Raise(3 * context.SmallBlind);
                }
                // there is an anction
                else
                {
                    var previousRoundActions = context.PreviousRoundActions.ToList();

					// opponent had an action
                    if (!previousRoundActions[previousRoundActions.Count - 1].PlayerName.Contains(this.Name))
                    {
                        // opponent made a raise
                        if (previousRoundActions[previousRoundActions.Count - 1].Action.Type == PlayerActionType.Raise)
                        {
							// 10% to call
                            if (context.MoneyToCall <= context.MoneyLeft * 0.1m)
                            {
                                return PlayerAction.CheckOrCall();
                            }
                            else
                            {
                                return PlayerAction.Fold();
                            }
                        }

                        if (context.CanCheck)
                        {
                            return PlayerAction.CheckOrCall();
                        }

                        if (context.MoneyToCall <= context.MoneyLeft * 0.1m)
                        {
                            return PlayerAction.CheckOrCall();
                        }
                        else
                        {
                            return PlayerAction.Fold();
                        }

                        throw new NotImplementedException("Somewhere there is missing logic");

                    }

                    throw new NotImplementedException("Somewhere there is missing logic");

                }

                throw new NotImplementedException("Somewhere there is missing logic");
            }
            else if (playHand == CardStrengthType.BestHand)
            {
                if (isOurTurn)
                {
                    return PlayerAction.Raise(4 * context.SmallBlind);
                }
                // there is an anction
                else
                {
                    var previousRoundActions = context.PreviousRoundActions.ToList();

					// opponent had an action
					if (!previousRoundActions[previousRoundActions.Count - 1].PlayerName.Contains(this.Name))
                    {
                        // opponent had raise
                        if (previousRoundActions[previousRoundActions.Count - 1].Action.Type == PlayerActionType.Raise)
                        {
                            // first met re-raise.
                            if (previousRoundActions.Count == 3)
                            {
                                return PlayerAction.Raise(context.MoneyToCall * 3);
                            }
                            else
                            {
                                return PlayerAction.Raise(context.MoneyLeft + 1);
                            }
                        }
                        else
                        {
                            if (context.CanCheck)
                            {
                                return PlayerAction.CheckOrCall();
                            }

                            if (context.MoneyToCall <= context.MoneyLeft * 0.15m)
                            {
                                return PlayerAction.CheckOrCall();
                            }
                            else
                            {
                                return PlayerAction.Fold();
                            }

                            throw new NotImplementedException("Somewhere there is missing logic");
                        }
                    }
                    else
                    {
                        return PlayerAction.Raise(context.MoneyToCall * 3);
                    }
                }

                throw new NotImplementedException("Somewhere there is missing logic");
            }

            throw new NotImplementedException("Somewhere there is missing logic");
        }

        private PlayerAction FlopAction(GetTurnContext context)
        {
			float chance = monteCarlo.CalculateWinningChance(this.FirstCard, this.SecondCard, this.CommunityCards);
			
            if (context.MoneyLeft < 500)
            {
                return this.MonteCarloLessThan500(chance, context);
                
            }
            if (context.MoneyLeft > 1500)
            {
                return this.MonteCarloGreaterThan1500(chance, context);
            }

            return this.MonteCarloNormal(chance, context);
        }

        private PlayerAction MonteCarloLessThan500(float chance, GetTurnContext context)
        {
            if (chance < 27)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            if (chance < 55)
            {
                return PlayerAction.CheckOrCall();
            }

            if (chance < 80)
            {
                return PlayerAction.Raise(context.CurrentPot);
            }

            return PlayerAction.Raise(context.MoneyLeft + 1);

        }

        private PlayerAction MonteCarloGreaterThan1500(float chance, GetTurnContext context)
        {
            if (chance < 25)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            if (chance < 50)
            {
                return PlayerAction.CheckOrCall();
            }

            if (chance < 82)
            {
                return PlayerAction.Raise(context.CurrentPot);
            }

            return PlayerAction.Raise(context.MoneyLeft + 1);

        }

        private PlayerAction MonteCarloNormal(float chance,  GetTurnContext context)
        {
            if (chance < 33)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            if (chance < 66)
            {
                return PlayerAction.CheckOrCall();
            }

            if (chance < 90)
            {
                return PlayerAction.Raise(context.CurrentPot);
            }

            return PlayerAction.Raise(context.MoneyLeft + 1);
        }

        private PlayerAction TurnAction(GetTurnContext context)
        {
            var cards = new List<Card>(this.CommunityCards);
            cards.Add(this.FirstCard);
            cards.Add(this.SecondCard);
            var hand = Helpers.GetHandRank(cards);

            if (hand == HandRankType.StraightFlush)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);
            }
            if (hand == HandRankType.FourOfAKind)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);
            }
            if (hand == HandRankType.FullHouse)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);

            }
            if (hand == HandRankType.Flush)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);

            }
            if (hand == HandRankType.Straight)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);
            }
            if (hand == HandRankType.ThreeOfAKind)
            {
                int bet = (int)Math.Round(0.90 * context.CurrentPot);
                return PlayerAction.Raise(bet);

            }
            if (hand == HandRankType.TwoPairs)
            {
                int bet = (int)Math.Round(0.80 * context.CurrentPot);
                return PlayerAction.Raise(bet);

            }

            if (hand == HandRankType.Pair)
            {
                int bet = (int)Math.Round(0.66 * context.CurrentPot);
                return PlayerAction.Raise(bet);
            }

            if (hand == HandRankType.HighCard)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            throw new NotImplementedException("Missing in the logic");
        }

        private PlayerAction RiverAction(GetTurnContext context)
        {
            var cards = new List<Card>(this.CommunityCards);
            cards.Add(this.FirstCard);
            cards.Add(this.SecondCard);
            var hand = Helpers.GetHandRank(cards);

            if (hand == HandRankType.StraightFlush)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);
            }
            if (hand == HandRankType.FourOfAKind)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);
            }
            if (hand == HandRankType.FullHouse)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);

            }
            if (hand == HandRankType.Flush)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);

            }
            if (hand == HandRankType.Straight)
            {
                return PlayerAction.Raise(context.MoneyLeft + 1);
            }
            if (hand == HandRankType.ThreeOfAKind)
            {
                int bet = (int)Math.Round(0.66 * context.CurrentPot);
                return PlayerAction.Raise(bet);

            }
            if (hand == HandRankType.TwoPairs)
            {
                int bet = (int)Math.Round(0.66 * context.CurrentPot);
                return PlayerAction.Raise(bet);

            }

            if (hand == HandRankType.Pair)
            {
                int bet = (int)Math.Round(0.66 * context.CurrentPot);
                return PlayerAction.Raise(bet);
            }

            if (hand == HandRankType.HighCard)
            {
                if (context.CanCheck)
                {
                    return PlayerAction.CheckOrCall();
                }
                else
                {
                    return PlayerAction.Fold();
                }
            }

            throw new NotImplementedException("Missing in the logic");
        }
    }
}
