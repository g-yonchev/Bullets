namespace Bullets.ConsoleSimulation
{
	using System;

	using Bullets.ConsoleSimulation.GameSimulators;
	using Bullets.ConsoleSimulation.GameSimulators.Battles;

	public class Program
	{
		public static void Main(string[] args)
		{
            SimulateGames(new BulletsVsSmartSimulator());
            SimulateGames(new BulletsVsSmartSimulator());
            SimulateGames(new BulletsVsSmartSimulator());
		    Console.WriteLine();
            SimulateGames(new BulletsVsHackerSimulator());
            SimulateGames(new BulletsVsHackerSimulator());
            SimulateGames(new BulletsVsHackerSimulator());
        }

		private static void SimulateGames(IGameSimulator gameSimulator)
		{
			Console.WriteLine($"Running {gameSimulator.GetType().Name}...");

			var simulationResult = gameSimulator.Simulate(100);

			Console.WriteLine(simulationResult.SimulationDuration);
			Console.WriteLine($"Total games: {simulationResult.FirstPlayerWins:0,0} - {simulationResult.SecondPlayerWins:0,0}");
			Console.WriteLine($"Hands played: {simulationResult.HandsPlayed:0,0}");
			Console.WriteLine(new string('=', 75));
		}
	}
}
