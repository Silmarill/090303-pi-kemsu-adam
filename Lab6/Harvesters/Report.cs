using System;

public class Report {
  public int jobNumber;
  public int asteroidSpawnID;
  public int amountMined;

  public Report(int jobNumber, int asteroidSpawnID, int amountMined) {
    this.jobNumber = jobNumber;
    this.asteroidSpawnID = asteroidSpawnID;
    this.amountMined = amountMined;
  }

  public void Print() {
    Console.WriteLine(
      "    Job #" + jobNumber +
      " | Asteroid Spawn #" + asteroidSpawnID +
      " | Mined: " + amountMined
    );
  }
}
