using System;
using AsteroidSimulation.Chron;

namespace AsteroidSimulation.Models
{
  public class Asteroid : IChronListener
  {
    public static int globalCreateCounter = 0;
    public static int globalSpawnCounter = 0;
    public static Random random = new Random();
    public static int MaxEchosValue = 1000;
    public static int echosLossPerTick = 100;
    public static int rangeOffset = 1;


    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Asteroid()
    {
      CreateID = ++globalCreateCounter;
      MaxEchos = random.Next(echosLossPerTick, MaxEchosValue + rangeOffset);
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
