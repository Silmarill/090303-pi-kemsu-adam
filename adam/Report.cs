using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    Console.WriteLine($"Job {JobNumber}, Asteroid {AsteroidSpawnID}, Mined: {AmountMined}");
  }
}