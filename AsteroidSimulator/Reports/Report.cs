using System;

namespace AsteroidSimulator.Reports;

public class Report {
  public int JobNumber;
  public int AsteroidSpawnId;
  public int AmountMined;

  public Report(int jobNumber, int asteroidSpawnId, int amountMined) {
    JobNumber = jobNumber;
    AsteroidSpawnId = asteroidSpawnId;
    AmountMined = amountMined;
  }
}