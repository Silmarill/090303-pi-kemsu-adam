using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidSimulation.Models {
  public class Report {
    public int JobNumber;           // Номер задания у этого харвестера
    public int AsteroidSpawnID;     // SpawnID астероида
    public int AmountMined;         // Количество добытого ресурса

    public Report(int jobNumber, int asteroidSpawnID, int amountMined) {
      JobNumber = jobNumber;
      AsteroidSpawnID = asteroidSpawnID;
      AmountMined = amountMined;
    }

    public override string ToString() {
      return $"Job #{JobNumber}:Asteroid SpawnID={AsteroidSpawnID}, Mined={AmountMined}";
    }
  }
}
