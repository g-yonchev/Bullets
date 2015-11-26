using Bullets.AIs;
using TexasHoldem.Logic.Players;

namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	public class AlwaysCallVsAlwaysCallSimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new AlwaysCallPlayer();
		private readonly IPlayer secondPlayer = new AlwaysCallPlayer();

		protected override IPlayer GetFirstPlayer()
		{
			return this.firstPlayer;
		}

		protected override IPlayer GetSecondPlayer()
		{
			return this.secondPlayer;
		}
	}
}
