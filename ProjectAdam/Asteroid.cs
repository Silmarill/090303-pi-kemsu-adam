using System;
using System.Threading;

namespace ProjectAdam {
  public class Asteroid : IChronListener {
    private static int s_nextCreateId;

    public int CurrentEchos { get; private set; }
    public int MaxEchos { get; }
    public AsteroidState State { get; private set; }
    public int SpawnID { get; internal set; }
    public int CreateID { get; }

    public Asteroid() {
      CreateID = Interlocked.Increment(ref s_nextCreateId);
      MaxEchos = new Random().Next(100, 1001);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChronTick() {
      if (State != AsteroidState.Idle) {
        return;
      }

      CurrentEchos = Math.Max(0, CurrentEchos - 100);
      if (CurrentEchos == 0) {
        State = AsteroidState.Depleted;
      }
    }

    public void PrintInfo() {
      Console.WriteLine(
          $"CreateID={CreateID}, SpawnID={SpawnID}, MaxEchos={MaxEchos}, " +
          $"CurrentEchos={CurrentEchos}, State={State}");
    }
  }
}