using System;

namespace Project_Adam {
  public class Asteroid : IChroneListener {
    public int _nextCreateId = 0;
    public Random _random = new Random();
    public int MinEchosRange = 100;
    public int MaxEchosRange = 1001;

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Asteroid() {
      MaxEchos = _random.Next(MinEchosRange, MaxEchosRange);
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
      int echoReduction = 100;

      if (State == AsteroidState.Idle) {
        CurrentEchos -= echoReduction;
        if (CurrentEchos <= 0) {
          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        }
      }
    }
  }
}
