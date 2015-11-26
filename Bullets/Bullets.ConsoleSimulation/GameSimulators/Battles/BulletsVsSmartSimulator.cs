namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	using AIs;
	using Logic;
	using TexasHoldem.Logic.Players;

	public class BulletsVsSmartSimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new BulletPlayer();
		private readonly IPlayer secondPlayer = new SmartPlayer();

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
