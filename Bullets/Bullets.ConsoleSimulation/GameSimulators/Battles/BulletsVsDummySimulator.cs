namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	using Bullets.AIs;
	using Bullets.Logic;
	using TexasHoldem.Logic.Players;

	public class BulletsVsDummySimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new BulletPlayer();
		private readonly IPlayer secondPlayer = new DummyPlayer();

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
