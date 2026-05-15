namespace AsteroidSimulator.Models {
  /// <summary>Отчёт о завершённой «смене» добычи (по методичке).</summary>
  public class Report {
    public int JobNumber { get; }
    public int AsteroidSpawnID { get; }
    public int AmountMined { get; }

    public Report(int jobNumber, int asteroidSpawnID, int amountMined) {
      JobNumber = jobNumber;
      AsteroidSpawnID = asteroidSpawnID;
      AmountMined = amountMined;
    }

    public override string ToString() {
      return "Job #" + JobNumber + " | SpawnId=" + AsteroidSpawnID + " | amount " + AmountMined + " Echos";
    }
  }
}
