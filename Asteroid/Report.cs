namespace Asteroid {
  public class Report {
    public int jobNumber;
    public int asteroidSpawnId;
    public int amountMined;

    public Report(int jobNumber, int asteroidSpawnId, int amountMined) {
      this.jobNumber = jobNumber;
      this.asteroidSpawnId = asteroidSpawnId;
      this.amountMined = amountMined;
    }

    public override string ToString() {
      return $"Job #{jobNumber} | Asteroid #{asteroidSpawnId} | Mined: {amountMined}";
    }
  }
}
