using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
public enum AsteroidState {
  Idle,
  Depleted,
  Mining
}

namespace Asteroids {
  public class Asteroid : IChroneListener {

    private static int _createCounter = 0;

    public int MaxEchos;
    public int CurrentEchos;
    public int SpawnID;
    public int CreateID;

    private static int _spawnCounter = 0;
    private static Random random = new Random();
    public AsteroidState State { get; set; }

    public Asteroid() {
      CreateID = ++_createCounter;
      MaxEchos = random.Next(100, 1000);
      Reset();
    }
    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      SpawnID = ++_spawnCounter;
    }

    public void OnChronTick() {
      if (State == AsteroidState.Idle) {

        CurrentEchos -= 100;

        if (CurrentEchos < 0 || CurrentEchos == 0) {

          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        }
      }
    }
    public override string ToString() {
      return $"Asteroid C:{CreateID} S:{SpawnID} | {CurrentEchos}/{MaxEchos} | {State}";
    }

  }
}
