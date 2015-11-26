namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	using AIs;
	using TexasHoldem.Logic.Players;

	public class SmartVsAlwaysCallSimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new SmartPlayer();

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
