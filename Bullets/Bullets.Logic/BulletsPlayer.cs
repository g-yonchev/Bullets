namespace Bullets.Logic
{
    using TablesForGoodCards;
    using EvaluationCriteriasPreFlop.EvaluationsWinnings;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Helpers;
    using TexasHoldem.Logic.Cards;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    public class BulletsPlayer : BasePlayer
    {
        private IMonteCarlo monteCarlo = new MonteCarlo();

        public override string Name { get; } = $"BulletsPlayer_{Guid.NewGuid()}";

        // logikata koqto e implementiral niki za igrata: pri vsqko pochvane na rund i dvata playera imat Raise(1..2...3.. increesing)
        public override PlayerAction GetTurn(GetTurnContext context)
        {
            return this.SecondStrategy(context);
        }

        private PlayerAction SecondStrategy(GetTurnContext context)
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
            CardValuationType playHand;

            // promenliva koqto pazi dali nie sme bili purvi ( ima samo dva raise v actionite)
            bool isOurTurn = context.PreviousRoundActions.Count == 2;

            if (context.MoneyLeft <= 500)
            {
                playHand = HandStrengthValuation.PreFlopNormal(this.FirstCard, this.SecondCard);

                if (playHand == CardValuationType.BestHand)
                {
                    return PlayerAction.Raise(context.MoneyLeft + 1);
                }

                if (playHand == CardValuationType.Playable)
                {
                    return PlayerAction.Raise(context.CurrentPot * 4);
                }

            }
            else if (context.MoneyLeft >= 1500)
            {
                playHand = HandStrengthValuation.PreFlopNormal(this.FirstCard, this.SecondCard);

                if (playHand == CardValuationType.BestHand)
                {
                    return PlayerAction.Raise(context.MoneyLeft + 1);
                }

                if (playHand == CardValuationType.Playable)
                {
                    return PlayerAction.Raise(context.CurrentPot * 4);
                }
            }
            else
            {
                playHand = HandStrengthValuation.PreFlopNormal(this.FirstCard, this.SecondCard);
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

            // podredi
            if (playHand == CardValuationType.Unplayable)
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
            else if (playHand == CardValuationType.Playable)
            {
                if (isOurTurn)
                {
                    return PlayerAction.Raise(3 * context.SmallBlind);
                }
                // there is an anction
                else
                {
                    var previousRoundActions = context.PreviousRoundActions.ToList();

                    if (!previousRoundActions[previousRoundActions.Count - 1].PlayerName.Contains(this.Name))
                    {
                        // opponent had raise
                        if (previousRoundActions[previousRoundActions.Count - 1].Action.Type == PlayerActionType.Raise)
                        {
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
            else if (playHand == CardValuationType.BestHand)
            {
                if (isOurTurn)
                {
                    return PlayerAction.Raise(4 * context.SmallBlind);
                }
                // there is an anction
                else
                {
                    var previousRoundActions = context.PreviousRoundActions.ToList();

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
            var chance = monteCarlo.CalculateWinningChance(
                this.FirstCard,
                this.SecondCard,
                this.CommunityCards);

            if (context.MoneyLeft < 500)
            {
                return this.MonteCarloLessThan500(chance, context);
                
            }
            if (context.MoneyLeft > 1500)
            {
                return this.MonteCarloGreaterThan1500(chance, context);
            }

            return this.MonteCarloNormal(chance, context);

            
            //var cards = new List<Card>(this.CommunityCards);
            //cards.Add(this.FirstCard);
            //cards.Add(this.SecondCard);
            //var hand = Helpers.GetHandRank(cards);

            //if (hand == HandRankType.StraightFlush)
            //{
            //    return PlayerAction.Raise(context.MoneyLeft + 1);
            //}
            //if (hand == HandRankType.FourOfAKind)
            //{
            //    return PlayerAction.Raise(context.MoneyLeft + 1);
            //}
            //if (hand == HandRankType.FullHouse)
            //{
            //    return PlayerAction.Raise(context.MoneyLeft + 1);

            //}
            //if (hand == HandRankType.Flush)
            //{
            //    return PlayerAction.Raise(context.MoneyLeft + 1);

            //}
            //if (hand == HandRankType.Straight)
            //{
            //    return PlayerAction.Raise(context.MoneyLeft + 1);
            //}
            //if (hand == HandRankType.ThreeOfAKind)
            //{
            //    int bet = (int)Math.Round(0.90 * context.CurrentPot);
            //    return PlayerAction.Raise(bet);

            //}
            //if (hand == HandRankType.TwoPairs)
            //{
            //    int bet = (int)Math.Round(0.80 * context.CurrentPot);
            //    return PlayerAction.Raise(bet);
            //}

            //if (hand == HandRankType.Pair)
            //{
            //    int bet = (int)Math.Round(0.66 * context.CurrentPot);
            //    return PlayerAction.Raise(bet);
            //}

            //if (hand == HandRankType.HighCard)
            //{
            //    if (context.CanCheck)
            //    {
            //        return PlayerAction.CheckOrCall();
            //    }
            //    else
            //    {
            //        return PlayerAction.Fold();
            //    }
            //}

            //throw new NotImplementedException("Missing in the logic");
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

        //private PlayerAction NomalStrategy(GetTurnContext context)
        //{
        //	if (context.RoundType == GameRoundType.PreFlop)
        //	{
        //		// this is first playing in the round. Each player starts with one Raise(1..2...3.. increesing)
        //		// and this logic and tables are from EvaluationAfterNoAction
        //		if (context.PreviousRoundActions.Count == 2)
        //		{
        //			// this is returned random number
        //			var rand = RandomProvider.Next(1, 10001);

        //			// this is randomizied percentage
        //			var randFloat = rand / (float)10000;


        //			// this is percentage from the table.
        //			var raisePercentage = EvaluationAfterNoAction.RaisePercentage(this.FirstCard, this.SecondCard);

        //			if (randFloat <= raisePercentage)
        //			{

        //				var raiseValue = (int)(raisePercentage + 1);
        //				//var raiseValue = 1;
        //				return PlayerAction.Raise(3);
        //			}

        //			var callPercentage = EvaluationAfterNoAction.CallPercentage(this.FirstCard, this.SecondCard);

        //			if (randFloat <= callPercentage)
        //			{
        //				return PlayerAction.CheckOrCall();
        //			}

        //			return PlayerAction.Fold();
        //		}


        //		var isCall = context.PreviousRoundActions.ToList().Any(a => !a.PlayerName.Contains("Bullets") && a.Action.Type == PlayerActionType.CheckCall);

        //		if (isCall)
        //		{
        //			// this is coeficient from the tables
        //			var raisePerc = EvaluationAfterOpponentsCall.RaisePercentage(this.FirstCard, this.SecondCard);

        //			// this is returned random number
        //			var rand = RandomProvider.Next(1, 10001);

        //			// this is randomizied coeficient
        //			var randFloat = rand / (float)10000;

        //			var winningPercentage = EvaluationWinning.WinningPercentage(this.FirstCard, this.SecondCard);

        //			if (randFloat <= raisePerc)
        //			{
        //				var raiseValue = (int)(Math.Round(winningPercentage * RandomProvider.Next(20, 51)));
        //				return PlayerAction.Raise(raiseValue);
        //			}
        //			else
        //			{

        //				return PlayerAction.CheckOrCall();
        //			}
        //		}
        //	}

        //	return PlayerAction.CheckOrCall();
        //}

    }
}
