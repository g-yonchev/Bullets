namespace Bullets.ConsoleSimulation
{
	using System;

	using Bullets.ConsoleSimulation.GameSimulators;
	using Bullets.ConsoleSimulation.GameSimulators.Battles;

	public class Program
	{
		public static void Main(string[] args)
		{
			//SimulateGames(new BulletsVsBulletsSimulator());
			//SimulateGames(new BulletsVsAlwaysCallSimulator());
			//SimulateGames(new BulletsVsDummySimulator());
			//SimulateGames(new SmartVsDummySimulator());
			SimulateGames(new BulletsVsSmartSimulator());
		}

		private static void SimulateGames(IGameSimulator gameSimulator)
		{
			Console.WriteLine($"Running {gameSimulator.GetType().Name}...");

			var simulationResult = gameSimulator.Simulate(10000);
			//var simulationResult = gameSimulator.Simulate(1);

			Console.WriteLine(simulationResult.SimulationDuration);
			Console.WriteLine($"Total games: {simulationResult.FirstPlayerWins:0,0} - {simulationResult.SecondPlayerWins:0,0}");
			Console.WriteLine($"Hands played: {simulationResult.HandsPlayed:0,0}");
			Console.WriteLine(new string('=', 75));
		}
	}
}
