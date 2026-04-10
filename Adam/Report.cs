using System;

namespace Harvester {
  public class Report {
    public int jobNumber;
    public int asteroidSpawnId;
    public int amountMined;

    public Report(int jobNumber, int asteroidSpawnId, int amountMined)
    {
      this.jobNumber = jobNumber;
      this.asteroidSpawnId = asteroidSpawnId;
      this.amountMined = amountMined;
    }

    public void PrintInfo()
    {
      Console.WriteLine($"    Задание #{jobNumber} | Астероид SpawnId: {asteroidSpawnId} | Добыто: {amountMined}");
    }
  }
}