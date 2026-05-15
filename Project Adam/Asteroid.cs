using System;
using AsteroidSimulator.Interfaces;

namespace AsteroidSimulator.Models {
  public class Asteroid : IChronListener {
    private const int MinEchos = 100;
    private const int MaxEchosExclusive = 1001;
    private const int ChronDepletionStep = 100;

    private static readonly Random SharedRandom = new Random();

    public static int GlobalSpawnCounter;
    public static int GlobalCreateCounter;

    public int CurrentEchos { get; private set; }
    public int MaxEchos { get; private set; }
    public AsteroidState State { get; private set; }
    public int CreateId { get; private set; }
    public int SpawnId { get; private set; }

    public static void ResetCounters() {
      GlobalSpawnCounter = 0;
      GlobalCreateCounter = 0;
    }

    public Asteroid() {
      AssignNewIdentity();
      ApplyRandomCapacity();
      RestoreIdleWithFullEchos();
      SpawnId = 0;
    }

    public void Reset() {
      RestoreIdleWithFullEchos();
      GlobalSpawnCounter = GlobalSpawnCounter + 1;
      SpawnId = GlobalSpawnCounter;
    }

    private void RestoreIdleWithFullEchos() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChronTick() {
      if (State != AsteroidState.Idle) {
        return;
      }

      int nextEchos;
      nextEchos = CurrentEchos - ChronDepletionStep;

      if (nextEchos <= 0) {
        CurrentEchos = 0;
        State = AsteroidState.Depleted;
      } else {
        CurrentEchos = nextEchos;
      }
    }

    public int Mine(int biteSize) {
      int zero;
      int minedAmount;

      zero = 0;

      if (State != AsteroidState.Mining) {
        return zero;
      }

      if (biteSize < CurrentEchos) {
        minedAmount = biteSize;
      } else {
        minedAmount = CurrentEchos;
      }

      CurrentEchos = CurrentEchos - minedAmount;

      if (CurrentEchos == zero) {
        State = AsteroidState.Depleted;
      }

      return minedAmount;
    }

    public void StartMining() {
      if (State == AsteroidState.Idle) {
        State = AsteroidState.Mining;
      }
    }

    public void StopMining() {
      if (State == AsteroidState.Mining) {
        if (CurrentEchos <= 0) {
          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        } else {
          State = AsteroidState.Idle;
        }
      }
    }

    private void AssignNewIdentity() {
      int nextCreateId;
      nextCreateId = GlobalCreateCounter + 1;
      GlobalCreateCounter = nextCreateId;
      CreateId = nextCreateId;
    }

    private void ApplyRandomCapacity() {
      MaxEchos = SharedRandom.Next(MinEchos, MaxEchosExclusive);
    }

    public override string ToString() {
      return "Asteroid #" + SpawnId + " (Created:" + CreateId + ") | " + CurrentEchos + "/" + MaxEchos + " | " + State;
    }
  }
}
