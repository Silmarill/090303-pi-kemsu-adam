using System;
using AsteroidZoneSimulation.Core;

namespace AsteroidZoneSimulation.Models {
  public class Asteroid : IChroneListener {
    int globalCreateCounter = 0;

    public int CurrentEchos;
    public int MaxEchos;
    public AsteroidState State;
    public int SpawnID;
    public int CreateID;

    public static Random random = new Random();

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
    }

    public void SetSpawnID(int id) {
      SpawnID = id;
    }

    public int ReduceEchos(int amount) {
      int actualAmount = Math.Min(amount, CurrentEchos);
      CurrentEchos -= actualAmount;

      if (CurrentEchos <= 0) {
        CurrentEchos = 0;
        State = AsteroidState.Depleted;
      }

      return actualAmount;
    }

    public void DisplayAsteroidInformation() {
      Console.WriteLine($"Create ID: {CreateID} | Spawn ID: {SpawnID} | Echoes: {CurrentEchos}/{MaxEchos} | State: {State}");
    }
  }
}
