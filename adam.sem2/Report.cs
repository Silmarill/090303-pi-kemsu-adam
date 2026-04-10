namespace AsteroidSimulator.Models {
  public class Report {
    public int JobNumber { get; set; }
    public int AsteroidSpawnID { get; set; }
    public int AmountMined { get; set; }

    public override string ToString()
    {
      return $"Job #{JobNumber} | Asteroid #{AsteroidSpawnID} | Mined {AmountMined}";
    }
  }
}