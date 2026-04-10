using System;
using AsteroidSimulator.Core;

namespace AsteroidSimulator.Asteroids;

public class Asteroid : IChronListener {
  private static int _nextCreateId = 1;
  private static int _nextSpawnId = 1;
  private static Random _random = new Random();

  public int CurrentEchos;
  public int MaxEchos;
  public AsteroidState State;
  public int SpawnID;
  public int CreateID;

  private const int MinEchos = 1;
  private const int MaxEchosValue = 10000000;
  private const int DegradationAmount = 100;

  public Asteroid() {
    CreateID = _nextCreateId++;
    MaxEchos = _random.Next(MinEchos, MaxEchosValue + 1);
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
    SpawnID = 0;
  }

  public void Reset() {
    CurrentEchos = MaxEchos;
    State = AsteroidState.Idle;
  }

  public void OnChronTick() {
    if (State == AsteroidState.Idle) {
      CurrentEchos = CurrentEchos - DegradationAmount;

      if (CurrentEchos <= 0) {
        CurrentEchos = 0;
        State = AsteroidState.Depleted;
      }
    }
  }

  public void SetSpawnID() {
    SpawnID = _nextSpawnId++;
  }
}