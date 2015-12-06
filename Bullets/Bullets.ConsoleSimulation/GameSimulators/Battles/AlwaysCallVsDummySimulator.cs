using Bullets.AIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TexasHoldem.Logic.Players;

namespace Bullets.ConsoleSimulation.GameSimulators.Battles
{
	public class AlwaysCallVsDummySimulator : BaseGameSimulator
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
