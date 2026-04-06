using System;
using AsteroidSimulation.Chron;

namespace AsteroidSimulation.Models
{
  public class Asteroid : IChronListener
  {
    private static int globalCreateCounter = 0;
    private static int globalSpawnCounter = 0;

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    private const int echosLossPerTick = 100;
    private static readonly Random random = new Random();

    public Asteroid()
    {
      CreateID = ++globalCreateCounter;
      MaxEchos = random.Next(100, 1001);
      Reset();
    }

    public void Reset()
    {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnID = ++globalSpawnCounter;
    }

    public void OnChronTick()
    {
      if (State == AsteroidState.Idle)
      {
        CurrentEchos -= echosLossPerTick;
        if (CurrentEchos <= 0)
        {
          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        }
      }
    }

    public void PrintInfo()
    {
      Console.WriteLine($"Астероид [CreateID: {CreateID}, SpawnID: {SpawnID}, MaxEchos: {MaxEchos}, CurrentEchos: {CurrentEchos}, State: {State}]");
    }
  }
}
