namespace AsteroidSimulator.Models {
  public class Report {
    public int JobNumber;
    public int AsteroidSpawnID;
    public int AmountMined;

    public override string ToString() {
      return $"Job #{this.JobNumber} | Asteroid #{this.AsteroidSpawnID} | Mined {this.AmountMined}";
    }
  }
}