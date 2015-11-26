namespace Bullets.AIs
{
	using System;

	using TexasHoldem.Logic.Players;

	public class AlwaysCallPlayer : BasePlayer
	{
		public override string Name { get; } = "AlwaysCallDummyPlayer_" + Guid.NewGuid();

		public override PlayerAction GetTurn(GetTurnContext context)
		{
			return PlayerAction.CheckOrCall();
		}
	}
}
