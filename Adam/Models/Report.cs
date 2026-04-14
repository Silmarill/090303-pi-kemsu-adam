using System;

namespace Adam.Models {
  public class Report {
    public int jobNumber;
    public int asteroidSpawnId;
    public double amountMined;
    public string harvesterName;

    public Report(int jobNum, int asteroidId, double amount, string name) {
      jobNumber = jobNum;
      asteroidSpawnId = asteroidId;
      amountMined = amount;
      harvesterName = name;
    }

    public void PrintReport() {
      Console.WriteLine("Job #" + jobNumber + " | Asteroid #" + asteroidSpawnId + " | Mined: " + amountMined + " | By: " + harvesterName);
    }
  }
}