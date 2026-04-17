using System;

namespace AsteroidSimulation {
  public class Report {
    private int _jobNumber;
    private int _asteroidSpawnId;
    private int _amountMined;

    public int JobNumber {
      get { return _jobNumber; }
    }

    public int AsteroidSpawnId {
      get { return _asteroidSpawnId; }
    }

    public int AmountMined {
      get { return _amountMined; }
    }

    public Report(int jobNumber, int asteroidSpawnId, int amountMined) {
      _jobNumber = jobNumber;
      _asteroidSpawnId = asteroidSpawnId;
      _amountMined = amountMined;
    }

    public override string ToString() {
      return $"Job #{_jobNumber} | Asteroid SpawnID: {_asteroidSpawnId} | Mined: {_amountMined}";
    }
  }
}