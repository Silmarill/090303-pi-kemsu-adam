using System;
using AsteroidZoneSimulation.Core;

namespace AsteroidZoneSimulation.Models {
  public class Asteroid : IChroneListener {
    int globalCreateCounter = 0;
    int losingEchos = 100;

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public Random random = new Random();

    public Asteroid() {
      MaxEchos = random.Next(100, 1001);
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
      CreateID = ++globalCreateCounter;
      SpawnID = 0;
    }

    public void Reset() {
      CurrentEchos = MaxEchos;
      State = AsteroidState.Idle;
    }

    public void OnChroneTick() {
      if (State == AsteroidState.Idle) {
        CurrentEchos -= losingEchos;

        if (CurrentEchos <= 0) {
          CurrentEchos = 0;
          State = AsteroidState.Depleted;
        }
      }
    }

    public void SetSpawnID(int id) {
      SpawnID = id;
    }

    public string GetAsteroidInformation() {
      return $"Create ID:{CreateID} | Spawn ID:{SpawnID} | Echoes:{CurrentEchos}/{MaxEchos} | State:{State}";
    }
  }
}
