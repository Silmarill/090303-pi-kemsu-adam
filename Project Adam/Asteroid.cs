using System;

namespace Project_Adam {
  public class Asteroid : IChroneListener {
    private static int _nextCreateId = 0;
    private static Random _random = new Random();

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Asteroid() {
      MaxEchos = _random.Next(100, 1001);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      CreateID = ++_nextCreateId;
      SpawnID = -1;
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChroneTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos -= 100;
        if (CurrentEchos <= 0) {
          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        }
      }
    }
  }
}
