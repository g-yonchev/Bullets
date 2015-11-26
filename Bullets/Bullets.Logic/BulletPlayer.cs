namespace Bullets.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using TexasHoldem.Logic.Cards;
	using TexasHoldem.Logic.Extensions;
	using TexasHoldem.Logic.Players;

	public class BulletPlayer : BasePlayer
	{
		private GetTurnContext context;
		private readonly List<string> players = new List<string>();

		public override string Name { get; } = $"BulletsPlayer_{Guid.NewGuid()}";

		public override PlayerAction GetTurn(GetTurnContext context)
		{
			this.context = context;
			//this.Print();
			var action = PlayerAction.Raise(5);
			return action;
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
