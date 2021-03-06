﻿namespace Bullets.AIs
{
	using System;

	using TexasHoldem.Logic;
	using TexasHoldem.Logic.Extensions;
	using TexasHoldem.Logic.Players;

	using Helpers;

	// TODO: This player is far far away from being smart!
	public class SmartPlayer : BasePlayer
	{
		public override string Name { get; } = "SmartPlayer_" + Guid.NewGuid();
		
		public override PlayerAction GetTurn(GetTurnContext context)
		{
			if (context.RoundType == GameRoundType.PreFlop)
			{
				var playHand = HandStrengthValuation.PreFlop(this.FirstCard, this.SecondCard);
				if (playHand == CardValuationType.Unplayable)
				{
					// ako drugiq e purvi i raisne to tuka ne moje da se vlezne. can check = false
					if (context.CanCheck)
					{
						return PlayerAction.CheckOrCall();
					}
					else
					{
						return PlayerAction.Fold();
					}
				}

				if (playHand == CardValuationType.Risky)
				{
					var smallBlindsTimes = RandomProvider.Next(1, 8);
					return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
				}

				if (playHand == CardValuationType.Recommended)
				{
					var smallBlindsTimes = RandomProvider.Next(6, 14);
					return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
				}

				return PlayerAction.CheckOrCall();
			}

			return PlayerAction.CheckOrCall();
		}
	}
}
