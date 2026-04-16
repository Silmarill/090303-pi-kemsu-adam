using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSpace {
  public class Report {
    public int JobNumber;
    public int AsteroidSpawnId;
    public int AmountMined;

    public Report(int jobNumber, int asteroidSpawnId, int amountMined) {
      JobNumber = jobNumber;
      AsteroidSpawnId = asteroidSpawnId;
      AmountMined = amountMined;
    }

    public string GetReportString() {
      return "Задание #" + JobNumber + ": Астероид #" + AsteroidSpawnId + ", Добыто: " + AmountMined + " ед.";
    }
  }
}
