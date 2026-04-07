using System;

namespace AsteroidSimulation {

  public class Asteroid : IChroneListener {
    private const int MinMaxEchos = 100;
    private const int MaxMaxEchos = 1000;
    private const int EchosDecreasePerTick = 100;
    private const int EmptyEchos = 0;

    private static int s_nextCreateId = 1;
    private static int s_nextSpawnId = 1;
    private static readonly Random s_random = new Random();

    public int CurrentEchos { get; private set; }
    public int MaxEchos { get; private set; }
    public AsteroidState State { get; private set; }
    public int SpawnID { get; private set; }
    public int CreateID { get; }

    public Asteroid() {
      CreateID = s_nextCreateId;
      s_nextCreateId++;

      MaxEchos = s_random.Next(MinMaxEchos, MaxMaxEchos + 1);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnID = EmptyEchos;
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChronTick() {
      if (State != AsteroidState.Idle) {
        return;
      }

      CurrentEchos -= EchosDecreasePerTick;

      if (CurrentEchos <= EmptyEchos) {
        CurrentEchos = EmptyEchos;
        State = AsteroidState.Depleted;
      }
    }

    public void OnSpawn() {
      SpawnID = s_nextSpawnId;
      s_nextSpawnId++;
    }
  }
}