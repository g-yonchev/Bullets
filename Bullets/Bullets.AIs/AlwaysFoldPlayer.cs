namespace Bullets.AIs
{
	using System;

	using TexasHoldem.Logic.Players;

	public class AlwaysFoldPlayer : BasePlayer
	{
		public override string Name { get; } = "AlwaysFoldDummyPlayer_" + Guid.NewGuid();

		public override PlayerAction GetTurn(GetTurnContext context)
		{
			return PlayerAction.Fold();
		}
	}
}
