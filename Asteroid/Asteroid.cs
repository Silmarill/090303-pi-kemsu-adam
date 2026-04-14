using Asteroid;
using System;

namespace Asteroid {
  public enum AsteroidState {
    Idle,
    Mining,
    Depleted
  }

  public class Asteroid : IChronListener {
    private static int _nextCreateId = 1;

    public AsteroidState state;
    public int currentEchos;
    public int maxEchos;
    public int spawnId;
    public int createId;

    public Asteroid() {
      Random rnd;
      rnd = new Random();

      int asteroidResMin;
      asteroidResMin = 100;

      int asteroidResMax;
      asteroidResMax = 1001;

      maxEchos = rnd.Next(asteroidResMin, asteroidResMax);
      currentEchos = maxEchos;
      state = AsteroidState.Idle;
      createId = ++_nextCreateId;
      spawnId = 0;
    }

    public void Reset() {
      currentEchos = maxEchos;
      state = AsteroidState.Idle;
      spawnId = 0;
    }

    public void OnChronTick() {
    }

    public void SetSpawnId(int id) {
      spawnId = id;
    }

    public void StartMining() {
      if (state == AsteroidState.Idle) {
        state = AsteroidState.Mining;
      }
    }

    public void StopMining() {
      if (state == AsteroidState.Mining) {
        state = AsteroidState.Idle;
      }
    }

    public int Mine(int biteSize) {
      int minedAmount;

      if (state != AsteroidState.Mining) {
        return 0;
      }

      if (currentEchos <= 0) {
        state = AsteroidState.Depleted;

        return 0;
      }

      if (currentEchos >= biteSize) {
        minedAmount = biteSize;
      } 
      
      else {
        minedAmount = currentEchos;
      }

      currentEchos = currentEchos - minedAmount;

      if (currentEchos == 0) {
        state = AsteroidState.Depleted;
      }

      return minedAmount;
    }

    public override string ToString() {
      string stateText;

      if (state == AsteroidState.Idle) {
        stateText = "Idle";
      } 
      
      else if (state == AsteroidState.Mining) {
        stateText = "Mining";
      } 
      
      else {
        stateText = "Depleted";
      }

      return $"[{createId}:{spawnId}] {currentEchos}/{maxEchos} | {stateText}";
    }
  }
}