using Asteroid;
using System;

namespace Asteroid {
  public enum AsteroidState {
    Idle,
    Depleted
  }

  public class Asteroid : IChronListener {
    private static int _nextCreateId = 1;

    public int currentEchos;
    public int maxEchos;
    public AsteroidState state;
    public int spawnId;
    public int createId;

    public Asteroid() {
      int asteroidResMin;
      asteroidResMin = 100;

      int asteroidResMax;
      asteroidResMax = 1001;

      Random rnd = new Random();
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
      int degradationAmount;
      degradationAmount = 100;

      if (state == AsteroidState.Idle) {
        currentEchos -= degradationAmount;

        if (currentEchos < 0) {
          currentEchos = 0;
        }

        if (currentEchos == 0) {
          state = AsteroidState.Depleted;
        }
      }
    }

    public void SetSpawnId(int id) {
      spawnId = id;
    }

    public override string ToString() {
      return $"[{createId}:{spawnId}] {currentEchos}/{maxEchos} {(state == AsteroidState.Idle ? "Active" : "Depleted")}";
    }
  }
}