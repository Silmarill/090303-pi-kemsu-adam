using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroids {
  public class Report {
    public int JobNumber;
    public int AsteroidSpawnID;
    public int AmountMined;

    public override string ToString() {
      return $"Job #{JobNumber} | Asteroid: {AsteroidSpawnID} | Mined: {AmountMined}";
    }
  }
}
