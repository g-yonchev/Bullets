namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	using Logic;
	using TexasHoldem.Logic.Players;

	public class BulletsVsBulletsSimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new BulletsPlayer();
		private readonly IPlayer secondPlayer = new BulletsPlayer();

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
