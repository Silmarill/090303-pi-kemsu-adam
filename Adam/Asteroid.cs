using Adam;
using System;

namespace Asteroids {
  public class Asteroid : IChronListener {
    private static int _nextCreateId = 1;
    private static int _nextSpawnId = 1;
    private static readonly Random _random = new Random();

    public int MaxEchos;
    public int CurrentEchos;
    public int SpawnID;
    public int CreateID;
    public AsteroidState State;

    private const int echosLossPerTick = 100;
    private const int minMaxEchos = 100;
    private const int maxMaxEchos = 1000;

    public Asteroid() {
      MaxEchos = _random.Next(minMaxEchos, maxMaxEchos + 1);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      CreateID = _nextCreateId;
      _nextCreateId = _nextCreateId + 1;
      SpawnID = 0;
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnID = _nextSpawnId;
      _nextSpawnId = _nextSpawnId + 1;
    }

    public void OnChronTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos = CurrentEchos - echosLossPerTick;
        if (CurrentEchos <= 0) {
          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        }
      }
    }

    public void PrintInfo() {
      Console.WriteLine($"Asteroid #{CreateID} (Spawn #{SpawnID}) | Echos: {CurrentEchos}/{MaxEchos} | {State}");
    }
  }
} 