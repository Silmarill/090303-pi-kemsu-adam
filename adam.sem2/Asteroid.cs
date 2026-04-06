using System;

namespace AsteroidSimulator.Models {
  public class Asteroid : Interfaces.IChronListener {
    public static int s_globalCreateCounter = 0;
    public static int s_globalSpawnCounter = 0;

    public int CurrentEchos { get; private set; }
    public int MaxEchos { get; private set; }
    public AsteroidState State { get; private set; }
    public int CreateId { get; private set; }
    public int SpawnId { get; private set; }

    public Asteroid()
    {
      Random random = new Random();
      int minEchos = 100;
      int maxEchos = 1000;

      MaxEchos = random.Next(minEchos, maxEchos);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      CreateId = ++s_globalCreateCounter;
      SpawnId = 0;
    }

    public void Reset()
    {
      Random random = new Random();
      int minEchos = 100;
      int maxEchos = 1000;

      MaxEchos = random.Next(minEchos, maxEchos);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnId = ++s_globalSpawnCounter;
    }

    public void OnChronTick()
    {
      if (State == AsteroidState.Idle)
      {
        CurrentEchos -= 100;

        if (CurrentEchos <= 0)
        {
          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        }
      }
    }

    public override string ToString()
    {
      return $"Asteroid #{SpawnId} (Created: #{CreateId}) | Echos: {CurrentEchos}/{MaxEchos} | State: {State}";
    }
  }
}