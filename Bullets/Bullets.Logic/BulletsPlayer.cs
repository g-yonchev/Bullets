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
		// these are for testing and printing purpouses.
		private GetTurnContext context;
		private readonly List<string> players = new List<string>();

		public override string Name { get; } = $"BulletsPlayer_{Guid.NewGuid()}";

		// logikata koqto e implementiral niki za igrata: pri vsqko pochvane na rund i dvata playera imat Raise(1..2...3.. increesing)
		public override PlayerAction GetTurn(GetTurnContext context)
		{

			this.context = context;

			return this.SecondStrategy(context);

			return this.NomalStrategy(context);

			// this is the strategy from smart player. it would be invoke when whe proove that the player is alwayscall. but how?! :D
			//return this.StrategyFromSmartPlayer(context);

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
				return this.TurnAction(context);
			}
			else
			{
				return this.RiverAction(context);
			}
		}

		private PlayerAction PreFlopAction(GetTurnContext context)
		{
			CardValuationType playHand;

			// promenliva koqto pazi dali nie sme bili purvi ( ima samo dva raise v actionite)
			bool isOurTurn = context.PreviousRoundActions.Count == 2;

			if (context.MoneyLeft <= 500)
			{
				playHand = HandStrengthValuation.PreFlopLessThan500Money(this.FirstCard, this.SecondCard);
			}
			else if (context.MoneyLeft >= 1500)
			{
				playHand = HandStrengthValuation.PreFlopGreaterThan500Money(this.FirstCard, this.SecondCard);
			}
			else
			{
				playHand = HandStrengthValuation.PreFlopNormal(this.FirstCard, this.SecondCard);
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
			if (hand ==  HandRankType.TwoPairs)
			{
				int bet = (int)Math.Round(0.80 * context.CurrentPot);
				return PlayerAction.Raise(bet);
			}

			if (hand ==HandRankType.Pair)
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

		private PlayerAction NomalStrategy(GetTurnContext context)
		{
			if (context.RoundType == GameRoundType.PreFlop)
			{
				// this is first playing in the round. Each player starts with one Raise(1..2...3.. increesing)
				// and this logic and tables are from EvaluationAfterNoAction
				if (context.PreviousRoundActions.Count == 2)
				{
					// this is returned random number
					var rand = RandomProvider.Next(1, 10001);

					// this is randomizied percentage
					var randFloat = rand / (float)10000;


					// this is percentage from the table.
					var raisePercentage = EvaluationAfterNoAction.RaisePercentage(this.FirstCard, this.SecondCard);

					if (randFloat <= raisePercentage)
					{

						var raiseValue = (int)(raisePercentage + 1);
						//var raiseValue = 1;
						return PlayerAction.Raise(3);
					}

					var callPercentage = EvaluationAfterNoAction.CallPercentage(this.FirstCard, this.SecondCard);

					if (randFloat <= callPercentage)
					{
						return PlayerAction.CheckOrCall();
					}

					return PlayerAction.Fold();
				}


				var isCall = context.PreviousRoundActions.ToList().Any(a => !a.PlayerName.Contains("Bullets") && a.Action.Type == PlayerActionType.CheckCall);

				if (isCall)
				{
					// this is coeficient from the tables
					var raisePerc = EvaluationAfterOpponentsCall.RaisePercentage(this.FirstCard, this.SecondCard);

					// this is returned random number
					var rand = RandomProvider.Next(1, 10001);

					// this is randomizied coeficient
					var randFloat = rand / (float)10000;

					var winningPercentage = EvaluationWinning.WinningPercentage(this.FirstCard, this.SecondCard);

					if (randFloat <= raisePerc)
					{
						var raiseValue = (int)(Math.Round(winningPercentage * RandomProvider.Next(20, 51)));
						return PlayerAction.Raise(raiseValue);
					}
					else
					{

						return PlayerAction.CheckOrCall();
					}
				}
			}

			return PlayerAction.CheckOrCall();
		}

		private void Print()
		{
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.Clear();
			if (players.Count == 0)
			{
				foreach (var item in this.context.PreviousRoundActions)
				{
					players.Add(item.PlayerName.Substring(0, item.PlayerName.Length - 43));
				}
			}

			Console.WriteLine();
			Console.WriteLine();

			Console.WriteLine(string.Join("Vs", players));
			Console.WriteLine($"MoneyLeft: {context.MoneyLeft} $");
			Console.WriteLine();
			Console.WriteLine($"===={this.context.RoundType}====");
			Console.Write($"Cards: ");
			PrintCards(FirstCard, SecondCard);
			Console.WriteLine();
			Console.Write($"       ");
			PrintCards(this.CommunityCards.ToArray());

			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine($"-CanCheck: {context.CanCheck}");
			Console.WriteLine($"-CurrentMaxBet: {context.CurrentMaxBet}");
			Console.WriteLine($"-CurrentPot: {context.CurrentPot}");
			Console.WriteLine($"-IsAllIn: {context.IsAllIn}");
			Console.WriteLine($"-MoneyToCall: {context.MoneyToCall}");
			Console.WriteLine($"-MyMoneyInTheRound: {context.MyMoneyInTheRound}");
			Console.WriteLine($"-SmallBlind: {context.SmallBlind}");
			Console.WriteLine();
			Console.WriteLine("PreviousRoundActions:");
			foreach (var action in this.context.PreviousRoundActions)
			{
				if (action.PlayerName.Contains("Bullets"))
				{
					Console.WriteLine($"Bullets: {action.Action}");
				}
				else
				{
					Console.WriteLine($"Opponent: {action.Action}");
				}
			}
			Console.WriteLine();
			Console.WriteLine();

			var key = Console.ReadKey();
			if (key.Key == ConsoleKey.Escape)
			{
				Environment.Exit(0);
			}
		}

		private void PrintCards(params Card[] cards)
		{
			foreach (var card in cards)
			{
				if (card.Suit == CardSuit.Diamond || card.Suit == CardSuit.Heart)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Black;
				}
				Console.Write(card + " ");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
		}
	}
}
