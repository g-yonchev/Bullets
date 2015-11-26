namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	using Logic;
	using TexasHoldem.Logic.Players;

	public class BulletsVsBulletsSimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new BulletPlayer();
		private readonly IPlayer secondPlayer = new BulletPlayer();

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
