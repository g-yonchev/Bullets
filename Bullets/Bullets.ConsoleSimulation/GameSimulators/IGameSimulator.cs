namespace Bullets.ConsoleSimulation.GameSimulators
{
	public interface IGameSimulator
	{
		GameSimulationResult Simulate(int numberOfGames);
	}
}
