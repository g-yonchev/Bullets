namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
    using Logic;
    using TexasHoldem.Logic.Players;

    public class BulletsVsHackerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new BulletsPlayer();
        private readonly IPlayer secondPlayer = new HackerPlayer();

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
