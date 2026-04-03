using System;
using Core;

namespace Asteroids {
  public class Asteroid : IChroneListener {
    private static int EchosDecrement;
    private static int MinEchosValue;
    private static int MaxEchosValue;
    private static int InitialSpawnId;

    public static int nextCreateId;
    public int currentEchos;
    public int maxEchos;
    public AsteroidState state;
    public int spawnId;
    public int createId;
    public static int nextSpawnId;

    static Asteroid()
    {
      EchosDecrement = 100;
      MinEchosValue = 100;
      MaxEchosValue = 1000;
      InitialSpawnId = -1;
      nextCreateId = 0;
      nextSpawnId = 0;
    }

    public Asteroid()
    {
      Random rand;

      rand = new Random();
      maxEchos = rand.Next(MinEchosValue, MaxEchosValue + 1);
      currentEchos = maxEchos;
      state = AsteroidState.Idle;
      createId = nextCreateId;
      ++nextCreateId;
      spawnId = InitialSpawnId;
    }

    public void Reset()
    {
      currentEchos = maxEchos;
      state = AsteroidState.Idle;
    }

    public void SetSpawnId()
    {
      spawnId = nextSpawnId;
      ++nextSpawnId;
    }

    public void OnChroneTick()
    {
      if (state == AsteroidState.Idle)
      {
        currentEchos = currentEchos - EchosDecrement;
        if (currentEchos <= 0)
        {
          currentEchos = 0;
          state = AsteroidState.Depleted;
        }
      }
    }

    public void PrintInfo()
    {
      Console.WriteLine($"  Астероид [CreateId: {createId}, SpawnId: {spawnId}] | " +
                        $"Echos: {currentEchos}/{maxEchos} | Состояние: {state}");
    }
  }
}