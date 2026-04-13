using System;

namespace AsteroidZoneSimulation.Models {
  public class Report {
    public int JobNumber;
    public int AsteroidSpawnID;
    public int AmountMined;

    public Report(int jobNumber, int asteroidSpawnID, int amountMined) {
      JobNumber = jobNumber;
      AsteroidSpawnID = asteroidSpawnID;
      AmountMined = amountMined;
    }

    public void Print() {
      Console.WriteLine($"    Задание #{JobNumber} | Астероид (SpawnID): {AsteroidSpawnID} | Добыто: {AmountMined} ед.");
    }
  }
}
