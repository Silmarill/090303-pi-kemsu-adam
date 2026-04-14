using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pattern_Observer_and_Object_pool__Laba_5.ProgramReport {
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
      Console.WriteLine($"JobNumber: {JobNumber}, AsteroidSpawnID: {AsteroidSpawnID}, AmountMined: {AmountMined}");
    }
  }
}
