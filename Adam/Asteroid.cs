using System;
using Core;

namespace Asteroids {
  public class Asteroid : IChroneListener {
    public int EchosDecrement;
    public int MinEchosValue;
    public int MaxEchosValue;
    public int InitialSpawnId;

    public int nextCreateId;
    public int currentEchos;
    public int maxEchos;
    public AsteroidState state;
    public int spawnId;
    public int createId;
    public int nextSpawnId;

    public Asteroid()
    {
      Random rand;
      int maxEchosValueInclusive;
      int offsetForRandom;

      offsetForRandom = 1;
      EchosDecrement = 100;
      MinEchosValue = 100;
      MaxEchosValue = 1000;
      InitialSpawnId = -1;
      nextCreateId = 0;
      nextSpawnId = 0;

      rand = new Random();
      maxEchosValueInclusive = MaxEchosValue + offsetForRandom;
      maxEchos = rand.Next(MinEchosValue, maxEchosValueInclusive);
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
    }

    public void PrintInfo()
    {
      Console.WriteLine($"  Астероид [CreateId: {createId}, SpawnId: {spawnId}] | " + $"Echos: {currentEchos}/{maxEchos} | Состояние: {state}");
    }

    public void Mine(int biteSize)
    {
      if (state == AsteroidState.Mining)
      {
        if (currentEchos >= biteSize)
        {
          currentEchos = currentEchos - biteSize;
        }
        else
        {
          currentEchos = 0;
        }

        if (currentEchos <= 0)
        {
          currentEchos = 0;
          state = AsteroidState.Depleted;
        }
      }
    }

    public bool StartMining()
    {
      if (state == AsteroidState.Idle)
      {
        state = AsteroidState.Mining;
        return true;
      }
      return false;
    }
  }
}