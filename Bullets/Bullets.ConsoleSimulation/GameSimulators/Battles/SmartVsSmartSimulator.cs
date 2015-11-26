namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	using AIs;
	using TexasHoldem.Logic.Players;

	public class SmartVsSmartSimulator : BaseGameSimulator
	{
		private readonly IPlayer firstPlayer = new SmartPlayer();
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
