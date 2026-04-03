using System;
using AsteroidSimulator.Core;

namespace AsteroidSimulator.Asteroids;

public class Asteroid : IChronListener {
  private static int _nextCreateId = 1;
  private static int _nextSpawnId = 1;

  public int CurrentEchos { get; private set; }
  public int MaxEchos { get; private set; }
  public AsteroidState State { get; private set; }
  public int SpawnID { get; private set; }
  public int CreateID { get; private set; }

  private static Random _random = new Random();

  public Asteroid() {
    CreateID = _nextCreateId++;
    MaxEchos = _random.Next(100, 1001);
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
      CurrentEchos -= 100;
      if (CurrentEchos <= 0) {
        CurrentEchos = 0;
        State = AsteroidState.Depleted;
      }
    }
  }

  public void SetSpawnID() {
    SpawnID = _nextSpawnId++;
  }

  public override string ToString() {
    return $"Ast #{CreateID} (Spawn:{SpawnID}) | Echos: {CurrentEchos,4}/{MaxEchos,4} | State: {State}";
  }
}