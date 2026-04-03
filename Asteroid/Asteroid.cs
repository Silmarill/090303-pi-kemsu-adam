using Asteroid;
using System;

namespace Asteroid {
  public enum AsteroidState {
    Idle,
    Depleted
  }

  public class Asteroid : IChronListener {
    private static int _nextCreateId = 1;

    public int currentEchos { get; private set; }
    public int maxEchos { get; private set; }
    public AsteroidState state { get; private set; }
    public int spawnId { get; private set; }
    public int createId { get; private set; }

    public Asteroid() {
      Random rnd = new Random();
      maxEchos = rnd.Next(100, 1001);
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
      if (state == AsteroidState.Idle) {
        currentEchos -= 100;

        if (currentEchos < 0) currentEchos = 0;

        if (currentEchos == 0) state = AsteroidState.Depleted;
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
