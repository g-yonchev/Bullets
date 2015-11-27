namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	using AIs;
	using Logic;
	using TexasHoldem.Logic.Players;

	public class BulletsVsAlwaysFoldSimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new BulletsPlayer();
		private readonly IPlayer secondPlayer = new AlwaysFoldPlayer();

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
